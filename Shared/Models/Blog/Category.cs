﻿using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Blog;
public class Category
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string ThumbnailImage { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; }

    [Required]
    [MaxLength(1024)]
    public string Description { get; set; }

    public List<Post> Posts { get; set; }
}
