using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class clientaddposfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientPositionId",
                table: "Client",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Client_ClientPositionId",
                table: "Client",
                column: "ClientPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_ClientPosition_ClientPositionId",
                table: "Client",
                column: "ClientPositionId",
                principalTable: "ClientPosition",
                principalColumn: "IdClientPosition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_ClientPosition_ClientPositionId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_ClientPositionId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ClientPositionId",
                table: "Client");
        }
    }
}
