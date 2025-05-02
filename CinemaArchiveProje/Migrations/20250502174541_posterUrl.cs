using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaArchiveProje.Migrations
{
    /// <inheritdoc />
    public partial class posterUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PosterPath",
                table: "Movies",
                newName: "PosterUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PosterUrl",
                table: "Movies",
                newName: "PosterPath");
        }
    }
}
