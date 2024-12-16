using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System.Linq;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.BLL.FormulariosSolicitud;
using FNTC.Finansoft.Accounting.DTO.FormulariosSolicitud;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.Geo;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class solicitudCreditosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: SolicitudCredito/solicitudCreditos
        public ActionResult Index()
        {


            var Listado = db.SolicitudCredito.Include(a => a.Terceros).ToList();

            return View(Listado);
        }

        // GET: SolicitudCredito/solicitudCreditos/Details/5
        public ActionResult Details(string id)
        {
            var datos = (from pc in db.SolicitudCredito where pc.id_persona == id select pc).FirstOrDefault();
            int idSol = datos.id_solicitud;
            //DateTime localDate = DateTime.Now;
            //       string fecha = localDate.ToLongDateString; 
            //string mifecha1 = DateTime.Now.ToLocalTime();
            string fecha = DateTime.Now.ToShortDateString();
            string Hora = DateTime.Now.ToLongTimeString();
            string mifecha = fecha + " || " + Hora; 
            ViewBag.dato = mifecha;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitudCredito solicitudCredito = db.SolicitudCredito.Find(idSol);
            if (solicitudCredito == null)
            {
                return HttpNotFound();
            }
            return View(solicitudCredito);
        }

        // GET: SolicitudCredito/solicitudCreditos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SolicitudCredito/solicitudCreditos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_solicitud,fecha,tieneViviendaPropia,viviendaFamiliar,viviendaHipotecada,valComercial,id_persona,id_destino,id_linea,val_solicitud,plazo,cuota,cancelaCredito,id_codeudor,pagare,id_codeudor2,empresa,fechaIngresoEmpresa,salarioEmpresa,cod1empresa,cod1fechaIngresoEmpresa,cod2empresa,cod2fechaIngresoEmpresa,asesorcomercial,nomFam1,nomFam2,ocuFam1,ocuFam2,dirFam1,dirFam2,celFam1,celFam2,nomPer1,nomPer2,ocuPer1,ocuPer2,dirPer1,dirPer2,celPer1,celPer2,estCivil,perCargo,estrato,timeResidencia,nomConyu,apeConyu,cedConyu,empConyu,celConyu,salPri,otrIng,comision,otrIngCon,detOrig,valArriendo,valSoste,valFin,otrGasto,casa,apto,finca,otro,cual,dirProp,ciuProp,escritura,notFec,nMatInm,valComProp,valHipProp,modelo,valVehiculo,valDeuVehiculo,fechVenCon,tiemLaborado,ParentescoFam1,ParentescoFam2,CiudadFam1,CiudadFam2,ParentescoPer1,ParentescoPer2,CiudadPer1,CiudadPer2,EstadoCivilCod1,EstadoCivilCod2,TotalIngresos,TotalGastos, ENDentidad1, ENDentidad2, ENDsaldoDeuda1, ENDsaldoDeuda2, ENDcuotaMensual1, ENDcuotaMensual2")] solicitudCredito solicitudCredito)
        {
            if (solicitudCredito.ENDsaldoDeuda1 != null)
            {
                solicitudCredito.ENDsaldoDeuda1 = solicitudCredito.ENDsaldoDeuda1.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.ENDsaldoDeuda2 != null)
            {
                solicitudCredito.ENDsaldoDeuda2 = solicitudCredito.ENDsaldoDeuda2.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.ENDcuotaMensual1 != null)
            {
                solicitudCredito.ENDcuotaMensual1 = solicitudCredito.ENDcuotaMensual1.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.ENDsaldoDeuda2 != null)
            {
                solicitudCredito.ENDsaldoDeuda2 = solicitudCredito.ENDsaldoDeuda2.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.valComercial != null)
            {
                solicitudCredito.valComercial = solicitudCredito.valComercial.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.cuota != null)
            {
                solicitudCredito.cuota = solicitudCredito.cuota.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.salarioEmpresa != null)
            {
                solicitudCredito.salarioEmpresa = solicitudCredito.salarioEmpresa.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.salPri != null)
            {
                solicitudCredito.salPri = solicitudCredito.salPri.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.TotalIngresos != null)
            {
                solicitudCredito.TotalIngresos = solicitudCredito.TotalIngresos.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.TotalGastos != null)
            {
                solicitudCredito.TotalGastos = solicitudCredito.TotalGastos.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.val_solicitud != null)
            {
                solicitudCredito.val_solicitud = solicitudCredito.val_solicitud.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.celConyu != null)
            {
                solicitudCredito.celConyu = solicitudCredito.celConyu.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.otrIng != null)
            {
                solicitudCredito.otrIng = solicitudCredito.otrIng.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.otrIngCon != null)
            {
                solicitudCredito.otrIngCon = solicitudCredito.otrIngCon.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.comision != null)
            {
                solicitudCredito.comision = solicitudCredito.comision.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valArriendo != null)
            {
                solicitudCredito.valArriendo = solicitudCredito.valArriendo.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valSoste != null)
            {
                solicitudCredito.valSoste = solicitudCredito.valSoste.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valFin != null)
            {
                solicitudCredito.valFin = solicitudCredito.valFin.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.otrGasto != null)
            {
                solicitudCredito.otrGasto = solicitudCredito.otrGasto.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valHipProp != null)
            {
                solicitudCredito.valHipProp = solicitudCredito.valHipProp.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valComProp != null)
            {
                solicitudCredito.valComProp = solicitudCredito.valComProp.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valDeuVehiculo != null)
            {
                solicitudCredito.valDeuVehiculo = solicitudCredito.valDeuVehiculo.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valVehiculo != null)
            {
                solicitudCredito.valVehiculo = solicitudCredito.valVehiculo.Replace('.'.ToString(), "");

            }


            //validar null para todos




            if (ModelState.IsValid)
            {
                var respuesta = new solicitudCreditoBLL().Create(solicitudCredito);
                if (respuesta)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(solicitudCredito);
        }

        public JsonResult GetFechaActual()
        {
            return Json(DateTime.Now, JsonRequestBehavior.AllowGet);
        }

        // GET: SolicitudCredito/solicitudCreditos/Edit/5
        public ActionResult Edit(int id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitudCredito solicitudCredito = db.SolicitudCredito.Find(id);
            if (solicitudCredito == null)
            {
                return HttpNotFound();
            }
            return View(solicitudCredito);
        }

        // POST: SolicitudCredito/solicitudCreditos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_solicitud,fecha,tieneViviendaPropia,viviendaFamiliar,viviendaHipotecada,valComercial,id_persona,id_destino,id_linea,val_solicitud,plazo,cuota,cancelaCredito,id_codeudor,pagare,id_codeudor2,empresa,fechaIngresoEmpresa,salarioEmpresa,cod1empresa,cod1fechaIngresoEmpresa,cod2empresa,cod2fechaIngresoEmpresa,asesorcomercial,nomFam1,nomFam2,ocuFam1,ocuFam2,dirFam1,dirFam2,celFam1,celFam2,nomPer1,nomPer2,ocuPer1,ocuPer2,dirPer1,dirPer2,celPer1,celPer2,estCivil,perCargo,estrato,timeResidencia,nomConyu,apeConyu,cedConyu,empConyu,celConyu,salPri,otrIng,comision,otrIngCon,detOrig,valArriendo,valSoste,valFin,otrGasto,casa,apto,finca,otro,cual,dirProp,ciuProp,escritura,notFec,nMatInm,valComProp,valHipProp,modelo,valVehiculo,valDeuVehiculo,fechVenCon,tiemLaborado,ParentescoFam1,ParentescoFam2,CiudadFam1,CiudadFam2,ParentescoPer1,ParentescoPer2,CiudadPer1,CiudadPer2,EstadoCivilCod1,EstadoCivilCod2,TotalIngresos,TotalGastos, ENDentidad1, ENDentidad2, ENDsaldoDeuda1, ENDsaldoDeuda2, ENDcuotaMensual1, ENDcuotaMensual2")] solicitudCredito solicitudCredito)
        {
            if (solicitudCredito.ENDsaldoDeuda1 != null)
            {
                solicitudCredito.ENDsaldoDeuda1 = solicitudCredito.ENDsaldoDeuda1.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.ENDsaldoDeuda2 != null)
            {
                solicitudCredito.ENDsaldoDeuda2 = solicitudCredito.ENDsaldoDeuda2.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.ENDcuotaMensual1 != null)
            {
                solicitudCredito.ENDcuotaMensual1 = solicitudCredito.ENDcuotaMensual1.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.ENDsaldoDeuda2 != null)
            {
                solicitudCredito.ENDsaldoDeuda2 = solicitudCredito.ENDsaldoDeuda2.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.valComercial != null)
            {
                solicitudCredito.valComercial = solicitudCredito.valComercial.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.cuota != null)
            {
                solicitudCredito.cuota = solicitudCredito.cuota.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.salarioEmpresa != null)
            {
                solicitudCredito.salarioEmpresa = solicitudCredito.salarioEmpresa.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.salPri != null)
            {
                solicitudCredito.salPri = solicitudCredito.salPri.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.TotalIngresos != null)
            {
                solicitudCredito.TotalIngresos = solicitudCredito.TotalIngresos.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.TotalGastos != null)
            {
                solicitudCredito.TotalGastos = solicitudCredito.TotalGastos.Replace('.'.ToString(), "");
            }
            if (solicitudCredito.val_solicitud != null)
            {
                solicitudCredito.val_solicitud = solicitudCredito.val_solicitud.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.celConyu != null)
            {
                solicitudCredito.celConyu = solicitudCredito.celConyu.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.otrIng != null)
            {
                solicitudCredito.otrIng = solicitudCredito.otrIng.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.otrIngCon != null)
            {
                solicitudCredito.otrIngCon = solicitudCredito.otrIngCon.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.comision != null)
            {
                solicitudCredito.comision = solicitudCredito.comision.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valArriendo != null)
            {
                solicitudCredito.valArriendo = solicitudCredito.valArriendo.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valSoste != null)
            {
                solicitudCredito.valSoste = solicitudCredito.valSoste.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valFin != null)
            {
                solicitudCredito.valFin = solicitudCredito.valFin.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.otrGasto != null)
            {
                solicitudCredito.otrGasto = solicitudCredito.otrGasto.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valHipProp != null)
            {
                solicitudCredito.valHipProp = solicitudCredito.valHipProp.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valComProp != null)
            {
                solicitudCredito.valComProp = solicitudCredito.valComProp.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valDeuVehiculo != null)
            {
                solicitudCredito.valDeuVehiculo = solicitudCredito.valDeuVehiculo.Replace('.'.ToString(), "");

            }
            if (solicitudCredito.valVehiculo != null)
            {
                solicitudCredito.valVehiculo = solicitudCredito.valVehiculo.Replace('.'.ToString(), "");

            }


            //validar null para todos


            if (ModelState.IsValid)
            {
                db.Entry(solicitudCredito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(solicitudCredito);
        }

        // GET: SolicitudCredito/solicitudCreditos/Delete/5
        public ActionResult Delete(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitudCredito solicitudCredito = db.SolicitudCredito.Find(id);
            if (solicitudCredito == null)
            {
                return HttpNotFound();
            }
            return View(solicitudCredito);
        }

        // POST: SolicitudCredito/solicitudCreditos/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            solicitudCredito solicitudCredito = db.SolicitudCredito.Find(id);
            db.SolicitudCredito.Remove(solicitudCredito);
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

        public ActionResult GetUsuario()
        {

            if (!User.Identity.IsAuthenticated)
            {
                var usuario = "noRegistrado";
                return Json(usuario, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var usuario = User.Identity.Name;
                return Json(usuario, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetPrestamoInfo(string terceroId)
        {

            int idDestino = db.Database.SqlQuery<int>("SELECT Destino_Id FROM dbo.Prestamos WHERE NIT='" + terceroId + "'").FirstOrDefault();
            int idPrestamo = db.Database.SqlQuery<int>("SELECT id FROM dbo.Prestamos WHERE NIT='" + terceroId + "'").FirstOrDefault();
            int contaPrestamo = db.Database.SqlQuery<int>("SELECT id FROM dbo.Prestamos WHERE NIT='" + terceroId + "'").Count();

            if (contaPrestamo > 0)
            {
                Prestamos prestamo = db.Prestamos.Find(idPrestamo);
                Destinos destino = db.Destinos.Find(idDestino);
                Lineas linea = db.Lineas.Find(destino.Lineas_Id);

                var response2 = new List<object>
                 {
                    new{

                        id_destino = prestamo.Destino_Id,
                        id_linea = destino.Lineas_Id,
                        nombre_destino = destino.Destino_Descripcion,
                        nombre_linea = linea.Lineas_Descripcion,
                        valorSolicitado = prestamo.Capital,
                        plazo = prestamo.Plazo,
                        cuota = prestamo.ValorPeriodo,
                        pagare = prestamo.Pagare,



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

        [HttpPost]
        public JsonResult GetTerceroInfo(string terceroId)
        {

            int contaPrestamo = db.Database.SqlQuery<int>("SELECT id FROM dbo.Prestamos WHERE NIT='" + terceroId + "'").Count();


            if (contaPrestamo > 0)
            {
                Tercero terceros = db.Terceros.Find(terceroId);
                Municipio municipiored = db.Municipio.Find(terceros.MUNICIPIO);

                Municipio muniNac = db.Municipio.Find(terceros.NACIO);
                Municipio muniExp = db.Municipio.Find(terceros.LUGAREXP);


                var response2 = new List<object>
                {
                    new{

                        ape1 =terceros.APELLIDO1,
                        ape2=terceros.APELLIDO2,
                        nom=terceros.NOMBRE1+" "+terceros.NOMBRE2,
                        ciudad = municipiored.Nom_muni,
                        barrio=terceros.BARRIO,
                        telefono=terceros.TEL,
                        celular=terceros.TELMOVIL,
                        salario=terceros.SALARIO,
                        dirRes=terceros.DIR,
                        correo=terceros.EMAIL,
                        NIT=terceros.NIT,
                        lugFecExp = muniExp.Nom_muni+" "+terceros.FECHAEXP.ToShortDateString(),
                        //lugFecNac = muniNac.Nom_muni+" "+terceros.FECHANAC.ToShortDateString()
                        lugFecNac = muniNac.Nom_muni+" "+terceros.FECHANAC.ToString()


                    }
                };

                return Json(response2);
            }
            else
            {
                string respuesta2 = "NO";
                {
                    return Json(respuesta2);
                }
            }


        }
        [HttpPost]
        public JsonResult GetTerceroInfo1(string terceroId)
        {

            if(terceroId != "")
            {
                Tercero terceros = db.Terceros.Find(terceroId);
                Municipio municipiored = db.Municipio.Find(terceros.MUNICIPIO);

                Municipio muniNac = db.Municipio.Find(terceros.NACIO);
                Municipio muniExp = db.Municipio.Find(terceros.LUGAREXP);


                var response2 = new List<object>
                {
                    new{

                        ape1 =terceros.APELLIDO1,
                        ape2=terceros.APELLIDO2,
                        nom=terceros.NOMBRE1+" "+terceros.NOMBRE2,
                        ciudad = municipiored.Nom_muni,
                        barrio=terceros.BARRIO,
                        telefono=terceros.TEL,
                        celular=terceros.TELMOVIL,
                        salario=terceros.SALARIO,
                        dirRes=terceros.DIR,
                        correo=terceros.EMAIL,
                        NIT=terceros.NIT,
                        lugFecExp = muniExp.Nom_muni+" "+terceros.FECHAEXP.ToShortDateString(),
                        //lugFecNac = muniNac.Nom_muni+" "+terceros.FECHANAC.ToShortDateString()
                        lugFecNac = muniNac.Nom_muni+" "+terceros.FECHANAC.ToString()


                    }
                };

                return Json(response2);
            }
            else
            {
                string resp = "NO";
                return Json(resp);
            }
           
        }        
    }
}
