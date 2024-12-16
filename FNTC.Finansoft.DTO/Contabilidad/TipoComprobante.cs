namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("acc.TiposComprobantes")]
    public partial class TipoComprobante
    {
        [Key]
        [StringLength(255)]
        public string CODIGO { get; set; }

        [StringLength(2)]
        public string CLASEComprobante { get; set; }

        //[StringLength(255)]
        //public string GRUPOTD { get; set; }

        [StringLength(255)]
        public string LIBRO { get; set; }

        [StringLength(255)]
        public string NOMBRE { get; set; }

        [StringLength(255)]
        public string CONSECUTIVO { get; set; }

        [ForeignKey("FormaPago")]
        public int? FormaPagoId { get; set; }


        public FormasPago FormaPago { get; set; }

        //public 

        public bool? CONSECMANU { get; set; }

        public bool? CONSENUME { get; set; }

        public byte CEROSCONSE { get; set; }

        //public bool? CAMBIAR { get; set; }

        public bool? INACTIVO { get; set; }

        public string Owner { get; set; }
        #region pendinete

        //[StringLength(255)]
        //public string IMPPOS { get; set; }

        //[StringLength(255)]
        //public string IMPPOSALT { get; set; }

        //[StringLength(255)]
        //public string IMPSERMOV { get; set; }

        //[StringLength(255)]
        //public string IMPPER { get; set; }

        //[StringLength(255)]
        //public string IMPPERX { get; set; }

        //[StringLength(255)]
        //public string IMPMEDPAG { get; set; }

        //[StringLength(255)]
        //public string NOIMP { get; set; }

        //[StringLength(255)]
        //public string PREFIJO { get; set; }

        //[StringLength(255)]
        //public string FECHRESOL { get; set; }

        //[StringLength(255)]
        //public string RESOLDIAN { get; set; }

        //[StringLength(255)]
        //public string DESDE { get; set; }

        //[StringLength(255)]
        //public string HASTA { get; set; }

        //[StringLength(255)]
        //public string VRESVENCE { get; set; }

        //[StringLength(255)]
        //public string VRESCON { get; set; }

        //[StringLength(255)]
        //public string NOIVA { get; set; }

        //[StringLength(255)]
        //public string ENCABE1 { get; set; }

        //[StringLength(255)]
        //public string ENCABE2 { get; set; }

        //[StringLength(255)]
        //public string ENCABE3 { get; set; }

        //[StringLength(255)]
        //public string ENCABE4 { get; set; }

        //[StringLength(255)]
        //public string ENCABE5 { get; set; }

        //[StringLength(255)]
        //public string DETPIE1 { get; set; }

        //[StringLength(255)]
        //public string DETPIE2 { get; set; }

        //[StringLength(255)]
        //public string DETPIE3 { get; set; }

        //[StringLength(255)]
        //public string DETPIE4 { get; set; }

        //[StringLength(255)]
        //public string DETPIE5 { get; set; }

        //[StringLength(255)]
        //public string DETAPIE { get; set; }

        //[StringLength(255)]
        //public string ENCABE { get; set; }

        //[StringLength(255)]
        //public string FPAGO { get; set; }

        //[StringLength(255)]
        //public string IMPNOM { get; set; }

        //[StringLength(255)]
        //public string IMPDIVS { get; set; }

        //[StringLength(255)]
        //public string PRECIO { get; set; }

        //[StringLength(255)]
        //public string PRECONIVA { get; set; }

        //[StringLength(255)]
        //public string NODESCTOS { get; set; }

        //[StringLength(255)]
        //public string IVACONSU { get; set; }

        //[StringLength(255)]
        //public string MAXITEMS { get; set; }

        //[StringLength(255)]
        //public string MAXITEMSX { get; set; }

        //[StringLength(255)]
        //public string CCOSTO { get; set; }

        //[StringLength(255)]
        //public string SUCCOSTO { get; set; }

        //[StringLength(255)]
        //public string CCOXITEM { get; set; }

        //[StringLength(255)]
        //public string IMPUESPEC { get; set; }

        //[StringLength(255)]
        //public string BODEGA { get; set; }

        //[StringLength(255)]
        //public string BODEGA2 { get; set; }

        //[StringLength(255)]
        //public string FORBODEGA { get; set; }

        //[StringLength(255)]
        //public string CONSEEVENT { get; set; }

        //[StringLength(255)]
        //public string DETAEVENT { get; set; }

        //[StringLength(255)]
        //public string IDSRED { get; set; }

        //[StringLength(255)]
        //public string USUPER { get; set; }

        //[StringLength(255)]
        //public string USUPER2 { get; set; }

        //[StringLength(255)]
        //public string USUPER3 { get; set; }

        //[StringLength(255)]
        //public string MANESERV { get; set; }

        //[StringLength(255)]
        //public string CTARETE { get; set; }

        //[StringLength(255)]
        //public string OPENCAJON { get; set; }

        //[StringLength(255)]
        //public string UTIREF1 { get; set; }

        //[StringLength(255)]
        //public string UTIREF2 { get; set; }

        //[StringLength(255)]
        //public string UTIREF3 { get; set; }

        //[StringLength(255)]
        //public string UTIREF4 { get; set; }

        //[StringLength(255)]
        //public string NOMREF1 { get; set; }

        //[StringLength(255)]
        //public string NOMREF2 { get; set; }

        //[StringLength(255)]
        //public string NOMREF3 { get; set; }

        //[StringLength(255)]
        //public string NOMREF4 { get; set; }

        //[StringLength(255)]
        //public string CTA1 { get; set; }

        //[StringLength(255)]
        //public string CTA2 { get; set; }

        //[StringLength(255)]
        //public string CTA3 { get; set; }

        //[StringLength(255)]
        //public string CTA4 { get; set; }

        //[StringLength(255)]
        //public string CTA5 { get; set; }

        //[StringLength(255)]
        //public string CTA6 { get; set; }

        //[StringLength(255)]
        //public string CTA7 { get; set; }

        //[StringLength(255)]
        //public string CTA8 { get; set; }

        //[StringLength(255)]
        //public string CTA9 { get; set; }

        //[StringLength(255)]
        //public string CTA10 { get; set; }

        //[StringLength(255)]
        //public string NCTA1 { get; set; }

        //[StringLength(255)]
        //public string NCTA2 { get; set; }

        //[StringLength(255)]
        //public string NCTA3 { get; set; }

        //[StringLength(255)]
        //public string NCTA4 { get; set; }

        //[StringLength(255)]
        //public string NCTA5 { get; set; }

        //[StringLength(255)]
        //public string NCTA6 { get; set; }

        //[StringLength(255)]
        //public string NCTA7 { get; set; }

        //[StringLength(255)]
        //public string NCTA8 { get; set; }

        //[StringLength(255)]
        //public string NCTA9 { get; set; }

        //[StringLength(255)]
        //public string NCTA10 { get; set; }

        //[StringLength(255)]
        //public string EXTERNO { get; set; }

        //[StringLength(255)]
        //public string LPREFIJ { get; set; }

        //[StringLength(255)]
        //public string MODO { get; set; }

        //[StringLength(255)]
        //public string NOIMPNIT { get; set; }

        //[StringLength(255)]
        //public string IMPPOSVAL { get; set; }

        //[StringLength(255)]
        //public string IMPITAGRU { get; set; }

        //[StringLength(255)]
        //public string IMPITAGRUX { get; set; }

        //[StringLength(255)]
        //public string IMPOBLI { get; set; }

        //[StringLength(255)]
        //public string INVEPERIO { get; set; }

        //[StringLength(255)]
        //public string FORFPAGO { get; set; } 
        #endregion
        #region Pendiente

        //[StringLength(255)]
        //public string BODEGAITEM { get; set; }

        //[StringLength(255)]
        //public string RECOSTEA { get; set; }

        //[StringLength(255)]
        //public string VENDEDOR { get; set; }

        //[StringLength(255)]
        //public string CTARETEV { get; set; }

        //[StringLength(255)]
        //public string CTARETEC { get; set; }

        //[StringLength(255)]
        //public string RETIVA { get; set; }

        //[StringLength(255)]
        //public string NOITDOCU { get; set; }

        //[StringLength(255)]
        //public string CONTABTR { get; set; }

        //[StringLength(255)]
        //public string PDA { get; set; }

        //[StringLength(255)]
        //public string AIU { get; set; }

        //[StringLength(255)]
        //public string ESENTRADA { get; set; }

        //[StringLength(255)]
        //public string ESBAJA { get; set; }

        //[StringLength(255)]
        //public string ESTRANSFER { get; set; }

        //[StringLength(255)]
        //public string ESRESPONSA { get; set; }

        //[StringLength(255)]
        //public string CONTAIMPOR { get; set; }

        //[StringLength(255)]
        //public string DIASPLAZO { get; set; }

        //[StringLength(255)]
        //public string VERCOLTER { get; set; }

        //[StringLength(255)]
        //public string VERCOLREF { get; set; }

        //[StringLength(255)]
        //public string NSALCONSEC { get; set; }

        //[StringLength(255)]
        //public string IMPCOPIAS { get; set; }

        //[StringLength(255)]
        //public string FORVENDE { get; set; }

        //[StringLength(255)]
        //public string LRETFTE { get; set; }

        //[StringLength(255)]
        //public string LRETCRE { get; set; }

        //[StringLength(255)]
        //public string LRETIVA { get; set; }

        //[StringLength(255)]
        //public string LRETICA { get; set; }

        //[StringLength(255)]
        //public string TARICA { get; set; }

        //[StringLength(255)]
        //public string DEPEND { get; set; }

        //[StringLength(255)]
        //public string NOMODCOSTO { get; set; }

        //[StringLength(255)]
        //public string TRASLA { get; set; }

        //[StringLength(255)]
        //public string TRASLASUC { get; set; }

        //[StringLength(255)]
        //public string MEMORIAS { get; set; }

        //[StringLength(255)]
        //public string VRLETRAS { get; set; }

        //[StringLength(255)]
        //public string SERMOV { get; set; }

        //[StringLength(255)]
        //public string FIRMAS { get; set; }

        //[StringLength(255)]
        //public string OLECTURA { get; set; }

        //[StringLength(255)]
        //public string NOCONSOLID { get; set; }

        //[StringLength(255)]
        //public string ESPCDESC { get; set; } 
        #endregion



        public TipoComprobante()
        {
            //this.FormaPago = new FormasPago();
        }
        /*
        [ForeignKey("Compr_ingreso")]
        public virtual ICollection<configCajero> configCajero { get; set; }

        [ForeignKey("Compr_egreso")]
        public virtual ICollection<configCajero> configCajero1 { get; set; }

        [ForeignKey("Tipocomprobante_caja")]
        public virtual ICollection<configCajero> configCajero2 { get; set; }
        */
    }
}
