using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.CodigosBanco")]
    public class CodigosBanco
    {
        [Key]
        public string codig_banco { get; set; }
        public string Banco { get; set; }

        [ForeignKey("nit_consignacion")]
        public virtual ICollection<FactOpcaja> FactOpcaja { get; set; }
        [ForeignKey("nit_consignacion1")]
        public virtual ICollection<FactOpcaja> FactOpcaja1 { get; set; }
        [ForeignKey("nit_consignacion2")]
        public virtual ICollection<FactOpcaja> FactOpcaja2 { get; set; }
        [ForeignKey("nit_consignacion3")]
        public virtual ICollection<FactOpcaja> FactOpcaja3 { get; set; }
        [ForeignKey("nit_consignacion4")]
        public virtual ICollection<FactOpcaja> FactOpcaja4 { get; set; }
        [ForeignKey("nit_consignacion5")]
        public virtual ICollection<FactOpcaja> FactOpcaja5 { get; set; }
    }
}