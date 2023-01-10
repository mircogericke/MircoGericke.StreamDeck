namespace MircoGericke.StreamDeck.Plugin.Context;

using System.Text.Json.Nodes;

using MircoGericke.StreamDeck.Connection.Messages;
using MircoGericke.StreamDeck.Connection.Model;
using MircoGericke.StreamDeck.Connection.Payloads;

public interface IActionContext
{
    ActionId ActionId { get; }
    ContextId ContextId { get; }
    DeviceId DeviceId { get; }

    ValueTask GetGlobalSettingsAsync(CancellationToken cancellationToken = default);
    ValueTask GetSettingsAsync(CancellationToken cancellationToken = default);
    ValueTask LogAsync(string message, CancellationToken cancellationToken = default);
    ValueTask OpenUrlAsync(Uri uri, CancellationToken cancellationToken = default);
    ValueTask SendToPropertyInspectorAsync(JsonObject payload, CancellationToken cancellationToken = default);
    ValueTask SetFeedbackAsync(Dictionary<string, string> payload, CancellationToken cancellationToken = default);
    ValueTask SetGlobalSettingsAsync(JsonObject payload, CancellationToken cancellationToken = default);
    ValueTask SetSettingsAsync(JsonObject payload, CancellationToken cancellationToken = default);
    ValueTask SetImageAsync(SetImagePayload payload, CancellationToken cancellationToken = default);
    ValueTask SetStateAsync(uint payload, CancellationToken cancellationToken = default);
    ValueTask SetTitleAsync(SetTitlePayload payload, CancellationToken cancellationToken = default);
    ValueTask ShowAlertAsync(CancellationToken cancellationToken = default);
    ValueTask ShowOkAsync(CancellationToken cancellationToken = default);
    ValueTask SwitchToProfileAsync(string profileName, CancellationToken cancellationToken = default, DeviceId? deviceId = default);
}
