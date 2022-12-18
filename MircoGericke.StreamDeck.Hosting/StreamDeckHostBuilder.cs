namespace MircoGericke.StreamDeck.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection;
using MircoGericke.StreamDeck.Hosting.Model;

public class StreamDeckHostBuilder
{
	internal StreamDeckHostBuilder(StreamDeckConnectionOptions options, StartupInfo info)
	{
		host = Host.CreateApplicationBuilder();

		Services
			.AddSingleton(options)
			.AddSingleton(info);
	}

	private readonly HostApplicationBuilder host;

	public IServiceCollection Services => host.Services;
	public ILoggingBuilder Logging => host.Logging;
	public StreamDeckHost Build() => new(host.Build());
}
