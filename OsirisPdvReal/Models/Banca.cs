using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Banca
    {

        [Key]
        public int BancaId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Nome")]
        public String NomeBanca { get; set; }
        public long? CPF { get; set; }
        public Jornaleiro Jornaleiro { get; set; }
        public ICollection<Jornaleiro> Jornaleiros { get; set; }
        public ICollection<Venda> Vendas { get; set; }

        public ICollection<ClienteBanca> ClienteBancas { get; set; }
        public ICollection<FornecedorBanca> FornecedorBanca { get; set; }


    }
}
