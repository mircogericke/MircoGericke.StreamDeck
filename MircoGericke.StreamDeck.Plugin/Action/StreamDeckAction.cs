namespace MircoGericke.StreamDeck.Plugin.Action;

using System.Text.Json.Nodes;

using MircoGericke.StreamDeck.Connection.Payloads;
using MircoGericke.StreamDeck.Plugin.Context;

public abstract partial class StreamDeckAction : IStreamDeckAction
{
	protected IActionContext Context { get; }

	protected StreamDeckAction(IActionContext context)
	{
		Context = context;
	}

	public virtual Task OnWillAppear(AppearancePayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	public virtual Task OnWillDisappear(AppearancePayload payload, CancellationToken cancellationToken) => Task.CompletedTask;

	public virtual Task OnDidReceiveGlobalSettings(ReceivedGlobalSettingsPayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	public virtual Task OnDidReceiveSettings(ReceivedSettingsPayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	public virtual Task OnRpc(JsonObject payload, CancellationToken cancellationToken) => Task.CompletedTask;

	#region disposable
	private bool isDisposed;

	protected virtual void DisposeUnmanaged() { }
	protected virtual void DisposedManaged() { }

	public void Dispose()
	{
		if (isDisposed)
			return;

		DisposedManaged();
		DisposeUnmanaged();
		GC.SuppressFinalize(this);
		isDisposed = true;
	}
	#endregion
}
