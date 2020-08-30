using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class ClienteBanca
    {
        public int BancaId { get; set; }
        public Banca Bancas { get; set; }
        public int ClienteId { get; set; }
        public Cliente Clientes { get; set; }
        public double ValorTotal { get; set; }
    }
}
