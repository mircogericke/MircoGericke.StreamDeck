namespace MircoGericke.StreamDeck.Plugin;
using System;

using MircoGericke.StreamDeck.Connection.Model;

public class ActionDescriptor
{
	public required Type Type { get; init; }
	public required ActionId Id { get; init; }
}