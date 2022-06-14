using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace med.Data.Migrations
{
    public partial class jobinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    IdApplication = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDatePlan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDateFact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishDatePlan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishDateFact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.IdApplication);
                    table.ForeignKey(
                        name: "FK_Application_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "IdEmployee");
                    table.ForeignKey(
                        name: "FK_Application_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "IdEquipment");
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    IdJob = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    StartDatePlan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDateFact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishDatePlan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishDateFact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoursPlan = table.Column<double>(type: "float", nullable: false),
                    HoursFact = table.Column<double>(type: "float", nullable: false),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.IdJob);
                    table.ForeignKey(
                        name: "FK_Job_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "IdApplication");
                    table.ForeignKey(
                        name: "FK_Job_JobType_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobType",
                        principalColumn: "IdJobType");
                    table.ForeignKey(
                        name: "FK_Job_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "IdStage");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Application_EmployeeId",
                table: "Application",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Application_EquipmentId",
                table: "Application",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_ApplicationId",
                table: "Job",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_JobTypeId",
                table: "Job",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_StageId",
                table: "Job",
                column: "StageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "Application");
        }
    }
}
