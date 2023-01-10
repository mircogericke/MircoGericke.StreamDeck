namespace MircoGericke.StreamDeck.Plugin;

using Microsoft.Extensions.DependencyInjection;

using MircoGericke.StreamDeck.Plugin.Action;

internal class ContextDescriptor
{
    public required IServiceScope Scope { get; init; }
    public required StreamDeckAction Instance { get; init; }
}
