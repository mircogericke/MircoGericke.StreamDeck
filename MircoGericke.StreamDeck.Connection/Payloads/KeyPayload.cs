namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Text.Json.Nodes;
using MircoGericke.StreamDeck.Connection.Model;

/// <summary>
/// Payload received when a key is pressed or released
/// </summary>
public class KeyPayload
{
    /// <summary>
    /// Current event settings
    /// </summary>
    public required JsonObject Settings { get; init; }
    public required KeyCoordinate Coordinates { get; init; }
    public uint? State { get; init; }
    public uint? UserDesiredState { get; init; }
    public required bool IsInMultiAction { get; init; }
}
