using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Posts.Queries;

public record GetPostsQueryRequest : IRequest<List<Post>>;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQueryRequest, List<Post>>
{
    private readonly AppDbContext context;

    public GetPostsQueryHandler(AppDbContext context)
    {
        this.context = context;
    }
    public async Task<List<Post>> Handle(GetPostsQueryRequest request, CancellationToken cancellationToken)
    {
        List<Post> postsdb = await context.Posts
             .Include(post => post.Category)
             .AsNoTracking()
            .ToListAsync(cancellationToken);

        return postsdb;
    }
}