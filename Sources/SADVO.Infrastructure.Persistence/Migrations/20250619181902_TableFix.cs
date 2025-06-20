using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADVO.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TableFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AsignacionCandidatos_PartidoRespaldaId",
                table: "AsignacionCandidatos",
                column: "PartidoRespaldaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionCandidatos_PartidosPoliticos_PartidoRespaldaId",
                table: "AsignacionCandidatos",
                column: "PartidoRespaldaId",
                principalTable: "PartidosPoliticos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionCandidatos_PartidosPoliticos_PartidoRespaldaId",
                table: "AsignacionCandidatos");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionCandidatos_PartidoRespaldaId",
                table: "AsignacionCandidatos");
        }
    }
}
