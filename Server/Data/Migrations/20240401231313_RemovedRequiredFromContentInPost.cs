using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRequiredFromContentInPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("4f38237f-5a49-4f4f-803c-b51bb503b7c8"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("7a58f248-f419-4643-b0ee-949f422c61a3"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("95ce2f22-313e-435f-a33f-6c45fe883fca"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("9e921a05-0786-49e7-a15e-3bc8ef1f2196"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b24e9288-4398-45f9-a3fa-ff574de6e107"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("f30feed2-8421-4197-9542-917c88e62ae5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("026749b3-f2a7-41a9-873f-82e9303e9500"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7721b335-da37-428a-9e5a-74ed430d38a3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c1e6e821-4a35-42d5-b0f2-2544d7651b03"));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "TEXT",
                maxLength: 65536,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 65536);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ThumbnailImage" },
                values: new object[,]
                {
                    { new Guid("662e4d35-71cc-4b78-8e10-d819bf6c3b7d"), "Description of category 3", "Category 3", "uploads/placeholder.jpg" },
                    { new Guid("74c25577-7dd8-47b1-a021-ef808ad0cd24"), "Description of category 1", "Category 1", "uploads/placeholder.jpg" },
                    { new Guid("cca6df3e-c22a-4586-8211-124e6df4970c"), "Description of category 2", "Category 2", "uploads/placeholder.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "Thumbnailimage", "Title" },
                values: new object[,]
                {
                    { new Guid("21481a83-7763-40b2-9613-a78d6dc6f459"), "Wilson OQuendo", new Guid("662e4d35-71cc-4b78-8e10-d819bf6c3b7d"), "", "Este es un extracto del post 6.", "01/04/2024 11:13", false, "uploads/placeholder.jpg", "Sexto Post" },
                    { new Guid("38c5baa2-c59b-41c0-96b9-c18464a9e345"), "Wilson OQuendo", new Guid("cca6df3e-c22a-4586-8211-124e6df4970c"), "", "Este es un extracto del post 2.", "01/04/2024 11:13", false, "uploads/placeholder.jpg", "Segundo Post" },
                    { new Guid("3f500016-6c13-4eeb-8040-4c632a1e5851"), "Wilson OQuendo", new Guid("74c25577-7dd8-47b1-a021-ef808ad0cd24"), "", "Este es un extracto del post 4.", "01/04/2024 11:13", false, "uploads/placeholder.jpg", "Cuarto Post" },
                    { new Guid("4f4281ee-5106-48cd-a4c3-5bd0f510cf6f"), "Wilson OQuendo", new Guid("74c25577-7dd8-47b1-a021-ef808ad0cd24"), "", "Este es un extracto del post 1.", "01/04/2024 11:13", false, "uploads/placeholder.jpg", "Primer Post" },
                    { new Guid("8d020f5c-0ee7-4616-abfc-da99eafd74d3"), "Wilson OQuendo", new Guid("662e4d35-71cc-4b78-8e10-d819bf6c3b7d"), "", "Este es un extracto del post 3.", "01/04/2024 11:13", false, "uploads/placeholder.jpg", "Tercer Post" },
                    { new Guid("c1a61afd-4462-44b5-9cde-804d8ae451b3"), "Wilson OQuendo", new Guid("cca6df3e-c22a-4586-8211-124e6df4970c"), "", "Este es un extracto del post 5.", "01/04/2024 11:13", false, "uploads/placeholder.jpg", "Quinto Post" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("21481a83-7763-40b2-9613-a78d6dc6f459"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("38c5baa2-c59b-41c0-96b9-c18464a9e345"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("3f500016-6c13-4eeb-8040-4c632a1e5851"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("4f4281ee-5106-48cd-a4c3-5bd0f510cf6f"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("8d020f5c-0ee7-4616-abfc-da99eafd74d3"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c1a61afd-4462-44b5-9cde-804d8ae451b3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("662e4d35-71cc-4b78-8e10-d819bf6c3b7d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("74c25577-7dd8-47b1-a021-ef808ad0cd24"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cca6df3e-c22a-4586-8211-124e6df4970c"));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "TEXT",
                maxLength: 65536,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 65536,
                oldNullable: true);

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
        }
    }
}
