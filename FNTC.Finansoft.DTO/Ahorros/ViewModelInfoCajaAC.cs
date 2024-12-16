using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Ahorros
{
    public class ViewModelInfoCajaAC
    {
        public string CodigoCaja { get; set; }
        public string NombreCaja { get; set; }
        public string IdCajero { get; set; }
        public string NombreCajero { get; set; }
        public string IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NumeroCuenta { get; set; }
        public string ValorCuota { get; set; }
        public string SaldoActual { get; set; }



        public ViewModelInfoCajaAC GetViewModelInfoCajaAC(string CodigoCaja, string NombreCaja, string IdCajero, string NombreCajero, string IdUsuario
                                                         ,string NombreUsuario, string NumeroCuenta, string ValorCuota, string SaldoActual)
        { 
            var model = new ViewModelInfoCajaAC(){ 
                CodigoCaja=CodigoCaja,
                NombreCaja=NombreCaja,
                IdCajero=IdCajero,  
                NombreCajero=NombreCajero,
                IdUsuario=IdUsuario,
                NombreUsuario=NombreUsuario,
                NumeroCuenta=NumeroCuenta,
                ValorCuota=ValorCuota,  
                SaldoActual=SaldoActual
            };
            return model;
        }

    }
}
