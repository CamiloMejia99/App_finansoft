using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;

namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class DescuentoNominaDAL
    {

        public bool CreateEnvi(DescuentoNomina nuevaCorreccion)
        {

            if (String.IsNullOrEmpty(nuevaCorreccion.CEDULA))
            {
                throw new ArgumentNullException("CEDULA");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.NOMBRE))
            {
                throw new ArgumentNullException("NOMBRE");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.CUENTA))
            {
                throw new ArgumentNullException("CUENTA");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.CONCEPTO))
            {
                throw new ArgumentNullException("CONCEPTO");
            }

            if (String.IsNullOrEmpty(nuevaCorreccion.CODCONCEPTO))
            {
                throw new ArgumentNullException("CODCONCEPTO");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.VALOR))
            {
                throw new ArgumentNullException("VALOR");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.TOTAL))
            {
                throw new ArgumentNullException("TOTAL");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.EMPRESA))
            {
                throw new ArgumentNullException("ÈMPRESA");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.PERIODO))
            {
                throw new ArgumentNullException("PERIODO");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.FECHA))
            {
                throw new ArgumentNullException("FECHA");
            }

            if (String.IsNullOrEmpty(nuevaCorreccion.TOTDISNOM))
            {
                throw new ArgumentNullException("TOTDISNOM");
            }
            if (String.IsNullOrEmpty(nuevaCorreccion.TOTDISNOMIND))
            {
                throw new ArgumentNullException("TOTDISNOMIND");
            }


            using (var ctx = new AccountingContext())
            {
                ctx.DescuentoNomina.Add(nuevaCorreccion);
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

        public bool UpdateDisc(DescuentoNomina updateobjeto)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(updateobjeto.CEDULA))
            {
                throw new ArgumentNullException("CEDULA");
            }
            if (String.IsNullOrEmpty(updateobjeto.NOMBRE))
            {
                throw new ArgumentNullException("NOMBRE");
            }
            if (String.IsNullOrEmpty(updateobjeto.CUENTA))
            {
                throw new ArgumentNullException("CUENTA");
            }
            if (String.IsNullOrEmpty(updateobjeto.CONCEPTO))
            {
                throw new ArgumentNullException("CONCEPTO");
            }
            if (String.IsNullOrEmpty(updateobjeto.CODCONCEPTO))
            {
                throw new ArgumentNullException("CODCONCEPTO");
            }
            if (String.IsNullOrEmpty(updateobjeto.VALOR))
            {
                throw new ArgumentNullException("VALOR");
            }
            if (String.IsNullOrEmpty(updateobjeto.TOTAL))
            {
                throw new ArgumentNullException("TOTAL");
            }
            if (String.IsNullOrEmpty(updateobjeto.EMPRESA))
            {
                throw new ArgumentNullException("EMPRESA");
            }

            if (String.IsNullOrEmpty(updateobjeto.PERIODO))
            {
                throw new ArgumentNullException("PERIODO");
            }
            if (String.IsNullOrEmpty(updateobjeto.FECHA))
            {
                throw new ArgumentNullException("FECHA");
            }
            if (String.IsNullOrEmpty(updateobjeto.TOTDISNOM))
            {
                throw new ArgumentNullException("TOTDISNOM");
            }
            if (String.IsNullOrEmpty(updateobjeto.TOTDISNOMIND))
            {
                throw new ArgumentNullException("TOTDISNOMIND");
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

        public bool DeleteDisc(DescuentoNomina CorreccionDelete)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.DescuentoNomina.Find(CorreccionDelete.ID) != null)
                    {
                        ctx.DescuentoNomina.Remove(CorreccionDelete);
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
