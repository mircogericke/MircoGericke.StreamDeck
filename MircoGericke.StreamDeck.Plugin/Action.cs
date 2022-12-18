namespace MircoGericke.StreamDeck.Plugin;

public abstract class Action : IDisposable
{
	internal static Action Missing { get; } = new MissingAction();

	private bool disposedValue;

	private class MissingAction : Action
	{
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
