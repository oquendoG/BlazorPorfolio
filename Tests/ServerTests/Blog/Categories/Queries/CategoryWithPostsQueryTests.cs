using AutoFixture;
using FluentAssertions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;
using Tests.Configurations;
using Tests.ServerTests.Helpers;

namespace Tests.ServerTests.Blog.Categories.Queries;

public class CategoryWithPostsQueryTests
{
    private readonly Fixture fixture;
    private readonly AppDbContext contextFake;
    private readonly TypeAdapterConfig config;
    private readonly DbContextOptions<AppDbContext> options;
    private readonly (List<Category>, List<Post>) dataGenerated;
    private List<Category> receivedCategories = new();

    public CategoryWithPostsQueryTests()
    {
        config = Configuration.MapsterConfigurationForCategory();
        fixture = new Fixture();
        options = HelperMethods.GenerateOptions();
        contextFake = new(options);
        dataGenerated = CreateTestData();
    }


    [Fact]
    public async Task Handle_ShouldRetrieveCategoriesSuccessfully()
    {
        receivedCategories = await GetDataFromDataBaseAsync();
        receivedCategories.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_ShouldRetrieveSameCount()
    {
        receivedCategories = await GetDataFromDataBaseAsync();
        receivedCategories.Should().HaveCount(dataGenerated.Item1.Count);
    }

    [Fact]
    public async Task Handle_ShoudRetrieveCategoriesWithPosts()
    {
        receivedCategories = await GetDataFromDataBaseAsync();
        //To avoid cyclic redundancy
        List<Post> postsWithoutCategories =
            receivedCategories
            .SelectMany(cat => cat.Posts
                .Select(post => new Post()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Excerpt = post.Excerpt,
                    Thumbnailimage = post.Thumbnailimage,
                    Content = post.Content,
                    PublishDate = post.PublishDate,
                    Published = post.Published,
                    Author = post.Author,   
                    CategoryId = post.CategoryId
                }))
            .ToList();

        /*the last one because of AsNotracking(), AsNoTracking for some reason works well
        in production but not in tests*/
        receivedCategories[2].Posts.Should().BeEquivalentTo(postsWithoutCategories);
    }

    private (List<Category>, List<Post>) CreateTestData()
    {
        List<Post> posts = fixture.Build<Post>()
           .Without(post => post.Category)
           .CreateMany(3)
           .ToList();

        contextFake.AddRange(posts);
        contextFake.SaveChanges();

        List<Category> categories = fixture.Build<Category>()
            .With(cat => cat.Posts, posts)
            .CreateMany(3)
            .ToList();

        contextFake.AddRange(categories);
        contextFake.SaveChanges();

        return (categories, posts);
    }

    private async Task<List<Category>> GetDataFromDataBaseAsync()
    {
        GetCategoriesWithPostsQueryHandler Handler = new(contextFake);
        List<CategoryPostsDTO> result = await Handler
            .Handle(new GetCategoriesWithPostsQueryRequest(), new CancellationToken());

        List<Category> receivedCategories = result
            .Adapt<List<CategoryPostsDTO>, List<Category>>(config);

        return receivedCategories;
    }
}