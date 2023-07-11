using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Feats.Blog.Categories.Queries;

public record CheckIfCategoryExistsQuery(Guid Id) : IRequest<bool>;

public class CheckIfCategoryExistsHandler : IRequestHandler<CheckIfCategoryExistsQuery, bool>
{
    private readonly AppDbContext dbContext;

    public CheckIfCategoryExistsHandler(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> Handle(CheckIfCategoryExistsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Categories.AnyAsync(cat => cat.Id == request.Id, cancellationToken);
    }
}