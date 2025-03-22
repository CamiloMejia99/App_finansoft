using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.BLL.Caja;
using FNTC.Finansoft.Accounting.BLL.Movimientos;
using FNTC.Finansoft.Accounting.BLL.ProcesosCrediticios;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.TercerosOtrasEntidades;
using FNTC.Finansoft.Areas.Aportes.Controllers;
using FNTC.Finansoft.UI.Areas.Terceros.Controllers;
using FNTC.Framework.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace FNTC.Finansoft.UI.Areas.OperativaDeCaja.Controllers
{
    
    [Authorize]
    public class FactOpcajasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: FactOpcajas
        public ActionResult Index(string fechaDesde,string nit)
        {

            //inicio select list para terceros
            List<SelectListItem> terceros = new List<SelectListItem>();
            terceros.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<Tercero> listadoTerceros = db.Terceros.ToList();


            foreach (var item in listadoTerceros)
            {
                terceros.Add(new SelectListItem { Text = item.NIT+" | "+ item.NOMBRE1 + " " + item.NOMBRE2+" "+item.APELLIDO1+" "+item.APELLIDO2, Value = item.NIT });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin select list para terceros
            ViewBag.terceros = terceros;

            DateTime fecha = DateTime.Now;
            var fac = new List<FactOpcaja>();
            
            var factOpcaja = db.FactOpcaja;
            if (Session["nit"+User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                if(nit!="" && nit!=null && fechaDesde != null && fechaDesde != "")
                {
                    fecha = Convert.ToDateTime(fechaDesde);
                    fac = db.FactOpcaja.Where(p => p.fecha.Year == fecha.Year && p.fecha.Month == fecha.Month && p.fecha.Day == fecha.Day && p.nit_propietario_cuenta==nit).OrderBy(x => x.id).ToList();

                }
                else if((nit!="" && nit!=null ) && (fechaDesde == null || fechaDesde == ""))
                {
                    fac = db.FactOpcaja.Where(p => p.nit_propietario_cuenta == nit).OrderBy(x => x.id).ToList();
                }else if((nit == "" || nit == null) && (fechaDesde != null && fechaDesde != ""))
                {
                    fecha = Convert.ToDateTime(fechaDesde);
                    fac = db.FactOpcaja.Where(p => p.fecha.Year == fecha.Year && p.fecha.Month == fecha.Month && p.fecha.Day == fecha.Day).OrderBy(x => x.id).ToList();
                }

                return View(fac);
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        public ActionResult facturasDesembolso()
        {
            var factOpCajaDesembolsos = db.factOpCajaDesembolso;
            if (Session["nit"+User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                return View(factOpCajaDesembolsos.ToList());
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        public ActionResult facturasCuotasCreditos()
        {
            var factOpCajaConsCuotaCredito = db.factOpCajaConsCuotaCredito;
            if (Session["nit"+User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                return View(factOpCajaConsCuotaCredito.ToList());
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        public ActionResult facturasCuotasCreditosEntidadDos()
        {
            var factOpCajaConsCuotaCreditoEntidadDos = db.factOpCajaConsCuotaCreditoEntidadDos;
            if (Session["nit" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                return View(factOpCajaConsCuotaCreditoEntidadDos.ToList());
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        public ActionResult Cuentas()
        {
            var cuentasAhorro = db.FichasAhorros.Where(f => f.tipoPago == "Caja");
            return View(cuentasAhorro.ToList());
        }
        public ActionResult CuentaAportes()
        {
            var cuentasAporte = db.FichasAportes.Where(f => f.tipoPago == "Caja");
            return View(cuentasAporte.ToList());
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactOpcaja factOpcaja = db.FactOpcaja.Find(id);
            if (factOpcaja == null)
            {
                return HttpNotFound();
            }

            //obtenemos los movimientos adicionales a caja y la cuenta configurada para aportes ordinarios
            var movimientos = db.Movimientos.Where(x => x.TIPO == factOpcaja.TIPO && x.NUMERO == factOpcaja.NUMERO).ToList();
            if(movimientos.Count() >0)
            //movimientos.RemoveRange(1, 1);//se elimina las cuentas de cuenta de caja y la de aportes y se deja las demás
            ViewBag.movimientos = movimientos;
            return View(factOpcaja);



        }

        private void EnviarCorreo(string destinatario, string asunto, string cuerpoHtml)
        {
            try
            {
                MailMessage ms = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                ms.From = new MailAddress("loantech99@gmail.com");
                ms.To.Add(new MailAddress(destinatario));

                ms.Subject = asunto;

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(cuerpoHtml, Encoding.UTF8, MediaTypeNames.Text.Html);
                ms.AlternateViews.Add(htmlView);

                smtp.Host = "smtp.gmail.com"; // Servidor SMTP
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("loantech99@gmail.com", "Facil1234*");
                smtp.EnableSsl = true;

                smtp.Send(ms);
                Console.WriteLine("Correo enviado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
        public ActionResult CreateFactura(FactOpcaja factOpcaja)
        {
            if (ModelState.IsValid)
            {
                db.FactOpcaja.Add(factOpcaja);
                db.SaveChanges();

                // Enviar correo de confirmación
                string destinatario = "camilomp100@gmail.com"; // Puedes obtenerlo de la base de datos
                string asunto = "Factura Generada";
                string cuerpoHtml = $"<h1>Detalles de Factura</h1><p>Factura N°: {factOpcaja.id}</p>";

                EnviarCorreo(destinatario, asunto, cuerpoHtml);

                return RedirectToAction("Details", new { id = factOpcaja.id });
            }
            return View(factOpcaja);
        }


        public ActionResult DetalleFacturaAhorroContractual(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactOpcaja factOpcaja = db.FactOpcaja.Find(id);
            if (factOpcaja == null)
            {
                return HttpNotFound();
            }

            return View(factOpcaja);
        }

        public ActionResult DetailsRet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactOpcaja factOpcaja = db.FactOpcaja.Find(id);
            if (factOpcaja == null)
            {
                return HttpNotFound();
            }
            return View(factOpcaja);
        }

        public ActionResult DetailsDesembolso(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factOpCajaDesembolsos factOpCajaDesembolsos = db.factOpCajaDesembolso.Find(id);
            if (factOpCajaDesembolsos == null)
            {
                return HttpNotFound();
            }
            return View(factOpCajaDesembolsos);
        }

        public ActionResult DetailsConsCuotaCredito(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factOpCajaConsCuotaCredito factOpCajaConsCuotaCredito = db.factOpCajaConsCuotaCredito.Find(id);
            if (factOpCajaConsCuotaCredito == null)
            {
                return HttpNotFound();
            }
            return View(factOpCajaConsCuotaCredito);
        }

        public ActionResult DetailsConsAbonoCredito(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factOpCajaConsCuotaCredito factOpCajaConsCuotaCredito = db.factOpCajaConsCuotaCredito.Find(id);
            if (factOpCajaConsCuotaCredito == null)
            {
                return HttpNotFound();
            }
            return View(factOpCajaConsCuotaCredito);
        }

        public ActionResult DetailsConsCuotaCreditoEntidadDos(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factOpCajaConsCuotaCreditoEntidadDos factOpCajaConsCuotaCreditoEntidadDos = db.factOpCajaConsCuotaCreditoEntidadDos.Find(id);
            if (factOpCajaConsCuotaCreditoEntidadDos == null)
            {
                return HttpNotFound();
            }
            return View(factOpCajaConsCuotaCreditoEntidadDos);
        }

        public ActionResult DetailsRetCheque(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactOpcaja factOpcaja = db.FactOpcaja.Find(id);
            if (factOpcaja == null)
            {
                return HttpNotFound();
            }
            return View(factOpcaja);
        }


        public ActionResult seleccionCuentaAhorro(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FichasAhorros fichasAhorros = db.FichasAhorros.Find(id);
            if (fichasAhorros == null)
            {
                return HttpNotFound();
            }
            else
            {
                Session["Seleccionada" + User.Identity.Name] = fichasAhorros.numeroCuenta;
                return RedirectToAction("CuentaOperacion");
            }
            //return View(fichasAhorros);
        }

        public ActionResult seleccionCuentaAportes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FichasAportes fichasAportes = db.FichasAportes.Find(id);
            if (fichasAportes == null)
            {
                return HttpNotFound();
            }
            else
            {
                Session["Seleccionada" + User.Identity.Name] = fichasAportes.numeroCuenta;
                return RedirectToAction("CuentaOperacion");
            }
            //return View(fichasAhorros);
        }

        public JsonResult GetAporteNumero(string nitAsociado,int id)
        {
            int contador = 1;
            int numeroCuota = 1;
            var aportes = db.FactOpcaja.Where(x => x.nit_propietario_cuenta == nitAsociado).OrderBy(x=>x.fecha).ToList();
            foreach(var item in aportes)
            {
                if (item.id == id)
                {
                    numeroCuota = contador;
                }
                contador++;
            }

            //var AporteNumero = (from pc in db.FactOpcaja where pc.nit_propietario_cuenta == nitAsociado select pc).Count();
            return Json(numeroCuota, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Logopcaja()
        {
            var fech = DateTime.Now.Date;
            Session["fecha" + User.Identity.Name] = fech.ToString("yyyy-MM-dd");

            Session["nit" + User.Identity.Name] = null;

            //manejaFechas56987545454
            DateTime localDateTime = DateTime.Now;
            localDateTime = localDateTime.ToLocalTime();
            var otra = localDateTime.ToString();

            CultureInfo Colombia = CultureInfo.CreateSpecificCulture("es-CO");
            var hora = Convert.ToDateTime(otra).ToString(Colombia.DateTimeFormat);

            return View();
        }
        // POST: FactOpcajas/Create
        [HttpPost]
        [Authorize]
        public ActionResult Logopcaja(string nit, string fecha1)
        {
            if (nit == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configCajero configCajero = db.configCajero.Find(nit); //consultamos cajero registrado
            if (configCajero == null)
            {
                ViewBag.mensaje = "Nit no encontrado ";
                //return HttpNotFound();
            }
            else if(nit != User.Identity.Name)
            {
                ViewBag.mensaje = "El usuario ingresado no coincide con el usuario que ha iniciado sesion";
            }
            else
            {
                //Valores de configuracion Cajero
                Session["nit"+User.Identity.Name] = configCajero.Nit_cajero;
                Session["cod_caja" + User.Identity.Name] = configCajero.Codigo_caja;
                Session["cta_cheques" + User.Identity.Name] = configCajero.Cta_cheque;
                Session["cta_efectivo" + User.Identity.Name] = configCajero.Cta_efectivo;
                Session["comp_ingreso" + User.Identity.Name] = configCajero.Compr_ingreso;
                Session["comp_egreso" + User.Identity.Name] = configCajero.Compr_egreso;
                Session["cc_transacciones" + User.Identity.Name] = configCajero.centrocosto;

                string nnnn = Session["nit" + User.Identity.Name].ToString();
                string codCaja = Session["cod_caja" + User.Identity.Name].ToString();
                Caja caja = db.Caja.Where(x => x.Codigo_caja == codCaja).FirstOrDefault();//consultamos la Caja a la cual pertenece el cajero 
                if (caja == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    //datos de Caja
                    Session["nombre_caja" + User.Identity.Name] = caja.Nombre_caja;
                    Session["actual" + User.Identity.Name] = caja.consecutivo_actual;
                    Session["tope_maximo" + User.Identity.Name] = caja.TopeMaximo_caja;
                    Session["con_fin" + User.Identity.Name] = caja.consecutivo_fin;
                    Session["con_ini" + User.Identity.Name] = caja.Consecutivo_ini;
                    Session["serie" + User.Identity.Name] = caja.Serie;
                    Session["agencia" + User.Identity.Name] = caja.agencia;

                    Tercero terceros = db.Terceros.Find(Session["nit"] + User.Identity.Name);// identificamos el nombre del cajero
                    if (terceros == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        string sessioncod_caja = Session["cod_caja" + User.Identity.Name].ToString();
                        string sessionnit = Session["nit" + User.Identity.Name].ToString();

                        var cuadre = (from pc in db.CuadreCajaPorCajero where pc.fecha == fecha1 && pc.codigo_caja == sessioncod_caja && pc.nit_cajero == sessionnit && pc.cierre == 0 select pc).Count();
                        if (cuadre == 0)
                        {
                            //creamos el registro en la tabla cuadre parcial de caja con la fecha y el codigo de caja para evitar repetidos
                            //var insertcuadre1 = "INSERT INTO dbo.CuadreCajaPorCajero (fecha, codigo_caja, nit_cajero, retiros_efectivo, retiros_cheque, consignacion_efectivo, consignacion_cheque, cierre,horacierre,tope)" +
                            //                    "VALUES('" + fecha1 + "','" + Session["cod_caja" + User.Identity.Name] + "','" + Session["nit" + User.Identity.Name] + "',0,0,0,0,0,''," + Session["tope_maximo" + User.Identity.Name] + ")";
                            //db.Database.ExecuteSqlCommand(insertcuadre1);
                            var cuadreCajaPorCajero = new CuadreCajaPorCajero()
                            {

                                fecha = fecha1,
                                codigo_caja = Session["cod_caja"+User.Identity.Name].ToString(),
                                nit_cajero = Session["nit" + User.Identity.Name].ToString(),
                                retiros_efectivo = 0,
                                retiros_cheque = 0,
                                consignacion_efectivo = 0,
                                consignacion_cheque = 0,
                                cierre = 0,
                                horacierre = new DateTime(1900,1,1),
                                tope = 0

                            };

                            db.CuadreCajaPorCajero.Add(cuadreCajaPorCajero);
                            db.SaveChanges();
                            // para cuader parcial de caja

                            Session["nombre" + User.Identity.Name] = terceros.NOMBRE1 + " " + terceros.NOMBRE2 + " " + terceros.APELLIDO1 + " " + terceros.APELLIDO2;

                            ViewBag.nombre = Session["nombre" + User.Identity.Name];

                            return RedirectToAction("CuentaOperacion");
                        }
                        else
                        {
                            // se extrae los valores de cuadre parciade caja
                            Session["IDcuadre" + User.Identity.Name] = (from pc in db.CuadreCajaPorCajero where pc.fecha == fecha1 && pc.codigo_caja == sessioncod_caja && pc.nit_cajero == sessionnit && pc.cierre == 0 select pc.id).Single();
                            Session["nombre" + User.Identity.Name] = terceros.NOMBRE1 + " " + terceros.NOMBRE2 + " " + terceros.APELLIDO1 + " " + terceros.APELLIDO2;

                            ViewBag.nombre = Session["nombre" + User.Identity.Name];
                            return RedirectToAction("CuentaOperacion");
                        }
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddTerceroEntidadDos(string nombreAso, string cedulaAso, string cuota)
        {
            ListaTercerosEntidadDos = new TercerosController().ConsultartercerosEntidadDos().ToList().Select(p => new SelectListItem { Text = p.nombre + " || " + p.cedula, Value = p.cedula, Selected = false }); ;



            ViewBag.ListaTercerosEntidadDos = ListaTercerosEntidadDos;

            var terceroEntidadDosCount = (from pc in db.tercerosEntidadDos where pc.cedula == cedulaAso select pc).Count();
            if (terceroEntidadDosCount != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
            var terceroEntidadDos = new tercerosEntidadDos()
            {
                nombre = nombreAso,
                cedula = cedulaAso,
                cuota = cuota
            };
            db.tercerosEntidadDos.Add(terceroEntidadDos);
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CuentaOperacionEntidadDos()
        {
            ListaTercerosEntidadDos = new TercerosController().ConsultartercerosEntidadDos().ToList().Select(p => new SelectListItem { Text = p.nombre + " || " + p.cedula, Value = p.cedula, Selected = false }); ;



            ViewBag.ListaTercerosEntidadDos = ListaTercerosEntidadDos;

            var fech = DateTime.Now.Date;
            Session["fecha" + User.Identity.Name] = fech.ToString("yyyy-MM-dd");
            if (Session["nit" + User.Identity.Name] != null && Session["IDcuadre" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                Caja caja = db.Caja.Find(Session["cod_caja" + User.Identity.Name]);

                Session["actual" + User.Identity.Name] = caja.consecutivo_actual;
                Session["Serie" + User.Identity.Name] = caja.Serie;
                Session["agencia" + User.Identity.Name] = caja.agencia;
                Session["tope_maximo" + User.Identity.Name] = caja.TopeMaximo_caja;
                Session["Factura" + User.Identity.Name] = Session["agencia" + User.Identity.Name] + "-" + Session["cod_caja" + User.Identity.Name] + "-" + Session["Serie" + User.Identity.Name] + "-" + Session["actual" + User.Identity.Name];
                Session["CUENTA" + User.Identity.Name] = null;

                var sessiondsa = Session["IDcuadre" + User.Identity.Name];
                var sefdsfndsa = Session["fecha" + User.Identity.Name];


                int idcuadre = db.Database.SqlQuery<int>("SELECT id FROM dbo.CuadreCajaPorCajero WHERE fecha='" + Session["fecha" + User.Identity.Name] + "' AND codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'and nit_cajero='" + Session["nit" + User.Identity.Name] + "'AND cierre=0").FirstOrDefault();
                Session["IDcuadre" + User.Identity.Name] = idcuadre;
                CuadreCajaPorCajero CuadreCajaPorCajero = db.CuadreCajaPorCajero.Find(idcuadre);//  buscamos el Id de cuadre parcial de caja 

                Session["retiros_efectivo" + User.Identity.Name] = CuadreCajaPorCajero.retiros_efectivo;
                Session["retiros_cheque" + User.Identity.Name] = CuadreCajaPorCajero.retiros_cheque;
                Session["consignacion_efectivo" + User.Identity.Name] = CuadreCajaPorCajero.consignacion_efectivo;
                Session["consignacion_cheque" + User.Identity.Name] = CuadreCajaPorCajero.consignacion_cheque;
                Session["tope" + User.Identity.Name] = CuadreCajaPorCajero.tope;

                return View(CuadreCajaPorCajero);
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        // metodos cuenta operacion son para encontrar las cuentas de quien desea hacer el retiro o consignacion 
        public ActionResult CuentaOperacion()
        {

            ///ListaFichasAportes = new AportesController().ConsultarFichasAportes().ToList().Select(p => new SelectListItem { Text = p.NIT+" || "+p.NombreComercial+" "+p.NOMBRE1+" "+p.NOMBRE2+" "+p.APELLIDO1+" "+p.APELLIDO2, Value = p.NIT, Selected = false });
            var ListaTerceros = new TercerosController().ConsultarTerceros().ToList().Select(p => new SelectListItem { Text = p.NIT + " || " + p.NombreComercial + " " + p.NOMBRE1 + " " + p.NOMBRE2 + " " + p.APELLIDO1 + " " + p.APELLIDO2, Value = p.NIT, Selected = false }); ;
            ViewBag.ListaTerceros = ListaTerceros;

            var fech = DateTime.Now.Date;
            Session["fecha" + User.Identity.Name] = fech.ToString("yyyy-MM-dd");
            if (Session["nit" + User.Identity.Name] != null && Session["IDcuadre" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                Caja caja = db.Caja.Find(Session["cod_caja" + User.Identity.Name]);

                Session["actual" + User.Identity.Name] = caja.consecutivo_actual;
                Session["Serie" + User.Identity.Name] = caja.Serie;
                Session["agencia" + User.Identity.Name] = caja.agencia;
                Session["tope_maximo" + User.Identity.Name] = caja.TopeMaximo_caja;
                Session["Factura" + User.Identity.Name] = Session["agencia" + User.Identity.Name] + "-" + Session["cod_caja" + User.Identity.Name] + "-" + Session["Serie" + User.Identity.Name] + "-" + Session["actual" + User.Identity.Name];
                Session["CUENTA" + User.Identity.Name] = null;

                var sessiondsa = Session["IDcuadre" + User.Identity.Name];
                var sefdsfndsa = Session["fecha" + User.Identity.Name];


                int idcuadre = db.Database.SqlQuery<int>("SELECT id FROM dbo.CuadreCajaPorCajero WHERE fecha='" + Session["fecha" + User.Identity.Name] + "' AND codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'and nit_cajero='" + Session["nit" + User.Identity.Name] + "'AND cierre=0").FirstOrDefault();
                Session["IDcuadre" + User.Identity.Name] = idcuadre;
                CuadreCajaPorCajero CuadreCajaPorCajero = db.CuadreCajaPorCajero.Find(idcuadre);//  buscamos el Id de cuadre parcial de caja 

                Session["retiros_efectivo" + User.Identity.Name] = CuadreCajaPorCajero.retiros_efectivo;
                Session["retiros_cheque" + User.Identity.Name] = CuadreCajaPorCajero.retiros_cheque;
                Session["consignacion_efectivo" + User.Identity.Name] = CuadreCajaPorCajero.consignacion_efectivo;
                Session["consignacion_cheque" + User.Identity.Name] = CuadreCajaPorCajero.consignacion_cheque;
                Session["tope" + User.Identity.Name] = CuadreCajaPorCajero.tope;

                return View(CuadreCajaPorCajero);
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        [Authorize]
        public ActionResult PagoCredito(string pagare)
        {
            
            var Informacion = new ProcesoCrediticioBLL().GetInfoCredito(User.Identity.Name, pagare);

            if (Informacion == null) { ViewBag.error = "Ha ocurrido un error al obtener la información"; }
                

            return View(Informacion);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PagoAporte(FactOpcaja facturaAporte)
        { 

            return View();        
        }

        private IEnumerable<SelectListItem> ListaFichasAportes;
        [HttpPost]
        public ActionResult CuentaOperacion(string cuenta, string operacion,string valor)
        {
            if (operacion == "Cons_Cre")
            {
                Session["op"] = "Cons";
                Session["op1"] = "Consignación Cuota Credito";

                return RedirectToAction("PagoCredito", new { pagare = cuenta });
            }

            if (operacion == "Des_Cre")
            {
                Session["op1" + User.Identity.Name] = "Desembolso";
                int idAportes = db.Database.SqlQuery<int>("SELECT id FROM apo.FichasAportes WHERE NumeroCuenta='" + cuenta + "'").FirstOrDefault();// AND tipoPago='Caja'").FirstOrDefault();
                Session["IdAportes" + User.Identity.Name] = idAportes;
                FichasAportes fichasAportes = db.FichasAportes.Find(Session["IdAportes" + User.Identity.Name]);
                if (fichasAportes == null)
                {
                    ViewBag.mensaje = "No se encontro Cuenta Registrada ";
                }
                else
                {
                    Session["nit_cuenta" + User.Identity.Name] = fichasAportes.idPersona;

                    Tercero terceros = db.Terceros.Find(Session["nit_cuenta" + User.Identity.Name]); //Para Extrare el Nombre desde Terceros
                    Session["cuenta" + User.Identity.Name] = fichasAportes.idPersona;
                    Session["Tipo_pago" + User.Identity.Name] = fichasAportes.tipoPago;
                    Session["saldo_cuenta" + User.Identity.Name] = fichasAportes.totalAportes;
                    Session["nombre_cuenta" + User.Identity.Name] = terceros.NOMBRE1 + " " + terceros.NOMBRE2 + " " + terceros.APELLIDO1 + " " + terceros.APELLIDO2;
                    Session["bandera" + User.Identity.Name] = "aporte";
                    return RedirectToAction("CreateDesembolso");
                }
            }

            ListaFichasAportes =  new AportesController().ConsultarFichasAportes().ToList().Select(p => new SelectListItem { Text = p.NIT + " || " + p.NombreComercial + " " + p.NOMBRE1 + " " + p.NOMBRE2 + " " + p.APELLIDO1 + " " + p.APELLIDO2, Value = p.NIT, Selected = false }); ;

            ViewBag.FichasAportesAsociados = ListaFichasAportes;

            if (operacion == "Ret") //condicion cuando hace retiros
            {
                Session["op" + User.Identity.Name] = "Ret";
                Session["op1" + User.Identity.Name] = "Retiro Efectivo";
                Session["CUENTA" + User.Identity.Name] = cuenta;

                int idAhorros = db.Database.SqlQuery<int>("SELECT id FROM aho.FichasAhorros WHERE numeroCuenta='" + Session["CUENTA" + User.Identity.Name] + "' AND tipoPago='Caja'").FirstOrDefault();
                Session["IdAhorro" + User.Identity.Name] = idAhorros;
                FichasAhorros fichasAhorros = db.FichasAhorros.Find(Session["IdAhorro" + User.Identity.Name]);// encontramos la cuenta
                if (fichasAhorros == null)
                {
                    ViewBag.mensaje = "No se encontro la Cuenta ";

                }
                else
                {
                    Session["nit_cuenta" + User.Identity.Name] = fichasAhorros.idPersona;

                    Tercero terceros = db.Terceros.Find(Session["nit_cuenta" + User.Identity.Name]);
                    Session["cuenta" + User.Identity.Name] = fichasAhorros.numeroCuenta;
                    Session["Tipo_pago" + User.Identity.Name] = fichasAhorros.tipoPago;
                    Session["saldo_cuenta" + User.Identity.Name] = fichasAhorros.totalAhorros;
                    Session["valor_cuota" + User.Identity.Name] = fichasAhorros.valorCuota;
                    Session["nombre_cuenta" + User.Identity.Name] = terceros.NOMBRE1 + " " + terceros.NOMBRE2 + " " + terceros.APELLIDO1 + " " + terceros.APELLIDO2;

                    int Saldo_cuenta;
                    int Valor_cuota;
                    Valor_cuota = Convert.ToInt32(Session["valor_cuota" + User.Identity.Name]);
                    Saldo_cuenta = Convert.ToInt32(Session["saldo_cuenta" + User.Identity.Name]);

                    if (Saldo_cuenta < Valor_cuota)
                    {
                        ViewBag.mensaje = "Fondos Insuficientes para realizar retiros";
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("CreateRet");
                    }
                }
            }
            else if (operacion == "Cons")
            {

                //opcion consignacion
                Session["op" + User.Identity.Name] = "Cons";
                Session["op1" + User.Identity.Name] = "Consignación";
                int idAhorros = db.Database.SqlQuery<int>("SELECT id FROM aho.FichasAhorros WHERE numeroCuenta='" + cuenta + "' AND tipoPago='Caja'").FirstOrDefault();
                Session["IdAhorro" + User.Identity.Name] = idAhorros;

                FichasAhorros fichasAhorros = db.FichasAhorros.Find(Session["IdAhorro" + User.Identity.Name]);
                if (fichasAhorros == null)
                {
                    int idAportes = db.Database.SqlQuery<int>("SELECT id FROM apo.FichasAportes WHERE NumeroCuenta='" + cuenta + "'").FirstOrDefault();// AND tipoPago='Caja'").FirstOrDefault();
                    Session["IdAportes" + User.Identity.Name] = idAportes;
                    FichasAportes fichasAportes = db.FichasAportes.Find(Session["IdAportes" + User.Identity.Name]);
                    if (fichasAportes == null)
                    {
                        ViewBag.mensaje = "No se encontro Cuenta Registrada ";
                    }
                    else
                    {
                        Session["nit_cuenta" + User.Identity.Name] = fichasAportes.idPersona;

                        Tercero terceros = db.Terceros.Find(Session["nit_cuenta" + User.Identity.Name]); //Para Extrare el Nombre desde Terceros
                        Session["cuenta" + User.Identity.Name] = fichasAportes.numeroCuenta;
                        Session["Tipo_pago" + User.Identity.Name] = fichasAportes.tipoPago;
                        Session["saldo_cuenta" + User.Identity.Name] = fichasAportes.totalAportes;
                        Session["nombre_cuenta" + User.Identity.Name] = terceros.NOMBRE1 + " " + terceros.NOMBRE2 + " " + terceros.APELLIDO1 + " " + terceros.APELLIDO2;
                        Session["bandera" + User.Identity.Name] = "aporte";
                        return RedirectToAction("CreateCons", new { cuenta = fichasAportes.numeroCuenta });
                    }
                }
                else
                {

                    Session["nit_cuenta" + User.Identity.Name] = fichasAhorros.idPersona;
                    Tercero terceros = db.Terceros.Find(Session["nit_cuenta"]);

                    Session["cuenta" + User.Identity.Name] = fichasAhorros.numeroCuenta;
                    Session["Tipo_pago" + User.Identity.Name] = fichasAhorros.tipoPago;
                    Session["saldo_cuenta" + User.Identity.Name] = fichasAhorros.totalAhorros;
                    Session["nombre_cuenta" + User.Identity.Name] = terceros.NOMBRE1 + " " + terceros.NOMBRE2 + " " + terceros.APELLIDO1 + " " + terceros.APELLIDO2;

                    Session["bandera" + User.Identity.Name] = "ahorro";
                    return RedirectToAction("CreateCons");
                }

            }
            else if (operacion == "ConsAhorroC")
            {
                return RedirectToAction("CreateConsAC", new { cuenta = cuenta });
            }
            //{
                
            //    /* else
            //     {
            //         Session["op"] = "Ret_che";
            //         Session["op1"] = "Retiro Cheque";
            //         Session["CUENTA"] = cuenta;

            //         int idAhorros = db.Database.SqlQuery<int>("SELECT id FROM aho.FichasAhorros WHERE numeroCuenta='" + Session["CUENTA"] + "' AND tipoPago='Caja'").FirstOrDefault();
            //         Session["IdAhorro"] = idAhorros;

            //         FichasAhorros fichasAhorros = db.FichasAhorros.Find(Session["IdAhorro"]);// encontramos la cuenta
            //         if (fichasAhorros == null)
            //         {
            //             ViewBag.mensaje = "No se encontro la Cuenta ";
            //         }
            //         else
            //         {
            //             Session["nit_cuenta"] = fichasAhorros.idPersona;
            //             Tercero terceros = db.Terceros.Find(Session["nit_cuenta"]);

            //             Session["cuenta"] = fichasAhorros.numeroCuenta;
            //             Session["Tipo_pago"] = fichasAhorros.tipoPago;
            //             Session["saldo_cuenta"] = fichasAhorros.totalAhorros;
            //             Session["valor_cuota"] = fichasAhorros.valorCuota;
            //             Session["nombre_cuenta"] = terceros.NOMBRE1 + " " + terceros.NOMBRE2 + " " + terceros.APELLIDO1 + " " + terceros.APELLIDO2;

            //             int Saldo_cuenta;
            //             int Valor_cuota;
            //             Valor_cuota = Convert.ToInt32(Session["valor_cuota"]);
            //             Saldo_cuenta = Convert.ToInt32(Session["saldo_cuenta"]);

            //             if (Saldo_cuenta < Valor_cuota)
            //             {
            //                 ViewBag.mensaje = "Fondos Insuficientes para realizar retiros";
            //                 return View();
            //             }
            //             else
            //             {
            //                 return RedirectToAction("CreateRetCheque");
            //             }
            //         }
            //     }
            //     */
            //}
            return View();
        }
        private IEnumerable<SelectListItem> ListaTercerosEntidadDos;

        [HttpPost]
        public ActionResult CuentaOperacionEntidadDos(string cuenta, string operacion)
        {

            ListaTercerosEntidadDos = new TercerosController().ConsultartercerosEntidadDos().ToList().Select(p => new SelectListItem { Text = p.nombre + " || " + p.cedula, Value = p.cedula, Selected = false }); ;

            ViewBag.ListaTercerosEntidadDos = ListaTercerosEntidadDos;

            var tercero = (from pc in db.tercerosEntidadDos where pc.cedula == cuenta select pc).Single();

            var factOpCajaConsCuotaCredito = new factOpCajaConsCuotaCreditoEntidadDos()
            {
                fecha = DateTime.Now,
                factura = Session["Factura" + User.Identity.Name].ToString(),
                cedula = cuenta,
                codigoCaja = Session["cod_caja" + User.Identity.Name].ToString(),
                nitCajero = Session["nit" + User.Identity.Name].ToString(),
                valorConsignado = tercero.cuota
            };

            return RedirectToAction("CreateConsCuotaCreditoEntidadDos", factOpCajaConsCuotaCredito);
        }

        public ActionResult CreateRet()
        {
            if (Session["nit" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                Session["fechaHora" + User.Identity.Name] = Convert.ToString(DateTime.Now);
                Session["Seleccionada" + User.Identity.Name] = null;
                TipoComprobante tiposComprobantes = db.TiposComprobantes.Find(Session["comp_egreso" + User.Identity.Name]);
                Session["consecutivo" + User.Identity.Name] = tiposComprobantes.CONSECUTIVO;
                return View();
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRet([Bind(Include = "id,fecha,factura,operacion,codigo_caja,nit_cajero,numero_cuenta,nit_propietario_cuenta,nombre_propietario_cuenta,valor_recibido,valor_efectivo,vueltas,valor_cheque,numero_cheque,consignacion,observacion,saldo_total_cuenta,total,nit_consignacion,valor_cheque1,numero_cheque1,nit_consignacion1,valor_cheque2,numero_cheque2,nit_consignacion2,valor_cheque3,numero_cheque3,nit_consignacion3,valor_cheque4,numero_cheque4,nit_consignacion4,valor_cheque5,numero_cheque5,nit_consignacion5,total_cheques")] FactOpcaja factOpcaja)
        {

            if (factOpcaja.valor_efectivo == null)
            {
                factOpcaja.valor_efectivo = 0;
            }

            if (factOpcaja.valor_efectivo == 0)
            {
                ViewBag.mensaje = "No se puede hacer retiro de 0 (cero) $ ";
            }
            else
            {

                decimal topemaximo = Convert.ToDecimal(Session["tope_maximo" + User.Identity.Name]);

                if (topemaximo < factOpcaja.total)
                {
                    ViewBag.mensaje = "Tope maximo de caja alcanzado. Abastecer caja y actualizar la pagina para continuar con el retiro.";
                }
                else
                {
                    int Valor_cuota;
                    Valor_cuota = Convert.ToInt32(Session["valor_cuota" + User.Identity.Name]);

                    if (factOpcaja.saldo_total_cuenta < Valor_cuota)
                    {
                        ModelState.AddModelError("saldo_total_cuenta", "sin fondos en la cuenta");
                        ViewBag.mensaje = "Cuenta sinfondos Para Realizar Retiro";

                    }
                    else
                    {

                        if (ModelState.IsValid)
                        {
                            if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
                            {
                                //modificamos consecutivo actual para incrementar de uno en uno  la factura
                                var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                                db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                            }
                            else
                            {
                                var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                                db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                            }

                            //Actualizar el cuadre y tope de caja
                            var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET retiros_efectivo=retiros_efectivo+" + factOpcaja.total + ", tope=tope-" + factOpcaja.total + "WHERE fecha='" + Session["fecha" + User.Identity.Name] + "'AND codigo_caja='" + factOpcaja.codigo_caja + "' AND nit_cajero='" + factOpcaja.nit_cajero + "' AND cierre=0";
                            db.Database.ExecuteSqlCommand(updatecuadre);

                            var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja-" + factOpcaja.total + "WHERE Codigo_caja='" + factOpcaja.codigo_caja + "'";
                            db.Database.ExecuteSqlCommand(updatetopecaja);

                            // actualizamos tabla Fichas de ahorro
                            var udateAhorro = "UPDATE aho.FichasAhorros SET totalAhorros=totalAhorros-" + factOpcaja.total + " WHERE numeroCuenta='" + factOpcaja.numero_cuenta + "' AND tipoPago='Caja'";

                            string ano = Convert.ToString(DateTime.Now.Year);
                            string mes = Convert.ToString(DateTime.Now.Month);
                            string dia = Convert.ToString(DateTime.Now.Day);
                            string fechaOp = Convert.ToString(DateTime.Now);
                            string fpagoRetiro = "213005001";// es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas


                            double tope = Convert.ToDouble(Session["tope" + User.Identity.Name]);

                            //Generar comprobante.
                            var Comprobante = "INSERT INTO acc.Comprobantes (TIPO, NUMERO, ANO, MES, DIA, CCOSTO, ELIMINADO, DETALLE, TERCERO, FPAGO, CTAFPAGO, NUMEXTERNO, VRTOTAL, SUMDBCR, FECHARealiz, MODIFICA, EXPORTADO, MARCASEG, BLOQUEADO, NUMIMP, PC,USUARIO,ANULADO)VALUES('" + Session["comp_egreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + ano + "','" + mes + "','" + dia + "','" + Session["cc_transacciones" + User.Identity.Name] + "',NULL,'RETIRO DESDE CAJA','" + factOpcaja.nit_propietario_cuenta + "',NULL,'" + fpagoRetiro + "',NULL," + factOpcaja.total + ",0,'" + fechaOp + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'False')";
                            db.Database.ExecuteSqlCommand(Comprobante); 





                            //MOVIMIENTOS 
                            var movimieno1 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_egreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + Session["cta_efectivo" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','AHORROS',0," + factOpcaja.total + ",0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
                            db.Database.ExecuteSqlCommand(movimieno1);





                            var movimieno2 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_egreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + fpagoRetiro + "','" + factOpcaja.nit_propietario_cuenta + "','AHORROS'," + factOpcaja.total + ",0,0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
                            db.Database.ExecuteSqlCommand(movimieno2);


                            //SaldosCCS
                            var saldosCCS = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_efectivo" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "',0," + factOpcaja.total + "," + factOpcaja.total + ")";
                            db.Database.ExecuteSqlCommand(saldosCCS);
                            var saldosCCS1 = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpagoRetiro + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "'," + factOpcaja.total + ",0," + factOpcaja.total + ")";
                            db.Database.ExecuteSqlCommand(saldosCCS1);
                            //saldoscuentas
                            var saldoscuentas = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_efectivo" + User.Identity.Name] + "','" + ano + "','" + mes + "',0," + factOpcaja.total + "," + factOpcaja.total + ")";
                            db.Database.ExecuteSqlCommand(saldoscuentas);
                            var saldoscuentas1 = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpagoRetiro + "','" + ano + "','" + mes + "'," + factOpcaja.total + ",0," + factOpcaja.total + ")";
                            db.Database.ExecuteSqlCommand(saldoscuentas1);
                            var saldosterceros = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_efectivo" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "',0," + factOpcaja.total + "," + factOpcaja.total + ")";
                            db.Database.ExecuteSqlCommand(saldosterceros);
                            //sadosterceros
                            var saldosterceros1 = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpagoRetiro + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "'," + factOpcaja.total + ",0," + factOpcaja.total + ")";
                            db.Database.ExecuteSqlCommand(saldosterceros1);


                            //actulizamos consecutivo
                            var updateconsecutivoTiposComprobantes = "UPDATE acc.TiposComprobantes SET CONSECUTIVO=CONSECUTIVO+1 WHERE CODIGO='" + Session["comp_egreso" + User.Identity.Name] + "'";
                            db.Database.ExecuteSqlCommand(updateconsecutivoTiposComprobantes);

                            db.Database.ExecuteSqlCommand(udateAhorro);
                            db.FactOpcaja.Add(factOpcaja);
                            db.SaveChanges();
                            return RedirectToAction("DetailsRet/" + factOpcaja.id);
                        }
                    }
                }

            }




            return View(factOpcaja);

        }


        public ActionResult CreateRetCheque()
        {
            if (Session["nit" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                Session["fechaHora" + User.Identity.Name] = Convert.ToString(DateTime.Now);
                Session["Seleccionada" + User.Identity.Name] = null;
                TipoComprobante tiposComprobantes = db.TiposComprobantes.Find(Session["comp_egreso" + User.Identity.Name]);
                Session["consecutivo" + User.Identity.Name] = tiposComprobantes.CONSECUTIVO;
                return View();
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRetCheque([Bind(Include = "id,fecha,factura,operacion,codigo_caja,nit_cajero,numero_cuenta,nit_propietario_cuenta,nombre_propietario_cuenta,valor_recibido,valor_efectivo,vueltas,valor_cheque,numero_cheque,consignacion,observacion,saldo_total_cuenta,total,nit_consignacion,valor_cheque1,numero_cheque1,nit_consignacion1,valor_cheque2,numero_cheque2,nit_consignacion2,valor_cheque3,numero_cheque3,nit_consignacion3,valor_cheque4,numero_cheque4,nit_consignacion4,valor_cheque5,numero_cheque5,nit_consignacion5,total_cheques")] FactOpcaja factOpcaja)
        {
            CuentaMayor planCuentas = db.PlanCuentas.Find(Session["cta_cheques" + User.Identity.Name]);
            decimal saldo_cta_cheque = Convert.ToDecimal(planCuentas.Saldo);

            if (saldo_cta_cheque < factOpcaja.total)
            {
                ViewBag.mensaje = "No existe saldo en La cuenta Para realizar Retiros en cheque";
            }
            else
            {
                int Valor_cuota;
                Valor_cuota = Convert.ToInt32(Session["valor_cuota" + User.Identity.Name]);
                if (factOpcaja.saldo_total_cuenta < Valor_cuota)
                {
                    ModelState.AddModelError("saldo_total_cuenta", "sin fondos en la cuenta");
                    ViewBag.mensaje = "Cuenta sinfondos Para Realizar Retiro";

                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
                        {
                            //modificamos consecutivo actual para incrementar de uno en uno  la factura
                            var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                            db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                        }
                        else
                        {
                            var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                            db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                        }

                        // actualizamos tabla Fichas de ahorro
                        var udateAhorro = "UPDATE aho.FichasAhorros SET totalAhorros=totalAhorros-" + factOpcaja.total + " WHERE numeroCuenta='" + factOpcaja.numero_cuenta + "' AND tipoPago='Caja'";
                        db.Database.ExecuteSqlCommand(udateAhorro);

                        //Actualizar el cuadre 
                        var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET retiros_cheque=retiros_cheque+" + factOpcaja.total + "WHERE fecha='" + Session["fecha" + User.Identity.Name] + "'AND codigo_caja='" + factOpcaja.codigo_caja + "' AND nit_cajero='" + factOpcaja.nit_cajero + "' AND cierre=0";
                        db.Database.ExecuteSqlCommand(updatecuadre);

                        //actualizamos plan cuentas para los retiros de cheque
                        var plancuentas = "UPDATE acc.PlanCuentas SET Saldo=Saldo-" + factOpcaja.valor_cheque + "WHERE CODIGO='" + Session["cta_cheque" + User.Identity.Name] + "'";
                        db.Database.ExecuteSqlCommand(plancuentas);
                        //***********************************************************************************

                        string ano = Convert.ToString(DateTime.Now.Year);
                        string mes = Convert.ToString(DateTime.Now.Month);
                        string dia = Convert.ToString(DateTime.Now.Day);
                        string fechaOp = Convert.ToString(DateTime.Now);
                        string fpagoRetiro = "213005001";// es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas

                        //CONTRUIR EL COMPROBANTE
                        /*
                        var consecutivoComprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == "CE3" & x.INACTIVO == false);

                        var comprobante = new Comprobante()
                        {
                            TIPO = "CE3",
                            NUMERO = consecutivoComprobante.CONSECUTIVO,
                            ANO = Convert.ToString(DateTime.Now.Year),
                            MES = Convert.ToString(DateTime.Now.Month),
                            DIA = Convert.ToString(DateTime.Now.Day),
                            CCOSTO = "00",
                            DETALLE = "DESEMBOLSO",
                            TERCERO = prestamo.NIT,
                            CTAFPAGO = cuentaCreditoCod,
                            VRTOTAL = prestamo.Capital,
                            SUMDBCR = 0,
                            FECHARealiz = DateTime.Now,
                            ANULADO = false
                        };

                        db.Comprobantes.Add(comprobante);

                        //COMTRUIR LA LISTA DE MOVIMIENTOS
                        List<Movimiento> listaDeMovimientos = new List<Movimiento>();
                        var mov1 = new Movimiento()
                        {
                            TIPO = "CE3",
                            NUMERO = consecutivoComprobante.CONSECUTIVO,
                            CUENTA = cuentaCreditoCod,
                            TERCERO = prestamo.NIT,
                            DETALLE = "DESEMBOLSO",
                            DEBITO = 0,
                            CREDITO = prestamo.Capital,
                            BASE = 0,
                            CCOSTO = "07",
                            FECHAMOVIMIENTO = DateTime.Now,
                        };

                        listaDeMovimientos.Add(mov1);
                        var mov2 = new Movimiento()
                        {
                            TIPO = "CE3",
                            NUMERO = consecutivoComprobante.CONSECUTIVO,
                            CUENTA = cuentaDebitoCod,
                            TERCERO = prestamo.NIT,
                            DETALLE = "DESEMBOLSO",
                            DEBITO = prestamo.Capital,
                            CREDITO = 0,
                            BASE = 0,
                            CCOSTO = "07",
                            FECHAMOVIMIENTO = DateTime.Now,
                        };
                        listaDeMovimientos.Add(mov2);

                        var result = false;


                        var comprobanteConst = new ComprobanteBO();
                        result = comprobanteConst.AsentarDesembolso(listaDeMovimientos, Convert.ToInt32(consecutivoComprobante.CONSECUTIVO));

                        if (result)
                        {
                            db.SaveChanges();
                        }
                        */


                        //Generar comprobante.
                        var Comprobante = "INSERT INTO acc.Comprobantes (TIPO, NUMERO, ANO, MES, DIA, CCOSTO, ELIMINADO, DETALLE, TERCERO, FPAGO, CTAFPAGO, NUMEXTERNO, VRTOTAL, SUMDBCR, FECHARealiz, MODIFICA, EXPORTADO, MARCASEG, BLOQUEADO, NUMIMP, PC,USUARIO,ANULADO)VALUES('" + Session["comp_egreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + ano + "','" + mes + "','" + dia + "','" + Session["cc_transacciones" + User.Identity.Name] + "',NULL,'RETIRO CHEQUE CAJA','" + factOpcaja.nit_propietario_cuenta + "',NULL,'" + fpagoRetiro + "',NULL," + factOpcaja.total + ",0,'" + fechaOp + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'False')";
                        db.Database.ExecuteSqlCommand(Comprobante);




                        //MOVIMIENTOS 
                        var movimieno1 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_egreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','AHORROS',0," + factOpcaja.total + ",0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
                        db.Database.ExecuteSqlCommand(movimieno1);



                        var movimieno2 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_egreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + fpagoRetiro + "','" + factOpcaja.nit_propietario_cuenta + "','AHORROS'," + factOpcaja.total + ",0,0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
                        db.Database.ExecuteSqlCommand(movimieno2);








                        //SaldosCCS
                        var saldosCCS = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "',0," + factOpcaja.total + "," + factOpcaja.total + ")";
                        db.Database.ExecuteSqlCommand(saldosCCS);
                        var saldosCCS1 = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpagoRetiro + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "'," + factOpcaja.total + ",0," + factOpcaja.total + ")";
                        db.Database.ExecuteSqlCommand(saldosCCS1);
                        //saldoscuentas
                        var saldoscuentas = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + ano + "','" + mes + "',0," + factOpcaja.total + "," + factOpcaja.total + ")";
                        db.Database.ExecuteSqlCommand(saldoscuentas);
                        var saldoscuentas1 = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpagoRetiro + "','" + ano + "','" + mes + "'," + factOpcaja.total + ",0," + factOpcaja.total + ")";
                        db.Database.ExecuteSqlCommand(saldoscuentas1);
                        //sadosterceros
                        var saldosterceros = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "',0," + factOpcaja.total + "," + factOpcaja.total + ")";
                        db.Database.ExecuteSqlCommand(saldosterceros);
                        var saldosterceros1 = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpagoRetiro + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "'," + factOpcaja.total + ",0," + factOpcaja.total + ")";
                        db.Database.ExecuteSqlCommand(saldosterceros1);
                        //actulizamos consecutivo
                        var updateconsecutivoTiposComprobantes = "UPDATE acc.TiposComprobantes SET CONSECUTIVO=CONSECUTIVO+1 WHERE CODIGO='" + Session["comp_egreso" + User.Identity.Name] + "'";
                        db.Database.ExecuteSqlCommand(updateconsecutivoTiposComprobantes);

                        db.FactOpcaja.Add(factOpcaja);
                        db.SaveChanges();
                        return RedirectToAction("DetailsRetCheque/" + factOpcaja.id);
                    }
                }
            }
            return View(factOpcaja);
        }

        public ActionResult CreateConsCuotaCreditoEntidadDos(factOpCajaConsCuotaCreditoEntidadDos modelParam)
        {
            if (Session["nit" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                var tercero = (from pc in db.tercerosEntidadDos where pc.cedula == modelParam.cedula select pc).Single();
                ViewBag.nombreTercero = tercero.nombre;
                return View(modelParam);
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        public JsonResult getHistorialCredito(string pagare)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
            decimal saldoCapital = db.HistorialCreditos.Where(x => x.pagare == pagare && x.estado != "pazYsalvo").Select(x => x.saldoCapital).FirstOrDefault();
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            IList<HistorialCreditos> HistorialCreditosList = db.HistorialCreditos.Where(x => x.pagare == pagare && x.estado != "abono").ToList();
            List<Array> codigos = new List<Array>();
            foreach (var item in HistorialCreditosList)
            {
                if (item.estado != "pazYsalvo")
                {
                    var estado = "Al día";
                    DateTime fechaProximoPago = new DateTime(item.fechaProximoPago.Year, item.fechaProximoPago.Month, item.fechaProximoPago.Day);
                    ;
                    if (item.estado == "enMora")
                    {
                        estado = "En Mora";
                    }
                    decimal capitalAPagar;
                    if (saldoCapital < item.proximaCuota)
                    {
                        capitalAPagar = saldoCapital;
                    }
                    else
                    {
                        capitalAPagar = item.proximaCuota - item.interesCorriente - item.valorCosto;
                    }


                    string[] nombres = new string[9];
                    nombres[0] = item.id.ToString();
                    nombres[1] = item.numeroCuota.ToString();
                    nombres[2] = estado;
                    nombres[3] = item.interesCorriente.ToString("N0", formato);
                    nombres[4] = capitalAPagar.ToString("N0", formato);
                    nombres[5] = item.valorCosto.ToString("N0", formato);
                    nombres[6] = item.interesMora.ToString("N0", formato);
                    nombres[7] = item.diasEnMora.ToString("N0", formato);
                    nombres[8] = fechaProximoPago.ToString("N0", formato);

                    codigos.Add(nombres);
                }

            }
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getValorCuota(Array ids)
        {
            decimal valorCuota = 0;
            string pagareConsultar = "1";
            foreach (string item in ids)
            {
                var idAConsultar = Convert.ToInt32(item);
                var HistorialCredito = db.HistorialCreditos.Where(x => x.id == idAConsultar).Single();
                pagareConsultar = HistorialCredito.pagare;

                if (HistorialCredito.estado == "enMora")
                {
                    decimal saldoCapital = db.HistorialCreditos.Where(x => x.pagare == pagareConsultar && x.estado != "pazYsalvo").Select(x => x.saldoCapital).FirstOrDefault();
                    decimal capitalAPagar;
                    if (saldoCapital < HistorialCredito.proximaCuota)
                    {
                        capitalAPagar = saldoCapital;
                        valorCuota = HistorialCredito.interesCorriente + HistorialCredito.interesMora + HistorialCredito.valorCosto + capitalAPagar;
                    }
                    else
                    {
                        valorCuota = valorCuota + HistorialCredito.interesCorriente + HistorialCredito.capitalEnMora + HistorialCredito.interesMora + HistorialCredito.valorCosto;
                    }
                }
                else if (HistorialCredito.estado != "pazYsalvo")
                {
                    decimal saldoCapital = db.HistorialCreditos.Where(x => x.pagare == pagareConsultar && x.estado != "pazYsalvo").Select(x => x.saldoCapital).FirstOrDefault();
                    decimal capitalAPagar;
                    if (saldoCapital < HistorialCredito.proximaCuota)
                    {
                        capitalAPagar = saldoCapital;
                        valorCuota = valorCuota + HistorialCredito.interesCorriente + HistorialCredito.valorCosto + capitalAPagar;
                    }
                    else
                    {
                        capitalAPagar = HistorialCredito.proximaCuota - HistorialCredito.interesCorriente - HistorialCredito.valorCosto;

                        valorCuota = valorCuota + HistorialCredito.interesCorriente + HistorialCredito.valorCosto + capitalAPagar;
                    }
                }
            }
            //Calcular todo el interes de mora
            //var HistorialPorPagare = db.HistorialCreditos.Where(x => x.pagare == pagareConsultar).ToList();
            //decimal mora = 0;
            //foreach (var his in HistorialPorPagare)
            //{
            //    if (his.estado == "enMora")
            //    {
            //        mora = mora + his.interesMora;
            //    }
            //}
            //valorCuota = valorCuota + mora;
            return Json(valorCuota, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSaldoCapital(string pagare)
        {
            decimal saldoCapital = db.HistorialCreditos.Where(x => x.pagare == pagare && x.estado != "pazYsalvo").Select(x => x.saldoCapital).FirstOrDefault();
            return Json(saldoCapital, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateConsCuotaCredito(string pagare)
        {
            if (Session["nit" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                var credito = (from pc in db.Creditos where pc.Pagare == pagare select pc).FirstOrDefault();
                var fecha = DateTime.Now;
                var factura = Session["Factura" + User.Identity.Name].ToString();
                var codCaja = Session["cod_caja" + User.Identity.Name].ToString();
                var nitCajero = Session["nit" + User.Identity.Name].ToString();
                var Tercero = (from pc in db.Terceros where pc.NIT == credito.Creditos_Cedula select pc).Single();
                ViewBag.NombreTercero = Tercero.NOMBRE1 + " " + Tercero.NOMBRE2 + " " + Tercero.APELLIDO1 + " " + Tercero.APELLIDO2;

                
                var amortizacion = (from pc in db.Amortizaciones where pc.pagare == credito.Pagare && pc.calendarioDePagos == "Fecha" select pc).First();

                var HistorialCreditosList = db.HistorialCreditos.Where(x => x.pagare == credito.Pagare).ToList();
                decimal totalAPagar = 0;

                var factOpCajaConsCuotaCredito = new factOpCajaConsCuotaCredito()
                {
                    fecha = fecha,
                    factura = factura,
                    NIT = credito.Creditos_Cedula,
                    codigoCaja = codCaja,
                    valorConsignado = amortizacion.valorCuota,
                    nitCajero = nitCajero,
                    pagare = credito.Pagare
                };

                return View(factOpCajaConsCuotaCredito);
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        public ActionResult CreateCons(string cuenta)
        {
            if (Session["nit" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                Session["fechaHora" + User.Identity.Name] = Convert.ToString(DateTime.Now);
                Session["Seleccionada" + User.Identity.Name] = null;
                ViewBag.nit_consignacion = new SelectList(db.CodigosBanco, "codig_banco", "Banco");
                ViewBag.nit_consignacion1 = new SelectList(db.CodigosBanco, "codig_banco", "Banco");
                ViewBag.nit_consignacion2 = new SelectList(db.CodigosBanco, "codig_banco", "Banco");
                ViewBag.nit_consignacion3 = new SelectList(db.CodigosBanco, "codig_banco", "Banco");
                ViewBag.nit_consignacion4 = new SelectList(db.CodigosBanco, "codig_banco", "Banco");
                ViewBag.nit_consignacion5 = new SelectList(db.CodigosBanco, "codig_banco", "Banco");
                TipoComprobante tiposComprobantes = db.TiposComprobantes.Find(Session["comp_ingreso" + User.Identity.Name]);
                Session["consecutivo" + User.Identity.Name] = tiposComprobantes.CONSECUTIVO;
                var NumeroCuentaAporte = Session["cuenta" + User.Identity.Name].ToString();
                var VarFactOpcaja = new FactOpcaja();
                

                string valorCuota = (from pc in db.FichasAportes where pc.numeroCuenta == cuenta select pc.valor).FirstOrDefault();
                ViewBag.ValorCuota = valorCuota;

                return View(VarFactOpcaja);
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }
        [HttpPost]
        public JsonResult GetAutoCompletAfiliadosApEx(string cadena)
        {

            var cad = (from AportEx in db.FichaAfiliadosAporteEx
                       from Tercer in db.Terceros
                       where AportEx.idPersona == Tercer.NIT && AportEx.Estado == true
                       
                                 select new
                                 { Id = Tercer.NIT, 
                                   Nombre = (Tercer.NombreComercial + Tercer.NOMBRE1 + " " + Tercer.NOMBRE2 + " " + Tercer.APELLIDO1 + " " + Tercer.APELLIDO2 + " || " + Tercer.NIT).ToUpper(),
                                   NoCuenta = AportEx.NumeroCuenta,
                                   TotalCuenta = AportEx.totalAportesEx,
                                   ConfiguracionApo = AportEx.IdConfiguracion
                                   
                                 })
                .Where(x => x.Id.Contains(cadena) || x.Nombre.Contains(cadena)).ToList();
           

            return Json(cad, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CreateConsAporteEx() 

        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConsAporteEx(FactOpcaja factOpcaja)
        {
            var respuesta = new BLLOperacionesCaja().PagarAporteExtra(factOpcaja.codigo_caja, factOpcaja.nit_cajero, factOpcaja.nit_propietario_cuenta, factOpcaja.numero_cuenta, factOpcaja.observacion, factOpcaja.operacion,factOpcaja.saldo_total_cuenta,factOpcaja.total);

            if (respuesta != null)
            {
                return RedirectToAction("DetallesApEx/" + respuesta.Id);
            }

            return View(factOpcaja);
        }
        public ActionResult DetallesApEx(int? id)
        { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactOpcaja factOpcaja = db.FactOpcaja.Find(id);
            if (factOpcaja == null)
            {
                return HttpNotFound();
            }

            //obtenemos los movimientos adicionales a caja y la cuenta configurada para aportes
            var movimientos = db.Movimientos.Where(x => x.TIPO == factOpcaja.TIPO && x.NUMERO == factOpcaja.NUMERO).ToList();
            movimientos.RemoveRange(1, 1);//se elimina las cuentas de cuenta de caja y la de aportes y se deja las demás
            ViewBag.movimientos = movimientos;
            return View(factOpcaja);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConsCuotaCredito([Bind(Include = "id,fecha,factura,NIT,codigoCaja,pagare,valorConsignado,nitCajero,abonoCapital")] factOpCajaConsCuotaCredito factOpCajaConsCuotaCredito)
        {
            var nuevoAbonoCapital = (factOpCajaConsCuotaCredito.abonoCapital).ToString();
            var nuevoAbonoCapitalInt = Convert.ToInt32(factOpCajaConsCuotaCredito.abonoCapital);
            var valorCuotaFija = factOpCajaConsCuotaCredito.valorConsignado.Replace('.', ',');
            var vrCuotaCredito = Convert.ToDecimal(valorCuotaFija);
            if (nuevoAbonoCapitalInt != 0)
            {
                var valorConsignadoDecimal = Convert.ToDecimal(factOpCajaConsCuotaCredito.abonoCapital);
                //sI consecutivo actual es igual al final serie inclementa en uno
                if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
                {
                    var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                    db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                }
                else// si no es igual solo actualiza consecutivo de factura
                {
                    var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                    db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                }

                //Actualizar el cuadre y tope de caja
                var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET tope=tope+" + nuevoAbonoCapital + "WHERE fecha='" + Session["fecha" + User.Identity.Name] + "'AND codigo_caja='" + factOpCajaConsCuotaCredito.codigoCaja + "' AND nit_cajero='" + factOpCajaConsCuotaCredito.nitCajero + "' AND cierre=0";
                db.Database.ExecuteSqlCommand(updatecuadre);
                var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja+" + nuevoAbonoCapital + "WHERE Codigo_caja='" + factOpCajaConsCuotaCredito.codigoCaja + "'";
                db.Database.ExecuteSqlCommand(updatetopecaja);

                //CONTRUIR EL COMPROBANTE

                var cuenta = (from pc in db.Cuentas where pc.Funcion == "F4" select pc).Single();
                var Comprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == cuenta.TipoComprobante & x.INACTIVO == false);
                var cajaPago = (from pc in db.Caja where pc.Codigo_caja == factOpCajaConsCuotaCredito.codigoCaja select pc).Single();
                var credito = (from pc in db.Creditos where pc.Creditos_Cedula == factOpCajaConsCuotaCredito.NIT select pc).Single();
                var comprobanteNew = new Comprobante()
                {
                    TIPO = cuenta.TipoComprobante,
                    NUMERO = Comprobante.CONSECUTIVO,
                    ANO = Convert.ToString(DateTime.Now.Year),
                    MES = Convert.ToString(DateTime.Now.Month),
                    DIA = Convert.ToString(DateTime.Now.Day),
                    CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                    DETALLE = "CONSIGNACION CUOTA CREDITO",
                    TERCERO = factOpCajaConsCuotaCredito.NIT,
                    CTAFPAGO = cajaPago.cta_abastecimiento,
                    VRTOTAL = valorConsignadoDecimal,
                    SUMDBCR = 0,
                    FECHARealiz = DateTime.Now,
                    ANULADO = false
                };

                db.Comprobantes.Add(comprobanteNew);
                var HistorialPorPagare = db.HistorialCreditos.Where(x => x.pagare == factOpCajaConsCuotaCredito.pagare).ToList();
                var item = (from pc in db.HistorialCreditos where pc.pagare == factOpCajaConsCuotaCredito.pagare && pc.estado != "pazYsalvo" && pc.estado != "abono" select pc).First();
                decimal aInteresMora = 0;
                decimal aInteresCorriente = 0;
                decimal aSeguros = 0;
                decimal aCapital = 0;
                decimal aCapitalMora = 0;
                decimal newSaldoCapital = item.saldoCapital;
                string estado = item.estado;
                decimal newInteresMora = 0;
                decimal newInteresCorriente = 0;
                decimal newCapitalMora = item.capitalEnMora;

                if (valorConsignadoDecimal >= item.interesMora)
                {
                    aInteresMora = item.interesMora;
                    item.abonoInteresMora = aInteresMora;
                    valorConsignadoDecimal = valorConsignadoDecimal - item.interesMora;
                    //item.interesMora = 0;  
                    newInteresMora = 0;
                    if (valorConsignadoDecimal >= item.interesCorriente)
                    {
                        aInteresCorriente = item.interesCorriente;
                        item.AbonoInteresCorriente = aInteresCorriente;
                        valorConsignadoDecimal = valorConsignadoDecimal - item.interesCorriente;
                        //item.interesCorriente = 0;
                        newInteresCorriente = 0;
                        if (valorConsignadoDecimal >= item.valorCosto)
                        {
                            aSeguros = item.valorCosto;
                            valorConsignadoDecimal = valorConsignadoDecimal - item.valorCosto;
                            //item.valorCosto = 0;

                            if (valorConsignadoDecimal >= item.capitalEnMora)
                            {
                                aCapitalMora = item.capitalEnMora;
                                valorConsignadoDecimal = valorConsignadoDecimal - item.capitalEnMora;
                                newCapitalMora = 0;
                                //item.saldoCapital = item.saldoCapital - valorConsignadoDecimal;
                                aCapital = valorConsignadoDecimal;
                                newSaldoCapital = item.saldoCapital - aCapital - aCapitalMora;
                                item.abonoCapital = aCapital + aCapitalMora;
                            }
                            else
                            {
                                aCapital = valorConsignadoDecimal;
                                item.abonoCapital = aCapital;
                                newSaldoCapital = item.saldoCapital - aCapital;
                                newCapitalMora = item.capitalEnMora - aCapital;
                            }
                        }
                        else
                        {
                            item.valorCosto = item.valorCosto - valorConsignadoDecimal;
                            aSeguros = valorConsignadoDecimal;
                        }
                    }
                    else
                    {
                        newInteresCorriente = item.interesCorriente - valorConsignadoDecimal;
                        aInteresCorriente = valorConsignadoDecimal;
                        item.AbonoInteresCorriente = aInteresCorriente;
                    }
                }
                else
                {
                    newInteresMora = item.interesMora - valorConsignadoDecimal;
                    aInteresMora = valorConsignadoDecimal;
                    item.abonoInteresMora = aInteresMora;
                }

                item.estado = "abono";

                var nuevoHistorial = new HistorialCreditos()
                {
                    fecha = DateTime.Now,
                    idFactura = 0,
                    NIT = item.NIT,
                    pagare = item.pagare,
                    abonoCapital = 0,
                    abonoInteresMora = 0,
                    AbonoInteresCorriente = 0,
                    valorCosto = 0,
                    saldoCapital = newSaldoCapital,
                    proximaCuota = item.proximaCuota,
                    capitalEnMora = newCapitalMora,
                    TazaInteresMora = item.TazaInteresMora,
                    TazaInteresCorriente = item.TazaInteresCorriente,
                    diasCausados = item.diasCausados,
                    diasEnMora = item.diasEnMora,
                    numeroCuota = item.numeroCuota,
                    fechaProximoPago = item.fechaProximoPago,
                    interesCorrienteMora = item.interesCorrienteMora,
                    interesCorriente = newInteresCorriente,
                    estado = estado,
                    interesMora = newInteresMora
                };
                db.HistorialCreditos.Add(nuevoHistorial);

                //CONSTRUIR LA LISTA DE MOVIMIENTOS
                List<Movimiento> listaDeMovimientos = new List<Movimiento>();
                //CAJA
                var mov1 = new Movimiento()
                {
                    TIPO = cuenta.TipoComprobante,
                    NUMERO = Comprobante.CONSECUTIVO,
                    CUENTA = cajaPago.cta_abastecimiento,
                    TERCERO = factOpCajaConsCuotaCredito.NIT,
                    DETALLE = "CONSIGNACION CUOTA CREDITO",
                    DEBITO = nuevoAbonoCapitalInt,
                    CREDITO = 0,
                    BASE = 0,
                    CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                    FECHAMOVIMIENTO = DateTime.Now,
                    DOCUMENTO = item.id.ToString()
                };
                listaDeMovimientos.Add(mov1);

                if (aCapital > 0)
                {
                    //CAPITAL
                    var cuentaCapital = (from pc in db.Cuentas where pc.Funcion == "F1" select pc.Cuenta_Cod).Single();
                    var mov2 = new Movimiento()
                    {
                        TIPO = cuenta.TipoComprobante,
                        NUMERO = Comprobante.CONSECUTIVO,
                        CUENTA = cuentaCapital,
                        TERCERO = factOpCajaConsCuotaCredito.NIT,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        DEBITO = 0,
                        CREDITO = aCapital,
                        BASE = 0,
                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now,
                        DOCUMENTO = item.id.ToString()
                    };
                    listaDeMovimientos.Add(mov2);
                }

                if (aInteresCorriente > 0)
                {
                    //INTERESES
                    var cuentaIntereses = cuenta.Cuenta_Cod;
                    var mov3 = new Movimiento()
                    {
                        TIPO = cuenta.TipoComprobante,
                        NUMERO = Comprobante.CONSECUTIVO,
                        CUENTA = cuentaIntereses,
                        TERCERO = factOpCajaConsCuotaCredito.NIT,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        DEBITO = 0,
                        CREDITO = aInteresCorriente,
                        BASE = 0,
                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now,
                        DOCUMENTO = item.id.ToString()
                    };
                    listaDeMovimientos.Add(mov3);
                }

                if (aInteresMora > 0)
                {
                    //INTERESES MORA
                    var cuentaInteresMora = (from pc in db.Cuentas where pc.Funcion == "F6" select pc).Single();

                    var mov4 = new Movimiento()
                    {
                        TIPO = cuenta.TipoComprobante,
                        NUMERO = Comprobante.CONSECUTIVO,
                        CUENTA = cuentaInteresMora.Cuenta_Cod,
                        TERCERO = factOpCajaConsCuotaCredito.NIT,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        DEBITO = 0,
                        CREDITO = aInteresMora,
                        BASE = 0,
                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now,
                        DOCUMENTO = item.id.ToString()
                    };
                    listaDeMovimientos.Add(mov4);
                }

                if (aSeguros > 0)
                {
                    //SEGURO
                    var cuentaSeguro = (from pc in db.Cuentas where pc.Funcion == "F9" select pc.Cuenta_Cod).Single();

                    var mov5 = new Movimiento()
                    {
                        TIPO = cuenta.TipoComprobante,
                        NUMERO = Comprobante.CONSECUTIVO,
                        CUENTA = cuentaSeguro,
                        TERCERO = factOpCajaConsCuotaCredito.NIT,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        DEBITO = 0,
                        CREDITO = aSeguros,
                        BASE = 0,
                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now,
                        DOCUMENTO = item.id.ToString()
                    };
                    listaDeMovimientos.Add(mov5);
                }

                var result = false;

                var comprobanteConst = new ComprobanteBO();
                result = comprobanteConst.AsentarConsignacionCaja(listaDeMovimientos, Convert.ToInt32(Comprobante.CONSECUTIVO), cuenta.TipoComprobante);

                if (result)
                {
                    factOpCajaConsCuotaCredito.pagare = credito.Pagare;
                    factOpCajaConsCuotaCredito.abonoCapital = Convert.ToString(aCapital + aCapitalMora);
                    factOpCajaConsCuotaCredito.interesCorriente = Convert.ToString(aInteresCorriente);
                    factOpCajaConsCuotaCredito.interesMora = Convert.ToString(aInteresMora);
                    factOpCajaConsCuotaCredito.seguros = Convert.ToString(aSeguros);
                    if (newSaldoCapital < 0)
                    {
                        newSaldoCapital = 0;
                    }
                    factOpCajaConsCuotaCredito.saldoCapital = Convert.ToString(newSaldoCapital);
                    string totalCuotas = Convert.ToString(credito.Creditos_Plazo);

                    factOpCajaConsCuotaCredito.valorConsignado = nuevoAbonoCapital;

                    db.factOpCajaConsCuotaCredito.Add(factOpCajaConsCuotaCredito);

                    db.SaveChanges();
                }

                return RedirectToAction("DetailsConsAbonoCredito/" + factOpCajaConsCuotaCredito.id);
            }
            else
            {
                factOpCajaConsCuotaCredito.valorConsignado = factOpCajaConsCuotaCredito.valorConsignado.Replace(".", ",");
                var valorConsignadoDecimal = Convert.ToDecimal(factOpCajaConsCuotaCredito.valorConsignado);
                //sI consecutivo actual es igual al final serie inclementa en uno
                if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
                {
                    string cod_caja = Session["cod_caja" + User.Identity.Name].ToString();
                    var updatecajaConsecutivo = db.Caja.Where(x => x.Codigo_caja == cod_caja).FirstOrDefault();
                    updatecajaConsecutivo.consecutivo_actual = updatecajaConsecutivo.Consecutivo_ini;
                    updatecajaConsecutivo.Serie += 1;
                    db.Entry(updatecajaConsecutivo).State = System.Data.Entity.EntityState.Modified;
                    //var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja"] + "'";
                    //db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                }
                else// si no es igual solo actualiza consecutivo de factura
                {
                    var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                    db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                }

                //Actualizar el cuadre y tope de caja
                string sesionFecha = Session["fecha" + User.Identity.Name].ToString();
                var updatecuadre = db.CuadreCajaPorCajero.Where(x => x.fecha == sesionFecha && x.codigo_caja == factOpCajaConsCuotaCredito.codigoCaja && x.nit_cajero == factOpCajaConsCuotaCredito.nitCajero && x.cierre == 0).FirstOrDefault();
                updatecuadre.tope += Convert.ToDecimal(factOpCajaConsCuotaCredito.valorConsignado);
                db.Entry(updatecuadre).State = System.Data.Entity.EntityState.Modified;

                //var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET tope=tope+" + factOpCajaConsCuotaCredito.valorConsignado + "WHERE fecha='" + Session["fecha"] + "'AND codigo_caja='" + factOpCajaConsCuotaCredito.codigoCaja + "' AND nit_cajero='" + factOpCajaConsCuotaCredito.nitCajero + "' AND cierre=0";
                //db.Database.ExecuteSqlCommand(updatecuadre);
                var updatetopecaja = db.Caja.Where(x => x.Codigo_caja == factOpCajaConsCuotaCredito.codigoCaja).FirstOrDefault();
                updatetopecaja.TopeMaximo_caja += Convert.ToDouble(factOpCajaConsCuotaCredito.valorConsignado);
                db.Entry(updatetopecaja).State = System.Data.Entity.EntityState.Modified;
                //var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja+" + factOpCajaConsCuotaCredito.valorConsignado + "WHERE Codigo_caja='" + factOpCajaConsCuotaCredito.codigoCaja + "'";
                //db.Database.ExecuteSqlCommand(updatetopecaja);


                //CONTRUIR EL COMPROBANTE
                factOpCajaConsCuotaCredito.valorConsignado = factOpCajaConsCuotaCredito.valorConsignado.Replace(".", ",");
                var cuenta = (from pc in db.Cuentas where pc.Funcion == "F4" select pc).Single();
                var Comprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == cuenta.TipoComprobante & x.INACTIVO == false);
                var cajaPago = (from pc in db.Caja where pc.Codigo_caja == factOpCajaConsCuotaCredito.codigoCaja select pc).Single();
                var valorConsignadoEnDecimal = Convert.ToDecimal(factOpCajaConsCuotaCredito.valorConsignado);
                var credito = (from pc in db.Creditos where pc.Pagare == factOpCajaConsCuotaCredito.pagare select pc).Single();
                var comprobanteNew = new Comprobante()
                {
                    TIPO = cuenta.TipoComprobante,
                    NUMERO = Comprobante.CONSECUTIVO,
                    ANO = Convert.ToString(DateTime.Now.Year),
                    MES = Convert.ToString(DateTime.Now.Month),
                    DIA = Convert.ToString(DateTime.Now.Day),
                    CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                    DETALLE = "CONSIGNACION CUOTA CREDITO",
                    TERCERO = factOpCajaConsCuotaCredito.NIT,
                    CTAFPAGO = cajaPago.cta_abastecimiento,
                    VRTOTAL = valorConsignadoEnDecimal,
                    SUMDBCR = 0,
                    FECHARealiz = DateTime.Now,
                    ANULADO = false
                };

                db.Comprobantes.Add(comprobanteNew);
                var HistorialPorPagare = db.HistorialCreditos.Where(x => x.pagare == factOpCajaConsCuotaCredito.pagare).ToList();
                var item = (from pc in db.HistorialCreditos where pc.pagare == factOpCajaConsCuotaCredito.pagare && pc.estado != "pazYsalvo" && pc.estado != "abono" select pc).First();
                decimal aInteresMora = 0;
                decimal aInteresCorriente = 0;
                decimal aSeguros = 0;
                decimal aCapital = 0;
                decimal newSaldoCapital = 0;
                var numeroCuota = 0;

                //foreach (var his in HistorialPorPagare)
                //{
                //    if (his.estado == "enMora")
                //    {
                //        aInteresMora = aInteresMora + his.interesMora;
                //    }
                //}

                if (item.estado == "enMora")
                {
                    aInteresCorriente = item.interesCorriente + aInteresCorriente;
                    aSeguros = item.valorCosto + aSeguros;
                    aCapital = item.capitalEnMora + aCapital;
                    aInteresMora = item.interesMora;
                    newSaldoCapital = item.saldoCapital - aCapital;
                    numeroCuota = item.numeroCuota;

                    item.fecha = DateTime.Now;
                    item.idFactura = 0;
                    item.abonoCapital = item.capitalEnMora;
                    item.abonoInteresMora = aInteresMora;
                    item.AbonoInteresCorriente = item.interesCorriente;
                    item.estado = "pazYsalvo";

                    var HistorialPorPagare2 = db.HistorialCreditos.Where(x => x.pagare == factOpCajaConsCuotaCredito.pagare).ToList();
                    foreach (var his in HistorialPorPagare2)
                    {
                        if (his.interesMora < item.interesMora)
                        {
                            his.interesMora = 0;
                        }
                    }

                    if (newSaldoCapital <= 0)
                    {
                        var dataHistorial = HistorialPorPagare.Where(x => x.estado == "normal").FirstOrDefault();
                        if (dataHistorial.saldoCapital <= 0 && dataHistorial.numeroCuota == 0)
                        {
                            var updateHis = db.HistorialCreditos.Find(dataHistorial.id);
                            updateHis.estado = "liquidado";
                            db.Entry(updateHis).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }
                else if (item.estado == "diasTerminados")
                {
                    aInteresCorriente = item.interesCorriente;
                    aSeguros = item.valorCosto + aSeguros;

                    var interesCorriente = item.interesCorriente;
                    var abonoCapital = item.proximaCuota - interesCorriente - item.valorCosto;
                    aCapital = abonoCapital + aCapital;
                    newSaldoCapital = item.saldoCapital - aCapital;

                    var fechaProxPago = item.fechaProximoPago.AddMonths(1);

                    item.fecha = DateTime.Now;
                    item.idFactura = 0;
                    item.abonoCapital = abonoCapital;
                    item.AbonoInteresCorriente = interesCorriente;
                    item.estado = "pazYsalvo";
                    numeroCuota = item.numeroCuota;

                    if(newSaldoCapital<=0)
                    {
                        var dataHistorial = HistorialPorPagare.Where(x => x.estado == "normal").FirstOrDefault();
                        if (dataHistorial.saldoCapital <= 0 && dataHistorial.numeroCuota == 0)
                        {
                            var updateHis = db.HistorialCreditos.Find(dataHistorial.id);
                            updateHis.estado = "liquidado";
                            db.Entry(updateHis).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    

                }
                else if (item.estado == "normal")
                {
                    var prestamo = (from d in db.Prestamos where d.Pagare == item.pagare select d).First();
                    aInteresCorriente = item.interesCorriente + aInteresCorriente;
                    aSeguros = item.valorCosto + aSeguros;

                    var interesCorriente = item.interesCorriente;
                    //Jme
                    // var abonoCapital = item.proximaCuota - interesCorriente - item.valorCosto;
                    var valorCuota = factOpCajaConsCuotaCredito.valorConsignado.Replace('.',',');
                    var vrCuota = Convert.ToDecimal(valorCuota);
                    var abonoCapital = vrCuota - interesCorriente - item.valorCosto;
                    aCapital = abonoCapital + aCapital;
                    newSaldoCapital = item.saldoCapital - aCapital;

                   

                    var nuevoSaldoCapital = item.saldoCapital - abonoCapital;
                    var interescorrienteNew = Convert.ToDecimal(nuevoSaldoCapital) * (item.TazaInteresCorriente / 100);
                   var fechaProxPago = item.fechaProximoPago.AddMonths(1);

                    item.fecha = DateTime.Now;
                    item.idFactura = 0;
                    item.abonoCapital = abonoCapital;
                    item.AbonoInteresCorriente = interesCorriente;
                    item.estado = "pazYsalvo";
                    numeroCuota = item.numeroCuota;

                    if(newSaldoCapital<=0)
                    {
                        var nuevoHistorial = new HistorialCreditos()
                            {
                                fecha = DateTime.Now,
                                idFactura = 0,
                                NIT = item.NIT,
                                pagare = item.pagare,
                                abonoCapital = 0,
                                abonoInteresMora = 0,
                                AbonoInteresCorriente = 0,
                                valorCosto = 0,
                                saldoCapital = 0,
                                proximaCuota = 0,
                                capitalEnMora = 0,
                                TazaInteresMora = 0,
                                TazaInteresCorriente = 0,
                                diasCausados = 0,
                                diasEnMora = 0,
                                numeroCuota = 0,
                                fechaProximoPago = fechaProxPago,
                                interesCorrienteMora = 0,
                                interesCorriente = 0,
                                estado = "liquidado",
                                interesMora = 0
                            };
                        db.HistorialCreditos.Add(nuevoHistorial);
                        
                    }
                    else
                    {
                        var nuevoHistorial = new HistorialCreditos()
                        {
                            fecha = DateTime.Now,
                            idFactura = 0,
                            NIT = item.NIT,
                            pagare = item.pagare,
                            abonoCapital = 0,
                            abonoInteresMora = 0,
                            AbonoInteresCorriente = 0,
                            valorCosto = prestamo.costoAdicionalEnEltiempo,
                            saldoCapital = nuevoSaldoCapital,
                            proximaCuota = item.proximaCuota,
                            capitalEnMora = 0,
                            TazaInteresMora = item.TazaInteresMora,
                            TazaInteresCorriente = item.TazaInteresCorriente,
                            diasCausados = 0,
                            diasEnMora = 0,
                            numeroCuota = item.numeroCuota + 1,
                            fechaProximoPago = fechaProxPago,
                            interesCorrienteMora = 0,
                            interesCorriente = interescorrienteNew,
                            estado = "normal",
                            interesMora = 0
                        };
                        db.HistorialCreditos.Add(nuevoHistorial);
                    }
                    
                }


                //CONSTRUIR LA LISTA DE MOVIMIENTOS
                List<Movimiento> listaDeMovimientos = new List<Movimiento>();
                //var paraCaja = Convert.ToDecimal(factOpCajaConsCuotaCredito.valorConsignado);
                var paraCaja = vrCuotaCredito;
                //CAJA
                var mov1 = new Movimiento()
                {
                    TIPO = cuenta.TipoComprobante,
                    NUMERO = Comprobante.CONSECUTIVO,
                    CUENTA = cajaPago.cta_abastecimiento,
                    TERCERO = factOpCajaConsCuotaCredito.NIT,
                    DETALLE = "CONSIGNACION CUOTA CREDITO",
                    DEBITO = paraCaja,
                    CREDITO = 0,
                    BASE = 0,
                    CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                    FECHAMOVIMIENTO = DateTime.Now,
                    DOCUMENTO = item.id.ToString()
                };
                listaDeMovimientos.Add(mov1);

                if (aCapital > 0)
                {
                    //CAPITAL
                    var cuentaCapital = (from pc in db.Cuentas where pc.Funcion == "F1" select pc.Cuenta_Cod).Single();
                    var mov2 = new Movimiento()
                    {
                        TIPO = cuenta.TipoComprobante,
                        NUMERO = Comprobante.CONSECUTIVO,
                        CUENTA = cuentaCapital,
                        TERCERO = factOpCajaConsCuotaCredito.NIT,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        DEBITO = 0,
                        CREDITO = aCapital,
                        BASE = 0,
                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now,
                        DOCUMENTO = item.id.ToString()
                    };
                    listaDeMovimientos.Add(mov2);
                }

                if (aInteresCorriente > 0)
                {
                    //INTERESES
                    var cuentaIntereses = cuenta.Cuenta_Cod;
                    var mov3 = new Movimiento()
                    {
                        TIPO = cuenta.TipoComprobante,
                        NUMERO = Comprobante.CONSECUTIVO,
                        CUENTA = cuentaIntereses,
                        TERCERO = factOpCajaConsCuotaCredito.NIT,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        DEBITO = 0,
                        CREDITO = aInteresCorriente,
                        BASE = 0,
                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now,
                        DOCUMENTO = item.id.ToString()
                    };
                    listaDeMovimientos.Add(mov3);
                }

                if (aInteresMora > 0)
                {
                    //INTERESES MORA
                    var cuentaInteresMora = (from pc in db.Cuentas where pc.Funcion == "F6" select pc).Single();
                    var mov4 = new Movimiento()
                    {
                        TIPO = cuenta.TipoComprobante,
                        NUMERO = Comprobante.CONSECUTIVO,
                        CUENTA = cuentaInteresMora.Cuenta_Cod,
                        TERCERO = factOpCajaConsCuotaCredito.NIT,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        DEBITO = 0,
                        CREDITO = aInteresMora,
                        BASE = 0,
                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now,
                        DOCUMENTO = item.id.ToString()
                    };
                    listaDeMovimientos.Add(mov4);
                }

                if (aSeguros > 0)
                {
                    var cuentaSeguro = (from pc in db.Cuentas where pc.Funcion == "F9" select pc.Cuenta_Cod).Single();
                    var mov5 = new Movimiento()
                    {
                        TIPO = cuenta.TipoComprobante,
                        NUMERO = Comprobante.CONSECUTIVO,
                        CUENTA = cuentaSeguro,
                        TERCERO = factOpCajaConsCuotaCredito.NIT,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        DEBITO = 0,
                        CREDITO = aSeguros,
                        BASE = 0,
                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now,
                    };
                    listaDeMovimientos.Add(mov5);
                }

                var result = false;

                var comprobanteConst = new ComprobanteBO();
                result = comprobanteConst.AsentarConsignacionCaja(listaDeMovimientos, Convert.ToInt32(Comprobante.CONSECUTIVO), cuenta.TipoComprobante);

                if (result)
                {
                    factOpCajaConsCuotaCredito.pagare = credito.Pagare;
                    factOpCajaConsCuotaCredito.abonoCapital = Convert.ToString(aCapital);
                    factOpCajaConsCuotaCredito.interesCorriente = Convert.ToString(aInteresCorriente);
                    factOpCajaConsCuotaCredito.interesMora = Convert.ToString(aInteresMora);
                    factOpCajaConsCuotaCredito.seguros = Convert.ToString(aSeguros);
                    if(newSaldoCapital < 0)
                    {
                        newSaldoCapital = 0;
                    }
                    factOpCajaConsCuotaCredito.saldoCapital = Convert.ToString(newSaldoCapital);
                    string totalCuotas = Convert.ToString(credito.Creditos_Plazo);

                    factOpCajaConsCuotaCredito.numeroCuota = (numeroCuota) + "/" + totalCuotas;

                    factOpCajaConsCuotaCredito.valorConsignado = vrCuotaCredito.ToString();

                    db.factOpCajaConsCuotaCredito.Add(factOpCajaConsCuotaCredito);

                    db.SaveChanges();
                }

                return RedirectToAction("DetailsConsCuotaCredito/" + factOpCajaConsCuotaCredito.id);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConsCuotaCreditoEntidadDos([Bind(Include = "id,fecha,factura,cedula,codigoCaja,valorResibido,valorConsignado,nitCajero")] factOpCajaConsCuotaCreditoEntidadDos paramFactOpCaja, string paraquenodeerror)
        {
            //sI consecutivo actual es igual al final serie inclementa en uno
            if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
            {
                var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            }
            else// si no es igual solo actualiza consecutivo de factura
            {
                var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            }

            //Actualizar el cuadre y tope de caja
            var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET tope=tope+" + paramFactOpCaja.valorConsignado + "WHERE fecha='" + Session["fecha" + User.Identity.Name] + "'AND codigo_caja='" + paramFactOpCaja.codigoCaja + "' AND nit_cajero='" + paramFactOpCaja.nitCajero + "' AND cierre=0";
            db.Database.ExecuteSqlCommand(updatecuadre);
            var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja+" + paramFactOpCaja.valorConsignado + "WHERE Codigo_caja='" + paramFactOpCaja.codigoCaja + "'";
            db.Database.ExecuteSqlCommand(updatetopecaja);

            var valorTotalenDecimal = Convert.ToDecimal(paramFactOpCaja.valorConsignado);
            var cajaPago = (from pc in db.Caja where pc.Codigo_caja == paramFactOpCaja.codigoCaja select pc).Single();
            //CONTRUIR EL COMPROBANTE

            var Comprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == "RC1" & x.INACTIVO == false);

            var comprobanteNew = new Comprobante()
            {
                TIPO = "RC1",
                NUMERO = Comprobante.CONSECUTIVO,
                ANO = Convert.ToString(DateTime.Now.Year),
                MES = Convert.ToString(DateTime.Now.Month),
                DIA = Convert.ToString(DateTime.Now.Day),
                CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                DETALLE = "CONSIGNACION CUOTA CREDITO CREDIEMPRENDER",
                TERCERO = paramFactOpCaja.cedula,
                CTAFPAGO = cajaPago.cta_abastecimiento,
                VRTOTAL = valorTotalenDecimal,
                SUMDBCR = 0,
                FECHARealiz = DateTime.Now,
                ANULADO = false
            };

            db.Comprobantes.Add(comprobanteNew);

            //CONSTRUIR LA LISTA DE MOVIMIENTOS
            List<Movimiento> listaDeMovimientos = new List<Movimiento>();

            //CAJA
            var mov1 = new Movimiento()
            {
                TIPO = "RC1",
                NUMERO = Comprobante.CONSECUTIVO,
                CUENTA = cajaPago.cta_abastecimiento,
                TERCERO = paramFactOpCaja.cedula,
                DETALLE = "CONSIGNACION CUOTA CREDITO CREDIEMPRENDER",
                DEBITO = valorTotalenDecimal,
                CREDITO = 0,
                BASE = 0,
                CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                FECHAMOVIMIENTO = DateTime.Now,
            };

            listaDeMovimientos.Add(mov1);

            //CONTRAPARTIDA
            var mov2 = new Movimiento()
            {
                TIPO = "RC1",
                NUMERO = Comprobante.CONSECUTIVO,
                CUENTA = "260505001",
                TERCERO = paramFactOpCaja.cedula,
                DETALLE = "CONSIGNACION CUOTA CREDITO",
                DEBITO = 0,
                CREDITO = valorTotalenDecimal,
                BASE = 0,
                CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
                FECHAMOVIMIENTO = DateTime.Now,
            };
            listaDeMovimientos.Add(mov2);

            var result = false;

            var comprobanteConst = new ComprobanteBO();
            result = comprobanteConst.AsentarConsignacionCaja(listaDeMovimientos, Convert.ToInt32(Comprobante.CONSECUTIVO), "RC1");

            if (result)
            {
                paramFactOpCaja.fecha = DateTime.Now;
                db.factOpCajaConsCuotaCreditoEntidadDos.Add(paramFactOpCaja);
                db.SaveChanges();
            }

            return RedirectToAction("DetailsConsCuotaCreditoEntidadDos/" + paramFactOpCaja.id);
        }

        public ActionResult CreateDesembolso()
        {
            if (Session["nit" + User.Identity.Name] != null)// para evitar Visualizacion de los registros de las facturas sin haberse registrado al cajero 
            {
                var fecha = DateTime.Now;
                var nitAsociado = Session["cuenta" + User.Identity.Name].ToString();
                var factura = Session["Factura" + User.Identity.Name].ToString();
                var codCaja = Session["cod_caja" + User.Identity.Name].ToString();
                var nitCajero = Session["nit" + User.Identity.Name].ToString();
                var pagare = (from pc in db.Creditos where pc.Creditos_Cedula == nitAsociado select pc.Pagare).Single();
                var prestamo = (from pc in db.Prestamos where pc.Pagare == pagare select pc).Single();
                var cantidadDeCostosPrestamos = (from pc in db.CostosPrestamos where pc.Pagare == pagare select pc).Count();
                var sumaDeCostos = 0;
                var Tercero = (from pc in db.Terceros where pc.NIT == nitAsociado select pc).Single();
                ViewBag.NombreTercero = Tercero.NOMBRE1 + " " + Tercero.NOMBRE2 + " " + Tercero.APELLIDO1 + " " + Tercero.APELLIDO2;

                if (cantidadDeCostosPrestamos != 0)
                {
                    var listaDeCostos = db.CostosPrestamos.Where(a => a.Pagare == pagare).ToList();
                    foreach (var costo in listaDeCostos)
                    {
                        if (costo.seCobraComo == 4)
                        {
                            var costoAdicional = db.Costos_Adicionales.Where(b => b.CA_Id == costo.CA_Id).Single();

                            var cuentaCosto = Convert.ToString(costoAdicional.Cuenta_Cod);
                            var valorCosto = Convert.ToDecimal(costoAdicional.CA_Valor);

                            sumaDeCostos = sumaDeCostos + Convert.ToInt32(valorCosto);
                        }
                        else if (costo.seCobraComo == 5)
                        {
                            var costoAdicional = db.Costos_Adicionales.Where(b => b.CA_Id == costo.CA_Id).Single();
                            var cuentaCosto = Convert.ToString(costoAdicional.Cuenta_Cod);
                            var valorPorcentaje = Convert.ToDecimal(costoAdicional.CA_Porcentaje);
                            valorPorcentaje = (valorPorcentaje / 100) * prestamo.Capital;
                            sumaDeCostos = sumaDeCostos + Convert.ToInt32(valorPorcentaje);
                        }
                    }
                }
                var valorDesembolsado = prestamo.Capital - sumaDeCostos;

                var VarFactOpcajaDesembolso = new factOpCajaDesembolsos()
                {
                    fecha = fecha,
                    factura = factura,
                    NIT = nitAsociado,
                    codigoCaja = codCaja,
                    pagare = pagare,
                    valorDesembolsado = Convert.ToString(valorDesembolsado),
                    nitCajero = nitCajero
                };

                return View(VarFactOpcajaDesembolso);
            }
            else
            {
                return RedirectToAction("Logopcaja");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult CreateDesembolso([Bind(Include = "id,fecha,factura,NIT,codigoCaja,pagare,valorDesembolsado,nitCajero")] factOpCajaDesembolsos factOpCajaDesembolsos)
        {
            //sI consecutivo actual es igual al final serie inclementa en uno
            if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
            {
                var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            }
            else// si no es igual solo actualiza consecutivo de factura
            {
                var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
                db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            }

            //Actualizar el cuadre y tope de caja
            var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET tope=tope-" + factOpCajaDesembolsos.valorDesembolsado + "WHERE fecha='" + Session["fecha" + User.Identity.Name] + "'AND codigo_caja='" + factOpCajaDesembolsos.codigoCaja + "' AND nit_cajero='" + factOpCajaDesembolsos.nitCajero + "' AND cierre=0";
            db.Database.ExecuteSqlCommand(updatecuadre);
            var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja-" + factOpCajaDesembolsos.valorDesembolsado + "WHERE Codigo_caja='" + factOpCajaDesembolsos.codigoCaja + "'";
            db.Database.ExecuteSqlCommand(updatetopecaja);

            var credito = (from pc in db.Creditos where pc.Pagare == factOpCajaDesembolsos.pagare select pc).Single();
            credito.Creditos_Estado = true;
            db.Entry(credito).State = System.Data.Entity.EntityState.Modified;

            db.factOpCajaDesembolso.Add(factOpCajaDesembolsos);
            db.SaveChanges();
            return RedirectToAction("DetailsDesembolso/" + factOpCajaDesembolsos.id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCons(FactOpcaja factOpcaja)
        {
            #region old code
            //factOpcaja.total = factOpcaja.valor_efectivo;
            ////por si la casilla en la consignacion queda en blanco se pone automatico el cero(0)
            //if (factOpcaja.valor_cheque == null)
            //{
            //    factOpcaja.valor_cheque = 0;
            //}
            //if (factOpcaja.valor_cheque1 == null)
            //{
            //    factOpcaja.valor_cheque1 = 0;
            //}
            //if (factOpcaja.valor_cheque2 == null)
            //{
            //    factOpcaja.valor_cheque2 = 0;
            //}
            //if (factOpcaja.valor_cheque3 == null)
            //{
            //    factOpcaja.valor_cheque3 = 0;
            //}
            //if (factOpcaja.valor_cheque4 == null)
            //{
            //    factOpcaja.valor_cheque4 = 0;
            //}
            //if (factOpcaja.valor_cheque5 == null)
            //{
            //    factOpcaja.valor_cheque5 = 0;
            //}


            //string fpago = "";
            //if (ModelState.IsValid)
            //{
            //    if (factOpcaja.vueltas >= 0)
            //    {
            //        string ano = Convert.ToString(DateTime.Now.Year);
            //        string mes = Convert.ToString(DateTime.Now.Month);
            //        string dia = Convert.ToString(DateTime.Now.Day);
            //        string fechaOp = Convert.ToString(DateTime.Now);
            //        // es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas


            //        if ((factOpcaja.valor_efectivo != 0 && factOpcaja.valor_efectivo != null) && (factOpcaja.total_cheques != 0 && factOpcaja.total_cheques != null))
            //        {
            //            //sI consecutivo actual es igual al final serie inclementa en uno
            //            if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
            //            {
            //                var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
            //                db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            //            }
            //            else// si no es igual solo actualiza consecutivo de factura
            //            {
            //                var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
            //                db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            //            }


            //            String bandera = Convert.ToString(Session["bandera" + User.Identity.Name]);// bandera para determina tipo de cuenta si es ahorro o aporte

            //            if (bandera == "ahorro")
            //            {
            //                var udateAhorro = "UPDATE aho.FichasAhorros SET totalAhorros=totalAhorros+" + factOpcaja.total + " WHERE numeroCuenta='" + factOpcaja.numero_cuenta + "' AND tipoPago='Caja'";
            //                db.Database.ExecuteSqlCommand(udateAhorro);
            //                fpago = "213005001";// es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas
            //            }
            //            if (bandera == "aporte")
            //            {
            //                var updateAporte = db.FichasAportes.Where(x => x.numeroCuenta == factOpcaja.numero_cuenta && x.tipoPago == "Caja").FirstOrDefault();
            //                updateAporte.totalAportes = Convert.ToInt64((factOpcaja.total) + Convert.ToInt64(updateAporte.totalAportes)).ToString();
            //                db.Entry(updateAporte).State = System.Data.Entity.EntityState.Modified;
            //                //var updateAporte = "UPDATE apo.FichasAportes SET totalAportes=totalAportes+" + factOpcaja.total + " WHERE NumeroCuenta='" + factOpcaja.numero_cuenta + "' AND tipoPago='Caja'";
            //                //db.Database.ExecuteSqlCommand(updateAporte);
            //                fpago = "310505001";// es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas
            //            }
            //            //Actualizar el cuadre y tope de caja
            //            var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET consignacion_aconsignacion_aconsignacion_aconsignacion_aconsignacion_aconsignacion_
            //            efectivo=consignacion_efectivo+" + factOpcaja.valor_efectivo + ", consignacion_cheque=consignacion_cheque+" + factOpcaja.valor_cheque + ",tope=tope+" + factOpcaja.valor_efectivo + "WHERE fecha='" + Session["fecha" + User.Identity.Name] + "'AND codigo_caja='" + factOpcaja.codigo_caja + "' AND nit_cajero='" + factOpcaja.nit_cajero + "' AND cierre=0";
            //            db.Database.ExecuteSqlCommand(updatecuadre);
            //            var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja+" + factOpcaja.valor_efectivo + "WHERE Codigo_caja='" + factOpcaja.codigo_caja + "'";
            //            db.Database.ExecuteSqlCommand(updatetopecaja);
            //            //Actualizamos plan cuentas para las consignaciones en cheque.
            //            var plancuentas = "UPDATE acc.PlanCuentas SET Saldo=Saldo+" + factOpcaja.valor_cheque + "WHERE CODIGO='" + Session["cta_cheque" + User.Identity.Name] + "'";
            //            db.Database.ExecuteSqlCommand(plancuentas);
            //            //****************** operaciones efectivo*****************
            //            //Generar comprobante.
            //            var Comprobante = "INSERT INTO acc.Comprobantes (TIPO, NUMERO, ANO, MES, DIA, CCOSTO, ELIMINADO, DETALLE, TERCERO, FPAGO, CTAFPAGO, NUMEXTERNO, VRTOTAL, SUMDBCR, FECHARealiz, MODIFICA, EXPORTADO, MARCASEG, BLOQUEADO, NUMIMP, PC,USUARIO,ANULADO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + ano + "','" + mes + "','" + dia + "','" + Session["cc_transacciones" + User.Identity.Name] + "',NULL,'CONSIGNACION CAJA','" + factOpcaja.nit_propietario_cuenta + "',NULL,'" + fpago + "',NULL," + factOpcaja.valor_efectivo + ",0,'" + fechaOp + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'False')";
            //            db.Database.ExecuteSqlCommand(Comprobante);
            //            //MOVIMIENTOS 
            //            var movimieno1 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["bandera" + User.Identity.Name] + "',0," + factOpcaja.valor_efectivo + ",0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
            //            db.Database.ExecuteSqlCommand(movimieno1);
            //            var movimieno2 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + Session["cta_efectivo" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["bandera" + User.Identity.Name] + "'," + factOpcaja.valor_efectivo + ",0,0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
            //            db.Database.ExecuteSqlCommand(movimieno2);
            //            //SaldosCCS
            //            var saldosCCS = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_efectivo + "," + factOpcaja.valor_efectivo + ")";
            //            db.Database.ExecuteSqlCommand(saldosCCS);
            //            var saldosCCS1 = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_efectivo" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "'," + factOpcaja.valor_efectivo + ",0," + factOpcaja.valor_efectivo + ")";
            //            db.Database.ExecuteSqlCommand(saldosCCS1);
            //            //saldoscuentas
            //            var saldoscuentas = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_efectivo + "," + factOpcaja.valor_efectivo + ")";
            //            db.Database.ExecuteSqlCommand(saldoscuentas);
            //            var saldoscuentas1 = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_efectivo" + User.Identity.Name] + "','" + ano + "','" + mes + "'," + factOpcaja.valor_efectivo + ",0," + factOpcaja.valor_efectivo + ")";
            //            db.Database.ExecuteSqlCommand(saldoscuentas1);
            //            //sadosterceros
            //            var saldosterceros = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_efectivo + "," + factOpcaja.valor_efectivo + ")";
            //            db.Database.ExecuteSqlCommand(saldosterceros);
            //            var saldosterceros1 = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_efectivo" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "'," + factOpcaja.valor_efectivo + ",0," + factOpcaja.valor_efectivo + ")";
            //            db.Database.ExecuteSqlCommand(saldosterceros1);
            //            //actulizamos consecutivo
            //            var updateconsecutivoTiposComprobantes = "UPDATE acc.TiposComprobantes SET CONSECUTIVO=CONSECUTIVO+1 WHERE CODIGO='" + Session["comp_ingreso" + User.Identity.Name] + "'";
            //            db.Database.ExecuteSqlCommand(updateconsecutivoTiposComprobantes);
            //            //********************************************************
            //            //****************** operaciones cheque*******************
            //            TipoComprobante tiposComprobantes = db.TiposComprobantes.Find(Session["comp_ingreso" + User.Identity.Name]);
            //            Session["consecutivo" + User.Identity.Name] = tiposComprobantes.CONSECUTIVO;
            //            //Generar comprobante.
            //            var Comprobante1 = "INSERT INTO acc.Comprobantes (TIPO, NUMERO, ANO, MES, DIA, CCOSTO, ELIMINADO, DETALLE, TERCERO, FPAGO, CTAFPAGO, NUMEXTERNO, VRTOTAL, SUMDBCR, FECHARealiz, MODIFICA, EXPORTADO, MARCASEG, BLOQUEADO, NUMIMP, PC,USUARIO,ANULADO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + ano + "','" + mes + "','" + dia + "','" + Session["cc_transacciones" + User.Identity.Name] + "',NULL,'CONSIGNACION CHEQUE CAJA','" + factOpcaja.nit_propietario_cuenta + "',NULL,'" + fpago + "',NULL," + factOpcaja.valor_cheque + ",0,'" + fechaOp + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'False')";
            //            db.Database.ExecuteSqlCommand(Comprobante1);
            //            //MOVIMIENTOS 
            //            var movimieno3 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["bandera" + User.Identity.Name] + "',0," + factOpcaja.valor_cheque + ",0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
            //            db.Database.ExecuteSqlCommand(movimieno3);
            //            var movimieno4 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["bandera" + User.Identity.Name] + "'," + factOpcaja.valor_cheque + ",0,0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
            //            db.Database.ExecuteSqlCommand(movimieno4);
            //            //SaldosCCS
            //            var saldosCCS2 = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_cheque + "," + factOpcaja.valor_cheque + ")";
            //            db.Database.ExecuteSqlCommand(saldosCCS2);
            //            var saldosCCS3 = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "'," + factOpcaja.valor_cheque + ",0," + factOpcaja.valor_cheque + ")";
            //            db.Database.ExecuteSqlCommand(saldosCCS3);
            //            //saldoscuentas
            //            var saldoscuentas2 = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_cheque + "," + factOpcaja.valor_cheque + ")";
            //            db.Database.ExecuteSqlCommand(saldoscuentas2);
            //            var saldoscuentas3 = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + ano + "','" + mes + "'," + factOpcaja.valor_cheque + ",0," + factOpcaja.valor_cheque + ")";
            //            db.Database.ExecuteSqlCommand(saldoscuentas3);
            //            //sadosterceros
            //            var saldosterceros2 = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_cheque + "," + factOpcaja.valor_cheque + ")";
            //            db.Database.ExecuteSqlCommand(saldosterceros2);
            //            var saldosterceros3 = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "'," + factOpcaja.valor_cheque + ",0," + factOpcaja.valor_cheque + ")";
            //            db.Database.ExecuteSqlCommand(saldosterceros3);

            //            //********************************************************
            //            //actulizamos consecutivo
            //            var updateconsecutivoTiposComprobantes1 = "UPDATE acc.TiposComprobantes SET CONSECUTIVO=CONSECUTIVO+1 WHERE CODIGO='" + Session["comp_ingreso" + User.Identity.Name] + "'";
            //            db.Database.ExecuteSqlCommand(updateconsecutivoTiposComprobantes1);
            //            var plancuentassaldocheques = "UPDATE acc.PlanCuentas SET Saldo=Saldo+" + factOpcaja.total + " WHERE CODIGO='" + fpago + "'";
            //            db.Database.ExecuteSqlCommand(plancuentassaldocheques);
            //            db.FactOpcaja.Add(factOpcaja);
            //            db.SaveChanges();
            //            return RedirectToAction("Details/" + factOpcaja.id);

            //        }
            //        else
            //        {

            //            if (factOpcaja.total_cheques != 0 && factOpcaja.total_cheques != null)
            //            {
            //                //sI consecutivo actual es igual al final serie inclementa en uno
            //                if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
            //                {
            //                    var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
            //                    db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            //                }
            //                else// si no es igual solo actualiza consecutivo de factura
            //                {
            //                    var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
            //                    db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            //                }


            //                String bandera = Convert.ToString(Session["bandera" + User.Identity.Name]);// bandera para determina tipo de cuenta si es ahorro o aporte

            //                if (bandera == "ahorro")
            //                {
            //                    var udateAhorro = "UPDATE aho.FichasAhorros SET totalAhorros=totalAhorros+" + factOpcaja.total + " WHERE numeroCuenta='" + factOpcaja.numero_cuenta + "' AND tipoPago='Caja'";
            //                    db.Database.ExecuteSqlCommand(udateAhorro);
            //                    fpago = "213005001";// es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas
            //                }
            //                if (bandera == "aporte")
            //                {
            //                    var updateAporte = db.FichasAportes.Where(x => x.numeroCuenta == factOpcaja.numero_cuenta && x.tipoPago == "Caja").FirstOrDefault();
            //                    updateAporte.totalAportes = Convert.ToInt64((factOpcaja.total) + Convert.ToInt64(updateAporte.totalAportes)).ToString();
            //                    db.Entry(updateAporte).State = System.Data.Entity.EntityState.Modified;
            //                    //var updateAporte = "UPDATE apo.FichasAportes SET totalAportes=totalAportes+" + factOpcaja.total + " WHERE NumeroCuenta='" + factOpcaja.numero_cuenta + "' AND tipoPago='Caja'";
            //                    //db.Database.ExecuteSqlCommand(updateAporte);
            //                    fpago = "310505001";// es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas
            //                }
            //                //Actualizar el cuadre y tope de caja
            //                string fechaCuadre = Session["fecha" + User.Identity.Name].ToString();
            //                var updatecuadre = db.CuadreCajaPorCajero.Where(x => x.fecha == fechaCuadre && x.codigo_caja == factOpcaja.codigo_caja && x.nit_cajero == factOpcaja.nit_cajero && x.cierre == 0).FirstOrDefault();
            //                if (updatecuadre != null)
            //                {
            //                    updatecuadre.consignacion_efectivo += factOpcaja.valor_efectivo;
            //                    updatecuadre.consignacion_cheque += factOpcaja.valor_cheque;
            //                    updatecuadre.tope += factOpcaja.valor_efectivo;
            //                    db.Entry(updatecuadre).State = System.Data.Entity.EntityState.Modified;
            //                }
            //                //var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET consignacion_efectivo=consignacion_efectivo+" + factOpcaja.valor_efectivo + ", consignacion_cheque=consignacion_cheque+" + factOpcaja.valor_cheque + ",tope=tope+" + factOpcaja.valor_efectivo + "WHERE fecha='" + Session["fecha" + User.Identity.Name] + "'AND codigo_caja='" + factOpcaja.codigo_caja + "' AND nit_cajero='" + factOpcaja.nit_cajero + "' AND cierre=0";
            //                //db.Database.ExecuteSqlCommand(updatecuadre);

            //                var updatetopecaja = db.Caja.Where(x => x.Codigo_caja == factOpcaja.codigo_caja).FirstOrDefault();
            //                if (updatetopecaja != null)
            //                {
            //                    updatetopecaja.TopeMaximo_caja += Convert.ToDouble(factOpcaja.valor_efectivo);
            //                    db.Entry(updatetopecaja).State = System.Data.Entity.EntityState.Modified;
            //                }
            //                //var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja+" + factOpcaja.valor_efectivo + "WHERE Codigo_caja='" + factOpcaja.codigo_caja + "'";
            //                //db.Database.ExecuteSqlCommand(updatetopecaja);
            //                //Actualizamos plan cuentas para las consignaciones en cheque.
            //                var plancuentas = "UPDATE acc.PlanCuentas SET Saldo=Saldo+" + factOpcaja.valor_cheque + "WHERE CODIGO='" + Session["cta_cheque" + User.Identity.Name] + "'";
            //                db.Database.ExecuteSqlCommand(plancuentas);

            //                //Generar comprobante.
            //                var Comprobante = "INSERT INTO acc.Comprobantes (TIPO, NUMERO, ANO, MES, DIA, CCOSTO, ELIMINADO, DETALLE, TERCERO, FPAGO, CTAFPAGO, NUMEXTERNO, VRTOTAL, SUMDBCR, FECHARealiz, MODIFICA, EXPORTADO, MARCASEG, BLOQUEADO, NUMIMP, PC,USUARIO,ANULADO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + ano + "','" + mes + "','" + dia + "','" + Session["cc_transacciones" + User.Identity.Name] + "',NULL,'CONSIGNACION CHEQUE CAJA','" + factOpcaja.nit_propietario_cuenta + "',NULL,'" + fpago + "',NULL," + factOpcaja.valor_cheque + ",0,'" + fechaOp + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'False')";
            //                db.Database.ExecuteSqlCommand(Comprobante);
            //                //MOVIMIENTOS 
            //                var movimieno1 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["bandera" + User.Identity.Name] + "',0," + factOpcaja.valor_cheque + ",0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
            //                db.Database.ExecuteSqlCommand(movimieno1);
            //                var movimieno2 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('" + Session["comp_ingreso" + User.Identity.Name] + "'," + Session["consecutivo" + User.Identity.Name] + ",'" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["bandera" + User.Identity.Name] + "'," + factOpcaja.valor_cheque + ",0,0,'" + Session["cc_transacciones" + User.Identity.Name] + "','" + fechaOp + "',NULL)";
            //                db.Database.ExecuteSqlCommand(movimieno2);
            //                //SaldosCCS
            //                var saldosCCS = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_cheque + "," + factOpcaja.valor_cheque + ")";
            //                db.Database.ExecuteSqlCommand(saldosCCS);
            //                var saldosCCS1 = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + Session["cc_transacciones" + User.Identity.Name] + "','" + ano + "','" + mes + "'," + factOpcaja.valor_cheque + ",0," + factOpcaja.valor_cheque + ")";
            //                db.Database.ExecuteSqlCommand(saldosCCS1);
            //                //saldoscuentas
            //                var saldoscuentas = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_cheque + "," + factOpcaja.valor_cheque + ")";
            //                db.Database.ExecuteSqlCommand(saldoscuentas);
            //                var saldoscuentas1 = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + ano + "','" + mes + "'," + factOpcaja.valor_cheque + ",0," + factOpcaja.valor_cheque + ")";
            //                db.Database.ExecuteSqlCommand(saldoscuentas1);
            //                //sadosterceros
            //                var saldosterceros = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + fpago + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "',0," + factOpcaja.valor_cheque + "," + factOpcaja.valor_cheque + ")";
            //                db.Database.ExecuteSqlCommand(saldosterceros);
            //                var saldosterceros1 = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('" + Session["cta_cheques" + User.Identity.Name] + "','" + factOpcaja.nit_propietario_cuenta + "','" + ano + "','" + mes + "'," + factOpcaja.valor_cheque + ",0," + factOpcaja.valor_cheque + ")";
            //                db.Database.ExecuteSqlCommand(saldosterceros1);
            //                //actulizamos consecutivo
            //                var updateconsecutivoTiposComprobantes = "UPDATE acc.TiposComprobantes SET CONSECUTIVO=CONSECUTIVO+1 WHERE CODIGO='" + Session["comp_ingreso" + User.Identity.Name] + "'";
            //                var plancuentassaldocheques = "UPDATE acc.PlanCuentas SET Saldo=Saldo+" + factOpcaja.total + " WHERE CODIGO='" + fpago + "'";
            //                db.Database.ExecuteSqlCommand(plancuentassaldocheques);
            //                db.Database.ExecuteSqlCommand(updateconsecutivoTiposComprobantes);
            //                db.FactOpcaja.Add(factOpcaja);
            //                db.SaveChanges();
            //                return RedirectToAction("Details/" + factOpcaja.id);
            //            }
            //            else
            //            {
            //                if (factOpcaja.valor_efectivo != 0 && factOpcaja.valor_efectivo != null)
            //                {

            //                    //sI consecutivo actual es igual al final serie inclementa en uno
            //                    if (Session["actual" + User.Identity.Name] == Session["con_fin" + User.Identity.Name])
            //                    {
            //                        var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
            //                        db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            //                    }
            //                    else// si no es igual solo actualiza consecutivo de factura
            //                    {
            //                        var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja" + User.Identity.Name] + "'";
            //                        db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
            //                    }


            //                    String bandera = Convert.ToString(Session["bandera" + User.Identity.Name]);// bandera para determina tipo de cuenta si es ahorro o aporte

            //                    if (bandera == "ahorro")
            //                    {
            //                        var udateAhorro = "UPDATE aho.FichasAhorros SET totalAhorros=totalAhorros+" + factOpcaja.total + " WHERE numeroCuenta='" + factOpcaja.numero_cuenta + "' AND tipoPago='Caja'";
            //                        db.Database.ExecuteSqlCommand(udateAhorro);
            //                        fpago = "213005001";// es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas
            //                    }
            //                    if (bandera == "aporte")
            //                    {
            //                        var updateAporte = db.FichasAportes.Where(x => x.numeroCuenta == factOpcaja.numero_cuenta && x.tipoPago=="Caja").FirstOrDefault();
            //                        updateAporte.totalAportes = Convert.ToInt64((factOpcaja.total) + Convert.ToInt64(updateAporte.totalAportes)).ToString();
            //                        db.Entry(updateAporte).State = System.Data.Entity.EntityState.Modified;

            //                        //var updateAporte = "UPDATE apo.FichasAportes SET totalAportes=totalAportes+" + factOpcaja.total + " WHERE NumeroCuenta='" + factOpcaja.numero_cuenta + "' AND tipoPago='Caja'";
            //                        //db.Database.ExecuteSqlCommand(updateAporte);
            //                        fpago = "310505001";// es la cuenta que se define para realizar los retirosesta cuentasaleporparte de plancuentas
            //                    }

            //                    //Actualizar el cuadre y tope de caja
            //                    string fechaCuadre = Session["fecha" + User.Identity.Name].ToString();
            //                    var updatecuadre = db.CuadreCajaPorCajero.Where(x => x.fecha == fechaCuadre && x.codigo_caja == factOpcaja.codigo_caja && x.nit_cajero == factOpcaja.nit_cajero && x.cierre == 0).FirstOrDefault();
            //                    if(updatecuadre != null)
            //                    {
            //                        updatecuadre.consignacion_efectivo += factOpcaja.valor_efectivo;
            //                        updatecuadre.consignacion_cheque += factOpcaja.valor_cheque;
            //                        updatecuadre.tope += factOpcaja.valor_efectivo;
            //                        db.Entry(updatecuadre).State = System.Data.Entity.EntityState.Modified;
            //                    }
            //                    //var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET consignacion_efectivo=consignacion_efectivo+" + factOpcaja.valor_efectivo + ", consignacion_cheque=consignacion_cheque+" + factOpcaja.valor_cheque + ",tope=tope+" + factOpcaja.valor_efectivo + "WHERE fecha='" + Session["fecha" + User.Identity.Name] + "'AND codigo_caja='" + factOpcaja.codigo_caja + "' AND nit_cajero='" + factOpcaja.nit_cajero + "' AND cierre=0";
            //                    //db.Database.ExecuteSqlCommand(updatecuadre);

            //                    var updatetopecaja = db.Caja.Where(x => x.Codigo_caja == factOpcaja.codigo_caja).FirstOrDefault();
            //                    if(updatetopecaja!=null)
            //                    {
            //                        updatetopecaja.TopeMaximo_caja += Convert.ToDouble(factOpcaja.valor_efectivo);
            //                        db.Entry(updatetopecaja).State = System.Data.Entity.EntityState.Modified;
            //                    }
            //                    //var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja+" + factOpcaja.valor_efectivo + "WHERE Codigo_caja='" + factOpcaja.codigo_caja + "'";
            //                    //db.Database.ExecuteSqlCommand(updatetopecaja);
            //                    //Actualizamos plan cuentas para las consignaciones en cheque.
            //                    var plancuentas = "UPDATE acc.PlanCuentas SET Saldo=Saldo+" + factOpcaja.valor_cheque + "WHERE CODIGO='" + Session["cta_cheque" + User.Identity.Name] + "'";
            //                    db.Database.ExecuteSqlCommand(plancuentas);


            //                    //CONTRUIR EL COMPROBANTE

            //                    string comprobanteIngreso = Session["comp_ingreso" + User.Identity.Name].ToString();
            //                    var consecutivoComprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == comprobanteIngreso & x.INACTIVO == false);

            //                    var comprobante = new Comprobante()
            //                    {
            //                        TIPO = comprobanteIngreso,
            //                        NUMERO = consecutivoComprobante.CONSECUTIVO,
            //                        ANO = Convert.ToString(DateTime.Now.Year),
            //                        MES = Convert.ToString(DateTime.Now.Month),
            //                        DIA = Convert.ToString(DateTime.Now.Day),
            //                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
            //                        DETALLE = "CONSIGNACION CAJA",
            //                        TERCERO = factOpcaja.nit_propietario_cuenta,
            //                        CTAFPAGO = fpago,
            //                        VRTOTAL = factOpcaja.valor_efectivo,
            //                        SUMDBCR = 0,
            //                        FECHARealiz = DateTime.Now,
            //                        ANULADO = false
            //                    };

            //                    db.Comprobantes.Add(comprobante);

            //                    //CONSTRUIR LA LISTA DE MOVIMIENTOS
            //                    List<Movimiento> listaDeMovimientos = new List<Movimiento>();
            //                    var mov1 = new Movimiento()
            //                    {
            //                        TIPO = comprobanteIngreso,
            //                        NUMERO = consecutivoComprobante.CONSECUTIVO,
            //                        CUENTA = fpago,
            //                        TERCERO = factOpcaja.nit_propietario_cuenta,
            //                        DETALLE = Session["bandera" + User.Identity.Name].ToString(),
            //                        DEBITO = 0,
            //                        CREDITO = factOpcaja.valor_efectivo - 10000,
            //                        BASE = 0,
            //                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
            //                        FECHAMOVIMIENTO = DateTime.Now,
            //                    };

            //                    listaDeMovimientos.Add(mov1);
            //                    var mov2 = new Movimiento()
            //                    {
            //                        TIPO = comprobanteIngreso,
            //                        NUMERO = consecutivoComprobante.CONSECUTIVO,
            //                        CUENTA = Session["cta_efectivo" + User.Identity.Name].ToString(),
            //                        TERCERO = factOpcaja.nit_propietario_cuenta,
            //                        DETALLE = Session["bandera" + User.Identity.Name].ToString(),
            //                        DEBITO = factOpcaja.valor_efectivo,
            //                        CREDITO = 0,
            //                        BASE = 0,
            //                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
            //                        FECHAMOVIMIENTO = DateTime.Now,
            //                    };
            //                    listaDeMovimientos.Add(mov2);

            //                    var mov3 = new Movimiento()
            //                    {
            //                        TIPO = comprobanteIngreso,
            //                        NUMERO = consecutivoComprobante.CONSECUTIVO,
            //                        CUENTA = "416540001",
            //                        TERCERO = factOpcaja.nit_propietario_cuenta,
            //                        DETALLE = Session["bandera" + User.Identity.Name].ToString(),
            //                        DEBITO = 0,
            //                        CREDITO = 10000,
            //                        BASE = 0,
            //                        CCOSTO = Session["cc_transacciones" + User.Identity.Name].ToString(),
            //                        FECHAMOVIMIENTO = DateTime.Now,
            //                    };
            //                    listaDeMovimientos.Add(mov3);

            //                    var result = false;

            //                    var comprobanteConst = new ComprobanteBO();
            //                    result = comprobanteConst.AsentarConsignacionCaja(listaDeMovimientos, Convert.ToInt32(consecutivoComprobante.CONSECUTIVO), comprobanteIngreso);

            //                    if (result)
            //                    {
            //                        db.FactOpcaja.Add(factOpcaja);
            //                        var errors = ModelState.Values.SelectMany(v => v.Errors);
            //                        db.SaveChanges();
            //                    }


            //                    return RedirectToAction("Details/" + factOpcaja.id);
            //                }
            //                else
            //                {
            //                    ModelState.AddModelError("total", "No hay transacciones realizdas");
            //                    ViewBag.err = "No esta relizando ninguna transaccion u operacion";

            //                    ViewBag.nit_consignacion = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion);
            //                    ViewBag.nit_consignacion1 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion1);
            //                    ViewBag.nit_consignacion2 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion2);
            //                    ViewBag.nit_consignacion3 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion3);
            //                    ViewBag.nit_consignacion4 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion4);
            //                    ViewBag.nit_consignacion5 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion5);
            //                    return View(factOpcaja);
            //                }
            //            }

            //        }
            //    }
            //    else
            //    {
            //        ViewBag.err = "Confirme Transaccion compruebe valor aportes";
            //        ViewBag.nit_consignacion = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion);
            //        ViewBag.nit_consignacion1 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion1);
            //        ViewBag.nit_consignacion2 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion2);
            //        ViewBag.nit_consignacion3 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion3);
            //        ViewBag.nit_consignacion4 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion4);
            //        ViewBag.nit_consignacion5 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion5);
            //        return View(factOpcaja);
            //    }

            //}

            #endregion

            var respuesta = new BLLOperacionesCaja().PagarAporte(factOpcaja.numero_cuenta, User.Identity.Name, factOpcaja.valor_efectivo);
            

            ViewBag.nit_consignacion = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion);
            ViewBag.nit_consignacion1 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion1);
            ViewBag.nit_consignacion2 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion2);
            ViewBag.nit_consignacion3 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion3);
            ViewBag.nit_consignacion4 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion4);
            ViewBag.nit_consignacion5 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion5);

            if (respuesta.Status)
            {
                return RedirectToAction("Details/" + respuesta.Id);
            }   
            else
                return View(factOpcaja);
        }


        #region AHORRO CONTRACTUAL

        [Authorize(Roles = "Admin,Opcaja")]
        public async Task<ActionResult> CreateConsAC(string cuenta) //Vista de pago ahorro contractual
        {
            try
            {
                var modelCajaAC = await new BLLOperacionesCaja().GetModelInfoCajaAC(cuenta, User.Identity.Name);
                if (modelCajaAC == null)
                    return RedirectToAction("Logopcaja");

                ViewBag.ModelCajaAC = modelCajaAC;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Logopcaja");
            }

        }

        [Authorize(Roles = "Admin,Opcaja")]
        public async Task<JsonResult> ConsignacionAhorroContractual(string NumeroCuenta,string valor,string observacion)
        {
            var valorPago = Convert.ToDecimal(valor.Replace(".","").Trim());
            var respuesta = await new BLLOperacionesCaja().PagarAhorroAC(NumeroCuenta, User.Identity.Name,valorPago,observacion);

            if (respuesta.Status)
            {
                return new JsonResult { Data = new { status = true, IdFactura=respuesta.Id.ToString() } };
                
            }
            else
                return new JsonResult { Data = new { status = false, mensaje = respuesta.Mensaje } };
            
        }

        #endregion

        // GET: FactOpcajas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactOpcaja factOpcaja = db.FactOpcaja.Find(id);
            if (factOpcaja == null)
            {
                return HttpNotFound();
            }
            ViewBag.codigo_caja = new SelectList(db.Caja, "Codigo_caja", "Nombre_caja", factOpcaja.codigo_caja);
            ViewBag.nit_consignacion = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion);
            ViewBag.nit_consignacion1 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion1);
            ViewBag.nit_consignacion2 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion2);
            ViewBag.nit_consignacion3 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion3);
            ViewBag.nit_consignacion4 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion4);
            ViewBag.nit_consignacion5 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion5);
            ViewBag.nit_cajero = new SelectList(db.configCajero, "Nit_cajero", "Codigo_caja", factOpcaja.nit_cajero);
            ViewBag.nit_cajero = new SelectList(db.Terceros, "NIT", "DIGVER", factOpcaja.nit_cajero);
            return View(factOpcaja);
        }

        [Authorize]
        public ActionResult EditFacturaCredito(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using(var ctx = new AccountingContext())
            {
                var Factura = ctx.factOpCajaConsCuotaCredito.Find(id);
                if(Factura==null)
                {
                    return HttpNotFound();
                }
                return View(Factura);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFacturaCredito(factOpCajaConsCuotaCredito FacturaCredito)
        {
            if(ModelState.IsValid)
            {
                db.Entry(FacturaCredito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("facturasCuotasCreditos");
            }
            return View(FacturaCredito);
        }

        // POST: FactOpcajas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,factura,operacion,codigo_caja,nit_cajero,numero_cuenta,nit_propietario_cuenta,nombre_propietario_cuenta,valor_recibido,valor_efectivo,vueltas,valor_cheque,numero_cheque,consignacion,observacion,saldo_total_cuenta,total,nit_consignacion,valor_cheque1,numero_cheque1,nit_consignacion1,valor_cheque2,numero_cheque2,nit_consignacion2,valor_cheque3,numero_cheque3,nit_consignacion3,valor_cheque4,numero_cheque4,nit_consignacion4,valor_cheque5,numero_cheque5,nit_consignacion5,total_cheques")] FactOpcaja factOpcaja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factOpcaja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.codigo_caja = new SelectList(db.Caja, "Codigo_caja", "Nombre_caja", factOpcaja.codigo_caja);
            ViewBag.nit_consignacion = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion);
            ViewBag.nit_consignacion1 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion1);
            ViewBag.nit_consignacion2 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion2);
            ViewBag.nit_consignacion3 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion3);
            ViewBag.nit_consignacion4 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion4);
            ViewBag.nit_consignacion5 = new SelectList(db.CodigosBanco, "codig_banco", "Banco", factOpcaja.nit_consignacion5);
            ViewBag.nit_cajero = new SelectList(db.configCajero, "Nit_cajero", "Codigo_caja", factOpcaja.nit_cajero);
            ViewBag.nit_cajero = new SelectList(db.Terceros, "NIT", "DIGVER", factOpcaja.nit_cajero);
            return View(factOpcaja);
        }

        // GET: FactOpcajas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactOpcaja factOpcaja = db.FactOpcaja.Find(id);
            if (factOpcaja == null)
            {
                return HttpNotFound();
            }
            return View(factOpcaja);
        }

        // POST: FactOpcajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FactOpcaja factOpcaja = db.FactOpcaja.Find(id);
            db.FactOpcaja.Remove(factOpcaja);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult GetInfoPagare(string Ncuenta)
        {
            if (Ncuenta != "")
            {
                string cedula = (from pc in db.FichasAportes where pc.numeroCuenta == Ncuenta select pc.idPersona).FirstOrDefault();

                if (cedula.Length > 0)
                {
                    /*
                    if (Session["actual"] == Session["con_fin"])
                    {
                        var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=Consecutivo_ini, Serie=serie+1 WHERE Codigo_caja='" + Session["cod_caja"] + "'";
                        db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                    }
                    else// si no es igual solo actualiza consecutivo de factura
                    {
                        var updatecajaConsecutivo = "UPDATE dbo.Caja SET consecutivo_actual=consecutivo_actual+1 WHERE Codigo_caja='" + Session["cod_caja"] + "'";
                        db.Database.ExecuteSqlCommand(updatecajaConsecutivo);
                    }

                    //Actualizar el cuadre y tope de caja
                    var updatecuadre = "UPDATE dbo.CuadreCajaPorCajero SET consignacion_efectivo=consignacion_efectivo+" + factOpcaja.valor_efectivo + ", consignacion_cheque=consignacion_cheque+" + factOpcaja.valor_cheque + ",tope=tope+" + factOpcaja.valor_efectivo + "WHERE fecha='" + Session["fecha"] + "'AND codigo_caja='" + factOpcaja.codigo_caja + "' AND nit_cajero='" + factOpcaja.nit_cajero + "' AND cierre=0";
                    db.Database.ExecuteSqlCommand(updatecuadre);
                    var updatetopecaja = "UPDATE dbo.Caja SET TopeMaximo_caja=TopeMaximo_caja+" + factOpcaja.valor_efectivo + "WHERE Codigo_caja='" + factOpcaja.codigo_caja + "'";
                    db.Database.ExecuteSqlCommand(updatetopecaja);
                    */
                    //string pagare = (from pc in db.Creditos orderby pc.Prestamo_Id descending where pc.Creditos_Cedula == cedula select pc.Pagare).FirstOrDefault(); 
                    int idCredito = (from pc in db.Creditos where pc.Creditos_Cedula == cedula && pc.Creditos_Estado == false select pc.Creditos_Id).FirstOrDefault();

                    if (idCredito > 0)
                    {
                        BCreditos datos = db.Creditos.Find(idCredito);
                        Tercero terceros = db.Terceros.Find(cedula);

                        var response2 = new List<object>
                     {
                        new{

                            pagare=datos.Pagare,
                            ced = cedula,
                            nombre=terceros.NOMBRE+" "+terceros.APELLIDO1+" "+terceros.APELLIDO2,
                            capital=datos.Capital,
                        }
                    };
                        return Json(response2);
                    }
                    else
                    {
                        string respuesta = "NO";
                        return Json(respuesta);
                    }

                }
                else
                {
                    string respuesta = "NO";
                    return Json(respuesta);
                }
            }
            else
            {
                string respuesta = "NO";
                return Json(respuesta);
            }
        }

        [HttpPost]
        public JsonResult VerificaCreditoAsociado(string cuenta)
        {
            var ficha = db.FichasAportes.Where(x => x.numeroCuenta == cuenta).FirstOrDefault();
            var credito = db.Creditos.Where(x => x.Creditos_Cedula == cuenta).ToList();
            string error = "";
            if (credito.Count==0)
            {

                error = "El asociado no tiene créditos pendientes por pagar";
                return new JsonResult { Data = new { status = false, error } };

            }
            else
            {
                return new JsonResult { Data = new { status = true } };

            }

        }

        public JsonResult GetCuentasItem(string tipo, string nit)
        {
            List<Array> cuentas = new List<Array>();
            try
            {
                using (var ctx = new AccountingContext())
                {
                    if (tipo == "0")
                    {
                        return new JsonResult { Data = new { status = false } };
                    }
                    else if (tipo == "Cons_Ahorro" || tipo == "Ret_Ahorro" || tipo == "Ret_Int_Ahorro")
                    {
                        int num = db.FichasAhorros.Where(x => x.idPersona == nit).Count();
                        if (num > 0)
                        {
                            var consultaa = db.FichasAhorros.Where(x => x.idPersona == nit).ToList();
                            foreach (var item in consultaa)
                            {
                                string[] nombres = new string[1];
                                nombres[0] = item.numeroCuenta.ToString();
                                cuentas.Add(nombres);
                            }

                        }
                    }
                    else if (tipo == "Cons")
                    {
                        int num = db.FichasAportes.Where(x => x.idPersona == nit).Count();
                        if (num > 0)
                        {
                            var consultaa = db.FichasAportes.Where(x => x.idPersona == nit && x.activa == true).ToList();
                            foreach (var item in consultaa)
                            {
                                string[] nombres = new string[1];
                                nombres[0] = item.numeroCuenta.ToString();
                                cuentas.Add(nombres);
                            }
                        }
                    }
                    else if (tipo == "ConsAhorroC")
                    {

                        var consultaa = ctx.FichasAhorroContractual.Where(x => x.IdAsociado == nit && x.Estado == true).ToList();
                        foreach (var item in consultaa)
                        {
                            string[] nombres = new string[1];
                            nombres[0] = item.NumeroCuenta.ToString();
                            cuentas.Add(nombres);
                        }

                    }
                    else if (tipo == "Cons_Cre")
                    {
                        int num = db.Creditos.Where(x => x.Creditos_Cedula == nit).Count();
                        if (num > 0)
                        {
                            //var consultaa = db.Creditos.Where(x => x.Creditos_Cedula == nit).ToList();
                            var consultaa = (from c in db.Creditos
                                             join t in db.TotalesCreditos on c.Pagare equals t.Pagare
                                             where c.Creditos_Cedula == nit && t.Estado != "LQ"
                                             select new { c }).ToList();
                            foreach (var item in consultaa)
                            {
                                string[] nombres = new string[1];
                                nombres[0] = item.c.Pagare.ToString();
                                cuentas.Add(nombres);
                            }
                        }

                    }
                    else if (tipo == "Des_Cre")
                    {
                        int num = db.Prestamos.Where(x => x.NIT == nit && x.estado == false).Count();
                        if (num > 0)
                        {
                            var consultaa = db.Prestamos.Where(x => x.NIT == nit && x.estado == false).ToList();
                            foreach (var item in consultaa)
                            {
                                string[] nombres = new string[1];
                                nombres[0] = item.Pagare.ToString();
                                cuentas.Add(nombres);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            
            return Json(cuentas, JsonRequestBehavior.AllowGet);
        }

        #region PROCESOS DE ADMINISTRADOR

        [Authorize(Roles = "Admin")]
        public ActionResult ProcesosAdministrador()
        {
            return View();
        }


        #endregion

    }
}
