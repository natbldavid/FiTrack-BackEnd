using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToWorkoutSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "tbl_workout_sessions",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_user_id_is_active",
                table: "tbl_workout_sessions",
                columns: new[] { "user_id", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_workout_sessions_user_id_is_active",
                table: "tbl_workout_sessions");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "tbl_workout_sessions");
        }
    }
}
