﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<CompraFornecedores> CompraFornecedores { get; set; }
        public DbSet<FornecedorBanca> FornecedorBanca { get; set; }
        public DbSet<JornaleiroBanca> JornaleiroBanca { get; set; }
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
            modelBuilder.Entity<Banca>().HasMany(t => t.Vendas).WithOne(t => t.Bancas);
            modelBuilder.Entity<Tipo>().HasMany(t => t.Jornaleiro).WithOne(t => t.tipo);
            modelBuilder.Entity<Cliente>().HasMany(t => t.Vendas).WithOne(t => t.Clientes);

            modelBuilder.Entity<Fornecedor>().HasOne(e => e.Status).WithMany(e => e.Fornecedores).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Compras>().HasOne(e => e.Status).WithMany(e => e.Compras).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Cliente>().HasOne(e => e.Status).WithMany(e => e.Clientes).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Jornaleiro>().HasOne(e => e.Status).WithMany(e => e.Jornaleiros).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Venda>().HasOne(e => e.Status).WithMany(e => e.Vendas).HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Jornaleiro>().HasOne(e => e.tipo).WithMany(e => e.Jornaleiro).HasForeignKey(e => e.TipoId);
            modelBuilder.Entity<Venda>().HasOne(e => e.Bancas).WithMany(e => e.Vendas).HasForeignKey(e => e.BancaId);
            modelBuilder.Entity<Venda>().HasOne(e => e.Clientes).WithMany(e => e.Vendas).HasForeignKey(e => e.ClienteId);


            //many to many
            modelBuilder.Entity<ClienteBanca>().HasKey(x => new { x.BancaId, x.ClienteId });
            modelBuilder.Entity<FornecedorBanca>().HasKey(x => new { x.BancaId, x.CNPJ });
            modelBuilder.Entity<JornaleiroBanca>().HasKey(x => new { x.BancaId, x.CPF });
            modelBuilder.Entity<CompraFornecedores>().HasKey(x => new { x.ComprasId, x.CNPJ });
            modelBuilder.Entity<ProdutoCompras>().HasKey(x => new { x.ComprasId, x.ProdutoId });
            modelBuilder.Entity<VendaProduto>().HasKey(x => new { x.VendaId, x.ProdutoId });

            modelBuilder.Entity<ClienteBanca>().HasOne(x => x.Bancas).WithMany(x => x.ClienteBancas).HasForeignKey(x => x.BancaId);
            modelBuilder.Entity<FornecedorBanca>().HasOne(x => x.Bancas).WithMany(x => x.FornecedorBanca).HasForeignKey(x => x.BancaId);
            modelBuilder.Entity<JornaleiroBanca>().HasOne(x => x.Bancas).WithMany(x => x.JornaleiroBanca).HasForeignKey(x => x.BancaId);
            modelBuilder.Entity<CompraFornecedores>().HasOne(x => x.Compras).WithMany(x => x.ComprasFornecedor).HasForeignKey(x => x.ComprasId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProdutoCompras>().HasOne(x => x.Produtos).WithMany(x => x.ProdutoCompras).HasForeignKey(x => x.ProdutoId);
            modelBuilder.Entity<VendaProduto>().HasOne(x => x.Produtos).WithMany(x => x.VendaProduto).HasForeignKey(x => x.ProdutoId);

            modelBuilder.Entity<ClienteBanca>().HasOne(x => x.Clientes).WithMany(x => x.ClienteBancas).HasForeignKey(x => x.ClienteId);
            modelBuilder.Entity<FornecedorBanca>().HasOne(x => x.Fornecedores).WithMany(x => x.FornecedorBanca).HasForeignKey(x => x.CNPJ);
            modelBuilder.Entity<JornaleiroBanca>().HasOne(x => x.Jornaleiro).WithMany(x => x.JornaleiroBanca).HasForeignKey(x => x.BancaId);
            modelBuilder.Entity<CompraFornecedores>().HasOne(x => x.Fornecedor).WithMany(x => x.ComprasFornecedor).HasForeignKey(x => x.CNPJ).IsRequired().OnDelete(DeleteBehavior.Restrict);
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
