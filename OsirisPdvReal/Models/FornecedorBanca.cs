using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class FornecedorBanca
    {
        public int CNPJ { get; set; }
        public Fornecedor Fornecedores { get; set; }
        public int BancaId  { get; set; }
        public Banca Bancas { get; set; }

    }
}
