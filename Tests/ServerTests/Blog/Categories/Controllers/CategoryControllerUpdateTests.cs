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
public class CategoryControllerUpdateTests
{
    private readonly CategoriesController categoriesController;
    public readonly AppDbContext contextFake;
    private readonly Fixture fixture;
    private readonly Mock<IWebHostEnvironment> hostEnvironmentMoq;
    private readonly Mock<IMediator> mediatorMoq;
    private readonly DbContextOptions<AppDbContext> options;

    public CategoryControllerUpdateTests()
    {
        fixture = new();
        hostEnvironmentMoq = new();
        mediatorMoq = new();
        options = HelperMethods.GenerateOptions();
        contextFake = new(options);
        categoriesController = new(mediatorMoq.Object, hostEnvironmentMoq.Object);
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        CategoryDTO? category = new()
        {
            Id = Guid.NewGuid(),
            Name = String.Empty,
            ThumbnailImage = String.Empty,
        };

        categoriesController.ModelState
            .AddModelError("Name", "Required");
        categoriesController.ModelState
            .AddModelError("ThumbnailImage", "Required");

        IActionResult? result = await categoriesController.Update(category.Id, category);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        CategoryDTO? category = null;
        Guid id = Guid.Empty;

        IActionResult? result = await categoriesController.Update(id, category);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenCategoryDoesNotExists()
    {
        CategoryDTO? category = fixture.Create<CategoryDTO>();

        mediatorMoq.Setup(med => med.Send(It.IsAny<CheckIfCategoryExistsQueryRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        IActionResult result = await categoriesController.Update(category.Id, category);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task ShouldReturn500Error_WhenModelIsInvalid()
    {
        CategoryDTO? category = new();
        categoriesController.StatusCode(500);

        mediatorMoq.Setup(med => med.Send(It.IsAny<UpdateCategoryCommandRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(0);


        IActionResult result = await categoriesController.Create(category)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task ShouldReturnOK_WhenModelIsValid()
    {
        CategoryDTO? category = fixture.Create<CategoryDTO>();
        Category categorytoDb = category.Adapt<Category>();

        contextFake.Add(categorytoDb);
        contextFake.SaveChanges();

        CategoryDTO categoryToUpdate = fixture.Build<CategoryDTO>()
            .With(cat => cat.Id, category.Id)
            .Create();

        UpdateCategoryCommandRequest request = new(categoryToUpdate);

        mediatorMoq.Setup(mediator => mediator.Send(request, default))
                  .ReturnsAsync(1);

        mediatorMoq.Setup(med => med.Send(It.IsAny<CheckIfCategoryExistsQueryRequest>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(true);

        categoriesController.StatusCode(200);

        IActionResult result = await categoriesController.Update(categorytoDb.Id, categoryToUpdate)!;
        ObjectResult? objectResult = (result as ObjectResult)!;

        objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().BeSameAs(categoryToUpdate);
    }
}
