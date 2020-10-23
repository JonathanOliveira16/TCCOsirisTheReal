using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Cliente
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CPFcliente { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(80, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Nome cliente")]
        public string NomeCliente { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Favor insira um e-mail", ErrorMessageResourceName = "teste")]
        [StringLength(80, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "E-mail cliente")]
        public string EmailCliente { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(15, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Telefone")]
        public string TelefoneCliente { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "CEP")]
        public string CEPcliente { get; set; }
        public ICollection<ClienteBanca> ClienteBancas { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public ICollection<Venda> Vendas { get; set; }
    }
}
