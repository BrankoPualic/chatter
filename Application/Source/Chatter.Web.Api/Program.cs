using Chatter.DependencyRegister;
using Chatter.Domain;
using Chatter.Persistence;
using Chatter.Web.Api;
using Chatter.Web.Api.Middlewares;
using Chatter.Web.Api.Objects;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
	.AddNewtonsoftJson(options =>
	{
		options.SerializerSettings.ContractResolver = new ApplicationContractResolver();
		options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

		options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
		options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

		options.SerializerSettings.Error = (sender, args) => throw args.ErrorContext.Error;
	})
	.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddScoped<IIdentityUser, IdentityUser>();
builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AllAplicationServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

	try
	{
		var migrationResult = context.Migrate();

		logger.LogInformation("{result}", migrationResult.Result);
		migrationResult.Migrations.ForEach(_ => logger.LogInformation("{log}", _));
	}
	catch (Exception ex)
	{
		logger.LogError("MIGRATION - FAILED");
		logger.LogError(ex, "An error occurred while migrating the database.");
	}
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder
	.AllowAnyHeader()
	.AllowAnyMethod()
	.AllowCredentials()
	.WithOrigins(Chatter.Common.Settings.Audience)
	);

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();

app.MapControllers();

app.Run();