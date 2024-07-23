using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EichkustMusic.Tracks.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PlaylistUserIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Playlists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Playlists");
        }
    }
}
