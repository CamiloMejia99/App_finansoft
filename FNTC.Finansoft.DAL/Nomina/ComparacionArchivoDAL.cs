using System;

namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("nom.ComparacionArchivos")]
    public class ComparacionArchivoDAL
    {

        public bool CreateCom(ComparacionArchivo nuevaComparacion)
        {
            //el nombre debe existir
            if (String.IsNullOrEmpty(nuevaComparacion.EMPRESA))
            {
                throw new ArgumentNullException("EMPRESA");
            }
            if (String.IsNullOrEmpty(nuevaComparacion.PERDEDUCC))
            {
                throw new ArgumentNullException("PERDEDUCC");
            }

            if (String.IsNullOrEmpty(nuevaComparacion.ORDEND))
            {
                throw new ArgumentNullException("ORDEND");
            }

            using (var ctx = new AccountingContext())
            {
                ctx.ComparacionArchivo.Add(nuevaComparacion);
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

        public bool UpdateDisc(ComparacionArchivo updateobjeto)
        {
            //el nombre debe existir
            if (String.IsNullOrEmpty(updateobjeto.EMPRESA))
            {
                throw new ArgumentNullException("EMPRESA");
            }
            if (String.IsNullOrEmpty(updateobjeto.PERDEDUCC))
            {
                throw new ArgumentNullException("PERDEDUCC");
            }
            if (String.IsNullOrEmpty(updateobjeto.CAMBIO))
            {
                throw new ArgumentNullException("CAMBIO");
            }
            if (String.IsNullOrEmpty(updateobjeto.ORDEND))
            {
                throw new ArgumentNullException("ORDEND");
            }
            if (String.IsNullOrEmpty(updateobjeto.PLANO))
            {
                throw new ArgumentNullException("PLANO");
            }
            if (String.IsNullOrEmpty(updateobjeto.RUTA))
            {
                throw new ArgumentNullException("RUTA");
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

        public bool DeleteDisc(ComparacionArchivo ComparacionDelete)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.ComparacionArchivo.Find(ComparacionDelete.ID) != null)
                    {
                        ctx.ComparacionArchivo.Remove(ComparacionDelete);
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
