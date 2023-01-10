namespace MircoGericke.StreamDeck.Connection.Messages;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;

public class SendToPropertyInspectorMessage : ContextMessage<JsonObject>
{
	public override string Event => "sendToPropertyInspector";

	[JsonPropertyName("action")]
	public required ActionId ActionId { get; init; }
}
