
using FNTC.Finansoft.UI.Areas.ControlErrores.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FNTC.Finansoft.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
#warning esto debe ir en un projecto aparte dado que el automapper queda en el appDomain
            //   FNTC.Finansoft.UI.Areas.Terceros.ViewModels.JqGrid.ConfigureViewModel.Configure();
            FNTC.Finansoft.Accounting.Init.Init.ConfigureAutoMapper();

            // FNTC.Finansoft.AutoMapper.Init.
            
        }

        protected void Application_Error()
        {

            try
            {

               

                Exception ex = Server.GetLastError();
                HttpException httpexception = ex as HttpException;
                String accion = "";
                //string path1 = @"h:\root\home\gerentefinantec-001\www\pruebasASOPASCUALINOS\log";
                //string path1 = @"log";
                
                // 4. Obtener el directorio base del programa 
                string path1 = System.AppDomain.CurrentDomain.BaseDirectory+"log";
                Log oLog = new Log(path1);
                string men = ex.Message + " ---_" + ex.TargetSite;
                oLog.Add(men);
                

                if (httpexception != null)
                {
                    int httpCode = httpexception.GetHttpCode();
                    switch (httpCode)
                    {
                        case (int)HttpStatusCode.Unauthorized:
                            accion = "Error401";
                            break;
                        case (int)HttpStatusCode.NotFound:
                            accion = "Error404";
                            break;
                        case (int)HttpStatusCode.ServiceUnavailable:
                            accion = "Error502";
                            break;
                        case (int)HttpStatusCode.BadGateway:
                            accion = "Error502";
                            break;
                        case (int)HttpStatusCode.GatewayTimeout:
                            accion = "Error504";
                            break;
                        default:
                            accion = "ErrorGeneral";
                            break;
                    }
                }
                else
                {
                    accion = "ErrorGeneral";
                }
                Context.ClearError();
                RouteData rutaerror = new RouteData();
                rutaerror.DataTokens.Add("area", "ControlErrores");
                rutaerror.Values.Add("Controller", "Errores");
                rutaerror.Values.Add("action", accion);
                IController controlador = new ControlErroresController();
                controlador.Execute(
                    new RequestContext(new HttpContextWrapper(Context), rutaerror));



            }
            catch (Exception ex)
            {
                string script = @"<script type='text/javascript'>
                            alert('{0}');
                        </script>";

                
            }
        }


    }
    }


