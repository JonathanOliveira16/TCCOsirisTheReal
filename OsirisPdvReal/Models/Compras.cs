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
        [Display(Name = "Quantidade")]
        public int QuantidadeCompra { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data da compra")]
        public DateTime DataCompra { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Valor da compra")]
        public String ValorCompra { get; set; }
        public ICollection<ProdutoCompras> ProdutoCompras { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }

        public long? CNPJ { get; set; }
        public Fornecedor fornecedor { get; set; }


    }
}
