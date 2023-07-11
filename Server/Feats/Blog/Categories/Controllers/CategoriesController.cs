using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Feats.Blog.Categories.Commands;
using Server.Feats.Blog.Categories.DTOs;
using Server.Feats.Blog.Categories.Queries;

namespace Server.Feats.Blog.Categories.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IWebHostEnvironment hostEnvironment;

    public CategoriesController(IMediator mediator, IWebHostEnvironment hostEnvironment)
    {
        this.mediator = mediator;
        this.hostEnvironment = hostEnvironment;
    }

    #region CrudOperations
    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> Get()
    {
        return await mediator.Send(new GetCategoriesQuery());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        CategoryPostsDTO category =
            await mediator.Send(new GetByIdQuery(id, false));

        return Ok(category);
    }

    //website.com/api/categories/withposts
    [HttpGet("withposts")]
    public async Task<IActionResult> GetWithPosts()
    {
        return Ok(await mediator.Send(new GetCategoriesWithPostsQuery()));
    }

    //website.com/api/categories/id
    [HttpGet("withposts/{id}")]
    public async Task<IActionResult> GetWithPosts(Guid id)
    {
        CategoryPostsDTO category =
            await mediator.Send(new GetByIdQuery(id, true));

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryDTO category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (category is null)
        {
            return BadRequest(ModelState);
        }

        int result = await mediator.Send(new CreateCategoryCommand(category));
        if (result == 0)
        {
            return StatusCode(500, "Ha hábido una excepción por favor comuniquese con el administrador del sistema o mire los logs");
        }

        return Created("Creado correctamente", category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDTO category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id == Guid.Empty || category is null)
        {
            return BadRequest(ModelState);
        }

        bool checking = await mediator.Send(new CheckIfCategoryExistsQuery(id));
        if (!checking)
        {
            return NotFound("La categoría no existe");
        }

        int result = await mediator.Send(new UpdateCategoryCommand(category));
        if (result == 0)
        {
            return StatusCode(500, "Ha hábido una excepción por favor comuniquese con el administrador del sistema o mire los logs");
        }

        return Ok("Actualizado correctamente");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!ModelState.IsValid || id == Guid.Empty)
        {
            return BadRequest(ModelState);
        }

        CategoryPostsDTO categoryDb = await mediator.Send(new GetByIdQuery(id, false));
        if (categoryDb is null)
        {
            return NotFound("La categoría no existe");
        }

        if (categoryDb.ThumbnailImage != "uploads/placeholder.jpg")
        {
            string filename = categoryDb.ThumbnailImage.Split('/').Last();
            System.IO.File.Delete($"{hostEnvironment.ContentRootPath}\\wwwroot\\uploads{filename}");
        }

        int result = await mediator.Send(new DeleteCategoryCommand(categoryDb));
        if (result == 0)
        {
            return StatusCode(500, "Ha hábido una excepción por favor comuniquese con el administrador del sistema o mire los logs");
        }

        return NoContent();
    }
    #endregion
}
