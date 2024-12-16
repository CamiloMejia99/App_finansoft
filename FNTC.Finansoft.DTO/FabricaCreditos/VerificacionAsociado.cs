using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.VerificacionAso")]
    public class VerificacionAsociado
    {
        [Key]
        public int IdVerificacion { get; set; }
        public int idasociado { get; set; }
        public int idsolicitud { get; set; }
        public string direccionresidencia { get; set; }
        public string observacionesdir { get; set; }
        public string telefonocelular { get; set; }
        public string observacionestele { get; set; }
        public string correoelectronico { get; set; }
        public string observacionescor { get; set; }
        public string ocupacionoficio { get; set; }
        public string observacionesocu { get; set; }
        public string estadocivil { get; set; }
        public string observacionesest { get; set; }
        public string nombredelconyuge { get; set; }
        public string observacionesnomcon { get; set; }
        public string telefonodelconyuge { get; set; }
        public string observacionestelecon { get; set; }
        public string tipovivienda { get; set; }
        public string observacionestip { get; set; }
        public string nombredelnegocio { get; set; }
        public string observacionesnomnego { get; set; }
        public string direcciontrabajo { get; set; }
        public string observacionesdiretrab { get; set; }
        public string tiempodelnegocio { get; set; }
        public string observacionestiemp { get; set; }
        public string noempleados { get; set; }
        public string observacionesnoempl { get; set; }
        public string ingresosmensuales { get; set; }
        public string observacionesingres { get; set; }
        public string gastosmensuales { get; set; }
        public string observacionesgastos { get; set; }

    }
}



