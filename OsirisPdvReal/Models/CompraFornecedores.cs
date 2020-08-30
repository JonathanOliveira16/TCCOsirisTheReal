using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class CompraFornecedores
    {
        public int? FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public int? ComprasId { get; set; }
        public Compras Compras { get; set; }
        public DateTime DataCompra { get; set; }
        public double ValorCompra { get; set; }
    }
}
