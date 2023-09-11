using Mapster;
using MediatR;
using Server.Data;
using Shared.Models.Blog;
using Microsoft.EntityFrameworkCore;
using Server.Feats.Blog.Categories.DTOs;

namespace Server.Feats.Blog.Categories.Queries;

public record GetCategoriesQueryRequest : IRequest<List<CategoryDTO>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQueryRequest, List<CategoryDTO>>
{
    private readonly AppDbContext context;

    public GetCategoriesQueryHandler(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<List<CategoryDTO>> Handle(GetCategoriesQueryRequest request, CancellationToken cancellationToken)
    {
        List<Category> categoriesDb = await context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return categoriesDb.Adapt<List<CategoryDTO>>();
    }
}