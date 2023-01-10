namespace MircoGericke.StreamDeck.Connection.Messages;

using System.Diagnostics.CodeAnalysis;

using MircoGericke.StreamDeck.Connection.Payloads;

public class LogMessage : StreamDeckMessage<LogPayload>
{
	public override string Event => "logMessage";

	[SetsRequiredMembers]
	public LogMessage(string message)
	{
		Payload = new() { Message = message };
	}
}
