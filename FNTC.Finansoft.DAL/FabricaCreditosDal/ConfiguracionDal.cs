using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.FabricaCreditosDal
{
    public class ConfiguracionDal
    {
        AccountingContext contextoFC = new AccountingContext();

        public List<agencias> obtenerAgencias()
        {
            using (contextoFC)
            {
                return contextoFC.agencias.ToList();
            }
        }//obtenerSedes obtenerUsuario


        public List<FCDependencias> obtenerDependencias()
        {
            using (contextoFC)
            {
                return contextoFC.FCDependencias.ToList();
            }
        }//obtenerSedes
        public List<FCSedes> obtenerSedes()
        {
            using (contextoFC)
            {
                return contextoFC.FCSedes.ToList();
            }
        }//obtenerSedes
        public List<CentralRiesgo> obtenerCentralesRiesgo()
        {
            using (contextoFC)
            {
                return contextoFC.CentralRiesgo.ToList();
            }
        }//obtenerCentralesRiesgo

        public List<Prestamos> obtenerGruposCreditoPorIdLinea(string id)
        {
            var idCreditos = (id);
            using (var idCredito = new AccountingContext())
            {
                return idCredito.Prestamos.Where(g => g.NIT == idCreditos).ToList();
            }
        }//obtenerGruposCreditoPorIdLinea

        public List<FCActividades> obtenerActividades()
        {
            using (contextoFC)
            {
                return contextoFC.FCActividades.ToList();
            }
        }//obtenerActividades

        public bool guardarActividad(FCActividades actividad, string modo)
        {
            using (contextoFC)
            {
                if (modo == "Registrar")
                {
                    contextoFC.FCActividades.Add(actividad);
                }
                if (modo == "Modificar")
                {
                    //actualizar cambiando el estado del modelo recibido 
                    contextoFC.Entry(actividad).State = EntityState.Modified;
                }
                try
                {
                    contextoFC.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }//guardarActividad

        public bool guardarDocumentosPorActividad(List<FCDocumentosActividad> documentos)
        {
            var respuesta = false;
            var idActividad = documentos.FirstOrDefault().idActividad;
            using (contextoFC)
            {
                var actualizar = contextoFC.FCDocumentosActividad.Where(x => x.idActividad == idActividad);
                var cambios = 0;
                foreach (var item in actualizar)
                {
                    contextoFC.FCDocumentosActividad.Remove(item);
                    cambios++;
                }
                try
                {
                    if (cambios != 0) { contextoFC.SaveChanges(); }
                    if (documentos.FirstOrDefault().idDocumento != 0)
                    {
                        foreach (var item in documentos)
                        {
                            contextoFC.FCDocumentosActividad.Add(item);
                        }
                        contextoFC.SaveChanges();
                        respuesta = true;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return respuesta;
        }//guardarDocumentosPorActividad

        public List<FCDocumentos> obtenerDocumentos()
        {
            using (contextoFC)
            {
                return contextoFC.FCDocumentos.ToList();
            }
        }//obtenerDocumentos

        public List<FCDocumentosActividad> obtenerDocumentosPorActividad(string idActividad)
        {
            var idActi = int.Parse(idActividad);
            using (contextoFC)
            {
                return contextoFC.FCDocumentosActividad.Where(x => x.idActividad == idActi).ToList();
            }
        }//obtenerDocumentosPorActividad

        /// <summary>
        /// OPERARIO CONFIGURACIONES
        /// </summary>
        /// <returns></returns>

        public FCConfiguracion obtenerConfiguracionPredeterminada()
        {
            using (contextoFC)
            {
                return contextoFC.FCConfiguracion.Where(c => c.activa == "Si").FirstOrDefault();
            }
        }//obtenerConfiguraciones
        public List<FCMotivosDevolucion> obtenerMotivosDevolucion()
        {
            using (contextoFC)
            {
                return contextoFC.FCMotivosDevolucion.ToList();
            }
        }//obtenerMotivosDevolucion
        public FCActividades obtenerActividadPorIdSolicitud(int id)
        {
            using (var contextoFC = new AccountingContext())
            {
                var idAso = contextoFC.FCSolicitudes.Where(s => s.idSolicitud == id).FirstOrDefault().idActividad;
                return contextoFC.FCActividades.Where(a => a.idActividadAso == idAso).FirstOrDefault();
            }
        }//obtenerActividadPorIdSolicitud

        //ObtenerEX

        public string ObtenerEX(string ruta)
        {
            using (contextoFC)
            {
                var rut = contextoFC.FCDocumentosAsociados.Where(t => t.direccionDocumento == ruta).FirstOrDefault().Extencion;
                if (rut == null) return null;
                return rut;
            }
        }//Obtener
        public string ObtenerEXTENCION(string ruta)
        {
            using (contextoFC)
            {
                var rut = contextoFC.EnvioPlano.Where(t => t.DireccionPlano == ruta).FirstOrDefault().Extencion;
                if (rut == null) return null;
                return rut;
            }
        }//Obtener
        public string ObtenerNombre(string ruta)
        {
            using (contextoFC)
            {
                var Nombre = contextoFC.FCDocumentosAsociados.Where(t => t.direccionDocumento == ruta).FirstOrDefault().NombreDocumento;
                if (Nombre == null) return null;
                return Nombre;
            }
        }//Obtener
        public string ObtenerNombres(string ruta)
        {
            using (contextoFC)
            {
                var Nombre = contextoFC.EnvioPlano.Where(t => t.DireccionPlano == ruta).FirstOrDefault().NombreArchivoPlano;
                if (Nombre == null) return null;
                return Nombre;
            }
        }//Obtener
        public string ObtenerNoIdetificacion(string id)
        {
            using (contextoFC)
            {
                var NoIdentificacion = contextoFC.Prestamos.Where(t => t.Pagare == id).FirstOrDefault().NIT;
                if (NoIdentificacion == null) return null;
                return NoIdentificacion;
            }
        }//Obtener
        public string ObtenerNombreAsociadoCR(string id)
        {
            using (contextoFC)
            {
                var NoIdentificacion = contextoFC.Prestamos.Where(t => t.Pagare == id).FirstOrDefault().NOMBRE;
                if (NoIdentificacion == null) return null;
                return NoIdentificacion;
            }
        }//Obtener
        public int ObtenerIdTotalesCre(string id)
        {
            using (contextoFC)
            {
                var IdTotales = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().Id;
                if (IdTotales == 0) return 0;
                return IdTotales;
            }
        }//Obtener
        public string ObtenerPagareCR(int id)
        {
            using (contextoFC)
            {
                var pagareCR = contextoFC.TotalesCreditos.Where(t => t.Id == id).FirstOrDefault().Pagare;
                if (pagareCR == null) return null;
                return pagareCR;
            }
        }//Obtener
        public long ObtenerVTDelPrestamo(string id)
        {
            using (contextoFC)
            {
                var VTDelPrestamo = contextoFC.Prestamos.Where(t => t.Pagare == id).FirstOrDefault().Capital;
                if (VTDelPrestamo == 0) return 0;
                return VTDelPrestamo;
            }
        }//Obtener
        public decimal ObtenerPlazoCR(string id)
        {
            using (contextoFC)
            {
                var VTDelPrestamo = contextoFC.Prestamos.Where(t => t.Pagare == id).FirstOrDefault().Plazo;
                if (VTDelPrestamo == 0) return 0;
                return VTDelPrestamo;
            }
        }//Obtener
        public int ObtenerIdPrestamoCR(string id)
        {
            using (contextoFC)
            {
                var idPrest = contextoFC.Prestamos.Where(t => t.Pagare == id).FirstOrDefault().id;
                if (idPrest == 0) return 0;
                return idPrest;
            }
        }//Obtener
        public decimal ObtenerCapitalTotalCR(string id)
        {
            using (contextoFC)
            {
                var VTDelPrestamo = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().CapitalTotal;
                if (VTDelPrestamo == 0) return 0;
                return VTDelPrestamo;
            }
        }//Obtener
        public string ObtenerEstadoCredito(string id)
        {
            using (contextoFC)
            {
                var Estadocre = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().Estado;
                if (Estadocre == null) return null;
                return Estadocre;
            }
        }//Obtener
        public decimal ObtenerSaldoCapitalCR(string id)
        {
            using (contextoFC)
            {
                var VTDelPrestamo = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().SaldoCapital;
                if (VTDelPrestamo == 0) return 0;
                return VTDelPrestamo;
            }
        }//Obtener
        public decimal ObtenerTotalInteCorrie(string id)
        {
            using (contextoFC)
            {
                var VTDelPrestamo = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().InteresCorrienteTotal;
                if (VTDelPrestamo == 0) return 0;
                return VTDelPrestamo;
            }
        }//Obtener

        public decimal ObtenerIntereMoraTotal(string id)
        {
            using (contextoFC)
            {
                var InteMTotal = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().InteresMoraTotal;
                if (InteMTotal == 0) return 0;
                return InteMTotal;
            }
        }//Obtener
        public decimal ObtenerIntereMoraPendiente(string id)
        {
            using (contextoFC)
            {
                var intemora = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().InteresMoraPendiente;
                if (intemora == 0) return 0;
                return intemora;
            }
        }//Obtener
        public int ObtenerDiasMora(string id)
        {
            using (contextoFC)
            {
                var dias = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().DiasMora;
                if (dias == 0) return 0;
                return dias;
            }
        }//Obtener
        public string ObtenerEstadoCre(string id)
        {
            using (contextoFC)
            {
                var estado = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().Estado;
                if (estado == null) return null;
                return estado;
            }
        }//Obtener

        public DateTime ObtenerFechaProximoPago(string id)
        {
            using (contextoFC)
            {
                var fechaRegistrob = DateTime.Now;
                var VTDelPrestamo = contextoFC.TotalesCreditos.Where(t => t.Pagare == id).FirstOrDefault().FechaProximoPago;
                if (VTDelPrestamo == null) return fechaRegistrob;
                return VTDelPrestamo;
            }
        }//Obtener

        public string ObtenerNombresAsociado(string id)
        {
            using (contextoFC)
            {
                var persona = contextoFC.Terceros.Where(t => t.NIT == id).FirstOrDefault();
                if (persona == null) return null;
                var personaEncontrada = persona.NOMBRE1 + " " + persona.NOMBRE2 + " " + persona.APELLIDO1 + " " + persona.APELLIDO2;
                return personaEncontrada;
            }
        }//ObtenerNombresAsociado
        public string ObtenerNombreAsociadoPrestamos(string id)
        {
            using (contextoFC)
            {
                var PrestamosData = contextoFC.Prestamos.Where(t => t.Pagare == id).FirstOrDefault().Pagare;
                if (PrestamosData == null) return null;
                var validador = contextoFC.ValidarHuella.Where(t => t.Pagare == PrestamosData && t.TipoDeUsuario == "Asociado").FirstOrDefault();
                if (validador == null)
                {
                    return null;
                }
                var nomaso = "";
                nomaso = validador.Pagare;
                var NombreAso = contextoFC.Prestamos.Where(t => t.Pagare == nomaso).FirstOrDefault().NOMBRE;
                if (NombreAso == null) return null;
                return NombreAso;
            }
        }//ObtenerNombresAsociado
        public string ObtenerTokenAsociado(string id)
        {
            using (contextoFC)
            {
                var PrestamosData= "NULL";
                PrestamosData = contextoFC.Prestamos.Where(t => t.Pagare == id).FirstOrDefault().Pagare;
                var Tokenaso = contextoFC.ValidarHuella.Where(t => t.Pagare == PrestamosData && t.TipoDeUsuario == "Asociado").FirstOrDefault();
                if (Tokenaso == null)
                {
                    return null;
                }
                var Token = "";
                 Token = Tokenaso.Token;
                return Token;
            }
        }//ObtenerNombresAsociado
        public string ObtenerTokenCodeudor(string id)
        {
            using (contextoFC)
            {
                var PrestamosDatas = "NULL";
                 PrestamosDatas = contextoFC.Prestamos.Where(t => t.Pagare == id).FirstOrDefault().Pagare;
                
                var tokencode = contextoFC.ValidarHuella.Where(t => t.Pagare == PrestamosDatas && t.TipoDeUsuario == "Codeudor").FirstOrDefault();
                if (tokencode == null)
                {

                    return null;
                };
                var toto = tokencode.Token;
                return toto;
            }
        }//ObtenerNombresAsociado
        public string ObtenerNombreCodeudor(string id) 
        {
            using (contextoFC)
            {
               
                var PrestamosDataN = contextoFC.Prestamos.Where(f => f.Pagare == id).FirstOrDefault().Pagare;
                
                var validador = contextoFC.ValidarHuella.Where(w => w.Pagare == PrestamosDataN && w.TipoDeUsuario == "Codeudor").FirstOrDefault();
                if (validador == null)
                {
                    return null;
                }
                var ValidarC = validador.IdCedula; 
                var CedulaCode = contextoFC.UserH.Where(t => t.Id == ValidarC).FirstOrDefault().Cedula;
                if (CedulaCode == null)
                {
                    return null;
                }
                var nombrecode = contextoFC.Terceros.Where(t => t.NIT == CedulaCode).FirstOrDefault();
                var NombreCompletoCode = nombrecode.NOMBRE1  + " " + nombrecode.APELLIDO1 ;
                return NombreCompletoCode;

            }
        }//ObtenerNombresAsociado


        public string ObtenerNombreDelAsociado(int id)
        {
            using (contextoFC)
            {
                var nit = contextoFC.FCSolicitudes.Where(t => t.idSolicitud == id).FirstOrDefault().idAsociado;
                if (nit == 0) return null;

                int numEntero = nit;
                string numCadena = numEntero + "";

                var persona = contextoFC.Terceros.Where(t => t.NIT == numCadena).FirstOrDefault();
                if (persona == null) return null;
                var personaEncontrada = persona.NOMBRE1 + " " + persona.NOMBRE2 + " " + persona.APELLIDO1 + " " + persona.APELLIDO2;
                return personaEncontrada;

            }
        }//ObtenerNombresAsociado
        public string ObtenerPermiso(string id)
        {
            using (contextoFC)
            {
                var user = contextoFC.AspNetUsersApp.Where(r => r.UserName == id).FirstOrDefault().Id;
                var us = contextoFC.FCRolesUsuario.Where(s => s.NIT_Usuario == user).FirstOrDefault().Rol_Operario;
                if (us == null) return null;
                return us;

            }
        }//Obtener
        public string ObtenerPermisoA(string id)
        {
            using (contextoFC)
            {
                var user = contextoFC.AspNetUsersApp.Where(r => r.UserName == id).FirstOrDefault().Id;
                var us = contextoFC.FCRolesUsuario.Where(s => s.NIT_Usuario == user).FirstOrDefault().Rol_Analista;
                if (us == null) return null;
                return us;

            }
        }//Obtener
        public string ObtenerEstado(int id)
        {
            using (contextoFC)
            {
                var estad = contextoFC.FCSolicitudes.Where(r => r.idSolicitud == id).FirstOrDefault().estado;
                if (estad == null) return null;
                return estad;

            }
        }//Obtener
        public string ObtenerPreEstado(int id)
        {
            using (contextoFC)
            {
                var estadPre = contextoFC.FCSolicitudes.Where(r => r.idSolicitud == id).FirstOrDefault().PreEstado;
                if (estadPre == null) return null;
                return estadPre;

            }
        }//Obtener
        public string ObtenerEstadoRC(int id)
        {
            using (contextoFC)
            {

                var estadPres = contextoFC.FCPasosAp.Where(r => r.idsolicitud == id).FirstOrDefault().EstadoAnRC;
                if (estadPres == null) return null;
                return estadPres;

            }
        }//Obtener
        public string ObtenerEstadoDoc(int id)
        {
            using (contextoFC)
            {

                var estadP = contextoFC.FCPasosAp.Where(r => r.idsolicitud == id).FirstOrDefault().EstadoAnDoc;
                if (estadP == null) return null;
                return estadP;

            }
        }//Obtener
        public string ObtenerEstados(int id)
        {
            using (contextoFC)
            {

                var estados = contextoFC.FCSolicitudes.Where(r => r.idSolicitud == id).FirstOrDefault().estado;
                if (estados == null) return null;
                return estados;

            }
        }//Obtener
        public string ObtenerEstadoAnRC(int id)
        {
            using (contextoFC)
            {
                var estadoRC = contextoFC.FCPasosAp.Where(r => r.idsolicitud == id).FirstOrDefault().EstadoAnRC;
                if (estadoRC == null) return null;
                return estadoRC;

            }
        }//Obtener
        public string ObtenerEstadoAnDoc(int id)
        {
            using (contextoFC)
            {
                var estadoDoc = contextoFC.FCPasosAp.Where(r => r.idsolicitud == id).FirstOrDefault().EstadoAnDoc;
                if (estadoDoc == null) return null;
                return estadoDoc;

            }
        }//Obtener
        public string ObtenerPermisoE(string id)
        {
            using (contextoFC)
            {
                var user = contextoFC.AspNetUsersApp.Where(r => r.UserName == id).FirstOrDefault().Id;
                var us = contextoFC.FCRolesUsuario.Where(s => s.NIT_Usuario == user).FirstOrDefault().Rol_Ente;
                if (us == null) return null;
                return us;

            }
        }//Obtener
        public string ObtenerPermisoI(string id)
        {
            using (contextoFC)
            {
                var user = contextoFC.AspNetUsersApp.Where(r => r.UserName == id).FirstOrDefault().Id;
                var us = contextoFC.FCRolesUsuario.Where(s => s.NIT_Usuario == user).FirstOrDefault().Rol_Informativo;
                if (us == null) return null;
                return us;

            }
        }//Obtener
        public int ObtenerDependencia(string useractual)
        {
            using (contextoFC)
            {
                var idDep = contextoFC.AspNetUsersApp.Where(r => r.UserName == useractual).FirstOrDefault().Id;
                var id = contextoFC.FCRolesUsuario.Where(s => s.NIT_Usuario == idDep).FirstOrDefault().IdDependencia;
                if (id == 0) return 0;
                return id;

            }
        }//Obtener
        public string Permiso(string id)
        {
            using (contextoFC)
            {
                var username = contextoFC.AspNetUsersApp.Where(r => r.UserName == id).FirstOrDefault().Id;
                if (username == null) return null;
                var us = contextoFC.FCRolesUsuario.Where(s => s.NIT_Usuario == username).FirstOrDefault();
                if (us == null)
                {
                    return null;
                };
                var UserS = us.NIT_Usuario;
                var usa = contextoFC.AspNetUsersApp.Where(a => a.Id == UserS).FirstOrDefault().UserName;
                if (usa == null) return null;
                return usa;

            }
        }//ObtenerAutorizacion
        public string ObtenerAutorizacion(int id)
        {
            using (contextoFC)
            {
                var Auto = contextoFC.FCSolicitudes.Where(r => r.idPrestamo == id).FirstOrDefault();

                if (Auto == null)
                {
                    return null;
                }
                var Autor = Auto.estado;
                return Autor;


            }
        }//ObtenerAutorizacion
        public int ObtenerValorPrestamo(int id)
        {
            using (contextoFC)
            {
                var presoli = contextoFC.FCSolicitudes.Where(t => t.idSolicitud == id).FirstOrDefault().idPrestamo;
                if (presoli == 0) return 0;
                var PrestaValor = contextoFC.Prestamos.Where(r => r.id == presoli).FirstOrDefault().Capital;
                if (PrestaValor == 0) return 0;
                int capital = Convert.ToInt32(PrestaValor);
                return capital;

            }
        }//Obtener
        public int ObtenerSalario(int id)
        {
            using (contextoFC)
            {
                var Salario = contextoFC.FCSolicitudes.Where(t => t.idSolicitud == id).FirstOrDefault().Salario;
                if (Salario == 0) return 0;
                int number = Decimal.ToInt32(Salario);
                return number;

            }
        }//Obtener
        public int ObtenerMin(int id)
        {
            using (contextoFC)
            {
                var min = contextoFC.FCSolicitudes.Where(t => t.idSolicitud == id).FirstOrDefault().idDependencia;
                if (min == 0) return 0;
                var Mini = contextoFC.FCDependencias.Where(r => r.idDependencia == min).FirstOrDefault().montoMinimo;
                if (Mini == 0) return 0;
                int minimo = Convert.ToInt32(Mini);
                return minimo;

            }
        }//Obtener
        public int ObtenerMax(int id)
        {
            using (contextoFC)
            {
                var max = contextoFC.FCSolicitudes.Where(t => t.idSolicitud == id).FirstOrDefault().idDependencia;
                if (max == 0) return 0;
                var Mxa = contextoFC.FCDependencias.Where(r => r.idDependencia == max).FirstOrDefault().montoMaximo;
                if (Mxa == 0) return 0;
                int maximo = Convert.ToInt32(Mxa);
                return maximo;

            }
        }//Obtener
        public int ObtenerAsociado(int id)
        {
            using (contextoFC)
            {
                var Asocia = contextoFC.FCSolicitudes.Where(t => t.idSolicitud == id).FirstOrDefault().idAsociado;
                if (Asocia == 0) return 0;
                return Asocia;

            }
        }//Obtener
        public int ObtenerSolicitudE(int id)
        {
            using (contextoFC)
            {
                var IdSolid = contextoFC.FCDocumentosAsociados.Where(t => t.idDoc == id).FirstOrDefault().idSolicitud;
                if (IdSolid == 0) return 0;
                return IdSolid;

            }
        }//Obtener
        public string ObtenerNombreAso(int idVerific)
        {
            using (contextoFC)
            {
                var idaso = contextoFC.FCSolicitudes.Where(t => t.IdVerificacionAso == idVerific).FirstOrDefault().idAsociado;
                if (idaso == 0) return null;

                int numEntero = idaso;
                string numCadena = numEntero + "";

                var persona = contextoFC.Terceros.Where(t => t.NIT == numCadena).FirstOrDefault();
                if (persona == null) return null;
                var personaEncontrada = persona.NOMBRE1 + " " + persona.NOMBRE2 + " " + persona.APELLIDO1 + " " + persona.APELLIDO2;
                return personaEncontrada;


            }
        }//Obtener
        public string ObtenerNombreRefe(int IDREFCODE)
        {
            using (contextoFC)
            {
                var persona = contextoFC.DataReferenciasCodeudorFC.Where(t => t.IDREFCODE == IDREFCODE).FirstOrDefault();
                if (persona == null) return null;
                var personaEncontrada = persona.NOMBRE1 + " " + persona.NOMBRE2 + " " + persona.APELLIDO1 + " " + persona.APELLIDO2;
                return personaEncontrada;


            }
        }//Obtener
        public int ObtenerSolicitudAso(int idVerific)
        {
            using (contextoFC)
            {
                var idaso = contextoFC.FCSolicitudes.Where(t => t.IdVerificacionAso == idVerific).FirstOrDefault().idSolicitud;
                if (idaso == 0) return 0;
                return idaso;


            }
        }//Obtener
        public int ObtenerIdSoliA(int IDREFCODE)
        {
            using (contextoFC)
            {
                var idaso = contextoFC.DataReferenciasCodeudorFC.Where(t => t.IDREFCODE == IDREFCODE).FirstOrDefault().IDSOLICITUD;
                if (idaso == 0) return 0;
                return idaso;


            }
        }//Obtener
        public int ObteneridAso(int idVerific)
        {
            using (contextoFC)
            {
                var idaso = contextoFC.FCSolicitudes.Where(t => t.IdVerificacionAso == idVerific).FirstOrDefault().idAsociado;
                if (idaso == 0) return 0;
                return idaso;

            }
        }//Obtener
        public DateTime ObtenerFechaSol(int idVerific)
        {
            using (contextoFC)
            {
                var fechasol = contextoFC.FCSolicitudes.Where(t => t.IdVerificacionAso == idVerific).FirstOrDefault().fechaRegistro;
                return fechasol;

            }
        }//Obtener
        public int ObtenerSolicitud(int id)
        {
            using (contextoFC)
            {
                var Sol = contextoFC.DataReferenciasCodeudorFC.Where(t => t.IDREFCODE == id).FirstOrDefault().IDSOLICITUD;
                if (Sol == 0) return 0;
                return Sol;

            }
        }//Obtener
        //ObtenerAsociado

        public int ObtenerNoCuotasPrestamo(int id)
        {
            using (contextoFC)
            {
                var presoli = contextoFC.FCSolicitudes.Where(t => t.idSolicitud == id).FirstOrDefault().idPrestamo;
                if (presoli == 0) return 0;
                var PrestaValor = contextoFC.Prestamos.Where(r => r.id == presoli).FirstOrDefault().Plazo;
                if (PrestaValor == 0) return 0;
                int plazo = Convert.ToInt32(PrestaValor);
                return plazo;

            }
        }//Obtener
        public string ObtenerNombreDependencia(int id)
        {
            using (contextoFC)
            {
                var depe = contextoFC.FCSolicitudes.Where(t => t.idSolicitud == id).FirstOrDefault().idDependencia;
                if (depe == 0) return null;
                var depende = contextoFC.FCDependencias.Where(r => r.idDependencia == depe).FirstOrDefault().nombreDependencia;
                if (depende == null) return null;
                return depende;

            }
        }//Obtener

        public string ObtenerNombresMunicipio(string id)
        {
            using (contextoFC)
            {
                var persona = contextoFC.Terceros.Where(t => t.NIT == id && t.ESASOCIADO == 1).FirstOrDefault();
                if (persona == null) return null;
                var personaEncontrada = persona.TEL;
                return personaEncontrada;
            }
        }//ObtenerNombresMunicipio

        public bool TieneSolicitudesPendientes(string id)
        {
            var idAso = int.Parse(id);

            var solicitudesPendientes = false;
            using (contextoFC)
            {
                //Una solicitud por asociado
                var SolicitudesAso = contextoFC.FCSolicitudes.Where(s => s.idAsociado == idAso).ToList();
                if (SolicitudesAso == null) return false;
                foreach (var sol in SolicitudesAso)
                {
                    if (sol.estado.Equals("Pendiente ") || sol.estado.Equals("Fase-N1-Op") || sol.estado.Equals("Fase-N2-An")) solicitudesPendientes = true;
                }
                return solicitudesPendientes;
            }
        }//VerificarEstadoAsociado 

        public List<FCConfiguracion> obtenerConfiguraciones()
        {
            using (contextoFC)
            {
                return contextoFC.FCConfiguracion.ToList();
            }
        }//obtenerConfiguraciones

        private string AsignarAnalista()
        {
            //var contextoFC = new ContextoFabricaCreditos();
            using (SqlConnection con = new SqlConnection(contextoFC.Database.Connection.ConnectionString))
            {
                con.Open();
                //Quien esta desocupado ? XD
                var ConsultaSql = "select idEmpleado from FabricaCreditosOperarios where tieneTrabajo = 0 and analista = 1";
                var cmd = new SqlCommand(ConsultaSql, con);
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        var id = reader[0];
                        return id.ToString();
                    }
                    // quien tiene menos tareas?
                    ConsultaSql = "SELECT idOperarioAnalista, count(idOperarioAnalista) as NTareas FROM FabricaCreditosTareas where estadoAnalista = 'Pendiente' group by idOperarioAnalista order by NTareas";
                    cmd = new SqlCommand(ConsultaSql, con);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        var id = reader[0];
                        return id.ToString();
                    }
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
            return 0.ToString();
        }


    }
}
