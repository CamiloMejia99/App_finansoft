using FNTC.Finansoft.Accounting.DTO;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Email.Controllers
{
    public class EmailController : Controller
    {

        AccountingContext db = new AccountingContext();
        // GET: Email/Email
        public ActionResult Index()
        {
            return View();
        }


        public struct DateTimeSpan
        {
            private readonly int years;
            private readonly int months;
            private readonly int days;
            private readonly int hours;
            private readonly int minutes;
            private readonly int seconds;
            private readonly int milliseconds;

            public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
            {
                this.years = years;
                this.months = months;
                this.days = days;
                this.hours = hours;
                this.minutes = minutes;
                this.seconds = seconds;
                this.milliseconds = milliseconds;
            }

            public int Years { get { return years; } }
            public int Months { get { return months; } }
            public int Days { get { return days; } }
            public int Hours { get { return hours; } }
            public int Minutes { get { return minutes; } }
            public int Seconds { get { return seconds; } }
            public int Milliseconds { get { return milliseconds; } }

            enum Phase { Years, Months, Days, Done }

            public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
            {
                if (date2 < date1)
                {
                    var sub = date1;
                    date1 = date2;
                    date2 = sub;
                }

                DateTime current = date1;
                int years = 0;
                int months = 0;
                int days = 0;

                Phase phase = Phase.Years;
                DateTimeSpan span = new DateTimeSpan();
                int officialDay = current.Day;

                while (phase != Phase.Done)
                {
                    switch (phase)
                    {
                        case Phase.Years:
                            if (current.AddYears(years + 1) > date2)
                            {
                                phase = Phase.Months;
                                current = current.AddYears(years);
                            }
                            else
                            {
                                years++;
                            }
                            break;
                        case Phase.Months:
                            if (current.AddMonths(months + 1) > date2)
                            {
                                phase = Phase.Days;
                                current = current.AddMonths(months);
                                if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                    current = current.AddDays(officialDay - current.Day);
                            }
                            else
                            {
                                months++;
                            }
                            break;
                        case Phase.Days:
                            if (current.AddDays(days + 1) > date2)
                            {
                                current = current.AddDays(days);
                                var timespan = date2 - current;
                                span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                                phase = Phase.Done;
                            }
                            else
                            {
                                days++;
                            }
                            break;
                    }
                }

                return span;
            }
        }


        public static bool IsValidEmail(string strMailAddress)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strMailAddress, @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        [HttpPost]
        public JsonResult getCorreo(string nit)
        {
            string error = "";
            var dataTercero = db.Terceros.Where(x => x.NIT == nit).FirstOrDefault();
            if(dataTercero!=null)
            {
                if(dataTercero.EMAIL!=null && dataTercero.EMAIL!="")
                {
                    string correo = dataTercero.EMAIL;
                    bool bandera = IsValidEmail(correo);
                    if(!bandera)
                    {
                        error = "El correo registrado no contiene un formato válido. Por favor verifique los datos del asociado";
                        return new JsonResult { Data = new { status = false,error } };
                    }else
                    {
                        return new JsonResult { Data = new { status = true,correo } };
                    }

                }
                else
                {
                    error = "El asociado no tiene un correo registrado";
                    return new JsonResult { Data = new { status = false,error } };
                }
            }

            return new JsonResult { Data = new { status = true } };
        }

        [HttpPost]
        public JsonResult sendAllEmailAsync()
        {
            var asociados = db.Terceros.Where(x => x.EMAIL != "" && x.EMAIL != null).ToList();
            int n = 0;
            var texto = "activo";
            if(asociados!=null)
            {
                foreach(var item in asociados)
                {

                    bool bandera = sendEmail(item.EMAIL, item.NIT);
                    if (bandera == false)
                    {
                        return new JsonResult { Data = new { status = false } };
                    }
                }
                
            }
           return new JsonResult { Data = new { status = true, numero = n.ToString() } };

        }

        public bool sendEmail(string para, string nit)
        {
            string subject = "Estado de Cuenta Asociacion Mutual 'Asopascualina' " ;
            string message = "";
            var Estado = "1";
            var query = db.ConfiguracionCorreo.Where(x => x.estado == "1").ToList();

            if (query != null)
            {
                
                var email = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.email).Single();
                var pass = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.password).Single();
                var smtpClient = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.smtp).Single();
                var puertoSmtp = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.puerto).Single();


                try
                {
                    MailMessage correo = new MailMessage();
                    correo.From = new MailAddress(email);
                    correo.To.Add(para);
                    correo.Subject = subject;
                    correo.Body = message;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;

                    var actionPDF = new ActionAsPdf("EstadoDeCuentaPDF", new { nit })
                    {
                        FileName = nit + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 }
                    };

                    byte[] applicationPDFData = actionPDF.BuildPdf(this.ControllerContext);
                    MemoryStream pdfStream = new MemoryStream(applicationPDFData);
                    Attachment pdf = new Attachment(pdfStream, nit + ".pdf");

                    correo.Attachments.Add(pdf);


                    //configuración del servidor smtp

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = smtpClient;
                    smtp.Port = puertoSmtp;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Timeout = 10000;//
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;//
                    smtp.UseDefaultCredentials = false;
                    string cuentaCorreo = email;
                    string passwordCorreo = pass;
                    correo.BodyEncoding = UTF8Encoding.UTF8;//
                    correo.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;//
                    smtp.Credentials = new System.Net.NetworkCredential(cuentaCorreo, passwordCorreo);
                    smtp.Send(correo);

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
           
        }


        [HttpPost]
        public JsonResult EnviarCorreo(string asunto,string mensaje,string para,string nit)
        {


            string subject = "Estado de Cuenta Asociacion Mutual 'Asopascualina'";
            string message = "";
            var Estado = "1";
            var query = db.ConfiguracionCorreo.Where(x => x.estado == "1").ToList();

            if (query != null)
            {
                var email = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.email).Single();
                var pass = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.password).Single();
                var smtpClient = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.smtp).Single();
                var puertoSmtp = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.puerto).Single();

                if (asunto != "")
                {
                    subject = asunto;
                }
                if (mensaje != "")
                {
                    message = mensaje;
                }

                try
                {
                    MailMessage correo = new MailMessage();
                    correo.From = new MailAddress(email);
                    correo.To.Add(para);
                    correo.Subject = subject;
                    correo.Body = message;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;

                    var actionPDF = new ActionAsPdf("EstadoDeCuentaPDF", new { nit })
                    {
                        FileName = nit + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 }
                    };

                    byte[] applicationPDFData = actionPDF.BuildPdf(this.ControllerContext);
                    MemoryStream pdfStream = new MemoryStream(applicationPDFData);
                    Attachment pdf = new Attachment(pdfStream, nit + ".pdf");

                    //STMP HOTMAIL
                    /* las credencial
                     * smtp.UseDefaultCredentials = false;
                     */

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = smtpClient;

                    smtp.Host = smtpClient;
                    smtp.Port = puertoSmtp;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Timeout = 10000;//
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;//
                    smtp.UseDefaultCredentials = false;
                    string cuentaCorreo = email;
                    string passwordCorreo = pass;
                    smtp.Credentials = new NetworkCredential(cuentaCorreo, passwordCorreo);
                    correo.BodyEncoding = UTF8Encoding.UTF8;//
                    correo.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;//
                    correo.Attachments.Add(pdf);
                    smtp.Send(correo);
                    return new JsonResult { Data = new { status = true } };


                }
                catch (Exception ex)
                {

                    return new JsonResult { Data = new { status = false } };
                }

            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        public JsonResult EnviarCorreoAportes (string asunto, string mensaje, string para, string nit)
        {


            string subject = "FACTURA APORTES ASOPASCUALINA";
            string message = "Señor(a) usuario, la asociacion mutual \"Asopascualina\" le comparte su factura de Aportes.";
            var Estado = "1";
            var query = db.ConfiguracionCorreo.Where(x => x.estado == "1").ToList();

            if (query != null)
            {
                var email = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.email).Single();
                var pass = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.password).Single();
                var smtpClient = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.smtp).Single();
                var puertoSmtp = (from ep in db.ConfiguracionCorreo where ep.estado == Estado select ep.puerto).Single();

                if (asunto != "")
                {
                    subject = asunto;
                }
                if (mensaje != "")
                {
                    message = mensaje;
                }

                try
                {
                    MailMessage correo = new MailMessage();
                    correo.From = new MailAddress(email);
                    correo.To.Add(para);
                    correo.Subject = subject;
                    correo.Body = message;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;

                    var actionPDF = new ActionAsPdf("EstadoDeCuentaPDF", new { nit })
                    {
                        FileName = nit + ".pdf",
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 }
                    };

                    byte[] applicationPDFData = actionPDF.BuildPdf(this.ControllerContext);
                    MemoryStream pdfStream = new MemoryStream(applicationPDFData);
                    Attachment pdf = new Attachment(pdfStream, nit + ".pdf");

                    //STMP HOTMAIL
                    /* las credencial
                     * smtp.UseDefaultCredentials = false;
                     */

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = smtpClient;

                    smtp.Host = smtpClient;
                    smtp.Port = puertoSmtp;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Timeout = 10000;//
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;//
                    smtp.UseDefaultCredentials = false;
                    string cuentaCorreo = email;
                    string passwordCorreo = pass;
                    smtp.Credentials = new NetworkCredential(cuentaCorreo, passwordCorreo);
                    correo.BodyEncoding = UTF8Encoding.UTF8;//
                    correo.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;//
                    correo.Attachments.Add(pdf);
                    smtp.Send(correo);
                    return new JsonResult { Data = new { status = true } };


                }
                catch (Exception ex)
                {

                    return new JsonResult { Data = new { status = false } };
                }

            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }


        public ActionResult EstadoDeCuentaPDF(string nit)
        {
            //nit = "36994839";

            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            #region datosTerceros
            string documento = "";
            string nombreasociado = "";
            string salario = "";
            string antiguedad = "";
            string agencia = "";

            var tercero = (from pc in db.Terceros where pc.NIT == nit select pc).FirstOrDefault();
            if(tercero!=null)
            {
                documento = nit;
                nombreasociado = tercero.NOMBRE1 + " " + tercero.NOMBRE2 + " " + tercero.APELLIDO1 + " " + tercero.APELLIDO2;
                salario = Convert.ToInt32(tercero.SALARIO).ToString("N0", formato);
                var dataAgencia=(from pc in db.agencias where pc.codigoagencia == tercero.DEPENDENCIA select pc.nombreagencia).FirstOrDefault();
                if(dataAgencia!=null)
                {
                    agencia = dataAgencia;
                }
            }

            #endregion


            #region APORTES
            
            var fichaAporte = (from fp in db.FichasAportes where fp.idPersona == nit select fp).FirstOrDefault();

            List<Array> aportes = new List<Array>();
            if(fichaAporte!=null)
            {
                string[] data = new string[5];

                //obtener numero de meses desde que abrio la ficha de aporte hasta la fecha actual
                int diferenciaMeses = 0;
                int diferenciaanios = 0;
                DateTime fechApertura = Convert.ToDateTime(fichaAporte.fechaApertura);
                DateTime fechNow = DateTime.Now;
                var dateSpan = DateTimeSpan.CompareDates(fechApertura, fechNow);
                diferenciaMeses = dateSpan.Months;
                diferenciaanios = dateSpan.Years;
                if (diferenciaanios > 0)
                {
                    diferenciaMeses = diferenciaMeses + (diferenciaanios * 12);
                }
                //.......
                //obtener deuda total
                var dataPagosFichasAporte = db.FactOpcaja.Where(x => x.nit_propietario_cuenta == nit).ToList();

                int num = 0;
                int deudaTotal = 0, n = 0;
                if (dataPagosFichasAporte != null)
                {
                    num = dataPagosFichasAporte.Count();
                    deudaTotal = Convert.ToInt32(fichaAporte.valor) * ((diferenciaMeses + 1) - num);

                    n = (diferenciaMeses + 1) - num;
                    if (n < 0)
                    {
                        n = 0;
                        deudaTotal = 0;
                    }
                }
                //

                antiguedad = diferenciaMeses + " MESES";

                data[0] = fichaAporte.numeroCuenta;
                data[1] = fichaAporte.fechaApertura.ToString();
                data[2] = Convert.ToInt32(fichaAporte.valor.Replace(".", "")).ToString("N2",formato);
                data[3] = Convert.ToInt32(fichaAporte.totalAportes).ToString("N2", formato);
                var NumeroDePagos = (from pc in db.FactOpcaja where pc.nit_propietario_cuenta == tercero.NIT select pc).Count();
                var SaldoEnMora = ((diferenciaMeses - NumeroDePagos) * (Convert.ToInt32(fichaAporte.valor))).ToString("N2", formato);
                data[4] = deudaTotal.ToString("N2", formato);

                aportes.Add(data);
            }//fin if

            #endregion

            #region creditos
            decimal totalCapital = 0, totalMora = 0, totalG = 0, totalCapitalMora = 0, totalCorriente = 0;
            var consulta = (from C in db.Creditos
                            join P in db.Prestamos on C.Prestamo_Id equals P.id
                            where C.Creditos_Cedula == nit && C.Prestamo_Id == P.id
                            select new { C.Pagare, P.fechadesembolso, C.Creditos_Cuota, P.Plazo }

                  ).ToList();
            List<Array> prestamos = new List<Array>();
            if(consulta.Count>0)
            {
                foreach (var item in consulta)
                {
                    var historial = db.HistorialCreditos.Where(x => x.pagare == item.Pagare).ToList();
                    decimal totalDiaMora = 0;
                    decimal cuota = historial.OrderByDescending(x => x.id).Select(x => x.proximaCuota).FirstOrDefault();
                    decimal capital = historial.Where(x => x.estado != "pazYsalvo" && x.estado != "abono").Select(x => x.saldoCapital).FirstOrDefault();
                    decimal capitalMora = historial.Where(x => x.estado != "pazYsalvo" && x.estado != "abono").Select(x => x.capitalEnMora).Sum();
                    decimal corrienteNormal = historial.Where(x => x.estado == "normal").Select(x => x.interesCorriente).FirstOrDefault();
                    totalCapital += capital;
                    totalCapitalMora += capitalMora;
                    totalCorriente += corrienteNormal;
                    var saldoDia = db.HistorialCreditos.Where(x => x.estado == "enMora" && x.pagare == item.Pagare).ToList();
                    foreach (var item2 in saldoDia)
                    {
                        totalDiaMora += item2.capitalEnMora + item2.interesMora + item2.interesCorriente + item2.valorCosto;
                    }

                    totalMora += totalDiaMora;

                    string[] prestamo = new string[7];

                    prestamo[0] = item.Pagare;
                    prestamo[1] = item.fechadesembolso.ToString("yyyy-MM-dd");
                    prestamo[2] = cuota.ToString("N2", formato);
                    prestamo[3] = capital.ToString("N2", formato);
                    prestamo[4] = item.Plazo.ToString("N0", formato);
                    prestamo[5] = (totalDiaMora).ToString("N0", formato);
                    prestamo[6] = (capital + totalCorriente + totalDiaMora - capitalMora).ToString("N2", formato);


                    prestamos.Add(prestamo);
                }

                totalG = totalCapital + totalCorriente + totalMora - totalCapitalMora;
            }
            
            #endregion

            #region ViewBag

            ViewBag.documento = documento;
            ViewBag.nombreAsociado = nombreasociado;
            ViewBag.salario = salario;
            ViewBag.antiguedad = antiguedad;
            ViewBag.agencia = agencia;
            ViewBag.aportes = aportes;
            ViewBag.prestamos = prestamos;
            ViewBag.totalCapital = totalCapital.ToString("N2",formato);
            ViewBag.totalMora = totalMora.ToString("N2", formato);
            ViewBag.totalGeneral = totalG.ToString("N2", formato);


            #endregion
            return View();
        }

        public ActionResult Print()
        {
            string nit = "36994839";

            string filePath = Server.MapPath("~/Temporal/"+nit+".pdf");
            var actionPDF = new ActionAsPdf("EstadoDeCuentaPDF", new {nit})
            {
                FileName = nit+".pdf",
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = {Left=1,Right=1}
            };

            //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

            //var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            //fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
            //fileStream.Close();
            return actionPDF;

        }

    }
}