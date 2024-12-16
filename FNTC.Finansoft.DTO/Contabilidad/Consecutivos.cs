/*
 La idea es poder tener control de los consecutivos asignados para no overlapeear
 * Esta tabla es de tipo temporal
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    public class Consecutivos
    {
        [Key]
        public int Id { get; set; }
        public string Documento { get; set; }
        public int Consecutivo { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
