namespace MircoGericke.StreamDeck.Connection.Model;

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

[DebuggerDisplay("{value}")]
[JsonConverter(typeof(ContextIdConverter))]
public readonly struct ContextId : IEquatable<ContextId>
{
	private readonly string value;

	public ContextId(string value)
	{
		this.value = value;
	}

	public override string ToString() => value;
	public override int GetHashCode() => value.GetHashCode();
	public override bool Equals(object? obj) => (obj is ContextId other && Equals(other)) || base.Equals(obj);

	public bool Equals(ContextId other) => Equals(value, other.value);

	public static bool operator ==(ContextId first, ContextId second) => first.Equals(second);
	public static bool operator !=(ContextId first, ContextId second) => !first.Equals(second);
}

public class ContextIdConverter : JsonConverter<ContextId>
{
	public override ContextId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> new(reader.GetString() ?? throw new JsonException("Expected string."));

	public override void Write(Utf8JsonWriter writer, ContextId value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString());
}