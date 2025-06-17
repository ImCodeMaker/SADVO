using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADVO.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCandidatosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "PartidosPoliticosId",
                table: "Candidatos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_PartidosPoliticosId",
                table: "Candidatos",
                column: "PartidosPoliticosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatos_PartidosPoliticos_PartidosPoliticosId",
                table: "Candidatos",
                column: "PartidosPoliticosId",
                principalTable: "PartidosPoliticos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatos_PartidosPoliticos_PartidosPoliticosId",
                table: "Candidatos");

            migrationBuilder.DropIndex(
                name: "IX_Candidatos_PartidosPoliticosId",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "PartidosPoliticosId",
                table: "Candidatos");

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
    }
}
