﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Venda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? VendaId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data da venda")]
        public DateTime DataVenda { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(30, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Valor da venda")]
        public string ValorVenda { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Quantidade")]
        public int QuantidadeVendida { get; set; }
        public string ProdutosSalvos { get; set; }
        public List<Produto> ItemVenda { get; set; }
        public int? StatusId { get; set; }
        public Status Status { get; set; }
        public int BancaId { get; set; }
        public Banca Bancas { get; set; }
        public long? CPFvJ { get; set; }
        public Jornaleiro Jornaleiros { get; set; }
        public long CPFcliente { get; set; }
        public Cliente Clientes { get; set; }
        public ICollection<VendaProduto> VendaProduto { get; set; }

    }
}
