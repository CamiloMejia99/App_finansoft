using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Auditoria
{
    [Table("dbo.Empresa")]
    public class Empresaa
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("terceroFK")]
        [StringLength(20)]
        [Required]
        public string nit { get; set; }

        [StringLength(250)]
        [Required]
        public string nombre { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime fechaCreacion { get; set; }

        [Required]
        public int numAsociados { get; set; }

        [StringLength(250)]
        public string correo { get; set; }

        [StringLength(250)]
        public string direccion { get; set; }

        [StringLength(15)]
        public string telefono { get; set; }

        [Required]
        [ForeignKey("tipoEmpresaFK")]
        public int tipoEmpresa { get; set; }


        public virtual TipoEmpresa tipoEmpresaFK { get; set; }
        public virtual Tercero terceroFK { get; set; }

    }
}
