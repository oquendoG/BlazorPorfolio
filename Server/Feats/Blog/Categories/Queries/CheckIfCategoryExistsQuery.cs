using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Feats.Blog.Categories.Queries;

public record CheckIfCategoryExistsQueryRequest(Guid Id) : IRequest<bool>;

public class CheckIfCategoryExistsQueryHandler : IRequestHandler<CheckIfCategoryExistsQueryRequest, bool>
{
    private readonly AppDbContext dbContext;

    public CheckIfCategoryExistsQueryHandler(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> Handle(CheckIfCategoryExistsQueryRequest request, CancellationToken cancellationToken)
    {
        return await dbContext.Categories
            .AsNoTracking()
            .AnyAsync(cat => cat.Id == request.Id, cancellationToken);
    }
}