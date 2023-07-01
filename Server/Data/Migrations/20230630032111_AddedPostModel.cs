using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPostModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("704123e9-79bd-41e5-afbb-776e43fc3224"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9258f62c-a4da-4218-a56a-26c336ca8e1b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b3ad1cc3-a280-4556-9011-3fbf2e201062"));

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Excerpt = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Thunbnailimage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 65536, nullable: false),
                    PublishDate = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Categories_CategoryId",
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
                    { new Guid("7cbf1695-5b80-4759-b170-207d892cb7a0"), "Description of category 1", "Category 1", "uploads/placeholder.jpg" },
                    { new Guid("ae303d15-baa8-40c8-ab75-1b50176a8cec"), "Description of category 2", "Category 2", "uploads/placeholder.jpg" },
                    { new Guid("d52ce8e2-5887-4f28-897d-b450b4510dd6"), "Description of category 3", "Category 3", "uploads/placeholder.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Post",
                columns: new[] { "Id", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "Thunbnailimage", "Title" },
                values: new object[,]
                {
                    { new Guid("557f5c87-7b91-40f0-afb4-c2b47db86411"), "Wilson OQuendo", new Guid("d52ce8e2-5887-4f28-897d-b450b4510dd6"), "", "Este es un extracto del post 6.", "30/06/2023 03:21", false, "uploads/placeholder.jpg", "Sexto Post" },
                    { new Guid("5c741e22-a811-467c-af9b-ca939e28fdf9"), "Wilson OQuendo", new Guid("7cbf1695-5b80-4759-b170-207d892cb7a0"), "", "Este es un extracto del post 4.", "30/06/2023 03:21", false, "uploads/placeholder.jpg", "Cuarto Post" },
                    { new Guid("8671a8d1-9632-481a-a139-c5a260ce2073"), "Wilson OQuendo", new Guid("7cbf1695-5b80-4759-b170-207d892cb7a0"), "", "Este es un extracto del post 1.", "30/06/2023 03:21", false, "uploads/placeholder.jpg", "Primer Post" },
                    { new Guid("976e66fb-f922-46c1-9888-2734f145f609"), "Wilson OQuendo", new Guid("ae303d15-baa8-40c8-ab75-1b50176a8cec"), "", "Este es un extracto del post 2.", "30/06/2023 03:21", false, "uploads/placeholder.jpg", "Segundo Post" },
                    { new Guid("ac361cd7-1f95-46e8-9216-4d879fb1c5c7"), "Wilson OQuendo", new Guid("d52ce8e2-5887-4f28-897d-b450b4510dd6"), "", "Este es un extracto del post 3.", "30/06/2023 03:21", false, "uploads/placeholder.jpg", "Tercer Post" },
                    { new Guid("b61ec2dd-8302-4356-be1c-70476bc3c040"), "Wilson OQuendo", new Guid("ae303d15-baa8-40c8-ab75-1b50176a8cec"), "", "Este es un extracto del post 5.", "30/06/2023 03:21", false, "uploads/placeholder.jpg", "Quinto Post" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_CategoryId",
                table: "Post",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7cbf1695-5b80-4759-b170-207d892cb7a0"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ae303d15-baa8-40c8-ab75-1b50176a8cec"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d52ce8e2-5887-4f28-897d-b450b4510dd6"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ThumbnailImage" },
                values: new object[,]
                {
                    { new Guid("704123e9-79bd-41e5-afbb-776e43fc3224"), "Description of category 1", "Category 1", "uploads/placeholder.jpg" },
                    { new Guid("9258f62c-a4da-4218-a56a-26c336ca8e1b"), "Description of category 2", "Category 2", "uploads/placeholder.jpg" },
                    { new Guid("b3ad1cc3-a280-4556-9011-3fbf2e201062"), "Description of category 3", "Category 3", "uploads/placeholder.jpg" }
                });
        }
    }
}
