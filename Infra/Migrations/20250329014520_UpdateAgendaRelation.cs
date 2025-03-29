using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAgendaRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_Doctors_Id",
                table: "Agendas");

            migrationBuilder.AlterColumn<bool>(
                name: "Available",
                table: "Agendas",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "Agendas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_DoctorId",
                table: "Agendas",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_Doctors_DoctorId",
                table: "Agendas",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_Doctors_DoctorId",
                table: "Agendas");

            migrationBuilder.DropIndex(
                name: "IX_Agendas_DoctorId",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Agendas");

            migrationBuilder.AlterColumn<bool>(
                name: "Available",
                table: "Agendas",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_Doctors_Id",
                table: "Agendas",
                column: "Id",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
