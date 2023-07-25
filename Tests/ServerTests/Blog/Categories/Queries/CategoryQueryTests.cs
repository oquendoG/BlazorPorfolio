using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;
using Shared.Models.Blog;

namespace Tests.ServerTests.Blog.Categories.Queries;

public class CategoryQueryTests
{
    private readonly Mock<AppDbContext> DbContextMock;
    private readonly GetCategoriesQueryHandler Handler;
    private readonly Fixture Fixture;
    public CategoryQueryTests()
    {
        DbContextMock = new Mock<AppDbContext>();
        Handler = new GetCategoriesQueryHandler(DbContextMock.Object);
        Fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_ShouldRetrieveCategoriesSuccessfully()
    {
        var request = Fixture.Create<GetCategoriesQueryRequest>();

        var expectedCategories = Fixture.Build<Category>()
            .Without(cat => cat.Posts)
            .CreateMany()
            .ToList();

        var mockSet = new Mock<DbSet<Category>>();
        mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(expectedCategories.AsQueryable().Provider);
        mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(expectedCategories.AsQueryable().Expression);
        mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(expectedCategories.AsQueryable().ElementType);
        mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(expectedCategories.AsQueryable().GetEnumerator());
        DbContextMock.Setup(c => c.Categories).Returns(mockSet.Object);
        List<CategoryDTO> result =
            await Handler.Handle(request, CancellationToken.None);

        Assert.Equal(expectedCategories.Count, result.Count);
        for (int i = 0; i < expectedCategories.Count; i++)
        {
            Assert.Equal(expectedCategories[i].Name, result[i].Name);
        }
    }
}