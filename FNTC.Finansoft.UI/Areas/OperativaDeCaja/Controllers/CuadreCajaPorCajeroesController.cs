using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.OperativaDeCaja.Controllers
{
    public class CuadreCajaPorCajeroesController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: CuadreCajaPorCajeroes
        public ActionResult Index()
        {		
            var cuadreCajaPorCajero = db.CuadreCajaPorCajero.Include(c => c.configCajero).Include(c => c.Caja);
            return View(cuadreCajaPorCajero.ToList());
        }

        // GET: CuadreCajaPorCajeroes/Details/5
      
        // GET: CuadreCajaPorCajeroes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuadreCajaPorCajero cuadreCajaPorCajero = db.CuadreCajaPorCajero.Find(id);
			if (cuadreCajaPorCajero == null)
			{
				return HttpNotFound();
			}
			else
			{
				Session["cajero"+User.Identity.Name] = cuadreCajaPorCajero.nit_cajero;
				Session["cod_caja"+User.Identity.Name] = cuadreCajaPorCajero.codigo_caja;
				Session["ret_efect" + User.Identity.Name] = cuadreCajaPorCajero.retiros_cheque;
				Session["ret_cheque" + User.Identity.Name] = cuadreCajaPorCajero.retiros_cheque;
				Session["cons_efect" + User.Identity.Name] = cuadreCajaPorCajero.consignacion_efectivo;
				Session["cons_cheque" + User.Identity.Name] = cuadreCajaPorCajero.consignacion_cheque;
				Session["tope" + User.Identity.Name] = cuadreCajaPorCajero.tope;
				configCajero configcajero = db.configCajero.Find(Session["cajero" + User.Identity.Name]);
				Session["cta_efectivo" + User.Identity.Name] = configcajero.Cta_efectivo;
				Session["cta_cheque" + User.Identity.Name] = configcajero.Cta_cheque;
				Session["contrapartida_banco" + User.Identity.Name] = configcajero.Contr_banco;
				Session["contrapartida_otro" + User.Identity.Name] = configcajero.Contr_otro;
				Session["centrocosto" + User.Identity.Name] = configcajero.centrocosto;
				Session["CentroCostoCaja" + User.Identity.Name] = configcajero.CentroCostoCaja;
				Session["comprobante_cierre" + User.Identity.Name] = configcajero.Tipocomprobante_caja;
				
				TipoComprobante tiposComprobantes = db.TiposComprobantes.Find(Session["comprobante_cierre" + User.Identity.Name]);
				Session["consecutivo" + User.Identity.Name] =tiposComprobantes.CONSECUTIVO;
				return View(cuadreCajaPorCajero);


			}
            
        }

        // POST: CuadreCajaPorCajeroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CuadreCajaPorCajero cuadreCajaPorCajero = db.CuadreCajaPorCajero.Find(id);
			int cierrecons =  Convert.ToInt32(cuadreCajaPorCajero.cierre);
			if (cierrecons == 0)
			{
								
				Session["fallo" + User.Identity.Name] = "si";
				string ano = Convert.ToString(DateTime.Now.Year);
				string mes = Convert.ToString(DateTime.Now.Month);
				string dia = Convert.ToString(DateTime.Now.Day);
                //string fechacierre = Convert.ToString(DateTime.Now);
                DateTime fechacierre = DateTime.Now;

                var cuadre = db.CuadreCajaPorCajero.FirstOrDefault(j => j.id == id);
                cuadre.horacierre = fechacierre;
                cuadre.cierre = 1;
                db.Entry(cuadre).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


				decimal tope = cuadreCajaPorCajero.tope;

                /*
				var plancuentas = "UPDATE acc.PlanCuentas SET Saldo=Saldo+"+tope+"WHERE CODIGO='"+Session["contrapartida_otro"]+"'";
				db.Database.ExecuteSqlCommand(plancuentas);
				//Generar comprobante para cierre de caja.
				var Comprobante = "INSERT INTO acc.Comprobantes (TIPO, NUMERO, ANO, MES, DIA, CCOSTO, ELIMINADO, DETALLE, TERCERO, FPAGO, CTAFPAGO, NUMEXTERNO, VRTOTAL, SUMDBCR, FECHARealiz, MODIFICA, EXPORTADO, MARCASEG, BLOQUEADO, NUMIMP, PC,USUARIO,ANULADO)VALUES('"+Session["comprobante_cierre"]+"',"+Session["consecutivo"]+",'"+ano+"','"+mes+"','"+dia+"','"+Session["CentroCostoCaja"]+"',NULL,'Cierre Caja','"+Session["cajero"]+"',NULL,'"+Session["contrapartida_otro"]+"',NULL,"+tope+",0,'"+fechacierre+ "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'False')";
				db.Database.ExecuteSqlCommand(Comprobante);
				//MOVIMIENTOS 
				var movimieno1 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('"+Session["comprobante_cierre"]+"',"+Session["consecutivo"]+",'"+Session["cta_efectivo"]+"','"+Session["cajero"]+"','CAJEROS',0,"+tope+",0,'"+Session["CentroCostoCaja"]+"','"+fechacierre+"',NULL)";
				db.Database.ExecuteSqlCommand(movimieno1);
				var movimieno2 = "INSERT INTO acc.Movimientos (TIPO, NUMERO, CUENTA, TERCERO, DETALLE, DEBITO, CREDITO, BASE, CCOSTO, FECHAMOVIMIENTO, DOCUMENTO)VALUES('"+Session["comprobante_cierre"]+"',"+Session["consecutivo"]+",'"+Session["contrapartida_otro"]+"','"+Session["cajero"]+"','CAJA GENERAL',"+tope+ ",0,0,'"+Session["CentroCostoCaja"]+"','"+fechacierre+"',NULL)";
				db.Database.ExecuteSqlCommand(movimieno2);
				//SaldosCCS
				var saldosCCS = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('"+Session["cta_efectivo"]+"','"+Session["cajero"]+"','"+Session["CentroCostoCaja"]+"','"+ano+"','"+mes+"',0,"+tope+","+tope+")";
				db.Database.ExecuteSqlCommand(saldosCCS);
				var saldosCCS1 = "INSERT INTO acc.SaldosCCs (CUENTA, TERCERO, CCOSTO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('"+Session["contrapartida_otro"]+"','"+Session["cajero"]+"','"+Session["CentroCostoCaja"]+"','"+ano+"','"+mes+"',"+tope+",0,"+tope+")";
				db.Database.ExecuteSqlCommand(saldosCCS1);
				//saldoscuentas
				var saldoscuentas = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('"+Session["cta_efectivo"]+"','"+ano+"','"+mes+"',0,"+tope+","+tope+")";
				db.Database.ExecuteSqlCommand(saldoscuentas);
				var saldoscuentas1 = "INSERT INTO acc.SaldosCuentas (CODIGO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('"+Session["contrapartida_otro"]+"','"+ano+"','"+mes+"',"+tope+",0,"+tope+")";
				db.Database.ExecuteSqlCommand(saldoscuentas1);
				//sadosterceros
				var saldosterceros = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('"+Session["cta_efectivo"]+"','"+Session["cajero"]+"','"+ano+"','"+mes+"',0,"+tope+","+tope+")";
				db.Database.ExecuteSqlCommand(saldosterceros);
				var saldosterceros1 = "INSERT INTO acc.SaldosTerceros (CODIGO, TERCERO, ANO, MES, MDEBITO, MCREDITO, SALDO) VALUES('"+Session["contrapartida_otro"]+"','"+Session["cajero"]+"','"+ano+"','"+mes+"',"+tope+",0,"+tope+")";
				db.Database.ExecuteSqlCommand(saldosterceros1);
				//actulizamos consecutivo		
				Session["cons"] = Convert.ToInt32(Session["consecutivo"]) + 1;
				var updateconsecutivoTiposComprobantes = "UPDATE acc.TiposComprobantes SET CONSECUTIVO='"+ Session["cons"] + "' WHERE CODIGO='" + Session["comprobante_cierre"] + "'";
				db.Database.ExecuteSqlCommand(updateconsecutivoTiposComprobantes);
                */

                //CONTRUIR EL COMPROBANTE

                string comprobanteCierre = Session["comprobante_cierre" + User.Identity.Name].ToString();
                var consecutivoComprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == comprobanteCierre & x.INACTIVO == false);


                var comprobante = new Comprobante()
                {
                    TIPO = comprobanteCierre,
                    NUMERO = consecutivoComprobante.CONSECUTIVO,
                    ANO = Convert.ToString(DateTime.Now.Year),
                    MES = Convert.ToString(DateTime.Now.Month),
                    DIA = Convert.ToString(DateTime.Now.Day),
                    CCOSTO = Session["CentroCostoCaja" + User.Identity.Name].ToString(),
                    DETALLE = "Cierre Caja",
                    TERCERO = Session["cajero" + User.Identity.Name].ToString(),
                    CTAFPAGO = Session["contrapartida_otro" + User.Identity.Name].ToString(),
                    VRTOTAL = tope,
                    SUMDBCR = 0,
                    FECHARealiz = DateTime.Now,
                    ANULADO = false
                };

                db.Comprobantes.Add(comprobante);

                //CONSTRUIR LA LISTA DE MOVIMIENTOS
                List<Movimiento> listaDeMovimientos = new List<Movimiento>();
                var mov1 = new Movimiento()
                {
                    TIPO = comprobanteCierre,
                    NUMERO = consecutivoComprobante.CONSECUTIVO,
                    CUENTA = Session["cta_efectivo" + User.Identity.Name].ToString(),
                    TERCERO = Session["cajero" + User.Identity.Name].ToString(),
                    DETALLE = "CAJEROS",
                    DEBITO = 0,
                    CREDITO = tope,
                    BASE = 0,
                    CCOSTO = Session["CentroCostoCaja" + User.Identity.Name].ToString(),
                    FECHAMOVIMIENTO = DateTime.Now,
                };

                listaDeMovimientos.Add(mov1);
                var mov2 = new Movimiento()
                {
                    TIPO = comprobanteCierre,
                    NUMERO = consecutivoComprobante.CONSECUTIVO,
                    CUENTA = Session["contrapartida_otro" + User.Identity.Name].ToString(),
                    TERCERO = Session["cajero" + User.Identity.Name].ToString(),
                    DETALLE = "CAJA GENERAL",
                    DEBITO = tope,
                    CREDITO = 0,
                    BASE = 0,
                    CCOSTO = Session["CentroCostoCaja" + User.Identity.Name].ToString(),
                    FECHAMOVIMIENTO = DateTime.Now,
                };
                listaDeMovimientos.Add(mov2);

                var result = false;

                var comprobanteConst = new ComprobanteBO();
                result = comprobanteConst.AsentarCierreCaja(listaDeMovimientos, Convert.ToInt32(consecutivoComprobante.CONSECUTIVO), comprobanteCierre);

                if (result)
                {
                    string codcaja = Convert.ToString(Session["cod_caja" + User.Identity.Name]);
                    var cajaencero = "UPDATE dbo.Caja SET TopeMaximo_caja=0 WHERE Codigo_caja='" + codcaja + "'";
                    db.Database.ExecuteSqlCommand(cajaencero);

                    db.SaveChanges();
                }
               
                return RedirectToAction("Index");               
            }
			else
			{
				Session["fallo" + User.Identity.Name] = "no";
			}
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
    }
}
