namespace Chatter.Domain;

[AttributeUsage(AttributeTargets.Field)]
public class CssClassAttribute(string cssClass) : Attribute
{
	public string CssClass { get; set; } = cssClass;
}