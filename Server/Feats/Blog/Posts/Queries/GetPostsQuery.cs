using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Feats.Blog.Posts.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Posts.Queries;

public record GetPostsQueryRequest : IRequest<List<PostDTO>>;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQueryRequest, List<PostDTO>>
{
    private readonly AppDbContext context;

    public GetPostsQueryHandler(AppDbContext context)
    {
        this.context = context;
    }
    public async Task<List<PostDTO>> Handle(GetPostsQueryRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<Post> postsdb = await context.Posts
             .AsNoTracking()
            .ToListAsync(cancellationToken);

        return postsdb.Adapt<List<PostDTO>>();
    }
}