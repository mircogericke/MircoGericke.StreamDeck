namespace MircoGericke.StreamDeck.Connection.Model;

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

[DebuggerDisplay("{value}")]
[JsonConverter(typeof(DeviceIdConverter))]
public readonly struct DeviceId : IEquatable<DeviceId>
{
	private readonly string value;

	public DeviceId(string value)
	{
		this.value = value;
	}

	public override string ToString() => value;
	public override int GetHashCode() => value.GetHashCode();
	public override bool Equals(object? obj) => (obj is DeviceId other && Equals(other)) || base.Equals(obj);

	public bool Equals(DeviceId other) => Equals(value, other.value);

	public static bool operator ==(DeviceId first, DeviceId second) => first.Equals(second);
	public static bool operator !=(DeviceId first, DeviceId second) => !first.Equals(second);
}

public class DeviceIdConverter : JsonConverter<DeviceId>
{
	public override DeviceId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> new(reader.GetString() ?? throw new JsonException("Expected string."));

	public override void Write(Utf8JsonWriter writer, DeviceId value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString());
}