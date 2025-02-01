using Chatter.Common;
using Chatter.Domain;
using Chatter.Web.Api.Controllers._Base;
using Chatter.Web.Api.Objects;
using Chatter.Web.Api.ReinforcedTypings.Generator;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Attributes;
using Reinforced.Typings.Fluent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Chatter.Web.Api.ReinforcedTypings;

public static class FluentConfiguration
{
	private static readonly Action<ClassExportBuilder> _classConfiguration = config =>
		config.WithPublicProperties()
		.ConfigureTypeMapping()
		//.ExportTo("classes.ts")
		.OverrideNamespace("api")
		.WithCodeGenerator<DtoClassGenerator>();

	private static readonly Action<ClassExportBuilder> _serviceConfiguration = config =>
		config
		.AddImport("{ Injectable }", "@angular/core")
		.AddImport("{ HttpParams, HttpClient }", "@angular/common/http")
		.AddImport("{ SettingsService }", "../services/settings.service")
		.AddImport("{ Observable }", "rxjs")
		.AddImport("{ map }", "rxjs/operators")
		//.ExportTo("services.ts")
		.ConfigureTypeMapping()
		.OverrideNamespace("api.Controller")
		.WithCodeGenerator<AngularControllerGenerator>();

	private static readonly Action<EnumExportBuilder> _enumProviderConfiguration = config =>
		config
		.AddImport("{ Injectable }", "@angular/core")
		//.ExportTo("providers.ts")
		.OverrideNamespace("api")
		.WithCodeGenerator<EnumProviderGenerator>();

	public static void Configure(Reinforced.Typings.Fluent.ConfigurationBuilder builder)
	{
		builder.Global(config => config.CamelCaseForProperties()
								.DontWriteWarningComment()
							   .AutoOptionalProperties()
							   .UseModules(true, false));

		// Controllers

		builder.ExportAsClasses(
			Assembly.GetAssembly(typeof(BaseController)).ExportedTypes
			.Where(i => i.Namespace.StartsWith($"{Constants.SOLUTION_NAME}.Web.Api.Controllers"))
			.OrderBy(i => i.Name)
			.OrderBy(i => i.Name != nameof(BaseController))
			.ToArray(),
			_serviceConfiguration
			);

		// Dtos

		var coreAssembly = Assembly.Load($"{Constants.SOLUTION_NAME}.Application");
		var dtos = coreAssembly
			.GetTypes()
			.Where(t => t.IsClass
				&& t.Namespace != null
				&& (t.Namespace.Contains($"{Constants.SOLUTION_NAME}.Application.Dtos") || t.Namespace.Contains($"{Constants.SOLUTION_NAME}.Application.Search"))
				&& !t.IsDefined(typeof(CompilerGeneratedAttribute), false)
				&& !t.IsDefined(typeof(TsIgnoreAttribute), false)
				&& !t.Name.Contains("Validator"));

		// Dto Classes

		var additionalClasses = new List<Type>([typeof(EnumProvider)]);
		builder.ExportAsClasses(
			dtos.Concat(additionalClasses)
			.OrderBy(i => i.Name)
			.ToArray(),
			_classConfiguration
			);

		// Enum Providers
		builder.ExportAsEnums(
			[typeof(Providers)],
			_enumProviderConfiguration
			);

		// Enums

		Type[] enums = [
			typeof(eSystemRole),
			typeof(eGender),
			typeof(eMessageStatus),
			typeof(eMessageType),
			typeof(eChatRole),
		];

		builder.ExportAsEnums(enums,
			config =>
				config
				.OverrideNamespace("api")
			//.ExportTo("enums.ts")
			);
	}

	private static TBuilder ConfigureTypeMapping<TBuilder>(this TBuilder config)
		where TBuilder : ClassOrInterfaceExportBuilder
	{
		config
			.Substitute(typeof(Guid), new RtSimpleTypeName("string"))
			.Substitute(typeof(DateTime), new RtSimpleTypeName("Date"))
			.Substitute(typeof(DateOnly), new RtSimpleTypeName("Date"))
			.Substitute(typeof(IFormFile), new RtSimpleTypeName("File"));
		//.Substitute(typeof(Guid?), new RtSimpleTypeName("string | null"));

		return config;
	}
}