using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class desfazerTeste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendaProdutoId",
                table: "VendaProduto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendaProdutoId",
                table: "VendaProduto",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
