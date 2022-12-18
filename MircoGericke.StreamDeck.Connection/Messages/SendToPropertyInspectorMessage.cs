namespace MircoGericke.StreamDeck.Connection.Messages;
using System.Text.Json.Nodes;

internal class SendToPropertyInspectorMessage : PluginMessage<JsonObject>
{
	public override string Event => "sendToPropertyInspector";

	public required string Action { get; init; }
}
