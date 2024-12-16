using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("fac.Productos")]
    public class producto
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string nomProducto { get; set; }


        [Display(Name = "Precio de Entrada")]
        public decimal precioEntrada { get; set; }


        [Display(Name = "Precio de Salida")]
        public decimal precioSalida { get; set; }

        [Required]
        [ForeignKey("ivaFK")]
        [Display(Name = "Iva")]
        public int iva { get; set; }

        [Required]
        [Display(Name = "Inventario Inicial")]
        public int inventarioInicial { get; set; }

        [Required]
        [NotMapped]
        [Display(Name = "Precio de Entrada")]
        public string auxPrecioEntrada { set; get; }

        [Required]
        [NotMapped]
        [Display(Name = "Precio de Salida")]
        public string auxPrecioSalida { set; get; }


        public virtual iva ivaFK { get; set; }
    }
}
