using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendaProdutoId",
                table: "VendaProduto",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendaProdutoId",
                table: "VendaProduto");
        }
    }
}
