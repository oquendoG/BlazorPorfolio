using Mapster;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Tests.ServerTests.Configurations;
public static class Configuration
{
    public static TypeAdapterConfig MapsterConfigurationForCategory()
    {
        TypeAdapterConfig config = new();

        config.NewConfig<CategoryPostsDTO, Category>()
        .TwoWays()
        .MaxDepth(2)
        .PreserveReference(true);

        config.NewConfig<PostDTO, Post>()
        .TwoWays()
        .MaxDepth(1)
        .Ignore(dest => dest.Category)
        .PreserveReference(true);

        config.NewConfig<List<CategoryPostsDTO>, List<Category>>()
        .MaxDepth(2)
        .PreserveReference(true);

        config.NewConfig<List<PostDTO>, List<Post>>()
       .MaxDepth(2)
       .TwoWays()
       .PreserveReference(true);

        return config;
    }
}
