using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Store.Api.Contracts.Requests;
using Store.Api.Contracts.Validators;
using Store.Api.Security;

namespace Store.Api.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Security",
        };

        var securityReq = new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        };

        services.AddSwaggerGen(o =>
        {
            o.AddSecurityDefinition("Bearer", securityScheme);
            o.AddSecurityRequirement(securityReq);
        });
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtConfig jwtConfig)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = jwtConfig.Key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<CartRequest>, CartRequestValidator>();
        services.AddScoped<IValidator<ItemRequest>, ItemRequestValidator>();
        services.AddScoped<IValidator<OrderRequest>, OrderRequestValidator>();
        services.AddScoped<IValidator<PagedRequest>, PagedRequestValidator>();
        services.AddScoped<IValidator<AuthRequest>, AuthRequestValidator>();
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
        services.AddScoped<IValidator<UpdatePasswordRequest>, UpdatePasswordRequestValidator>();
        return services;
    }

    public static JwtConfig GetJwtConfig(this ConfigurationManager configuration)
    {
        return new JwtConfig(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], configuration["Jwt:Key"]);
    }
}