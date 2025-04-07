using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class MoveHourlyPriceToAgendaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyPrice",
                table: "Doctors");

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyPrice",
                table: "Agendas",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyPrice",
                table: "Agendas");

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyPrice",
                table: "Doctors",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
