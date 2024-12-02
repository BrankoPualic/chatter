using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OData.Query;
using Reinforced.Typings;
using Reinforced.Typings.Ast;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Generators;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Chatter.Web.Api.ReinforcedTypings.Generator;

public class AngularActionCallGenerator : MethodCodeGenerator
{
	public override RtFunction GenerateNode(MethodInfo element, RtFunction result, TypeResolver resolver)
	{
		result = base.GenerateNode(element, result, resolver);
		if (result is null)
		{
			return null;
		}

		// Check parameters for [FromForm] attribute
		var Parameters = element.GetParameters();
		for (int i = 0; i < Parameters.Length; i++)
		{
			var parameter = Parameters[i];
			var fromFormAttribute = parameter.GetCustomAttribute<FromFormAttribute>();
			if (fromFormAttribute != null)
			{
				// If [FromForm] is present, set the parameter type to FormData
				result.Arguments[i].Type = new RtSimpleTypeName("FormData");
			}
		}

		var returnType = result.ReturnType;
		if (returnType is RtSimpleTypeName && ((RtSimpleTypeName)returnType).TypeName == "void")
		{
			returnType = resolver.ResolveTypeName(typeof(object));
		}
		result.ReturnType = new RtSimpleTypeName($"Observable<{returnType}>");

		var allParameters = element.GetParameters().Select(c => c.Name).ToList();
		var parameterTypes = element.GetParameters().Select(c => c.ParameterType).ToList();

		var controller = element.DeclaringType.Name.Replace("Controller", string.Empty);
		var path = $"{controller}/{element.Name}";

		var angularAttribute = (AngularMethodAttribute)Attribute.GetCustomAttribute(element, typeof(AngularMethodAttribute));

		var httpMethodAttribute = (HttpMethodAttribute)Attribute.GetCustomAttribute(element, typeof(HttpMethodAttribute));

		var isAttributeWithoutBody = httpMethodAttribute == null ||
			httpMethodAttribute is HttpGetAttribute ||
			httpMethodAttribute is HttpDeleteAttribute ||
			httpMethodAttribute is HttpOptionsAttribute ||
			httpMethodAttribute is HttpHeadAttribute;

		var parametersInRoute = new List<string>();
		var parametersInRouteFormated = string.Empty;
		if (httpMethodAttribute != null)
		{
			var template = httpMethodAttribute.Template;
			if (!string.IsNullOrEmpty(template))
			{
				parametersInRoute = Regex.Matches(template, "{(.*?)}").Select(m => m.Groups[1].Value).ToList();
				if (parametersInRoute.Any())
				{
					parametersInRouteFormated = template;
					foreach (var parameter in parametersInRoute)
					{
						parametersInRouteFormated = parametersInRouteFormated.Replace($"{{{parameter}}}", $"' + {parameter} + '");
					}

					parametersInRouteFormated = parametersInRouteFormated.Substring(0, parametersInRouteFormated.Length - 4);
				}
			}
		}

		var allowedAnonymousAttribute = Attribute.GetCustomAttribute(element.DeclaringType, typeof(AllowAnonymousAttribute));

		if (allowedAnonymousAttribute == null)
		{
			allowedAnonymousAttribute = Attribute.GetCustomAttribute(element, typeof(AllowAnonymousAttribute));
		}

		var code = new StringBuilder();
		var parameters = allParameters.Except(parametersInRoute).ToList();

		if (parameters.Count == 1 && !isAttributeWithoutBody && parameterTypes[0].BaseType != typeof(ODataQueryOptions))
		{
			code.AppendLine($"const body = <any>{parameters.Single()};");
		}
		else if (parameters.Any() && parameterTypes[0].BaseType != typeof(ODataQueryOptions))
		{
			var joinedParameters = string.Join(",", parameters.Select(p => $"'{p}': {p}"));
			code.AppendLine($"const body = <any>{{{joinedParameters}}};");
		}

		if (isAttributeWithoutBody)
		{
			code.Append($"return this.httpClient.{GetHttpMethod(httpMethodAttribute)}");
			if (returnType.IsString())
			{
				code.AppendLine($"(");
			}
			else
			{
				code.AppendLine($"<{returnType}>(");
			}

			code.Append($"this.settingsService.createApiUrl('{path}')");

			if (parametersInRoute.Any())
			{
				code.Append(" + '/" + parametersInRouteFormated);
			}

			if (parameters.Any() && parameterTypes[0].BaseType == typeof(ODataQueryOptions))
			{
				code.Append(" + queryOptions");
			}

			code.AppendLine(",");

			code.AppendLine("{");
			if (parameters.Any())
			{
				code.AppendLine("\tparams: new HttpParams({ fromObject: body }),");
			}
		}
		else
		{
			code.Append($"return this.httpClient.{GetHttpMethod(httpMethodAttribute)}");
			if (returnType.IsString())
			{
				code.AppendLine($"(");
			}
			else
			{
				code.AppendLine($"<{returnType}>(");
			}

			code.Append($"this.settingsService.createApiUrl('{path}')");

			if (parametersInRoute.Any())
			{
				code.Append(" + '/" + parametersInRouteFormated);
			}

			if (parameters.Any() && parameterTypes[0].BaseType == typeof(ODataQueryOptions))
			{
				code.Append(" + queryOptions");
			}

			code.AppendLine(",");

			if (parameters.Any())
			{
				code.AppendLine("body,");
			}
			else
			{
				code.AppendLine("null,");
			}

			code.AppendLine("{");
		}

		if (angularAttribute.IsArrayBuffer)
		{
			code.AppendLine("\tresponseType: 'arraybuffer',");
		}
		else if (returnType.IsString())
		{
			code.AppendLine("\tresponseType: 'text',");
		}
		else
		{
			code.AppendLine("\tresponseType: 'json',");
		}

		code.AppendLine("\tobserve: 'response',");
		code.AppendLine($"\twithCredentials: {(allowedAnonymousAttribute == null).ToString().ToLower()}");
		code.AppendLine("})");
		code.AppendLine(".pipe(map(response => response.body!));");

		result.Body = new RtRaw(code.ToString());

		return result;
	}

	private string GetHttpMethod(HttpMethodAttribute httpMethodAttribute)
	{
		switch (httpMethodAttribute)
		{
			case HttpPostAttribute _:
				return "post";

			case HttpPutAttribute _:
				return "put";

			case HttpDeleteAttribute _:
				return "delete";

			case HttpPatchAttribute _:
				return "patch";

			case HttpOptionsAttribute _:
				return "options";

			case HttpHeadAttribute _:
				return "head";

			case HttpGetAttribute _:
			default:
				return "get";
		}
	}
}