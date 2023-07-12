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
        try
        {
            Category category = await dbContext.Categories.FindAsync(new object[] { request.Category.Id }, cancellationToken);

            if (category != null)
            {
                // Copia solo las propiedades modificadas del objeto request.Category a la entidad existente
                dbContext.Entry(category).CurrentValues.SetValues(request.Category);

                // Llama a SaveChanges() para guardar los cambios en la base de datos
                return await dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // La entidad no existe en la base de datos
                return 0;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al actualizar la categoría");
            return 0;
        }
    }
}
