//using FNTC.Finansoft.Accounting.BLL;
//using Newtonsoft.Json;
//using System;
//using System.Linq;
//using System.Web.Mvc;

//namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.Movimientos
//{
//    public class EdisonController : Controller
//    {
//        private ComprobanteBO comprobante;
//        // private PartidaDoble entries;
//        /*
//         Los mivimientos se soportan en Comprobante den PartidaDoble
//         * Lo creo y lo alojo en Session
//         * Al crearlo debo elegir el tipo de comrpbante y traer su consecutivo
//         * Si es CC o CE debo elegir la forma de pago y agregarla como anotacion
//         * Expongo funcionalidad para agregar Anotacion
//         * Expongo funcionalidad para mostrarr estado del comprobante
//         *  Es decir : Issues 
//         *              Saldo
//         *              
//         * Grabar Comprobante
//         */

//        public ActionResult Index() { return View(); }

//        private void ComprobanteToSession(ComprobanteBO cBO)
//        {
//            if (Session["Comprobante"] == null)
//            {

//            }

//        }

//        public ActionResult Nuevo()
//        {

//            //si tengo un comprobante en session para nuevo lo myesto


//            var tiposComprobantes = new FNTC.Finansoft.Accounting
//                .DAL.Comprobantes.ComprobantesDAL()
//                .GetAllTiposComprobantes();



//            return View(tiposComprobantes);
//        }

//        [HttpPost]
//        public ActionResult Nuevo(string tipo, FormCollection col)
//        {

//            if (tipo == "")
//            {
//                tipo = col[0];
//            }
//            //creo un nuevo objeto del tipo especificado
//            //ESTO VA EN bll EN GetNewComprobante(string tipoComprobante)
//            //var comprobante = Activator.CreateInstance
//            //    ("FNTC.Finansoft.Accounting.BLL"
//            //    , "ComprobanteBLL");

//            //   var comprobante = new ComprobantesService().GetNuevoComprobante(tipo);
//            comprobante = this.CreateNuevoComprobante(tipo);

//            //segun el tipo comprobante devuelvo la vista para la clase
//            var clase = tipo.Substring(0, 2);
//            //en tipo viene el tipoComprobante

//            var fpago = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante("RC");
//            ViewBag.FormasPago = fpago;
//            switch (clase)
//            {
//                case "RC": return PartialView("ReciboDeCaja", comprobante);
//                case "CE": return PartialView("ReciboDeCaja", comprobante);
//                case "NC": return PartialView("NotaContabilidad", comprobante);
//                case "SI": return PartialView("NotaContabilidad", comprobante);
//                default: return PartialView("Error");
//                //  break;
//            }

//            return View(tipo);
//        }

//        /// <summary>
//        /// Crea un nuevo comprobante del tipo especificado. me lo devuelve con sui consecutivo
//        /// </summary>
//        /// <param name="tipo">tipos : RC+n ,CE,NC,SI </param>
//        /// <returns></returns>
//        /// 
//#warning esto va en BLL
//        private ComprobanteBO CreateNuevoComprobante(string tipo)
//        {
//            comprobante = new ComprobanteBO(tipo);
//            Session["ComprobanteNuevo"] = comprobante;

//            return comprobante;
//        }

//        private ComprobanteBO GetComprobanteFromSession(string cookieName)
//        {
//            ComprobanteBO cBO;
//            if (Session[cookieName] != null)
//            {
//                cBO = (ComprobanteBO)Session[cookieName];
//            }
//            else
//            {
//                cBO = null;
//            }
//            return cBO;
//        }

//        private void SetComprobanteToSession(ComprobanteBO cBO, string cookieName)
//        {
//            Session["ComprobanteNuevo"] = cBO;
//        }


//        //////////ENTRIES ////////////////////////////////////////////////
//        /// <summary>
//        /// Agrega una nueva entrada en el comprobante y devuelve el consecutivo asignado
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult AddEntry()
//        {
//            //esto puede lanzar excepcion
//            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
//            //debe exixstir el comprobante
//            if (comprobante == null)
//            {
//                return Json(new { error = "No existe el comprobante" }, JsonRequestBehavior.AllowGet);
//            }
//            var entry = new Anotacion("", 0, 0, "", "", "","");
//            entry.Index = this.GetEntryConsecutive();
//            //entry.Index = index;
//            comprobante.Entries.Add(entry);

//            //devuelvo el comprobante a la session
//            this.SetComprobanteToSession(comprobante, "ComprobanteNuevo");

//            return Json(entry.Index, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public ActionResult UpdateEntry(/*Anotacion entry,*/ FormCollection col)
//        {
//            //modelbinding not working
//            var newEntry = new Anotacion();
//            newEntry.Cuenta = col[0];
//            newEntry.Descripcion = col[1];
//            newEntry.Tercero = col[2];
//            newEntry.CentroDeCosto = col[3];

//            var _base = Decimal.Parse(col[4].Equals("") ? "0" : col[4]);
//            newEntry.Base = _base;

//            var _debito = Decimal.Parse(col[5].Equals("") ? "0" : col[5]);
//            newEntry.Debito = _debito;

//            var _credito = Decimal.Parse(col[6].Equals("") ? "0" : col[6]);
//            newEntry.Credito = _credito;


//            newEntry.Index = Int32.Parse(col[7].Equals("") ? "-1" : col[7]);


//            var comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
//            var old = comprobante.Entries.Where(x => x.Index == newEntry.Index).First(); //esto puede lanzarte al otro mundo
//            var oldIndex = comprobante.Entries.IndexOf(old);

//            try
//            {
//                comprobante.Entries.Remove(old);
//                comprobante.Entries.Insert(oldIndex, newEntry);

//                //set comprobante to session
//                this.SetComprobanteToSession(comprobante, "ComprobanteNuevo");
//                return Json(new { result = true });
//            }
//            catch (Exception e)
//            {
//                return Json(new { result = false, message = e.Message });

//            }

//            //que pasa cuando estoy actualizando?
//            //    comprobante.Entries.Add(entry);
//            //  return Json(new { result = true }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public ActionResult RemoveEntry(int? index)
//        {
//            //get comprobante
//            var c = this.GetComprobanteFromSession("ComprobanteNuevo");
//            if (c == null)
//            {
//                return Json(new { result = false, message = "el comprobante esta nulo" });
//            }
//            try
//            {
//                var entryToBeRemoved = c.Entries.Where(x => x.Index == index).FirstOrDefault();
//                if (entryToBeRemoved == null)
//                {
//                    //error no se pudo encontrar una entrada con ese index
//                    return Json(new { result = false, message = "error no se pudo encontrar una entrada con ese index" });
//                }

//                var result = c.Entries.Remove(entryToBeRemoved);
//                return Json(result);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
//                //throw;
//            }
//            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
//        }

//        private int GetEntryConsecutive()
//        {
//            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
//            //validacion
//            var consecutive = -1;
//            if (comprobante != null)
//            {
//                if (comprobante.Entries.Count == 0)
//                {
//                    consecutive = 1;
//                }
//                else
//                {
//                    var c = comprobante.Entries.OrderBy(x => x.Index).Last().Index + 1;
//                    consecutive = c;
//                }

//            }
//            //return Json(consecutive, JsonRequestBehavior.AllowGet);
//            return consecutive;
//        }

//        [HttpPost]
//        public ActionResult UpdateComprobante(FormCollection col)
//        {
//            var c = GetComprobanteFromSession("ComprobanteNuevo");
//            if (c == null)
//            {
//                //error
//                return View("Error");
//            }

//            //c.Consecutivo = col[0]; //este no se debe cambiar
//            c.FechaComprobante = Convert.ToDateTime(col[1]);
//            c.Detalle = col[2];
//            c.FPago = col[3];
//            var p6 = col[4]; //este campo no se quien es
//            c.NumeroExterno = col[5];

//            return Json(true, JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult Pruebas()
//        {
//            ComprobanteBO c;
//            var cBO = GetComprobanteFromSession("ComprobanteNuevo");
//            if (cBO == null) //si esta nulo entonces cre uno nuevo
//            {
//                c = this.CreateNuevoComprobante("RC1");
//            }
//            else
//            {
//                c = cBO;
//            }


//            //parametros de la vista
//            var fpago = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante("RC");
//            ViewBag.FormasPago = fpago;
//            return View(c);
//        }

//        [HttpPost]
//        public ActionResult Pruebas(FormCollection col, ComprobanteBO c)
//        {
//            return View(c);
//        }

//        public ActionResult NotaContabilidad()
//        {

//            ComprobanteBO c;
//            var cBO = GetComprobanteFromSession("ComprobanteNuevo");
//            if (cBO == null) //si esta nulo entonces cre uno nuevo
//            {
//                c = this.CreateNuevoComprobante("RC1");
//            }
//            else
//            {
//                c = cBO;
//            }

//            var fpago = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL().GetFormaDePagoByClaseComprobante("RC");
//            ViewBag.FormasPago = fpago;
//            return View("NotaContabilidad", c);
//        }

//        public ActionResult ReciboDeCaja()
//        {
//            var comprobante = new ComprobanteBO("RC1");
//            return View("ReciboDeCaja", comprobante);
//        }

//        public ActionResult ComprobanteEgreso()
//        {
//            var comprobante = new ComprobanteBO("CE1");
//            return View("ReciboDeCaja", comprobante);
//        }

//        public ActionResult Verify()
//        {
//            comprobante = this.GetComprobanteFromSession("ComprobanteNuevo");
//            if (comprobante == null)
//            {
//                return Json(new { Error = "comrpobante nulo" }, JsonRequestBehavior.AllowGet);
//            }

//            var issues = new IssueStruct();
//            var IsOk = comprobante.Verify(out issues);
//            //  var pretty = JsonConvert.SerializeObject(new { Issues = issues, IsOk = IsOk }, Formatting.Indented);
//            return Json(new { Issues = issues, IsOk = IsOk }, "application/json", JsonRequestBehavior.AllowGet);
//        }

//        [Obsolete("", true)]
//        public ActionResult CreateComprobante(string tipoComprobamte)
//        {
//            //  obtengo el consecutivo del tipocomprobante
//            // entries = new PartidaDoble();
//            //creo un cormprobante vacio y lo pongo en session
//            comprobante = new ComprobanteBO(tipoComprobamte);
//            //esto debe tener un tokern
//            int consecutivo = Int32.Parse(comprobante.GetConsecutivo());
//            Session["Comprobante" + tipoComprobamte + consecutivo] = comprobante;

//            return Json(consecutivo, JsonRequestBehavior.AllowGet);
//        }
//    }
//}