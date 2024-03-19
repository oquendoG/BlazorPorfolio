using AutoFixture;
using FluentAssertions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Data;
using Server.Feats.Blog.Categories.Commands;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;
using Tests.ServerTests.Helpers;

namespace Tests.ServerTests.Blog.Categories.Commands;

public class CategoryCommandTests
{
    public readonly AppDbContext contextFake;
    public readonly Mock<AppDbContext> contextMock;
    public readonly FaultyDbContext faultyDbContext;
    private readonly Fixture fixture;
    private readonly Mock<ILogger<CreateCategoryCommandHandler>> loggerCreate;
    private readonly Mock<ILogger<DeleteCategoryCommandHandler>> loggerDelete;
    private readonly Mock<ILogger<UpdateCategoryCommandHandler>> loggerUpdate;
    private readonly Mock<IMediator> mediatorMoq;
    private readonly DbContextOptions<AppDbContext> options;

    public CategoryCommandTests()
    {
        options = HelperMethods.GenerateOptions();
        contextFake = new(options);
        contextMock = new(options);
        faultyDbContext = new(options);
        fixture = new();
        loggerCreate = new();
        loggerUpdate = new();
        loggerDelete = new();
        mediatorMoq = new();
    }


    #region CreateTests
    [Fact]
    public async Task ShouldCreateCategorySuccessfully_WhenModelIsValidAddingCategory()
    {
        CategoryDTO? category = fixture.Create<CategoryDTO>();

        CreateCategoryCommandHandler handler = new(contextFake, loggerCreate.Object);
        int result = await handler.Handle(new CreateCategoryCommandRequest(category), default);

        result.Should().Be(1);
    }

    [Fact]
    public async Task ShouldReturnZero_WhenExceptionIsFoundAddingCategory()
    {
        CategoryDTO? category = null;

        CreateCategoryCommandHandler handler = new(contextFake, loggerCreate.Object);
        mediatorMoq.Setup(mediator => mediator.Send(handler, default))
                  .ReturnsAsync(0);

        int result = await handler.Handle(new CreateCategoryCommandRequest(category), default);

        result.Should().Be(0);
    }

    [Fact]
    public async Task ShouldProduceAndException_WhenErrorIsFoundAddingCategory()
    {
        CreateCategoryCommandHandler handler = new(contextMock.Object, loggerCreate.Object);

        int result = await handler
             .Handle(new CreateCategoryCommandRequest(new CategoryDTO()), default);

        result.Should().Be(0);
    }
    #endregion


    #region UpdateTests
    [Fact]
    public async Task ShouldUpdateCategorySuccessfully_WhenModelIsValid()
    {
        Category categoryToBd = await AddCategoryToDb();

        CategoryDTO? category = categoryToBd.Adapt<CategoryDTO>();
        category.Description = Guid.NewGuid().ToString();

        UpdateCategoryCommandHandler handler = new(contextFake, loggerUpdate.Object);
        int result = await handler.Handle(new UpdateCategoryCommandRequest(category), default);

        result.Should().Be(1);
    }

    private async Task<Category> AddCategoryToDb()
    {
        Category categoryToBd = fixture.Build<Category>()
                    .Without(cat => cat.Posts)
                    .Create();
        contextFake.Add(categoryToBd);
        await contextFake.SaveChangesAsync();
        return categoryToBd;
    }

    [Fact]
    public async Task ShouldReturnZero_WhenCategoryDoesntExit()
    {
        _ = await AddCategoryToDb();

        CategoryDTO? category = fixture.Create<CategoryDTO>();

        UpdateCategoryCommandHandler handler = new(contextFake, loggerUpdate.Object);
        int result = await handler.Handle(new UpdateCategoryCommandRequest(category), It.IsAny<CancellationToken>());

        result.Should().Be(0);
    }

    [Fact]
    public async Task ShouldReturnZero_WhenExceptionFound()
    {
        faultyDbContext.Database.EnsureCreated();

        Category? category = faultyDbContext.Categories.First();

        CategoryDTO categoryDTO = category.Adapt<CategoryDTO>();

        UpdateCategoryCommandHandler handler = new(faultyDbContext, loggerUpdate.Object);
        int result = await handler.Handle(new UpdateCategoryCommandRequest(categoryDTO), It.IsAny<CancellationToken>());

        result.Should().Be(0);
    }
    #endregion

    #region DeleteTests
    [Fact]
    public async Task ShouldDeleteCategorySuccessfully_WhenModelIsValid()
    {

        CategoryPostsDTO category = await CreateEntitytoDeleteDetached();
        DeleteCategoryCommandHandler handler = new(contextFake, loggerDelete.Object);
        int result = await handler.Handle(new DeleteCategoryCommandRequest(category), It.IsAny<CancellationToken>());

        result.Should().Be(1);
    }

    private async Task<CategoryPostsDTO> CreateEntitytoDeleteDetached()
    {
        CategoryPostsDTO? category = fixture.Build<CategoryPostsDTO>()
            .Without(cat => cat.Posts)
            .Create();
        Category categoryToDelete = category.Adapt<Category>();

        contextFake.Add(categoryToDelete);
        int isSaved = await contextFake.SaveChangesAsync();
        contextFake.Entry(categoryToDelete).State = EntityState.Detached;

        return category;
    }

    [Fact]
    public async Task ShouldReturnZero_IfExceptionIsFound()
    {
        faultyDbContext.Database.EnsureCreated();
        Category categoryDb = faultyDbContext.Categories.First();

        CategoryPostsDTO category = categoryDb.Adapt<CategoryPostsDTO>();
        DeleteCategoryCommandHandler handler = new(faultyDbContext, loggerDelete.Object);
        int result = await handler.Handle(new DeleteCategoryCommandRequest(category), It.IsAny<CancellationToken>());

        result.Should().Be(0);
    }

    #endregion

}
