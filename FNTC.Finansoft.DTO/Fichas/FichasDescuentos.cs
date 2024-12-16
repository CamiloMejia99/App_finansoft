using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Fichas
{
    [Table("des.FichasDescuentos")]
    public class FichasDescuentos
    {
        [Key]
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public Nullable<int> IdConfiguracion { get; set; }
        [ForeignKey("Tercero")]
        public string idPersona { get; set; }
        public string tipoPago { get; set; }
        public string valor { get; set; }
        public string valorCuota { get; set; }
        public string totalDescuentos { get; set; }
        public Nullable<System.DateTime> FechaApertura { get; set; }
        public Nullable<bool> activa { get; set; }

        //public virtual Configuracion2 Configuracion2 { get; set; }
        public virtual Tercero Tercero { get; set; }
    }
}