using Lib.Web.Mvc.JQuery.JqGrid;
using Lib.Web.Mvc.JQuery.JqGrid.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.Accounting
{

    //Al Crear una cuenta
#warning Debe solicitar si es cuenta depreciacion, cuenta de impuesto
#warning las cuentas de depreciacion solo operan en Activos Fijos
    /*
     si es una cuenta de impuesto me pide base, tarifa(%), tipo = retefuente() - reteica - si es iva, retecre
     * 
     * 
     */
    public partial class CuentaDTO
    {


        [DisplayName("CODIGO")]
        [StringLength(9, MinimumLength = 9)]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Left, Width = 10)]
        [JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 1, FormRowPosition = 1)]
        public string CODIGO { get; set; }


        [DisplayName("Nombre")]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Right, Width = 20)]
        [JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 2, FormRowPosition = 1)]
        [Required]
        public string NOMBRE { get; set; }

        // public string NIIF { get; set; }

        [DisplayName("Naturaleza")]
        // [HiddenInput(DisplayValue = true)] //ocultar
        [JqGridColumnEditable
            (true, "TiposNits", "~/../../Params", EditType = JqGridColumnEditTypes.Select, EditHidden = true
            , FormColumnPosition = 1, FormRowPosition = 2)]
        [Required]
        public string NAT { get; set; }


        [JqGridColumnEditable
         (true, EditType = JqGridColumnEditTypes.CheckBox, EditHidden = true
         , FormColumnPosition = 1, FormRowPosition = 3)]
        [DisplayName("Requiere Tercero")]
        [HiddenInput(DisplayValue = false)] //ocultar de la gri
        public string REQTER { get; set; }


        [JqGridColumnEditable
          (true, EditType = JqGridColumnEditTypes.CheckBox, EditHidden = true
          , FormColumnPosition = 1, FormRowPosition = 4)]
        [DisplayName("Requiere Centro de Costo")]
        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string REQCCO { get; set; }


        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string REQDOCR { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string REQARTR { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string PORINC { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string RECIPROCA { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string CORRIENTE { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string VALIDASALD { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string INACTIVO { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string CODPUB { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string NOVALDOCR { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string PARAMESP { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string CTANIIF { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string VALIDLIB { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string CTAREMPL { get; set; }

        [HiddenInput(DisplayValue = false)] //ocultar de la grilla
        public string FLUJOEFEC { get; set; }


        //[DisplayName("Identificacíon")]
        //[HiddenInput(DisplayValue = false)] //ocultar
        //[JqGridColumnEditable
        //    (true, "TiposNits", "~/../../Params", EditType = JqGridColumnEditTypes.Select, EditHidden = true)]
        //[StringLength(2)]
        //[Required]
        //public string CLASEID { get; set; }

        //[DisplayName("Nit/CC")]
        //[JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 2, FormRowPosition = 1)]
        //[StringLength(20)]
        //public string NIT { get; set; }








        //[StringLength(255)]
        //[JqGridColumnEditable(false)]
        //public string NOMBRE { get; set; }




        //[HiddenInput(DisplayValue = false)] //ocultar
        //[StringLength(255)]
        //public string CODIGO { get; set; }

        /// <summary>
        /// si es persona juridica uestra p.juridica sino p.natural
        /// </summary>


        #region DatosPersonales



        [HiddenInput(DisplayValue = false)] //ocultar
        [StringLength(255)]
        public string REPLEGAL { get; set; }

        #endregion
        //[HiddenInput(DisplayValue = false)] //ocultar
        //[StringLength(255)]
        //public string INACTIVO { get; set; }
    }
}
