using Chatter.DependencyRegister;
using Chatter.Persistence;
using Chatter.Web.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.PropertyNamingPolicy = null;
		options.JsonSerializerOptions.DictionaryKeyPolicy = null;
	});
builder.Services.AddHttpContextAccessor();

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

app.MapControllers();

app.Run();