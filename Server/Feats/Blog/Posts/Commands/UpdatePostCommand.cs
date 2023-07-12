using Mapster;
using MediatR;
using Server.Data;
using Server.Feats.Blog.Posts.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Posts.Commands;

public record UpdatePostCommandRequest(PostDTO OldPost, PostDTO UpdatedPost) : IRequest<int>;
public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommandRequest, int>
{
    private readonly AppDbContext dbContext;
    private readonly ILogger logger;

    public UpdatePostCommandHandler(AppDbContext dbContext, ILogger<UpdatePostCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    public async Task<int> Handle(UpdatePostCommandRequest request, CancellationToken cancellationToken)
    {
        Post PostToUpdate = request.UpdatedPost.Adapt<Post>();
        if (PostToUpdate.Published)
        {
            if (!request.OldPost.Published)
            {
                PostToUpdate.PublishDate = DateTimeOffset.UtcNow.ToString("dd/MM/yyyy hh:mm");
            }
            else
            {
                PostToUpdate.PublishDate = request.OldPost.PublishDate;
            }
        }
        else
        {
            PostToUpdate.PublishDate = string.Empty;
        }

        try
        {
            Post OldPost = await dbContext.Posts.FindAsync(new object[] { request.OldPost.Id }, cancellationToken);
            dbContext.Entry(OldPost).CurrentValues.SetValues(PostToUpdate);
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al actualizar la categoría");
            return 0;
        }
    }
}
