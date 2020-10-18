using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class VendasBancaRelatorio
    {
        public String Posicao { get; set; }
        public int QuantidadeDeVendas { get; set; }
        public String NomeBanca { get; set; }
        public String ValorTotalVenda { get; set; }
        public String Bairro { get; set; }
    }
}
