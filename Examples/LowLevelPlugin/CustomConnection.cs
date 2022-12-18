namespace LowLevelPlugin;
using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection;
using MircoGericke.StreamDeck.Connection.Events;
using MircoGericke.StreamDeck.Connection.Model;

internal class CustomConnection : StreamDeckConnection
{
	private readonly ILogger<CustomConnection> logger;

	public CustomConnection(
		// Inject services here
		ILogger<CustomConnection> logger,
		ILogger<StreamDeckConnection> connectionLogger,
		StreamDeckConnectionOptions options
	) : base(connectionLogger, options)
	{
		this.logger = logger;
	}

	// each event is an overridable method on the connection
	protected override async Task OnConnected(EventArgs e, CancellationToken cancellationToken)
	{
		// there are methods for each message
		await LogMessageAsync("Log this!");
		logger.LogInformation("Connected");
	}

	protected override Task OnDeviceDidConnect(DeviceDidConnectEvent e, CancellationToken cancellationToken)
	{
		logger.LogInformation("Connected to device: {deviceName}", e.DeviceInfo.Name);
		return Task.CompletedTask;
	}

	protected async override Task OnWillAppear(WillAppearEvent e, CancellationToken cancellationToken)
	{
		await SetImageAsync("", e.ContextId.ToString(), SdkTarget.HardwareAndSoftware, null);
	}
}
