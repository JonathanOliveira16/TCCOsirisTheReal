﻿using System;
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
        public long CPF { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(100, ErrorMessage = "Limite de caracteres atingido")]
        [Display(Name = "Nome")]
        public string NomeJornaleiro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Favor insira um e-mail",ErrorMessageResourceName = "teste")]
        [StringLength(80, ErrorMessage = "Limite de caracteres atingido")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string EmailJornaleiro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Telefone")]
        public string TelefoneJornaleiro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Tamanho de senha inválido")]
        [Display(Name = "Senha")]
        public string SenhaJornaleiro { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        [Display(Name = "Permissão")]
        public int TipoId { get; set; }
        public Tipo tipo { get; set; }
        public ICollection<Venda> Vendas { get; set; }

    }
}
