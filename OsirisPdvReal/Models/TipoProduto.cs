using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class TipoProduto
    {
        [Key]
        public int TipoProdId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]

        public string NomeTipoProduto { get; set; }
        public ICollection<Produto> Produtos { get; set; }
    }
}
