using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class tipoProdEBairro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoProdId",
                table: "Produto",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CEPFornecedor",
                table: "Fornecedores",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Bancas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TipoProdutos",
                columns: table => new
                {
                    TipoProdId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTipoProduto = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProdutos", x => x.TipoProdId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_TipoProdId",
                table: "Produto",
                column: "TipoProdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_TipoProdutos_TipoProdId",
                table: "Produto",
                column: "TipoProdId",
                principalTable: "TipoProdutos",
                principalColumn: "TipoProdId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_TipoProdutos_TipoProdId",
                table: "Produto");

            migrationBuilder.DropTable(
                name: "TipoProdutos");

            migrationBuilder.DropIndex(
                name: "IX_Produto_TipoProdId",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "TipoProdId",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Bancas");

            migrationBuilder.AlterColumn<string>(
                name: "CEPFornecedor",
                table: "Fornecedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 9);
        }
    }
}
