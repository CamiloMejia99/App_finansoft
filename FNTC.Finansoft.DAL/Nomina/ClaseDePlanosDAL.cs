
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;

namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class ClaseDePlanosDAL
    {
        public bool CreateClaseDePlano(ClaseDePlano nuevoPlano)
        {
            //los datos deben de existir
            if (String.IsNullOrEmpty(nuevoPlano.NOMBRE))
            {
                throw new ArgumentNullException("NOMBRE");
            }
            if (String.IsNullOrEmpty(nuevoPlano.TIPOPLANO))
            {
                throw new ArgumentNullException("TIPOPLANO");
            }
            if (String.IsNullOrEmpty(nuevoPlano.TIPORECEPCION))
            {
                throw new ArgumentNullException("TIPORECEPCION");
            }
            if (String.IsNullOrEmpty(nuevoPlano.EXTENSION))
            {
                throw new ArgumentNullException("EXTENSION");
            }
            if (String.IsNullOrEmpty(nuevoPlano.DELIMITADOR))
            {
                throw new ArgumentNullException("DELIMITADOR");
            }

            //es ok. pero esta validacion va en el modelo
            using (var ctx = new AccountingContext())
            {
                ctx.ClaseDePlano.Add(nuevoPlano);
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

        public bool UpdateClaseDePlano(ClaseDePlano updatePlano)
        {
            //los datos deben de existir
            if (String.IsNullOrEmpty(updatePlano.NOMBRE))
            {
                throw new ArgumentNullException("NOMBRE");
            }
            if (String.IsNullOrEmpty(updatePlano.TIPOPLANO))
            {
                throw new ArgumentNullException("TIPOPLANO");
            }
            if (String.IsNullOrEmpty(updatePlano.TIPORECEPCION))
            {
                throw new ArgumentNullException("TIPORECEPCION");
            }
            if (String.IsNullOrEmpty(updatePlano.EXTENSION))
            {
                throw new ArgumentNullException("EXTENSION");
            }
            if (String.IsNullOrEmpty(updatePlano.DELIMITADOR))
            {
                throw new ArgumentNullException("DELIMITADOR");
            }

            using (var ctx = new AccountingContext())
            {
                ctx.Entry(updatePlano).State = System.Data.Entity.EntityState.Modified;

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

        public bool DeleteClaseDePlano(ClaseDePlano deltePlano)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.ClaseDePlano.Find(deltePlano.ID) != null)
                    {
                        ctx.ClaseDePlano.Remove(deltePlano);
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
