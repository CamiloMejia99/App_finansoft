namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
    using Geo;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ter.Terceros")]
    public partial class Tercero
    {
        [Key]
        [StringLength(20)]
        public string NIT { get; set; }

        [StringLength(1)]
        public string DIGVER { get; set; }

        [StringLength(2)]
        [Required]
        public string CLASEID { get; set; }

        [StringLength(255)]
        public string CODIGO { get; set; }

        public string NIVEL { get; set; }
        public int CARGO { get; set; }

        public Nullable<bool> EsPERJURIDICA { get; set; }

        #region DatosPersonales
        [StringLength(255)]
        [Required]
        public string NOMBRE { get; set; }

        [StringLength(255)]
        public string NombreComercial { get; set; }

        [StringLength(255)]
        public string REPLEGAL { get; set; }

        [StringLength(255)]

        public string NOMBRE1 { get; set; }

        [StringLength(255)]
        public string NOMBRE2 { get; set; }

        [StringLength(255)]
        public string APELLIDO1 { get; set; }

        [StringLength(255)]
        public string APELLIDO2 { get; set; }

        [StringLength(255)]
        public string ESTADOCIVIL { get; set; }

        [StringLength(255)]
        public string SEXO { get; set; }
        #endregion

        [ForeignKey("lugarNacimientoFK")]
        public Nullable<int> NACIO { get; set; }

        public DateTime? FECHANAC { get; set; }

        [ForeignKey("profesionFK")]
        public Nullable<int> PROFESION { get; set; }


        ////[ForeignKey("profesion")]
        //[ForeignKey("profesionFK")]
        //Profesion Prof { get; set; }

        public List<TerceroDependencia> TerceroDependencia { get; set; }

        public List<TerceroMunicipio> TerceroMunicipio { get; set; }

        public Nullable<int> SALARIO { get; set; }

        [ForeignKey("agenciaFK")]
        public Nullable<int> DEPENDENCIA { get; set; }

        [StringLength(255)]
        public string VIVIENDA { get; set; }

        public DateTime FECHAEXP { get; set; }

        [ForeignKey("lugarExpedFK")]
        public int LUGAREXP { get; set; }

        #region Direccion
        [StringLength(255)]
        public string DIR { get; set; }

        [StringLength(255)]
        public string DIR2 { get; set; }

        [StringLength(255)]
        public string TEL { get; set; }

        [StringLength(255)]
        public string TELEXT { get; set; }

        [StringLength(255)]
        public string TELMOVIL { get; set; }

        [StringLength(255)]
        public string FAX { get; set; }

        [ForeignKey("municipioFK")]
        public Nullable<int> MUNICIPIO { get; set; }

        [StringLength(255)]
        public string BARRIO { get; set; }

        [StringLength(255)]
        public string EMAIL { get; set; }
        #endregion


        #region Es
        // [StringLength(255)]
        public Nullable<bool> EsCLIENTE { get; set; }

        public Nullable<bool> EsPROVEEDOR { get; set; }

        public Nullable<bool> ESPATRONAL { get; set; }



        public Nullable<bool> ESCODEUDOR { get; set; }

        public Nullable<bool> ESEMPLEADO { get; set; }



        public Nullable<bool> ESBANCO { get; set; }

        public Nullable<bool> ESOFICIAL { get; set; }

        public Nullable<bool> ESUNIOFI { get; set; }

        public Nullable<bool> ESASEGURADORA { get; set; }


        #region esto lo debe montar asociado:Tercero
        public Nullable<int> ESASOCIADO { get; set; }

        //public Nullable<bool> EXASOCIADO { get; set; }

        //public Nullable<bool> ESBENEFICIARIO { get; set; }

        #endregion



        #endregion



        [StringLength(255)]
        public string REGIMEN { get; set; }

        [StringLength(255)]
        public string GRANCONTR { get; set; }



        [StringLength(255)]
        public string CIUU { get; set; }



        [StringLength(255)]
        public string ENCARGADO { get; set; }




        [StringLength(255)]
        public string OBSERVA { get; set; }

        [StringLength(255)]
        public string FOTO { get; set; }

        public DateTime? FECHAR { get; set; }

        [StringLength(255)]
        public string INACTIVO { get; set; }

        //public DateTime FECHAAFILIACION { get; set; }

        #region Auditoria

        [StringLength(255)]
        public string USUARIO { get; set; }

        [StringLength(255)]
        public string FUPDATEU { get; set; }

        public DateTime? FUPDATE { get; set; }
        #endregion


        #region Verificar

        #endregion        
        /*
         * 
         * 
        public virtual ICollection<FichasAhorros> FichasAhorros { get; set; }
        public virtual ICollection<FichasAportes> FichasAportes { get; set; }
        public virtual ICollection<CuadreCajaPorCajero> CuadreCajaPorCajeroes { get; set; }
        public virtual ICollection<FactOpcaja> FactOpcajas { get; set; }
        public virtual ICollection<FichasDescuentos> FichasDescuentos { get; set; }
        */


        public virtual Municipio municipioFK { get; set; }
        public virtual Municipio lugarExpedFK { get; set; }
        public virtual Municipio lugarNacimientoFK { get; set; }
        public virtual Profesion profesionFK { get; set; }
        public virtual agencias agenciaFK { get; set; }
    }
}
