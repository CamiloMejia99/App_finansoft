using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;



namespace FNTC.Finansoft.Accounting.DTO.Accounting
{
    public class TipoComprobanteDTO
    {

        [Required]
        public string CODIGO { get; set; }

        [Required]
        public string CLASEComprobante { get; set; }
        public List<SelectListItem> _clasesComprobante { get; set; }

        public FormasDePagoDTO FormaPago { get; set; }
        public int? FormaPagoID { get; set; }

        //  public string FORMADePago { get; set; }
        public List<SelectListItem> _formaDePago { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }

        // [Required]
        [StringLength(255)]
        public string CONSECUTIVO { get; set; }

        public bool INACTIVO { get; set; }

        public string Owner { get; set; }



        public TipoComprobanteDTO()
        {
            _clasesComprobante = new List<SelectListItem>();
            _formaDePago = new List<SelectListItem>();
            FormaPago = new FormasDePagoDTO();

            CONSECUTIVO = "0";
        }






    }



}

