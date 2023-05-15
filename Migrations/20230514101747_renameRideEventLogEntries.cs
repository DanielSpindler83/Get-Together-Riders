using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Get_Together_Riders.Migrations
{
    public partial class renameRideEventLogEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideEventLogEntrys_RideEvents_RideEventID",
                table: "RideEventLogEntrys");

            migrationBuilder.DropForeignKey(
                name: "FK_RideEventLogEntrys_Riders_RiderID",
                table: "RideEventLogEntrys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RideEventLogEntrys",
                table: "RideEventLogEntrys");

            migrationBuilder.RenameTable(
                name: "RideEventLogEntrys",
                newName: "RideEventLogEntries");

            migrationBuilder.RenameIndex(
                name: "IX_RideEventLogEntrys_RiderID",
                table: "RideEventLogEntries",
                newName: "IX_RideEventLogEntries_RiderID");

            migrationBuilder.RenameIndex(
                name: "IX_RideEventLogEntrys_RideEventID",
                table: "RideEventLogEntries",
                newName: "IX_RideEventLogEntries_RideEventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RideEventLogEntries",
                table: "RideEventLogEntries",
                column: "RideEventLogEntryID");

            migrationBuilder.AddForeignKey(
                name: "FK_RideEventLogEntries_RideEvents_RideEventID",
                table: "RideEventLogEntries",
                column: "RideEventID",
                principalTable: "RideEvents",
                principalColumn: "RideEventID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RideEventLogEntries_Riders_RiderID",
                table: "RideEventLogEntries",
                column: "RiderID",
                principalTable: "Riders",
                principalColumn: "RiderID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideEventLogEntries_RideEvents_RideEventID",
                table: "RideEventLogEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_RideEventLogEntries_Riders_RiderID",
                table: "RideEventLogEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RideEventLogEntries",
                table: "RideEventLogEntries");

            migrationBuilder.RenameTable(
                name: "RideEventLogEntries",
                newName: "RideEventLogEntrys");

            migrationBuilder.RenameIndex(
                name: "IX_RideEventLogEntries_RiderID",
                table: "RideEventLogEntrys",
                newName: "IX_RideEventLogEntrys_RiderID");

            migrationBuilder.RenameIndex(
                name: "IX_RideEventLogEntries_RideEventID",
                table: "RideEventLogEntrys",
                newName: "IX_RideEventLogEntrys_RideEventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RideEventLogEntrys",
                table: "RideEventLogEntrys",
                column: "RideEventLogEntryID");

            migrationBuilder.AddForeignKey(
                name: "FK_RideEventLogEntrys_RideEvents_RideEventID",
                table: "RideEventLogEntrys",
                column: "RideEventID",
                principalTable: "RideEvents",
                principalColumn: "RideEventID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RideEventLogEntrys_Riders_RiderID",
                table: "RideEventLogEntrys",
                column: "RiderID",
                principalTable: "Riders",
                principalColumn: "RiderID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
