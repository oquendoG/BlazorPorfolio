using Shared.Models.Blog;
using System.ComponentModel.DataAnnotations;

namespace Server.Feats.Blog.Categories.DTOs;

public class CategoryDTO
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Name { get; set; }

    [Required]
    [MaxLength(128)]
    public string ThumbnailImage { get; set; }

    [Required]
    [MaxLength(1024)]
    public string Description { get; set; }
}