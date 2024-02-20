using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Server.Data;
using Server.Feats.Blog.Categories.Commands;
using Server.Feats.Blog.Categories.Controllers;
using Server.Feats.Blog.Categories.DTOs;
using System.Net;
using Tests.ServerTests.Helpers;

namespace Tests.ServerTests.Blog.Categories.Controllers;
public class CategoryControllerCreateTests
{
    private readonly CategoriesController categoriesController;
    private readonly Mock<IMediator> mediatorMoq;
    private readonly Mock<IWebHostEnvironment> hostEnvironmentMoq;
    private readonly DbContextOptions<AppDbContext> options;
    public readonly AppDbContext contextFake;
    private readonly Fixture fixture;

    public CategoryControllerCreateTests()
    {
        fixture = new();
        mediatorMoq = new();
        hostEnvironmentMoq = new();
        options = HelperMethods.GenerateOptions();
        contextFake = new(options);
        categoriesController = new(mediatorMoq.Object, hostEnvironmentMoq.Object);
    }

    [Fact]
    public async Task Controller_ModelState_ShouldReturnBadRequest()
    {
        CategoryDTO? category = new()
        {
            Name = String.Empty,
            ThumbnailImage = String.Empty,
        };

        categoriesController.ModelState
            .AddModelError("Name", "Required");
        categoriesController.ModelState
            .AddModelError("ThumbnailImage", "Required");

        IActionResult? result = await categoriesController.Create(category);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Controller_ShouldNotBeNull()
    {
        CategoryDTO? category = null;

        IActionResult? result = await categoriesController.Create(category);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Controller_ModelState_ShouldReturn500Error()
    {
        CategoryDTO? category = new();
        categoriesController.StatusCode(500);

        mediatorMoq.Setup(mediator => mediator.Send(It.IsAny<CreateCategoryCommandRequest>(), default))
                  .ReturnsAsync(0);


        IActionResult result = await categoriesController.Create(category)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Controller_ShouldReturnCreated()
    {
        CategoryDTO? category = fixture.Create<CategoryDTO>();
        categoriesController.StatusCode(201);

        mediatorMoq.Setup(mediator => mediator.Send(It.IsAny<CreateCategoryCommandRequest>(), default))
                  .ReturnsAsync(1);


        IActionResult result = await categoriesController.Create(category)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }
}
