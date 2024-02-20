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
    private readonly Mock<IMediator> mediatorMoq;
    private readonly Mock<IWebHostEnvironment> hostEnvironmentMoq;
    private readonly DbContextOptions<AppDbContext> options;
    public readonly AppDbContext contextFake;
    private readonly Fixture fixture;

    public CategoryControllerUpdateTests()
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
    public async Task Controller_ShouldNotBeNull()
    {
        CategoryDTO? category = null;
        Guid id = Guid.Empty;

        IActionResult? result = await categoriesController.Update(id, category);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Controller_ShouldCheckCategoryNotExists()
    {
        CategoryDTO? category = fixture.Create<CategoryDTO>();

        mediatorMoq.Setup(med => med.Send(It.IsAny<CheckIfCategoryExistsQueryRequest>(), default))
            .ReturnsAsync(false);

        var result = await categoriesController.Update(category.Id, category);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Controller_ModelState_ShouldReturn500Error()
    {
        CategoryDTO? category = new();
        categoriesController.StatusCode(500);

        mediatorMoq.Setup(med => med.Send(It.IsAny<UpdateCategoryCommandRequest>(), default))
                  .ReturnsAsync(0);


        IActionResult result = await categoriesController.Create(category)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Controller_ShouldReturnOK()
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

        mediatorMoq.Setup(med => med.Send(It.IsAny<CheckIfCategoryExistsQueryRequest>(), default))
           .ReturnsAsync(true);

        categoriesController.StatusCode(200);
        IActionResult result = await categoriesController.Update(categorytoDb.Id, categoryToUpdate)!;
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().BeSameAs(categoryToUpdate);
    }
}
