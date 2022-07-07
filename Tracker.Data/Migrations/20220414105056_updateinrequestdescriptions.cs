using Microsoft.EntityFrameworkCore.Migrations;

namespace Tracker.Data.Migrations
{
    public partial class updateinrequestdescriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestStatusId",
                table: "RequestDescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestDescriptions_RequestStatusId",
                table: "RequestDescriptions",
                column: "RequestStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDescriptions_requestStatuses_RequestStatusId",
                table: "RequestDescriptions",
                column: "RequestStatusId",
                principalTable: "requestStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestDescriptions_requestStatuses_RequestStatusId",
                table: "RequestDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_RequestDescriptions_RequestStatusId",
                table: "RequestDescriptions");

            migrationBuilder.DropColumn(
                name: "RequestStatusId",
                table: "RequestDescriptions");
        }
    }
}
