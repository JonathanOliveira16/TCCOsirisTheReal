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
        [Display(Name = "Nome")]
        public string NomeProduto { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor")]
        public String ValorProduto { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Estoque")]
        public int QuantideProduto { get; set; }
        public ICollection<ProdutoCompras> ProdutoCompras { get; set; }
        public ICollection<VendaProduto> VendaProduto { get; set; }
        public int TipoProdId { get; set; }
        public TipoProduto tipoProduto { get; set; }
    }
}
