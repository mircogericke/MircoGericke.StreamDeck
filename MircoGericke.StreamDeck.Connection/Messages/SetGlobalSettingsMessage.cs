namespace MircoGericke.StreamDeck.Connection.Messages;
using System.Text.Json.Nodes;

public class SetGlobalSettingsMessage : ContextMessage<JsonObject>
{
	public override string Event => "setGlobalSettings";
}
