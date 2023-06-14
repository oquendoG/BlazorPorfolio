using Microsoft.EntityFrameworkCore;
using Shared.Models.Blog;

namespace Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(){}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Category[] categoriesToSeed = new Category[3];
        for (int i = 1; i < 4; i++)
        {
            categoriesToSeed[i - 1] = new Category()
            {
                Id = Guid.NewGuid(),
                ThumbnailImage = "uploads/placeholder.jpg",
                Name = $"Category {i}",
                Description = $"Description of category {i}"
            };
        }

        modelBuilder.Entity<Category>().HasData(categoriesToSeed);
        base.OnModelCreating(modelBuilder);
    }
}