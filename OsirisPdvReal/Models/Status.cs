using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Status
    {
        [Key]
        public int? StatusId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(30, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "status")]
        public String NomeStatus { get; set; }
        //TODO mapear no contexto
        public ICollection<Fornecedor> Fornecedores { get; set; }
        public ICollection<Compras> Compras { get; set; }
        public ICollection<Cliente> Clientes { get; set; }
        public ICollection<Jornaleiro> Jornaleiros { get; set; }
        public ICollection<Venda> Vendas { get; set; }
    }
}
