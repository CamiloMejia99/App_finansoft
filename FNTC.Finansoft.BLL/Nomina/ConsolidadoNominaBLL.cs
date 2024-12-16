using FNTC.Finansoft.Accounting.DTO.Nomina;
using System.Collections.Generic;
using System.Linq;


namespace FNTC.Finansoft.Accounting.BLL.Nomina
{
    public class ConsolidadoNominaBLL
    {
        private DAL.Nomina.ConsolidadoNominaDAL dal;
        //crud de clase de plano

        public ConsolidadoNominaBLL()
        {
            dal = new DAL.Nomina.ConsolidadoNominaDAL();
        }

        public List<ConsolidadoNomina> GetConsolidadoNomina()
        {
            using (var ctx = new FNTC.Finansoft.Accounting.DTO.AccountingContext())
            {
                List<ConsolidadoNomina> lista = ctx.ConsolidadoNomina.ToList();
                return lista;

                //filtrado
                /* var comprobantes = ctx.ClaseDePlano.ToList();
                 return comprobantes;*/
            }
        }

    }
}
