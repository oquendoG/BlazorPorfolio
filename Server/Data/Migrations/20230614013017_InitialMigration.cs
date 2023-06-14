using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                constraints: table => table.PrimaryKey("PK_Categories", x => x.Id));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
