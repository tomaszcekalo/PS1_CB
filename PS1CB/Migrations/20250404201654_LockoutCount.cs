using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PS1CB.Migrations
{
    /// <inheritdoc />
    public partial class LockoutCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LockoutCount",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockoutCount",
                table: "AspNetUsers");
        }
    }
}
