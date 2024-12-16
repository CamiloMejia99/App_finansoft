
using FNTC.Finansoft.Accounting.DAL.PlanDeCuentas;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.BLL.PlanCuentas
{

    public class PlanCuentasBLL
    {
        // FNTC.Finansoft.Accounting.DAL.Model.AccountingContext ctx;

        public PlanCuentasBLL()
        {
            //  ctx = new DAL.Model.AccountingContext();
        }


        public List<CuentasTree> GetCuentas4TreeView(string term = "")
        {
            var dal = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL();
            List<CuentaMayor> cuentas = dal.GetCuentas(term);
            List<CuentaMayor> todasLasCuentas = dal.GetCuentas();

            var cta = cuentas.OrderBy(x => x.CODIGO.Length);

            List<CuentasTree> cuentasTree = new List<CuentasTree>();

            var _clases = cuentas.Where(x => x.CODIGO.Length == 1).ToList();
            if (_clases.Count == 0)
            {
                var __clases = cuentas.Select(x => x.CODIGO.Substring(0, 1)).Distinct().ToList();
                foreach (var item in __clases)
                {
                    _clases.Add(todasLasCuentas.Find(x => x.CODIGO == item));
                }
            }

            var _grupos = cuentas.Where(x => x.CODIGO.Length == 2).Distinct().ToList();
            if (_grupos.Count == 0)
            {
                var __grupos = cuentas.Select(x => x.CODIGO.Substring(0, 2)).Distinct().ToList();

                foreach (var item in __grupos)
                {
                    _grupos.Add(todasLasCuentas.Find(x => x.CODIGO == item));


                }

            }
            foreach (var item in _grupos)
            {
                //si no existe la clase para el grupo
                if (_clases.Find(x => x.CODIGO == item.CODIGO.Substring(0, 1)) == null)
                {
                    _clases.Add(todasLasCuentas.Find(x => x.CODIGO == item.CODIGO.Substring(0, 1)));
                }
            }

            var _cuentas = cuentas.Where(x => x.CODIGO.Length == 4).ToList();
            if (_cuentas.Count == 0)
            {
                var __cuentas = cuentas.Select(x => x.CODIGO.Substring(0, 4)).Distinct().ToList();
                foreach (var item in __cuentas)
                {
                    _cuentas.Add(todasLasCuentas.Find(x => x.CODIGO == item));

                }
            }
            foreach (var item in _cuentas)
            {
                //si no existe el grupo  para la cuenta
                if (_grupos.Find(x => x.CODIGO == item.CODIGO.Substring(0, 2)) == null)
                {
                    _grupos.Add(todasLasCuentas.Find(x => x.CODIGO == item.CODIGO.Substring(0, 2)));
                }
            }


            var _subcuentas = cuentas.Where(x => x.CODIGO.Length == 6).Distinct().ToList();
            if (_subcuentas.Count == 0)
            {
                var __subcuentas = cuentas.Select(x => x.CODIGO.Substring(0, 6)).Distinct().ToList();
                foreach (var item in __subcuentas)
                {
                    _subcuentas.Add(todasLasCuentas.Find(x => x.CODIGO == item));

                }
            }
            foreach (var item in _subcuentas)
            {
                //si no existe el cuenta  para la sbucuenta
                if (_cuentas.Find(x => x.CODIGO == item.CODIGO.Substring(0, 4)) == null)
                {
                    _cuentas.Add(todasLasCuentas.Find(x => x.CODIGO == item.CODIGO.Substring(0, 4)));
                }
            }



            var auxiliares = cuentas.Where(x => x.CODIGO.Length == 9).ToList();
            var _auxiliares = new List<CuentaMayor>();
            foreach (var item in auxiliares)
            {
                _auxiliares.Add(todasLasCuentas.Find(x => x.CODIGO == item.CODIGO));
            }




            cuentasTree.AddRange(_clases.Select(x => new CuentasTree() { id = Int32.Parse(x.CODIGO), name = x.NOMBRE, CODIGO = x.CODIGO }));

            var nodoPadre = new CuentasTree();
            var nodoHijo = new CuentasTree();

            foreach (var _clase in _clases)
            {
                nodoPadre = cuentasTree.Find(y => y.CODIGO == _clase.CODIGO);

                foreach (var _grupo in _grupos.Where(x => x.CODIGO.Substring(0, 1) == nodoPadre.CODIGO))
                {
                    nodoPadre.children
                        .Add(new CuentasTree()
                        {
                            CODIGO = _grupo.CODIGO,
                            id = Int32.Parse(_grupo.CODIGO),
                            name = _grupo.NOMBRE
                        });

                    nodoHijo = nodoPadre.children.Find(y => y.CODIGO == _grupo.CODIGO);

                    foreach (var _cuenta in _cuentas.Where(x => x.CODIGO.Substring(0, 2) == nodoHijo.CODIGO))
                    {
                        nodoHijo.children
                            .Add(new CuentasTree()
                            {
                                CODIGO = _cuenta.CODIGO,
                                id = Int32.Parse(_cuenta.CODIGO),
                                name = _cuenta.NOMBRE
                            });

                        var n3 = nodoHijo.children.Find(y => y.CODIGO == _cuenta.CODIGO);

                        foreach (var _subcuenta in _subcuentas.Where(x => x.CODIGO.Substring(0, 4) == n3.CODIGO))
                        {
                            n3.children
                                .Add(new CuentasTree()
                                {
                                    CODIGO = _subcuenta.CODIGO,
                                    id = Int32.Parse(_subcuenta.CODIGO),
                                    name = _subcuenta.NOMBRE
                                });

                            var n4 = n3.children.Find(y => y.CODIGO == _subcuenta.CODIGO);

                            foreach (var auxiliar in _auxiliares.Where(x => x.CODIGO.Substring(0, 6) == n4.CODIGO))
                            {
                                n4.children
                                    .Add(new CuentasTree()
                                    {
                                        CODIGO = auxiliar.CODIGO,
                                        id = Int32.Parse(auxiliar.CODIGO),
                                        name = auxiliar.NOMBRE
                                    });
                            }
                        }
                    }
                }
            }

            return cuentasTree;
        }

        public List<CuentaMayor> GetCuentasAuxiliares()
        {
            var respuesta = new PlanDeCuentasDAL().GetCuentasAuxliares();
            return respuesta;   
        }

        public class CuentasTree
        {

            public int id { get; set; }
            public string CODIGO { get; set; }
            public string name { get; set; }
            public List<CuentasTree> children { get; set; }

            public CuentasTree()
            {
                children = new List<CuentasTree>();
            }
        }

    }


}
