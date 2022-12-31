namespace MircoGericke.StreamDeck.Connection.Messages;

internal class GetGlobalSettingsMessage : PluginMessage
{
	public override string Event => "getGlobalSettings";
}
