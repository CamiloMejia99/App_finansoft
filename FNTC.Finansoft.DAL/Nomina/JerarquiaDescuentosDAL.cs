
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class JerarquiaDescuentosDAL
    {
        public bool CreateJerarquiaDescuento(JerarquiaDescuento nuevaJerarquia)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(nuevaJerarquia.CODIGO))
            {
                throw new ArgumentNullException("CODIGO");
            }

            //el codigo debe ser un numero 

            var codigo = 0;
            var esNumerico = Int32.TryParse(nuevaJerarquia.CODIGO, out codigo);

            if (esNumerico)
            {
                //debe ser subcuenta
                var digitosCuenta = codigo.ToString().Count();
                if (digitosCuenta <= 9) //una cuenta mayor es de maximo 6 digitos
                {

                    //es ok. pero esta validacion va en el modelo
                    using (var ctx = new AccountingContext())
                    {

                        var orden = 0;
                        try
                        {
                            orden = ctx.JerarquiaDescuento.Max(p => p.ORDEN);
                        }
                        catch (Exception e)
                        {
                            orden = 0;
                        }
                        orden++;
                        nuevaJerarquia.ORDEN = (short)orden;

                        ctx.JerarquiaDescuento.Add(nuevaJerarquia);
                        try
                        {
                            //nuevaJerarquia.NAT = (char)nuevaJerarquia.NAT;
                            return ctx.SaveChanges() > 0 ? true : false;
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }

                    throw new ArgumentOutOfRangeException("ORDEN", "El Orden debe ser mayor que cero");
                }

                throw new ArgumentOutOfRangeException("CODIGO", "El codigo debe de ser de 9 digitos");
            }
            throw new ArgumentOutOfRangeException("CODIGO", "El codigo debe ser númerico ");

        }

        public bool UpdateJerarquiaDescuento(JerarquiaDescuento updateobjeto)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(updateobjeto.CODIGO))
            {
                throw new ArgumentNullException("CODIGO");
            }

            //el codigo debe ser un numero 

            var codigo = 0;
            var esNumerico = Int32.TryParse(updateobjeto.CODIGO, out codigo);

            if (esNumerico)
            {
                //debe ser subcuenta
                var digitosCuenta = codigo.ToString().Count();
                if (digitosCuenta > 6)
                {
                    //es ok. pero esta validacion va en el modelo
                    if (updateobjeto.ORDEN > 0)
                    {
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
                    throw new ArgumentOutOfRangeException("ORDEN", "El Orden debe ser mayor que cero");
                }
                throw new ArgumentOutOfRangeException("CODIGO", "El codigo no es correcto");
            }
            throw new ArgumentOutOfRangeException("CODIGO", "El codigo no es correcto");

        }

        public bool DeleteJerarquiaDescuento(JerarquiaDescuento descuentoDelete)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.JerarquiaDescuento.Find(descuentoDelete.CODIGO) != null)
                    {
                        ctx.JerarquiaDescuento.Remove(descuentoDelete);
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
