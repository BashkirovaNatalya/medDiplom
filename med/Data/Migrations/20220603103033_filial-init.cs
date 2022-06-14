using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class filialinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filial",
                columns: table => new
                {
                    IdFilial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    OrganizationIdOrganization = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filial", x => x.IdFilial);
                    table.ForeignKey(
                        name: "FK_Filial_Organization_OrganizationIdOrganization",
                        column: x => x.OrganizationIdOrganization,
                        principalTable: "Organization",
                        principalColumn: "IdOrganization");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filial_OrganizationIdOrganization",
                table: "Filial",
                column: "OrganizationIdOrganization");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filial");
        }
    }
}
