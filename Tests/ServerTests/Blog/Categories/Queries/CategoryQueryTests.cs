using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;
using Tests.ServerTests.Helpers;

namespace Tests.ServerTests.Blog.Categories.Queries;

public class CategoryQueryTests
{
    private readonly Fixture fixture;
    private readonly AppDbContext contextFake;
    private readonly DbContextOptions<AppDbContext> options;
    public CategoryQueryTests()
    {
        fixture = new Fixture();
        options = HelperMethods.GenerateOptions();
        contextFake = new (options);
    }

    [Fact]
    public async Task Handle_ShouldRetrieveCategoriesSuccessfully()
    {
        List<Post> posts = fixture.Build<Post>()
            .Without(post => post.Category)
            .CreateMany()
            .ToList();

        List<Category> categories = fixture.Build<Category>()
            .With(cat => cat.Posts, posts)
            .CreateMany()
            .ToList();

        contextFake.AddRange(categories);
        await contextFake.SaveChangesAsync();

        GetCategoriesQueryHandler Handler = new(contextFake);
        List<CategoryDTO> result = await Handler
            .Handle(new GetCategoriesQueryRequest(), new CancellationToken());

        Assert.NotNull(result);
    }
}