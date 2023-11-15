using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItsCheck.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterUser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ambulances_AmbulanceId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AmbulanceId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ambulances_AmbulanceId",
                table: "AspNetUsers",
                column: "AmbulanceId",
                principalTable: "Ambulances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ambulances_AmbulanceId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AmbulanceId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ambulances_AmbulanceId",
                table: "AspNetUsers",
                column: "AmbulanceId",
                principalTable: "Ambulances",
                principalColumn: "Id");
        }
    }
}
