namespace MircoGericke.StreamDeck.Plugin;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using MircoGericke.StreamDeck.Hosting;

public static class StreamDeckHostBuilderExtensions
{
	public static StreamDeckHostBuilder WithPlugin(this StreamDeckHostBuilder builder)
	{
		builder.Services.AddHostedService<PluginManager>();
		builder.Services.AddScoped<ActionContext>();
		builder.Services.AddScoped<IActionContext>(v => v.GetRequiredService<ActionContext>());
		return builder;
	}

	public static StreamDeckHostBuilder UseAction<T>(this StreamDeckHostBuilder host, string actionId)
		where T : Action
	{
		return host.UseAction(typeof(T), actionId);
	}
	public static StreamDeckHostBuilder UseAction(this StreamDeckHostBuilder host, Type actionType, string actionId)
	{
		if (!actionType.IsAssignableTo(typeof(Action)))
			throw new NotSupportedException($"Type {actionType} is not compatible with type {typeof(Action)}.");

		var descriptor = new ActionDescriptor
		{
			Id = new(actionId),
			Type = actionType,
		};

		host.Services.AddSingleton(actionType);
		host.Services.AddSingleton(descriptor);

		return host;
	}

	public static StreamDeckHostBuilder UseActions(this StreamDeckHostBuilder host, Assembly? assembly = null)
	{
		assembly ??= Assembly.GetCallingAssembly();

		var actions = assembly
			.GetTypes()
			.Where(type => type.IsAssignableTo(typeof(Action)))
			.Select(type => (type, attr: type.GetCustomAttribute<ActionIdAttribute>()))
			.Where(v => v.attr is not null);

		foreach (var (type, attr) in actions)
		{
			host.UseAction(type, attr!.Id.ToString());
		}

		return host;
	}
}
