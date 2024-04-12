using FluentValidation;
using Server.Feats.Blog.Auth.Dtos;

namespace Server.Feats.Blog.Auth.Validators;

public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress()
            .NotEmpty()
                .WithMessage("El correo no puede estar vacío");

        RuleFor(u => u.Password)
            .MinimumLength(6)
                .WithMessage("La contraseña debe ser mínimo 6 letras")
            .NotEmpty()
                .WithMessage("La contraseña no debe estar vacía")
            .Matches("[0-9]")
                .WithMessage("Debe contener uno o más numeros")
            .WithName("Contraseña");
    }
}
