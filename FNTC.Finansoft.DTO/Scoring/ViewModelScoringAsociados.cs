using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    public class ViewModelScoringAsociados
    {

        public int id { get; set; }



        [Display(Name = "NIT")]
        [Remote("ValidacionNIT", "TERCEROS", ErrorMessage = "Ingrese nit")]
        [Required]
        public string NIT { get; set; }
        //public virtual Terceros Terceros { get; set; }

        [Display(Name = "NOMBRE")]
        [Required]
        public string NOMBRE { get; set; }

        [Display(Name = "SALARIO")]
        [Required]
        public string SALARIO { get; set; }

        [Display(Name = "ESTADO CIVIL")]
        [Required]
        public string ESTADOCIVIL { get; set; }

        [Display(Name = "SEXO")]
        [Required]
        public string SEXO { get; set; }

        [Display(Name = "FECHA NACIMIENTO")]
        [Required]
        public DateTime FECHANAC { get; set; }
    }
}
