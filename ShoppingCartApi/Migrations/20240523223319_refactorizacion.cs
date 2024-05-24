using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCartApi.Migrations
{
    /// <inheritdoc />
    public partial class refactorizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Products",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Products",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Products",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Products",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "Products",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "Id");
        }
    }
}
