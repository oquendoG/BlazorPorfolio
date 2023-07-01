using Server.Data;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;

namespace Server.Extensions;

public static class AppServiceExtensions
{
    public static void AddServiceExtensions(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCategoriesHandler).Assembly));
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("corspolicy", builder =>
                                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
    }
}
