using Mapster;
using MediatR;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.Commands;

public record DeleteCategoryCommand(CategoryPostsDTO Category) : IRequest<int>;
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, int>
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<DeleteCategoryCommandHandler> logger;

    public DeleteCategoryCommandHandler(AppDbContext dbContext, ILogger<DeleteCategoryCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    public async Task<int> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = request.Category.Adapt<Category>();
        try
        {
            dbContext.Categories.Remove(category);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al crear la categoría");
            return 0;
        }
    }
}
