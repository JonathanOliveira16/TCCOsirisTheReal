using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class ClienteBanca
    {
        public int BancaId { get; set; }
        public Banca Bancas { get; set; }
        public int ClienteId { get; set; }
        [ForeignKey(nameof(ClienteId))]
        public Cliente Clientes { get; set; }
        public double ValorTotal { get; set; }
    }
}
