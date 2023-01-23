namespace MircoGericke.StreamDeck.Plugin;

using Microsoft.Extensions.DependencyInjection;

using MircoGericke.StreamDeck.Plugin.Action;

internal sealed class ContextDescriptor : IDisposable
{
    public required IServiceScope Scope { get; init; }
    public required IStreamDeckAction Instance { get; init; }

	public void Dispose()
  {
    Instance?.Dispose();
    Scope?.Dispose();
  }
}
