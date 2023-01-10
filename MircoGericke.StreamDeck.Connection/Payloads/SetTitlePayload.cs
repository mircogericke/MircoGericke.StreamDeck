namespace MircoGericke.StreamDeck.Connection.Payloads;
using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;

public class SetTitlePayload
{
	public required string Title { get; init; }

	public required SdkTarget Target { get; init; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? State { get; init; }
}
