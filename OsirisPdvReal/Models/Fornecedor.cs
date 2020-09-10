using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Fornecedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public long? CNPJ { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Nome fornecedor")]
        public string NomeFornecedor { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Favor insira um e-mail", ErrorMessageResourceName = "teste")]
        [StringLength(80, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "E-mail fornecedor")]
        public string EmailFornecedor { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(16, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Telefone fornecedor")]
        public string TelefoneFornecedor { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Ponto focal")]
        public string PontoFocalFornecedor { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Logradouro")]
        public string LogradouroFornecedor { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(9, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "CEP")]
        public string CEPFornecedor { get; set; }
        public ICollection<FornecedorBanca> FornecedorBanca { get; set; }
        public ICollection<Compras> Compras { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }



    }
}
