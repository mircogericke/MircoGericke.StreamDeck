namespace LowLevelPlugin;
using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection;
using MircoGericke.StreamDeck.Connection.Events;
using MircoGericke.StreamDeck.Connection.Messages;
using MircoGericke.StreamDeck.Connection.Model;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
		// each message has it's own type to use with SendAsync
		await SendAsync(new LogMessage("Log This!"), cancellationToken);
	}

	protected async override Task OnKeyDown(KeyDownEvent e, CancellationToken cancellationToken)
	{
		// SixLabors.ImageSharp is a fully managed image manipulation library that's easy to use for your stream deck plugin.
		// Be aware that the library may require a commercial license if used in a commerical environment.

		using var image = new Image<Rgba32>(144, 144, Color.Black);
		image.Mutate(ctx => ctx
			.DrawLines(Color.White, 2f, new(0f, 0f), new(144f, 144f))
			.DrawLines(Color.White, 2f, new(144f, 0f), new(0f, 144f))
		);

		await SendAsync(new SetImageMessage()
		{
			ContextId = e.ContextId,
			Payload = new()
			{
				Image = image.ToBase64String(PngFormat.Instance),
				Target = SdkTarget.HardwareAndSoftware,
			}
		}, cancellationToken);
	}

	protected async override Task OnKeyUp(KeyUpEvent e, CancellationToken cancellationToken)
	{
		await SendAsync(new SetImageMessage()
		{
			ContextId = e.ContextId,
			Payload = new()
			{
				Target = SdkTarget.HardwareAndSoftware,
			}
		}, cancellationToken);
	}
}
