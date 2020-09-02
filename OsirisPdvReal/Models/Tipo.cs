using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OsirisPdvReal.Models
{
    public class Tipo
    {
        [Key]
        public int TipoId { get; set; }
        
        public String NomeTipo { get; set; }
        public ICollection<Jornaleiro> Jornaleiro { get; set; }

    }
}
