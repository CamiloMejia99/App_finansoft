using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    public partial class DisNomina
    {
        [Key]
        public int ID { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Empresa")]
        public int EMPRESA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Periodo Deducción")]
        public string PERDEDUCC { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nro de Periodos")]
        public int PERIODO { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Generar")]
        public string GENERAR { get; set; }

        public List<SelectListItem> _empresas { get; set; }
    }
}
