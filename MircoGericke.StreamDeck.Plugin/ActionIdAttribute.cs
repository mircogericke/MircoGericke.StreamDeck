namespace MircoGericke.StreamDeck.Plugin;

using MircoGericke.StreamDeck.Connection.Model;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ActionIdAttribute : Attribute
{
	public ActionIdAttribute(string id)
	{
		Id = new(id);
	}

	public ActionId Id { get; }
}
