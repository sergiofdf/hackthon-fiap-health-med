using Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEspecialidadeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove a coluna antiga de string
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Doctors");

            // Adiciona a nova coluna como inteiro
            migrationBuilder.AddColumn<int>(
                name: "Specialty",
                table: "Doctors",
                type: "integer",
                nullable: false,
                defaultValue: (int)Specialties.Geral); // Definir um valor padrão evita problemas
            
            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Specialty",
                table: "Doctors",
                column: "Specialty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_Specialty",
                table: "Doctors");
        }
    }
}
