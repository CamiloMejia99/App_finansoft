using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;/*cambiar a DTO*/
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.BLL.ClaseDePlanos
{
    public class ClaseDePlanosBLL
    {
        private DAL.Nomina.ClaseDePlanosDAL dal;
        //crud de clase de plano

        public ClaseDePlanosBLL()
        {
            dal = new DAL.Nomina.ClaseDePlanosDAL();
        }

        public List<ClaseDePlano> GetClaseDePlano()
        {
            using (var ctx = new AccountingContext())
            {
                List<ClaseDePlano> lista = ctx.ClaseDePlano.ToList();
                return lista;

                //filtrado
                /* var comprobantes = ctx.ClaseDePlano.ToList();
                 return comprobantes;*/
            }

            /* List<ClaseDePlano> lista = dal
             var dto = new List<FormasDePagoDTO>();
             switch (clasePlano)
             {
                 case "RC":
                     dto = dal.GetAllFormaDePago().Where(x => x.AplicaParaReciboCaja_Ingresos).ToList();
                     break;
                 case "CE":
                     dto = dal.GetAllFormaDePago().Where(x => x.AplicaPara_ComprobanteEgreso_Pagos).ToList();
                     break;
                 default:
                     break;
             }

             return dto;*/
        }
    }
}
