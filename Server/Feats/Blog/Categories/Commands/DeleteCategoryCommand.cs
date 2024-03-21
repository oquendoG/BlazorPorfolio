using Mapster;
using MediatR;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.Commands;

public record DeleteCategoryCommandRequest(CategoryPostsDTO Category) : IRequest<int>;
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommandRequest, int>
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<DeleteCategoryCommandHandler> logger;

    public DeleteCategoryCommandHandler(AppDbContext dbContext, ILogger<DeleteCategoryCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    public async Task<int> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        Category category = request.Category.Adapt<Category>();
        try
        {
            dbContext.Categories.Remove(category);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al eliminar la categoría");
            return 0;
        }
    }
}
