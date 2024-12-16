using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    public class TipoCuentaImpuestos
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int ID { get; set; }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        //esto va en parametrosGenerales
    }
}
