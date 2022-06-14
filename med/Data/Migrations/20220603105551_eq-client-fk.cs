using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class eqclientfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ClientId",
                table: "Equipment",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Client_ClientId",
                table: "Equipment",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "IdClient");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Client_ClientId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_ClientId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Equipment");
        }
    }
}
