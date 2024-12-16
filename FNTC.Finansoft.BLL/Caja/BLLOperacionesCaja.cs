using FNTC.Finansoft.Accounting.DAL.Caja;
using FNTC.Finansoft.Accounting.DTO.Ahorros;
using FNTC.Finansoft.Accounting.DTO.Aportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.BLL.Caja
{
    public  class BLLOperacionesCaja
    {
        public RespuestaAporte PagarAporte(string cuenta, string cajero, decimal valor)
        { 
            var respuesta = new OperacionesCaja().PagarAporte(cuenta, cajero, valor);
            return respuesta;   
        }



        #region OPERACIONES AHORRO CONTRACTUAL
        public async Task<ViewModelInfoCajaAC> GetModelInfoCajaAC(string NumeroCuenta, string IdCajero)
        {
            return await new OperacionesCaja().GetModelInfoCajaAC(NumeroCuenta, IdCajero);
        }

        public async Task<RespuestaAhorro> PagarAhorroAC(string cuenta, string cajero, decimal valor,string observacion)
        {
            var respuesta = await new OperacionesCaja().PagarAhorroAC(cuenta, cajero, valor,observacion);
            return respuesta;
        }
        #endregion


        public RespuestaAporte PagarAporteExtra(string cuentacodigo_caja, string nit_cajero, string nit_propietario_cuenta, string numero_cuenta, string observacion, string operacion, decimal saldo_total_cuenta, decimal total)
        {
            var respuesta = new OperacionesCaja().PagarAporteEx(cuentacodigo_caja, nit_cajero, nit_propietario_cuenta, numero_cuenta, observacion, operacion, saldo_total_cuenta, total);
            return respuesta;
        }

    }
}
