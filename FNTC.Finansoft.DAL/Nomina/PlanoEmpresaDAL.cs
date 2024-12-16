using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;

namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class PlanoEmpresaDAL
    {
        public bool CreatePlanoEmpresa(PlanoEmpresa nuevoPlano)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(nuevoPlano.CODIGOEMP))
            {
                throw new ArgumentNullException("CODIGO_EMPRESA");
            }
            if (String.IsNullOrEmpty(nuevoPlano.NOMBREMP))
            {
                throw new ArgumentNullException("NOMBRE_EMPRESA");
            }



            using (var ctx = new AccountingContext())
            {

                try
                {
                    //nuevoSobrante.NAT = (char)nuevoSobrante.NAT;
                    return ctx.SaveChanges() > 0 ? true : false;
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }
        public bool UpdatePlanoCuenta(PlanoEmpresa updateobjeto)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(updateobjeto.CODIGOEMP))
            {
                throw new ArgumentNullException("CODIGO_EMPRESA");
            }
            if (String.IsNullOrEmpty(updateobjeto.NOMBREMP))
            {
                throw new ArgumentNullException("NOMBRE_EMPRESA");
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

        public bool DeletePlanoEmpresa(PlanoEmpresa planoDelete)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.SeleccionCuenta.Find(planoDelete.CODIGOEMP) != null)
                    {
                        ctx.PlanoEmpresa.Remove(planoDelete);
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
