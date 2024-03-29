﻿using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Server.Data;
using Server.Feats.Blog.Posts.Queries;
using Shared.Models.Blog;
using Tests.ServerTests.Helpers;

namespace Tests.ServerTests.Blog.Posts.Queries;

public class PostsQueryTests
{
    private readonly AppDbContext contextFake;
    private readonly DbContextOptions<AppDbContext> dbContextOptions;
    private readonly Fixture fixture;
    public PostsQueryTests()
    {
        dbContextOptions = HelperMethods.GenerateOptions();
        contextFake = new AppDbContext(dbContextOptions);
        fixture = new();
    }

    private async Task<List<Post>> CreateTestPosts()
    {
        List<Post> posts = fixture
            .Build<Post>()
            .Without(post => post.Category)
            .CreateMany(5)
            .ToList();

        contextFake.AddRange(posts);
        int result = await contextFake.SaveChangesAsync();

        return posts;
    }

    [Fact]
    public async Task ShouldReturnPosts_WhenFound()
    {
        _ = await CreateTestPostsWithCategories();

        GetPostsQueryHandler handler = new(contextFake);

        List<Post> posts = await handler
            .Handle(new GetPostsQueryRequest(), It.IsAny<CancellationToken>());

        posts.Should().NotBeEmpty();
        posts.Should().HaveCount(5);
    }

    private async Task<List<Post>> CreateTestPostsWithCategories()
    {
        Category category = fixture
            .Build<Category>()
            .Without(cat => cat.Posts)
            .Create();

        contextFake.Add(category);
        await contextFake.SaveChangesAsync();

        List<Post> posts = fixture
            .Build<Post>()
            .With(post=> post.Category, category)
            .CreateMany(5)
            .ToList();

        contextFake.AddRange(posts);
        await contextFake.SaveChangesAsync();

        return posts;
    }

    [Fact]
    public async Task ShouldReturnPost_WhenIdIsFound()
    {
        List<Post> postsFromDb = await CreateTestPosts();

        GetPostByIdQueryhandler handler = new(contextFake);

        PostDTO post = await handler
            .Handle(new GetPostByIdQueryRequest(postsFromDb[0].Id), It.IsAny<CancellationToken>());

        post.Should().NotBeNull();
        postsFromDb[0].Id.Should().Be(post.Id);
    }

    [Fact]
    public async Task ShouldReturnPostExistence_WhenIdIsFound()
    {
        List<Post> postsFromDb = await CreateTestPosts();

        CheckIfPostExistsQueryHandler handler = new(contextFake);

        bool isInExistence = await handler
            .Handle(new CheckIfPostExistsQueryRequest(postsFromDb[0].Id), It.IsAny<CancellationToken>());

        isInExistence.Should().BeTrue();
    }
}
