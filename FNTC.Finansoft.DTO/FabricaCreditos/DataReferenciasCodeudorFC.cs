using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.ReferenciasCodeudor")]
    public class DataReferenciasCodeudorFC
    {
        [Key]
        public int IDREFCODE { get; set; }

        public string NIT { get; set; }

        public string NOMBRE1 { get; set; }

        public string NOMBRE2 { get; set; }

        public string APELLIDO1 { get; set; }

        public string APELLIDO2 { get; set; }

        public string GENERO { get; set; }

        public int EDAD { get; set; }

        public string TELEFONO { get; set; }
        public string CELULAR { get; set; }

        public string CORREO { get; set; }

        public string DIRECCIONRESIDENCIA { get; set; }

        public int IDSOLICITUD { get; set; }

        public string ACTIVIDADECONOMICA { get; set; }

        public string PARENTESCO { get; set; }

        public string VERIFICACION { get; set; }

        public string INFADICIONAL { get; set; }
        public string TIEMPODECONOCER { get; set; }
        public string TIPODEDOCUMENTO { get; set; }
        public string LUGARDEEXPEDICION { get; set; }
        public DateTime FECHAEXPEDICION { get; set; }
        public DateTime FECHANACIMIENTO { get; set; }
        public string LUGARDENACIMIENTO { get; set; }
        public decimal INGRESOSMENSUALES { get; set; }
        public decimal EGRESOSMENSUALES { get; set; }
        public string DESCRIPCIONDEBIENES { get; set; }
        public int CANTIDAD { get; set; }
        public decimal VALOR { get; set; }
        public string VNOMBRECOMPLETO { get; set; }
        public string VTELEFONOOCELULAR { get; set; }
        public string VDIRECCIONRESIDENCIA { get; set; }
        public string VACTIVIDADECONOMICA { get; set; }
        public string VPARENTESCO { get; set; }
        public string VTIEMPODECONOCER { get; set; }
        public string BNOMBRECOMPLETO { get; set; }
        public string BTELEFONOOCELULAR { get; set; }
        public string BDIRECCIONRESIDENCIA { get; set; }
        public string BACTIVIDADECONOMICA { get; set; }
        public string BPARENTESCO { get; set; }
        public string BTIEMPODECONOCER { get; set; }
        public string VINGRESOS { get; set; }
        public string VEGRESOS { get; set; }
        public string BINGRESOS { get; set; }
        public string BEGRESOS { get; set; }
        public string VCORREO { get; set; }
    }
}