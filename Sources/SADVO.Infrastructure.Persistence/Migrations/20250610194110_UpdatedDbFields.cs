using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADVO.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDbFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Ciudadanos");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Ciudadanos");

            migrationBuilder.RenameColumn(
                name: "Fecha_Creacion",
                table: "PuestosElectivos",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "Fecha_Creacion",
                table: "PartidosPoliticos",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "Fecha_Creacion",
                table: "Ciudadanos",
                newName: "FechaCreacion");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreCompleto",
                table: "Ciudadanos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Candidatos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "Ciudadanos");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "PuestosElectivos",
                newName: "Fecha_Creacion");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "PartidosPoliticos",
                newName: "Fecha_Creacion");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Ciudadanos",
                newName: "Fecha_Creacion");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Ciudadanos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Ciudadanos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Candidatos",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
