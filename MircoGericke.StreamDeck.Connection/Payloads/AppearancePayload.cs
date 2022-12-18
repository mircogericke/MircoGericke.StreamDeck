namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Text.Json.Nodes;

using MircoGericke.StreamDeck.Connection.Model;

public class AppearancePayload
{
	public required JsonObject Settings { get; init; }
	public required KeyCoordinate Coordinates { get; init; }
	public uint? State { get; set; }
	public required bool IsInMultiAction { get; init; }
	public required string Controller { get; init; }
}
