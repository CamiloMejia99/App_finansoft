////using AutoMapper;
////using .Terceros;
//using .Terceros;
//using Lib.Web.Mvc.JQuery.JqGrid;
//using Lib.Web.Mvc.JQuery.JqGrid.DataAnnotations;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace FNTC.Finansoft.UI.Areas.Terceros.ViewModels.JqGrid
//{
////#error Subir Email direccion 
//    public partial class TerceroViewModel
//    {
//        [DisplayName("Identificacíon")]
//        [HiddenInput(DisplayValue = false)] //ocultar
//        [JqGridColumnEditable
//            (true, "TiposNits", "~/../../Params", EditType = JqGridColumnEditTypes.Select, EditHidden = true)]
//        [StringLength(2)]
//        [Required]
//        public string CLASEID { get; set; }

//        [DisplayName("Nit/CC")]
//        [JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 2, FormRowPosition = 1)]
//        [StringLength(20)]
//        [JqGridColumnLayout(Alignment = JqGridAlignments.Right, Width = 100)]
//        public string NIT { get; set; }

//        [JqGridColumnFormatter("$.DIGVER")] //aca debo llamar al verificador de nits
//        [JqGridColumnLayout(Alignment=JqGridAlignments.Center, Width = 35)]
//        [JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 3, FormRowPosition = 1,AutoSize=true)]
        
//        [DisplayName("D.V")]
//        [StringLength(2)]
//        public string DIGVER { get; set; }
     
//        [JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 1, FormRowPosition = 2)]
//        // [HiddenInput(DisplayValue = false)] //ocultar
//        [HiddenInput(DisplayValue = true)]
//        public string NOMBRE1 { get; set; }
  
//        [JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 2, FormRowPosition = 2)]
//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string NOMBRE2 { get; set; }

//        //#error aca muestra en el form de edit
//      //  [JqGridColumnEditable(true, EditHidden = true)]
//        [JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 1, FormRowPosition = 3)]
//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string APELLIDO1 { get; set; }

//        [JqGridColumnEditable(true, EditHidden = true, FormColumnPosition = 2, FormRowPosition = 3)]
//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string APELLIDO2 { get; set; }


//        [StringLength(255)]
//        [JqGridColumnEditable(false)]
//        public string NOMBRE { get; set; }

//        [DisplayName("Nom.Comercial")]
//        [StringLength(255)]
//        [JqGridColumnEditable(true)]
//        public string NombreComercial { get; set; }


//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string CODIGO { get; set; }

//        /// <summary>
//        /// si es persona juridica uestra p.juridica sino p.natural
//        /// </summary>
//        [HiddenInput(DisplayValue = false)] //ocultar
//        public Nullable<bool> EsPERJURIDICA { get; set; }

//        #region DatosPersonales



//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string REPLEGAL { get; set; }

//        #endregion
//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string INACTIVO { get; set; }
//        #region Direccion

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string DIR { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string DIR2 { get; set; }

//        [DisplayName("Telefono")]
//        [JqGridColumnEditable(true)]
//        [StringLength(255)]
//        public string TEL { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string TELEXT { get; set; }

//        [DisplayName("Celular")]
//        [JqGridColumnEditable(true)]
//        [StringLength(255)]
//        public string TELMOVIL { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string FAX { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string PAIS { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string DEPTO { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string CIUDAD { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string BARRIO { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        [StringLength(255)]
//        public string EMAIL { get; set; }
//        #endregion

//        #region Es
//        // [StringLength(255)]
//        [HiddenInput(DisplayValue = false)] //ocultar
//        public Nullable<bool> EsCLIENTE { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        public Nullable<bool> EsPROVEEDOR { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        public Nullable<bool> ESPATRONAL { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        public Nullable<bool> EsVENDEDOR { get; set; }

//        [HiddenInput(DisplayValue = false)] //ocultar
//        public Nullable<bool> EsCOBRADOR { get; set; }

//        //public Nullable<bool> ESCODEUDOR { get; set; }

//        //public Nullable<bool> ESEMPLEADO { get; set; }

//        //public Nullable<bool> ESCOMISION { get; set; }

//        //public Nullable<bool> ESTRANSPOR { get; set; }

//        //public Nullable<bool> ESVEHICULO { get; set; }

//        //public Nullable<bool> ESBANCO { get; set; }

//        //public Nullable<bool> ESOFICIAL { get; set; }

//        //public Nullable<bool> ESUNIOFI { get; set; }

//        //public Nullable<bool> ESASEGURADORA { get; set; }


//        #region esto lo debe montar asociado:Tercero
//        //public Nullable<bool> ESASOCIADO { get; set; }

//        //public Nullable<bool> EXASOCIADO { get; set; }

//        //public Nullable<bool> ESBENEFICIARIO { get; set; }

//        #endregion


//        ////[StringLength(255)]
//        ////public string EsVENDEDOR { get; set; }

//        #endregion

//        #region Ventas

//        //[StringLength(255)]
//        //public string PROPIETA { get; set; }

//        //[StringLength(255)]
//        //public string AGENTE { get; set; }

//        //[StringLength(255)]
//        //public string BANCO { get; set; }

//        //[StringLength(255)]
//        //public string GRUPO { get; set; }

//        //[StringLength(255)]
//        //public string SUBGRUPO { get; set; }

//        //[StringLength(255)]
//        //public string CLASETER { get; set; }

//        //[StringLength(255)]
//        //public string ZONA { get; set; }

//        //public double? CUPO { get; set; }

//        //public double? CUPO2 { get; set; }

//        //[StringLength(255)]
//        //public string CALIFICA { get; set; } 
//        #endregion


//        [StringLength(255)]
//        [JqGridColumnEditable
//            (true, "TiposRegimenes", "~/../../Params", EditType = JqGridColumnEditTypes.Select, EditHidden = true)]
//        public string REGIMEN { get; set; }
//#warning Eliminar de tercetros
//        #region No va - revisar en terceros
//        //[StringLength(255)]
//        //public string RETEFTE { get; set; }

//        //[StringLength(255)]
//        //public string RETETODO { get; set; }

//        //[StringLength(255)]
//        //public string NORETECRE { get; set; }

//        //[StringLength(255)]
//        //public string GRANCONTR { get; set; }

//        //[StringLength(255)]
//        //public string AUTORETE { get; set; }

//        //[StringLength(255)]
//        //public string RETEICA { get; set; }

//        //[StringLength(255)]
//        //public string TARICA { get; set; }

//        //[StringLength(255)]
//        //public string NOIVA { get; set; }

//        //[StringLength(255)]
//        //public string ACTIECON { get; set; }

//        //[StringLength(255)]
//        //public string CODPUB { get; set; }

//        //[StringLength(255)]
//        //public string ENCARGADO { get; set; }

//        //public DateTime? NACIO { get; set; }

//        //[StringLength(255)]
//        //public string PRECIO { get; set; }

//        //[StringLength(255)]
//        //public string FPAGO { get; set; }

//        //[StringLength(255)]
//        //public string CONDPAGO { get; set; }

//        //[StringLength(255)]
//        //public string NODATACRED { get; set; }

//        //[StringLength(255)]
//        //public string PLAZOMAX { get; set; }

//        //[StringLength(255)]
//        //public string PLAZO { get; set; }

//        //[StringLength(255)]
//        //public string PLAZO2 { get; set; }

//        //[StringLength(255)]
//        //public string PLAZO3 { get; set; }

//        //[StringLength(255)]
//        //public string PDTOCLI { get; set; }

//        //[StringLength(255)]
//        //public string PDTOCLI2 { get; set; }

//        //[StringLength(255)]
//        //public string PDTOCLI3 { get; set; }

//        //[StringLength(255)]
//        //public string TDTOCLI { get; set; }

//        //[StringLength(255)]
//        //public string TDTOCLI2 { get; set; }

//        //[StringLength(255)]
//        //public string TDTOCLI3 { get; set; }

//        //[StringLength(255)]
//        //public string PDTOCOND { get; set; }

//        //[StringLength(255)]
//        //public string PDTOCOND2 { get; set; }

//        //[StringLength(255)]
//        //public string PDTOCOND3 { get; set; }

//        //[StringLength(255)]
//        //public string CUENTAB { get; set; }

//        //[StringLength(255)]
//        //public string CUENTABAC { get; set; }

//        //[StringLength(255)]
//        //public string CODSOCIAL { get; set; }

//        //[StringLength(255)]
//        //public string TRECIPRO { get; set; } 
//        #endregion


//        //[StringLength(255)]
//        //public string LATITUD { get; set; }

//        //[StringLength(255)]
//        //public string LONGITUD { get; set; }

//        //[StringLength(255)]
//        //public string USUARIOV { get; set; }

//        //[StringLength(255)]
//        //public string CONVENIOP { get; set; }

//        #region Verificar
//        //[StringLength(255)]
//        //public string PORCAIU_A { get; set; }

//        //[StringLength(255)]
//        //public string PORCAIU_I { get; set; }

//        //[StringLength(255)]
//        //public string PORCAIU_U { get; set; }

//        //[StringLength(255)]
//        //public string NODESCTOS { get; set; }

//        //public DateTime? ULTVENTA { get; set; }

//        //[StringLength(255)]
//        //public string PFINANCIA { get; set; }

//        //[StringLength(255)]
//        //public string MENSACOMP { get; set; } 
//        #endregion

//        //[StringLength(255)]
//        //public string DECLARA { get; set; }

//        //[StringLength(255)]
//        //public string OBSERVA { get; set; }

//        //[StringLength(255)]
//        //public string FOTO { get; set; }

//        //public DateTime? FECHAR { get; set; }

//        #region Auditoria

//        //[StringLength(255)]
//        //public string USUARIO { get; set; }

//        //[StringLength(255)]
//        //public string FUPDATEU { get; set; }

//        //public DateTime? FUPDATE { get; set; }
//        #endregion
//    }

//}