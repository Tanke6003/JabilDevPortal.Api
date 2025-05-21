using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JabilDevPortal.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Applications");
        }
    }
}
