namespace MircoGericke.StreamDeck.Plugin.Action;
using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using MircoGericke.StreamDeck.Connection.Payloads;

public interface IStreamDeckAction : IDisposable, IAsyncDisposable
{
	Task InitializeAsync(AppearancePayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	Task OnDidReceiveGlboalSettings(ReceivedGlobalSettingsPayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	Task OnDidReceiveSettings(ReceivedSettingsPayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	Task OnRpc(JsonObject payload, CancellationToken cancellationToken) => Task.CompletedTask;
}
