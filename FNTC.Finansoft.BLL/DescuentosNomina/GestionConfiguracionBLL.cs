using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNTC.Finansoft.Accounting.DAL.DescuentosNomina;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.DescuentosNomina;


namespace FNTC.Finansoft.Accounting.BLL.DescuentosNomina
{
    public class GestionConfiguracionBLL
    {
        private static GestionConfiguracionDAL obj = new GestionConfiguracionDAL();
        
        public bool AgregarCuenta(OrdenDePrioridadPagos ordenDePrioridadPagos)
        {
            var respuesta = new GestionConfiguracionDAL().AgregarCuenta(ordenDePrioridadPagos);
            return respuesta;
        }
        public bool AgregarClasifiacion(TipoPagos tipoPagos)
        {
            var respuesta = new GestionConfiguracionDAL().AgregarClasifiacion(tipoPagos);
            return respuesta;
        }
        public bool AgregarEstructuraPlanos(EstructuraPlanos estructuraPlanos)
        {
            var respuesta = new GestionConfiguracionDAL().AgregarEstructuraPlanos(estructuraPlanos);
            return respuesta;
        }
        public bool AgregarNuevaDiscriminacion(RelacionPlanosDiscriminacion relacionPlanosDiscriminacion, DatosDiscriminacionPlanos datosDiscriminacionPlanos)
        {
            var respuesta = new GestionConfiguracionDAL().AgregarNuevaDiscriminacion(relacionPlanosDiscriminacion, datosDiscriminacionPlanos);
            return respuesta;
        }
        public bool AgregarRelacionPlanosEmpresa(RelacionPlanosEmpresa relacionPlanosEmpresa)
        {
            var respuesta = new GestionConfiguracionDAL().AgregarRelacionPlanosEmpresa(relacionPlanosEmpresa);
            return respuesta;
        }
        public bool AgregarRelacionPlanos(RelacionPlanosEmpresa relacionPlanosEmpresa)
        {
            var respuesta = new GestionConfiguracionDAL().AgregarRelacionPlanos(relacionPlanosEmpresa);
            return respuesta; 
        }
        public bool AgregarNuevoCampo(ConformacionDeLosPlanos conformacionDeLosPlanos)
        {
            var respuesta = new GestionConfiguracionDAL().AgregarNuevoCampo(conformacionDeLosPlanos);
            return respuesta;
        }
        public bool ActualizarSaldosSobrantes(SaldosSobrantes saldosSobrantes)
        {
            var respuesta = new GestionConfiguracionDAL().ActualizarSaldosSobrantes(saldosSobrantes);
            return respuesta;
        }
        public bool ActualizarContraPartida(ContraPartida contraPartida)
        {
            var respuesta = new GestionConfiguracionDAL().ActualizarContraPartida(contraPartida);
            return respuesta;
        }
        public bool EditarCuenta(OrdenDePrioridadPagos ordenDePrioridadPagos)
        {
            var respuesta = new GestionConfiguracionDAL().EditarCuenta(ordenDePrioridadPagos);
            return respuesta;
        }
        public bool EditarDetallesAso(DatosDiscriminacionPlanos datosDiscriminacionPlanos, ControlDeMovimientos controlDeMovimientos)
        {
            var respuesta = new GestionConfiguracionDAL().EditarDetallesAso(datosDiscriminacionPlanos, controlDeMovimientos);
            return respuesta;
        }
        public bool EditarEstructuraPlanos(EstructuraPlanos estructuraPlanos)
        {
            var respuesta = new GestionConfiguracionDAL().EditarEstructuraPlanos(estructuraPlanos);
            return respuesta;
        }
        public bool EditarCampo(ConformacionDeLosPlanos conformacionDeLosPlanos)
        {
            var respuesta = new GestionConfiguracionDAL().EditarCampo(conformacionDeLosPlanos);
            return respuesta;
        }
        public bool Eliminar(OrdenDePrioridadPagos ordenDePrioridadPagos, ControlDeMovimientos controlDeMovimientos)
        {
            var respuesta = new GestionConfiguracionDAL().Eliminar(ordenDePrioridadPagos, controlDeMovimientos);
            return respuesta;
        }
        public bool EliminarEstructuraPlanos(EstructuraPlanos estructuraPlanos, ControlDeMovimientos controlDeMovimientos)
        {
            var respuesta = new GestionConfiguracionDAL().EliminarEstructuraPlanos(estructuraPlanos, controlDeMovimientos);
            return respuesta;
        }
        public bool EliminarCampo(ConformacionDeLosPlanos conformacionDeLosPlanos, ControlDeMovimientos controlDeMovimientos)
        {
            var respuesta = new GestionConfiguracionDAL().EliminarCampo(conformacionDeLosPlanos, controlDeMovimientos);
            return respuesta;
        }
        public bool EditarClasificacion(TipoPagos tipo)
        {
            var respuesta = new GestionConfiguracionDAL().EditarClasificacion(tipo);
            return respuesta;
        }


    }
}
