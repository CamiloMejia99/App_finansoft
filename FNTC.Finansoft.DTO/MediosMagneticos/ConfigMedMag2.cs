using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.MediosMagneticos
{

    public partial class ConfigMedMag2
    {
        [Key]
        [Required]
        public int idConfigMedMag { get; set; }
        [Required]
        public int CuentaContable { get; set; }
        [Required]
        public int formato { get; set; }
        [Required]
        public string concepto { get; set; }
        [Required]
        public int categoria { get; set; }
        [Required]
        public int acumuladopor { get; set; }
        [Required]
        public int anvigente { get; set; }
        public string CuentaContablePlanCuenta { get; set; }

        public List<SelectListItem> CuentasContables { get; set; }

        public List<SelectListItem> Formatos { get; set; }
        public List<SelectListItem> Conceptos { get; set; }

    }

}