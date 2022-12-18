namespace MircoGericke.StreamDeck.Connection.Messages;

using System.Text.Json.Serialization;

internal class GetGlobalSettingsMessage : PluginMessage
{
	public override string Event => "getGlobalSettings";
}
