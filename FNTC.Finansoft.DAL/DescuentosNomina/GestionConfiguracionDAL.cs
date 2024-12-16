using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNTC.Finansoft.Accounting.DTO.DescuentosNomina;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DAL.DescuentosNomina;

namespace FNTC.Finansoft.Accounting.DAL.DescuentosNomina
{
    public class GestionConfiguracionDAL
    {
        AccountingContext contextoFC = new AccountingContext();

        public bool AgregarCuenta(OrdenDePrioridadPagos ordenDePrioridadPagos)
        {
            using (var db = contextoFC)
            {
                try
                {

                    Guid Id = Guid.NewGuid();
                    var IDC = Id.ToString();
                    ordenDePrioridadPagos.IdOrdenPrioridadPagos = IDC;
                    ordenDePrioridadPagos.CodigoCuenta = ordenDePrioridadPagos.CodigoCuenta;
                    ordenDePrioridadPagos.DescripcionPagos = ordenDePrioridadPagos.DescripcionPagos;
                    ordenDePrioridadPagos.OrdenPagos = ordenDePrioridadPagos.OrdenPagos;
                    ordenDePrioridadPagos.UserControlPagos = ordenDePrioridadPagos.UserControlPagos;
                    ordenDePrioridadPagos.FechaCreacionPagos = DateTime.Now;
                    db.OrdenDePrioridadPagos.Add(ordenDePrioridadPagos);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }

        public bool AgregarClasifiacion(TipoPagos tipoPagos)
        {
            using (var db = contextoFC)
            {
                try
                {
                    Guid Id = Guid.NewGuid();
                    var IDC = Id.ToString();
                    var Conteo = (from s in db.TipoPagos select s).Count();
                    string string1 = (tipoPagos.NombrePago).ToString();
                    string upperString = string1.ToUpper();
                    tipoPagos.IdTiposPagos = IDC;
                    tipoPagos.NombrePago = upperString;
                    tipoPagos.FechaCreacion = DateTime.Now;
                    tipoPagos.UserControl = tipoPagos.UserControl;
                    tipoPagos.Orden = Conteo + 1;
                    db.TipoPagos.Add(tipoPagos);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool AgregarEstructuraPlanos(EstructuraPlanos estructuraPlanos)
        {
            using (var db = contextoFC)
            {
                try
                {

                    string string1 = (estructuraPlanos.NombreEstructuraPlanos).ToString();
                    string upperString = string1.ToUpper();
                    estructuraPlanos.NombreEstructuraPlanos = upperString;
                    estructuraPlanos.FechaCreacionEstructuraPlanos = DateTime.Now;
                    estructuraPlanos.UserControlEstructuraPlanos = estructuraPlanos.UserControlEstructuraPlanos;
                    estructuraPlanos.EstadoEstructuraPlanos = true;
                    db.EstructuraPlanos.Add(estructuraPlanos);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool AgregarNuevaDiscriminacion(RelacionPlanosDiscriminacion relacionPlanosDiscriminacion, DatosDiscriminacionPlanos datosDiscriminacionPlanos)
        {
            using (var db = contextoFC)
            {
                var DataRelacion = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == relacionPlanosDiscriminacion.IdRelacionEmpresa select s).FirstOrDefault();
                var CodigoEmpresa = DataRelacion.CodigoEmpresa;
                var CodigoPlano = DataRelacion.CodigoPlano;
                string Periodo = relacionPlanosDiscriminacion.PeriodoDeduccion.ToString();
                string DateA = DateTime.Now.ToString("yyyy");
                string PeriodoDeduccion = Periodo + ":" + DateA;
                var FechaRegistro = relacionPlanosDiscriminacion.FechaCreacion;
                var userName = relacionPlanosDiscriminacion.UserControl;
                var ContadorInt = (from s in db.RelacionPlanosDiscriminacion where s.IdEmpresa == CodigoEmpresa && s.IdPlano == CodigoPlano select s).Count();
                var contador = ContadorInt + 1;

                string CurrentDate = "01/01/0001 12:00:00 a. m.";

                DateTime DateObject = Convert.ToDateTime(CurrentDate);

                if (relacionPlanosDiscriminacion.FechaFinal == DateObject && relacionPlanosDiscriminacion.FechaInicial != DateObject)
                {
                    relacionPlanosDiscriminacion.FechaFinal = relacionPlanosDiscriminacion.FechaInicial;
                }
                if (relacionPlanosDiscriminacion.FechaFinal == DateObject && relacionPlanosDiscriminacion.FechaInicial == DateObject)
                {
                    relacionPlanosDiscriminacion.FechaInicial = DateTime.Now;
                    relacionPlanosDiscriminacion.FechaFinal = DateTime.Now;
                }
                Guid Id = Guid.NewGuid();
                var Identidad = Id.ToString();
                 
                try
                {
                    if (relacionPlanosDiscriminacion.IdRelacionEmpresa != 0)
                    {
                        relacionPlanosDiscriminacion.IdPlano = CodigoPlano;
                        relacionPlanosDiscriminacion.NoDiscriminacionPlano = contador;
                        relacionPlanosDiscriminacion.IdRelacionEmpresa = relacionPlanosDiscriminacion.IdRelacionEmpresa;
                        relacionPlanosDiscriminacion.IdEmpresa = CodigoEmpresa;
                        relacionPlanosDiscriminacion.UserControl = userName;
                        relacionPlanosDiscriminacion.FechaCreacion = FechaRegistro;
                        relacionPlanosDiscriminacion.PeriodoDeduccion = PeriodoDeduccion;
                        relacionPlanosDiscriminacion.EstadoContable = "Pendiente Validacion";
                        relacionPlanosDiscriminacion.Identificador = Identidad;
                        db.RelacionPlanosDiscriminacion.Add(relacionPlanosDiscriminacion);


                        //string cad = "", campos = "*";
                        //List<string> camps = db.ArchivoPlano.Where(p => p.CLASEPLANO == clase_plano).OrderBy(p => p.ORDEN).Select(p => p.TipoDeCampo.DESCRIPCION).ToList();

                        //campos = String.Join(", ", camps);

                        //var consulta = "Select " + campos + " From apo.FichasAportes INNER JOIN ter.Terceros ON ter.Terceros.NIT = apo.FichasAportes.idPersona";


                        var Datos = (from pc in db.FichasAportes.Include(Pc => Pc.Terceros) select pc).ToList();
                        //IList<ConsolidadoNomina> Consolidado = db.ConsolidadoNomina.ToList();
                        //db.ConsolidadoNomina.RemoveRange(db.ConsolidadoNomina.Where(c => c.EMPRESA == EMPRESA));


                        //cad = "Select " + campos + " From nom.DescuentosNominaConsolidadosNominas where EMPRESA= " + EMPRESA + "";
                        foreach (var item in Datos)

                        {

                            //ConsolidadoNomina dp = new ConsolidadoNomina();
                            var Cedula = item.idPersona;
                            List<string> aportes = db.FichasAportes.Where(p => p.idPersona == Cedula && p.activa == true).Select(p => p.valor).ToList();
                            int[] arregloInt2 = aportes.Select(x => Convert.ToInt32(x)).ToArray();
                            int TotalAporte = arregloInt2.Sum();

                            //List<decimal> Creditos = db.Creditos.Where(p => p.Creditos_Cedula == Cedula).Select(p => p.ValorCuotaMes).ToList();
                            //var MoviTipEs = db.MovimientosTipoEstado.Where(p => p.Cedula == Cedula).Select(p => p.IDMovTipEs).Max();
                            //List<string> TipoDeEstad = db.MovimientosTipoEstado.Where(p => p.IDMovTipEs == MoviTipEs).Select(p => p.Estado).ToList();
                            //decimal TotalCreditos = Creditos.Sum();

                            //decimal TotalDescuento = TotalAporte + TotalCreditos;

                            datosDiscriminacionPlanos.NitEmpresa = CodigoEmpresa;
                            datosDiscriminacionPlanos.DigitoVerificacion = "1";
                            datosDiscriminacionPlanos.TipoDeEstadoProceso = "A";
                            datosDiscriminacionPlanos.NitAsociado = item.idPersona;
                            datosDiscriminacionPlanos.NombreCompleto = item.Terceros.NOMBRE;
                            datosDiscriminacionPlanos.PrimerNombre = item.Terceros.NOMBRE1;
                            datosDiscriminacionPlanos.SegundoNombre = item.Terceros.NOMBRE2;
                            datosDiscriminacionPlanos.PrimerApellido = item.Terceros.APELLIDO1;
                            datosDiscriminacionPlanos.SegunoApellido = item.Terceros.APELLIDO2;
                            datosDiscriminacionPlanos.TotalAportes = TotalAporte;
                            datosDiscriminacionPlanos.idPlano = CodigoPlano;
                            datosDiscriminacionPlanos.NoDiscriminacion = contador;
                            datosDiscriminacionPlanos.idEmpresaRelacion = relacionPlanosDiscriminacion.IdRelacionEmpresa;
                            datosDiscriminacionPlanos.PeriodoDeduccion = PeriodoDeduccion;
                            datosDiscriminacionPlanos.FechaInicial = relacionPlanosDiscriminacion.FechaInicial;
                            datosDiscriminacionPlanos.FechaFinal = relacionPlanosDiscriminacion.FechaFinal;
                            datosDiscriminacionPlanos.FechaCreacion = DateTime.Now;
                            datosDiscriminacionPlanos.UserControl = userName;
                            datosDiscriminacionPlanos.EstadoDisPlanoAsociado = true;
                            datosDiscriminacionPlanos.EstadoContable = "Pendiente Validacion";

                            db.DatosDiscriminacionPlanos.Add(datosDiscriminacionPlanos);
                            db.SaveChanges();
                        }

                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool AgregarRelacionPlanosEmpresa(RelacionPlanosEmpresa relacionPlanosEmpresa)
        {
            using (var db = contextoFC)
            {
                try
                {
                    relacionPlanosEmpresa.CodigoEmpresa = relacionPlanosEmpresa.CodigoEmpresa;
                    relacionPlanosEmpresa.CodigoPlano = 0;
                    relacionPlanosEmpresa.FechaCreacionRelacionPlanosEmpresa = DateTime.Now;
                    relacionPlanosEmpresa.UserControlRelacionPlanosEmpresa = relacionPlanosEmpresa.UserControlRelacionPlanosEmpresa;
                    relacionPlanosEmpresa.EstadoRelacionPlanosEmpresa = true;
                    db.RelacionPlanosEmpresa.Add(relacionPlanosEmpresa);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool AgregarRelacionPlanos(RelacionPlanosEmpresa relacionPlanosEmpresa)
        {
            using (var db = contextoFC)
            {
                try
                {

                    relacionPlanosEmpresa.CodigoEmpresa = relacionPlanosEmpresa.CodigoEmpresa;
                    relacionPlanosEmpresa.CodigoPlano = relacionPlanosEmpresa.CodigoPlano;
                    relacionPlanosEmpresa.FechaCreacionRelacionPlanosEmpresa = DateTime.Now;
                    relacionPlanosEmpresa.UserControlRelacionPlanosEmpresa = relacionPlanosEmpresa.UserControlRelacionPlanosEmpresa;
                    relacionPlanosEmpresa.EstadoRelacionPlanosEmpresa = true;
                    db.RelacionPlanosEmpresa.Add(relacionPlanosEmpresa);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool AgregarNuevoCampo(ConformacionDeLosPlanos conformacionDeLosPlanos)
        {
            using (var db = contextoFC)
            {
                try
                {
                    var contenido = "";
                    if (conformacionDeLosPlanos.ContenidoValorPredeterminado == null)
                    {
                        contenido = "--";
                    }
                    else
                    {
                        contenido = conformacionDeLosPlanos.ContenidoValorPredeterminado;
                    }
                    var Conteo = (from s in db.ConformacionDeLosPlanos where s.IdPlanos == conformacionDeLosPlanos.IdPlanos select s).Count();
                    conformacionDeLosPlanos.IdPlanos = conformacionDeLosPlanos.IdPlanos;
                    conformacionDeLosPlanos.NombreCampo = conformacionDeLosPlanos.NombreCampo;
                    conformacionDeLosPlanos.ValorNulo = conformacionDeLosPlanos.ValorNulo;
                    conformacionDeLosPlanos.OrdenCampo = Conteo + 1;
                    conformacionDeLosPlanos.FechaCreacionCampo = DateTime.Now;
                    conformacionDeLosPlanos.UserControlCampo = conformacionDeLosPlanos.UserControlCampo;
                    conformacionDeLosPlanos.ValorPredeterminado = conformacionDeLosPlanos.ValorPredeterminado;
                    conformacionDeLosPlanos.EstadoCampo = conformacionDeLosPlanos.EstadoCampo;
                    conformacionDeLosPlanos.ContenidoValorPredeterminado = contenido;
                    conformacionDeLosPlanos.Campo = conformacionDeLosPlanos.Campo;
                    db.ConformacionDeLosPlanos.Add(conformacionDeLosPlanos);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool ActualizarContraPartida(ContraPartida contraPartida)
        {
            using (var db = contextoFC)
            {
                var configuracionesAnteriores = contextoFC.ContraPartida.Where(c => c.Estado == true).ToList();

                foreach (var config in configuracionesAnteriores)
                {
                    config.Estado = false;
                }

                contextoFC.ContraPartida.Add(contraPartida);

                try
                {
                    contraPartida.UserControl = contraPartida.UserControl;
                    contraPartida.Estado = true;
                    contraPartida.FechaCreacion = DateTime.Now;
                    contextoFC.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;

                }

            }
        }
        public bool ActualizarSaldosSobrantes(SaldosSobrantes saldosSobrantes)
        {
            using (var db = contextoFC)
            {
                var configuracionesAnteriores = contextoFC.SaldosSobrantes.Where(c => c.Estado == true).ToList();

                foreach (var config in configuracionesAnteriores)
                {
                    config.Estado = false;
                }

                contextoFC.SaldosSobrantes.Add(saldosSobrantes);

                try
                {
                    saldosSobrantes.UserControl = saldosSobrantes.UserControl;
                    saldosSobrantes.Estado = true;
                    saldosSobrantes.FechaCreacion = DateTime.Now;
                    contextoFC.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;

                }

            }
        }
        public bool EditarCuenta(OrdenDePrioridadPagos ordenDePrioridadPagos)
        {
            using (var db = contextoFC)
            {
                try
                {
                    var Validacion = contextoFC.TipoPagos.Where(a => a.IdTiposPagos == ordenDePrioridadPagos.OrdenPagos).FirstOrDefault();
                    if (Validacion == null)
                    {

                        var EditOrden = db.OrdenDePrioridadPagos.Find(ordenDePrioridadPagos.IdOrdenPrioridadPagos);
                        EditOrden.DescripcionPagos = ordenDePrioridadPagos.DescripcionPagos;
                        EditOrden.UserControlPagos = ordenDePrioridadPagos.UserControlPagos;
                        EditOrden.FechaCreacionPagos = DateTime.Now;
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        var validar = contextoFC.OrdenDePrioridadPagos.Where(a => a.OrdenPagos == ordenDePrioridadPagos.OrdenPagos).FirstOrDefault();
                        if (validar == null)
                        {

                            var EditOrdenOne = db.OrdenDePrioridadPagos.Find(ordenDePrioridadPagos.IdOrdenPrioridadPagos);
                            EditOrdenOne.DescripcionPagos = ordenDePrioridadPagos.DescripcionPagos;
                            EditOrdenOne.OrdenPagos = ordenDePrioridadPagos.OrdenPagos;
                            EditOrdenOne.DescripcionPagos = ordenDePrioridadPagos.DescripcionPagos;
                            EditOrdenOne.UserControlPagos = ordenDePrioridadPagos.UserControlPagos;
                            EditOrdenOne.FechaCreacionPagos = DateTime.Now;
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            var IdPrincipal = ordenDePrioridadPagos.IdOrdenPrioridadPagos;
                            var EditarPrimeroOrden = contextoFC.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == ordenDePrioridadPagos.IdOrdenPrioridadPagos).FirstOrDefault().OrdenPagos;


                            var EditarSegundoOrden = ordenDePrioridadPagos.OrdenPagos;
                            var Buscador = contextoFC.OrdenDePrioridadPagos.Where(a => a.OrdenPagos == EditarSegundoOrden).FirstOrDefault().IdOrdenPrioridadPagos;

                            var EditOrdenOn = db.OrdenDePrioridadPagos.Find(IdPrincipal);
                            EditOrdenOn.DescripcionPagos = ordenDePrioridadPagos.DescripcionPagos;
                            EditOrdenOn.OrdenPagos = EditarSegundoOrden;
                            EditOrdenOn.UserControlPagos = ordenDePrioridadPagos.UserControlPagos;
                            EditOrdenOn.FechaCreacionPagos = DateTime.Now;

                            var EditOrdenTwo = db.OrdenDePrioridadPagos.Find(Buscador);
                            EditOrdenTwo.OrdenPagos = EditarPrimeroOrden;
                            EditOrdenTwo.UserControlPagos = ordenDePrioridadPagos.UserControlPagos;
                            EditOrdenTwo.FechaCreacionPagos = DateTime.Now;


                            db.SaveChanges();
                            return true;
                        }
                    }

                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool EditarDetallesAso(DatosDiscriminacionPlanos datosDiscriminacionPlanos, ControlDeMovimientos controlDeMovimientos)
        { 
            using (var db = contextoFC) 
            {
                try
                {
                    var EditarDatos = db.DatosDiscriminacionPlanos.Find(datosDiscriminacionPlanos.IdDisPlanos);
                    var TotalAnterior = EditarDatos.TotalAportes;
                    var DAT = datosDiscriminacionPlanos.TotalAportes;
                    EditarDatos.TotalAportes = DAT;

                    controlDeMovimientos.FechaCreacion = DateTime.Now;
                    controlDeMovimientos.UserControl = datosDiscriminacionPlanos.UserControl;
                    controlDeMovimientos.Detalles = "USUARIO: " + controlDeMovimientos.UserControl + "/ EDITO VALOR - TOTAL DESCUENTO: " + EditarDatos.TotalAportes + " /VALOR ANTERIOR: " + TotalAnterior +  " / DESCUENTOS DE NOMINA - ID ASOCIADO: " + datosDiscriminacionPlanos.IdDisPlanos;
                    db.ControlDeMovimientos.Add(controlDeMovimientos);
                    
                    db.SaveChanges();
                    return true;

                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool EditarClasificacion(TipoPagos tipo)
        {
            using (var db = contextoFC)
            {
                try
                {
                    var orden = contextoFC.TipoPagos.Where(a => a.IdTiposPagos == tipo.IdTiposPagos).Select(a => a.Orden).FirstOrDefault();

                    if (orden == tipo.Orden)
                    {

                        return true;
                    }
                    else
                    {
                        var EditarOne = orden;
                        var EditarTwo = tipo.Orden;

                        var CuentaOne = contextoFC.TipoPagos.Where(a => a.Orden == EditarOne).Select(a => a.IdTiposPagos).FirstOrDefault();
                        var CuentaTwo = contextoFC.TipoPagos.Where(a => a.Orden == EditarTwo).Select(a => a.IdTiposPagos).FirstOrDefault();

                        var EditOrdenOne = db.TipoPagos.Find(CuentaOne);
                        EditOrdenOne.Orden = EditarTwo;

                        var EditOrdenTwo = db.TipoPagos.Find(CuentaTwo);
                        EditOrdenTwo.Orden = EditarOne;


                        db.SaveChanges();
                        return true;
                    }



                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool EditarEstructuraPlanos(EstructuraPlanos estructuraPlanos)
        {
            using (var db = contextoFC)
            {
                try
                {
                    string string1 = (estructuraPlanos.NombreEstructuraPlanos).ToString();
                    string upperString = string1.ToUpper();

                    var EstructuraEditar = db.EstructuraPlanos.Find(estructuraPlanos.IdEstructuraPlanos);
                    EstructuraEditar.NombreEstructuraPlanos = upperString;
                    EstructuraEditar.FechaCreacionEstructuraPlanos = DateTime.Now;
                    EstructuraEditar.UserControlEstructuraPlanos = estructuraPlanos.UserControlEstructuraPlanos;
                    EstructuraEditar.EstadoEstructuraPlanos = estructuraPlanos.EstadoEstructuraPlanos;

                    db.SaveChanges();
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public bool EditarCampo(ConformacionDeLosPlanos conformacionDeLosPlanos)
        {
            using (var db = contextoFC)
            {
                try
                {
                    var validarN = contextoFC.ConformacionDeLosPlanos.Where(a => a.OrdenCampo == conformacionDeLosPlanos.OrdenCampo && a.IdConformacionDeLosPlanos == conformacionDeLosPlanos.IdConformacionDeLosPlanos).FirstOrDefault();
                    if (validarN != null)
                    {
                        var OrdenEditar = db.ConformacionDeLosPlanos.Find(conformacionDeLosPlanos.IdConformacionDeLosPlanos);

                        OrdenEditar.NombreCampo = conformacionDeLosPlanos.NombreCampo;
                        OrdenEditar.ValorNulo = conformacionDeLosPlanos.ValorNulo;
                        OrdenEditar.FechaCreacionCampo = DateTime.Now;
                        OrdenEditar.UserControlCampo = conformacionDeLosPlanos.UserControlCampo;
                        OrdenEditar.EstadoCampo = conformacionDeLosPlanos.EstadoCampo;
                        OrdenEditar.ValorPredeterminado = conformacionDeLosPlanos.ValorPredeterminado;
                        OrdenEditar.ContenidoValorPredeterminado = conformacionDeLosPlanos.ContenidoValorPredeterminado;

                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        var idOrdenDos = contextoFC.ConformacionDeLosPlanos.Where(a => a.OrdenCampo == conformacionDeLosPlanos.OrdenCampo && a.IdPlanos == conformacionDeLosPlanos.IdPlanos).FirstOrDefault().IdConformacionDeLosPlanos;
                        var OrdenUno = contextoFC.ConformacionDeLosPlanos.Where(a => a.IdConformacionDeLosPlanos == conformacionDeLosPlanos.IdConformacionDeLosPlanos).FirstOrDefault().OrdenCampo;

                        var OrdenEditarDos = db.ConformacionDeLosPlanos.Find(conformacionDeLosPlanos.IdConformacionDeLosPlanos);
                        OrdenEditarDos.NombreCampo = conformacionDeLosPlanos.NombreCampo;
                        OrdenEditarDos.ValorNulo = conformacionDeLosPlanos.ValorNulo;
                        OrdenEditarDos.OrdenCampo = conformacionDeLosPlanos.OrdenCampo;
                        OrdenEditarDos.FechaCreacionCampo = DateTime.Now;
                        OrdenEditarDos.UserControlCampo = conformacionDeLosPlanos.UserControlCampo;
                        OrdenEditarDos.EstadoCampo = conformacionDeLosPlanos.EstadoCampo;
                        OrdenEditarDos.ValorPredeterminado = conformacionDeLosPlanos.ValorPredeterminado;
                        OrdenEditarDos.ContenidoValorPredeterminado = conformacionDeLosPlanos.ContenidoValorPredeterminado;

                        var OrdenEditarUno = db.ConformacionDeLosPlanos.Find(idOrdenDos);
                        OrdenEditarUno.OrdenCampo = OrdenUno;

                        db.SaveChanges();
                        return true;
                    }

                }
                catch (Exception)
                {
                    return false;
                }
            }

        }
        public bool Eliminar(OrdenDePrioridadPagos ordenDePrioridadPagos, ControlDeMovimientos controlDeMovimientos)
        {
            bool estado = false;

            try
            {
                using (var db = contextoFC)
                {

                    var Prioridad = db.OrdenDePrioridadPagos.Find(ordenDePrioridadPagos.IdOrdenPrioridadPagos);
                    db.OrdenDePrioridadPagos.Remove(Prioridad);

                    controlDeMovimientos.FechaCreacion = DateTime.Now;
                    controlDeMovimientos.UserControl = ordenDePrioridadPagos.UserControlPagos;
                    controlDeMovimientos.Detalles = "USUARIO: " + ordenDePrioridadPagos.UserControlPagos + "/ ELIMINA ORDEN DE PRIORIDAD - DESCUENTOS DE NOMINA.";
                    db.ControlDeMovimientos.Add(controlDeMovimientos);

                    db.SaveChanges();

                    estado = true;
                }
            }
            catch (Exception)
            {
                estado = false;
                //throw;
            }
            return estado;
        }
        public bool EliminarEstructuraPlanos(EstructuraPlanos estructuraPlanos, ControlDeMovimientos controlDeMovimientos)
        {
            bool estado = false;

            try
            {
                using (var db = contextoFC)
                {
                    var Prioridad = db.EstructuraPlanos.Find(estructuraPlanos.IdEstructuraPlanos);
                    db.EstructuraPlanos.Remove(Prioridad);

                    controlDeMovimientos.FechaCreacion = DateTime.Now;
                    controlDeMovimientos.UserControl = estructuraPlanos.UserControlEstructuraPlanos;
                    controlDeMovimientos.Detalles = "USUARIO: " + estructuraPlanos.UserControlEstructuraPlanos + "/ ESTRUCTURA PLANOS - DESCUENTOS DE NOMINA.";
                    db.ControlDeMovimientos.Add(controlDeMovimientos);

                    db.SaveChanges();

                    estado = true;
                }
            }
            catch (Exception)
            {
                estado = false;
                //throw;
            }
            return estado;
        }
        public bool EliminarCampo(ConformacionDeLosPlanos conformacionDeLosPlanos, ControlDeMovimientos controlDeMovimientos)
        {
            bool estado = false;

            try
            {
                using (var db = contextoFC)
                {


                    ConformacionDeLosPlanos conformacion = db.ConformacionDeLosPlanos.Find(conformacionDeLosPlanos.IdConformacionDeLosPlanos);
                    db.ConformacionDeLosPlanos.Remove(conformacion);


                    db.SaveChanges();

                    List<ConformacionDeLosPlanos> lista = db.ConformacionDeLosPlanos.Where(p => p.OrdenCampo > conformacion.OrdenCampo).OrderBy(p => p.OrdenCampo).ToList();
                    foreach (ConformacionDeLosPlanos obj in lista)
                    {
                        var orden = obj.OrdenCampo - 1;
                        obj.OrdenCampo = (short)orden;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    controlDeMovimientos.FechaCreacion = DateTime.Now;
                    controlDeMovimientos.UserControl = conformacionDeLosPlanos.UserControlCampo;
                    controlDeMovimientos.Detalles = "USUARIO: " + conformacionDeLosPlanos.UserControlCampo + "/ ELIMINO CAMPO - DESCUENTOS DE NOMINA.";
                    db.ControlDeMovimientos.Add(controlDeMovimientos);

                    db.SaveChanges();

                    estado = true;
                }
            }
            catch (Exception)
            {
                estado = false;
                //throw;
            }
            return estado;
        }

    }
}