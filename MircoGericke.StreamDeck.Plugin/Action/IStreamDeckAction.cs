namespace MircoGericke.StreamDeck.Plugin.Action;
using System;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using MircoGericke.StreamDeck.Connection.Events;
using MircoGericke.StreamDeck.Connection.Payloads;

public interface IStreamDeckAction : IDisposable
{
	Task OnWillAppear(AppearancePayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	Task OnWillDisappear(AppearancePayload payload, CancellationToken cancellationToken) => Task.CompletedTask;

	Task OnPropertyInspectorDidAppear(CancellationToken cancellationToken) => Task.CompletedTask;
	Task OnPropertyInspectorDidDisappear(CancellationToken cancellationToken) => Task.CompletedTask;

	Task OnDidReceiveGlobalSettings(ReceivedGlobalSettingsPayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	Task OnDidReceiveSettings(ReceivedSettingsPayload payload, CancellationToken cancellationToken) => Task.CompletedTask;
	Task OnRpc(JsonObject payload, CancellationToken cancellationToken) => Task.CompletedTask;
}
