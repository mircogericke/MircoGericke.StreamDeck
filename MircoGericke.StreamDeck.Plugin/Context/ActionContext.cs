namespace MircoGericke.StreamDeck.Plugin.Context;

using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using MircoGericke.StreamDeck.Connection.Messages;
using MircoGericke.StreamDeck.Connection.Model;
using MircoGericke.StreamDeck.Connection.Payloads;

internal class ActionContext : IActionContext
{
    private readonly PluginManager manager;

    public ActionContext(PluginManager manager)
    {
        this.manager = manager;
    }

    public ActionId ActionId { get; set; }
    public ContextId ContextId { get; set; }
    public DeviceId DeviceId { get; set; }

    public ValueTask GetGlobalSettingsAsync(CancellationToken cancellationToken = default)
        => manager.SendAsync(new GetGlobalSettingsMessage() { ContextId = ContextId }, cancellationToken);

    public ValueTask GetSettingsAsync(CancellationToken cancellationToken = default)
        => manager.SendAsync(new GetSettingsMessage() { ContextId = ContextId }, cancellationToken);

    public ValueTask LogAsync(string message, CancellationToken cancellationToken = default)
        => manager.SendAsync(new LogMessage(message), cancellationToken);

    public ValueTask OpenUrlAsync(Uri uri, CancellationToken cancellationToken = default)
        => manager.SendAsync(new OpenUrlMessage(uri), cancellationToken);

    public ValueTask SendToPropertyInspectorAsync(JsonObject payload, CancellationToken cancellationToken = default)
        => manager.SendAsync(new SendToPropertyInspectorMessage()
        {
            ActionId = ActionId,
            ContextId = ContextId,
            Payload = payload,
        }, cancellationToken);

    public ValueTask SetFeedbackAsync(Dictionary<string, string> payload, CancellationToken cancellationToken = default)
        => manager.SendAsync(new SetFeedbackMessage()
        {
            ContextId = ContextId,
            Payload = payload,
        }, cancellationToken);

    public ValueTask SetGlobalSettingsAsync(JsonObject payload, CancellationToken cancellationToken = default)
        => manager.SendAsync(new SetGlobalSettingsMessage()
        {
            ContextId = ContextId,
            Payload = payload,
        }, cancellationToken);

    public ValueTask SetImageAsync(SetImagePayload payload, CancellationToken cancellationToken = default)
        => manager.SendAsync(new SetImageMessage()
        {
            ContextId = ContextId,
            Payload = payload,
        }, cancellationToken);

    public ValueTask SetSettingsAsync(JsonObject payload, CancellationToken cancellationToken = default)
        => manager.SendAsync(new SetSettingsMessage()
        {
            ContextId = ContextId,
            Payload = payload,
        }, cancellationToken);

    public ValueTask SetStateAsync(uint payload, CancellationToken cancellationToken = default)
        => manager.SendAsync(new SetStateMessage()
        {
            ContextId = ContextId,
            Payload = new() { State = payload },
        }, cancellationToken);

    public ValueTask SetTitleAsync(SetTitlePayload payload, CancellationToken cancellationToken = default)
        => manager.SendAsync(new SetTitleMessage()
        {
            ContextId = ContextId,
            Payload = payload,
        }, cancellationToken);

    public ValueTask ShowAlertAsync(CancellationToken cancellationToken = default)
        => manager.SendAsync(new ShowAlertMessage()
        {
            ContextId = ContextId,
        }, cancellationToken);

    public ValueTask ShowOkAsync(CancellationToken cancellationToken = default)
        => manager.SendAsync(new ShowOkMessage()
        {
            ContextId = ContextId,
        }, cancellationToken);

    public ValueTask SwitchToProfileAsync(string profileName, CancellationToken cancellationToken = default, DeviceId? deviceId = default)
        => manager.SendAsync(new SwitchToProfileMessage()
        {
            ContextId = ContextId,
            DeviceId = deviceId ?? DeviceId,
            Payload = new() { Profile = profileName },
        }, cancellationToken);
}
