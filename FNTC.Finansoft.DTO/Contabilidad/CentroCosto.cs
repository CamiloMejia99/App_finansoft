using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    [Table("acc.CentrosCostos")]
    public class CentroCosto
    {
        [Key]
        public string Codigo { get; set; }

        [Required]
        public string Nombre { get; set; }

        public Nullable<bool> Activo { get; set; }

        [ForeignKey("CentroCostoCaja")]
        public virtual ICollection<configCajero> configCajero { get; set; }

        [ForeignKey("Tipocomprobante_caja")]
        public virtual ICollection<configCajero> configCajero1 { get; set; }
    }
}
