﻿using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Feats.Blog.Categories.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.Queries;

public record GetCategoriesWithPostsQueryRequest : IRequest<List<CategoryPostsDTO>>;

public class GetCategoriesWithPostsQueryHandler : IRequestHandler<GetCategoriesWithPostsQueryRequest, List<CategoryPostsDTO>>
{
    private readonly AppDbContext dbContext;

    public GetCategoriesWithPostsQueryHandler(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<List<CategoryPostsDTO>> Handle(GetCategoriesWithPostsQueryRequest request, CancellationToken cancellationToken)
    {
        List<Category> categoriesDb = await dbContext.Categories
            .AsNoTracking()
            .Include(category => category.Posts)
            .ToListAsync(cancellationToken);
        List<CategoryPostsDTO> categories = categoriesDb.Adapt<List<Category>, List<CategoryPostsDTO>>();
        return categories;
    }
}
