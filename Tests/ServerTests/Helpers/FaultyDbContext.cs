using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models.Blog;

namespace Tests.ServerTests.Helpers;
public class FaultyDbContext : AppDbContext
{
    public FaultyDbContext(DbContextOptions<AppDbContext> options) : base(options) { 

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new Exception("Error");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Category category = new()
        {
            Id = Guid.NewGuid(),
            ThumbnailImage = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
        };
        modelBuilder.Entity<Category>().HasData(category);
    }
}
