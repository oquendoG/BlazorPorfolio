using Shared.Validators;
using System.ComponentModel.DataAnnotations;

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
    [NoPeriods(ErrorMessage = "El nombre no debe tener puntos (.)")]
    [SpacesInARow(ErrorMessage = "El nombre de la categoría contiene mas de 3 espacios seguidos")]
    public string Name { get; set; }

    [Required]
    [MaxLength(1024)]
    public string Description { get; set; }

    public List<Post> Posts { get; set; }
}
