using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class imageinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobImage",
                columns: table => new
                {
                    IdJobImage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobImage", x => x.IdJobImage);
                    table.ForeignKey(
                        name: "FK_JobImage_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "IdJob");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobImage_JobId",
                table: "JobImage",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobImage");
        }
    }
}
