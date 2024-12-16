using FNTC.Finansoft.Accounting.DTO.Terceros;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FormulariosSolicitud
{
    [Table("dbo.solicitudCredito")]
    public class solicitudCredito
    {
        [Key]
        [Required]
        public int id_solicitud { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public string fecha { get; set; }

        [StringLength(2)]
        [Required]
        [Display(Name = "¿Tiene vivienda propia?")]
        public string tieneViviendaPropia { get; set; }


        [StringLength(2)]
        [Display(Name = "¿Es vivinda familiar?")]
        public string viviendaFamiliar { get; set; }

        [StringLength(2)]
        [Display(Name = "¿Vivienda hipotecada?")]
        public string viviendaHipotecada { get; set; }

        [Display(Name = "Valor comercial")]
        public string valComercial { get; set; }

        [Required]
        [StringLength(20)]
        [ForeignKey("Terceros")]
        [Display(Name = "N° Documento")]
        public string id_persona { get; set; }

        [Required]
        [Display(Name = "Destino")]
        public int id_destino { get; set; }

        [Required]
        [Display(Name = "linea")]
        public int id_linea { get; set; }

        [Required]
        [Display(Name = "Valor solicitado")]
        public string val_solicitud { get; set; }

        [Required]
        [Display(Name = "Plazo")]
        public decimal plazo { get; set; }

        [Required]
        [Display(Name = "Cuota")]
        public string cuota { get; set; }

        [StringLength(2)]
        [Display(Name = "¿Cancela crédito anterior?")]
        public string cancelaCredito { get; set; }


        [StringLength(20)]
        [Display(Name = "N° Documento")]
        public string id_codeudor { get; set; }

        [StringLength(50)]
        public string pagare { get; set; }

        [StringLength(20)]
        public string id_codeudor2 { get; set; }


        [Required]
        [StringLength(50)]
        public string empresa { get; set; }

        [Required]
        public string fechaIngresoEmpresa { get; set; }

        [Required]
        [StringLength(50)]
        public string salarioEmpresa { get; set; }

        [StringLength(50)]
        public string cod1empresa { get; set; }


        public string cod1fechaIngresoEmpresa { get; set; }

        [StringLength(50)]
        public string cod2empresa { get; set; }

        [StringLength(250)]
        public string asesorcomercial { get; set; }

        public string cod2fechaIngresoEmpresa { get; set; }

        //referencias familiares.....

        [StringLength(250)]
        [Required]
        public string nomFam1 { get; set; }

        [StringLength(250)]
        public string nomFam2 { get; set; }

        [StringLength(100)]
        [Required]
        public string ocuFam1 { get; set; }

        [StringLength(100)]
        public string ocuFam2 { get; set; }

        [StringLength(100)]
        [Required]
        public string dirFam1 { get; set; }

        [StringLength(100)]
        public string dirFam2 { get; set; }

        [StringLength(20)]
        [Required]
        public string celFam1 { get; set; }

        [StringLength(20)]
        public string celFam2 { get; set; }
        //.............................

        //referencias personales.....

        [StringLength(250)]
        [Required]
        public string nomPer1 { get; set; }

        [StringLength(250)]
        public string nomPer2 { get; set; }

        [StringLength(100)]
        [Required]
        public string ocuPer1 { get; set; }

        [StringLength(100)]
        public string ocuPer2 { get; set; }

        [StringLength(100)]
        [Required]
        public string dirPer1 { get; set; }

        [StringLength(100)]
        public string dirPer2 { get; set; }

        [StringLength(20)]
        [Required]
        public string celPer1 { get; set; }

        [StringLength(20)]
        public string celPer2 { get; set; }
        //.............................
        [Required]
        [StringLength(50)]
        public string estCivil { get; set; }


        public int perCargo { get; set; }


        public int estrato { get; set; }


        public int timeResidencia { get; set; }

        [StringLength(250)]
        public string nomConyu { get; set; }

        [StringLength(250)]
        public string apeConyu { get; set; }

        [StringLength(250)]
        public string cedConyu { get; set; }

        [StringLength(250)]
        public string empConyu { get; set; }

        [StringLength(250)]
        public string celConyu { get; set; }

        //....................info financiera................
        [Required]
        [StringLength(250)]
        public string salPri { get; set; }


        [StringLength(250)]
        public string otrIng { get; set; }

        [Required]
        [StringLength(250)]
        public string comision { get; set; }


        [StringLength(250)]
        public string otrIngCon { get; set; }


        public string detOrig { get; set; }

        [Required]
        [StringLength(250)]
        public string valArriendo { get; set; }

        [Required]
        [StringLength(250)]
        public string valSoste { get; set; }
        [Required]
        [StringLength(250)]
        public string valFin { get; set; }

        [Required]
        [StringLength(250)]
        public string otrGasto { get; set; }


        public bool casa { get; set; }

        public bool apto { get; set; }

        public bool finca { get; set; }

        public bool otro { get; set; }

        [StringLength(250)]
        public string cual { get; set; }

        [StringLength(250)]
        public string dirProp { get; set; }

        [StringLength(250)]
        public string ciuProp { get; set; }

        [StringLength(250)]
        public string escritura { get; set; }

        [StringLength(250)]
        public string notFec { get; set; }

        [StringLength(250)]
        public string nMatInm { get; set; }

        [StringLength(250)]
        public string valComProp { get; set; }

        [StringLength(250)]
        public string valHipProp { get; set; }

        [StringLength(250)]
        public string modelo { get; set; }

        [StringLength(250)]
        public string valVehiculo { get; set; }

        [StringLength(250)]
        public string valDeuVehiculo { get; set; }

        [Required]
        [StringLength(50)]
        public string fechVenCon { get; set; }


        public int tiemLaborado { get; set; }

        [Required]
        [StringLength(250)]
        public string ParentescoFam1 { get; set; }

        [StringLength(250)]
        public string ParentescoFam2 { get; set; }

        [Required]
        [StringLength(250)]
        public string CiudadFam1 { get; set; }

        [StringLength(250)]
        public string CiudadFam2 { get; set; }

        [Required]
        [StringLength(250)]
        public string ParentescoPer1 { get; set; }


        [StringLength(250)]
        public string ParentescoPer2 { get; set; }

        [Required]
        [StringLength(250)]
        public string CiudadPer1 { get; set; }

        [StringLength(250)]
        public string CiudadPer2 { get; set; }

        [StringLength(50)]
        public string EstadoCivilCod1 { get; set; }

        [StringLength(50)]
        public string EstadoCivilCod2 { get; set; }

        [Required]
        [StringLength(250)]
        public string TotalIngresos { get; set; }

        [Required]
        [StringLength(250)]
        public string TotalGastos { get; set; }


        [StringLength(250)]
        public string ENDentidad1 { get; set; } //END para campos de endeudamiento


        [StringLength(250)]
        public string ENDentidad2 { get; set; }


        [StringLength(50)]
        public string ENDsaldoDeuda1 { get; set; }


        [StringLength(50)]
        public string ENDsaldoDeuda2 { get; set; }


        [StringLength(50)]
        public string ENDcuotaMensual1 { get; set; }


        [StringLength(50)]
        public string ENDcuotaMensual2 { get; set; }


        public virtual Tercero Terceros { get; set; }


    }
}
