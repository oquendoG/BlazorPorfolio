using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;

namespace Server.Feats.Blog.Categories.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator mediator;

    public CategoriesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> Get()
    {
        return await mediator.Send(new GetCategoriesQuery());
    }
}
