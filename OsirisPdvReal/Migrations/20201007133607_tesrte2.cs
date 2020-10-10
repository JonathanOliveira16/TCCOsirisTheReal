using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class tesrte2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<long>(
                name: "CPFvJ",
                table: "Vendas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_CPFvJ",
                table: "Vendas",
                column: "CPFvJ");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Jornaleiros_CPFvJ",
                table: "Vendas",
                column: "CPFvJ",
                principalTable: "Jornaleiros",
                principalColumn: "CPF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Jornaleiros_CPFvJ",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_CPFvJ",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "CPFvJ",
                table: "Vendas");

            migrationBuilder.AddColumn<long>(
                name: "CpfJornaVenda",
                table: "Vendas",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_CpfJornaVenda",
                table: "Vendas",
                column: "CpfJornaVenda");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Jornaleiros_CpfJornaVenda",
                table: "Vendas",
                column: "CpfJornaVenda",
                principalTable: "Jornaleiros",
                principalColumn: "CPF");
        }
    }
}
