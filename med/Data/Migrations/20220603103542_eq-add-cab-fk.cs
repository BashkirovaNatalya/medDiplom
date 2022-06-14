using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class eqaddcabfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CabinetId",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_CabinetId",
                table: "Equipment",
                column: "CabinetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Cabinet_CabinetId",
                table: "Equipment",
                column: "CabinetId",
                principalTable: "Cabinet",
                principalColumn: "IdCabinet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Cabinet_CabinetId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_CabinetId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "CabinetId",
                table: "Equipment");
        }
    }
}
