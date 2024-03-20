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
        bool isInExistence = await dbContext.Posts
            .AsNoTracking()
            .AnyAsync(post => post.Id == request.Id, cancellationToken);

        return isInExistence;
    }
}