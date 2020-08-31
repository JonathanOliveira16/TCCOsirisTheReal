﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OsirisPdvReal.Models;

namespace OsirisPdvReal.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20200831044010_ClienteBancaAdicionadoaoContexto")]
    partial class ClienteBancaAdicionadoaoContexto
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OsirisPdvReal.Models.Banca", b =>
                {
                    b.Property<int>("BancaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("JornaleiroId")
                        .HasColumnType("int");

                    b.Property<string>("NomeBanca")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("BancaId");

                    b.HasIndex("JornaleiroId");

                    b.ToTable("Bancas");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Cliente", b =>
                {
                    b.Property<int>("ClienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmailCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("NomeCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("TelefoneCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.HasKey("ClienteId");

                    b.HasIndex("StatusId");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.ClienteBanca", b =>
                {
                    b.Property<int>("BancaId")
                        .HasColumnType("int");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<double>("ValorTotal")
                        .HasColumnType("float");

                    b.HasKey("BancaId", "ClienteId");

                    b.HasIndex("ClienteId");

                    b.ToTable("ClienteBancas");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.CompraFornecedores", b =>
                {
                    b.Property<int?>("ComprasId")
                        .HasColumnType("int");

                    b.Property<int?>("FornecedorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataCompra")
                        .HasColumnType("datetime2");

                    b.Property<double>("ValorCompra")
                        .HasColumnType("float");

                    b.HasKey("ComprasId", "FornecedorId");

                    b.HasIndex("FornecedorId");

                    b.ToTable("CompraFornecedores");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Compras", b =>
                {
                    b.Property<int?>("ComprasId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NomeItemCompra")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<int>("QuantidadeCompra")
                        .HasColumnType("int")
                        .HasMaxLength(9);

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("ComprasId");

                    b.HasIndex("StatusId");

                    b.ToTable("Compras");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Fornecedor", b =>
                {
                    b.Property<int?>("FornecedorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CEPFornecedor")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("EmailFornecedor")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("LogradouroFornecedor")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("NomeFornecedor")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("PontoFocalFornecedor")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("TelefoneFornecedor")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.HasKey("FornecedorId");

                    b.HasIndex("StatusId");

                    b.ToTable("Fornecedores");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.FornecedorBanca", b =>
                {
                    b.Property<int>("BancaId")
                        .HasColumnType("int");

                    b.Property<int>("FornecedorId")
                        .HasColumnType("int");

                    b.HasKey("BancaId", "FornecedorId");

                    b.HasIndex("FornecedorId");

                    b.ToTable("FornecedorBanca");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Jornaleiro", b =>
                {
                    b.Property<int>("JornaleiroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BancaId")
                        .HasColumnType("int");

                    b.Property<string>("EmailJornaleiro")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("NomeJornaleiro")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("SenhaJornaleiro")
                        .IsRequired()
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("TelefoneJornaleiro")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.HasKey("JornaleiroId");

                    b.HasIndex("BancaId");

                    b.HasIndex("StatusId");

                    b.ToTable("Jornaleiros");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.JornaleiroBanca", b =>
                {
                    b.Property<int>("BancaId")
                        .HasColumnType("int");

                    b.Property<int>("JornaleiroId")
                        .HasColumnType("int");

                    b.HasKey("BancaId", "JornaleiroId");

                    b.ToTable("JornaleiroBanca");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Produto", b =>
                {
                    b.Property<int>("ProdutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NomeProduto")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<int>("QuantideProduto")
                        .HasColumnType("int")
                        .HasMaxLength(35);

                    b.Property<double>("ValorProduto")
                        .HasColumnType("float")
                        .HasMaxLength(20);

                    b.Property<int?>("VendaId")
                        .HasColumnType("int");

                    b.HasKey("ProdutoId");

                    b.HasIndex("VendaId");

                    b.ToTable("Produto");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.ProdutoCompras", b =>
                {
                    b.Property<int>("ComprasId")
                        .HasColumnType("int");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.HasKey("ComprasId", "ProdutoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ProdutoCompras");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Status", b =>
                {
                    b.Property<int?>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NomeStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("StatusId");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Venda", b =>
                {
                    b.Property<int?>("VendaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BancaId")
                        .HasColumnType("int");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataVenda")
                        .HasColumnType("datetime2");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<double>("ValorVenda")
                        .HasColumnType("float")
                        .HasMaxLength(30);

                    b.HasKey("VendaId");

                    b.HasIndex("BancaId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("StatusId");

                    b.ToTable("Vendas");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.VendaProduto", b =>
                {
                    b.Property<int>("VendaId")
                        .HasColumnType("int");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("QuantidadeVendida")
                        .HasColumnType("int");

                    b.HasKey("VendaId", "ProdutoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("VendaProduto");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Banca", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Jornaleiro", "Jornaleiro")
                        .WithMany()
                        .HasForeignKey("JornaleiroId");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Cliente", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Status", "Status")
                        .WithMany("Clientes")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.ClienteBanca", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Banca", "Bancas")
                        .WithMany("ClienteBancas")
                        .HasForeignKey("BancaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OsirisPdvReal.Models.Cliente", "Clientes")
                        .WithMany("ClienteBancas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.CompraFornecedores", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Compras", "Compras")
                        .WithMany("ComprasFornecedor")
                        .HasForeignKey("ComprasId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OsirisPdvReal.Models.Fornecedor", "Fornecedor")
                        .WithMany("ComprasFornecedor")
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Compras", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Status", "Status")
                        .WithMany("Compras")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Fornecedor", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Status", "Status")
                        .WithMany("Fornecedores")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.FornecedorBanca", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Banca", "Bancas")
                        .WithMany("FornecedorBanca")
                        .HasForeignKey("BancaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OsirisPdvReal.Models.Fornecedor", "Fornecedores")
                        .WithMany("FornecedorBanca")
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Jornaleiro", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Banca", null)
                        .WithMany("Jornaleiros")
                        .HasForeignKey("BancaId");

                    b.HasOne("OsirisPdvReal.Models.Status", "Status")
                        .WithMany("Jornaleiros")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.JornaleiroBanca", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Banca", "Bancas")
                        .WithMany("JornaleiroBanca")
                        .HasForeignKey("BancaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OsirisPdvReal.Models.Jornaleiro", "Jornaleiro")
                        .WithMany("JornaleiroBanca")
                        .HasForeignKey("BancaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Produto", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Venda", null)
                        .WithMany("ItemVenda")
                        .HasForeignKey("VendaId");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.ProdutoCompras", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Compras", "Compras")
                        .WithMany("ProdutoCompras")
                        .HasForeignKey("ComprasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OsirisPdvReal.Models.Produto", "Produtos")
                        .WithMany("ProdutoCompras")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsirisPdvReal.Models.Venda", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Banca", "Bancas")
                        .WithMany("Vendas")
                        .HasForeignKey("BancaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OsirisPdvReal.Models.Cliente", "Clientes")
                        .WithMany("Vendas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OsirisPdvReal.Models.Status", "Status")
                        .WithMany("Vendas")
                        .HasForeignKey("StatusId");
                });

            modelBuilder.Entity("OsirisPdvReal.Models.VendaProduto", b =>
                {
                    b.HasOne("OsirisPdvReal.Models.Produto", "Produtos")
                        .WithMany("VendaProduto")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OsirisPdvReal.Models.Venda", "Vendas")
                        .WithMany("VendaProduto")
                        .HasForeignKey("VendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
