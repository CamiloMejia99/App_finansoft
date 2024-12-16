namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("nom.DescuentosNominaConsolidadosNominas")]

    public class ConsolidadoNomina
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("DescuentosNominaPlanoEmpresas")]
        [Required]
        public int EMPRESA { get; set; }

        [StringLength(50)]
        public string NITEMPRESA { get; set; }
        [StringLength(10)]
        public string DigitoVerificacion { get; set; }
        [StringLength(10)]
        public string TipoDeEstado { get; set; }
        [StringLength(255)]
        public string PERIODO { get; set; }
        [StringLength(255)]
        public string GENERAR { get; set; }

        [StringLength(255)]
        public string idPersona { get; set; }

        [StringLength(255)]
        public string NombreCompleto { get; set; }

        [StringLength(255)]
        public string NOMBRE1 { get; set; }

        [StringLength(255)]
        public string NOMBRE2 { get; set; }

        [StringLength(255)]
        public string APELLIDO1 { get; set; }


        [StringLength(255)]
        public string APELLIDO2 { get; set; }

        [StringLength(255)]
        public string NumeroCuenta { get; set; }

        [StringLength(255)]
        public string totalAportes { get; set; }

        [ForeignKey("agencias")]
        [Required]
        public int DEPENDENCIA { get; set; }

        public int clase_plano { get; set; }

        [StringLength(255)]
        public string FechaApertura { get; set; }

        [StringLength(255)]
        public string FECHADISCRIMINACION { get; set; }


        public virtual PlanoEmpresa DescuentosNominaPlanoEmpresas { get; set; }
        public virtual agencias agencias { get; set; }

    }
}
