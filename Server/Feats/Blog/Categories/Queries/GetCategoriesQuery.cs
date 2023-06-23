using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.Queries;

public record GetCategoriesQuery : IRequest<List<CategoryDTO>>;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDTO>>
{
    private readonly AppDbContext context;

    public GetCategoriesHandler(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<List<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<Category> categoriesDb = await context.Categories.ToListAsync(cancellationToken);
        return categoriesDb.Adapt<List<CategoryDTO>>();
    }
}
