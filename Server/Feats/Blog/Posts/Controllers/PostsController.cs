using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Feats.Blog.Posts.Commands;
using Server.Feats.Blog.Posts.Queries;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Posts.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IWebHostEnvironment hostEnvironment;

    public PostsController(IMediator mediator, IWebHostEnvironment hostEnvironment)
    {
        this.mediator = mediator;
        this.hostEnvironment = hostEnvironment;
    }

    #region CrudOperations
    [HttpGet]
    public async Task<ActionResult<List<Post>>> Get()
    {
        return await mediator.Send(new GetPostsQueryRequest());
    }

    [HttpGet("dto/{id}")]
    public async Task<IActionResult> GetDto(Guid id)
    {
        PostDTO post = await mediator.Send(new GetPostByIdQueryRequest(id));
        PostDTO postDTO = post.Adapt<PostDTO>();

        return Ok(postDTO);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        PostDTO post = await mediator.Send(new GetPostByIdQueryRequest(id));

        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostDTO post)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (post is null)
        {
            return BadRequest(ModelState);
        }

        if (post.Published)
        {
            post.PublishDate = DateTimeOffset.UtcNow.ToString("dd/MM/yyyy hh:mm");
        }

        int result = await mediator.Send(new CreatePostCommandRequest(post));
        if (result == 0)
        {
            return StatusCode(500, "Ha hábido una excepción por favor comuniquese con el administrador del sistema o mire los logs");
        }

        return Created("Creado correctamente", post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PostDTO UpdatedPost)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id == Guid.Empty || UpdatedPost is null)
        {
            return BadRequest(ModelState);
        }

        PostDTO OldPost = await mediator.Send(new GetPostByIdQueryRequest(id));
        if(OldPost is null)
        {
            return NotFound();
        }

        int result = await mediator.Send(new UpdatePostCommandRequest(OldPost, UpdatedPost));
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

        PostDTO post = await mediator.Send(new GetPostByIdQueryRequest(id));
        if (post is null)
        {
            return NotFound("El post no existe");
        }

        if (post.Thumbnailimage != "uploads/placeholder.jpg")
        {
            string filename = post.Thumbnailimage.Split('/').Last();
            System.IO.File.Delete($"{hostEnvironment.ContentRootPath}\\wwwroot\\uploads{filename}");
        }

        int result = await mediator.Send(new DeletePostCommandRequest(post));
        if (result == 0)
        {
            return StatusCode(500, "Ha hábido una excepción por favor comuniquese con el administrador del sistema o mire los logs");
        }

        return Ok(result);
    }
    #endregion
}
