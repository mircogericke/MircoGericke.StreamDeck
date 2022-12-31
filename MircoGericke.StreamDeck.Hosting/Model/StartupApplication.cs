namespace MircoGericke.StreamDeck.Hosting.Model;

public class StartupApplication
{
	public required string Font { get; init; }
	public required string Language { get; init; }
	public required string Platform { get; init; }
	public required string PlatformVersion { get; init; }
	public required string Version { get; init; }
}
