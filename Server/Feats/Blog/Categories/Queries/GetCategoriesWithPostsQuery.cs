using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.Queries;

public record GetCategoriesWithPostsQuery : IRequest<List<CategoryPostsDTO>>;

public class GetWithPostsQueryHandler : IRequestHandler<GetCategoriesWithPostsQuery, List<CategoryPostsDTO>>
{
    private readonly AppDbContext dbContext;

    public GetWithPostsQueryHandler(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<List<CategoryPostsDTO>> Handle(GetCategoriesWithPostsQuery request, CancellationToken cancellationToken)
    {
        List<Category> categoriesDb = await dbContext.Categories
            .Include(category => category.Posts)
            .ToListAsync(cancellationToken);
        List<CategoryPostsDTO> categories = categoriesDb.Adapt<List<Category>, List<CategoryPostsDTO>>();
        return categories;
    }
}
