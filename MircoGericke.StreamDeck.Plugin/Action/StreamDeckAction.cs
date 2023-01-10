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

	public virtual Task InitializeAsync(AppearancePayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	public virtual Task OnDidReceiveGlboalSettings(ReceivedGlobalSettingsPayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	public virtual Task OnDidReceiveSettings(ReceivedSettingsPayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	public virtual Task OnRpc(JsonObject payload, CancellationToken cancellationToken) => Task.CompletedTask;

	#region (async) disposable
	private bool disposedValue;

	protected virtual void DisposeCore(bool disposeManaged)
	{
		if (!disposedValue)
		{
			disposedValue = true;
		}
	}

	protected virtual ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;

	public void Dispose()
	{
		DisposeCore(disposeManaged: true);
		GC.SuppressFinalize(this);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		DisposeCore(disposeManaged: false);
		GC.SuppressFinalize(this);
	}
	#endregion
}
