namespace MircoGericke.StreamDeck.Connection;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection.Events;
using MircoGericke.StreamDeck.Connection.Messages;

internal partial class StreamDeckSocket : IDisposable
{
	private readonly ClientWebSocket websocket;
	private readonly ILogger logger;
	private readonly StreamDeckConnectionOptions options;
	private readonly ChannelReader<StreamDeckMessage> sendingChannel;
	private readonly CancellationTokenSource cts = new();
	private readonly Func<StreamDeckEvent, CancellationToken, Task> onEvent;

	private Task? writeTask;
	private Task? readTask;
	private bool disposedValue;

	public StreamDeckSocket(
		StreamDeckConnectionOptions options,
		ChannelReader<StreamDeckMessage> sendingChannel,
		ILogger logger,
		Func<StreamDeckEvent, CancellationToken, Task> onEvent
	)
	{
		this.logger = logger;
		this.options = options;
		this.onEvent = onEvent;
		this.sendingChannel = sendingChannel;
		websocket = new ClientWebSocket();
	}

	public async Task ConnectAsync(CancellationToken cancellationToken)
	{
		logger.LogTrace(nameof(ConnectAsync) + " connecting to {@options}.", options);
		await websocket.ConnectAsync(new Uri($"ws://localhost:{options.Port}"), cancellationToken);

		if (websocket.State != WebSocketState.Open)
		{
			logger.LogCritical(nameof(ConnectAsync) + " failed - web socket not open: {state}.", websocket.State);
			return;
		}

		writeTask = Task.Run(() => WriteAllAsync(cts.Token), cancellationToken);
		readTask = Task.Run(() => ReadAllAsync(cts.Token), cancellationToken);
	}

	public Task Connection => Task.WhenAny(writeTask ?? Task.CompletedTask, readTask ?? Task.CompletedTask);

	public async Task DisconnectAsync(CancellationToken cancellationToken)
	{
		logger.LogTrace(nameof(ConnectAsync) + " disconnecting from to {@options}.", options);
		cts.Cancel();
		try
		{
			if (writeTask != null)
			{
				try
				{
					await writeTask.WaitAsync(cancellationToken);
				}
				catch (OperationCanceledException) { }
			}

			if (readTask != null)
			{
				try
				{
					await readTask.WaitAsync(cancellationToken);
				}
				catch (OperationCanceledException) { }
			}

			await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "requested by client", cancellationToken);
		}
		finally
		{
			websocket.Dispose();
			cts.Dispose();
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				websocket.Dispose();
				cts.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
