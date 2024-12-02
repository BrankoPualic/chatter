using Reinforced.Typings.Attributes;

namespace Chatter.Web.Api.ReinforcedTypings.Generator;

public class AngularMethodAttribute : TsFunctionAttribute
{
	public AngularMethodAttribute(Type returnType)
	{
		StrongType = returnType;

		CodeGeneratorType = typeof(AngularActionCallGenerator);
	}

	public bool IsArrayBuffer { get; set; }
}