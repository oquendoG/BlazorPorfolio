using Mapster;
using MediatR;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.Commands;

public record CreateCategoryCommandRequest(CategoryDTO Category) : IRequest<int>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommandRequest, int>
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<CreateCategoryCommandHandler> logger;

    public CreateCategoryCommandHandler(AppDbContext dbContext, ILogger<CreateCategoryCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    public async Task<int> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        Category category = request.Category.Adapt<Category>();
        try
        {
            dbContext.Categories.Add(category);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al crear la categoría");
            return 0;
        }
    }
}
