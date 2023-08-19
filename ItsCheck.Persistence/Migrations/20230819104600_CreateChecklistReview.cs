using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ItsCheck.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateChecklistReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChecklistReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Observation = table.Column<string>(type: "text", nullable: false),
                    AmbulanceId = table.Column<int>(type: "integer", nullable: false),
                    ChecklistId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistReviews_Ambulances_AmbulanceId",
                        column: x => x.AmbulanceId,
                        principalTable: "Ambulances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistReviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistReviews_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistReviews_AmbulanceId",
                table: "ChecklistReviews",
                column: "AmbulanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistReviews_ChecklistId",
                table: "ChecklistReviews",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistReviews_UserId",
                table: "ChecklistReviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistReviews");
        }
    }
}
