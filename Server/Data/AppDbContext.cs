using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Blog;

namespace Server.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Categories seed
        Guid[] CategoryIds = new Guid[3];
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

            CategoryIds[i - 1] = categoriesToSeed[i - 1].Id;
        }

        modelBuilder.Entity<Category>().HasData(categoriesToSeed);
        #endregion        
        modelBuilder.Entity<Post>(
            entity =>
            {
                entity.HasOne(post => post.Category)
                .WithMany(category => category.Posts)
                .HasForeignKey("CategoryId");
            }
        );

        #region Post seed

        Post[] postsSeed = new Post[6];
        for (int i = 1; i < 7; i++)
        {
            string postTitle = string.Empty;
            Guid categoryId = Guid.Empty;

            switch (i)
            {
                case 1:
                    postTitle = "Primer Post";
                    categoryId = CategoryIds[0];
                    break;
                case 2:
                    postTitle = "Segundo Post";
                    categoryId = CategoryIds[1];
                    break;
                case 3:
                    postTitle = "Tercer Post";
                    categoryId = CategoryIds[2];
                    break;
                case 4:
                    postTitle = "Cuarto Post";
                    categoryId = CategoryIds[0];
                    break;
                case 5:
                    postTitle = "Quinto Post";
                    categoryId = CategoryIds[1];
                    break;
                case 6:
                    postTitle = "Sexto Post";
                    categoryId = CategoryIds[2];
                    break;
            }

            postsSeed[i - 1] = new Post()
            {
                Id = Guid.NewGuid(),
                Title = postTitle,
                Thumbnailimage = "uploads/placeholder.jpg",
                Excerpt = $"Este es un extracto del post {i}.",
                Content = string.Empty,
                PublishDate = DateTimeOffset.UtcNow.ToString("dd/MM/yyyy hh:mm"),
                Author = "Wilson OQuendo",
                CategoryId = categoryId
            };
        }
        modelBuilder.Entity<Post>().HasData(postsSeed);
        #endregion

        #region Administrator role seed
        const string administratorRole = "Administrator";
        IdentityRole administratorRoleToSeed = new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = administratorRole,
            NormalizedName = administratorRole.ToUpperInvariant(),

        };

        modelBuilder.Entity<IdentityRole>().HasData(administratorRoleToSeed);

        #endregion

        #region Administrator user seed
        const string administratorUserEmail = "oquendo999@gmail.com";
        IPasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();

        IdentityUser administratorUser = new()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = administratorUserEmail,
            NormalizedUserName = administratorUserEmail.ToUpperInvariant(),
            Email = administratorUserEmail,
            NormalizedEmail = administratorUserEmail.ToUpperInvariant(),
            PasswordHash = string.Empty

        };

        administratorUser.PasswordHash = passwordHasher.HashPassword(administratorUser, "Admin12345*");

        modelBuilder.Entity<IdentityUser>().HasData(administratorUser);

        #endregion

        #region Add administrator to role
        IdentityUserRole<string> identityUserRole = new()
        {
            RoleId =  administratorRoleToSeed.Id,
            UserId = administratorUser.Id
        };

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(identityUserRole);

        #endregion

        base.OnModelCreating(modelBuilder);
    }
}