namespace MircoGericke.StreamDeck.Connection.Messages;
using System.Text.Json.Nodes;

internal class SetGlobalSettingsMessage : PluginMessage<JsonObject>
{
	public override string Event => "setGlobalSettings";
}
