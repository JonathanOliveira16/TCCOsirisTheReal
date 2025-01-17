﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OsirisPdvReal.Migrations
{
    public partial class criacaoBd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeStatus = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

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

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    TipoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTipo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.TipoId);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    CPFcliente = table.Column<long>(nullable: false),
                    NomeCliente = table.Column<string>(maxLength: 80, nullable: false),
                    EmailCliente = table.Column<string>(maxLength: 80, nullable: false),
                    TelefoneCliente = table.Column<string>(maxLength: 15, nullable: false),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.CPFcliente);
                    table.ForeignKey(
                        name: "FK_Clientes_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    CNPJ = table.Column<long>(nullable: false),
                    NomeFornecedor = table.Column<string>(maxLength: 100, nullable: false),
                    EmailFornecedor = table.Column<string>(maxLength: 80, nullable: false),
                    TelefoneFornecedor = table.Column<string>(maxLength: 16, nullable: false),
                    PontoFocalFornecedor = table.Column<string>(maxLength: 100, nullable: false),
                    LogradouroFornecedor = table.Column<string>(maxLength: 100, nullable: false),
                    CEPFornecedor = table.Column<string>(maxLength: 9, nullable: false),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.CNPJ);
                    table.ForeignKey(
                        name: "FK_Fornecedores_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    ComprasId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeItemCompra = table.Column<string>(maxLength: 80, nullable: false),
                    QuantidadeCompra = table.Column<int>(nullable: false),
                    DataCompra = table.Column<DateTime>(nullable: false),
                    ValorCompra = table.Column<string>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    CNPJ = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.ComprasId);
                    table.ForeignKey(
                        name: "FK_Compras_Fornecedores_CNPJ",
                        column: x => x.CNPJ,
                        principalTable: "Fornecedores",
                        principalColumn: "CNPJ",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compras_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompraFornecedores",
                columns: table => new
                {
                    CNPJ = table.Column<long>(nullable: false),
                    ComprasId = table.Column<int>(nullable: false),
                    FornecedorCNPJ = table.Column<long>(nullable: true),
                    DataCompra = table.Column<DateTime>(nullable: false),
                    ValorCompra = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraFornecedores", x => new { x.ComprasId, x.CNPJ });
                    table.ForeignKey(
                        name: "FK_CompraFornecedores_Compras_ComprasId",
                        column: x => x.ComprasId,
                        principalTable: "Compras",
                        principalColumn: "ComprasId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompraFornecedores_Fornecedores_FornecedorCNPJ",
                        column: x => x.FornecedorCNPJ,
                        principalTable: "Fornecedores",
                        principalColumn: "CNPJ",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClienteBanca",
                columns: table => new
                {
                    BancaId = table.Column<int>(nullable: false),
                    CPFcliente = table.Column<long>(nullable: false),
                    ValorTotal = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteBanca", x => new { x.BancaId, x.CPFcliente });
                    table.ForeignKey(
                        name: "FK_ClienteBanca_Clientes_CPFcliente",
                        column: x => x.CPFcliente,
                        principalTable: "Clientes",
                        principalColumn: "CPFcliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FornecedorBanca",
                columns: table => new
                {
                    CNPJ = table.Column<long>(nullable: false),
                    BancaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedorBanca", x => new { x.BancaId, x.CNPJ });
                    table.ForeignKey(
                        name: "FK_FornecedorBanca_Fornecedores_CNPJ",
                        column: x => x.CNPJ,
                        principalTable: "Fornecedores",
                        principalColumn: "CNPJ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jornaleiros",
                columns: table => new
                {
                    CPF = table.Column<long>(nullable: false),
                    NomeJornaleiro = table.Column<string>(maxLength: 100, nullable: false),
                    EmailJornaleiro = table.Column<string>(maxLength: 80, nullable: false),
                    TelefoneJornaleiro = table.Column<string>(nullable: false),
                    SenhaJornaleiro = table.Column<string>(maxLength: 20, nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    TipoId = table.Column<int>(nullable: false),
                    BancaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jornaleiros", x => x.CPF);
                    table.ForeignKey(
                        name: "FK_Jornaleiros_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Jornaleiros_Tipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "Tipos",
                        principalColumn: "TipoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bancas",
                columns: table => new
                {
                    BancaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeBanca = table.Column<string>(maxLength: 100, nullable: false),
                    Bairro = table.Column<string>(nullable: false),
                    CPF = table.Column<long>(nullable: true),
                    JornaleiroCPF = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancas", x => x.BancaId);
                    table.ForeignKey(
                        name: "FK_Bancas_Jornaleiros_JornaleiroCPF",
                        column: x => x.JornaleiroCPF,
                        principalTable: "Jornaleiros",
                        principalColumn: "CPF",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    VendaId = table.Column<int>(nullable: false),
                    DataVenda = table.Column<DateTime>(nullable: false),
                    ValorVenda = table.Column<double>(maxLength: 30, nullable: false),
                    QuantidadeVendida = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: true),
                    BancaId = table.Column<int>(nullable: false),
                    CPFcliente = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.VendaId);
                    table.ForeignKey(
                        name: "FK_Vendas_Bancas_BancaId",
                        column: x => x.BancaId,
                        principalTable: "Bancas",
                        principalColumn: "BancaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vendas_Clientes_CPFcliente",
                        column: x => x.CPFcliente,
                        principalTable: "Clientes",
                        principalColumn: "CPFcliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vendas_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    ProdutoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeProduto = table.Column<string>(maxLength: 80, nullable: false),
                    ValorProduto = table.Column<string>(nullable: false),
                    QuantideProduto = table.Column<int>(nullable: false),
                    TipoProdId = table.Column<int>(nullable: false),
                    VendaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.ProdutoId);
                    table.ForeignKey(
                        name: "FK_Produto_TipoProdutos_TipoProdId",
                        column: x => x.TipoProdId,
                        principalTable: "TipoProdutos",
                        principalColumn: "TipoProdId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Produto_Vendas_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Vendas",
                        principalColumn: "VendaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoCompras",
                columns: table => new
                {
                    ProdutoId = table.Column<int>(nullable: false),
                    ComprasId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoCompras", x => new { x.ComprasId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_ProdutoCompras_Compras_ComprasId",
                        column: x => x.ComprasId,
                        principalTable: "Compras",
                        principalColumn: "ComprasId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoCompras_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendaProduto",
                columns: table => new
                {
                    VendaId = table.Column<int>(nullable: false),
                    ProdutoId = table.Column<int>(nullable: false),
                    QuantidadeVendida = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendaProduto", x => new { x.VendaId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_VendaProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendaProduto_Vendas_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Vendas",
                        principalColumn: "VendaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "StatusId", "NomeStatus" },
                values: new object[,]
                {
                    { 1, "Ativo" },
                    { 2, "Inativo" },
                    { 3, "Cancelado" }
                });

            migrationBuilder.InsertData(
                table: "Tipos",
                columns: new[] { "TipoId", "NomeTipo" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Fornecedores",
                columns: new[] { "CNPJ", "CEPFornecedor", "EmailFornecedor", "LogradouroFornecedor", "NomeFornecedor", "PontoFocalFornecedor", "StatusId", "TelefoneFornecedor" },
                values: new object[] { 12345678912L, "123456789", "no@gmail.com", "sem endereço", "Sem fornecedor", "Sem ponto", 1, "11111111" });

            migrationBuilder.CreateIndex(
                name: "IX_Bancas_JornaleiroCPF",
                table: "Bancas",
                column: "JornaleiroCPF");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteBanca_CPFcliente",
                table: "ClienteBanca",
                column: "CPFcliente");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_StatusId",
                table: "Clientes",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraFornecedores_FornecedorCNPJ",
                table: "CompraFornecedores",
                column: "FornecedorCNPJ");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CNPJ",
                table: "Compras",
                column: "CNPJ");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_StatusId",
                table: "Compras",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FornecedorBanca_CNPJ",
                table: "FornecedorBanca",
                column: "CNPJ");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_StatusId",
                table: "Fornecedores",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Jornaleiros_BancaId",
                table: "Jornaleiros",
                column: "BancaId");

            migrationBuilder.CreateIndex(
                name: "IX_Jornaleiros_StatusId",
                table: "Jornaleiros",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Jornaleiros_TipoId",
                table: "Jornaleiros",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_TipoProdId",
                table: "Produto",
                column: "TipoProdId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_VendaId",
                table: "Produto",
                column: "VendaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoCompras_ProdutoId",
                table: "ProdutoCompras",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_VendaProduto_ProdutoId",
                table: "VendaProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_BancaId",
                table: "Vendas",
                column: "BancaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_CPFcliente",
                table: "Vendas",
                column: "CPFcliente");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_StatusId",
                table: "Vendas",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClienteBanca_Bancas_BancaId",
                table: "ClienteBanca",
                column: "BancaId",
                principalTable: "Bancas",
                principalColumn: "BancaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FornecedorBanca_Bancas_BancaId",
                table: "FornecedorBanca",
                column: "BancaId",
                principalTable: "Bancas",
                principalColumn: "BancaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jornaleiros_Bancas_BancaId",
                table: "Jornaleiros",
                column: "BancaId",
                principalTable: "Bancas",
                principalColumn: "BancaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bancas_Jornaleiros_JornaleiroCPF",
                table: "Bancas");

            migrationBuilder.DropTable(
                name: "ClienteBanca");

            migrationBuilder.DropTable(
                name: "CompraFornecedores");

            migrationBuilder.DropTable(
                name: "FornecedorBanca");

            migrationBuilder.DropTable(
                name: "ProdutoCompras");

            migrationBuilder.DropTable(
                name: "VendaProduto");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "TipoProdutos");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Jornaleiros");

            migrationBuilder.DropTable(
                name: "Bancas");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Tipos");
        }
    }
}
