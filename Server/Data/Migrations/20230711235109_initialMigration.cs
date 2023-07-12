using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ThumbnailImage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Excerpt = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Thumbnailimage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 65536, nullable: false),
                    PublishDate = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ThumbnailImage" },
                values: new object[,]
                {
                    { new Guid("026749b3-f2a7-41a9-873f-82e9303e9500"), "Description of category 3", "Category 3", "uploads/placeholder.jpg" },
                    { new Guid("7721b335-da37-428a-9e5a-74ed430d38a3"), "Description of category 2", "Category 2", "uploads/placeholder.jpg" },
                    { new Guid("c1e6e821-4a35-42d5-b0f2-2544d7651b03"), "Description of category 1", "Category 1", "uploads/placeholder.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "Thumbnailimage", "Title" },
                values: new object[,]
                {
                    { new Guid("4f38237f-5a49-4f4f-803c-b51bb503b7c8"), "Wilson OQuendo", new Guid("7721b335-da37-428a-9e5a-74ed430d38a3"), "", "Este es un extracto del post 2.", "11/07/2023 11:51", false, "uploads/placeholder.jpg", "Segundo Post" },
                    { new Guid("7a58f248-f419-4643-b0ee-949f422c61a3"), "Wilson OQuendo", new Guid("026749b3-f2a7-41a9-873f-82e9303e9500"), "", "Este es un extracto del post 6.", "11/07/2023 11:51", false, "uploads/placeholder.jpg", "Sexto Post" },
                    { new Guid("95ce2f22-313e-435f-a33f-6c45fe883fca"), "Wilson OQuendo", new Guid("026749b3-f2a7-41a9-873f-82e9303e9500"), "", "Este es un extracto del post 3.", "11/07/2023 11:51", false, "uploads/placeholder.jpg", "Tercer Post" },
                    { new Guid("9e921a05-0786-49e7-a15e-3bc8ef1f2196"), "Wilson OQuendo", new Guid("c1e6e821-4a35-42d5-b0f2-2544d7651b03"), "", "Este es un extracto del post 1.", "11/07/2023 11:51", false, "uploads/placeholder.jpg", "Primer Post" },
                    { new Guid("b24e9288-4398-45f9-a3fa-ff574de6e107"), "Wilson OQuendo", new Guid("c1e6e821-4a35-42d5-b0f2-2544d7651b03"), "", "Este es un extracto del post 4.", "11/07/2023 11:51", false, "uploads/placeholder.jpg", "Cuarto Post" },
                    { new Guid("f30feed2-8421-4197-9542-917c88e62ae5"), "Wilson OQuendo", new Guid("7721b335-da37-428a-9e5a-74ed430d38a3"), "", "Este es un extracto del post 5.", "11/07/2023 11:51", false, "uploads/placeholder.jpg", "Quinto Post" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
