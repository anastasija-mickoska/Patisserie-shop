using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patisserie.Migrations
{
    /// <inheritdoc />
    public partial class UserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_AspNetUsers_UserName",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Review");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserProducts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProducts_UserName",
                table: "UserProducts",
                newName: "IX_UserProducts_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Review",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_AspNetUsers_UserId",
                table: "UserProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_AspNetUsers_UserId",
                table: "UserProducts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserProducts",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_UserProducts_UserId",
                table: "UserProducts",
                newName: "IX_UserProducts_UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Review",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Review",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_AspNetUsers_UserName",
                table: "UserProducts",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
