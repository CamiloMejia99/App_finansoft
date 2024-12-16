namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    using FNTC.Finansoft.Accounting.DTO.Terceros;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("acc.Movimientos")]
    public partial class Movimiento
    {
        [Key]
        public int ID { get; set; }


        public Comprobante Comprobante { get; set; }

        [ForeignKey("Comprobante")]
        [StringLength(255)]
        [Column(Order = 1)]
        public string TIPO { get; set; }

        [ForeignKey("Comprobante")]
        [StringLength(15)]
        [Column(Order = 2)]
        public string NUMERO { get; set; }

        [StringLength(10)]
        [ForeignKey("cuentaFK")]
        public string CUENTA { get; set; }

        [StringLength(20)]
        [ForeignKey("terceroFK")]
        public string TERCERO { get; set; }

        [StringLength(255)]
        public string DETALLE { get; set; }

        [NotMapped]
        public string cuentaPagare { get; set; }

        public decimal DEBITO { get; set; }

        public decimal CREDITO { get; set; }

        public decimal BASE { get; set; }

        [StringLength(3)]
        public string CCOSTO { get; set; }

        public DateTime FECHAMOVIMIENTO { get; set; }

        public bool ANULADO { get; set; }

        //Cpalacios
        //Agregar nuevo campo
        [StringLength(4)]
        public string DOCUMENTO { get; set; }
        //[StringLength(3)]
        //public string SCCOSTO { get; set; }

        //[StringLength(3)]
        //public string DOCRELA { get; set; }



        #region Pendientes

        //public decimal BASE2 { get; set; }

        //[StringLength(255)]
        //public string CUOTA { get; set; }

        //[StringLength(255)]
        //public string ARTRELA { get; set; }

        //[StringLength(255)]
        //public string ARTCANT { get; set; }

        //[StringLength(255)]
        //public string BODEGA { get; set; }

        //[StringLength(255)]
        //public string LIBRO { get; set; }

        //[StringLength(255)]
        //public string ITEM { get; set; }

        //[StringLength(255)]
        //public string AUTO { get; set; }

        //[StringLength(255)]
        //public string CONCILIA { get; set; }

        //[StringLength(255)]
        //public string REF1 { get; set; }

        //[StringLength(255)]
        //public string REF2 { get; set; }

        //[StringLength(255)]
        //public string REF3 { get; set; }

        //[StringLength(255)]
        //public string REF4 { get; set; }

        //[StringLength(255)]
        //public string REFDESTINO { get; set; }

        //[StringLength(255)]
        //public string NOIMPRIMA { get; set; } 
        #endregion



        public virtual Tercero terceroFK { get; set; }
        public virtual CuentaMayor cuentaFK { get; set; }

    }
}
