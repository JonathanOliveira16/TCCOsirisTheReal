using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class ProdutoCompras
    {
        public int ProdutoId { get; set; }
        public Produto Produtos { get; set; }
        public int ComprasId { get; set; }
        public Compras Compras { get; set; }
    }
}
