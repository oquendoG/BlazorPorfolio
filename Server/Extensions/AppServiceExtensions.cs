using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;
using System.Text;

namespace Server.Extensions;

public static class AppServiceExtensions
{
    public static void AddMapsterConfigs(this IServiceCollection services)
    {
        TypeAdapterConfig<CategoryPostsDTO, Category>.NewConfig()
            .TwoWays()
            .MaxDepth(2)
            .PreserveReference(true);

        TypeAdapterConfig<PostDTO, Post>.NewConfig()
            .TwoWays()
            .PreserveReference(true);

        TypeAdapterConfig<List<CategoryPostsDTO>, List<Category>>
            .NewConfig()
            .MaxDepth(2)
            .PreserveReference(true);

        TypeAdapterConfig<List<PostDTO>, List<Post>>
            .NewConfig()
            .MaxDepth(2)
            .TwoWays()
            .PreserveReference(true);

        services.AddSingleton(serviceProvider => TypeAdapterConfig.GlobalSettings);
    }

    public static void AddMediatrConfigs(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCategoriesQueryHandler).Assembly));
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy(name: "CorsPolicy", builder =>
                                    builder
                                    .AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()));
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration[key: "Jwt:Issuer"],
                    ValidAudience = configuration[key: "Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
    }
}