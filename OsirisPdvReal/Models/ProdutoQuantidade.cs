using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class ProdutoQuantidade
    {
        public string NomeProduto { get; set; }
        public double ValorMensal { get; set; }
        public double ValorAnual { get; set; }
        public int QuantidadeMensal { get; set; }
        public int QuantidadeAnual { get; set; }
    }
}
