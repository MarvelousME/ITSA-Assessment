using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetClinic.Api.Migrations
{
    public partial class AddPetOwnerAndPetDetal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetOwners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetOwners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimalType = table.Column<int>(type: "int", nullable: false),
                    Breed = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PetOwnerId = table.Column<int>(type: "int", nullable: false),
                    PetOwnerId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetDetails_PetOwners_PetOwnerId",
                        column: x => x.PetOwnerId,
                        principalTable: "PetOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetDetails_PetOwners_PetOwnerId1",
                        column: x => x.PetOwnerId1,
                        principalTable: "PetOwners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetDetails_PetOwnerId",
                table: "PetDetails",
                column: "PetOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PetDetails_PetOwnerId1",
                table: "PetDetails",
                column: "PetOwnerId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetDetails");

            migrationBuilder.DropTable(
                name: "PetOwners");
        }
    }
}
