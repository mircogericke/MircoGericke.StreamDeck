namespace MircoGericke.StreamDeck.Connection.Messages;

public class GetGlobalSettingsMessage : ContextMessage
{
	public override string Event => "getGlobalSettings";
}
