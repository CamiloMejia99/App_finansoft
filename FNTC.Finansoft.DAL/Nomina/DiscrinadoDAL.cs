using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class DiscrinadoDAL
    {

        public bool CreateDisc(Discriminacion nuevaDisc)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(nuevaDisc.PERDEDUCC))
            {
                throw new ArgumentNullException("PERDEDUCC");
            }



            using (var ctx = new AccountingContext())
            {
                ctx.Discriminacion.Add(nuevaDisc);
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

        public bool UpdateDisc(Discriminacion updateobjeto)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(updateobjeto.PERDEDUCC))
            {
                throw new ArgumentNullException("PERDEDUCC");
            }



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

        public bool DeleteDisc(Discriminacion descuentoDelete)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.Discriminacion.Find(descuentoDelete.ID) != null)
                    {
                        ctx.Discriminacion.Remove(descuentoDelete);
                    }
                    return ctx.SaveChanges() > 0 ? true : false;
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }

        public static List<string> GetPeriodoEmp(string EMPRESA)
        {
            var lista = new List<string>();
            using (var ctx = new AccountingContext())
            {
                lista = ctx.Discriminacion.Select(p => p.PERDEDUCC).Distinct().ToList();//.Where(p => p.EMPRESA == EMPRESA)
            }
            return lista;
        }
    }

}
