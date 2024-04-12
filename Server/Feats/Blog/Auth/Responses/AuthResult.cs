using Microsoft.AspNetCore.Identity;

namespace Server.Feats.Blog.Auth.Responses;

public class AuthResult
{
    public object User { get; set; }
    public string Token { get; set; } = string.Empty;

    public bool HasError { get; set; } = false;

    public bool HasException { get; set; } = false;

    public List<string> Errors { get; set; }

}
