using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;

namespace FNTC.Finansoft.Accounting.DAL.Nomina
{

    public class CorrecionNominaDAL
    {

        public bool CreateEnvi(CorreccionNomina nuevaCorreccion)
        {
            //el nombre debe existir
            if (String.IsNullOrEmpty(nuevaCorreccion.EMPRESA))
            {
                throw new ArgumentNullException("EMPRESA");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.CEDULA))
            {
                throw new ArgumentNullException("CEDULA");
            }

            if (String.IsNullOrEmpty(nuevaCorreccion.AGENCIA))
            {
                throw new ArgumentNullException("AGENCIA");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.PERIODO))
            {
                throw new ArgumentNullException("PERIODO");
            }

            if (String.IsNullOrEmpty(nuevaCorreccion.CONCEPTO))
            {
                throw new ArgumentNullException("CONCEPTO");
            }


            using (var ctx = new AccountingContext())
            {
                ctx.CorreccionNomina.Add(nuevaCorreccion);
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

        public bool UpdateDisc(CorreccionNomina updateobjeto)
        {
            //el nombre debe existir
            if (String.IsNullOrEmpty(updateobjeto.EMPRESA))
            {
                throw new ArgumentNullException("EMPRESA");
            }
            if (String.IsNullOrEmpty(updateobjeto.CEDULA))
            {
                throw new ArgumentNullException("CEDULA");
            }

            if (String.IsNullOrEmpty(updateobjeto.AGENCIA))
            {
                throw new ArgumentNullException("AGENCIA");
            }

            if (String.IsNullOrEmpty(updateobjeto.CONCEPTO))
            {
                throw new ArgumentNullException("CONCEPTO");
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

        public bool DeleteDisc(CorreccionNomina CorreccionDelete)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.CorreccionNomina.Find(CorreccionDelete.ID) != null)
                    {
                        ctx.CorreccionNomina.Remove(CorreccionDelete);
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
