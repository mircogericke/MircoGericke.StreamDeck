namespace MircoGericke.StreamDeck.Hosting;

using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using MircoGericke.StreamDeck.Connection;
using MircoGericke.StreamDeck.Hosting.Model;

public class StreamDeckHost
{
	private readonly IHost host;

	internal StreamDeckHost(IHost host)
	{
		this.host = host;
	}

	public async Task RunAsync(CancellationToken cancellationToken = default)
		=> await host.RunAsync(cancellationToken);

	public static bool TryCreateBuilder(string[] args, [NotNullWhen(true)] out StreamDeckHostBuilder? builder)
	{
		var (options, info) = ParseArguments(args);

		if (options is null || info is null)
		{
			builder = null;
			return false;
		}

		builder = new(options, info);
		return true;
	}

	private static (StreamDeckConnectionOptions?, StartupInfo?) ParseArguments(string[] args)
	{
		static StartupInfo? ParseInfo(ArgumentResult result)
		{
			if (result.Tokens.Count != 1)
				return default;

			return JsonSerializer.Deserialize<StartupInfo>(result.Tokens[0].Value, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
		}

		var root = new RootCommand();

		var portOption = new Option<int?>("-port");
		var PluginOption = new Option<string?>("-pluginUUID");
		var registerEventOption = new Option<string?>("-registerEvent");
		var infoOption = new Option<StartupInfo?>("-info", parseArgument: ParseInfo);

		root.AddOption(portOption);
		root.AddOption(PluginOption);
		root.AddOption(registerEventOption);
		root.AddOption(infoOption);

		var parsed = root.Parse(args);
		var port = parsed.GetValueForOption(portOption);
		var uuid = parsed.GetValueForOption(PluginOption);
		var register = parsed.GetValueForOption(registerEventOption);
		var info = parsed.GetValueForOption(infoOption);

		if (port is null || uuid is null || register is null || info is null)
			return (null, null);

		return (
			new StreamDeckConnectionOptions() { Port = port.Value, Uuid = uuid, RegisterEvent = register },
			info
		);
	}
}
