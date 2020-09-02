using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class JornaleiroBanca
    {
        public int BancaId { get; set; }
        public Banca Bancas { get; set; }
        public int CPF { get; set; }
        public Jornaleiro Jornaleiro { get; set; }

    }
}
