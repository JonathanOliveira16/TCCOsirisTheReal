using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class adjustTablesId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VendaId",
                table: "Vendas",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "VendaId",
                table: "VendaProduto",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "VendaId",
                table: "Produto",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "VendaId",
                table: "Vendas",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "VendaId",
                table: "VendaProduto",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "VendaId",
                table: "Produto",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
