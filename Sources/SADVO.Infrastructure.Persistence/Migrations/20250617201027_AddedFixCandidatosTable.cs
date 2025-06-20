using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADVO.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedFixCandidatosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartidoPoliticoId",
                table: "Candidatos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_PartidoPoliticoId",
                table: "Candidatos",
                column: "PartidoPoliticoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatos_PartidosPoliticos_PartidoPoliticoId",
                table: "Candidatos",
                column: "PartidoPoliticoId",
                principalTable: "PartidosPoliticos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatos_PartidosPoliticos_PartidoPoliticoId",
                table: "Candidatos");

            migrationBuilder.DropIndex(
                name: "IX_Candidatos_PartidoPoliticoId",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "PartidoPoliticoId",
                table: "Candidatos");
        }
    }
}
