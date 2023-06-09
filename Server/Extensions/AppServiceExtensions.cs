﻿using Mapster;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Server.Feats.Blog.Posts.DTOs;
using Shared.Models.Blog;

namespace Server.Extensions; 

public static class AppServiceExtensions
{
    public static void AddMediatrConfigs(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCategoriesQueryHandler).Assembly));

        TypeAdapterConfig<Category, CategoryPostsDTO>
            .NewConfig()
            .TwoWays()
            .MaxDepth(2)
            .Fork(config => config.Default.PreserveReference(true));
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("corspolicy", builder =>
                                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
    }
}
