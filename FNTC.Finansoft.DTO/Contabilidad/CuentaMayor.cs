namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    using System.ComponentModel;
    //#error debe verificar el tipo de cuenta y donde opera, ejemploo activos fijos con depreciacion
    /*
     de inventarios con facturacion
     * para los submodulos solo deben mostrarse las cuentas que estan marcadas por ej
     * cartera 1405
     * es corriente
     * cuenta se sobreguira validacion = cuando va a menos de 0
     * 
     */
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("acc.PlanCuentas")]
    public class CuentaMayor
    {
        [Key]
        [StringLength(255)]
        [Required(ErrorMessage = "Requerido: Debe tener 9 digitos")]
        public string CODIGO { get; set; }

        [StringLength(255)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(1)]
        public string NATURALEZA { get; set; }
        // public List<SelectListItem> _naturaleza { get; set; }

        //private List<SelectListItem> GetNaturaleza()
        //{
        //    var listanaturaleza = new List<SelectListItem>();
        //    listanaturaleza.Add(new SelectListItem() { Text = "Débito" });
        //    listanaturaleza.Add(new SelectListItem() { Text = "Crédito" });

        //    return listanaturaleza;

        //}


        //public CuentaMayor()
        //{
        //    _naturaleza = GetNaturaleza();
        //}

        [DisplayName("Req. Tercero")]
        public bool REQTERCERO { get; set; }

        [DisplayName("Req. Costo")]
        public bool REQCCOSTO { get; set; }

        [DisplayName("Corriente")]
        public bool CORRIENTE { get; set; }

        [DisplayName("Valida Saldo")]
        public bool VALIDASALDO { get; set; }

        [DisplayName("Inactivo")]
        public bool INACTIVO { get; set; }


        //apara fiscales
        [DisplayName("Es cuenta impuesto")]
        public bool EsCuentaImpuesto { get; set; }

        [DisplayName("Aplica Para Direfencia NIIF")]
        public bool EsCuentaNIIF { get; set; }

        //para activos fijos
        public bool EsCuentaDepreciacion { get; set; }

        //Creditos
        public bool EsCuentaCreditos { get; set; }

        //Nomina
        public bool EsCuentaNomina { get; set; }

        //Comercial
        public bool EsCuentaInventarios { get; set; }

        //paraAportes y Ahorros Contractuales
        //public Nullable<bool> EsCuentaInventarios { get; set; }

        public decimal Saldo { get; set; }
        public decimal Porcentaje { get; set; }

        //niff
        [StringLength(255)]
        public string CTANIIF { get; set; }

        [StringLength(255)]
        public string VALIDLIB { get; set; }

        [StringLength(255)]
        public string CTAREMPL { get; set; }

        

        #region NO
        //[StringLength(255)]
        //public string FLUJOEFEC { get; set; }

        //[StringLength(255)]
        //public string CODPUB { get; set; }

        //[StringLength(255)]
        //public string NOVALDOCR { get; set; }

        //[StringLength(255)]
        //public string PARAMESP { get; set; }

        //[StringLength(255)]
        //public string REQDOCR { get; set; }

        //[StringLength(255)]
        //public string REQARTR { get; set; }

        //[StringLength(255)]
        //public string PORINC { get; set; }

        //[StringLength(255)]
        //public string RECIPROCA { get; set; } 
        #endregion
        /*
    [ForeignKey("Contr_banco")]
    public virtual ICollection<configCajero> configCajero { get; set; }
    [ForeignKey("Contr_otro")]
    public virtual ICollection<configCajero> configCajero1 { get; set; }
    [ForeignKey("Cta_efectivo")]
    public virtual ICollection<configCajero> configCajero2 { get; set; }
    [ForeignKey("Cta_cheque")]
    public virtual ICollection<configCajero> configCajero3 { get; set; }
    public virtual ICollection<Caja> Caja { get; set; }
    */
    }
}
