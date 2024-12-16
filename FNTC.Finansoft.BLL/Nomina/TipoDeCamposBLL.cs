using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;/*cambiar a DTO*/
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.BLL.TipoDeCampos
{
    public class TipoDeCamposBLL
    {
        private DAL.Nomina.TipoDeCamposDAL dal;
        //crud de clase de plano

        public TipoDeCamposBLL()
        {
            dal = new DAL.Nomina.TipoDeCamposDAL();
        }

        public List<TipoDeCampo> GetTipoDeCampos()
        {
            using (var ctx = new AccountingContext())
            {
                List<TipoDeCampo> lista = ctx.TipoDeCampo.ToList();
                return lista;

            }


        }
    }
}

