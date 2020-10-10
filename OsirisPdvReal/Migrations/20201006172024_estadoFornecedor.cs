using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class estadoFornecedor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstadoFornecedor",
                table: "Fornecedores",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Fornecedores",
                keyColumn: "CNPJ",
                keyValue: 12345678912L,
                column: "EstadoFornecedor",
                value: "Rio de Janeiro");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoFornecedor",
                table: "Fornecedores");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Produto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Produto_StatusId",
                table: "Produto",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Status_StatusId",
                table: "Produto",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
