#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

#endregion
/*
 TODO: 
 * Verificar  por cada cuenta si requiere tercero
 * * Verificar  por cada cuenta si requiere centro de costo
 * Crear un log por cada modificacion
 * Crear Metodo ActualizarComprobante
 */
namespace FNTC.Finansoft.Accounting.BLL
{

#warning un log para cada modificacion de un comprobante
    public sealed class Comprobante
    {
        #region Properties
        DAL.Model.Contabilidad.Comprobante _comprobante;
        public PartidaDoble Entries { get; private set; }
        public bool IsNew { get; private set; }
        public bool IsDeleted { get; private set; }

        /*AUDITORIA*/
        public string User { get; private set; }
        //FECHA OPERACION
        //IP
        //FECHA ULTIMA MODIFICACION -LOOOOOOOOG
        // 
        #endregion

        #region ctor
#warning que pasa con el consecutivo si estoy generando comprobantes desde varios compus al tiempo? lock?
        public Comprobante(string tipoComprobante, DateTime fechaComprobante
            , string detalle, PartidaDoble entries, string CCosto, string NumeroExterno = "")
        {
            _comprobante = new DAL.Model.Contabilidad.Comprobante();
            _comprobante.TIPO = tipoComprobante;
            _comprobante.NUMERO = GetConsecutivoComprobante(tipoComprobante).ToString();
            _comprobante.ANO = fechaComprobante.Year.ToString();
            _comprobante.MES = fechaComprobante.Month.ToString();
            _comprobante.DIA = fechaComprobante.Day.ToString();
            _comprobante.DETALLE = detalle;
            _comprobante.USUARIO = "test en Consola";//esto lo sacado de identity
            _comprobante.CCOSTO = CCosto;
            _comprobante.NUMEXTERNO = NumeroExterno;

            this.Entries = entries;
        }
        #endregion

        #region Methods
        #region verify
        /// <summary>
        /// Verifica si el comprobante esta correcto
        /// </summary>
        /// <param name="issues">out IssueStruct issues </param>
        /// <returns>true si esta correcto</returns>
        public bool Verify(out IssueStruct issues)
        {
            issues = new IssueStruct();

            if (Entries.IsBalanced() == false)
                issues.AddError(IssueMessages.msgUnbalancedVchr);

            if (string.IsNullOrEmpty(_comprobante.DETALLE))
                issues.AddWarning(IssueMessages.msgNoVchrNarration);

            int rownum = 1;

            //verificaciones de las anotaciones
            foreach (Anotacion en in Entries)
            {
                //si no tiene descripcion
                if (string.IsNullOrEmpty(en.Descripcion) == true)
                {
#warning si la entrada no tiene descripcion la copio de el comprobante?
                    issues.Warnings.Add(string.Format(IssueMessages.msgNoEntryNarration, rownum));
                }

                //si el centro de costo viene empty
                if (string.IsNullOrEmpty(en.CentroDeCosto) == true)
                    issues.AddError(string.Format(IssueMessages.msgNoCostCntr, rownum));
                var errors = new List<string>();

                //si tiene errores
                if (!(en.Verify(errors)))
                    foreach (var item in errors)
                    {
                        issues.AddError(item + " en Anotación " + rownum.ToString());
                    }

                //issues.AddError(string.Format(msgNoCostCntr, rownum));
                rownum++;
            }

            if (issues.Errors.Count > 0 || issues.Warnings.Count > 0)
                return false;
            else
                return true;
        }
        public bool Verify()
        {
            IssueStruct issues;
            return this.Verify(out issues);
        }

        private bool CuentaExiste(string codigo)
        {

            using (var ctx = new DAL.Model.AccountingContext())
            {

                //ctx.
            }
            throw new NotImplementedException();
        }

        private bool CuentaRequiereCC(string codigo)
        {
            throw new NotImplementedException();
        }

        private bool CuentaRequiereTercero(string codigo)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Settle
        public bool Asentar()
        {
            /*Construyto el commprobante*/
            if (!this.Verify())
            {
                throw new InvalidOperationException("El comprobante no se puede asentar mientras contenga errores");
            }
            else //no contiene errores-continuo
            {
                // comprobante. --esto va en DAL
                using (var ctx = new DAL.Model.AccountingContext())
                {
                    //creo el comrpobanmte 
                    //verifico el tipoComprobante y sacvo el autonumerico
                    //comprobante lleva sumacredeb
                    _comprobante.SUMDBCR = Entries.Balance;
                    _comprobante.VRTOTAL = Entries.Total;
                    this._comprobante.FECHARealiz = DateTime.Now; //?? fecha contabler y fecha de asiento

                    //agrego los movimientos
                    //obtengo las sumas de credito 

                    var ent = Entries.Select(x => x.GetMovimiento()).ToList();

                    //actualiza saldos en SaldosTerceros
                    //actualiza saldos en SaldosCcostos
                    //actualiza saldos en SaldosCuentas
                    //pbtengo la cuenta de SCuentas y actualizo
                    foreach (var item in ent)
                    {
                        item.TIPO = _comprobante.TIPO;
                        item.NUMERO = _comprobante.NUMERO;

                        #region SaldosCuentas
                        //generales
                        int ano = Int32.Parse(_comprobante.ANO);
                        int mes = Int32.Parse(_comprobante.MES);
                        decimal credito = item.CREDITO;
                        decimal debito = item.DEBITO;


                        //saldoCuentas
                        var saldoCuentra = ctx.SaldosCuentas.Find(item.CODIGO);
                        if (saldoCuentra == null)
                        {
                            throw new InvalidOperationException("No existe la cuenta ");
                        }
                        saldoCuentra.MCREDITO += credito;
                        saldoCuentra.MDEBITO += debito;
                        saldoCuentra.ANO = ano;
                        saldoCuentra.MES = mes;


                        #endregion

                        #region SaldosTerceros
                        //si no  existe en SaldosTerceros lo debo crear
                        //item requiere tercero?
                        var saldoTercero = ctx.SaldosTerceros
                            .Where(st => st.TERCERO == item.TERCERO
                            && st.MES == mes.ToString() && st.ANO == ano.ToString())
                            .FirstOrDefault();

                        if (null == saldoTercero)
                        {
                            saldoTercero = new DAL.Model.Contabilidad.SaldosTercero()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO
                            };
                            ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            saldoTercero.MDEBITO += debito;
                            saldoTercero.MCREDITO += credito;
                            ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                        }
                        saldoTercero.CODIGO = item.CODIGO;
                        saldoTercero.TERCERO = item.TERCERO;
                        saldoTercero.MES = mes.ToString();
                        saldoTercero.ANO = ano.ToString(); 
                        #endregion

                        //saldos CCs
                        //obtengo el registro en SaldosCCs si no existe lo creo
                        var saldoCC = ctx.SaldosCCs
                            .Where(st => st.CCOSTO == item.TERCERO
                            && st.MES == mes.ToString() && st.ANO == ano.ToString())
                            .FirstOrDefault();

                        if (null == saldoCC)
                        {
                            saldoCC = new DAL.Model.Contabilidad.SaldoCC()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO
                            };
                            ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            saldoCC.MDEBITO += debito;
                            saldoCC.MCREDITO += credito;
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Modified;
                        }

                        
                    }


                    ctx.Movimientos.AddRange(ent); //entradas
                    ctx.Comprobantes.Add(_comprobante); //comprobante

                    //actualizo consecutivo
                    var _tipoComprobante = ctx.TiposComprobantes.Find(_comprobante.TIPO);
                    _tipoComprobante.CONSECUTIVO = _comprobante.NUMERO;
                    ctx.Entry(_tipoComprobante).State = System.Data.Entity.EntityState.Modified;



                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected > 0) return true;
                }
            }
            return false;
        } 
        #endregion

        #region TODO
        /*
                Asentar?
             * : add el comprobante 
             * las sumas debo hacerlas en el periodo especifico
             *  hago las sumas en saldos cuentas
             *  hago las sumas en saldos terceros
             *  hago las sumas en saldos ccostos
             */
    

        #region Actualizar
        public bool ActualizarAsiento(int comprobanteId)
        {
            throw new NotImplementedException();
        }

        public bool ActualizarAsiento(DAL.Model.Contabilidad.Comprobante comprobante)
        {
            throw new NotImplementedException();
        } 
        #endregion

        #region Anular
        public bool AnularAsiento(int comprobanteID)
        {
            throw new NotImplementedException();
        }

        public bool AnularAsiento(DAL.Model.Contabilidad.Comprobante comprobante)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #region borrar
        public bool BorrarAsiento(int comprobanteId)
        {
            using (var ctx = new DAL.Model.AccountingContext())
            {
                var comprobante = ctx.Comprobantes.Find(comprobanteId);

                //obtengo todos los movimientos del comprobante
                var movimientos = 
                    ctx.Movimientos.Where(m => m.TIPO == comprobante.TIPO && m.NUMERO == comprobante.NUMERO).ToList();

                comprobante.MODIFICA = DateTime.Now.ToString();

                foreach (var item in movimientos)
                {
                        #region SaldosCuentas
                        decimal credito = item.CREDITO;
                        decimal debito = item.DEBITO;
                        int mes = Int32.Parse(comprobante.MES);
                        int ano = Int32.Parse(comprobante.ANO);

                        //saldoCuentas
                        var saldoCuentra = ctx.SaldosCuentas.Find(item.CODIGO);
                        if (saldoCuentra == null)
                        {
                            throw new InvalidOperationException("No existe la cuenta  en SCuentas:" + item.CODIGO);
                        }

                        saldoCuentra.MCREDITO -= credito;
                        saldoCuentra.MDEBITO -= debito;
                        saldoCuentra.ANO = ano;
                        saldoCuentra.MES = mes;
                        #endregion

                        //si no  existe en SaldosTerceros lo debo crear
                        //item requiere tercero?
                        var saldoTercero = ctx.SaldosTerceros
                            .Where(st => st.TERCERO == item.TERCERO
                            && st.MES == mes.ToString() && st.ANO == ano.ToString())
                            .FirstOrDefault();

                        if (null == saldoTercero)
                        {
                            //error
                        }
                        else
                        {
                            saldoTercero.MDEBITO -= debito;
                            saldoTercero.MCREDITO -= credito;
                            ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                        }
                        saldoTercero.CODIGO = item.CODIGO;
                        saldoTercero.TERCERO = item.TERCERO;
                        saldoTercero.MES = mes.ToString();
                        saldoTercero.ANO = ano.ToString();
                }


            }
            throw new NotImplementedException();
        }

        public bool BorrarAsiento(DAL.Model.Contabilidad.Comprobante comprobante)
        {
            throw new NotImplementedException();
        } 
        #endregion

        public bool ActualizarSaldos()
        {
         //   #error error
            throw new NotImplementedException();
        }
        
        #endregion
        #endregion

        /// <summary>
        /// Obtiene el consecutivo segun el tipo de comprobante
        /// </summary>
        /// <param name="tipoComprobante"></param>
        /// <returns>el consecutivo</returns>
        private int GetConsecutivoComprobante(string tipoComprobante)
        {
            using (var ctx = new DAL.Model.AccountingContext())
            {
                var comprobante = ctx.TiposComprobantes.Find(tipoComprobante);
                int consecutivo = Int32.Parse(comprobante.CONSECUTIVO) + 1;
                return consecutivo;
            }
        }

   
    }

    #region Issues
    public class IssueStruct
    {
        public List<string> Errors { get; private set; }
        public List<string> Warnings { get; private set; }

        public IssueStruct()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }

        public bool IsValid()
        {
            return Errors.Count == decimal.Zero;
        }
    }
    public static class IssueMessages
    {
        public static string msgNoCostCntr = "Falta centro de costo para {0} tipo cuenta {1}.";
        public static string msgUnbalancedVchr = "Comprobante no esta balanceado";
        public static string msgNoVchrNarration = "Comprobante requiere descripcion";
        public static string msgNoEntryNarration = "La anotacion {0} debe tener descripcion";
        public static string msgRequiereTercero = "LA anotacion {0} requiere tercero";
    }
    #endregion
}
