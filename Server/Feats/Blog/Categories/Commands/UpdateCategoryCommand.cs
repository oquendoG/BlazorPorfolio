using Mapster;
using MediatR;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.Commands;

public record UpdateCategoryCommandRequest(CategoryDTO Category) : IRequest<int>;
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommandRequest, int>
{
    private readonly AppDbContext dbContext;
    private readonly ILogger logger;

    public UpdateCategoryCommandHandler(AppDbContext dbContext, ILogger<UpdateCategoryCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    public async Task<int> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        Category category = request.Category.Adapt<Category>();
        try
        {
            dbContext.Categories.Update(category);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al actualizar la categoría");
            return 0;
        }
    }
}
