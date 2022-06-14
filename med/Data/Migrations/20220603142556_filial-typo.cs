using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class filialtypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filial_Organization_OrganizationIdOrganization",
                table: "Filial");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Filial");

            migrationBuilder.RenameColumn(
                name: "OrganizationIdOrganization",
                table: "Filial",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Filial_OrganizationIdOrganization",
                table: "Filial",
                newName: "IX_Filial_OrganizationId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNimber",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Filial_Organization_OrganizationId",
                table: "Filial",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "IdOrganization");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filial_Organization_OrganizationId",
                table: "Filial");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PhoneNimber",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Filial",
                newName: "OrganizationIdOrganization");

            migrationBuilder.RenameIndex(
                name: "IX_Filial_OrganizationId",
                table: "Filial",
                newName: "IX_Filial_OrganizationIdOrganization");

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "Filial",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Filial_Organization_OrganizationIdOrganization",
                table: "Filial",
                column: "OrganizationIdOrganization",
                principalTable: "Organization",
                principalColumn: "IdOrganization");
        }
    }
}
