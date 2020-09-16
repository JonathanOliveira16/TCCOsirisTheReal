using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OsirisPdvReal.Models;

namespace OsirisPdvReal.Models
{
    public class Contexto : DbContext
    {
        public DbSet<Jornaleiro> Jornaleiros { get; set; }
        public DbSet<Banca> Bancas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Compras> Compras { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<TipoProduto> TipoProdutos { get; set; }
        public DbSet<CompraFornecedores> CompraFornecedores { get; set; }
        public DbSet<FornecedorBanca> FornecedorBanca { get; set; }
        public DbSet<ProdutoCompras> ProdutoCompras { get; set; }
        public DbSet<VendaProduto> VendaProduto { get; set; }


        public Contexto(DbContextOptions<Contexto> opcoes) : base(opcoes)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //one to many
            modelBuilder.Entity<Status>().HasMany(t => t.Fornecedores).WithOne(t => t.Status);
            modelBuilder.Entity<Status>().HasMany(t => t.Compras).WithOne(t => t.Status);
            modelBuilder.Entity<Status>().HasMany(t => t.Clientes).WithOne(t => t.Status);
            modelBuilder.Entity<Status>().HasMany(t => t.Jornaleiros).WithOne(t => t.Status);
            modelBuilder.Entity<Status>().HasMany(t => t.Vendas).WithOne(t => t.Status);
            modelBuilder.Entity<TipoProduto>().HasMany(t => t.Produtos).WithOne(t => t.tipoProduto);
            modelBuilder.Entity<Fornecedor>().HasMany(t => t.Compras).WithOne(t => t.fornecedor).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Banca>().HasMany(t => t.Vendas).WithOne(t => t.Bancas);
            modelBuilder.Entity<Tipo>().HasMany(t => t.Jornaleiro).WithOne(t => t.tipo);
            modelBuilder.Entity<Cliente>().HasMany(t => t.Vendas).WithOne(t => t.Clientes);

            modelBuilder.Entity<Fornecedor>().HasOne(e => e.Status).WithMany(e => e.Fornecedores).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Produto>().HasOne(e => e.tipoProduto).WithMany(e => e.Produtos).HasForeignKey(e => e.TipoProdId);
            modelBuilder.Entity<Compras>().HasOne(e => e.fornecedor).WithMany(e => e.Compras).HasForeignKey(e => e.CNPJ).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Compras>().HasOne(e => e.Status).WithMany(e => e.Compras).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Cliente>().HasOne(e => e.Status).WithMany(e => e.Clientes).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Jornaleiro>().HasOne(e => e.Status).WithMany(e => e.Jornaleiros).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Venda>().HasOne(e => e.Status).WithMany(e => e.Vendas).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Jornaleiro>().HasOne(e => e.tipo).WithMany(e => e.Jornaleiro).HasForeignKey(e => e.TipoId);
            modelBuilder.Entity<Venda>().HasOne(e => e.Bancas).WithMany(e => e.Vendas).HasForeignKey(e => e.BancaId);
            modelBuilder.Entity<Venda>().HasOne(e => e.Clientes).WithMany(e => e.Vendas).HasForeignKey(e => e.CPFcliente);


            //many to many
            modelBuilder.Entity<ClienteBanca>().HasKey(x => new { x.BancaId, x.CPFcliente });
            modelBuilder.Entity<FornecedorBanca>().HasKey(x => new { x.BancaId, x.CNPJ });
            modelBuilder.Entity<CompraFornecedores>().HasKey(x => new { x.ComprasId, x.CNPJ });
            modelBuilder.Entity<ProdutoCompras>().HasKey(x => new { x.ComprasId, x.ProdutoId });
            modelBuilder.Entity<VendaProduto>().HasKey(x => new { x.VendaId, x.ProdutoId });

            modelBuilder.Entity<ClienteBanca>().HasOne(x => x.Bancas).WithMany(x => x.ClienteBancas).HasForeignKey(x => x.BancaId);
            modelBuilder.Entity<FornecedorBanca>().HasOne(x => x.Bancas).WithMany(x => x.FornecedorBanca).HasForeignKey(x => x.BancaId);
            modelBuilder.Entity<ProdutoCompras>().HasOne(x => x.Produtos).WithMany(x => x.ProdutoCompras).HasForeignKey(x => x.ProdutoId);
            modelBuilder.Entity<VendaProduto>().HasOne(x => x.Produtos).WithMany(x => x.VendaProduto).HasForeignKey(x => x.ProdutoId);

            modelBuilder.Entity<ClienteBanca>().HasOne(x => x.Clientes).WithMany(x => x.ClienteBancas).HasForeignKey(x => x.CPFcliente);
            modelBuilder.Entity<FornecedorBanca>().HasOne(x => x.Fornecedores).WithMany(x => x.FornecedorBanca).HasForeignKey(x => x.CNPJ);
            modelBuilder.Entity<ProdutoCompras>().HasOne(x => x.Compras).WithMany(x => x.ProdutoCompras).HasForeignKey(x => x.ComprasId);
            modelBuilder.Entity<VendaProduto>().HasOne(x => x.Vendas).WithMany(x => x.VendaProduto).HasForeignKey(x => x.VendaId);


            modelBuilder.Entity<Tipo>().HasData(
                  new Tipo
                  {
                      TipoId = 1,
                      NomeTipo = "Admin"
                  },
                  new Tipo
                  {
                      TipoId = 2,
                      NomeTipo = "User"
                  }
            );


            modelBuilder.Entity<Fornecedor>().HasData(
                  new Fornecedor
                  {
                      CNPJ = 12345678912,
                      NomeFornecedor = "Sem fornecedor",
                      EmailFornecedor = "no@gmail.com",
                      CEPFornecedor = "123456789",
                      LogradouroFornecedor = "sem endereço",
                      PontoFocalFornecedor = "Sem ponto",
                      TelefoneFornecedor = "11111111",
                      StatusId = 1
                  }
            );

            modelBuilder.Entity<Status>().HasData(
              new Status
              {
                  StatusId = 1,
                  NomeStatus = "Ativo"
              },
              new Status
              {
                  StatusId = 2,
                  NomeStatus = "Inativo"
              },
              new Status
              {
                  StatusId = 3,
                  NomeStatus = "Cancelado"
              }
          );


        }

        public DbSet<OsirisPdvReal.Models.Produto> Produto { get; set; }
    }
}
