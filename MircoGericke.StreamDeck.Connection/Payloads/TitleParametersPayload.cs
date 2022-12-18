namespace MircoGericke.StreamDeck.Connection.Payloads;
using System.Text.Json.Nodes;

using MircoGericke.StreamDeck.Connection.Model;

/// <summary>
/// Payload for TitleParametersDidChange Event
/// </summary>
public class TitleParametersPayload
{
	/// <summary>
	/// Settings JSON Object
	/// </summary>
	public JsonObject? Settings { get; init; }

	public KeyCoordinate Coordinates { get; init; }

	public uint State { get; init; }

	public required string Title { get; init; }

	// TODO: class definition
	public required JsonObject TitleParameters { get; init; }
}
