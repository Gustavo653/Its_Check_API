using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItsCheck.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmbulanceId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AmbulanceId",
                table: "AspNetUsers",
                column: "AmbulanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ambulances_AmbulanceId",
                table: "AspNetUsers",
                column: "AmbulanceId",
                principalTable: "Ambulances",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ambulances_AmbulanceId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AmbulanceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AmbulanceId",
                table: "AspNetUsers");
        }
    }
}
