using AutoFixture;
using FluentAssertions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;
using Tests.ServerTests.Helpers;

namespace Tests.ServerTests.Blog.Categories.Queries;
public class CategoryQueryTests
{

    private readonly AppDbContext contextFake;
    private readonly DbContextOptions<AppDbContext> dbContextOptions;
    private readonly Fixture fixture;
    private List<Category> categories = [];
    private readonly List<Category> generatedCategoriesWithoutPosts = [];
    private readonly List<Category> generatedCategoriesWithPosts = [];

    public CategoryQueryTests()
    {
        dbContextOptions = HelperMethods.GenerateOptions();
        contextFake = new(dbContextOptions);
        fixture = new Fixture();
        generatedCategoriesWithoutPosts = CreateTestDataForCategoriesWithoutPosts();
        generatedCategoriesWithPosts = CreateTestDataForCategoriesWithPosts();
    }

    private List<Category> CreateTestDataForCategoriesWithoutPosts()
    {
        List<Category> categories = fixture.Build<Category>()
            .Without(cat => cat.Posts)
            .CreateMany(5)
            .ToList();

        contextFake.AddRange(categories);
        contextFake.SaveChanges();

        return categories;
    }

    private List<Category> CreateTestDataForCategoriesWithPosts()
    {
        List<Post> posts = fixture.Build<Post>()
            .Without(post => post.Category)
            .CreateMany(5)
            .ToList();

        List<Category> categories = fixture.Build<Category>()
            .With(cat => cat.Posts, posts)
            .CreateMany(5)
            .ToList();

        contextFake.AddRange(categories);
        contextFake.SaveChanges();

        return categories;
    }

    [Fact]
    public async Task Handle_ShouldRetieveCategoriesSuccesfully()
    {
        categories = await GetDataFromDataBase();
        categories.Should().NotBeNullOrEmpty();

    }

    private async Task<List<Category>> GetDataFromDataBase()
    {
        GetCategoriesQueryHandler Handler = new(contextFake);
        List<CategoryDTO> result = await Handler
            .Handle(new GetCategoriesQueryRequest(), new CancellationToken());

        List<Category> receivedCategories = result
            .Adapt<List<CategoryDTO>, List<Category>>();

        return receivedCategories;
    }

    [Fact]
    public async Task Handle_ShouldRetrieveCategoryById()
    {
        GetCategoryByIdQueryhandler Handler = new(contextFake);
        CategoryPostsDTO result = await Handler
            .Handle(new GetCategoryByIdQueryRequest(generatedCategoriesWithoutPosts[0].Id, false), new CancellationToken());

        Category receivedCategory = result
            .Adapt<CategoryPostsDTO, Category>();

        receivedCategory.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldRetrieveCategoryByIdWithPosts()
    {
        GetCategoryByIdQueryhandler Handler = new(contextFake);
        CategoryPostsDTO result = await Handler
            .Handle(new GetCategoryByIdQueryRequest(
                generatedCategoriesWithPosts[^1].Id, true), 
                new CancellationToken());

        Category receivedCategory = result
            .Adapt<CategoryPostsDTO, Category>();

        //This for avoiding cyclic references
        foreach (var cat in generatedCategoriesWithPosts)
        {
            foreach (var post in cat.Posts)
            {
                post.Category = null;
            }
        }

        //this code for checking just the last one because of AsNotracking in handler
        receivedCategory.Should()
            .BeEquivalentTo(generatedCategoriesWithPosts[^1]);
    }
}
