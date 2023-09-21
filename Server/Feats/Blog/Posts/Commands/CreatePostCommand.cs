using Mapster;
using MediatR;
using Server.Data;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Posts.Commands;

public record CreatePostCommandRequest(PostDTO Category) : IRequest<int>;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommandRequest, int>
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<CreatePostCommandHandler> logger;

    public CreatePostCommandHandler(AppDbContext dbContext, ILogger<CreatePostCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    public async Task<int> Handle(CreatePostCommandRequest request, CancellationToken cancellationToken)
    {
        Post post = request.Category.Adapt<Post>();
        try
        {
            dbContext.Posts.Add(post);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al crear la categoría");
            return 0;
        }
    }
}