using Mapster;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;

namespace Server.Extensions; 

public static class AppServiceExtensions
{
    public static void AddMediatrConfigs(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCategoriesHandler).Assembly));

        TypeAdapterConfig<Category, CategoryPostsDTO>
            .NewConfig()
            .MaxDepth(2)
            .Fork(config => config.Default.PreserveReference(true));
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("corspolicy", builder =>
                                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
    }
}
