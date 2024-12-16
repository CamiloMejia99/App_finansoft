using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.MediosMagneticos
{
    [Table("med.ConfigMedMag")]
    public partial class ConfigMedMag
    {
        [Key]
        [Required]
        public int idConfigMedMag { get; set; }
        [Required]
        [ForeignKey("cuentmay")]
        public string CuentaContable { get; set; }
        [Required]
        [ForeignKey("format")]
        public int formato { get; set; }
        [Required]
        [ForeignKey("concept")]
        public int concepto { get; set; }
        [Required]
        [ForeignKey("categori")]
        public int categoria { get; set; }
        [Required]
        [ForeignKey("acumuladop")]
        public int acumuladopor { get; set; }
        [Required]
        public int anvigente { get; set; }

        public virtual formatos format { get; set; }
        public virtual conceptos concept { get; set; }
        public virtual categorias categori { get; set; }
        public virtual acumuladopor acumuladop { get; set; }
        public virtual CuentaMayor cuentmay { get; set; }

    }
   
}
