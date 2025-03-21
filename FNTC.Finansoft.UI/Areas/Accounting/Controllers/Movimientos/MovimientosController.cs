/*
   Los mivimientos se soportan en Comprobante den PartidaDoble
   * Lo creo y lo alojo en Session
   * Al crearlo debo elegir el tipo de comrpbante y traer su consecutivo
   * Si es CC o CE debo elegir la forma de pago y agregarla como anotacion
   * Expongo funcionalidad para agregar Anotacion
   * Expongo funcionalidad para mostrarr estado del comprobante
   *  Es decir : Issues 
   *              Saldo
   *              
   * Grabar Comprobante
   */
using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.BLL.Comprobantes;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Documentos;
using Rotativa;
using Rotativa.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;


namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.Movimientos
{
    public class MovimientosController : Controller
    {
        public ActionResult EditComprobante(string tipo, string numero)
        {
            //obtengo el comprobante para editar
            var comprobante = new ComprobantesBLL().GetComprobante(tipo, numero);

            var fpago = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante(comprobante.Clase);
            ViewBag.FP = fpago;
            this.SetComprobanteToSession(comprobante, "ComprobanteNuevo");
            return PartialView("FormatoComprobantes", comprobante);
        }

        private void ActualizaFormaDepago(ref ComprobanteBO comprobante)
        {
            //Si es CE o RC actualizo los debitos creditos de la FP
            if (comprobante.Clase != "NC" || comprobante.Clase != "SI")
            {
                if (comprobante.Clase == "RC")
                {
                    var fpOld = comprobante.Entries.Where(x => x.Index == 1).First();
                    comprobante.Entries.RemoveAt(0);
                    fpOld.Debito = comprobante.Credito - comprobante.Debito; //suma de todos los creditos
                    comprobante.Entries.Insert(0, fpOld);
                }

                if (comprobante.Clase == "CE")
                {
                    var fpOld = comprobante.Entries.Where(x => x.Index == 1).First();
                    comprobante.Entries.RemoveAt(0);
                    fpOld.Credito = comprobante.Debito - comprobante.Credito; //suma de todos los creditos
                    comprobante.Entries.Insert(0, fpOld);
                }
            }
        }


        #region Comprobantes

        public ActionResult Asentar()
        {
            var result = false;

            //obtento
            var comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            //ac podria llegar nulo y lanza error
            if (comprobante == null)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            if (comprobante.IsNew)
            {

                //si el comprobante no es nuevo estoy editando

                //grabo el comprobante
                if (comprobante.IsOK)
                {
                    result = comprobante.Asentar();
                    if (result)
                    {
                        this.SetComprobanteToSession(null, "ComprobanteNuevo");
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //estoy editando
                var result2 = false;
                result2 = EditarComprobante(comprobante);
                return Json(result2, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion



        private bool EditarComprobante(ComprobanteBO bo)
        {
            var result = false;
            var comprobante = bo._comprobante;

            if (bo.IsOK)
            {
                var eliminarSaldos = anularMovimientoEdicion(comprobante.TIPO, comprobante.NUMERO);
                result = bo.Editar();
                if (result)
                {
                    this.SetComprobanteToSession(null, "ComprobanteNuevo");
                    return result;
                }

                return result;
            }
            return result;
        }

        public bool anularMovimientoEdicion(string tipo, string numero)
        {
            using (var ctx = new AccountingContext())
            {
                var movimientos = ctx.Movimientos.Where(x => x.TIPO == tipo && x.NUMERO == numero).ToList();

                //try
                //{
                foreach (var item in movimientos)
                {
                    var tipocuenta = ctx.PlanCuentas.Where(c => c.CODIGO == item.CUENTA).Single().NATURALEZA;

                    var plancuentas = ctx.PlanCuentas.Where(p => p.CODIGO == item.CUENTA).Single();
                    var saldosccs = ctx.SaldosCCs.Where(p => p.CUENTA == item.CUENTA && p.MES == item.FECHAMOVIMIENTO.Month && p.ANO == item.FECHAMOVIMIENTO.Year && p.CCOSTO == item.CCOSTO).FirstOrDefault();
                    var saldoscuentas = ctx.SaldosCuentas.Where(p => p.CODIGO == item.CUENTA && p.MES == item.FECHAMOVIMIENTO.Month && p.ANO == item.FECHAMOVIMIENTO.Year).FirstOrDefault();
                    var saldosterceros = ctx.SaldosTerceros.Where(p => p.CODIGO == item.CUENTA && p.TERCERO == item.TERCERO && p.MES == item.FECHAMOVIMIENTO.Month && p.ANO == item.FECHAMOVIMIENTO.Year).FirstOrDefault();

                    if (tipocuenta == "C")
                    {
                        if (item.CREDITO == 0)
                        {
                            //plancuentas.Saldo += item.DEBITO;
                            if (saldosccs != null) { saldosccs.MCREDITO += item.DEBITO; }
                            if (saldoscuentas != null) { saldoscuentas.MCREDITO += item.DEBITO; }
                            if (saldosterceros != null) { saldosterceros.MCREDITO += item.DEBITO; }

                        }
                        else
                        {
                            //plancuentas.Saldo -= item.CREDITO;
                            if (saldosccs != null) { saldosccs.MCREDITO -= item.CREDITO; }
                            if (saldoscuentas != null) { saldoscuentas.MCREDITO -= item.CREDITO; }
                            if (saldosterceros != null) { saldosterceros.MCREDITO -= item.CREDITO; }

                        }
                    }
                    else
                    {
                        if (item.DEBITO == 0)
                        {
                            //plancuentas.Saldo += item.CREDITO;
                            if (saldosccs != null) { saldosccs.MDEBITO += item.CREDITO; }
                            if (saldoscuentas != null) { saldoscuentas.MDEBITO += item.CREDITO; }
                            if (saldosterceros != null) { saldosterceros.MDEBITO += item.CREDITO; }

                        }
                        else
                        {
                            //plancuentas.Saldo -= item.DEBITO;
                            if (saldosccs != null) { saldosccs.MDEBITO -= item.DEBITO; }
                            if (saldoscuentas != null) { saldoscuentas.MDEBITO -= item.DEBITO; }
                            if (saldosterceros != null) { saldosterceros.MDEBITO -= item.DEBITO; }

                        }
                    }
                    if (saldosccs != null) { saldosccs.SALDO = saldosccs.MDEBITO - saldosccs.MCREDITO; }
                    if (saldoscuentas != null) { saldoscuentas.SALDO = saldoscuentas.MDEBITO - saldoscuentas.MCREDITO; }
                    if (saldosterceros != null) { saldosterceros.SALDO = saldosterceros.MDEBITO - saldosterceros.MCREDITO; }



                }

                ctx.SaveChanges();

                return true;
            }
        }

        public ActionResult anularMovimientos(string tipo, string numero)
        {
            try
            {
                using (var ctx = new AccountingContext())
                {
                    var comprobante = ctx.Comprobantes.Where(x => x.TIPO == tipo && x.NUMERO == numero).SingleOrDefault();

                    var movimientos = ctx.Movimientos.Where(x => x.TIPO == tipo && x.NUMERO == numero).ToList();

                    //try
                    //{
                    foreach (var item in movimientos)
                    {
                        var tipocuenta = ctx.PlanCuentas.Where(c => c.CODIGO == item.CUENTA).Single().NATURALEZA;

                        var plancuentas = ctx.PlanCuentas.Where(p => p.CODIGO == item.CUENTA).Single();
                        var saldosccs = ctx.SaldosCCs.Where(p => p.CUENTA == item.CUENTA && p.MES == item.FECHAMOVIMIENTO.Month && p.ANO == item.FECHAMOVIMIENTO.Year && p.CCOSTO == item.CCOSTO).Single();
                        var saldoscuentas = ctx.SaldosCuentas.Where(p => p.CODIGO == item.CUENTA && p.MES == item.FECHAMOVIMIENTO.Month && p.ANO == item.FECHAMOVIMIENTO.Year).Single();
                        var saldosterceros = ctx.SaldosTerceros.Where(p => p.CODIGO == item.CUENTA && p.TERCERO == item.TERCERO && p.MES == item.FECHAMOVIMIENTO.Month && p.ANO == item.FECHAMOVIMIENTO.Year).Single();

                        if (tipocuenta == "C")
                        {
                            if (item.CREDITO == 0)
                            {
                                //plancuentas.Saldo += item.DEBITO;
                                saldosccs.MCREDITO += item.DEBITO;
                                saldoscuentas.MCREDITO += item.DEBITO;
                                saldosterceros.MCREDITO += item.DEBITO;
                            }
                            else
                            {
                                //plancuentas.Saldo -= item.CREDITO;
                                saldosccs.MCREDITO -= item.CREDITO;
                                saldoscuentas.MCREDITO -= item.CREDITO;
                                saldosterceros.MCREDITO -= item.CREDITO;
                            }
                        }
                        else
                        {
                            if (item.DEBITO == 0)
                            {
                                //plancuentas.Saldo += item.CREDITO;
                                saldosccs.MDEBITO += item.CREDITO;
                                saldoscuentas.MDEBITO += item.CREDITO;
                                saldosterceros.MDEBITO += item.CREDITO;
                            }
                            else
                            {
                                //plancuentas.Saldo -= item.DEBITO;
                                saldosccs.MDEBITO -= item.DEBITO;
                                saldoscuentas.MDEBITO -= item.DEBITO;
                                saldosterceros.MDEBITO -= item.DEBITO;
                            }
                        }
                        saldosccs.SALDO = saldosccs.MDEBITO - saldosccs.MCREDITO;
                        saldoscuentas.SALDO = saldoscuentas.MDEBITO - saldoscuentas.MCREDITO;
                        saldosterceros.SALDO = saldosterceros.MDEBITO - saldosterceros.MCREDITO;
                    }

                    comprobante.ANULADO = true;

                    ctx.SaveChanges();
                    TempData["exito"] = "El comprobante ha sido anulado con éxito.";
                    return RedirectToAction("Index", new { Controller = "Movimientos", Area = "Accounting" });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Ha ocurrido un error al eliminar el comprobante";
                return RedirectToAction("Index", new { Controller = "Movimientos", Area = "Accounting" });
            }

        }

        public ActionResult Dismiss()
        {
            SetComprobanteToSession(null, "ComprobanteNuevo");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void init(ref ComprobanteBO bo)
        {
            var fpago = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante(bo.Clase);
            ViewBag.FP = fpago;
        }

        //properties
        private ComprobanteBO comprobante;



        /// <summary>
        /// Crea un nuevo comprobante del tipo especificado. me lo devuelve con sui consecutivo
        /// </summary>
        /// <param name="tipo">tipos : RC+n ,CE,NC,SI </param>
        /// <returns></returns>
        /// 
        private ComprobanteBO CreateNuevoComprobante(string tipo)
        {
            comprobante = new ComprobanteBO(tipo);
            Session["ComprobanteNuevo"] = comprobante;
            return comprobante;
        }

        #region CRUD ENTRIES
        /// <summary>
        /// Agrega una nueva entrada en el comprobante y devuelve el consecutivo asignado
        /// </summary>
        /// <returns></returns>
        public ActionResult AddEntry()
        {
            //esto puede lanzar excepcion
            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            //debe exixstir el comprobante
            if (comprobante == null)
            {
                //retry
                System.Threading.Thread.Sleep(50);
                comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
                if (comprobante == null)
                {
                    return Json(new { error = "No existe el comprobante" }, JsonRequestBehavior.AllowGet);
                }
            }
            var entry = new Anotacion("", 0, 0, 0, "", "", "", "");
            entry.Index = this.GetEntryConsecutive();
            //entry.Index = index;
            comprobante.Entries.Add(entry);

            //devuelvo el comprobante a la session
            this.SetComprobanteToSession(comprobante, "ComprobanteNuevo");

            return Json(entry.Index, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateEntry(/*Anotacion entry,*/ FormCollection col)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            //modelbinding not working
            var newEntry = new Anotacion();

            #region NewEntry
            newEntry.Cuenta = col["Cuenta"] == null || col["Cuenta"].Equals("") ? "" : col["Cuenta"];


            newEntry.Descripcion = col["Descripcion"];
            newEntry.Tercero = col["Tercero"];
            newEntry.cuentaPagare = col["cuentaPagare"];

            newEntry.CentroDeCosto = col["CentroDeCosto"];


            var _base = Decimal.Parse(col["Base"].Equals("") ? "0" : col["Base"], culture);
            newEntry.Base = _base;

            var _debito = col["Debito"].Equals("") ? "0" : col["Debito"];
            newEntry.Debito = Decimal.Parse(_debito, culture);

            var _credito = Decimal.Parse(col["Credito"].Equals("") ? "0" : col["Credito"], culture);
            newEntry.Credito = _credito;

            newEntry.Index = Int32.Parse(col["Index"].Equals("") ? "-1" : col["Index"]);
            #endregion

            var comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");


            if (comprobante.Entries.Count != 0)
            {
                var old = comprobante.Entries.Where(x => x.Index == newEntry.Index).First(); //esto puede lanzarte al otro mundo
                var oldIndex = comprobante.Entries.IndexOf(old);

                try
                {
                    comprobante.Entries.Remove(old);
                    comprobante.Entries.Insert(oldIndex, newEntry);
                    if (comprobante.Clase != "NC" || comprobante.Clase != "SI")
                    {
                        this.ActualizaFormaDepago(ref comprobante);
                    }

                    ////para actualizar los saldos
                    //decimal c = comprobante.Credito ;
                    //decimal d= comprobante.Debito ;
                    //set comprobante to session
                    // comprobante.Entries.ElementAt(0).Debito = comprobante.Entries.ElementAt(0).Credito = 0;

                    this.SetComprobanteToSession(comprobante, "ComprobanteNuevo");
                    return Json(new { result = true });

                }
                catch (Exception e)
                {
                    return Json(new { result = false, message = e.Message });
                }
            }
            return Json(new { result = false, message = "El comprobante esta vacio" });
            //que pasa cuando estoy actualizando?
        }

        [HttpPost]
        public ActionResult RemoveEntry(int? index)
        {
            //get comprobante
            var comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            if (comprobante == null)
            {
                //retry
                System.Threading.Thread.Sleep(50);
                comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
                if (comprobante == null)
                {
                    return Json(new { result = false, message = "el comprobante esta nulo" });
                }
            }

            try
            {

                //si requiere forma de pago no debo dejar eliminar la primera
                if ((comprobante.Clase == "RC" || comprobante.Clase == "CE") && index == 1)
                {
                    return Json(new { result = false, message = "No se puede eliminar la Forma de Pago" });
                }
                var entryToBeRemoved = comprobante.Entries.Where(x => x.Index == index).FirstOrDefault();
                if (entryToBeRemoved == null)
                {
                    //error no se pudo encontrar una entrada con ese index
                    return Json(new { result = false, message = "error no se pudo encontrar una entrada con ese index" });
                }


                var _result = comprobante.Entries.Remove(entryToBeRemoved);
                if (comprobante.Clase == "RC" || comprobante.Clase == "CE")
                {
                    this.ActualizaFormaDepago(ref comprobante);
                }

                return Json(new { result = _result, message = "Eliminado" });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                //throw;
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateComprobante(FormCollection col)
        {
            var c = GetComprobanteFromSession("ComprobanteNuevo");
            if (c == null)
            {
                //error
                return View("Error");
            }

            //c.Consecutivo = col[0]; //este no se debe cambiar
            c.FechaComprobante = Convert.ToDateTime(col["FechaComprobante"]);
            c.Detalle = col["Detalle"];

            //solo si es distingo de RC
            if (c.Clase != "NC" && c.Clase != "SI")
            {
                c.FPago = col["FPago"];
                //var p6 = col[4]; //este campo no se quien es
                c.NumeroExterno = col["NumeroExterno"];

                //actualizo el tercero que viene de la FP

            }




            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion



        public ActionResult Verify()
        {
            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            if (comprobante == null) { return Json(new { Error = "Comprobante nulo" }, JsonRequestBehavior.AllowGet); }

            var issues = new IssueStruct();
            var IsOk = comprobante.Verify(out issues);

            var saldos = new
            {
                Credito = comprobante.Credito,
                Debito = comprobante.Debito,
                Balance = comprobante.Balance
            };
            var clase = comprobante.Clase;
            //if ((clase != "NC" || clase != "SI") )
            //{

            var _credito = comprobante.Entries.Count > 0 ? comprobante.Entries.ElementAt(0).Credito : 0;
            var _debito = comprobante.Entries.Count > 0 ? comprobante.Entries.ElementAt(0).Debito : 0;
            var formaPago = new
            {
                Debito = _debito,
                Credito = _credito
            };
            //return Json(new { Clase = clase, Issues = issues, IsOk = IsOk, Saldos = saldos, Error = "", FP = formaPago }, "application/json", JsonRequestBehavior.AllowGet);

            //  }



            return Json(new { Clase = clase, Issues = issues, IsOk = IsOk, Saldos = saldos, Error = "", FP = formaPago }, "application/json", JsonRequestBehavior.AllowGet);
        }

        private int GetEntryConsecutive()
        {
            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            //validacion
            var consecutive = -1;
            if (comprobante != null)
            {
                if (comprobante.Entries.Count == 0)
                {
                    consecutive = 1;
                }
                else
                {
                    var c = comprobante.Entries.OrderBy(x => x.Index).Last().Index + 1;
                    consecutive = c;
                }

            }
            //return Json(consecutive, JsonRequestBehavior.AllowGet);
            return consecutive;
        }


        #region ToSession
        private void SetComprobanteToSession(ComprobanteBO cBO, string cookieName)
        {
            Session["ComprobanteNuevo"] = cBO;
        }

        private void ComprobanteToSession(ComprobanteBO cBO)
        {
            if (Session["Comprobante"] == null)
            {

            }

        }

        private ComprobanteBO GetComprobanteFromSession(string cookieName)
        {
            ComprobanteBO cBO;
            if (Session[cookieName] != null)
            {
                cBO = (ComprobanteBO)Session[cookieName];
                //retry
                if (cBO == null)
                {
                    System.Threading.Thread.Sleep(150);
                    cBO = (ComprobanteBO)Session[cookieName];
                }
            }
            else
            {
                cBO = null;
            }
            return cBO;
        }
        #endregion

        #region Vistas
        public ActionResult Index()
        {
            //los tipos para el combo para filtrar
            var tipos = new FNTC.Finansoft.Accounting.DAL.Comprobantes.ComprobantesDAL().GetAllTiposComprobantes();
            ViewBag.Tipos = tipos.Select(t => new SelectListItem() { Text = t.CODIGO + " - " + t.NOMBRE, Value = t.CODIGO });
            return View();
        }

        public ActionResult IndexMovimientosEditados()
        {
            //los tipos para el combo para filtrar
            var tipos = new FNTC.Finansoft.Accounting.DAL.Comprobantes.ComprobantesDAL().GetAllTiposComprobantes();
            ViewBag.Tipos = tipos.Select(t => new SelectListItem() { Text = new StringBuilder().AppendFormat("{0,10} - {0,50}", t.CODIGO, t.NOMBRE).ToString(), Value = t.CODIGO });
            return View();
        }

        public ActionResult Nuevo()
        {

            #region //si aun no hay saldos iniciales  lanzo SI
            using (var ctx = new AccountingContext())
            {
                if (ctx.Comprobantes.Count() == 0)
                {
                    try
                    {
                        comprobante = this.CreateNuevoComprobante("SI1");
                        comprobante.Detalle = "SALDOS INICIALES";

                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('No existe un Tipo Comprobante para Saldos Iniciales');</script>");
                        return RedirectToAction("index", "Default", new { Area = "Dashboard", titulo = "catálogo", menu = "contabilidad" });
                        //http://localhost:81/Dashboard/Default#&movimientos
                    }
                    init(ref comprobante);
                    Response.Write("<script>alert('No hay Saldos Iniciales')</script>");
                    return PartialView("FormatoComprobantes", comprobante);
                }
            }
            #endregion
            Session.Remove("ComprobanteNuevo");
            #region Ya existe un comprobante ne sesion
            /*
            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            if (comprobante != null)
            {
                Response.Write("<script>alert('Ya existe un comprobante en proceso. Si desea crear uno nuevo debe descartar el anterior')</script>");
                ViewBag.NuevoExistente = true;

                init(ref comprobante);
                return PartialView("FormatoComprobantes", comprobante);
            }
            */
            #endregion

            // 
            var tiposComprobantes = new FNTC.Finansoft.Accounting
                .DAL.Comprobantes.ComprobantesDAL()
                .GetAllTiposComprobantes();

            return View(tiposComprobantes);
        }

        public ActionResult NuevoTres()
        {
            return View();
        }

        public ActionResult NuevoDos(string tipo)
        {

            //si ya exixte uno en sesion

            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            if (comprobante != null)
            {
                //tengo algo en memoria

                var fpago2 = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante(comprobante.Clase);
                ViewBag.FP = fpago2;
                return PartialView("FormatoComprobantes", comprobante);
            }

            //creo un nuevo objeto del tipo especificado
            //ESTO VA EN bll EN GetNewComprobante(string tipoComprobante)
            //var comprobante = Activator.CreateInstance
            //    ("FNTC.Finansoft.Accounting.BLL"
            //    , "ComprobanteBLL");

            //   var comprobante = new ComprobantesService().GetNuevoComprobante(tipo);
            comprobante = this.CreateNuevoComprobante(tipo);

            //segun el tipo comprobante devuelvo la vista para la clase
            var clase = tipo.Substring(0, 2);
            //en tipo viene el tipoComprobante

            var fpago = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante("RC");
            ViewBag.FP = fpago;


            //si es un RC o CE le agrego la forma de pago predeterminada -esto ya no va
            switch (clase)
            {
                case "RC": return PartialView("FormatoComprobantes", comprobante);
                case "CE": return PartialView("FormatoComprobantes", comprobante);
                case "NC": return PartialView("FormatoComprobantes", comprobante);
                case "SI": return PartialView("FormatoComprobantes", comprobante);
                case "DS": return PartialView("FormatoComprobantes", comprobante);
                default: return PartialView("Error");
                    //  break;
            }
        }

        [HttpPost]
        public ActionResult Nuevo(string tipo, FormCollection col)
        {

            //si ya exixte uno en sesion

            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            if (comprobante != null)
            {
                //tengo algo en memoria

                var fpago2 = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante(comprobante.Clase);
                ViewBag.FP = fpago2;
                return PartialView("FormatoComprobantes", comprobante);
            }

            if (tipo == "")
            {
                tipo = col[0];
            }
            //creo un nuevo objeto del tipo especificado
            //ESTO VA EN bll EN GetNewComprobante(string tipoComprobante)
            //var comprobante = Activator.CreateInstance
            //    ("FNTC.Finansoft.Accounting.BLL"
            //    , "ComprobanteBLL");

            //   var comprobante = new ComprobantesService().GetNuevoComprobante(tipo);
            comprobante = this.CreateNuevoComprobante(tipo);

            //segun el tipo comprobante devuelvo la vista para la clase
            var clase = tipo.Substring(0, 2);
            //en tipo viene el tipoComprobante

            var fpago = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante("RC");
            ViewBag.FP = fpago;


            //si es un RC o CE le agrego la forma de pago predeterminada -esto ya no va
            switch (clase)
            {
                case "RC": return PartialView("FormatoComprobantes", comprobante);
                case "CE": return PartialView("FormatoComprobantes", comprobante);
                case "NC": return PartialView("FormatoComprobantes", comprobante);
                case "SI": return PartialView("FormatoComprobantes", comprobante);
                case "DS": return PartialView("FormatoComprobantes", comprobante);
                default: return PartialView("Error");
                    //  break;
            }
        }

        public ActionResult Pruebas(string output = "pdf")
        {
            using (var db = new AccountingContext())
            {
                ComprobanteBO cBO;
                cBO = GetComprobanteFromSession("pruebas");

                if (cBO == null)
                {
                    cBO = (ComprobanteBO)TempData["pruebas"];
                }

                if (cBO == null) //si esta nulo entonces cre uno nuevo
                {

                    #region OLD
                    //cBO = this.CreateNuevoComprobante("CE7");
                    //#region Creo el bo
                    //var fpago2 = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante(comprobante.Clase);
                    //ViewBag.FP = fpago2;


                    //cBO.Detalle = "Comprobante egreso pruebas";
                    //cBO.FechaComprobante = DateTime.Now;
                    //cBO.NumeroExterno = "111";
                    ////var entradas = new 

                    //cBO.Entries.First(x => x.Index == 1).Tercero = "13256456";

                    ////cBO.(new Anotacion("111", 0, 2000, 0, "01", "forma de pago", "108502545"));
                    ////cBO.Entries.Add(new Anotacion("222", 2000, 0, 0,"01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("13256456", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    //cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545"));
                    ////cBO.AddEntry(new Anotacion("2000", 3000M, 0M, 0, "01", "algun campo", ""));
                    //cBO.AddEntry(new Anotacion("2000", 4000M, 0M, 0, "01", "algun campo", ""));
                    //#endregion

                    //this.SetComprobanteToSession(cBO, "pruebas"); 
                    #endregion

                    cBO = new FNTC.Finansoft.Accounting.BLL.Comprobantes.ComprobantesBLL().GetComprobante("RC1", "101");
                    SetComprobanteToSession(cBO, "pruebas");
                }

                comprobante = cBO;
                var valor = cBO.Entries.First(e => e.Index == 1);

                ViewBag.SumaLetras = FNTC.Framework.Converters.Numbers.NumerosALetras.Convertir((cBO.Debito).ToString(), true);

                if (cBO.Clase != "SI" || cBO.Clase == "NC")
                {
                    string nit = "";
                    ViewBag.Nit = nit = cBO.Tercero == null ? cBO.Entries.First(x => x.Index == 1).Tercero : cBO.Tercero;
                    var tercero = new FNTC.Finansoft.Accounting.BLL.Terceros.TercerosBLL().GetTerceroByNit(nit);
                    ViewBag.Nombre = tercero.NOMBRE == null ? "" : tercero.NOMBRE;
                    ViewBag.Direccion = tercero.DIR == null ? "" : tercero.DIR;
                    ViewBag.Telefono = tercero.TEL + " - " + tercero.TELMOVIL;
                }

                ViewBag.Anulado = cBO._comprobante.ANULADO;

                var comp = db.Comprobantes.Where(x => x.TIPO == cBO._comprobante.TIPO && x.NUMERO == cBO._comprobante.NUMERO).FirstOrDefault();
                ViewBag.observacion = comp.Observacion;

                //"asdasd";
                if (output == "pdf")
                {
                    return new ViewAsPdf("Pruebas", cBO)
                    {

                        PageSize = Size.Letter,
                        PageOrientation = Orientation.Portrait,
                        PageMargins = { Left = 0, Right = 0 },
                        IsLowQuality = true
                    };
                }


                //datos comprobante


                return View("Pruebas", cBO);
            }

        }

        public ActionResult GetDocumentoSoporte(string output = "pdf")
        {
            ComprobanteBO cBO;
            cBO = GetComprobanteFromSession("pruebas");

            if (cBO == null)
            {
                cBO = (ComprobanteBO)TempData["pruebas"];
            }

            if (cBO == null) //si esta nulo entonces cre uno nuevo
            {

                cBO = new FNTC.Finansoft.Accounting.BLL.Comprobantes.ComprobantesBLL().GetComprobante("RC1", "101");
                SetComprobanteToSession(cBO, "pruebas");
            }

            comprobante = cBO;
            var valor = cBO.Entries.First(e => e.Index == 1);

            ViewBag.SumaLetras = FNTC.Framework.Converters.Numbers.NumerosALetras.Convertir((valor.Debito == 0 ? valor.Credito : valor.Debito).ToString(), true);

            if (cBO.Clase != "SI" || cBO.Clase == "NC")
            {
                string nit = "";
                ViewBag.Nit = nit = cBO.Tercero == null ? cBO.Entries.First(x => x.Index == 1).Tercero : cBO.Tercero;
                var tercero = new FNTC.Finansoft.Accounting.BLL.Terceros.TercerosBLL().GetTerceroByNit(nit);
                ViewBag.Nombre = tercero.NOMBRE == null ? "" : tercero.NOMBRE;
                ViewBag.Direccion = tercero.DIR == null ? "" : tercero.DIR;
                ViewBag.Telefono = tercero.TEL + " - " + tercero.TELMOVIL;
            }

            ViewBag.Anulado = cBO._comprobante.ANULADO;


            //extraemos la configuracion de parámetros para documento soporte
            AccountingContext db = new AccountingContext();
            ConfigDocumentoSoporte configDS = new ConfigDocumentoSoporte();
            var dataConfig = db.ConfigDocumentoSoporte.Where(x => x.tipoComprobante == cBO.TipoComprobante).FirstOrDefault();
            if (dataConfig != null)
            {
                configDS = dataConfig;
            }
            ViewBag.configDS = configDS;

            //"asdasd";
            if (output == "pdf")
            {
                return new ViewAsPdf("DocumentoSoporte", cBO)
                {
                    //FileName = "Documento_Soporte_"+cBO.TipoComprobante+"_"+cBO.Consecutivo,
                    PageSize = Size.Letter,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = { Left = 0, Right = 0 },
                    IsLowQuality = true
                };
            }


            //datos comprobante


            return View("Pruebas", cBO);
        }

        public ActionResult NotaContabilidad(string output = "pdf")
        {
            ComprobanteBO cBO;
            cBO = GetComprobanteFromSession("pruebas");
            if (cBO == null) //si esta nulo entonces cre uno nuevo
            {
                cBO = this.CreateNuevoComprobante("NC1");
                this.SetComprobanteToSession(cBO, "pruebas");
            }

            var fpago2 = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante(comprobante.Clase);
            ViewBag.FP = fpago2;


            cBO.Detalle = "Comprobante egreso pruebas";
            cBO.FechaComprobante = DateTime.Now;
            cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545", ""));
            cBO.AddEntry(new Anotacion("2000", 3000M, 0M, 0, "01", "algun campo", "", ""));
            cBO.AddEntry(new Anotacion("2000", 4000M, 0M, 0, "01", "algun campo", "", ""));



            if (output == "pdf")
            {
                return new ViewAsPdf("Pruebas", cBO)
                {
                    //  FileName = "TestView.pdf",
                    PageSize = Size.A5,
                    PageOrientation = Orientation.Landscape,
                    //     PageMargins = { Left = 20, Right = 20 },
                    IsLowQuality = true
                }; ;
            }
            return View("Pruebas", cBO);


        }

        public ActionResult ReciboCaja(string output = "pdf")
        {
            ComprobanteBO cBO;
            cBO = GetComprobanteFromSession("pruebasRC");
            if (cBO == null) //si esta nulo entonces cre uno nuevo
            {
                cBO = this.CreateNuevoComprobante("RC1");
                this.SetComprobanteToSession(cBO, "pruebasRC");
            }

            var fpago2 = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante(comprobante.Clase);
            ViewBag.FP = fpago2;


            cBO.Detalle = "Recibo de Caja";
            cBO.FechaComprobante = DateTime.Now;
            cBO.AddEntry(new Anotacion("1234", 100000000.01M, 0, 0, "01", "forma de pago", "108502545", ""));
            cBO.AddEntry(new Anotacion("2000", 3000M, 0M, 0, "01", "algun campo", "", ""));
            cBO.AddEntry(new Anotacion("2000", 4000M, 0M, 0, "01", "algun campo", "", ""));

            if (output == "pdf")
            {
                return new ViewAsPdf("Pruebas", cBO);
            }
            return View("Pruebas", cBO);


        }

        #endregion



        public ActionResult ImprimirFromSession(string cookie = "ComprobanteNuevo")
        {
            if (cookie.Equals(""))
            {
                comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            }
            //si no esta en session debno contruirlo
            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
            return new ViewAsPdf("Pruebas", comprobante);
        }


        public ActionResult indexNuevo()
        {
            return View();
        }

    }




}