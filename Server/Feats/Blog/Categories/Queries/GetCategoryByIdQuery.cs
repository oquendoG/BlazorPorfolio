using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.Queries;

public record GetCategoryByIdQueryRequest(Guid CategoryId, bool WithPosts) : IRequest<CategoryPostsDTO>;

public class GetCategoryByIdQueryhandler : IRequestHandler<GetCategoryByIdQueryRequest, CategoryPostsDTO>
{
    private readonly AppDbContext dbContext;

    public GetCategoryByIdQueryhandler(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CategoryPostsDTO> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
    {
        Category category = null;
        if (request.WithPosts)
        {
            category = await dbContext.Categories
                .AsNoTracking()
                .Include(category => category.Posts)
                .FirstOrDefaultAsync(category => category.Id == request.CategoryId, cancellationToken);
        }
        else
        {
            category = await dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(category => category.Id == request.CategoryId, cancellationToken);
        }

        if (category is null)
        {
            return new CategoryPostsDTO();
        }

        return category.Adapt<CategoryPostsDTO>();
    }
}
