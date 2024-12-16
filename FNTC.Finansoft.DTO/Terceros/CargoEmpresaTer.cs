using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.CargoEmpresa")]
    public class CargoEmpresaTer
    {
        [Key]
        [Required]
        public int IDCargo { get; set; }
        [Required]
        [Display(Name = "Nombre Cargo")]
        public string NombreCargo { get; set; }
        [Required]
        [Display(Name = "Estado Cargo")]
        public bool EstadoCargo { get; set; }
    }
} 
