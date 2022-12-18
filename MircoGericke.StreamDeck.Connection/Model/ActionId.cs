namespace MircoGericke.StreamDeck.Connection.Model;

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

[DebuggerDisplay("{value}")]
[JsonConverter(typeof(ActionIdConverter))]
public readonly struct ActionId : IEquatable<ActionId>
{
	private readonly string value;

	public ActionId(string value)
	{
		this.value = value;
	}

	public override string ToString() => value;
	public override int GetHashCode() => value.GetHashCode();
	public override bool Equals(object? obj) => (obj is ActionId other && Equals(other)) || base.Equals(obj);

	public bool Equals(ActionId other) => Equals(value, other.value);

	public static bool operator ==(ActionId first, ActionId second) => first.Equals(second);
	public static bool operator !=(ActionId first, ActionId second) => !first.Equals(second);
}

public class ActionIdConverter : JsonConverter<ActionId>
{
	public override ActionId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> new(reader.GetString() ?? throw new JsonException("Expected string."));

	public override void Write(Utf8JsonWriter writer, ActionId value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString());
}