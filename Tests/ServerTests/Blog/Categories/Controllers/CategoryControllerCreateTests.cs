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
    public readonly AppDbContext contextFake;
    private readonly Fixture fixture;
    private readonly Mock<IWebHostEnvironment> hostEnvironmentMoq;
    private readonly Mock<IMediator> mediatorMoq;
    private readonly DbContextOptions<AppDbContext> options;
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
    public async Task ShouldReturnBadRequest_WhenModelIsInvalid()
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
    public async Task ShouldRetunBadRequest_WhenCategoryIsNull()
    {
        CategoryDTO? category = null;

        IActionResult? result = await categoriesController.Create(category);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task ShouldReturn500Error_WhenModelStateIsInvalid()
    {
        CategoryDTO? category = new();
        categoriesController.StatusCode(500);

        mediatorMoq.Setup(mediator => mediator.Send(It.IsAny<CreateCategoryCommandRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(0);


        IActionResult result = await categoriesController.Create(category)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task ShouldReturnCreated_WhenModelisValid()
    {
        CategoryDTO? category = fixture.Create<CategoryDTO>();
        categoriesController.StatusCode(201);

        mediatorMoq.Setup(mediator => mediator.Send(It.IsAny<CreateCategoryCommandRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(1);


        IActionResult result = await categoriesController.Create(category)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }
}
