using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Feats.Blog.Auth.Dtos;
using Server.Feats.Blog.Auth.Responses;
using Server.Feats.Blog.Auth.Validators;
using Server.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Feats.Blog.Auth.Commands;

public record SignInCommandRequest(UserDto User) : IRequest<AuthResult>;

public class SignInCommandHandler(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
                        ILogger<SignInCommandHandler> logger, IOptionsSnapshot<JWT> options) : IRequestHandler<SignInCommandRequest, AuthResult>
{
    private readonly UserValidator userValidator = new();
    private readonly SignInManager<IdentityUser> signInManager = signInManager;
    private readonly UserManager<IdentityUser> userManager = userManager;
    private readonly ILogger<SignInCommandHandler> logger = logger;
    private readonly JWT jwt = options.Value;

    public async Task<AuthResult> Handle(SignInCommandRequest request, CancellationToken cancellationToken)
    {

        AuthResult authResult;
        IdentityUser identityUser = null;
        string webToken = string.Empty;
        List<string> errors = [];

        try
        {
            SignInResult signingResult = await signInManager
                .PasswordSignInAsync(request.User.Email, request.User.Password, false, false);

            if (signingResult.Succeeded)
            {
                identityUser = await userManager.FindByEmailAsync(request.User.Email);
                webToken = await GenerateJSONWebToken(identityUser);

                authResult = new()
                {
                    User = request.User,
                    Token = webToken,
                    HasError = false,
                    Errors = [],
                };

                return authResult;

            }

            errors.Add("No se pudo iniciar sesión verifique usuario y contraseña");

            authResult = new()
            {
                User = request.User,
                Token = webToken,
                HasError = true,
                Errors = errors
            };

            return authResult;
        }
        catch (Exception ex)
        {
            logger
                .LogError(ex, "Se ha producido un error al intentar iniciar sesión contacte al administrador de sistema");

            errors =
            [
                "Se ha producido un error al intentar iniciar sesión contacte al administrador de sistema",
                $"{ex.Message}"
            ];

            authResult = new()
            {
                User = request.User,
                Token = string.Empty,
                HasError = false,
                HasException = true,
                Errors = errors
            };

            return authResult;
        }


    }

    private async Task<string> GenerateJSONWebToken(IdentityUser identityUser)
    {
        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(jwt.Key));
        SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, identityUser.Id),

        };

        IList<string> roleNames = await userManager.GetRolesAsync(identityUser);
        claims.AddRange(roleNames.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

        JwtSecurityToken jwtSecurityToken = new(
            issuer: jwt.Issuer,
            audience: jwt.Issuer,
            claims: claims,
            null,
            expires: DateTime.UtcNow.AddDays(7)
            );

        string securityToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return securityToken;
    }
}
