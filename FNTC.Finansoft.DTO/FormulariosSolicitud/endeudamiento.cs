using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.FormulariosSolicitud
{
    public class endeudamiento
    {

        public int id { get; set; }

        public int id_solicitud { get; set; }

        public int id_persona { get; set; }

        [Display(Name = "Entidad")]
        public string entidad { get; set; }

        [Display(Name = "Saldo deuda")]
        public float s_deuda { get; set; }

        [Display(Name = "Cuota Mensual")]
        public float cuota_mensual { get; set; }
    }
}
