namespace MircoGericke.StreamDeck.Connection.Messages;

public class RegisterEventMessage : StreamDeckMessage
{
	public override string Event { get; }
	public string Uuid { get; }

	public RegisterEventMessage(string evt, string uuid)
	{
		Event = evt;
		Uuid = uuid;
	}
}
