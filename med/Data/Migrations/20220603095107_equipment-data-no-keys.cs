using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class equipmentdatanokeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateNubber",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CommissioningYear",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DecommissioningYear",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ManufacturingYear",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PeriodTO",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateNubber",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "CommissioningYear",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "DecommissioningYear",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "ManufacturingYear",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "PeriodTO",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Equipment");
        }
    }
}
