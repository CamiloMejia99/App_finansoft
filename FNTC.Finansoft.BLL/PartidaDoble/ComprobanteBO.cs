/*
 TODO: 
 * Verificar  por cada cuenta si requiere tercero
 * * Verificar  por cada cuenta si requiere centro de costo
 * Crear un log por cada modificacion
 * Crear Metodo ActualizarComprobante
 * #warning que pasa con el consecutivo si estoy generando comprobantes desde varios compus al tiempo? lock?
#warning un log para cada modificacion de un comprobante
 */

#region using
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.BLL.Aportes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;


using System.Data;

using System.Net;
using System.Web;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.UI.Areas.ControlErrores.Controllers;


#endregion

namespace FNTC.Finansoft.Accounting.BLL
{
    public class ComprobanteBO
    {
        #region Properties

        public DTO.Contabilidad.Comprobante _comprobante;

        public string NombreComprobante { get { return this.GetNombreComprobante(); } }

        public string Clase { get { return this.GetClase(); } }

        public string TipoComprobante { get { return this.GetTipo(); } set { _comprobante.TIPO = value; } }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaComprobante { get { return this.GetFecha(); } set { this._comprobante.FECHARealiz = value; } }

        public string Tercero { get { return _comprobante.TERCERO; } set { _comprobante.TERCERO = value; } }

        public string Consecutivo { get { return this.GetConsecutivo(); } set { _comprobante.NUMERO = value; } }

        public string Detalle { get { return _comprobante.DETALLE; } set { this.SetDetalle(value); } }

        public string CCosto { get; set; }

        public string FPago { get { return _comprobante.FPAGO; } set { _comprobante.FPAGO = value; } }

        public string NumeroExterno { get { return _comprobante.NUMEXTERNO; } set { this.SetNumeroExterno(value); } }

        public decimal Balance { get { return Entries.Balance; } }

        public decimal Credito { get { return Entries.Credito; } }

        public decimal Debito { get { return Entries.Debito; } }

        public bool Anulado { get { return _comprobante.ANULADO; } set { _comprobante.ANULADO = value; } }

        public string Observacion { get { return _comprobante.Observacion; } set { _comprobante.Observacion = value; } }

        public bool IsNew { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsOK { get { return this.Verify(); } }

        public PartidaDoble Entries { get; private set; }

        public IssueStruct Issues { get; set; }

        /*AUDITORIA*/
        public string User { get; private set; }
        //FECHA OPERACION
        //IP
        //FECHA ULTIMA MODIFICACION -LOOOOOOOOG
        // 
        #endregion properties

        #region ctors

        public ComprobanteBO()
        {
            _comprobante = new DTO.Contabilidad.Comprobante();
            this.Entries = new PartidaDoble();
            this.IsNew = false;
        }

        public ComprobanteBO(string tipoComprobante)
        {
            this.IsNew = true;

            _comprobante = new DTO.Contabilidad.Comprobante();
            //traigo el tipo de comprobante
            var _tc = new DAL.Comprobantes.ComprobantesDAL().GetComprobanteByCODIGO(tipoComprobante);

            //si el comprobante es Saldos iniciales y no existe lo creo
            _comprobante.TIPO = tipoComprobante;
            SetConsecutivoComprobante(); //_tc++? 
            _comprobante.FECHARealiz = DateTime.Now;
            Entries = new PartidaDoble();
            IsNew = true;

            //si es RC o CE
            //var clase = _tc.CLASEComprobante;
            var clase = _comprobante.TIPO.Substring(0, 2);
            if (clase == "RC" || clase == "CE")
            {
                var codigo = "";
                //obtengo la cuenta de la fpago
                try
                {
                    try
                    {
                        _comprobante.FPAGO = _tc.FormaPagoID.ToString();
                        // codigo = _tc.FormaPago.CodigoCuenta;

                        var fp = new AccountingContext().FormasPago.Find(_tc.FormaPagoID);
                        codigo = fp.CodigoCuenta;

                        //_comprobante.FPAGO = fp.ID.ToString();
                    }
                    catch (Exception)
                    {
                        //esto no deberia pasar

                    }
                    var entryFPAgo = new Anotacion()
                    {
                        Cuenta = codigo,
                        Index = 1,
                        Credito = 0,
                        Debito = 0,
                        Descripcion = "Forma de Pago"
                    };
                    Entries.Add(entryFPAgo);
                }
                catch (Exception ex)
                {
                    //esto nunca deberia suceder 
                    //log
                    throw;
                }
            }
        }
        //yhggghhh
        public ComprobanteBO(
            string tipoComprobante, DateTime fechaComprobante, string detalle
            , string CCosto, string fPago, string NumeroExterno = "", PartidaDoble entries = null)
        {
            _comprobante = new DTO.Contabilidad.Comprobante();
            _comprobante.TIPO = tipoComprobante;
            SetConsecutivoComprobante();
            _comprobante.ANO = fechaComprobante.Year.ToString();
            _comprobante.MES = fechaComprobante.Month.ToString();
            _comprobante.DIA = fechaComprobante.Day.ToString();
            _comprobante.DETALLE = detalle;
            _comprobante.USUARIO = "test en Consola";//esto lo sacado de identity
            _comprobante.CCOSTO = CCosto;
            _comprobante.NUMEXTERNO = NumeroExterno;
            _comprobante.FPAGO = fPago;

            this.Entries = entries == null ? entries : null;
            if (this.Entries == null)
            {
                this.Entries = new PartidaDoble();
            }
        }

        #endregion

        //Estos van en comprobantesBLL
        #region METHODS

        #region VERIFY

        public bool Verify(out IssueStruct issues)
        {
            //var _isOk = false;
            issues = new IssueStruct();
            if (Entries.Count > 0)
            {
                if (Entries.IsBalanced() == false)
                    issues.AddError(IssueMessages.msgUnbalancedVchr, "0_Comprobante");
            }
            if (string.IsNullOrEmpty(_comprobante.DETALLE))
                issues.AddError(IssueMessages.msgNoVchrNarration, "0_Comprobante");

            if (Entries.Count == 0)
                issues.AddError("El Comprobante esta vacio", "0_Comprobante");

            //  int rownum = 1;

            //verificaciones de las anotaciones
            var errors = new List<string>();
            foreach (Anotacion en in Entries)
            {
                //TODO

                //si no tiene descripcion
                if (string.IsNullOrEmpty(en.Descripcion) == true)
                {
#warning si la entrada no tiene descripcion la copio de el comprobante?
                    issues.AddWarning(string.Format(IssueMessages.msgNoEntryNarration, en.Index), "2_Detalle", en.Index);
                }

                //si el centro de costo viene empty
                //if (string.IsNullOrEmpty(en.CentroDeCosto) == true)
                //    issues.AddError(string.Format(IssueMessages.msgNoCostCntr,en.Cuenta, rownum));

                //si tiene errores
                if (!(en.Verify(errors)))
                    foreach (var item in errors)
                    {
                        issues.AddError(item + " en Anotación " + en.Index.ToString(), "1_Cuenta", en.Index);
                    }

                using (var ctx = new AccountingContext())
                {
                    var ctaExiste = true;
                    //verifico si  cuenta requiere CCs Tercero
                    if (!this.CuentaExiste(en.Cuenta, ctx))
                    {

                        issues.AddError("La cuenta " + en.Cuenta + " en Anotación " + en.Index.ToString() + " No existe", "1_Cuenta", en.Index);
                        ctaExiste = false;
                    }//esto nunca deberia pasar


                    if (ctaExiste && this.CuentaRequiereCC(en.Cuenta, ctx))
                    {

                        //verifico si existe ceentro de costo
                        if (!this.CentroDeCostoExiste(en.CentroDeCosto, ctx))
                        {
                            //esto nunca deberia pasar
                            issues.AddError("No existe el centro de Costo especificado para la cuenta [" + en.Cuenta + "] [" + en.CentroDeCosto + "]", "4_CC", en.Index);
                        }
                        //si requiere y viene empty
                        if (String.IsNullOrEmpty(en.CentroDeCosto))
                        {
                            issues.AddError("Requiere centro de costo", "4_CC", en.Index);
                        }
                    }

                    //requiere terceri
                    if (ctaExiste && this.CuentaRequiereTercero(en.Cuenta, ctx))
                    {
                        //si requiere y viene empty
                        if (String.IsNullOrEmpty(en.Tercero))
                        {
                            issues.AddError("Cuenta : [" + en.Cuenta + "] en " + en.Index + " requiere Tercero", "3_Tercero", en.Index);
                            //si viene empty el tercero no va a existir
                        }
                        //verifico si existe tercero
                        else if (ctx.Terceros.Find(en.Tercero) == null)
                        {
                            //esto nunca deberia pasar
                            issues.AddError("No existe el tercero especificado en " + en.Index + " : " + en.Tercero, "0_Comprobante");
                        }
                    }
                }
            }
            issues.Errors.OrderBy(x => x.Key);

            Issues = issues;
            if (issues.Errors.Count > 0 || issues.Warnings.Count > 0)
                return false;
            else
                return true;
        }

        public bool Verify()
        {
            var i = this.Issues;
            return this.Verify(out i);
        }

        private bool CuentaExiste(string codigo, AccountingContext ctx)
        {
            return ctx.PlanCuentas.Find(codigo) != null;
        }

        private bool CuentaRequiereCC(string codigo, AccountingContext ctx)
        {
            return ctx.PlanCuentas.Find(codigo).REQCCOSTO == true;
        }

        private bool CuentaRequiereTercero(string codigo, AccountingContext ctx)
        {
            bool requiere = ctx.PlanCuentas.Find(codigo).REQTERCERO == true;
            return requiere;
        }

        private bool CentroDeCostoExiste(string cc, AccountingContext ctx)
        {
            return ctx.CentrosCostos.Find(cc) == null ? false : true;
        }

        #endregion

        #region SETTLE
        public bool Asentar()
        {
            //LISTA DE ERRORES
            //1. ERROR EN Verify()
            //2. ERROR EN isNew
            //3. YA EXISTE EL CONSECUTIVO
            //4. NO EXISTE EL AUXILIAR
            //5. SALDO INSUFICIENTE EN APORTES
            //6. NO ENTRA EN EL IF NI EN EL ELSE LO CUAL ES IMPOSIBLE :V
            //0. TODO OK
            using (var ctx = new AccountingContext())
            {
                /*Construyto el commprobante*/
                if (!this.Verify())
                {
                    // throw new InvalidOperationException("El comprobante no se puede asentar mientras contenga errores");
                    return false;
                }
                else //no contiene errores-continuo
                {
                    //verifico  que no exista con el mismo consecutivo
                    if (!this.IsNew)//si no es nuevo 
                    {
                        return false;
                    }

                    var consecutivo = _comprobante.NUMERO;
                    var existe = ctx.Comprobantes.Where(x => x.NUMERO == consecutivo && x.TIPO.Equals(_comprobante.TIPO)).FirstOrDefault() == null ? false : true;
                    if (existe)
                    {
                        //no se puede asentar como nuevo , xk ya existe
                        return false;
                    }

                    var ano = _comprobante.FECHARealiz.Year;
                    var mes = _comprobante.FECHARealiz.Month;
                    var dia = _comprobante.FECHARealiz.Day;

                    //creo el comrpobanmte 
                    _comprobante.SUMDBCR = Entries.Balance;
                    _comprobante.VRTOTAL = Entries.Total;
                    _comprobante.ANO = ano.ToString();
                    _comprobante.MES = mes.ToString();
                    _comprobante.DIA = dia.ToString();
                    _comprobante.FPAGO = this.FPago;
                    var primerIndex = Entries.First();
                    _comprobante.CTAFPAGO = Entries.First(e => e.Index == primerIndex.Index).Cuenta;
                    _comprobante.NUMEXTERNO = this.NumeroExterno;
                    _comprobante.CCOSTO = Entries.First(e => e.Index == primerIndex.Index).CentroDeCosto;
                    _comprobante.TERCERO = Entries.First(e => e.Index == primerIndex.Index).Tercero;
                    _comprobante.FECHARealiz = DateTime.Now; //?? fecha contabler y fecha de asiento

                    //agrego los movimientos
                    //obtengo las sumas de credito 

                    //lineas para pago de aportes por movimientos
                    var MovCuotaAdmon = new Movimiento();
                    bool BanderaAporte = false;
                    //---------------------------------

                    var entries = Entries.Select(x => x.GetMovimiento()).ToList();


                    //pbtengo la cuenta de SCuentas y actualizo
                    foreach (var item in entries)
                    {
                        var auxiliar = ctx.PlanCuentas.Find(item.CUENTA);
                        if (null == auxiliar)
                        {
                            //no he creado el auxiliar -esto no deberia pasar NUNCA!!!
                            return false;
                            throw new InvalidOperationException("No existe el auxiliar");
                        }


                        //si es cuenta de aporte realizamos el retiro o consignación de aporte
                        string cuentaAporte = item.CUENTA;
                        var configAporte = ctx.Configuracion1.Where(x => x.idCuenta == cuentaAporte && x.activo == true).FirstOrDefault();
                        if (configAporte != null)
                        {
                            var fichaAporte = ctx.FichasAportes.Where(x => x.numeroCuenta == item.cuentaPagare).FirstOrDefault();
                            if (fichaAporte != null)
                            {
                                long val = Convert.ToInt64(fichaAporte.totalAportes);
                                long valorRecibido = 0;
                                string operation = "";
                                if (item.DEBITO > 0)
                                {

                                    valorRecibido = Convert.ToInt64(entries.Select(x => x.DEBITO).Sum());
                                    if (valorRecibido > val)
                                    {
                                        return false;
                                    }
                                    val -= valorRecibido;
                                    operation = "Retiro Movimiento";
                                }
                                else
                                {
                                    val += Convert.ToInt64(item.CREDITO);
                                    valorRecibido = Convert.ToInt64(item.CREDITO);
                                    operation = "Consignacion Movimiento";

                                    item.CREDITO -= 10000;//disminuimos el valor de administración

                                    MovCuotaAdmon = new Movimiento()
                                    {
                                        TIPO = _comprobante.TIPO,
                                        NUMERO = _comprobante.NUMERO,
                                        CUENTA = "416540001",
                                        TERCERO = item.TERCERO,
                                        DETALLE = "CUOTAS DE ADMINISTRACION",
                                        DEBITO = 0,
                                        CREDITO = 10000,
                                        BASE = 0,
                                        CCOSTO = item.CCOSTO,
                                        FECHAMOVIMIENTO = new DateTime(ano, mes, dia)
                                    };
                                    BanderaAporte = true;
                                }
                                fichaAporte.totalAportes = val.ToString();
                                ctx.Entry(fichaAporte).State = System.Data.Entity.EntityState.Modified;

                                var caja = new FactOpcaja()
                                {
                                    fecha = DateTime.Now,
                                    factura = _comprobante.TIPO + "-" + _comprobante.NUMERO,
                                    codigo_caja = "52005",
                                    nit_cajero = "1",
                                    numero_cuenta = item.cuentaPagare,
                                    nit_propietario_cuenta = item.TERCERO,
                                    valor_recibido = valorRecibido,
                                    valor_efectivo = valorRecibido,
                                    saldo_total_cuenta = val,
                                    total = valorRecibido,
                                    operacion = operation

                                };

                                ctx.FactOpcaja.Add(caja);



                            }
                        }

                        //............................................

                        //generales
                        item.TIPO = _comprobante.TIPO;
                        item.NUMERO = _comprobante.NUMERO;
                        //item.FECHAMOVIMIENTO = DateTime.Now;
                        item.FECHAMOVIMIENTO = new DateTime(ano, mes, dia);

                        decimal credito = item.CREDITO;
                        decimal debito = item.DEBITO;


                        //actualiza saldos en SaldosTerceros
                        //actualiza saldos en SaldosCcostos
                        //actualiza saldos en SaldosCuentas
                        //////////////////////////////////////
                        #region SaldosCuentas
                        //al crear el auxiliar se crea con mes y ano en 0s
                        //verifico si existe el auxiliar no importa de que mes
                        //si no existe el registro para el mes lo creo de lo contrario opero
                        //obtengo la naturaleza
                        var naturaleza = auxiliar.NATURALEZA;

                        var saldoAuxiliar = ctx.SaldosCuentas
                         .Where(st => st.CODIGO == item.CUENTA
                         && st.MES == mes && st.ANO == ano)
                         .FirstOrDefault();

                        //si no existe para ese mes 
                        if (saldoAuxiliar == null)
                        {
                            saldoAuxiliar = new DTO.Contabilidad.SaldoCuenta()
                            {
                                CODIGO = item.CUENTA,
                                // Auxiliar = item.CUENTA,
                                ANO = ano,
                                MES = mes,
                                MDEBITO = debito,
                                MCREDITO = credito
                                //  SALDO = debito - credito,
                                //  NAT = naturaleza
                            };
#warning Revisar
                            //    saldoAuxiliar.Auxiliar = ctx.PlanCuentas.Find(item.CUENTA) as CuentaAuxiliar;

                            ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            saldoAuxiliar.MCREDITO += credito;
                            saldoAuxiliar.MDEBITO += debito;
                            saldoAuxiliar.ANO = ano;
                            saldoAuxiliar.MES = mes;
                            // saldoAuxiliar.SALDO += debito - credito;

                            ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Modified;
                        }
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoAuxiliar.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoAuxiliar.SALDO += credito - debito;
                        }

                        #endregion
                        /////////////////////////////////////
                        #region SaldosTerceros
                        //si no  existe en SaldosTerceros lo debo crear
                        //item requiere tercero?
                        var saldoTercero = ctx.SaldosTerceros
                            .Where(st => st.TERCERO == item.TERCERO
                            && st.MES == mes && st.ANO == ano && st.CODIGO == item.CUENTA)
                            .FirstOrDefault();

                        if (null == saldoTercero)
                        {
                            saldoTercero = new DTO.Contabilidad.SaldosTercero()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO,
                            };
                            ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            saldoTercero.MDEBITO += debito;
                            saldoTercero.MCREDITO += credito;
                            ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                        }
                        saldoTercero.CODIGO = item.CUENTA;
                        saldoTercero.TERCERO = item.TERCERO;
                        saldoTercero.MES = mes;
                        saldoTercero.ANO = ano;
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoTercero.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoTercero.SALDO += credito - debito;
                        }

                        #endregion
                        /////////////////////////////////////////////////////////////
                        #region SaldosCCs
                        //obtengo el registro en SaldosCCs si no existe lo creo excepto si no viene centro de costo
                        if (!item.CCOSTO.Equals(""))
                        {

                            var saldoCC = ctx.SaldosCCs
                                .Where(st => st.CCOSTO == item.CCOSTO
                                && st.MES == mes && st.ANO == ano && st.CUENTA == item.CUENTA)
                                .FirstOrDefault();

                            if (null == saldoCC)
                            {
                                saldoCC = new DTO.Contabilidad.SaldoCC()
                                {
                                    MDEBITO = item.DEBITO,
                                    MCREDITO = item.CREDITO
                                };
                                ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Added;

                                saldoCC.CCOSTO = item.CCOSTO;
                                saldoCC.CUENTA = item.CUENTA;
                                saldoCC.ANO = ano;
                                saldoCC.MES = mes;
                                saldoCC.TERCERO = item.TERCERO;
                            }

                            else
                            {
                                saldoCC.MDEBITO += debito;
                                saldoCC.MCREDITO += credito;
                                ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Modified;
                            }
                            if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                            {
                                saldoCC.SALDO += debito - credito;
                            }
                            else
                            {
                                saldoCC.SALDO += credito - debito;
                            }
                            #endregion
                        }

                        //////////////////////////////////////////////
                        //var configuracion = new BLLAportes().ObtenerConfiguracion();
                        //int CuentaInt = Int32.Parse(item.CUENTA);
                        //if (CuentaInt == configuracion.idCuenta)
                        //{
                        //    var fichamod = ctx.FichasAportes.Where(st => st.idPersona == item.TERCERO).FirstOrDefault();
                        //    var totalaporte = Int32.Parse(fichamod.totalAportes);
                        //    if (item.DEBITO == 0)
                        //    {
                        //        int aporte = Decimal.ToInt32(item.CREDITO);
                        //        int suma = totalaporte + aporte;
                        //        string myString = suma.ToString();
                        //        fichamod.totalAportes = myString;
                        //    }
                        //    else if (item.CREDITO == 0)
                        //    {
                        //        int aporte = Decimal.ToInt32(item.DEBITO);
                        //        if (totalaporte < aporte)
                        //        {
                        //            return false;
                        //        }
                        //        else
                        //        {
                        //            int suma = totalaporte - aporte;
                        //            string myString = suma.ToString();
                        //            fichamod.totalAportes = myString;
                        //        }
                        //    }

                        //    ctx.Entry(fichamod).State = System.Data.Entity.EntityState.Modified;

                        //}

                    }

                    if (BanderaAporte) { entries.Add(MovCuotaAdmon); }
                    ctx.Movimientos.AddRange(entries); //entradas
                    ctx.Comprobantes.Add(_comprobante); //comprobante

                    try
                    {
                        int rowsAffected = ctx.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            //si no  hubo problmeas actulizo consecutivois y saldos
                            //actualizo consecutivo -- esto debe ir en caso de success
                            var _tipoComprobante = ctx.TiposComprobantes.Find(_comprobante.TIPO);

                            _tipoComprobante.CONSECUTIVO = _comprobante.NUMERO;

                            ctx.Entry(_tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                            int rowsActualizaSaldos = ctx.SaveChanges();
                            //Actualizo los saldos
                            #region SaldoGeneralEnPlanDecuentas

                            this.Mayorizar(ctx, entries);
                            ctx.SaveChanges();
                            //agrupar del auxiliar hasta la subcuenta
                            //de la subcuenta hasta la cuenta
                            //de la cuenta al auxiliar
                            ///---el valor que traslado es el saldo del auxiliar = saldoAuxiliar.SALDO
                            //var saldoATransladar = saldoAuxiliar.SALDO;

                            //estrategia
                            //obtengo el saldo actualizado de los auxiliares y lanzo la actualizacion hacia arriba y la mando async parta que retone
                            //solo actualizo los que modifique --


                            //debo obtener
                            #endregion

                            //finalmente devuelvo true --? sino actualizo saldo?
                            return true;

                        }
                    }
                    catch (DbEntityValidationException dbE)
                    {
                        Console.WriteLine(dbE.Message);
                        string path1 = System.AppDomain.CurrentDomain.BaseDirectory + "log";
                        Log oLog = new Log(path1);
                        string men = dbE.Message + " ---_" + dbE.TargetSite;
                        oLog.Add(men);
                    }

                }
                return false;
            }
        }

        public bool AsentarDesembolso(List<Movimiento> movimientos, int ConsecutivoMov, string TipoDeComprobante)
        {
            using (var ctx = new AccountingContext())
            {
                var ano = DateTime.Now.Year;
                var mes = DateTime.Now.Month;
                var dia = DateTime.Now.Day;

                //pbtengo la cuenta de SCuentas y actualizo
                foreach (var item in movimientos)
                {
                    var auxiliar = ctx.PlanCuentas.Find(item.CUENTA);
                    if (null == auxiliar)
                    {
                        //no he creado el auxiliar -esto no deberia pasar NUNCA!!!
                        return false;
                        throw new InvalidOperationException("No existe el auxiliar");
                    }

                    decimal credito = item.CREDITO;
                    decimal debito = item.DEBITO;

                    //actualiza saldos en SaldosTerceros
                    //actualiza saldos en SaldosCcostos
                    //actualiza saldos en SaldosCuentas
                    //////////////////////////////////////
                    #region SaldosCuentas
                    //al crear el auxiliar se crea con mes y ano en 0s
                    //verifico si existe el auxiliar no importa de que mes
                    //si no existe el registro para el mes lo creo de lo contrario opero
                    //obtengo la naturaleza
                    var naturaleza = auxiliar.NATURALEZA;

                    var saldoAuxiliar = ctx.SaldosCuentas
                     .Where(st => st.CODIGO == item.CUENTA
                     && st.MES == mes && st.ANO == ano)
                     .FirstOrDefault();

                    //si no existe para ese mes 
                    if (saldoAuxiliar == null)
                    {
                        saldoAuxiliar = new DTO.Contabilidad.SaldoCuenta()
                        {
                            CODIGO = item.CUENTA,
                            // Auxiliar = item.CUENTA,
                            ANO = ano,
                            MES = mes,
                            MDEBITO = debito,
                            MCREDITO = credito
                            //  SALDO = debito - credito,
                            //  NAT = naturaleza
                        };
#warning Revisar
                        //    saldoAuxiliar.Auxiliar = ctx.PlanCuentas.Find(item.CUENTA) as CuentaAuxiliar;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoAuxiliar.MCREDITO += credito;
                        saldoAuxiliar.MDEBITO += debito;
                        saldoAuxiliar.ANO = ano;
                        saldoAuxiliar.MES = mes;
                        // saldoAuxiliar.SALDO += debito - credito;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Modified;
                    }
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoAuxiliar.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoAuxiliar.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////
                    #region SaldosTerceros
                    //si no  existe en SaldosTerceros lo debo crear
                    //item requiere tercero?
                    var saldoTercero = ctx.SaldosTerceros
                        .Where(st => st.TERCERO == item.TERCERO
                        && st.MES == mes && st.ANO == ano && st.CODIGO == item.CUENTA)
                        .FirstOrDefault();

                    if (null == saldoTercero)
                    {
                        saldoTercero = new DTO.Contabilidad.SaldosTercero()
                        {
                            MDEBITO = item.DEBITO,
                            MCREDITO = item.CREDITO,
                        };
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoTercero.MDEBITO += debito;
                        saldoTercero.MCREDITO += credito;
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                    }
                    saldoTercero.CODIGO = item.CUENTA;
                    saldoTercero.TERCERO = item.TERCERO;
                    saldoTercero.MES = mes;
                    saldoTercero.ANO = ano;
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoTercero.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoTercero.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////////////////////////////
                    #region SaldosCCs
                    //obtengo el registro en SaldosCCs si no existe lo creo excepto si no viene centro de costo
                    if (!item.CCOSTO.Equals(""))
                    {

                        var saldoCC = ctx.SaldosCCs
                            .Where(st => st.CCOSTO == item.CCOSTO
                            && st.MES == mes && st.ANO == ano && st.CUENTA == item.CUENTA)
                            .FirstOrDefault();

                        if (null == saldoCC)
                        {
                            saldoCC = new DTO.Contabilidad.SaldoCC()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO
                            };
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Added;

                            saldoCC.CCOSTO = item.CCOSTO;
                            saldoCC.CUENTA = item.CUENTA;
                            saldoCC.ANO = ano;
                            saldoCC.MES = mes;
                            saldoCC.TERCERO = item.TERCERO;
                        }

                        else
                        {
                            saldoCC.MDEBITO += debito;
                            saldoCC.MCREDITO += credito;
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Modified;
                        }
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoCC.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoCC.SALDO += credito - debito;
                        }
                        #endregion
                    }

                    //////////////////////////////////////////////
                    var configuracion = new BLLAportes().ObtenerConfiguracion();
                    int CuentaInt = Int32.Parse(item.CUENTA);
                    if (CuentaInt == configuracion.idCuenta)
                    {
                        var fichamod = ctx.FichasAportes.Where(st => st.idPersona == item.TERCERO).FirstOrDefault();
                        var totalaporte = Int32.Parse(fichamod.totalAportes);
                        if (item.DEBITO == 0)
                        {
                            int aporte = Decimal.ToInt32(item.CREDITO);
                            int suma = totalaporte + aporte;
                            string myString = suma.ToString();
                            fichamod.totalAportes = myString;
                        }
                        else if (item.CREDITO == 0)
                        {
                            int aporte = Decimal.ToInt32(item.DEBITO);
                            if (totalaporte < aporte)
                            {
                                return false;
                            }
                            else
                            {
                                int suma = totalaporte - aporte;
                                string myString = suma.ToString();
                                fichamod.totalAportes = myString;
                            }
                        }

                        ctx.Entry(fichamod).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                ctx.Movimientos.AddRange(movimientos); //entradas

                try
                {
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        //si no  hubo problmeas actulizo consecutivois y saldos
                        //actualizo consecutivo -- esto debe ir en caso de success
                        var _tipoComprobante = ctx.TiposComprobantes.Find(TipoDeComprobante);

                        _tipoComprobante.CONSECUTIVO = Convert.ToString(ConsecutivoMov + 1);

                        ctx.Entry(_tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                        int rowsActualizaSaldos = ctx.SaveChanges();
                        //Actualizo los saldos
                        #region SaldoGeneralEnPlanDecuentas

                        this.Mayorizar(ctx, movimientos);
                        ctx.SaveChanges();
                        //agrupar del auxiliar hasta la subcuenta
                        //de la subcuenta hasta la cuenta
                        //de la cuenta al auxiliar
                        ///---el valor que traslado es el saldo del auxiliar = saldoAuxiliar.SALDO
                        //var saldoATransladar = saldoAuxiliar.SALDO;

                        //estrategia
                        //obtengo el saldo actualizado de los auxiliares y lanzo la actualizacion hacia arriba y la mando async parta que retone
                        //solo actualizo los que modifique --


                        //debo obtener
                        #endregion

                        //finalmente devuelvo true --? sino actualizo saldo?
                        return true;

                    }
                }
                catch (DbEntityValidationException dbE)
                {
                    Console.WriteLine(dbE.Message);

                }

            }
            return false;

        }

        public bool AsentarConsignacionCaja(List<Movimiento> movimientos, int ConsecutivoMov, string comprobanteIngreso)
        {
            using (var ctx = new AccountingContext())
            {
                var ano = DateTime.Now.Year;
                var mes = DateTime.Now.Month;
                var dia = DateTime.Now.Day;

                //pbtengo la cuenta de SCuentas y actualizo
                foreach (var item in movimientos)
                {
                    var auxiliar = ctx.PlanCuentas.Find(item.CUENTA);
                    if (null == auxiliar)
                    {
                        //no he creado el auxiliar -esto no deberia pasar NUNCA!!!
                        return false;
                        throw new InvalidOperationException("No existe el auxiliar");
                    }

                    decimal credito = item.CREDITO;
                    decimal debito = item.DEBITO;

                    //actualiza saldos en SaldosTerceros
                    //actualiza saldos en SaldosCcostos
                    //actualiza saldos en SaldosCuentas
                    //////////////////////////////////////
                    #region SaldosCuentas
                    //al crear el auxiliar se crea con mes y ano en 0s
                    //verifico si existe el auxiliar no importa de que mes
                    //si no existe el registro para el mes lo creo de lo contrario opero
                    //obtengo la naturaleza
                    var naturaleza = auxiliar.NATURALEZA;

                    var saldoAuxiliar = ctx.SaldosCuentas
                     .Where(st => st.CODIGO == item.CUENTA
                     && st.MES == mes && st.ANO == ano)
                     .FirstOrDefault();

                    //si no existe para ese mes 
                    if (saldoAuxiliar == null)
                    {
                        saldoAuxiliar = new DTO.Contabilidad.SaldoCuenta()
                        {
                            CODIGO = item.CUENTA,
                            // Auxiliar = item.CUENTA,
                            ANO = ano,
                            MES = mes,
                            MDEBITO = debito,
                            MCREDITO = credito
                            //  SALDO = debito - credito,
                            //  NAT = naturaleza
                        };
#warning Revisar
                        //    saldoAuxiliar.Auxiliar = ctx.PlanCuentas.Find(item.CUENTA) as CuentaAuxiliar;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoAuxiliar.MCREDITO += credito;
                        saldoAuxiliar.MDEBITO += debito;
                        saldoAuxiliar.ANO = ano;
                        saldoAuxiliar.MES = mes;
                        // saldoAuxiliar.SALDO += debito - credito;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Modified;
                    }
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoAuxiliar.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoAuxiliar.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////
                    #region SaldosTerceros
                    //si no  existe en SaldosTerceros lo debo crear
                    //item requiere tercero?
                    var saldoTercero = ctx.SaldosTerceros
                        .Where(st => st.TERCERO == item.TERCERO
                        && st.MES == mes && st.ANO == ano && st.CODIGO == item.CUENTA)
                        .FirstOrDefault();

                    if (null == saldoTercero)
                    {
                        saldoTercero = new DTO.Contabilidad.SaldosTercero()
                        {
                            MDEBITO = item.DEBITO,
                            MCREDITO = item.CREDITO,
                        };
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoTercero.MDEBITO += debito;
                        saldoTercero.MCREDITO += credito;
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                    }
                    saldoTercero.CODIGO = item.CUENTA;
                    saldoTercero.TERCERO = item.TERCERO;
                    saldoTercero.MES = mes;
                    saldoTercero.ANO = ano;
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoTercero.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoTercero.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////////////////////////////
                    #region SaldosCCs
                    //obtengo el registro en SaldosCCs si no existe lo creo excepto si no viene centro de costo
                    if (!item.CCOSTO.Equals(""))
                    {

                        var saldoCC = ctx.SaldosCCs
                            .Where(st => st.CCOSTO == item.CCOSTO
                            && st.MES == mes && st.ANO == ano && st.CUENTA == item.CUENTA)
                            .FirstOrDefault();

                        if (null == saldoCC)
                        {
                            saldoCC = new DTO.Contabilidad.SaldoCC()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO
                            };
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Added;

                            saldoCC.CCOSTO = item.CCOSTO;
                            saldoCC.CUENTA = item.CUENTA;
                            saldoCC.ANO = ano;
                            saldoCC.MES = mes;
                            saldoCC.TERCERO = item.TERCERO;
                        }

                        else
                        {
                            saldoCC.MDEBITO += debito;
                            saldoCC.MCREDITO += credito;
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Modified;
                        }
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoCC.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoCC.SALDO += credito - debito;
                        }
                        #endregion
                    }

                }
                ctx.Movimientos.AddRange(movimientos); //entradas

                try
                {
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        //si no  hubo problmeas actulizo consecutivois y saldos
                        //actualizo consecutivo -- esto debe ir en caso de success
                        var _tipoComprobante = ctx.TiposComprobantes.Find(comprobanteIngreso);

                        _tipoComprobante.CONSECUTIVO = Convert.ToString(ConsecutivoMov + 1);

                        ctx.Entry(_tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                        int rowsActualizaSaldos = ctx.SaveChanges();
                        //Actualizo los saldos
                        #region SaldoGeneralEnPlanDecuentas

                        this.Mayorizar(ctx, movimientos);
                        ctx.SaveChanges();
                        //agrupar del auxiliar hasta la subcuenta
                        //de la subcuenta hasta la cuenta
                        //de la cuenta al auxiliar
                        ///---el valor que traslado es el saldo del auxiliar = saldoAuxiliar.SALDO
                        //var saldoATransladar = saldoAuxiliar.SALDO;

                        //estrategia
                        //obtengo el saldo actualizado de los auxiliares y lanzo la actualizacion hacia arriba y la mando async parta que retone
                        //solo actualizo los que modifique --


                        //debo obtener
                        #endregion

                        //finalmente devuelvo true --? sino actualizo saldo?
                        return true;

                    }
                }
                catch (DbEntityValidationException dbE)
                {
                    Console.WriteLine(dbE.Message);

                }

            }
            return false;

        }

        public bool AsentarMovimiento(List<Movimiento> movimientos, int ConsecutivoMov, string comprobanteIngreso)
        {
            using (var ctx = new AccountingContext())
            {
                var ano = DateTime.Now.Year;
                var mes = DateTime.Now.Month;
                var dia = DateTime.Now.Day;

                //pbtengo la cuenta de SCuentas y actualizo
                foreach (var item in movimientos)
                {
                    var auxiliar = ctx.PlanCuentas.Find(item.CUENTA);
                    if (null == auxiliar)
                    {
                        //no he creado el auxiliar -esto no deberia pasar NUNCA!!!
                        return false;
                        throw new InvalidOperationException("No existe el auxiliar");
                    }

                    decimal credito = item.CREDITO;
                    decimal debito = item.DEBITO;

                    //actualiza saldos en SaldosTerceros
                    //actualiza saldos en SaldosCcostos
                    //actualiza saldos en SaldosCuentas
                    //////////////////////////////////////
                    #region SaldosCuentas
                    //al crear el auxiliar se crea con mes y ano en 0s
                    //verifico si existe el auxiliar no importa de que mes
                    //si no existe el registro para el mes lo creo de lo contrario opero
                    //obtengo la naturaleza
                    var naturaleza = auxiliar.NATURALEZA;

                    var saldoAuxiliar = ctx.SaldosCuentas
                     .Where(st => st.CODIGO == item.CUENTA
                     && st.MES == mes && st.ANO == ano)
                     .FirstOrDefault();

                    //si no existe para ese mes 
                    if (saldoAuxiliar == null)
                    {
                        saldoAuxiliar = new DTO.Contabilidad.SaldoCuenta()
                        {
                            CODIGO = item.CUENTA,
                            // Auxiliar = item.CUENTA,
                            ANO = ano,
                            MES = mes,
                            MDEBITO = debito,
                            MCREDITO = credito
                            //  SALDO = debito - credito,
                            //  NAT = naturaleza
                        };
#warning Revisar
                        //    saldoAuxiliar.Auxiliar = ctx.PlanCuentas.Find(item.CUENTA) as CuentaAuxiliar;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoAuxiliar.MCREDITO += credito;
                        saldoAuxiliar.MDEBITO += debito;
                        saldoAuxiliar.ANO = ano;
                        saldoAuxiliar.MES = mes;
                        // saldoAuxiliar.SALDO += debito - credito;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Modified;
                    }
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoAuxiliar.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoAuxiliar.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////
                    #region SaldosTerceros
                    //si no  existe en SaldosTerceros lo debo crear
                    //item requiere tercero?
                    var saldoTercero = ctx.SaldosTerceros
                        .Where(st => st.TERCERO == item.TERCERO
                        && st.MES == mes && st.ANO == ano && st.CODIGO == item.CUENTA)
                        .FirstOrDefault();

                    if (null == saldoTercero)
                    {
                        saldoTercero = new DTO.Contabilidad.SaldosTercero()
                        {
                            MDEBITO = item.DEBITO,
                            MCREDITO = item.CREDITO,
                        };
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoTercero.MDEBITO += debito;
                        saldoTercero.MCREDITO += credito;
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                    }
                    saldoTercero.CODIGO = item.CUENTA;
                    saldoTercero.TERCERO = item.TERCERO;
                    saldoTercero.MES = mes;
                    saldoTercero.ANO = ano;
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoTercero.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoTercero.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////////////////////////////
                    #region SaldosCCs
                    //obtengo el registro en SaldosCCs si no existe lo creo excepto si no viene centro de costo
                    if (!item.CCOSTO.Equals(""))
                    {

                        var saldoCC = ctx.SaldosCCs
                            .Where(st => st.CCOSTO == item.CCOSTO
                            && st.MES == mes && st.ANO == ano && st.CUENTA == item.CUENTA)
                            .FirstOrDefault();

                        if (null == saldoCC)
                        {
                            saldoCC = new DTO.Contabilidad.SaldoCC()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO
                            };
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Added;

                            saldoCC.CCOSTO = item.CCOSTO;
                            saldoCC.CUENTA = item.CUENTA;
                            saldoCC.ANO = ano;
                            saldoCC.MES = mes;
                            saldoCC.TERCERO = item.TERCERO;
                        }

                        else
                        {
                            saldoCC.MDEBITO += debito;
                            saldoCC.MCREDITO += credito;
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Modified;
                        }
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoCC.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoCC.SALDO += credito - debito;
                        }
                        #endregion
                    }

                }
                ctx.Movimientos.AddRange(movimientos); //entradas

                try
                {
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        //si no  hubo problmeas actulizo consecutivois y saldos
                        //actualizo consecutivo -- esto debe ir en caso de success
                        var _tipoComprobante = ctx.TiposComprobantes.Find(comprobanteIngreso);

                        _tipoComprobante.CONSECUTIVO = Convert.ToString(ConsecutivoMov + 1);

                        ctx.Entry(_tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                        int rowsActualizaSaldos = ctx.SaveChanges();
                        //Actualizo los saldos
                        #region SaldoGeneralEnPlanDecuentas

                        this.Mayorizar(ctx, movimientos);
                        ctx.SaveChanges();
                        //agrupar del auxiliar hasta la subcuenta
                        //de la subcuenta hasta la cuenta
                        //de la cuenta al auxiliar
                        ///---el valor que traslado es el saldo del auxiliar = saldoAuxiliar.SALDO
                        //var saldoATransladar = saldoAuxiliar.SALDO;

                        //estrategia
                        //obtengo el saldo actualizado de los auxiliares y lanzo la actualizacion hacia arriba y la mando async parta que retone
                        //solo actualizo los que modifique --


                        //debo obtener
                        #endregion

                        //finalmente devuelvo true --? sino actualizo saldo?
                        return true;

                    }
                }
                catch (DbEntityValidationException dbE)
                {
                    Console.WriteLine(dbE.Message);

                }

            }
            return false;

        }

        public bool AsentarCausacion(List<Movimiento> movimientos, int ConsecutivoMov, string comprobanteIngreso)
        {
            using (var ctx = new AccountingContext())
            {
                var ano = DateTime.Now.Year;
                var mes = DateTime.Now.Month;
                var dia = DateTime.Now.Day;

                //pbtengo la cuenta de SCuentas y actualizo
                foreach (var item in movimientos)
                {
                    var auxiliar = ctx.PlanCuentas.Find(item.CUENTA);
                    if (null == auxiliar)
                    {
                        //no he creado el auxiliar -esto no deberia pasar NUNCA!!!
                        return false;
                        throw new InvalidOperationException("No existe el auxiliar");
                    }

                    decimal credito = item.CREDITO;
                    decimal debito = item.DEBITO;

                    //actualiza saldos en SaldosTerceros
                    //actualiza saldos en SaldosCcostos
                    //actualiza saldos en SaldosCuentas
                    //////////////////////////////////////
                    #region SaldosCuentas
                    //al crear el auxiliar se crea con mes y ano en 0s
                    //verifico si existe el auxiliar no importa de que mes
                    //si no existe el registro para el mes lo creo de lo contrario opero
                    //obtengo la naturaleza
                    var naturaleza = auxiliar.NATURALEZA;

                    var saldoAuxiliar = ctx.SaldosCuentas
                     .Where(st => st.CODIGO == item.CUENTA
                     && st.MES == mes && st.ANO == ano)
                     .FirstOrDefault();

                    //si no existe para ese mes 
                    if (saldoAuxiliar == null)
                    {
                        saldoAuxiliar = new DTO.Contabilidad.SaldoCuenta()
                        {
                            CODIGO = item.CUENTA,
                            // Auxiliar = item.CUENTA,
                            ANO = ano,
                            MES = mes,
                            MDEBITO = debito,
                            MCREDITO = credito
                            //  SALDO = debito - credito,
                            //  NAT = naturaleza
                        };
#warning Revisar
                        //    saldoAuxiliar.Auxiliar = ctx.PlanCuentas.Find(item.CUENTA) as CuentaAuxiliar;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoAuxiliar.MCREDITO += credito;
                        saldoAuxiliar.MDEBITO += debito;
                        saldoAuxiliar.ANO = ano;
                        saldoAuxiliar.MES = mes;
                        // saldoAuxiliar.SALDO += debito - credito;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Modified;
                    }
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoAuxiliar.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoAuxiliar.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////
                    #region SaldosTerceros
                    //si no  existe en SaldosTerceros lo debo crear
                    //item requiere tercero?
                    var saldoTercero = ctx.SaldosTerceros
                        .Where(st => st.TERCERO == item.TERCERO
                        && st.MES == mes && st.ANO == ano && st.CODIGO == item.CUENTA)
                        .FirstOrDefault();

                    if (null == saldoTercero)
                    {
                        saldoTercero = new DTO.Contabilidad.SaldosTercero()
                        {
                            MDEBITO = item.DEBITO,
                            MCREDITO = item.CREDITO,
                        };
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoTercero.MDEBITO += debito;
                        saldoTercero.MCREDITO += credito;
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                    }
                    saldoTercero.CODIGO = item.CUENTA;
                    saldoTercero.TERCERO = item.TERCERO;
                    saldoTercero.MES = mes;
                    saldoTercero.ANO = ano;
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoTercero.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoTercero.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////////////////////////////
                    #region SaldosCCs
                    //obtengo el registro en SaldosCCs si no existe lo creo excepto si no viene centro de costo
                    if (!item.CCOSTO.Equals(""))
                    {

                        var saldoCC = ctx.SaldosCCs
                            .Where(st => st.CCOSTO == item.CCOSTO
                            && st.MES == mes && st.ANO == ano && st.CUENTA == item.CUENTA)
                            .FirstOrDefault();

                        if (null == saldoCC)
                        {
                            saldoCC = new DTO.Contabilidad.SaldoCC()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO
                            };
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Added;

                            saldoCC.CCOSTO = item.CCOSTO;
                            saldoCC.CUENTA = item.CUENTA;
                            saldoCC.ANO = ano;
                            saldoCC.MES = mes;
                            saldoCC.TERCERO = item.TERCERO;
                        }

                        else
                        {
                            saldoCC.MDEBITO += debito;
                            saldoCC.MCREDITO += credito;
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Modified;
                        }
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoCC.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoCC.SALDO += credito - debito;
                        }
                        #endregion
                    }

                }
                ctx.Movimientos.AddRange(movimientos); //entradas

                try
                {
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        //si no  hubo problmeas actulizo consecutivois y saldos
                        //actualizo consecutivo -- esto debe ir en caso de success
                        var _tipoComprobante = ctx.TiposComprobantes.Find(comprobanteIngreso);

                        _tipoComprobante.CONSECUTIVO = Convert.ToString(ConsecutivoMov + 1);

                        ctx.Entry(_tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                        int rowsActualizaSaldos = ctx.SaveChanges();
                        //Actualizo los saldos
                        #region SaldoGeneralEnPlanDecuentas

                        this.Mayorizar(ctx, movimientos);
                        ctx.SaveChanges();

                        var ComprobanteUsar = ctx.TiposComprobantes.Find("CI3");

                        //agrupar del auxiliar hasta la subcuenta
                        //de la subcuenta hasta la cuenta
                        //de la cuenta al auxiliar
                        ///---el valor que traslado es el saldo del auxiliar = saldoAuxiliar.SALDO
                        //var saldoATransladar = saldoAuxiliar.SALDO;

                        //estrategia
                        //obtengo el saldo actualizado de los auxiliares y lanzo la actualizacion hacia arriba y la mando async parta que retone
                        //solo actualizo los que modifique --


                        //debo obtener
                        #endregion

                        //finalmente devuelvo true --? sino actualizo saldo?
                        return true;

                    }
                }
                catch (DbEntityValidationException dbE)
                {
                    Console.WriteLine(dbE.Message);

                }

            }
            return false;

        }

        public string proximoConsecutivo(string codigoComprobante)
        {
            string consec = "";
            using (var ctx = new AccountingContext())
            {
                var _tipoComprobante = ctx.TiposComprobantes.Find(codigoComprobante);
                consec = _tipoComprobante.CONSECUTIVO;
            }
            return consec;
        }

        public bool AsentarCierreCaja(List<Movimiento> movimientos, int ConsecutivoMov, string comprobanteIngreso)
        {
            using (var ctx = new AccountingContext())
            {
                var ano = DateTime.Now.Year;
                var mes = DateTime.Now.Month;
                var dia = DateTime.Now.Day;

                //pbtengo la cuenta de SCuentas y actualizo
                foreach (var item in movimientos)
                {
                    var auxiliar = ctx.PlanCuentas.Find(item.CUENTA);
                    if (null == auxiliar)
                    {
                        //no he creado el auxiliar -esto no deberia pasar NUNCA!!!
                        return false;
                        throw new InvalidOperationException("No existe el auxiliar");
                    }

                    decimal credito = item.CREDITO;
                    decimal debito = item.DEBITO;

                    //actualiza saldos en SaldosTerceros
                    //actualiza saldos en SaldosCcostos
                    //actualiza saldos en SaldosCuentas
                    //////////////////////////////////////
                    #region SaldosCuentas
                    //al crear el auxiliar se crea con mes y ano en 0s
                    //verifico si existe el auxiliar no importa de que mes
                    //si no existe el registro para el mes lo creo de lo contrario opero
                    //obtengo la naturaleza
                    var naturaleza = auxiliar.NATURALEZA;

                    var saldoAuxiliar = ctx.SaldosCuentas
                     .Where(st => st.CODIGO == item.CUENTA
                     && st.MES == mes && st.ANO == ano)
                     .FirstOrDefault();

                    //si no existe para ese mes 
                    if (saldoAuxiliar == null)
                    {
                        saldoAuxiliar = new DTO.Contabilidad.SaldoCuenta()
                        {
                            CODIGO = item.CUENTA,
                            // Auxiliar = item.CUENTA,
                            ANO = ano,
                            MES = mes,
                            MDEBITO = debito,
                            MCREDITO = credito
                            //  SALDO = debito - credito,
                            //  NAT = naturaleza
                        };
#warning Revisar
                        //    saldoAuxiliar.Auxiliar = ctx.PlanCuentas.Find(item.CUENTA) as CuentaAuxiliar;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoAuxiliar.MCREDITO += credito;
                        saldoAuxiliar.MDEBITO += debito;
                        saldoAuxiliar.ANO = ano;
                        saldoAuxiliar.MES = mes;
                        // saldoAuxiliar.SALDO += debito - credito;

                        ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Modified;
                    }
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoAuxiliar.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoAuxiliar.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////
                    #region SaldosTerceros
                    //si no  existe en SaldosTerceros lo debo crear
                    //item requiere tercero?
                    var saldoTercero = ctx.SaldosTerceros
                        .Where(st => st.TERCERO == item.TERCERO
                        && st.MES == mes && st.ANO == ano && st.CODIGO == item.CUENTA)
                        .FirstOrDefault();

                    if (null == saldoTercero)
                    {
                        saldoTercero = new DTO.Contabilidad.SaldosTercero()
                        {
                            MDEBITO = item.DEBITO,
                            MCREDITO = item.CREDITO,
                        };
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        saldoTercero.MDEBITO += debito;
                        saldoTercero.MCREDITO += credito;
                        ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                    }
                    saldoTercero.CODIGO = item.CUENTA;
                    saldoTercero.TERCERO = item.TERCERO;
                    saldoTercero.MES = mes;
                    saldoTercero.ANO = ano;
                    if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                    {
                        saldoTercero.SALDO += debito - credito;
                    }
                    else
                    {
                        saldoTercero.SALDO += credito - debito;
                    }

                    #endregion
                    /////////////////////////////////////////////////////////////
                    #region SaldosCCs
                    //obtengo el registro en SaldosCCs si no existe lo creo excepto si no viene centro de costo
                    if (!item.CCOSTO.Equals(""))
                    {

                        var saldoCC = ctx.SaldosCCs
                            .Where(st => st.CCOSTO == item.CCOSTO
                            && st.MES == mes && st.ANO == ano && st.CUENTA == item.CUENTA)
                            .FirstOrDefault();

                        if (null == saldoCC)
                        {
                            saldoCC = new DTO.Contabilidad.SaldoCC()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO
                            };
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Added;

                            saldoCC.CCOSTO = item.CCOSTO;
                            saldoCC.CUENTA = item.CUENTA;
                            saldoCC.ANO = ano;
                            saldoCC.MES = mes;
                            saldoCC.TERCERO = item.TERCERO;
                        }

                        else
                        {
                            saldoCC.MDEBITO += debito;
                            saldoCC.MCREDITO += credito;
                            ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Modified;
                        }
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoCC.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoCC.SALDO += credito - debito;
                        }
                        #endregion
                    }

                }
                ctx.Movimientos.AddRange(movimientos); //entradas

                try
                {
                    int rowsAffected = ctx.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        //si no  hubo problmeas actulizo consecutivois y saldos
                        //actualizo consecutivo -- esto debe ir en caso de success
                        var _tipoComprobante = ctx.TiposComprobantes.Find(comprobanteIngreso);

                        _tipoComprobante.CONSECUTIVO = Convert.ToString(ConsecutivoMov + 1);

                        ctx.Entry(_tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                        int rowsActualizaSaldos = ctx.SaveChanges();
                        //Actualizo los saldos
                        #region SaldoGeneralEnPlanDecuentas

                        this.Mayorizar(ctx, movimientos);
                        ctx.SaveChanges();
                        //agrupar del auxiliar hasta la subcuenta
                        //de la subcuenta hasta la cuenta
                        //de la cuenta al auxiliar
                        ///---el valor que traslado es el saldo del auxiliar = saldoAuxiliar.SALDO
                        //var saldoATransladar = saldoAuxiliar.SALDO;

                        //estrategia
                        //obtengo el saldo actualizado de los auxiliares y lanzo la actualizacion hacia arriba y la mando async parta que retone
                        //solo actualizo los que modifique --


                        //debo obtener
                        #endregion

                        //finalmente devuelvo true --? sino actualizo saldo?
                        return true;

                    }
                }
                catch (DbEntityValidationException dbE)
                {
                    Console.WriteLine(dbE.Message);

                }

            }
            return false;

        }

        public bool Editar()
        {
            using (var ctx = new AccountingContext())
            {
                /*Construyto el commprobante*/
                if (!this.Verify())
                {
                    // throw new InvalidOperationException("El comprobante no se puede asentar mientras contenga errores");
                    return false;
                }
                else //no contiene errores-continuo
                {
                    /*
                    //verifico  que no exista con el mismo consecutivo
                    if (!this.IsNew)//si no es nuevo 
                    {

                        return false;
                    }

                    var consecutivo = _comprobante.NUMERO;
                    var existe = ctx.Comprobantes.Where(x => x.NUMERO == consecutivo && x.TIPO.Equals(_comprobante.TIPO)).FirstOrDefault() == null ? false : true;
                    if (existe)
                    {
                        //no se puede asentar como nuevo , xk ya existe
                        return false;

                    }
                    */
                    var ano = _comprobante.FECHARealiz.Year;
                    var mes = _comprobante.FECHARealiz.Month;
                    var dia = _comprobante.FECHARealiz.Day;

                    //creo el comrpobanmte 
                    _comprobante.SUMDBCR = Entries.Balance;
                    _comprobante.VRTOTAL = Entries.Total;
                    _comprobante.ANO = ano.ToString();
                    _comprobante.MES = mes.ToString();
                    _comprobante.DIA = dia.ToString();
                    _comprobante.FPAGO = this.FPago;
                    var primerIndex = Entries.First();
                    _comprobante.CTAFPAGO = Entries.First(e => e.Index == primerIndex.Index).Cuenta;
                    _comprobante.NUMEXTERNO = this.NumeroExterno;
                    _comprobante.CCOSTO = Entries.First(e => e.Index == primerIndex.Index).CentroDeCosto;
                    _comprobante.TERCERO = Entries.First(e => e.Index == primerIndex.Index).Tercero;
                    _comprobante.FECHARealiz = DateTime.Now; //?? fecha contabler y fecha de asiento

                    //agrego los movimientos
                    //obtengo las sumas de credito 

                    var LastRegister = (from pc in ctx.ComprobantesEditadosAC where pc.NUMERO == _comprobante.NUMERO && pc.TIPO == _comprobante.TIPO select pc).Count();

                    string consecutivoNumeroEditado = "1";
                    if (LastRegister == 0)
                    {
                        consecutivoNumeroEditado = "1";
                    }
                    else
                    {
                        var suma = LastRegister + 1;
                        consecutivoNumeroEditado = suma.ToString();
                    }
                    var ComprobanteAnterior = (from pc in ctx.Comprobantes where pc.NUMERO == _comprobante.NUMERO && pc.TIPO == _comprobante.TIPO select pc).Single();

                    var NewComprobante = new ComprobantesEditados()
                    {
                        TIPO = ComprobanteAnterior.TIPO,
                        NUMERO = ComprobanteAnterior.NUMERO,
                        ANO = ComprobanteAnterior.ANO,
                        MES = ComprobanteAnterior.MES,
                        DIA = ComprobanteAnterior.DIA,
                        CCOSTO = ComprobanteAnterior.CCOSTO,
                        ELIMINADO = ComprobanteAnterior.ELIMINADO,
                        DETALLE = ComprobanteAnterior.DETALLE,
                        TERCERO = ComprobanteAnterior.TERCERO,
                        FPAGO = ComprobanteAnterior.FPAGO,
                        CTAFPAGO = ComprobanteAnterior.CTAFPAGO,
                        NUMEXTERNO = ComprobanteAnterior.NUMEXTERNO,
                        VRTOTAL = ComprobanteAnterior.VRTOTAL,
                        SUMDBCR = ComprobanteAnterior.SUMDBCR,
                        FECHARealiz = ComprobanteAnterior.FECHARealiz,
                        MODIFICA = ComprobanteAnterior.MODIFICA,
                        EXPORTADO = ComprobanteAnterior.EXPORTADO,
                        MARCASEG = ComprobanteAnterior.MARCASEG,
                        BLOQUEADO = ComprobanteAnterior.BLOQUEADO,
                        NUMIMP = ComprobanteAnterior.NUMIMP,
                        PC = ComprobanteAnterior.PC,
                        USUARIO = ComprobanteAnterior.USUARIO,
                        ANULADO = ComprobanteAnterior.ANULADO,
                        FECHAMODIFICACION = DateTime.Now,
                        NUMEROEDITADO = consecutivoNumeroEditado
                    };

                    ctx.ComprobantesEditadosAC.Add(NewComprobante);
                    var ComprobanteAEliminar = (from pc in ctx.Comprobantes where pc.NUMERO == _comprobante.NUMERO && pc.TIPO == _comprobante.TIPO select pc).Single();


                    var entries = Entries.Select(x => x.GetMovimiento()).ToList();

                    List<Movimiento> MovimientosAnteriores = (from pc in ctx.Movimientos where pc.NUMERO == _comprobante.NUMERO && pc.TIPO == _comprobante.TIPO select pc).ToList();
                    List<MovimientosEditados> MovimientosEditar = new List<MovimientosEditados>();
                    foreach (var mov in MovimientosAnteriores)
                    {
                        var NewMovimiento = new MovimientosEditados()
                        {
                            TIPO = mov.TIPO,
                            NUMERO = mov.NUMERO,
                            CUENTA = mov.CUENTA,
                            TERCERO = mov.TERCERO,
                            DETALLE = mov.DETALLE,
                            DEBITO = mov.DEBITO,
                            CREDITO = mov.CREDITO,
                            BASE = mov.BASE,
                            CCOSTO = mov.CCOSTO,
                            FECHAMOVIMIENTO = mov.FECHAMOVIMIENTO,
                            DOCUMENTO = mov.DOCUMENTO,
                            NUMEROEDITADO = consecutivoNumeroEditado
                        };
                        MovimientosEditar.Add(NewMovimiento);

                    }

                    ctx.MovimientosEditadosAC.AddRange(MovimientosEditar);
                    foreach (var mov2 in MovimientosAnteriores)
                    {
                        ctx.Movimientos.Remove(mov2);
                    }

                    ctx.Comprobantes.Remove(ComprobanteAEliminar);
                    ctx.SaveChanges();



                    //pbtengo la cuenta de SCuentas y actualizo
                    foreach (var item in entries)
                    {
                        var auxiliar = ctx.PlanCuentas.Find(item.CUENTA);
                        if (null == auxiliar)
                        {
                            //no he creado el auxiliar -esto no deberia pasar NUNCA!!!
                            return false;
                            throw new InvalidOperationException("No existe el auxiliar");
                        }

                        //generales
                        item.TIPO = _comprobante.TIPO;
                        item.NUMERO = _comprobante.NUMERO;
                        //item.FECHAMOVIMIENTO = DateTime.Now;
                        item.FECHAMOVIMIENTO = new DateTime(ano, mes, dia);

                        decimal credito = item.CREDITO;
                        decimal debito = item.DEBITO;


                        //actualiza saldos en SaldosTerceros
                        //actualiza saldos en SaldosCcostos
                        //actualiza saldos en SaldosCuentas
                        //////////////////////////////////////
                        #region SaldosCuentas
                        //al crear el auxiliar se crea con mes y ano en 0s
                        //verifico si existe el auxiliar no importa de que mes
                        //si no existe el registro para el mes lo creo de lo contrario opero
                        //obtengo la naturaleza
                        var naturaleza = auxiliar.NATURALEZA;

                        var saldoAuxiliar = ctx.SaldosCuentas
                         .Where(st => st.CODIGO == item.CUENTA
                         && st.MES == mes && st.ANO == ano)
                         .FirstOrDefault();

                        //si no existe para ese mes 
                        if (saldoAuxiliar == null)
                        {
                            saldoAuxiliar = new DTO.Contabilidad.SaldoCuenta()
                            {
                                CODIGO = item.CUENTA,
                                // Auxiliar = item.CUENTA,
                                ANO = ano,
                                MES = mes,
                                MDEBITO = debito,
                                MCREDITO = credito
                                //  SALDO = debito - credito,
                                //  NAT = naturaleza
                            };
#warning Revisar
                            //    saldoAuxiliar.Auxiliar = ctx.PlanCuentas.Find(item.CUENTA) as CuentaAuxiliar;

                            ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            saldoAuxiliar.MCREDITO += credito;
                            saldoAuxiliar.MDEBITO += debito;
                            saldoAuxiliar.ANO = ano;
                            saldoAuxiliar.MES = mes;
                            // saldoAuxiliar.SALDO += debito - credito;

                            ctx.Entry(saldoAuxiliar).State = System.Data.Entity.EntityState.Modified;
                        }
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoAuxiliar.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoAuxiliar.SALDO += credito - debito;
                        }

                        #endregion
                        /////////////////////////////////////
                        #region SaldosTerceros
                        //si no  existe en SaldosTerceros lo debo crear
                        //item requiere tercero?
                        var saldoTercero = ctx.SaldosTerceros
                            .Where(st => st.TERCERO == item.TERCERO
                            && st.MES == mes && st.ANO == ano && st.CODIGO == item.CUENTA)
                            .FirstOrDefault();

                        if (null == saldoTercero)
                        {
                            saldoTercero = new DTO.Contabilidad.SaldosTercero()
                            {
                                MDEBITO = item.DEBITO,
                                MCREDITO = item.CREDITO,
                            };
                            ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            saldoTercero.MDEBITO += debito;
                            saldoTercero.MCREDITO += credito;
                            ctx.Entry(saldoTercero).State = System.Data.Entity.EntityState.Modified;
                        }
                        saldoTercero.CODIGO = item.CUENTA;
                        saldoTercero.TERCERO = item.TERCERO;
                        saldoTercero.MES = mes;
                        saldoTercero.ANO = ano;
                        if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                        {
                            saldoTercero.SALDO += debito - credito;
                        }
                        else
                        {
                            saldoTercero.SALDO += credito - debito;
                        }

                        #endregion
                        /////////////////////////////////////////////////////////////
                        #region SaldosCCs
                        //obtengo el registro en SaldosCCs si no existe lo creo excepto si no viene centro de costo
                        if (!item.CCOSTO.Equals(""))
                        {

                            var saldoCC = ctx.SaldosCCs
                                .Where(st => st.CCOSTO == item.CCOSTO
                                && st.MES == mes && st.ANO == ano && st.CUENTA == item.CUENTA)
                                .FirstOrDefault();

                            if (null == saldoCC)
                            {
                                saldoCC = new DTO.Contabilidad.SaldoCC()
                                {
                                    MDEBITO = item.DEBITO,
                                    MCREDITO = item.CREDITO
                                };
                                ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Added;

                                saldoCC.CCOSTO = item.CCOSTO;
                                saldoCC.CUENTA = item.CUENTA;
                                saldoCC.ANO = ano;
                                saldoCC.MES = mes;
                                saldoCC.TERCERO = item.TERCERO;
                            }

                            else
                            {
                                saldoCC.MDEBITO += debito;
                                saldoCC.MCREDITO += credito;
                                ctx.Entry(saldoCC).State = System.Data.Entity.EntityState.Modified;
                            }
                            if (naturaleza == DTO.Contabilidad.NaturalezaCuenta.Debito)
                            {
                                saldoCC.SALDO += debito - credito;
                            }
                            else
                            {
                                saldoCC.SALDO += credito - debito;
                            }
                            #endregion
                        }

                        //////////////////////////////////////////////
                        var configuracion = new BLLAportes().ObtenerConfiguracion();
                        int CuentaInt = Int32.Parse(item.CUENTA);
                        if (CuentaInt == configuracion.idCuenta)
                        {
                            var fichamod = ctx.FichasAportes.Where(st => st.idPersona == item.TERCERO).FirstOrDefault();
                            var totalaporte = Int32.Parse(fichamod.totalAportes);
                            if (item.DEBITO == 0)
                            {
                                int aporte = Decimal.ToInt32(item.CREDITO);
                                int suma = totalaporte + aporte;
                                string myString = suma.ToString();
                                fichamod.totalAportes = myString;
                            }
                            else if (item.CREDITO == 0)
                            {
                                int aporte = Decimal.ToInt32(item.DEBITO);
                                if (totalaporte < aporte)
                                {
                                    return false;
                                }
                                else
                                {
                                    int suma = totalaporte - aporte;
                                    string myString = suma.ToString();
                                    fichamod.totalAportes = myString;
                                }
                            }

                            ctx.Entry(fichamod).State = System.Data.Entity.EntityState.Modified;

                        }

                    }

                    ctx.Movimientos.AddRange(entries); //entradas
                    ctx.Comprobantes.Add(_comprobante); //comprobante

                    try
                    {
                        int rowsAffected = ctx.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            //si no  hubo problmeas actulizo consecutivois y saldos
                            //actualizo consecutivo -- esto debe ir en caso de success
                            var _tipoComprobante = ctx.TiposComprobantes.Find(_comprobante.TIPO);

                            _tipoComprobante.CONSECUTIVO = _comprobante.NUMERO;

                            ctx.Entry(_tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                            int rowsActualizaSaldos = ctx.SaveChanges();
                            //Actualizo los saldos
                            #region SaldoGeneralEnPlanDecuentas

                            this.Mayorizar(ctx, entries);
                            ctx.SaveChanges();
                            //agrupar del auxiliar hasta la subcuenta
                            //de la subcuenta hasta la cuenta
                            //de la cuenta al auxiliar
                            ///---el valor que traslado es el saldo del auxiliar = saldoAuxiliar.SALDO
                            //var saldoATransladar = saldoAuxiliar.SALDO;

                            //estrategia
                            //obtengo el saldo actualizado de los auxiliares y lanzo la actualizacion hacia arriba y la mando async parta que retone
                            //solo actualizo los que modifique --


                            //debo obtener
                            #endregion

                            //finalmente devuelvo true --? sino actualizo saldo?
                            return true;

                        }
                    }
                    catch (DbEntityValidationException dbE)
                    {
                        Console.WriteLine(dbE.Message);

                    }

                }
                return false;
            }
        }

        private void ResetSaldos(AccountingContext ctx)
        {
            var conSaldos = ctx.PlanCuentas.Where(x => x.Saldo != 0);
            foreach (var item in conSaldos)
            {
                item.Saldo = 0;
                ctx.Entry(item).State = EntityState.Modified;
            }

            int rows = ctx.SaveChanges();

        }
        #endregion

        #region MAYORIZAR

        private void Mayorizar(AccountingContext ctx, List<Movimiento> movimientos)
        {
            try
            {
                Console.WriteLine("Inicio MAyorizar");
                Stopwatch sp = new Stopwatch();
                sp.Start();
                //this.ResetSaldos(ctx);//NO NECESITO REINICIAR LOS SALDO
                /*
                MayorizarAuxiliares(ctx);
                this.MayorizarASubcuentas(ctx);
                this.MayorizarACuentas(ctx);
                this.MayorizaAGrupo(ctx);
                this.MayorizarAClase(ctx);
                */
                //int rows = ctx.SaveChanges();

                foreach (var item in movimientos)
                {
                    var auxiliar = ctx.PlanCuentas.Find(item.CUENTA);
                    var subcuenta = ctx.PlanCuentas.Find(item.CUENTA.Substring(0, 6));
                    var cuenta = ctx.PlanCuentas.Find(item.CUENTA.Substring(0, 4));
                    var grupo = ctx.PlanCuentas.Find(item.CUENTA.Substring(0, 2));
                    var clase = ctx.PlanCuentas.Find(item.CUENTA.Substring(0, 1));

                    if (auxiliar.NATURALEZA == "D")
                    {
                        if (item.DEBITO > 0)
                        {
                            auxiliar.Saldo += item.DEBITO;
                            subcuenta.Saldo += item.DEBITO;
                            cuenta.Saldo += item.DEBITO;
                            grupo.Saldo += item.DEBITO;
                            clase.Saldo += item.DEBITO;
                        }
                        else
                        {
                            auxiliar.Saldo -= item.CREDITO;
                            subcuenta.Saldo -= item.CREDITO;
                            cuenta.Saldo -= item.CREDITO;
                            grupo.Saldo -= item.CREDITO;
                            clase.Saldo -= item.CREDITO;
                        }
                    }
                    else
                    {
                        if (item.DEBITO > 0)
                        {
                            auxiliar.Saldo -= item.DEBITO;
                            subcuenta.Saldo -= item.DEBITO;
                            cuenta.Saldo -= item.DEBITO;
                            grupo.Saldo -= item.DEBITO;
                            clase.Saldo -= item.DEBITO;
                        }
                        else
                        {
                            auxiliar.Saldo += item.CREDITO;
                            subcuenta.Saldo += item.CREDITO;
                            cuenta.Saldo += item.CREDITO;
                            grupo.Saldo += item.CREDITO;
                            clase.Saldo += item.CREDITO;
                        }
                    }
                    ctx.SaveChanges();
                }

                sp.Stop();
                Console.WriteLine(sp.Elapsed);
            }
            catch (Exception ex)
            {
                //throw new NotImplementedException();
            }

        }

        private void MayorizarAuxiliares(AccountingContext ctx)
        {
            var conSaldos = ctx.SaldosCuentas.ToList();
            foreach (var item in conSaldos)
            {
                //me traigo la cuenta desde plan de cuentas correspondiente a los auxiliares
                //Cpalacios condicional modificado
                var auxEnPlanDeCuentas = ctx.PlanCuentas.Find(item.CODIGO);
                if (auxEnPlanDeCuentas != null)
                {
                    auxEnPlanDeCuentas.Saldo += item.SALDO;
                }
                ctx.Entry(item).State = EntityState.Modified;
            }

            int rows = ctx.SaveChanges();
        }

        /// <summary>
        /// Mayorizo del Auxiliar a SubCuentas
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        /// 
        public bool MayorizarASubcuentas(AccountingContext ctx)
        {
            foreach (var item in ctx.SaldosCuentas)
            {
                //el auxiliar tiene hasta 9 digitos
                //saca loa subcuenta del auxiliar y 
                //mayorizo los auxiliares en subcuentas
                var subcuenta = item.CODIGO.Substring(0, 6);
                var sc = ctx.PlanCuentas.Find(subcuenta);
                sc.Saldo += item.SALDO;
                ctx.Entry(sc).State = EntityState.Modified;

                //debo havcer un foreach para mayorizar las subucuentas
                //mayorizo las subcuentas en cuentas
                //var cuenta = subcuenta.Substring(0, 4);
                //var c = ctx.PlanCuentas.Find(cuenta);
                //c.Saldo = item.SALDO;
                //ctx.Entry(c).State = EntityState.Modified;

                ////mayorixo clase
                var clase = subcuenta.Substring(0, 2);
                var sg = ctx.PlanCuentas.Find(clase);
                sc.Saldo = item.SALDO;
                ctx.Entry(sc).State = EntityState.Modified;

                //var grupo = subcuenta.Substring(0, 1);
                //var gl = ctx.PlanCuentas.Find(clase);
                //c.Saldo = item.SALDO;
                //ctx.Entry(gl).State = EntityState.Modified;
            }
            ctx.SaveChanges();
            return true;

            // var numeroCuentas = ctx.SaldosCuentas.Count();

            //  int rowsAffected = ctx.SaveChanges();

            //  var resultado = (rowsAffected == numeroCuentas) ? true : false;

            //   return resultado;

        }

        //de subcuentas a cuentas
        private bool MayorizarACuentas(AccountingContext ctx)
        {
#if RELEASE
#error error 1
#endif

            //tomo todas la subcuentas 
            foreach (var item in ctx.PlanCuentas.Where(x => x.CODIGO.Length == 6 && x.Saldo != 0))
            {
                //obtengo la cuenta
                var c = item.CODIGO.Substring(0, 4);
                var cuenta = ctx.PlanCuentas.Find(c);
                //mayorizo a 
                cuenta.Saldo += item.Saldo;
                ctx.Entry(cuenta).State = EntityState.Modified;
            }
            ctx.SaveChanges();

            //    int rows = ctx.SaveChanges();
            return true;
        }

        //de cuentas a grupo
        private bool MayorizaAGrupo(AccountingContext ctx)
        {
            //de todas las subcuentas
            foreach (var item in ctx.PlanCuentas.Where(x => x.CODIGO.Length == 4 && x.Saldo != 0))
            {
                //obtengo el grupo
                var grupo = item.CODIGO.Substring(0, 2);
                var cuentaGrupo = ctx.PlanCuentas.Find(grupo);
                //mayorizo a 
                cuentaGrupo.Saldo += item.Saldo;
                ctx.Entry(cuentaGrupo).State = EntityState.Modified;
            }

            ctx.SaveChanges();
            return true;
        }

        //de grupo a clase
        private bool MayorizarAClase(AccountingContext ctx)
        {
            //debug
            var debug = ctx.PlanCuentas.Where(x => x.Saldo != 0);

            //de todas las cuentas grupo 
            foreach (var item in ctx.PlanCuentas.Where(x => x.CODIGO.Length == 2 && x.Saldo != 0))
            {
                //obtengo la clase
                var clase = item.CODIGO.Substring(0, 1);
                var cuentaClase = ctx.PlanCuentas.Find(clase);
                //mayorizo a 
                cuentaClase.Saldo += item.Saldo;
                ctx.Entry(cuentaClase).State = EntityState.Modified;
            }

            ctx.SaveChanges();
            return true;

            //debo consultar desde  el dbcontext local y no desde base de datos para no llegar a incosistencias-estp àra todas las mayorizaciones
        }
        #endregion

        public bool AddEntry(Anotacion anotacion)
        {
            int count = this.Entries.Count;


            this.Entries.Add(anotacion);
            if (count > this.Entries.Count) //innecesario
                return true;
            return false;
        }

        #region TODO
        #region Actualizar
        public bool ActualizarAsiento(int comprobanteId)
        {
            throw new NotImplementedException();
        }

        public bool ActualizarAsiento(DTO.Contabilidad.Comprobante comprobante)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Anular
        public bool AnularAsiento(int comprobanteID)
        {
            throw new NotImplementedException();
        }

        public bool AnularAsiento(DTO.Contabilidad.Comprobante comprobante)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region borrar
        public bool BorrarAsiento(int comprobanteId)
        {
            using (var ctx = new AccountingContext())
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
                    var saldoCuentra = ctx.SaldosCuentas.First(x => x.CODIGO == item.CUENTA);
                    if (saldoCuentra == null)
                    {
                        throw new InvalidOperationException("No existe la cuenta  en SCuentas:" + item.CUENTA);
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
                        && st.MES == mes && st.ANO == ano)
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
                    saldoTercero.CODIGO = item.CUENTA;
                    saldoTercero.TERCERO = item.TERCERO;
                    saldoTercero.MES = mes;
                    saldoTercero.ANO = ano;
                }
            }
            throw new NotImplementedException();
        }

        public bool BorrarAsiento(DTO.Contabilidad.Comprobante comprobante)
        {
            throw new NotImplementedException();
        }
        #endregion

        public bool ActualizarSaldos()
        {
            //this.Debito = Entries.Debito;
            //this.Credito = Entries.Credito;
            //   #error error
            throw new NotImplementedException();
        }

        #endregion
        #endregion


        #region GETTERS

        public string GetConsecutivo() { return _comprobante.NUMERO; }
        private string GetClase() { return this.GetTipo().Substring(0, 2); }
        private string GetTipo() { return _comprobante.TIPO; }

        private string GetNombreComprobante()
        {
            var nombre = "";
            //esto va en el service de tiposComprobantes
            using (var ctx = new AccountingContext())
            {
                nombre = ctx.TiposComprobantes.First(x => x.CODIGO == _comprobante.TIPO).NOMBRE;
            }
            return nombre;
        }

        private DateTime GetFecha()
        {
            return _comprobante.FECHARealiz;
        }

        private string GetNumeroExterno()
        {
            return _comprobante.NUMEXTERNO;
        }
        #endregion


        #region SETTERS
        //esto es automatico pero en modo desatraso debe poder setearse
        public void SetConsecutivo(int consecutivo) { _comprobante.NUMERO = consecutivo.ToString(); }

        private void SetFechaComprobante(DateTime fechaComprobante) { _comprobante.FECHARealiz = fechaComprobante; }

        private void SetNumeroExterno(string numeroExterno) { _comprobante.NUMEXTERNO = numeroExterno.ToString(); }

        private void SetDetalle(string detalle) { _comprobante.DETALLE = detalle; } //esto no deberia llegar vacio

        private void SetConsecutivoComprobante()
        {
            var tipoComprobante = _comprobante.TIPO;
            using (var ctx = new AccountingContext())
            {
                var tipos = ctx.TiposComprobantes.ToList();
                var comprobante = ctx.TiposComprobantes.Find(tipoComprobante);
                int consecutivo = Int32.Parse(comprobante.CONSECUTIVO) + 1;
                _comprobante.NUMERO = consecutivo.ToString();
                //  return consecutivo;
            }
        }

        #endregion
    }
}
