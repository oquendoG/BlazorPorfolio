using Server.Feats.Blog.Categories.Queries;

namespace Server.Extensions;

public static class AppServiceExtensions
{
    public static void AddServiceExtensions(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCategoriesHandler).Assembly));
    }
}
