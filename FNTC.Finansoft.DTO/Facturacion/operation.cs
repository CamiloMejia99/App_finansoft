using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("fac.operation")]
    public class operation
    {
        public int id { get; set; }

        [ForeignKey("productoFK")]
        public int productId { get; set; }

        public int quantity { get; set; }

        [ForeignKey("operationType")]
        public int operationTypeId { get; set; }

        public int facturaId { get; set; }
        public DateTime date { get; set; }
        public decimal price { get; set; }
        public decimal discount { get; set; }
        public string userId { get; set; }

        public virtual producto productoFK { get; set; }
        public virtual operationType operationType { get; set; }
    }
}