using System.Diagnostics;

using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Hosting;
using MircoGericke.StreamDeck.Plugin;

if (!StreamDeckHost.TryCreateBuilder(args, out var builder))
{
	// You could do something else when the application is not started by the stream deck software.
	return;
}

#if DEBUG
Debugger.Launch();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
#endif

await builder
	.WithPlugin()
	.UseActions()
	.Build()
	.RunAsync();
