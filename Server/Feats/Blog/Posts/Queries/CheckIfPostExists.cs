using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Feats.Blog.Posts.Queries;

public record CheckIfPostExistsQueryRequest(Guid Id) : IRequest<bool>;

public class CheckIfPostExistsQueryHandler : IRequestHandler<CheckIfPostExistsQueryRequest, bool>
{
    private readonly AppDbContext dbContext;

    public CheckIfPostExistsQueryHandler(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> Handle(CheckIfPostExistsQueryRequest request, CancellationToken cancellationToken)
    {
        return await dbContext.Categories
            .AsNoTracking()
            .AnyAsync(cat => cat.Id == request.Id, cancellationToken);
    }
}