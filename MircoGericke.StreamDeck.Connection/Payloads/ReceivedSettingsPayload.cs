namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Text.Json.Nodes;

using MircoGericke.StreamDeck.Connection.Model;

/// <summary>
/// Payload that holds all the settings in the ReceivedSettings event
/// </summary>
public class ReceivedSettingsPayload
{
	/// <summary>
	/// Action's settings
	/// </summary>
	public JsonObject? Settings { get; init; }
	public required KeyCoordinate Coordinates { get; init; }
	public required bool IsInMultiAction { get; init; }
}
