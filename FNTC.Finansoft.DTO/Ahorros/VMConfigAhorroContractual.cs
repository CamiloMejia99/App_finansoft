using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Ahorros
{
    public class VMConfigAhorroContractual
    {
        [Key]
        public int Id { get; set; }

        
        public string NombreConfiguracion { get; set; }

        
        public string Prefijo { get; set; }


        public decimal ValorMinimo { get; set; }


        public decimal ValorMaximo { get; set; }

        
        public string IdComprobante { get; set; }

        
        public string IdCuenta { get; set; }

        
        public bool SeCausa { get; set; }


        public string AuxTasaEfectiva { get; set; }

        public decimal TasaEfectiva { get; set; }

        
        public bool Morosidad { get; set; }

        public bool Estado { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        public string FechaRegistro { get; set; }

        
        public string UserId { get; set; }

        public string AuxValorMinimo { get; set; }

        
        public string AuxValorMaximo { get; set; }
    }
}
