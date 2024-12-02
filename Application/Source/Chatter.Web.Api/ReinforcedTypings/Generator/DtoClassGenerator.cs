using Reinforced.Typings;
using Reinforced.Typings.Ast;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Generators;
using System.Reflection;
using System.Text;

namespace Chatter.Web.Api.ReinforcedTypings.Generator;

public class DtoClassGenerator : ClassCodeGenerator
{
	public override RtClass GenerateNode(Type element, RtClass result, TypeResolver resolver)
	{
		// Create a new class with the name of the DTO
		var className = element.Name;

		if (element.IsGenericType)
		{
			var classGenericTypes = element.GetGenericArguments();

			var genericValues = new StringBuilder();
			for (var i = 0; i < classGenericTypes.Length; i++)
			{
				if (i != 0)
					genericValues.Append(", ");

				genericValues.Append(classGenericTypes[i].Name);
			}

			className = $"{element.Name.Substring(0, element.Name.Length - 2)}<{genericValues}>";
		}

		result = new RtClass { Name = new RtSimpleTypeName(className) };

		// Get all public properties from the DTO
		var properties = element.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		foreach (var property in properties)
		{
			// Create a new property node for each DTO property
			var propNode = new RtField
			{
				Identifier = new RtIdentifier(property.Name),
				Type = resolver.ResolveTypeName(property.PropertyType),
			};

			// Add the property to the class
			result.Members.Add(propNode);
		}

		return result;
	}
}