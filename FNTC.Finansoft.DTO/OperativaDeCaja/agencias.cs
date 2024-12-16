using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    public class agencias
    {
        [Key]
        public int codigoagencia { get; set; }
        public string nombreagencia { get; set; }
        public string codpais { get; set; }
        public string codciudad { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string coddepto { get; set; }
        public DateTime fechacreacion { get; set; }
        public DateTime fechasistema { get; set; }
        public string email { get; set; }
        public DateTime fechatrabajo { get; set; }
        public string tipocorreo { get; set; }
        public string codigoagenciaseguro { get; set; }
        public string nombreperfil { get; set; }

        //public virtual ICollection<Caja> Caja { get; set; }
        //public virtual ICollection<RegistroAbastecimiento> RegistroAbastecimientos { get; set; }
    }
}