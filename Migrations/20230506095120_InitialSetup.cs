using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Get_Together_Riders.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RideEvents",
                columns: table => new
                {
                    RideEventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventCategory = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideEvents", x => x.RideEventID);
                });

            migrationBuilder.CreateTable(
                name: "Riders",
                columns: table => new
                {
                    RiderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BikeModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GTRNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riders", x => x.RiderID);
                });

            migrationBuilder.CreateTable(
                name: "RideEventEnrollments",
                columns: table => new
                {
                    RideEventEnrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiderID = table.Column<int>(type: "int", nullable: false),
                    RideEventID = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideEventEnrollments", x => x.RideEventEnrollmentID);
                    table.ForeignKey(
                        name: "FK_RideEventEnrollments_RideEvents_RideEventID",
                        column: x => x.RideEventID,
                        principalTable: "RideEvents",
                        principalColumn: "RideEventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideEventEnrollments_Riders_RiderID",
                        column: x => x.RiderID,
                        principalTable: "Riders",
                        principalColumn: "RiderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideEventEnrollments_RideEventID",
                table: "RideEventEnrollments",
                column: "RideEventID");

            migrationBuilder.CreateIndex(
                name: "IX_RideEventEnrollments_RiderID",
                table: "RideEventEnrollments",
                column: "RiderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideEventEnrollments");

            migrationBuilder.DropTable(
                name: "RideEvents");

            migrationBuilder.DropTable(
                name: "Riders");
        }
    }
}
