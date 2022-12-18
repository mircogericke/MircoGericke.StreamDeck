using System.Diagnostics;

using LowLevelPlugin;

using Microsoft.Extensions.DependencyInjection;

using MircoGericke.StreamDeck.Hosting;

#if DEBUG
Debugger.Launch();
#endif

if (!StreamDeckHost.TryCreateBuilder(args, out var builder))
{
	// You could do something else when the application is not started by the stream deck software.
	return;
}

builder.Services.AddHostedService<CustomConnection>();
// add additional services

var host = builder.Build();
await host.RunAsync();
