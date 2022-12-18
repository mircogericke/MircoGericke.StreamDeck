namespace MircoGericke.StreamDeck.Connection.Util;
using System;
using System.Buffers;

internal class MemorySegment<T> : ReadOnlySequenceSegment<T>
{
	private readonly IMemoryOwner<T>? owner;
	public MemorySegment(ReadOnlyMemory<T> memory, IMemoryOwner<T>? owner = null)
	{
		Memory = memory;
		this.owner = owner;
	}

	public MemorySegment<T> Append(ReadOnlyMemory<T> memory, IMemoryOwner<T>? owner = null)
	{
		var segment = new MemorySegment<T>(memory, owner)
		{
			RunningIndex = RunningIndex + Memory.Length
		};

		Next = segment;

		return segment;
	}

	public void Release()
	{
		ReadOnlySequenceSegment<T>? c = this;
		while (c is not null)
		{
			if (c is MemorySegment<T> mem)
				mem.owner?.Dispose();
			c = c.Next;
		}
	}
}
