using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetClinic.Api.Migrations
{
    public partial class SortOutMappinig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetDetails_PetOwners_PetOwnerId",
                table: "PetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PetDetails_PetOwners_PetOwnerId1",
                table: "PetDetails");

            migrationBuilder.DropIndex(
                name: "IX_PetDetails_PetOwnerId1",
                table: "PetDetails");

            migrationBuilder.DropColumn(
                name: "PetOwnerId1",
                table: "PetDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PetOwnerId",
                table: "PetDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Owner",
                table: "PetDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_PetDetails_PetOwners_PetOwnerId",
                table: "PetDetails",
                column: "PetOwnerId",
                principalTable: "PetOwners",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetDetails_PetOwners_PetOwnerId",
                table: "PetDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PetOwnerId",
                table: "PetDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Owner",
                table: "PetDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetOwnerId1",
                table: "PetDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetDetails_PetOwnerId1",
                table: "PetDetails",
                column: "PetOwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PetDetails_PetOwners_PetOwnerId",
                table: "PetDetails",
                column: "PetOwnerId",
                principalTable: "PetOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PetDetails_PetOwners_PetOwnerId1",
                table: "PetDetails",
                column: "PetOwnerId1",
                principalTable: "PetOwners",
                principalColumn: "Id");
        }
    }
}
