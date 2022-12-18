namespace MircoGericke.StreamDeck.Connection.Messages;

using System.Text.Json.Nodes;

internal class SetSettingsMessage : PluginMessage<JsonObject>
{
	public override string Event => "setSettings";
}
