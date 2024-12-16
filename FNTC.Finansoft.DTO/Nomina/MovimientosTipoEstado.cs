using System;
using System.Collections.Generic;



namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("nom.DescuentosNominaMovimientosTipoEstado")]
    public class MovimientosTipoEstado
    {
        [Key]
        public int IDMovTipEs { get; set; }
        public string Cedula { get; set; }
        public string Estado { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Detallemovimiento { get; set; }

    }
}