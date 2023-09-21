using Mapster;
using MediatR;
using Server.Data;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Posts.Commands;

public record DeletePostCommandRequest(PostDTO Post) : IRequest<int>;
public class DeletePostCommandHandler : IRequestHandler<DeletePostCommandRequest, int>
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<DeletePostCommandHandler> logger;

    public DeletePostCommandHandler(AppDbContext dbContext, ILogger<DeletePostCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    public async Task<int> Handle(DeletePostCommandRequest request, CancellationToken cancellationToken)
    {
        Post post = request.Post.Adapt<Post>();
        try
        {
            dbContext.Posts.Remove(post);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al actualizar el post");
            return 0;
        }
    }
}