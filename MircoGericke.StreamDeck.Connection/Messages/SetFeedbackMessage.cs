namespace MircoGericke.StreamDeck.Connection.Messages;
using System.Collections.Generic;

public class SetFeedbackMessage : ContextMessage<Dictionary<string, string>>
{
	public override string Event => "setFeedback";
}
