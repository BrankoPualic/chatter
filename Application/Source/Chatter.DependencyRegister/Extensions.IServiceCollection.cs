﻿using Chatter.Application.Behaviors;
using Chatter.Application.Identity.Interfaces;
using Chatter.Application.Identity.Services;
using Chatter.Application.Interfaces;
using Chatter.Application.Services;
using Chatter.Application.UseCases;
using Chatter.Domain;
using Chatter.Domain.Interfaces.Cloudinary;
using Chatter.Infrastructure.Storage;
using Chatter.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Chatter.DependencyRegister;

public static class Extensions
{
	public static IServiceCollection AllAplicationServices(this IServiceCollection services)
	{
		PersistenceServices(services);
		ApplicationServices(services);
		ApplicationIdentityService(services);
		InfrastructureServices(services);

		return services;
	}

	private static IServiceCollection PersistenceServices(IServiceCollection services)
	{
		services.AddScoped<IDatabaseContext, DatabaseContext>();
		return services;
	}

	private static IServiceCollection ApplicationServices(IServiceCollection services)
	{
		var assembly = typeof(BaseCommand).Assembly;

		services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
		services.AddValidatorsFromAssembly(assembly);
		services.AddAutoMapper(assembly);

		ApplicationPipelineBehaviors(services);

		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IUserManager, UserManager>();

		return services;
	}

	private static IServiceCollection ApplicationIdentityService(IServiceCollection services)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = Chatter.Common.Settings.Issuer,
				ValidAudience = Chatter.Common.Settings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Chatter.Common.Settings.JwtKey)),
				ClockSkew = TimeSpan.Zero
			};

			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					var accessToken = context.Request.Query["access_token"];

					var path = context.HttpContext.Request.Path;
					if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
						context.Token = accessToken;

					return Task.CompletedTask;
				}
			};
		});

		return services;
	}

	private static IServiceCollection InfrastructureServices(IServiceCollection services)
	{
		services.AddScoped<ICloudinaryService, CloudinaryService>();
		services.AddScoped<IBlobService, BlobService>();

		return services;
	}

	private static IServiceCollection ApplicationPipelineBehaviors(IServiceCollection services)
	{
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceMonitoringBehavior<,>));

		return services;
	}
}