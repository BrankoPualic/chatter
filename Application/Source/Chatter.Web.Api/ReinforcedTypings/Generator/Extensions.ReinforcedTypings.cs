using Reinforced.Typings.Ast.TypeNames;

namespace Chatter.Web.Api.ReinforcedTypings.Generator;

public static class Extensions
{
	public static bool IsString(this RtTypeName typeName) => typeName.ToString().ToLower() == "string";
}