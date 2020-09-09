using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Compras
    {
        [Key]
        public int? ComprasId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(80, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Nome Item")]
        public string NomeItemCompra { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(9, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Quantidade")]
        public int QuantidadeCompra { get; set; }
        public ICollection<CompraFornecedores> ComprasFornecedor { get; set; }
        public ICollection<ProdutoCompras> ProdutoCompras { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }


    }
}
