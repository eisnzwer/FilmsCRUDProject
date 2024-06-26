using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmsCRUDProject.Migrations
{
    /// <inheritdoc />
    public partial class NullableUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Users_UserId",
                table: "Films");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Films",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Users_UserId",
                table: "Films",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Users_UserId",
                table: "Films");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Films",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Users_UserId",
                table: "Films",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
