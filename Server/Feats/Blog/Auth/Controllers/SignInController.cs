using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Feats.Blog.Auth.Commands;
using Server.Feats.Blog.Auth.Dtos;
using Server.Feats.Blog.Auth.Validators;
using Shared.Responses;

namespace Server.Feats.Blog.Auth.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SignInController(ISender sender) : ControllerBase
{
    private readonly ISender sender = sender;

    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] UserDto user)
    {
        UserValidator userValidator = new();
        ValidationResult validationResult = userValidator.Validate(user);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        AuthResult result = await sender.Send(new SignInCommandRequest(user));

        if (result.HasException)
        {
            return StatusCode(500, result);
        }

        if (result.HasError)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }
}
