using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class jobnostage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Stage_StageId",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Job_StageId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "Job");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StageId",
                table: "Job",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Job_StageId",
                table: "Job",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Stage_StageId",
                table: "Job",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "IdStage");
        }
    }
}
