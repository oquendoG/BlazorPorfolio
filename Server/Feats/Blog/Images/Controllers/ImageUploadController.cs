using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Feats.Blog.Images.Commands;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Images.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator")]
public class ImageUploadController : ControllerBase
{
    private readonly IMediator mediator;

    public ImageUploadController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] UploadedImage uploadedImage)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(uploadedImage);
        }

        string relativeImagePath =
            await mediator.Send(new CreateImageCommandRequest(uploadedImage));
        if (relativeImagePath?.Length == 0)
        {
            return StatusCode(500, "Se ha producido un error por favor llame al administrador para solucionar el problema");
        }

        return Created("Imagen creada", relativeImagePath);
    }
}
