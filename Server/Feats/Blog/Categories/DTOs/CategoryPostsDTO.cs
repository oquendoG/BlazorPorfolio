using Server.Feats.Blog.Posts.DTOs;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Categories.DTOs;

public class CategoryPostsDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ThumbnailImage { get; set; }
    public string Description { get; set; }
    public List<PostDTO> Posts { get; set; }
}
