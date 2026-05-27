using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EArenaTournamentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TournamentLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoImageUrl",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoImageUrl",
                table: "Tournaments");
        }
    }
}
