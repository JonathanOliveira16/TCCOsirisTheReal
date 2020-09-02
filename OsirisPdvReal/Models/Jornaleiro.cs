using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Jornaleiro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CPF { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Nome")]
        public string NomeJornaleiro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Favor insira um e-mail",ErrorMessageResourceName = "teste")]
        [StringLength(80, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "E-mail")]
        public string EmailJornaleiro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(12, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Telefone")]
        public string TelefoneJornaleiro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.Password)]
        [StringLength(400, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Senha")]
        public string SenhaJornaleiro { get; set; }
        public ICollection<JornaleiroBanca> JornaleiroBanca { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int TipoId { get; set; }
        public Tipo tipo { get; set; }

    }
}
