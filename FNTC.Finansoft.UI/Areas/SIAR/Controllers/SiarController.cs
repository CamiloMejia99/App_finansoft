using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.SIAR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.SIAR.Controllers
{
    public class SiarController : Controller
    {
        AccountingContext db = new AccountingContext();
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

       
        // GET: SIAR/Siar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewLineas()
        {
            return View();
        }

        public ActionResult ViewPerdidaEsperada()
        {
            return View();
        }
        public ActionResult ViewAnalisisCredito()
        {
            //inicio select list para líneas
            List<SelectListItem> lineas = new List<SelectListItem>();
            lineas.Add(new SelectListItem { Text = "Todas", Value = "0" });
            IList<Lineas> listadoLineas = db.Lineas.ToList();
            foreach (var item in listadoLineas)
            {
                lineas.Add(new SelectListItem { Text = item.Lineas_Descripcion, Value = item.Lineas_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin select list para lineas

            //inicio select list para líneas
            List<SelectListItem> subClas = new List<SelectListItem>();
            subClas.Add(new SelectListItem { Text = "Todo", Value = "0" });
            IList<SIARsubclasificacionCartera> listadoSubClas = db.SIARsubclasificacionCartera.ToList();
            foreach (var item in listadoSubClas)
            {
                subClas.Add(new SelectListItem { Text = item.descripcion, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin select list para lineas

            ViewBag.lineas = lineas;
            ViewBag.subclasificacion = subClas;
            return View();
        }

        public JsonResult perdidaEsperada()
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            var listado = new List<ViewModelPerdidaEsperada>();
            var parametros = db.SIARparametros.Where(x => x.id == 1).FirstOrDefault();
            var calificaciones = db.SIARcalificacion.ToList();
            if (parametros!=null && parametros.estado == true && calificaciones.Count>0)
            {

                var historial = db.HistorialCreditos.Where(x => x.fecha <= parametros.fechaCorte).ToList();
                var pagares = historial.Select(x => x.pagare).Distinct().ToList();
                var creditos = db.Creditos.ToList();

                int contador = 1;
                foreach (var item in pagares)
                {
                    string calificacion = "NA";
                    int mora = 0;
                    decimal saldoCapital = 0;   

                    var dataH = historial.Where(x => x.pagare == item).ToList();
                    var condicion = dataH.Where(x => x.estado == "liquidado").FirstOrDefault();
                    if(condicion == null)//esta condición valida que el crédito aún no sea haya liquidado para que pertenezca a cartera
                    {
                        var ultData = dataH.OrderByDescending(x => x.id).FirstOrDefault();
                        if(ultData.estado == "normal" && ultData.numeroCuota == 0)
                        {
                            ultData = dataH.Take(dataH.Count - 1).OrderByDescending(x => x.id).FirstOrDefault();
                        }
                        var dataCredito = creditos.Where(x => x.Pagare == item).FirstOrDefault();

                        var dataMora = dataH.Where(x => x.estado == "enMora").ToList();
                        if (dataMora.Count > 0)
                        {
                            saldoCapital = dataMora.Select(x => x.saldoCapital).FirstOrDefault() + dataMora.Select(x => x.interesCorriente).Sum() + dataMora.Select(x => x.interesMora).Sum();
                            mora = dataMora.Select(x => x.diasEnMora).FirstOrDefault();
                        }
                        else
                        {
                            saldoCapital = ultData.saldoCapital + ultData.interesCorriente + ultData.interesMora;
                            mora = ultData.diasEnMora;
                        }
                        var auxCalificacion = calificaciones.Where(x => x.rangoIni <= mora && x.rangoFin >= mora).FirstOrDefault();
                        if (auxCalificacion != null)
                        {
                            calificacion = auxCalificacion.calificacion;
                        }
                        var objeto = new ViewModelPerdidaEsperada()
                        {
                            id = contador,
                            nit = ultData.NIT,
                            nombre = dataCredito.terceroFK.NOMBRE1 + " " + dataCredito.terceroFK.NOMBRE2 + " " + dataCredito.terceroFK.APELLIDO1 + " " + dataCredito.terceroFK.APELLIDO2,
                            mora = mora,
                            calificacion = calificacion,
                            saldo = "$" + saldoCapital.ToString("N2", formato),
                            linea = dataCredito.lineaFK.Lineas_Descripcion,
                            pagare = item
                        };

                        listado.Add(objeto);

                        contador++;
                    }
                    
                }//fin foreach


                //reiniciamos los parametros
                parametros.clasificacion = 0;
                parametros.linea = 0;
                parametros.subclasificacion = 0;
                parametros.fechaCorte = new DateTime(1753,1,1);
                parametros.estado = false;
                db.Entry(parametros).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }//fin if

            var respuesta = new DTODataTables<ViewModelPerdidaEsperada>
            {
                data = listado
            };
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getLineas()
        {
            var lineas = db.Lineas.ToList();
            var listado = new List<ViewModelLineas>();
            foreach (var item in lineas)
            {
                string estado = "";
                if(item.Lineas_Activo){estado = "Activo";}else{estado = "Inactivo";}
                var objeto = new ViewModelLineas()
                {
                    id = item.Lineas_Id,
                    descripcion = item.Lineas_Descripcion,
                    codigo = item.Lineas_Codigo,
                    estado = estado,
                    
                };

                listado.Add(objeto);

            }



            var respuesta = new DTODataTables<ViewModelLineas>
            {
                data = listado
            };
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult perdidaAcumulada()
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            var listado = new List<ViewModelPerdidaAcumulada>();
            var creditos = new List<BCreditos>();
            var parametros = db.SIARparametros.Where(x => x.id == 1).FirstOrDefault();
            var calificaciones = db.SIARcalificacion.ToList();
            //acumuladores
            decimal AA = 0, A = 0, BB = 0, B = 0, CC = 0, C = 0, D = 0, E = 0, totalSaldoCapital=0;

            //...
            if (parametros != null && parametros.estado == true && calificaciones.Count > 0)
            {
                var historial = db.HistorialCreditos.Where(x => x.fecha <= parametros.fechaCorte).ToList();
                var pagares = historial.Select(x => x.pagare).Distinct().ToList();
                if(parametros.linea!=0)
                {
                    creditos = db.Creditos.Where(x => x.Lineas_Id == parametros.linea).ToList();
                }
                else
                {
                    creditos = db.Creditos.ToList();
                }
                

                
                foreach (var item in pagares)
                {
                    //crear condicion para que cumpla la linea de credito y sumar en los acumuladores
                    var condicionCredito = creditos.Where(X => X.Pagare == item).FirstOrDefault();
                    if(condicionCredito!=null)
                    {
                        string calificacion = "NA";
                        int mora = 0;
                        decimal saldoCapital = 0;

                        var dataH = historial.Where(x => x.pagare == item).ToList();
                        var condicion = dataH.Where(x => x.estado == "liquidado").FirstOrDefault();
                        if (condicion == null)//esta condición valida que el crédito aún no sea haya liquidado para que pertenezca a cartera
                        {
                            var ultData = dataH.OrderByDescending(x => x.id).FirstOrDefault();
                            if (ultData.estado == "normal" && ultData.numeroCuota == 0)
                            {
                                ultData = dataH.Take(dataH.Count - 1).OrderByDescending(x => x.id).FirstOrDefault();
                            }
                            var dataCredito = creditos.Where(x => x.Pagare == item).FirstOrDefault();

                            var dataMora = dataH.Where(x => x.estado == "enMora").ToList();
                            if (dataMora.Count > 0)
                            {
                                saldoCapital = dataMora.Select(x => x.saldoCapital).FirstOrDefault() + dataMora.Select(x => x.interesCorriente).Sum() + dataMora.Select(x => x.interesMora).Sum();
                                mora = dataMora.Select(x => x.diasEnMora).FirstOrDefault();
                            }
                            else
                            {
                                saldoCapital = ultData.saldoCapital + ultData.interesCorriente + ultData.interesMora;
                                mora = ultData.diasEnMora;
                            }
                            var auxCalificacion = calificaciones.Where(x => x.rangoIni <= mora && x.rangoFin >= mora).FirstOrDefault();
                            if (auxCalificacion != null)
                            {
                                calificacion = auxCalificacion.calificacion;
                                if(auxCalificacion.calificacion == "A")
                                {
                                    A += saldoCapital;
                                }
                                else if (auxCalificacion.calificacion == "AA")
                                {
                                    AA += saldoCapital;
                                }
                                else if (auxCalificacion.calificacion == "B")
                                {
                                    B += saldoCapital;
                                }else if (auxCalificacion.calificacion == "BB")
                                {
                                    BB += saldoCapital;
                                }else if (auxCalificacion.calificacion == "CC")
                                {
                                    CC += saldoCapital;
                                }else if (auxCalificacion.calificacion == "C")
                                {
                                    C += saldoCapital;
                                }else if (auxCalificacion.calificacion == "D")
                                {
                                    D += saldoCapital;
                                }else if (auxCalificacion.calificacion == "E")
                                {
                                    E += saldoCapital;
                                }
                                totalSaldoCapital += saldoCapital;
                            }
                            
                        }
                    }
                     
                }//fin foreach

                int contador = 1;
                foreach (var item in calificaciones)
                {
                    string nomCalificacion = "";
                    decimal EA = 0,PEA=0,PDI=item.PDI,PE=0,PI=0;

                    if (item.calificacion == "A")
                    {
                        nomCalificacion = "A";
                        EA = A;
                    }
                    else if (item.calificacion == "AA")
                    {
                        nomCalificacion = "AA";
                        EA = AA;
                    }
                    else if (item.calificacion == "B")
                    {
                        nomCalificacion = "B";
                        EA = B;
                    }
                    else if (item.calificacion == "BB")
                    {
                        nomCalificacion = "BB";
                        EA = BB;
                    }
                    else if (item.calificacion == "CC")
                    {
                        nomCalificacion = "CC";
                        EA = CC;
                    }
                    else if (item.calificacion == "C")
                    {
                        nomCalificacion = "C";
                        EA = C;
                    }
                    else if (item.calificacion == "D")
                    {
                        nomCalificacion = "D";
                        EA = D;
                    }
                    else if (item.calificacion == "E")
                    {
                        nomCalificacion = "E";
                        EA = E;
                    }

                    PI = (EA * 100) / totalSaldoCapital;
                    var objeto = new ViewModelPerdidaAcumulada()
                    {
                        id=contador,
                        rango = item.descripcionRango,
                        calificacion = item.calificacion,
                        PI = PI.ToString("N2", formato) +"%",
                        EA = "$"+EA.ToString("N2",formato),
                        PDI=(item.PDI*100).ToString("N2", formato)+"%",
                        PE="$"+(EA*item.PDI).ToString("N2", formato),
                        PEA= "$" + E.ToString("N2", formato)

                    };
                    listado.Add(objeto);
                    contador++;
                }

                

                

                //reiniciamos los parametros
                parametros.clasificacion = 0;
                parametros.linea = 0;
                parametros.subclasificacion = 0;
                parametros.fechaCorte = new DateTime(1753, 1, 1);
                parametros.estado = false;
                db.Entry(parametros).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }//fin if

            
            var respuesta = new DTODataTables<ViewModelPerdidaAcumulada>
            {
                data = listado
            };
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult guardarParametros(string clasificacion, string linea, string subclasificacion, string fechaCorte)
        {
            //se optó por guardar los parámetros en la base de datos porque en la función ajax del datatable fue imposible enviarlos por ahí

            try
            {
                var parametros = db.SIARparametros.Where(x => x.id == 1).FirstOrDefault();
                if(parametros!=null)
                {
                    if(clasificacion!=null)
                    {
                        parametros.clasificacion = Convert.ToInt32(clasificacion);
                    }
                    else { parametros.clasificacion = 0; }
                    if (linea != null)
                    {
                        parametros.linea = Convert.ToInt32(linea);
                    }
                    else { parametros.linea = 0; }
                    if (subclasificacion != null)
                    {
                        parametros.subclasificacion = Convert.ToInt32(subclasificacion);
                    }
                    else { parametros.subclasificacion = 0; }

                    parametros.fechaCorte = Convert.ToDateTime(fechaCorte);
                    parametros.estado = true;

                    
                    db.Entry(parametros).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    int auxID = 1;
                    var data = new SIARparametros()
                    {
                        clasificacion = 0,
                        linea = 0,
                        subclasificacion = 0,
                        fechaCorte = Convert.ToDateTime(fechaCorte),
                        estado = true
                    };

                    db.SIARparametros.Add(data);
                    

                }

                db.SaveChanges();
                return new JsonResult { Data = new { status = true } };
            }
            catch (Exception ex)
            {
                
                return new JsonResult { Data = new { status = false } };
            }

            
        }
    }
}