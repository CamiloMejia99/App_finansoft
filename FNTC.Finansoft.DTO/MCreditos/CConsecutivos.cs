using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class CConsecutivos
    {
        public int id { get; set; }

        [Display(Name = "Agencia")]
        [Required]
        public int idAgencia { get; set; }

        [Display(Name = "Codificacion Pagare")]
        [Required]
        public string tipoCodPagare { get; set; }

        [Display(Name = "Consecutivo Pagare")]
        [Required]
        public int consecutivoPagareActual { get; set; }

        [Display(Name = "Codificacion Libranza")]
        [Required]
        public string tipoCodLibranza { get; set; }

        [Display(Name = "Consecutivo Libranza")]
        [Required]
        public int consecutivoLibranzaActual { get; set; }

        [Display(Name = "Linea")]
        [Required]
        public int idLinea { get; set; }

        [Display(Name = "Destino")]
        [Remote("ValidacionDestinoPagare", "CConsecutivos", ErrorMessage = "Ya se ha asignado un pagaré a este destino")]
        [Required]
        public int idDestino { get; set; }
        public int prestamo { get; set; }

        [Display(Name = "Activo?")]
        [Required]
        public bool estado { get; set; }
    }
}
