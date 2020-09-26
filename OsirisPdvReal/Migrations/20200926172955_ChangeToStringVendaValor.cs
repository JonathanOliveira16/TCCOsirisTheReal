using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class ChangeToStringVendaValor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValorVenda",
                table: "Vendas",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldMaxLength: 30);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ValorVenda",
                table: "Vendas",
                type: "float",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30);
        }
    }
}
