using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetClinic.Api.Migrations
{
    public partial class AddVieiwModelsWithIsActiveAndSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Breed",
                table: "PetDetails",
                newName: "BreedId");

            migrationBuilder.RenameColumn(
                name: "AnimalType",
                table: "PetDetails",
                newName: "AnimalTypeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PetOwners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PetOwners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AnimalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetDetails_AnimalTypeId",
                table: "PetDetails",
                column: "AnimalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PetDetails_BreedId",
                table: "PetDetails",
                column: "BreedId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetDetails_AnimalTypes_AnimalTypeId",
                table: "PetDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PetDetails_Breeds_BreedId",
                table: "PetDetails");

            migrationBuilder.DropTable(
                name: "AnimalTypes");

            migrationBuilder.DropTable(
                name: "Breeds");

            migrationBuilder.DropIndex(
                name: "IX_PetDetails_AnimalTypeId",
                table: "PetDetails");

            migrationBuilder.DropIndex(
                name: "IX_PetDetails_BreedId",
                table: "PetDetails");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PetOwners");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PetOwners");

            migrationBuilder.RenameColumn(
                name: "BreedId",
                table: "PetDetails",
                newName: "Breed");

            migrationBuilder.RenameColumn(
                name: "AnimalTypeId",
                table: "PetDetails",
                newName: "AnimalType");
        }
    }
}
