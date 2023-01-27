using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examplium.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserInfos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserInfos");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserInfos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos",
                column: "UserId");
        }
    }
}
