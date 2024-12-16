namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("acc.Comprobantes")]
    public partial class Comprobante
    {
        //la llave primaria debveria ser tipo + numero donde CLASE es FK de clases
        //[Key]
        //public int ID { get; set; }
        [Key]
        [StringLength(10)]
        [Column(Order = 1)]
        public string TIPO { get; set; }

        [Key]
        [StringLength(255)]
        [Column(Order = 2)]
        public string NUMERO { get; set; }

        [StringLength(4)]
        public string ANO { get; set; }

        [StringLength(2)]
        public string MES { get; set; }

        [StringLength(2)]
        public string DIA { get; set; }

        [StringLength(3)]
        public string CCOSTO { get; set; }


        public bool ANULADO { get; set; }

        public Nullable<bool> ELIMINADO { get; set; }

        [StringLength(255)]
        public string DETALLE { get; set; }

        [StringLength(20)]
        public string TERCERO { get; set; }

        //[StringLength(20)]
        //public string TERCERO2 { get; set; }

        [StringLength(255)]
        public string FPAGO { get; set; }

        [StringLength(255)]
        public string CTAFPAGO { get; set; }

        [StringLength(255)]
        public string NUMEXTERNO { get; set; }

        public decimal VRTOTAL { get; set; }


        public decimal SUMDBCR { get; set; }

        public DateTime FECHARealiz { get; set; }

        [StringLength(255)]
        public string MODIFICA { get; set; }




        //auditoria

        [StringLength(255)]
        public string EXPORTADO { get; set; }

        [StringLength(255)]
        public string MARCASEG { get; set; }

        [StringLength(255)]
        public string BLOQUEADO { get; set; }

        [StringLength(255)]
        public string NUMIMP { get; set; }
        [StringLength(255)]
        public string PC { get; set; }

        [StringLength(255)]
        public string USUARIO { get; set; }


        #region NO
        //public decimal VRFPAGO { get; set; }

        //public decimal VRTOTALP { get; set; }

        //[StringLength(255)]
        //public string TPAGO { get; set; }

        //[StringLength(255)]
        //public string VRDTORT { get; set; }

        //[StringLength(255)]
        //public string UFECHPAG { get; set; }

        //[StringLength(255)]
        //public string UFECHPAGX { get; set; }

        //[StringLength(255)]
        //public string PLANILLA { get; set; }


        //[StringLength(255)]
        //public string ANOP { get; set; }

        //[StringLength(255)]
        //public string PDTO { get; set; }

        //[StringLength(255)]
        //public string PDTOCOND { get; set; }

        //[StringLength(255)]
        //public string PDTOCOND2 { get; set; }

        //[StringLength(255)]
        //public string PDTOCOND3 { get; set; }

        //[StringLength(255)]
        //public string PDTOCOND4 { get; set; }

        //[StringLength(255)]
        //public string FECHAVEN { get; set; }

        //[StringLength(255)]
        //public string FECHAVEN2 { get; set; }

        //[StringLength(255)]
        //public string FECHAVEN3 { get; set; }

        //[StringLength(255)]
        //public string FECHAVEN4 { get; set; }

        //[StringLength(255)]
        //public string PLAZO { get; set; }

        //[StringLength(255)]
        //public string PLAZO2 { get; set; }

        //[StringLength(255)]
        //public string PLAZO3 { get; set; }

        //[StringLength(255)]
        //public string PLAZO4 { get; set; }

        //[StringLength(255)]
        //public string PDTOC { get; set; }

        //[StringLength(255)]
        //public string PDTOC2 { get; set; }

        //[StringLength(255)]
        //public string PDTOC3 { get; set; }

        //[StringLength(255)]
        //public string PDTOC4 { get; set; }

        //[StringLength(255)]
        //public string DEPEND { get; set; }

        //[StringLength(255)]
        //public string TIPOCON { get; set; }

        //[StringLength(255)]
        //public string NUMCON { get; set; }

        //[StringLength(255)]
        //public string TIPODOCP { get; set; }

        //[StringLength(255)]
        //public string NUMDOC { get; set; }

        //[StringLength(255)]
        //public string INGGAS { get; set; }

        //[StringLength(255)]
        //public string CDP { get; set; }

        //[StringLength(255)]
        //public string OBLIGAC { get; set; }

        //[StringLength(255)]
        //public string REGISTRO { get; set; }

        //[StringLength(255)]
        //public string EXPIRA { get; set; }

        //[StringLength(255)]
        //public string ESTACION { get; set; }
        //[StringLength(255)]
        //public string SALDO { get; set; }

        //[StringLength(255)]
        //public string NOAGRUPE { get; set; }

        //[StringLength(255)]
        //public string ZONA { get; set; }

        //[StringLength(255)]
        //public string PCOMI { get; set; }

        //[StringLength(255)]
        //public string NUMERO2 { get; set; }

        //[StringLength(255)]
        //public string VRCOMI { get; set; }

        //[StringLength(255)]
        //public string PRECIO { get; set; }

        //[StringLength(255)]
        //public string FECHA1 { get; set; }

        //[StringLength(255)]
        //public string FECHA2 { get; set; }

        //[StringLength(255)]
        //public string LOGICAL1 { get; set; }

        //[StringLength(255)]
        //public string LOGICAL2 { get; set; }

        //[StringLength(255)]
        //public string CONDIC { get; set; }

        //[StringLength(255)]
        //public string CONDIC2 { get; set; }

        //[StringLength(255)]
        //public string PROYECTO { get; set; }

        //[StringLength(255)]
        //public string AREA { get; set; }

        //[StringLength(255)]
        //public string SUCCOSTO { get; set; }

        //[StringLength(255)]
        //public string BODEGA1 { get; set; }

        //[StringLength(255)]
        //public string BODEGA2 { get; set; }

        //[StringLength(255)]
        //public string LIBRO { get; set; }

        //[StringLength(255)]
        //public string AFECTA { get; set; } 
        #endregion
    }
}
