using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.Solicitudes")]
    public class FCSolicitudes
    {
        [Key]
        public int idSolicitud { get; set; }
        public int idAsociado { get; set; }
        public int IdVerificacionAso { get; set; }
        [Display(Name = "Fecha Registro")]
        public DateTime fechaRegistro { get; set; }
        [Display(Name = "Estado")]
        public string estado { get; set; }
        [Required]
        [ForeignKey("Prestamos")]
        public int idPrestamo { get; set; }
        [Required]
        [ForeignKey("ActividadesAsociado")]
        public int idActividad { get; set; }
        [Required]
        [ForeignKey("Dependencias")]
        public int idDependencia { get; set; }
        [Required]
        [ForeignKey("Sedes")]
        public int idSede { get; set; }
        [Required]
        [ForeignKey("CentralRiesgo")]
        public int idCentralDeRiesgo { get; set; }
        [Required]
        [ForeignKey("Configuracion")]
        public int idPrioridad { get; set; }

        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Salario { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal OtrosIngresos { get; set; }
        public string Descripcion { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Renta { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Prestamo { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Transporte { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Alimentacion { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal RDO { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Servicios { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal IntPM { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal OtrosGastos { get; set; }
        public string DescripcionG { get; set; }
        [Display(Name = "Estado")]
        public string PreEstado { get; set; }
        public string ComentariosAnalista { get; set; }
        public string ComentariosEnte { get; set; }
        public int Motivos { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Ventas { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Compras { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal IngresosNegocio { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal PrestamosCI { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Arriendo { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal SueldoPS { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal ServiciosPublicos { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal OtrosEgresos { get; set; }
        public string DescripcionEgresos { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal CajaBancos { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Inversiones { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal CuentasPorCobrar { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Mercancias { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal MueblesYEnseres { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Vehiculos { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal TerrenosYEdificios { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal OtrosActivos { get; set; }
        public string DescripcionActivos { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Proveedores { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal Obligaciones { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal OtrosPasivos { get; set; }


        public virtual Prestamos Prestamos { get; set; }
        public virtual FCActividades ActividadesAsociado { get; set; }
        public virtual CentralRiesgo CentralRiesgo { get; set; }
        public virtual FCConfiguracion Configuracion { get; set; }
        public virtual FCSedes Sedes { get; set; }
        public virtual FCDependencias Dependencias { get; set; }

    }
}
