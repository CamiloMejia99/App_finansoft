using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    public partial class TerceroDTO
    {

        [StringLength(100)]
        public string NOMBRE { get; set; }

        [DisplayName("Nom.Comercial")]
        [StringLength(100)]
        //[Required]
        public string NombreComercial { get; set; }


        //[HiddenInput(DisplayValue = false)] //ocultar
        [DisplayName("Tipo Documento")]
        [StringLength(2)]
        [Required]
        public string CLASEID { get; set; }

        public List<SelectListItem> _clasesID { get; set; }


        [Key]
        [DisplayName("Número Documento")]
        [StringLength(15)]
        //[Placeholder("")]
        public string NIT { get; set; }



        [DisplayName("D.V")]
        [StringLength(1)]
        public string DIGVER { get; set; }

        [StringLength(255)]
        [Required]
        public string PAISDOC { get; set; }

        [StringLength(255)]
        [Required]
        public string DEPTODOC { get; set; }

        [Required]
        [StringLength(255)]
        public string LUGAREXP { get; set; }

        public List<SelectListItem> _ciudades { get; set; }

        [Required]
        [DisplayName("Fecha Expedicion")]
        public DateTime FECHAEXP { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE1 { get; set; }

        //[HiddenInput(DisplayValue = false)] //ocultar
        [StringLength(255)]
        public string NOMBRE2 { get; set; }

        //[HiddenInput(DisplayValue = false)] //ocultar
        [StringLength(100)]
        [Required]
        public string APELLIDO1 { get; set; }

        //[HiddenInput(DisplayValue = false)] //ocultar
        [StringLength(255)]
        public string APELLIDO2 { get; set; }

        [StringLength(255)]
        [Required]
        public string PAISNAC { get; set; }

        [StringLength(255)]
        [Required]
        public string DEPTONAC { get; set; }

        [Required]
        [StringLength(255)]
        public string NACIO { get; set; }

        [Required]
        [DisplayName("Fecha Nacimiento")]
        public DateTime FECHANAC { get; set; }

        [Required]
        [StringLength(255)]
        public string ESTADOCIVIL { get; set; }

        [Required]
        [StringLength(255)]
        public string SEXO { get; set; }

        public string Categoria { get; set; }


        [StringLength(255)]
        public string CODIGO { get; set; }

        public string NIVEL { get; set; }
        public int CARGO { get; set; }

        /// <summary>

        /// </summary>

        public bool EsPERJURIDICA { get; set; }

        #region DatosPersonales




        [StringLength(255)]
        public string REPLEGAL { get; set; }




        #endregion

        [StringLength(255)]
        public string INACTIVO { get; set; }
        #region Direccion
        [StringLength(255)]
        public string DIR { get; set; }

        [StringLength(255)]
        public string DIR2 { get; set; }

        [StringLength(255)]
        public string BARRIO { get; set; }

        [Phone]
        public string TEL { get; set; }

        [StringLength(255)]
        public string TELEXT { get; set; }


        [Phone]
        public string TELMOVIL { get; set; }

        [StringLength(255)]
        public string FAX { get; set; }

        [StringLength(255)]
        [Required]
        public string PAIS { get; set; }
        public List<SelectListItem> _paises { get; set; }


        public TerceroDTO()
        {
            _departamentos = GetDepartamentos();
            _ciudades = GetCiudades();
            //_regimen = GetRegimen();
            _regimen = new List<SelectListItem>();
        }

        [StringLength(255)]
        [Required]
        public string DEPTO { get; set; }
        public List<SelectListItem> _departamentos { get; set; }

        private List<SelectListItem> GetDepartamentos()
        {
            var listadptos = new List<SelectListItem>();
            listadptos.Add(new SelectListItem() { Text = "Dpto 1" });
            listadptos.Add(new SelectListItem() { Text = "Dpto 2" });
            listadptos.Add(new SelectListItem() { Text = "Dpto 3" });
            listadptos.Add(new SelectListItem() { Text = "Dpto 4" });

            return listadptos;


        }


        [StringLength(255)]
        public string MUNICIPIO { get; set; }



        private List<SelectListItem> GetCiudades()
        {
            var listaciudades = new List<SelectListItem>();
            listaciudades.Add(new SelectListItem() { Text = "Ciudad 1" });
            listaciudades.Add(new SelectListItem() { Text = "Ciudad 2" });
            listaciudades.Add(new SelectListItem() { Text = "Ciudad 3" });
            listaciudades.Add(new SelectListItem() { Text = "Ciudad 4" });

            return listaciudades;
        }

        [StringLength(255)]
        [EmailAddress]
        public string EMAIL { get; set; }
        #endregion

        [Required]
        [StringLength(255)]
        public string PROFESION { get; set; }
        public List<SelectListItem> _profesiones { get; set; }


        [StringLength(255)]
        public string SALARIO { get; set; }


        [StringLength(255)]
        public string EMPRESA { get; set; }
        public List<SelectListItem> _empresas { get; set; }

        public string AGENCIA { get; set; }


        [StringLength(255)]
        public string DEPENDENCIA { get; set; }
        public List<SelectListItem> _dependencias { get; set; }

        [Required]
        [StringLength(255)]
        public string VIVIENDA { get; set; }
        public List<SelectListItem> _tiposvivienda { get; set; }
        public List<SelectListItem> _sexo;
        public List<SelectListItem> _estadocivil;

        #region Es
        // [StringLength(255)]
        public Nullable<bool> EsCLIENTE { get; set; }

        public Nullable<bool> EsPROVEEDOR { get; set; }

        public Nullable<bool> ESPATRONAL { get; set; }

        public Nullable<bool> EsVENDEDOR { get; set; }

        public Nullable<bool> EsCOBRADOR { get; set; }

        //public Nullable<bool> ESCODEUDOR { get; set; }

        //public Nullable<bool> ESEMPLEADO { get; set; }

        //public Nullable<bool> ESCOMISION { get; set; }

        //public Nullable<bool> ESTRANSPOR { get; set; }

        //public Nullable<bool> ESVEHICULO { get; set; }

        //public Nullable<bool> ESBANCO { get; set; }

        //public Nullable<bool> ESOFICIAL { get; set; }

        //public Nullable<bool> ESUNIOFI { get; set; }

        //public Nullable<bool> ESASEGURADORA { get; set; }


        #region esto lo debe montar asociado:Tercero
        public int ESASOCIADO { get; set; }

        //public Nullable<bool> EXASOCIADO { get; set; }

        //public Nullable<bool> ESBENEFICIARIO { get; set; }

        #endregion


        ////[StringLength(255)]
        ////public string EsVENDEDOR { get; set; }

        #endregion

        #region Ventas

        //[StringLength(255)]
        //public string PROPIETA { get; set; }

        //[StringLength(255)]
        //public string AGENTE { get; set; }

        //[StringLength(255)]
        //public string BANCO { get; set; }

        //[StringLength(255)]
        //public string GRUPO { get; set; }

        //[StringLength(255)]
        //public string SUBGRUPO { get; set; }

        //[StringLength(255)]
        //public string CLASETER { get; set; }

        //[StringLength(255)]
        //public string ZONA { get; set; }

        //public double? CUPO { get; set; }

        //public double? CUPO2 { get; set; }

        //[StringLength(255)]
        //public string CALIFICA { get; set; } 
        #endregion

        [StringLength(255)]
        public string REGIMEN { get; set; }
        public List<SelectListItem> _regimen { get; set; }

        private List<SelectListItem> GetRegimen()
        {
            var listaregimen = new List<SelectListItem>();
            listaregimen.Add(new SelectListItem() { Text = "Regimen 1" });
            listaregimen.Add(new SelectListItem() { Text = "Regimen 2" });
            listaregimen.Add(new SelectListItem() { Text = "Regimen 3" });
            listaregimen.Add(new SelectListItem() { Text = "Regimen 4" });

            return listaregimen;


        }

#warning Eliminar de tercetros
        #region No va - revisar en terceros
        //[StringLength(255)]
        //public string RETEFTE { get; set; }

        //[StringLength(255)]
        //public string RETETODO { get; set; }

        //[StringLength(255)]
        //public string NORETECRE { get; set; }

        //[StringLength(255)]
        //public string GRANCONTR { get; set; }

        //[StringLength(255)]
        //public string AUTORETE { get; set; }

        //[StringLength(255)]
        //public string RETEICA { get; set; }

        //[StringLength(255)]
        //public string TARICA { get; set; }

        //[StringLength(255)]
        //public string NOIVA { get; set; }

        //[StringLength(255)]
        //public string ACTIECON { get; set; }

        //[StringLength(255)]
        //public string CODPUB { get; set; }

        //[StringLength(255)]
        //public string ENCARGADO { get; set; }


        //[StringLength(255)]
        //public string PRECIO { get; set; }

        //[StringLength(255)]
        //public string FPAGO { get; set; }

        //[StringLength(255)]
        //public string CONDPAGO { get; set; }

        //[StringLength(255)]
        //public string NODATACRED { get; set; }

        //[StringLength(255)]
        //public string PLAZOMAX { get; set; }

        //[StringLength(255)]
        //public string PLAZO { get; set; }

        //[StringLength(255)]
        //public string PLAZO2 { get; set; }

        //[StringLength(255)]
        //public string PLAZO3 { get; set; }

        //[StringLength(255)]
        //public string PDTOCLI { get; set; }

        //[StringLength(255)]
        //public string PDTOCLI2 { get; set; }

        //[StringLength(255)]
        //public string PDTOCLI3 { get; set; }

        //[StringLength(255)]
        //public string TDTOCLI { get; set; }

        //[StringLength(255)]
        //public string TDTOCLI2 { get; set; }

        //[StringLength(255)]
        //public string TDTOCLI3 { get; set; }

        //[StringLength(255)]
        //public string PDTOCOND { get; set; }

        //[StringLength(255)]
        //public string PDTOCOND2 { get; set; }

        //[StringLength(255)]
        //public string PDTOCOND3 { get; set; }

        //[StringLength(255)]
        //public string CUENTAB { get; set; }

        //[StringLength(255)]
        //public string CUENTABAC { get; set; }

        //[StringLength(255)]
        //public string CODSOCIAL { get; set; }

        //[StringLength(255)]
        //public string TRECIPRO { get; set; } 
        #endregion


        [StringLength(255)]
        public string LATITUD { get; set; }

        [StringLength(255)]
        public string LONGITUD { get; set; }

        [StringLength(255)]
        public string USUARIOV { get; set; }

        [StringLength(255)]
        public string CONVENIOP { get; set; }

        #region Verificar
        //[StringLength(255)]
        //public string PORCAIU_A { get; set; }

        //[StringLength(255)]
        //public string PORCAIU_I { get; set; }

        //[StringLength(255)]
        //public string PORCAIU_U { get; set; }

        //[StringLength(255)]
        //public string NODESCTOS { get; set; }

        //public DateTime? ULTVENTA { get; set; }

        //[StringLength(255)]
        //public string PFINANCIA { get; set; }

        //[StringLength(255)]
        //public string MENSACOMP { get; set; } 
        #endregion

        [StringLength(255)]
        public string DECLARA { get; set; }

        [StringLength(255)]
        public string OBSERVA { get; set; }

        [StringLength(255)]
        public string FOTO { get; set; }

        public DateTime? FECHAR { get; set; }

        #region Auditoria

        [StringLength(255)]
        public string USUARIO { get; set; }

        [StringLength(255)]
        public string FUPDATEU { get; set; }

        public DateTime? FUPDATE { get; set; }
        #endregion
    }
}

