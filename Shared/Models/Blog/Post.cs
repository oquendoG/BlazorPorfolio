﻿using Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Blog;

public class Post
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(128)]
    [NoPeriods(ErrorMessage = "El título no debe tener puntos (.)")]
    [SpacesInARow(ErrorMessage = "El título del post contiene mas de 3 espacios seguidos")]
    public string Title { get; set; }

    [Required]
    [MaxLength(512)]
    public string Excerpt { get; set; }

    [Required]
    [MaxLength(256)]
    public string Thumbnailimage { get; set; }

    [MaxLength(65536)]
    public string Content { get; set; }

    [Required]
    [MaxLength(32)]
    public string PublishDate { get; set; }

    [Required]
    public bool Published { get; set; }

    [Required]
    [MaxLength(128)]
    public string Author { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}
