namespace MircoGericke.StreamDeck.Plugin;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MircoGericke.StreamDeck.Connection.Model;
using MircoGericke.StreamDeck.Hosting;
using MircoGericke.StreamDeck.Plugin.Action;
using MircoGericke.StreamDeck.Plugin.Context;

public static class StreamDeckHostBuilderExtensions
{
	public static StreamDeckHostBuilder WithPlugin(this StreamDeckHostBuilder builder)
		=> builder.WithPlugin<PluginManager>();

	public static StreamDeckHostBuilder WithPlugin<TPluginManager>(this StreamDeckHostBuilder builder)
		where TPluginManager: PluginManager
	{
		builder.Services
			.AddSingleton<TPluginManager>()
			.AddHostedService<PluginManager>(ctx => ctx.GetRequiredService<TPluginManager>())
			.AddScoped<IActionContext, ActionContext>();

		return builder;
	}

	public static StreamDeckHostBuilder UseAction<T>(this StreamDeckHostBuilder host, string actionId)
		where T : StreamDeckAction
		=> host.UseAction(typeof(T), new(actionId));

	public static StreamDeckHostBuilder UseAction(this StreamDeckHostBuilder host, Type actionType, ActionId actionId)
	{
		if (!actionType.IsAssignableTo(typeof(IStreamDeckAction)))
			throw new NotSupportedException($"Type {actionType} is not compatible with type {typeof(StreamDeckAction)}.");

		var descriptor = new ActionDescriptor
		{
			Id = actionId,
			Type = actionType,
		};

		host.Services.AddSingleton(descriptor);
		host.Services.AddScoped(actionType);

		return host;
	}

	public static StreamDeckHostBuilder UseActions(this StreamDeckHostBuilder host, Assembly? assembly = null)
	{
		assembly ??= Assembly.GetCallingAssembly();

		var actions = assembly
			.GetTypes()
			.Where(type => type.IsAssignableTo(typeof(IStreamDeckAction)))
			.Select(type => (type, attr: type.GetCustomAttribute<ActionIdAttribute>()))
			.Where(v => v.attr is not null);

		foreach (var (type, attr) in actions)
		{
			host.UseAction(type, attr!.Id);
		}

		return host;
	}
}
