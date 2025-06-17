using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADVO.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedTableAsignacionDirigente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsignacionDirigentes_PartidosPoliticos_partidosPoliticosId",
                table: "AsignacionDirigentes");

            migrationBuilder.DropIndex(
                name: "IX_AsignacionDirigentes_partidosPoliticosId",
                table: "AsignacionDirigentes");

            migrationBuilder.DropColumn(
                name: "partidosPoliticosId",
                table: "AsignacionDirigentes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAsignacion",
                table: "AsignacionDirigentes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAsignacion",
                table: "AsignacionDirigentes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "partidosPoliticosId",
                table: "AsignacionDirigentes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionDirigentes_partidosPoliticosId",
                table: "AsignacionDirigentes",
                column: "partidosPoliticosId");

            migrationBuilder.AddForeignKey(
                name: "FK_AsignacionDirigentes_PartidosPoliticos_partidosPoliticosId",
                table: "AsignacionDirigentes",
                column: "partidosPoliticosId",
                principalTable: "PartidosPoliticos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
