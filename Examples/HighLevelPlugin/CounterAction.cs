namespace HighLevelPlugin;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection.Model;
using MircoGericke.StreamDeck.Connection.Payloads;
using MircoGericke.StreamDeck.Plugin;
using MircoGericke.StreamDeck.Plugin.Action;
using MircoGericke.StreamDeck.Plugin.Context;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

// needs to match the UUID in the manifest
[ActionId("HighLevelPlugin.Counter")]
public class CounterAction : StreamDeckAction, IKeypadAction
{
	private readonly ILogger logger;
	private readonly Image<Rgba32> defaultImage;

	private CounterSettings settings = new();

	public CounterAction(
		ILogger<CounterAction> logger,
		IActionContext context
	) : base(context)
	{
		this.logger = logger;
		defaultImage = LoadDefaultStateImage();
	}

	public override Task OnWillAppear(AppearancePayload payload, CancellationToken cancellationToken)
	{
		logger.LogWarning("Initialize {payload}", JsonSerializer.Serialize(payload));
		settings = payload.Settings.Deserialize<CounterSettings>() ?? settings;
		return Task.CompletedTask;
	}

	public override async Task OnRpc(JsonObject payload, CancellationToken cancellationToken)
	{
		logger.LogWarning("OnRpc {payload}", JsonSerializer.Serialize(payload));
	}

	public override Task OnDidReceiveSettings(ReceivedSettingsPayload payload, CancellationToken cancellationToken)
	{
		logger.LogWarning("got settings {settings}", JsonSerializer.Serialize(payload));
		settings = payload.Settings?.Deserialize<CounterSettings>() ?? settings;

		return Task.CompletedTask;
	}

	public async Task OnKeyDown(KeyPayload payload, CancellationToken cancellationToken)
	{
		settings.CurrentValue++;
		await Context.SetSettingsAsync((JsonObject)JsonSerializer.SerializeToNode(settings)!, cancellationToken);

		if (SystemFonts.TryGet("Courier New", out var family))
		{
			var font = family.CreateFont(72);
			var options = new TextOptions(font)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Origin = new(defaultImage.Width / 2f, defaultImage.Height / 2f),
			};

			var image = defaultImage
				.Clone(ctx => ctx.DrawText(options, settings.CurrentValue.ToString(), Color.White))
				.ToBase64String(PngFormat.Instance);

			await Context.SetImageAsync(new() { Image = image, Target = SdkTarget.HardwareAndSoftware }, cancellationToken);
		}
	}

	public async Task OnKeyUp(KeyPayload payload, CancellationToken cancellationToken)
	{
	}

	protected override void DisposedManaged()
	{
		base.DisposedManaged();
		defaultImage?.Dispose();
	}

	private static Image<Rgba32> LoadDefaultStateImage()
	{
		var baseDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
		var imgPath = Path.Combine(baseDir, "Actions", "Counter", "state@2x.png");
		return Image.Load<Rgba32>(imgPath);
	}
}
