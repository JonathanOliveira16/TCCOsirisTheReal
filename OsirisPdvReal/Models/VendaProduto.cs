using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class VendaProduto
    {
       
        public int VendaId { get; set; }
        public Venda Vendas { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produtos { get; set; }
        public int QuantidadeVendida { get; set; }
        public double ValorTotalDoProduto { get; set; }
    }
}
