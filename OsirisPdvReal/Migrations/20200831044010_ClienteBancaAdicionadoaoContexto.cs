using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class ClienteBancaAdicionadoaoContexto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClienteBanca_Bancas_BancaId",
                table: "ClienteBanca");

            migrationBuilder.DropForeignKey(
                name: "FK_ClienteBanca_Clientes_ClienteId",
                table: "ClienteBanca");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClienteBanca",
                table: "ClienteBanca");

            migrationBuilder.RenameTable(
                name: "ClienteBanca",
                newName: "ClienteBancas");

            migrationBuilder.RenameIndex(
                name: "IX_ClienteBanca_ClienteId",
                table: "ClienteBancas",
                newName: "IX_ClienteBancas_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClienteBancas",
                table: "ClienteBancas",
                columns: new[] { "BancaId", "ClienteId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClienteBancas_Bancas_BancaId",
                table: "ClienteBancas",
                column: "BancaId",
                principalTable: "Bancas",
                principalColumn: "BancaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClienteBancas_Clientes_ClienteId",
                table: "ClienteBancas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClienteBancas_Bancas_BancaId",
                table: "ClienteBancas");

            migrationBuilder.DropForeignKey(
                name: "FK_ClienteBancas_Clientes_ClienteId",
                table: "ClienteBancas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClienteBancas",
                table: "ClienteBancas");

            migrationBuilder.RenameTable(
                name: "ClienteBancas",
                newName: "ClienteBanca");

            migrationBuilder.RenameIndex(
                name: "IX_ClienteBancas_ClienteId",
                table: "ClienteBanca",
                newName: "IX_ClienteBanca_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClienteBanca",
                table: "ClienteBanca",
                columns: new[] { "BancaId", "ClienteId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClienteBanca_Bancas_BancaId",
                table: "ClienteBanca",
                column: "BancaId",
                principalTable: "Bancas",
                principalColumn: "BancaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClienteBanca_Clientes_ClienteId",
                table: "ClienteBanca",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
