using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EichkustMusic.Tracks.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlbumCoverImagePathAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImagePath",
                table: "Albums",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImagePath",
                table: "Albums");
        }
    }
}
