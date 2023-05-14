using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Get_Together_Riders.Migrations
{
    public partial class addRideEventLogEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RideEventLogEntrys",
                columns: table => new
                {
                    RideEventLogEntryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiderID = table.Column<int>(type: "int", nullable: false),
                    RideEventID = table.Column<int>(type: "int", nullable: false),
                    SuburbLeftFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KmsToCheckIn = table.Column<int>(type: "int", nullable: false),
                    TotalKms = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideEventLogEntrys", x => x.RideEventLogEntryID);
                    table.ForeignKey(
                        name: "FK_RideEventLogEntrys_RideEvents_RideEventID",
                        column: x => x.RideEventID,
                        principalTable: "RideEvents",
                        principalColumn: "RideEventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideEventLogEntrys_Riders_RiderID",
                        column: x => x.RiderID,
                        principalTable: "Riders",
                        principalColumn: "RiderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideEventLogEntrys_RideEventID",
                table: "RideEventLogEntrys",
                column: "RideEventID");

            migrationBuilder.CreateIndex(
                name: "IX_RideEventLogEntrys_RiderID",
                table: "RideEventLogEntrys",
                column: "RiderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideEventLogEntrys");
        }
    }
}
