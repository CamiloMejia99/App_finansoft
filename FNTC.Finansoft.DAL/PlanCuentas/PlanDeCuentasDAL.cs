using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.PlanDeCuentas
{
    public class PlanDeCuentasDAL
    {

        #region CuentasMayores
        #region CRUD Cuentas Mayores
        public bool CreateCuenta(CuentaMayor nuevaCuenta)
        {
            //el nombre debe exixstor
            if (String.IsNullOrEmpty(nuevaCuenta.NOMBRE))
            {
                throw new ArgumentNullException("NOMBRE");
            }
            if (String.IsNullOrEmpty(nuevaCuenta.CODIGO))
            {
                throw new ArgumentNullException("CODIGO");
            }

            //el codigo debe ser un numero 

            var codigo = 0;
            var esNumerico = Int32.TryParse(nuevaCuenta.CODIGO, out codigo);

            if (esNumerico)
            {
                //debe ser subcuenta
                var digitosCuenta = codigo.ToString().Count();
                if (digitosCuenta <= 6) //una cuenta mayor es de maximo 6 digitos
                {
                    //es ok. pero esta validacion va en el modelo
                    using (var ctx = new AccountingContext())
                    {
                        ctx.PlanCuentas.Add(nuevaCuenta);
                        try
                        {
                            //nuevaCuenta.NAT = (char)nuevaCuenta.NAT;
                            return ctx.SaveChanges() > 0 ? true : false;
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                }
                throw new ArgumentOutOfRangeException("CODIGO", "El codigo no es correcto");
            }
            throw new ArgumentOutOfRangeException("CODIGO", "El codigo no es correcto");

        }

        public bool UpdateCuenta(CuentaMayor updatedCuenta)
        {
            //el nombre debe exixstor
            if (String.IsNullOrEmpty(updatedCuenta.NOMBRE))
            {
                throw new ArgumentNullException("NOMBRE");
            }
            if (String.IsNullOrEmpty(updatedCuenta.CODIGO))
            {
                throw new ArgumentNullException("CODIGO");
            }

            //el codigo debe ser un numero 

            var codigo = 0;
            var esNumerico = Int32.TryParse(updatedCuenta.CODIGO, out codigo);

            if (esNumerico)
            {
                //debe ser subcuenta
                var digitosCuenta = codigo.ToString().Count();
                if (digitosCuenta > 6)
                {
                    //es ok. pero esta validacion va en el modelo
                    using (var ctx = new AccountingContext())
                    {
                        ctx.Entry(updatedCuenta).State = System.Data.Entity.EntityState.Modified;

                        try
                        {
                            return ctx.SaveChanges() > 0 ? true : false;
                        }
                        catch (Exception e)
                        {

                            throw;
                        }
                    }
                }
                throw new ArgumentOutOfRangeException("CODIGO", "El codigo no es correcto");
            }
            throw new ArgumentOutOfRangeException("CODIGO", "El codigo no es correcto");

        }

        public bool DeleteCuenta(CuentaMayor cuenta2Delete)
        {
            using (var ctx = new AccountingContext())
            {

                try
                {
                    //si existe
                    if (ctx.PlanCuentas.Find(cuenta2Delete.CODIGO) != null)
                    {
                        //si tiene saldos en SaldosCuentas

                    }
                    return ctx.SaveChanges() > 0 ? true : false;
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }
        #endregion
        #endregion

        #region CuentasAuxiliares
        #region crud Cuentas Auxiliares
        public Result CreateCuentaAuxiliar(CuentaAuxiliar nuevaCuenta)
        {
            var r = new Result();
            //el nombre debe exixstor
            if (String.IsNullOrEmpty(nuevaCuenta.NOMBRE))
            {
                r.ErrorsWithKey.Add("NOMBRE", "NOMBRE no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }
            if (String.IsNullOrEmpty(nuevaCuenta.CODIGO))
            {
                r.ErrorsWithKey.Add("CODIGO", "CODIGO no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }

            //el codigo debe ser un numero 
            var codigo = 0;
            var esNumerico = Int32.TryParse(nuevaCuenta.CODIGO, out codigo);

            if (esNumerico)
            {
                //debe ser subcuenta
                var digitosCuenta = codigo.ToString().Count();
                if (digitosCuenta == 9)
                {
                    //110505001
                    //es ok.  validacion va en el modelo tmb
                    using (var ctx = new AccountingContext())
                    {
                        //debe existir la mayor
                        var ctaMayor = nuevaCuenta.CODIGO.Substring(0, 6);
                        var mayor = ctx.PlanCuentas.Find(ctaMayor);

                        if (null == mayor)
                        {
                            r.ErrorsWithKey.Add("CODIGO", "No existe cuenta mayor para la auxiliar  : " + codigo);
                            return r;

                        }
                        //ya existe?
                        if (null == ctx.PlanCuentas.Find(nuevaCuenta.CODIGO))
                        {
                            //si existe un auxiliar con el mismo nombre
                            var existe = ctx.Auxiliares.Where(x => x.NOMBRE == nuevaCuenta.NOMBRE).Count() > 0 ? true : false;
                            if (existe)
                            {
                                r.ResultCode = ResultCode.Duplicated;
                                r.ErrorsWithKey.Add("NOMBRE", "Ya existe una cuenta con ese NOMBRE " + nuevaCuenta.NOMBRE);
                                return r;

                            }
                            ctx.PlanCuentas.Add(nuevaCuenta);
                            r.RowsAffected = ctx.SaveChanges();
                            r.ResultCode = ResultCode.Added;
                            //return ctx.SaveChanges() > 0 ? true : false;
                            return r;

                        }
                        else
                            r.ErrorsWithKey.Add("CODIGO", "Ya existe una cuenta con ese mismo CODIGO : " + nuevaCuenta.CODIGO);
                    }
                }
                else//numero de digitos
                {
                    r.ErrorsWithKey.Add("CODIGO", "CODIGO- El codigo no es correcto: + de 6 digitos");
                    r.ResultCode = ResultCode.Error;
                }
            }
            else //codigo no es numerico            
            {
                r.ResultCode = ResultCode.Error;
                r.ErrorsWithKey.Add("CODIGO", "CODIGO -El codigo no es correcto: el codigo debe ser numerico");
            }
            r.ResultCode = ResultCode.Error;
            return r;
        }

        public Result CreateCuentaMayor(CuentaMayor DataCM, string TipoCuenta)
        {

            var r = new Result();
            var codigo = 0;
            var esNumerico = Int32.TryParse(DataCM.CODIGO, out codigo);


            if (String.IsNullOrEmpty(DataCM.NOMBRE))
            {
                r.ErrorsWithKey.Add("NOMBRE", "El Campo Nombre es: Obligatorios");
                r.ResultCode = ResultCode.Error;
            }
            else
            if (String.IsNullOrEmpty(DataCM.CODIGO))
            {
                r.ErrorsWithKey.Add("CODIGO", "El Campo Codigo es: Obligatorios");
                r.ResultCode = ResultCode.Error;
            }
            else
            if (TipoCuenta == null)
            {
                r.ErrorsWithKey.Add("CODIGO", "Para continuar debe de seleccionar un tipo de Cuenta: Obligatorio");
                r.ResultCode = ResultCode.Error;
            }
            else
            {

                if (TipoCuenta == "GRUPO")

                {
                    if (DataCM.CODIGO.Length != 2)
                    {
                        r.ErrorsWithKey.Add("CODIGO", "Un GRUPO solo puede contener dos (2) números \n siendo el primero el número de la CLASE (Cuenta Mayor)");
                        r.ResultCode = ResultCode.Error;
                    }

                    if (esNumerico)
                    {

                        var Tipo = 0;


                        if (TipoCuenta == "GRUPO")
                        {
                            Tipo = 2;
                        }
                        else if (TipoCuenta == "CUENTA")
                        {
                            Tipo = 4;
                        }
                        else if (TipoCuenta == "SUBCUENTA")
                        {
                            Tipo = 6;
                        }

                        var TamCuenta = Tipo;
                        var digitosCuenta = codigo.ToString().Count();

                        if (digitosCuenta == Tipo && Tipo == 2)
                        {
                            using (var ctx = new AccountingContext())
                            {
                                var ctaMayor = DataCM.CODIGO.Substring(0, 2);
                                var mayor = ctx.PlanCuentas.Find(ctaMayor);

                                if (null == ctx.PlanCuentas.Find(DataCM.CODIGO))
                                {
                                    var existe = ctx.Auxiliares.Where(x => x.NOMBRE == DataCM.NOMBRE).Count() > 0 ? true : false;
                                    if (existe)
                                    {
                                        r.ResultCode = ResultCode.Duplicated;
                                        r.ErrorsWithKey.Add("NOMBRE", "Ya existe una CUENTA con ese NOMBRE " + DataCM.NOMBRE);
                                        return r;

                                    }
                                    else
                                    {
                                        ctx.PlanCuentas.Add(DataCM);
                                        r.RowsAffected = ctx.SaveChanges();
                                        r.ResultCode = ResultCode.Added;
                                        return r;
                                    }


                                }
                                else
                                    r.ErrorsWithKey.Add("CODIGO", "Ya existe una CUENTA con ese mismo CODIGO : " + DataCM.CODIGO);
                            }
                        }

                    }
                    else
                    {
                        r.ResultCode = ResultCode.Error;
                        r.ErrorsWithKey.Add("CODIGO", "CODIGO -El codigo no es correcto: el codigo debe ser numérico");
                    }
                }
                if (TipoCuenta == "CUENTA")
                {
                    if (DataCM.CODIGO.Length != 4)
                    {
                        r.ErrorsWithKey.Add("CODIGO", "Una CUENTA solo puede contener cuatro (4) números \n siendo los primeros 2 los números del GRUPO");
                        r.ResultCode = ResultCode.Error;
                    }
                    if (esNumerico)
                    {

                        var Tipo = 0;


                        if (TipoCuenta == "GRUPO")
                        {
                            Tipo = 2;
                        }
                        else if (TipoCuenta == "CUENTA")
                        {
                            Tipo = 4;
                        }
                        else if (TipoCuenta == "SUBCUENTA")
                        {
                            Tipo = 6;
                        }

                        var TamCuenta = Tipo;
                        var digitosCuenta = codigo.ToString().Count();

                        if (digitosCuenta == Tipo && Tipo == 4)
                        {
                            using (var ctx = new AccountingContext())
                            {
                                var ctaMayor = DataCM.CODIGO.Substring(0, 4);
                                var mayor = ctx.PlanCuentas.Find(ctaMayor);

                                if (null == ctx.PlanCuentas.Find(DataCM.CODIGO))
                                {
                                    var existe = ctx.Auxiliares.Where(x => x.NOMBRE == DataCM.NOMBRE).Count() > 0 ? true : false;
                                    if (existe)
                                    {
                                        r.ResultCode = ResultCode.Duplicated;
                                        r.ErrorsWithKey.Add("NOMBRE", "Ya existe una CUENTA con ese NOMBRE " + DataCM.NOMBRE);
                                        return r;

                                    }
                                    else
                                    {
                                        string test = DataCM.CODIGO;
                                        for (var i = 0; i < test.Length; ++i)
                                        {
                                            Console.WriteLine(test[i]);
                                        }
                                        var GrupoA = test[0];
                                        var GrupoB = test[1];
                                        var GrupoC = GrupoA + "" + GrupoB;

                                        var GrupoR = ctx.PlanCuentas.Where(x => x.CODIGO == GrupoC).Count() > 0 ? true : false;
                                        if (GrupoR)
                                        {
                                            ctx.PlanCuentas.Add(DataCM);
                                            r.RowsAffected = ctx.SaveChanges();
                                            r.ResultCode = ResultCode.Added;
                                            return r;
                                        }
                                        else
                                        {
                                            r.ResultCode = ResultCode.Duplicated;
                                            r.ErrorsWithKey.Add("CODIGO", "No existe el GRUPO " + "(" + GrupoC + ") " + ": Para crear una CUENTA debe estar ligada a un GRUPO");
                                            return r;
                                        }


                                    }


                                }
                                else
                                    r.ErrorsWithKey.Add("CODIGO", "Ya existe una CUENTA con ese mismo CODIGO : " + DataCM.CODIGO);
                            }
                        }


                    }
                    else
                    {
                        r.ResultCode = ResultCode.Error;
                        r.ErrorsWithKey.Add("CODIGO", "CODIGO -El codigo no es correcto: el codigo debe ser numérico");
                    }

                }

                if (TipoCuenta == "SUBCUENTA")
                {
                    if (DataCM.CODIGO.Length != 6)
                    {
                        r.ErrorsWithKey.Add("CODIGO", "Una SUBCUENTA solo puede contener seis (6) números \n siendo los primeros 4, los números de la cuenta a la que esta ligada");
                        r.ResultCode = ResultCode.Error;
                    }
                    if (esNumerico)
                    {

                        var Tipo = 0;


                        if (TipoCuenta == "GRUPO")
                        {
                            Tipo = 2;
                        }
                        else if (TipoCuenta == "CUENTA")
                        {
                            Tipo = 4;
                        }
                        else if (TipoCuenta == "SUBCUENTA")
                        {
                            Tipo = 6;
                        }

                        var TamCuenta = Tipo;
                        var digitosCuenta = codigo.ToString().Count();

                        if (digitosCuenta == Tipo && Tipo == 6)
                        {
                            using (var ctx = new AccountingContext())
                            {
                                var ctaMayor = DataCM.CODIGO.Substring(0, 6);
                                var mayor = ctx.PlanCuentas.Find(ctaMayor);

                                if (null == ctx.PlanCuentas.Find(DataCM.CODIGO))
                                {
                                    var existe = ctx.PlanCuentas.Where(x => x.NOMBRE == DataCM.NOMBRE).Count() > 0 ? true : false;
                                    if (existe)
                                    {
                                        r.ResultCode = ResultCode.Duplicated;
                                        r.ErrorsWithKey.Add("NOMBRE", "Ya existe una cuenta con ese NOMBRE " + DataCM.NOMBRE);
                                        return r;

                                    }
                                    else
                                    {
                                        string test = DataCM.CODIGO;
                                        for (var i = 0; i < test.Length; ++i)
                                        {
                                            Console.WriteLine(test[i]);
                                        }
                                        var GrupoA = test[0];
                                        var GrupoB = test[1];
                                        var GrupoC = test[2];
                                        var GrupoD = test[3];
                                        var GrupoE = GrupoA + "" + GrupoB + "" + GrupoC + "" + GrupoD;

                                        var GrupoR = ctx.PlanCuentas.Where(x => x.CODIGO == GrupoE).Count() > 0 ? true : false;
                                        if (GrupoR)
                                        {
                                            ctx.PlanCuentas.Add(DataCM);
                                            r.RowsAffected = ctx.SaveChanges();
                                            r.ResultCode = ResultCode.Added;
                                            return r;
                                        }
                                        else
                                        {
                                            r.ResultCode = ResultCode.Duplicated;
                                            r.ErrorsWithKey.Add("CODIGO", "No Existe la Cuenta " + "(" + GrupoE + ") " + ": Para crear una SUBCUENTA esta debe estar ligada a una cuenta");
                                            return r;
                                        }


                                    }


                                }
                                else
                                    r.ErrorsWithKey.Add("CODIGO", "Ya existe una cuenta con ese mismo CÓDIGO : " + DataCM.CODIGO);
                            }
                        }

                    }
                    else
                    {
                        r.ResultCode = ResultCode.Error;
                        r.ErrorsWithKey.Add("CODIGO", "CODIGO -El código no es correcto: el código debe ser numérico");
                    }

                }

            }

            r.ResultCode = ResultCode.Error;
            return r;
        }

        public Result CreateCuentaNiif(CuentaAuxiliar nuevaCuenta)
        {
            var r = new Result();
            //el nombre debe exixstor
            if (String.IsNullOrEmpty(nuevaCuenta.NOMBRE))
            {
                r.ErrorsWithKey.Add("NOMBRE", "NOMBRE no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }
            if (String.IsNullOrEmpty(nuevaCuenta.CODIGO))
            {
                r.ErrorsWithKey.Add("CODIGO", "CODIGO no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }

            //el codigo debe ser un numero 
            var codigo = 0;
            var esNumerico = Int32.TryParse(nuevaCuenta.CODIGO, out codigo);

            if (esNumerico)
            {
                //debe ser subcuenta
                var digitosCuenta = codigo.ToString().Count();
                if (digitosCuenta == 9)
                {
                    //110505001
                    //es ok.  validacion va en el modelo tmb
                    using (var ctx = new AccountingContext())
                    {
                        //ya existe?
                        if (null == ctx.PlanCuentas.Find(nuevaCuenta.CODIGO))
                        {
                            //si existe un auxiliar con el mismo nombre
                            var existe = ctx.Auxiliares.Where(x => x.NOMBRE == nuevaCuenta.NOMBRE).Count() > 0 ? true : false;
                            if (existe)
                            {
                                r.ResultCode = ResultCode.Duplicated;
                                r.ErrorsWithKey.Add("NOMBRE", "Ya existe una cuenta con ese NOMBRE " + nuevaCuenta.NOMBRE);
                                return r;

                            }
                            ctx.PlanCuentas.Add(nuevaCuenta);
                            r.RowsAffected = ctx.SaveChanges();
                            r.ResultCode = ResultCode.Added;
                            //return ctx.SaveChanges() > 0 ? true : false;
                            return r;

                        }
                        else
                            r.ErrorsWithKey.Add("CODIGO", "Ya existe una cuenta con ese mismo CÓDIGO : " + nuevaCuenta.CODIGO);
                    }
                }
                else//numero de digitos
                {
                    r.ErrorsWithKey.Add("CODIGO", "CÓDIGO- El código no es correcto: + de 6 digitos");
                    r.ResultCode = ResultCode.Error;
                }
            }
            else //codigo no es numerico            
            {
                r.ResultCode = ResultCode.Error;
                r.ErrorsWithKey.Add("CODIGO", "CÓDIGO -El código no es correcto: el código debe ser numérico");
            }
            r.ResultCode = ResultCode.Error;
            return r;
        }


        public Result CreateCuentaImpusto(CuentaImpuestos nuevaCuenta)
        {
            var r = new Result();
            //el nombre debe exixstor
            if (String.IsNullOrEmpty(nuevaCuenta.NOMBRE))
            {
                r.ErrorsWithKey.Add("NOMBRE", "NOMBRE no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }
            if (String.IsNullOrEmpty(nuevaCuenta.CODIGO))
            {
                r.ErrorsWithKey.Add("CODIGO", "CÓDIGO no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }

            //el codigo debe ser un numero 
            var codigo = 0;
            var esNumerico = Int32.TryParse(nuevaCuenta.CODIGO, out codigo);

            if (esNumerico)
            {
                //debe ser subcuenta
                var digitosCuenta = codigo.ToString().Count();
                if (digitosCuenta == 9)
                {
                    //110505001
                    //es ok.  validacion va en el modelo tmb
                    using (var ctx = new AccountingContext())
                    {
                        //debe existir la mayor
                        var ctaMayor = nuevaCuenta.CODIGO.Substring(0, 6);
                        var mayor = ctx.PlanCuentas.Find(ctaMayor);

                        if (null == mayor)
                        {
                            r.ErrorsWithKey.Add("CODIGO", "No existe cuenta mayor para la auxiliar  : " + codigo);
                            return r;

                        }
                        //ya existe?
                        if (null == ctx.PlanCuentas.Find(nuevaCuenta.CODIGO))
                        {
                            //si existe un auxiliar con el mismo nombre
                            var existe = ctx.Auxiliares.Where(x => x.NOMBRE == nuevaCuenta.NOMBRE).Count() > 0 ? true : false;
                            if (existe)
                            {
                                r.ResultCode = ResultCode.Duplicated;
                                r.ErrorsWithKey.Add("NOMBRE", "Ya existe una cuenta con ese NOMBRE " + nuevaCuenta.NOMBRE);
                                return r;

                            }
                            ctx.CuentasImpuestos.Add(nuevaCuenta);
                            r.RowsAffected = ctx.SaveChanges();
                            r.ResultCode = ResultCode.Added;
                            //return ctx.SaveChanges() > 0 ? true : false;
                            return r;

                        }
                        else
                            r.ErrorsWithKey.Add("CODIGO", "Ya existe una cuenta con ese mismo CODIGO : " + nuevaCuenta.CODIGO);
                    }
                }
                else//numero de digitos
                {
                    r.ErrorsWithKey.Add("CODIGO", "CÓDIGO- El código no es correcto: + de 6 digitos");
                    r.ResultCode = ResultCode.Error;
                }
            }
            else //codigo no es numerico            
            {
                r.ResultCode = ResultCode.Error;
                r.ErrorsWithKey.Add("CODIGO", "CÓDIGO -El código no es correcto: el código debe ser numérico");
            }
            r.ResultCode = ResultCode.Error;
            return r;
        }

        public Result UpdateCuentaAuxiliar(CuentaAuxiliar updatedCuenta)
        {
            //solo puedo cambiar el nombre y atribustos
            var r = new Result();
            //el nombre debe exixstor
            if (String.IsNullOrEmpty(updatedCuenta.NOMBRE))
            {
                r.ErrorsWithKey.Add("NOMBRE", "NOMBRE no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }
            //debe tener seleccionada la naturaleza
            if (String.IsNullOrEmpty(updatedCuenta.NATURALEZA))
            {
                r.ErrorsWithKey.Add("NATURALEZA", "NATURALEZA no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }

            using (var ctx = new AccountingContext())
            {
                //si existe otro auxiliar con el mismo nombre
                var existe = ctx.Auxiliares.Where
                    (x => x.NOMBRE == updatedCuenta.NOMBRE && x.CODIGO != updatedCuenta.CODIGO)
                    .Count() > 0 ? true : false;
                if (existe)
                {
                    r.ResultCode = ResultCode.Duplicated;
                    r.ErrorsWithKey.Add("NOMBRE", "Ya existe una cuenta con ese NOMBRE " + updatedCuenta.NOMBRE);
                    return r;
                }
                try
                {
                    ctx.Entry(updatedCuenta).State = System.Data.Entity.EntityState.Modified;
                    r.RowsAffected = ctx.SaveChanges();
                    r.ResultCode = ResultCode.Updated;
                    return r;
                }
                catch (Exception e)
                {
                    r.ResultCode = ResultCode.Error;
                    r.Errors.Add(1, e.Message);
                    return r;
                }
            }

        }

        public bool DeleteCuentaAuxiliar(CuentaAuxiliar cuenta2Delete)
        {
            using (var ctx = new AccountingContext())
            {

                try
                {
                    //si existe
                    var cta = ctx.Auxiliares.Find(cuenta2Delete.CODIGO);
                    if (null != cta)
                    {
                        if (!this.CuentaAuxiliarTieneSaldos(cta.CODIGO))
                        {
                            ctx.Entry(cta).State = System.Data.Entity.EntityState.Deleted;
                            return ctx.SaveChanges() > 0 ? true : false;
                        }
                        return false;//no se puede borrar , tiene saldos
                    }
                }
                catch (Exception e)
                {
                    return false;
                    //throw;
                }
                return false;
            }
        }

        public List<CuentaMayor> GetCuentasAuxliares()
        {
            var list = new List<CuentaMayor>();
            try
            {
                using (var ctx = new AccountingContext())
                {
                    list = ctx.PlanCuentas.Where(x => x.CODIGO.Length == 9).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return list;
        }
        #endregion

        #region methods
        #region Gets
        private List<CuentaAuxiliar> GetCuentasAuxiliares(string term = "", int page = 0, int rowsPerPage = 0)
        {
            term = term.ToUpper();
            var cuentas = new List<CuentaAuxiliar>();
            //var cuentas
            using (var ctx = new AccountingContext())
            {
                cuentas =
                    ctx.Auxiliares.Where(pc => pc.CODIGO.Substring(0, term.Length).Contains(term) || pc.NOMBRE.Contains(term))
                    .OrderBy(o => o.CODIGO).ToList();
                //var planCuentas = new List<PlanCuentas>();
                //ctx.PlanCuentas.ToList().ForEach(item =>
                //    _terceros.Add(AutoMapper.Mapper.Map<TerceroDTO>(item)));
            }
            return cuentas;
        }

        /// <summary>
        /// Obtiene las cuentas que contienen el term y son de tipo type
        /// </summary>
        /// <param name="term"></param>
        /// <param name="page"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="type">0 o default : todas las cuentas 1:Cuentas Mayores 2: Auxiliares 3: Impuestos</param>
        /// <returns></returns>
        public List<CuentaMayor> GetCuentas(string term = "", int page = 0, int rowsPerPage = 0, int type = 0)
        {
            var cuentas = new List<CuentaMayor>();

            using (var ctx = new AccountingContext())
            {
                if (type == 4) //exacta
                {
                    var cuenta = ctx.PlanCuentas.
                   FirstOrDefault(pc => pc.CODIGO.Equals(term) || pc.NOMBRE.Equals(term));

                    if (cuenta == null)
                    {
                        cuenta = new CuentaMayor();
                    }

                    if (cuenta.EsCuentaImpuesto)
                    {
                        cuentas.Add(cuenta);
                    }

                }


                cuentas = ctx.PlanCuentas.
                    Where(pc => pc.CODIGO.Contains(term) || pc.NOMBRE.Contains(term))
                  .OrderBy(o => o.CODIGO).ToList();
            }
            switch (type)
            {

                case 1:
                    cuentas = cuentas.
                    Where(p => p.GetType() == typeof(CuentaMayor)).ToList(); break;
                case 2:
                    cuentas = cuentas.
                    Where(p => p.GetType() == typeof(CuentaAuxiliar)
                        || p.GetType() == typeof(CuentaImpuestos)).
                    ToList(); break;
                case 3:
                    cuentas = cuentas.
                        Where(p => p.GetType() == typeof(CuentaImpuestos))
                        .ToList(); break;
                default:
                    break;
            }

            return cuentas;
        }

        public List<CuentaMayor> GetCuentasNIIF()
        {
            var cuentas = new List<CuentaMayor>();

            using (var ctx = new AccountingContext())
            {
                cuentas = ctx.PlanCuentas.
                    Where(pc => pc.CTANIIF != null)
                  .OrderBy(o => o.CODIGO).ToList();
            }

            cuentas = cuentas.
                    Where(p => p.GetType() == typeof(CuentaAuxiliar)
                        || p.GetType() == typeof(CuentaImpuestos)).
                    ToList();

            return cuentas;
        }

        public List<CuentaMayor> GetCuentasParaGruposActivosFijos()
        {
            var cuentas = new List<CuentaMayor>();

            using (var ctx = new AccountingContext())
            {
                cuentas = ctx.PlanCuentas.OrderBy(o => o.CODIGO).ToList();
            }

            cuentas = cuentas.
                    Where(p => p.GetType() == typeof(CuentaAuxiliar)
                        || p.GetType() == typeof(CuentaImpuestos)).
                    ToList();

            return cuentas;
        }

        public List<CuentaMayor> GetCuentasNiif()
        {
            var cuentas = new List<CuentaMayor>();

            using (var ctx = new AccountingContext())
            {
                cuentas = ctx.PlanCuentas.
                    Where(pc => pc.EsCuentaNIIF == true)
                  .OrderBy(o => o.CODIGO).ToList();
            }

            cuentas = cuentas.
                    Where(p => p.GetType() == typeof(CuentaAuxiliar)
                        || p.GetType() == typeof(CuentaImpuestos)).
                    ToList();

            return cuentas;
        }

        // metodo que retorna una lista de cuentas depeniendo del numero de caracteres que contenga el codigo de la cuenta
        public List<CuentaMayor> GetCuentasPorNivel(string nivel)
        {
            var cuentasFiltradas = new List<CuentaMayor>();
            using (var ctx = new AccountingContext())
            {
                var cuentas = ctx.PlanCuentas.ToList();

                switch (nivel)
                {
                    case "1":
                        cuentasFiltradas = cuentas.Where(x => x.CODIGO.Length == 1).ToList();
                        break;
                    case "2":
                        cuentasFiltradas = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2).ToList();
                        break;
                    case "3":
                        cuentasFiltradas = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4).ToList();
                        break;
                    case "4":
                        cuentasFiltradas = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4 || x.CODIGO.Length == 6).ToList();
                        break;
                    case "5":
                        cuentasFiltradas = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4 || x.CODIGO.Length == 6 || x.CODIGO.Length == 9).ToList();
                        break;
                }
            }
            return cuentasFiltradas;
        }

        public CuentaMayor GetCuenta(string term)
        {
            using (var ctx = new AccountingContext())
            {
                /*var cuenta = ctx.PlanCuentas.
                    First(pc => pc.CODIGO == (term) || pc.NOMBRE == (term));
                return cuenta;*/
                var cta = ctx.CuentasImpuestos.Where(x => x.CODIGO == term).First();
                return cta;
            }

        }


        public CuentaMayor GetCuentaImpuestos(string term)
        {
            using (var ctx = new AccountingContext())
            {
                var cta = ctx.CuentasImpuestos.Where(x => x.CODIGO == term).First();
                return cta;
            }

        }
        #endregion


        private bool CuentaAuxiliarTieneSaldos(string codigoCuenta)
        {
            using (var ctx = new AccountingContext())
            {
                var cta = ctx.SaldosCuentas.Where(x => x.CODIGO == codigoCuenta).First();
                if (cta != null)
                {
                    //si tiene saldos en SaldosCuentas
                    if (cta.SALDO != 0
                        || cta.MCREDITO != 0 || cta.MDEBITO != 0
                        ) //tiene saldo
                        return true;
                    return false;
                }
                else
                {
                    throw new ArgumentNullException("La CuantaAuxiliar no Existe: " + codigoCuenta);
                }
            }
        }
        #endregion

        #endregion


        #region NO VAN

        //NO VAN
        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <param name="page"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="type">1 mayor, 2 auxiliares, 3 impuestos, etc</param>
        /// <returns></returns>
        public List<CuentaMayor> GetCuentas2(string term = "", int page = 0, int rowsPerPage = 0, int type = 1)
        {
            term = term.ToUpper();
            var cuentas = new List<CuentaMayor>();
            //var cuentas
            using (var ctx = new AccountingContext())
            {

                switch (type)
                {
                    case 1:
                        cuentas =
                   ctx.PlanCuentas.Where(pc => pc.CODIGO.Substring(0, term.Length).Contains(term) || pc.NOMBRE.Contains(term))
                   .OrderBy(o => o.CODIGO).ToList(); break;
                    // case 2: var auxs =
                    //ctx.PlanCuentas.Where(pc => pc.CODIGO.Substring(0, term.Length).Contains(term) || pc.NOMBRE.Contains(term))
                    //.OrderBy(o => o.CODIGO).ToList().Where(x =>  typeof; break;
                    case 3:
                        var imps =
                   ctx.CuentasImpuestos.Where(pc => pc.CODIGO.Substring(0, term.Length).Contains(term) || pc.NOMBRE.Contains(term))
                   .OrderBy(o => o.CODIGO).ToList().Select(x => (CuentaMayor)x); break;
                    default:
                        break;
                }
                cuentas =
                    ctx.PlanCuentas.Where(pc => pc.CODIGO.Substring(0, term.Length).Contains(term) || pc.NOMBRE.Contains(term))
                    .OrderBy(o => o.CODIGO).ToList();

            }
            return cuentas;
        }

        private List<CuentaMayor> GetCuentasAuxiliares2(string term = "")
        {
            term = term.ToUpper();
            var cuentas = new List<CuentaMayor>();
            //var cuentas
            using (var ctx = new AccountingContext())
            {
                cuentas =
                    ctx.PlanCuentas.Where(pc =>
                        pc.CODIGO.Substring(0, term.Length).
                        Contains(term) || pc.NOMBRE.Contains(term)

                        )
                    .OrderBy(o => o.CODIGO).ToList();
            }
            return cuentas;
        }

        public Result UpdateCuentaImpuestos(CuentaImpuestos updatedCuenta)
        {
            //solo puedo cambiar el nombre y atribustos
            var r = new Result();
            //el nombre debe exixstor
            if (String.IsNullOrEmpty(updatedCuenta.NOMBRE))
            {
                r.ErrorsWithKey.Add("NOMBRE", "NOMBRE no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }
            //debe tener seleccionada la naturaleza
            if (String.IsNullOrEmpty(updatedCuenta.NATURALEZA))
            {
                r.ErrorsWithKey.Add("NATURALEZA", "NATURALEZA no puede estar vacio");
                r.ResultCode = ResultCode.Error;
            }

            using (var ctx = new AccountingContext())
            {
                //si existe otro auxiliar con el mismo nombre
                var existe = ctx.CuentasImpuestos.Where
                    (x => x.NOMBRE == updatedCuenta.NOMBRE && x.CODIGO != updatedCuenta.CODIGO)
                    .Count() > 0 ? true : false;
                if (existe)
                {
                    r.ResultCode = ResultCode.Duplicated;
                    r.ErrorsWithKey.Add("NOMBRE", "Ya existe una cuenta con ese NOMBRE " + updatedCuenta.NOMBRE);
                    return r;
                }
                try
                {
                    ctx.Entry(updatedCuenta).State = System.Data.Entity.EntityState.Modified;
                    r.RowsAffected = ctx.SaveChanges();
                    r.ResultCode = ResultCode.Updated;
                    return r;
                }
                catch (Exception e)
                {
                    r.ResultCode = ResultCode.Error;
                    r.Errors.Add(1, e.Message);
                    return r;
                }
            }

        }


        #endregion
    }
}
