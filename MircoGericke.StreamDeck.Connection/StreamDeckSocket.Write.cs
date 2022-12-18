namespace MircoGericke.StreamDeck.Connection;
using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection.Util;

partial class StreamDeckSocket
{
	private async Task WriteAllAsync(CancellationToken cancellationToken)
	{
		try
		{
			await foreach (var message in sendingChannel.ReadAllAsync(cancellationToken))
			{
				logger.LogTrace("Writing {@message}.", message);
				// TODO: utilize Utf8JsonWriter to write to a pre-allocated buffer to prevent allocation on each request
				var buffer = JsonSerializer.SerializeToUtf8Bytes(message, message.GetType(),Constants.JsonOptions);
				await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
			}
		}
		catch (OperationCanceledException) { }
		catch (Exception ex)
		{
			logger.LogError(ex, nameof(ClientWebSocket.SendAsync) + " failed.");
		}
	}
}
