using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(80, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Nome produto")]
        public string NomeProduto { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(20, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Valor produto")]
        public double ValorProduto { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(35, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Quantidade produto")]
        public int QuantideProduto { get; set; }
        public ICollection<ProdutoCompras> ProdutoCompras { get; set; }
        public ICollection<VendaProduto> VendaProduto { get; set; }


    }
}
