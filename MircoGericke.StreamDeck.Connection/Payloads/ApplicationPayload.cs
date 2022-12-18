namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Text.Json.Serialization;

public class ApplicationPayload
{
    [JsonPropertyName("application")]
    public required string Name { get; init; }
}
