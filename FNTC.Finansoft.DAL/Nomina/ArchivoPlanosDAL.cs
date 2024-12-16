
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;
using System.Linq;


namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class ArchivoPlanosDAL
    {

        public bool CreateArchivosPlano(ArchivoPlano nuevoPlano)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(nuevoPlano.CONCEPTO))
            {
                throw new ArgumentNullException("CONCEPTO");
            }
            if (String.IsNullOrEmpty(nuevoPlano.TIPDATO))
            {
                throw new ArgumentNullException("TIPDATO");
            }
            if (String.IsNullOrEmpty(nuevoPlano.LONGITUD))
            {
                throw new ArgumentNullException("LONGITUD");
            }
            if (String.IsNullOrEmpty(nuevoPlano.ALINEACION))
            {
                throw new ArgumentNullException("ALINEACION");
            }
            if (String.IsNullOrEmpty(nuevoPlano.RELLENO))
            {
                throw new ArgumentNullException("RELLENO");
            }
            if (String.IsNullOrEmpty(nuevoPlano.VALPREDETERINADO))
            {
                throw new ArgumentNullException("VALPREDETERINADO");

            }


            using (var ctx = new AccountingContext())
            {
                var orden = 0;
                try
                {
                    orden = ctx.ArchivoPlano.Max(p => p.ORDEN);
                }
                catch (Exception e)
                {
                    orden = 0;
                }
                orden++;
                nuevoPlano.ORDEN = orden;

                ctx.ArchivoPlano.Add(nuevoPlano);
                try
                {
                    //nuevaCuenta.NAT = (char)nuevaCuenta.NAT;
                    return ctx.SaveChanges() > 0 ? true : false;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public bool UpdateArchivoPlano(ArchivoPlano updateobjeto)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(updateobjeto.CONCEPTO))
            {
                throw new ArgumentNullException("CONCEPTO");
            }
            if (String.IsNullOrEmpty(updateobjeto.TIPDATO))
            {
                throw new ArgumentNullException("TIPDATO");
            }
            if (String.IsNullOrEmpty(updateobjeto.LONGITUD))
            {
                throw new ArgumentNullException("LONGITUD");
            }
            if (String.IsNullOrEmpty(updateobjeto.ALINEACION))
            {
                throw new ArgumentNullException("ALINEACION");
            }
            if (String.IsNullOrEmpty(updateobjeto.RELLENO))
            {
                throw new ArgumentNullException("RELLENO");
            }
            if (String.IsNullOrEmpty(updateobjeto.VALPREDETERINADO))
            {
                throw new ArgumentNullException("VALPREDETERINADO");

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

        public bool DeleteArchivosPlano(ArchivoPlano descuentoDelete)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.ArchivoPlano.Find(descuentoDelete.ID) != null)
                    {
                        ctx.ArchivoPlano.Remove(descuentoDelete);
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