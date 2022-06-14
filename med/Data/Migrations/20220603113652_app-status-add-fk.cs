using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class appstatusaddfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatusId",
                table: "Application",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Application_ApplicationStatusId",
                table: "Application",
                column: "ApplicationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_ApplicationStatus_ApplicationStatusId",
                table: "Application",
                column: "ApplicationStatusId",
                principalTable: "ApplicationStatus",
                principalColumn: "IdApplicationStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_ApplicationStatus_ApplicationStatusId",
                table: "Application");

            migrationBuilder.DropIndex(
                name: "IX_Application_ApplicationStatusId",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "ApplicationStatusId",
                table: "Application");
        }
    }
}
