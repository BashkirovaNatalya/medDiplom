using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class licenseinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "License",
                columns: table => new
                {
                    IdLicense = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_License", x => x.IdLicense);
                    table.ForeignKey(
                        name: "FK_License_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "IdEmployee");
                    table.ForeignKey(
                        name: "FK_License_EquipmentType_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentType",
                        principalColumn: "IdEquipmentType");
                });

            migrationBuilder.CreateIndex(
                name: "IX_License_EmployeeId",
                table: "License",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_License_EquipmentTypeId",
                table: "License",
                column: "EquipmentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "License");
        }
    }
}
