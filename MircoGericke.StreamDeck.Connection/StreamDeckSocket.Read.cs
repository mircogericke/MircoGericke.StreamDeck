namespace MircoGericke.StreamDeck.Connection;
using System;
using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection.Events;
using MircoGericke.StreamDeck.Connection.Util;

partial class StreamDeckSocket
{
	private const int BufferSize = 1024 * 1024;

	private async Task ReadAllAsync(CancellationToken cancellationToken)
	{
		using var bufferOwner = MemoryPool<byte>.Shared.Rent(BufferSize);
		try
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				var evt = await ReadMessageAsync(bufferOwner.Memory, cancellationToken);

				if (evt is null)
				{
					if (websocket.CloseStatus is null)
						continue; // deserialize returned null
					else
						return; // socket connection lost
				}

				try
				{
					await onEvent(evt, cancellationToken);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "Unhandled 3rd party exception when triggering event {@event}.", evt);
				}
			}
		}
		catch (OperationCanceledException) { }
		catch (Exception ex)
		{
			logger.LogError(ex, $"{nameof(ReadAllAsync)} failed.");
		}
	}

	private async Task<StreamDeckEvent?> ReadMessageAsync(Memory<byte> primaryBuffer, CancellationToken cancellationToken)
	{
		var remainingBuffer = primaryBuffer;

		while (!cancellationToken.IsCancellationRequested)
		{
			ValueWebSocketReceiveResult result = await websocket.ReceiveAsync(remainingBuffer, cancellationToken);

			if (result.MessageType == WebSocketMessageType.Close)
			{
				return null;
			}

			if (result.MessageType != WebSocketMessageType.Text)
			{
				continue;
			}

			if (result.Count > 0)
			{
				remainingBuffer = remainingBuffer[result.Count..];
			}

			if (result.EndOfMessage)
			{
				return DeserializeEvent(new ReadOnlySequence<byte>(primaryBuffer[0..^remainingBuffer.Length]));
			}

			if (remainingBuffer.Length == 0)
			{
				return await ReadLongMessageAsync(primaryBuffer, cancellationToken);
			}
		}

		throw new TaskCanceledException(null, null, cancellationToken);
	}

	private async Task<StreamDeckEvent?> ReadLongMessageAsync(Memory<byte> primaryBuffer, CancellationToken cancellationToken)
	{
		var root = new MemorySegment<byte>(primaryBuffer);
		var last = root;
		var bufferOwner = MemoryPool<byte>.Shared.Rent(BufferSize);
		var remainingBuffer = bufferOwner.Memory;

		while (!cancellationToken.IsCancellationRequested)
		{
			ValueWebSocketReceiveResult result = await websocket.ReceiveAsync(remainingBuffer, cancellationToken);

			if (result.MessageType == WebSocketMessageType.Close)
			{
				return null;
			}

			if (result.MessageType != WebSocketMessageType.Text)
			{
				continue;
			}

			if (result.Count > 0)
			{
				remainingBuffer = remainingBuffer[result.Count..];
			}

			if (result.EndOfMessage)
			{
				last = last.Append(bufferOwner.Memory[0..^remainingBuffer.Length], bufferOwner);
				var evt = DeserializeEvent(new ReadOnlySequence<byte>(root, 0, last, last.Memory.Length));
				root.Release();
				return evt;
			}

			if (remainingBuffer.Length == 0)
			{
				last = last.Append(bufferOwner.Memory, bufferOwner);
				bufferOwner = MemoryPool<byte>.Shared.Rent(BufferSize);
				remainingBuffer = bufferOwner.Memory;
			}
		}

		throw new TaskCanceledException(null, null, cancellationToken);
	}

	private StreamDeckEvent? DeserializeEvent(ReadOnlySequence<byte> buffer)
	{
		var reader = new Utf8JsonReader(buffer);
		var document = JsonSerializer.Deserialize<JsonDocument>(ref reader, Constants.JsonOptions);

		if (document is null || !document.RootElement.TryGetProperty("event", out var element) || element.GetString() is not string eventName)
		{
			logger.LogError("Invalid event JSON: {buffer}", Encoding.UTF8.GetString(buffer));
			return null;
		}

		var type = StreamDeckEvent.TypeOf(eventName);

		StreamDeckEvent? result;
		try
		{
			result = document.Deserialize(type, Constants.JsonOptions) as StreamDeckEvent;
		}
		catch (JsonException ex)
		{
			logger.LogError(ex, "Event Deserialization failed: {document}", document.RootElement.ToString());
			return null;
		}

		if (result is null)
		{
			logger.LogError("Unknown event received from Stream Deck: {buffer}", Encoding.UTF8.GetString(buffer));
		}

		logger.LogTrace("Deserialized Event {@event}", result);
		return result;
	}
}
