namespace MircoGericke.StreamDeck.Plugin;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection;
using MircoGericke.StreamDeck.Connection.Events;
using MircoGericke.StreamDeck.Connection.Model;

public class PluginManager : StreamDeckConnection
{
	private readonly ILogger<PluginManager> logger;
	private readonly IServiceProvider provider;

	private readonly IReadOnlyDictionary<ActionId, ActionDescriptor> descriptors;
	private readonly ConcurrentDictionary<ContextId, ContextDescriptor> instances = new();

	public PluginManager(
		IServiceProvider provider,
		ILogger<PluginManager> logger,
		ILogger<StreamDeckConnection> connectionLogger,
		StreamDeckConnectionOptions options,
		IEnumerable<ActionDescriptor> descriptors
	) : base(connectionLogger, options)
	{
		this.logger = logger;
		this.provider = provider;
		this.descriptors = descriptors.ToDictionary(v => v.Id);
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledDomainException;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (disposing)
		{
			AppDomain.CurrentDomain.UnhandledException -= OnUnhandledDomainException;
		}
	}

	private void OnUnhandledDomainException(object sender, UnhandledExceptionEventArgs e)
	{
		if(e.ExceptionObject is Exception ex)
		{
			logger.LogCritical(ex, "Uncaught Exception in AppDomain.");
		}
		else
		{
			logger.LogCritical("Uncaught Exception in AppDomain: {@exceptin}.", e.ExceptionObject);
		}
	}

	Action Instantiate(IServiceScope scope, ActionId id)
	{
		if (!descriptors.TryGetValue(id, out var desc))
		{
			logger.LogError("Could not find descriptor for action {actionId}.", id);
			return Action.Missing;
		}

		var value = scope.ServiceProvider.GetService(desc.Type);

		if (value is not Action act)
		{
			logger.LogError("Could not instantiate {@descriptor}.", desc);
			return Action.Missing;
		}

		return act;
	}

	private ContextDescriptor CreateContext(ContextId contextId, ActionId actionId, DeviceId deviceId)
	{
		var scope = provider.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<ActionContext>();
		context.ContextId = contextId;
		context.ActionId = actionId;
		context.DeviceId = deviceId;

		var action = Instantiate(scope, actionId);

		return new()
		{
			Scope = scope,
			Instance = action,
		};
	}

	private Action GetInstance(ContextId contextId, ActionId actionId, DeviceId deviceId)
	{
		var descriptor = instances.GetOrAdd(contextId, _ => CreateContext(contextId, actionId, deviceId));
		return descriptor.Instance;
	}

	protected override Task OnWillAppear(WillAppearEvent e, CancellationToken cancellationToken)
	{
		_ = GetInstance(e.ContextId, e.ActionId, e.DeviceId);
		return Task.CompletedTask;
	}
}
