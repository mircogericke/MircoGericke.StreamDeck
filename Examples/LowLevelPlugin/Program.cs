using System.Diagnostics;

using LowLevelPlugin;

using Microsoft.Extensions.DependencyInjection;

using MircoGericke.StreamDeck.Hosting;

if (!StreamDeckHost.TryCreateBuilder(args, out var builder))
{
	// You could do something else when the application is not started by the stream deck software.
	return;
}

#if DEBUG
Debugger.Launch();
#endif

builder.Services.AddHostedService<CustomConnection>();
// add additional services

var host = builder.Build();
await host.RunAsync();
