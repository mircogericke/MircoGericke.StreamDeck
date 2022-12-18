namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Text.Json.Nodes;

/// <summary>
/// Payload that holds all the settings in the ReceivedGlobalSettings event
/// </summary>
public class ReceivedGlobalSettingsPayload
{
    /// <summary>
    /// Global settings object
    /// </summary>
    public JsonObject? Settings { get; init; }
}
