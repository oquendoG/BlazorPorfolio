using AutoFixture;
using FluentAssertions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Server.Data;
using Server.Feats.Blog.Categories.Commands;
using Server.Feats.Blog.Categories.Controllers;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;
using System.Net;
using Tests.ServerTests.Helpers;

namespace Tests.ServerTests.Blog.Categories.Controllers;
public class CategoryControllerDeleteTests
{
    public readonly AppDbContext contextFake;
    private readonly CategoriesController categoriesController;
    private readonly Fixture fixture;
    private readonly Mock<IWebHostEnvironment> hostEnvironmentMoq;
    private readonly Mock<IMediator> mediatorMoq;
    private readonly DbContextOptions<AppDbContext> options;

    public CategoryControllerDeleteTests()
    {
        options = HelperMethods.GenerateOptions();
        contextFake = new(options);
        fixture = new();
        hostEnvironmentMoq = new();
        mediatorMoq = new();
        categoriesController = new(mediatorMoq.Object, hostEnvironmentMoq.Object);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        CategoryDTO? category = new()
        {
            Id = Guid.Empty,
            Name = String.Empty,
            ThumbnailImage = String.Empty,
        };
        categoriesController.StatusCode(400);

        categoriesController.ModelState
            .AddModelError("Id", "Required");

        IActionResult? result = await categoriesController.Delete(category.Id);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenCategoryDoesntExit()
    {
        Guid id = Guid.NewGuid();

        IActionResult? result = await categoriesController.Delete(id);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task ShouldReturn500Error_WhenExceptionOcurred()
    {
        Category categoryToDb = AddCategoryWithPostsToDb();

        CategoryPostsDTO categoryPosts = categoryToDb.Adapt<CategoryPostsDTO>();
        GetCategoryByIdQueryRequest request = new(categoryToDb.Id, false);
        mediatorMoq.Setup(mediator => mediator.Send(request, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(categoryPosts);

        mediatorMoq.Setup(mediator => mediator.Send(It.IsAny<DeleteCategoryCommandRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(0);

        IActionResult result = await categoriesController.Delete(categoryToDb.Id)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    private Category AddCategoryWithPostsToDb()
    {
        Guid CategoryId = Guid.NewGuid();
        List<Post> posts = fixture.Build<Post>()
            .Without(p => p.Category)
            .With(p => p.CategoryId, CategoryId)
            .CreateMany(3)
            .ToList();

        contextFake.AddRange(posts);
        contextFake.SaveChanges();

        Category categoryToDb = fixture.Build<Category>()
             .With(cat => cat.Posts, posts)
             .With(cat => cat.Id, CategoryId)
             .With(cat => cat.ThumbnailImage, "uploads/placeholder.jpg")
             .Create();

        contextFake.Add(categoryToDb);
        contextFake.SaveChanges();
        return categoryToDb;
    }

    [Fact]
    public async Task ShouldDeleteCategorySuccessfully_WhenModelIsValid()
    {
        Category categoryToDb = AddCategoryWithPostsToDb();

        CategoryPostsDTO categoryPosts = categoryToDb.Adapt<CategoryPostsDTO>();
        GetCategoryByIdQueryRequest request = new(categoryToDb.Id, false);
        mediatorMoq.Setup(mediator => mediator.Send(request, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(categoryPosts);

        mediatorMoq.Setup(mediator => mediator.Send(It.IsAny<DeleteCategoryCommandRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(1);

        IActionResult result = await categoriesController.Delete(categoryToDb.Id)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().BeSameAs(categoryPosts);
    }
}
