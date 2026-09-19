using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minimal_api_cronJobs.Migrations
{
    /// <inheritdoc />
    public partial class RenameJobFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Jobs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Jobs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Jobs",
                newName: "Desc");

            migrationBuilder.RenameColumn(
                name: "CriadoEm",
                table: "Jobs",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "AtualizadoEm",
                table: "Jobs",
                newName: "UpdateOn");

            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Jobs",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Jobs",
                newName: "IdJob");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateOn",
                table: "Jobs",
                newName: "AtualizadoEm");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Jobs",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Jobs",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Jobs",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Jobs",
                newName: "CriadoEm");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Jobs",
                newName: "Ativo");

            migrationBuilder.RenameColumn(
                name: "IdJob",
                table: "Jobs",
                newName: "Id");
        }
    }
}
