using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;

namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class ConsolidadoNominaDAL
    {

        public bool UpdateConsolidado(ConsolidadoNomina updateobjeto)
        {
            //el nombre debe existir





            using (var ctx = new AccountingContext())
            {
                ctx.Entry(updateobjeto).State = System.Data.Entity.EntityState.Modified;

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
    }
}
