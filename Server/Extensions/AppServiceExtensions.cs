using Mapster;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;

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

        services.AddSingleton(sp => TypeAdapterConfig.GlobalSettings);
    }

    public static void AddMediatrConfigs(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCategoriesQueryHandler).Assembly));
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("corspolicy", builder =>
                                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
    }
}