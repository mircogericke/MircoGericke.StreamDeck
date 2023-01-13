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
using MircoGericke.StreamDeck.Plugin.Action;
using MircoGericke.StreamDeck.Plugin.Context;

public class PluginManager : StreamDeckConnection
{
	private readonly ILogger<PluginManager> logger;
	private readonly IServiceScopeFactory scopeFactory;

	private readonly IReadOnlyDictionary<ActionId, ActionDescriptor> descriptors;
	private readonly ConcurrentDictionary<ContextId, ContextDescriptor> instances = new();

	public PluginManager(
		IServiceScopeFactory scopeFactory,
		ILogger<PluginManager> logger,
		ILogger<StreamDeckConnection> connectionLogger,
		StreamDeckConnectionOptions options,
		IEnumerable<ActionDescriptor> descriptors
	) : base(connectionLogger, options)
	{
		this.logger = logger;
		this.scopeFactory = scopeFactory;
		this.descriptors = descriptors.ToDictionary(v => v.Id);
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledDomainException;
	}

	protected override void DisposeManaged()
	{
		base.DisposeManaged();
		AppDomain.CurrentDomain.UnhandledException -= OnUnhandledDomainException;
	}

	private void OnUnhandledDomainException(object sender, UnhandledExceptionEventArgs e)
	{
		if (e.ExceptionObject is Exception ex)
		{
			logger.LogCritical(ex, "Uncaught Exception in AppDomain.");
		}
		else
		{
			logger.LogCritical("Uncaught Exception in AppDomain: {@exception}.", e.ExceptionObject);
		}
	}

	private StreamDeckAction Instantiate(IServiceScope scope, ActionId id)
	{
		if (!descriptors.TryGetValue(id, out var desc))
		{
			logger.LogError("Could not find descriptor for action {actionId}.", id);
			return StreamDeckAction.Missing;
		}

		var value = scope.ServiceProvider.GetService(desc.Type);

		if (value is not StreamDeckAction act)
		{
			logger.LogError("Could not instantiate {@descriptor}.", desc);
			return StreamDeckAction.Missing;
		}

		return act;
	}

	private ContextDescriptor CreateContext(ContextId contextId, ActionId actionId, DeviceId deviceId)
	{
		var scope = scopeFactory.CreateScope();

		if (scope.ServiceProvider.GetRequiredService<IActionContext>() is ActionContext context)
		{
			context.ContextId = contextId;
			context.ActionId = actionId;
			context.DeviceId = deviceId;
		}
		else
		{
			throw new InvalidOperationException($"{nameof(IActionContext)} type is not {nameof(ActionContext)}. Did you overwrite it?");
		}

		var action = Instantiate(scope, actionId);

		return new()
		{
			Scope = scope,
			Instance = action,
		};
	}

	private StreamDeckAction GetOrAdd(ContextEvent e)
	{
		var descriptor = instances.GetOrAdd(e.ContextId, _ => CreateContext(e.ContextId, e.ActionId, e.DeviceId));
		return descriptor.Instance;
	}

	private StreamDeckAction? TryGet(ActionEvent e)
	{
		if (instances.TryGetValue(e.ContextId, out var descriptor))
			return descriptor.Instance;
		return null;
	}

	protected override Task OnWillAppear(WillAppearEvent e, CancellationToken cancellationToken)
		=> GetOrAdd(e).InitializeAsync(e.Payload, cancellationToken);

	protected async override Task OnWillDisappear(WillDisappearEvent e, CancellationToken cancellationToken)
	{
		if (instances.TryRemove(e.ContextId, out var descriptor))
		{
			await descriptor.Instance.DisposeAsync().ConfigureAwait(false);
			descriptor.Scope.Dispose();
		}
	}

	protected override Task OnApplicationDidLaunch(ApplicationDidLaunchEvent e, CancellationToken cancellationToken) => base.OnApplicationDidLaunch(e, cancellationToken);
	protected override Task OnApplicationDidTerminate(ApplicationDidTerminateEvent e, CancellationToken cancellationToken) => base.OnApplicationDidTerminate(e, cancellationToken);

	protected override Task OnConnected(EventArgs e, CancellationToken cancellationToken) => base.OnConnected(e, cancellationToken);
	protected override Task OnDisconnected(EventArgs e, CancellationToken cancellationToken) => base.OnDisconnected(e, cancellationToken);

	protected override Task OnDeviceDidConnect(DeviceDidConnectEvent e, CancellationToken cancellationToken) => base.OnDeviceDidConnect(e, cancellationToken);

	protected override Task OnDeviceDidDisconnect(DeviceDidDisconnectEvent e, CancellationToken cancellationToken) => base.OnDeviceDidDisconnect(e, cancellationToken);

	protected override Task OnDialPress(DialPressEvent e, CancellationToken cancellationToken) => base.OnDialPress(e, cancellationToken);

	protected override Task OnDialRotate(DialRotateEvent e, CancellationToken cancellationToken) => base.OnDialRotate(e, cancellationToken);
	protected override Task OnDidReceiveGlobalSettings(DidReceiveGlobalSettingsEvent e, CancellationToken cancellationToken) => base.OnDidReceiveGlobalSettings(e, cancellationToken);
	protected override Task OnDidReceiveSettings(DidReceiveSettingsEvent e, CancellationToken cancellationToken) => GetOrAdd(e).OnDidReceiveSettings(e.Payload, cancellationToken);

	protected override Task OnKeyDown(KeyDownEvent e, CancellationToken cancellationToken)
	{
		if (GetOrAdd(e) is IKeypadAction a)
			return a.OnKeyDown(e.Payload, cancellationToken);

		logger.LogWarning("Action type mismatch: expected action '{action}' to implement '{type}'.", e.ActionId, typeof(IKeypadAction));
		return Task.CompletedTask;
	}

	protected override Task OnKeyUp(KeyUpEvent e, CancellationToken cancellationToken)
	{
		if (GetOrAdd(e) is IKeypadAction a)
			return a.OnKeyUp(e.Payload, cancellationToken);

		logger.LogWarning("Action type mismatch: expected action '{action}' to implement '{type}'.", e.ActionId, typeof(IKeypadAction));
		return Task.CompletedTask;
	}

	protected override Task OnPropertyInspectorDidAppear(PropertyInspectorDidAppearEvent e, CancellationToken cancellationToken) => base.OnPropertyInspectorDidAppear(e, cancellationToken);
	protected override Task OnPropertyInspectorDidDisappear(PropertyInspectorDidDisappearEvent e, CancellationToken cancellationToken) => base.OnPropertyInspectorDidDisappear(e, cancellationToken);
	protected override Task OnSendToPlugin(SendToPluginEvent e, CancellationToken cancellationToken) => TryGet(e)?.OnRpc(e.Payload, cancellationToken) ?? Task.CompletedTask;
	protected override Task OnSystemDidWakeUp(SystemDidWakeUpEvent e, CancellationToken cancellationToken) => base.OnSystemDidWakeUp(e, cancellationToken);
	protected override Task OnTitleParametersDidChange(TitleParametersDidChangeEvent e, CancellationToken cancellationToken) => base.OnTitleParametersDidChange(e, cancellationToken);
	protected override Task OnTouchTap(TouchTapEvent e, CancellationToken cancellationToken) => base.OnTouchTap(e, cancellationToken);
	protected override Task OnUnknownEvent(StreamDeckEvent e, CancellationToken cancellationToken) => base.OnUnknownEvent(e, cancellationToken);
}
