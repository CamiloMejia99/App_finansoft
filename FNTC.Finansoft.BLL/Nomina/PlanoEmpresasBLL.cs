using FNTC.Finansoft.Accounting.DTO.Nomina;
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.BLL.PlanoEmpresas
{
    public class PlanoEmpresasBLL
    {
        private DAL.Nomina.PlanoEmpresaDAL dal;
        //crud de clase de plano

        public PlanoEmpresasBLL()
        {
            dal = new DAL.Nomina.PlanoEmpresaDAL();
        }

        public List<PlanoEmpresa> GetPlanoEmpresa()
        {
            using (var ctx = new FNTC.Finansoft.Accounting.DTO.AccountingContext())
            {
                List<PlanoEmpresa> lista = ctx.PlanoEmpresa.ToList();
                return lista;

                //filtrado
                /* var comprobantes = ctx.ClaseDePlano.ToList();
                 return comprobantes;*/
            }
        }



    }
}
