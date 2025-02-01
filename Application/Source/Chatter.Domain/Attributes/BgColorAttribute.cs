namespace Chatter.Domain;

[AttributeUsage(AttributeTargets.Field)]
public class BgColorAttribute(string color) : Attribute
{
	public string BgColor { get; } = color;
}