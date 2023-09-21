using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Posts.Queries;

public record GetPostByIdQueryRequest(Guid PostId) : IRequest<PostDTO>;

public class GetPostByIdQueryhandler : IRequestHandler<GetPostByIdQueryRequest, PostDTO>
{
    private readonly AppDbContext dbContext;

    public GetPostByIdQueryhandler(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<PostDTO> Handle(GetPostByIdQueryRequest request, CancellationToken cancellationToken)
    {
        Post postdb = await dbContext.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(post => post.Id == request.PostId, cancellationToken);

        return postdb.Adapt<PostDTO>();
    }
}
