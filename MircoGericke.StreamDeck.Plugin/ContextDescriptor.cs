namespace MircoGericke.StreamDeck.Plugin;

using Microsoft.Extensions.DependencyInjection;

internal class ContextDescriptor
{
	public required IServiceScope Scope { get; init; }
	public required Action Instance { get; init; }
}
