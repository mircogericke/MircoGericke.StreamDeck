namespace MircoGericke.StreamDeck.Connection.Messages;

using System.Text.Json.Nodes;

public class SetSettingsMessage : ContextMessage<JsonObject>
{
	public override string Event => "setSettings";
}
