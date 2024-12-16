using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;

namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class TipoDeCamposDAL
    {
        public bool CreateTipoCuenta(TipoDeCampo nuevo_Cuenta)
        {
            //el nombre debe existir


            if (String.IsNullOrEmpty(nuevo_Cuenta.NOMBRECAMPO))
            {
                throw new ArgumentNullException("NOMBRECAMPO");
            }
            if (String.IsNullOrEmpty(nuevo_Cuenta.DESCRIPCION))
            {
                throw new ArgumentNullException("DESCRIPCION");
            }
            using (var ctx = new AccountingContext())
            {
                ctx.TipoDeCampo.Add(nuevo_Cuenta);
                try
                {
                    return ctx.SaveChanges() > 0 ? true : false;
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            //el codigo debe ser un numero 


        }
        public bool UpdateTipoDeCuenta(TipoDeCampo updateobjeto)
        {
            //el nombre debe existir


            if (String.IsNullOrEmpty(updateobjeto.NOMBRECAMPO))
            {
                throw new ArgumentNullException("NOMBRECAMPO");
            }
            if (String.IsNullOrEmpty(updateobjeto.DESCRIPCION))
            {
                throw new ArgumentNullException("DESCRIPCION");
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



        public bool DeleteClaseDePlano(TipoDeCampo deltePlano)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.TipoDeCampo.Find(deltePlano.ID) != null)
                    {
                        ctx.TipoDeCampo.Remove(deltePlano);
                    }
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
