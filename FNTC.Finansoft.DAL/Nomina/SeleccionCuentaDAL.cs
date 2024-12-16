using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;
using System;
using System.Linq;
namespace FNTC.Finansoft.Accounting.DAL.Nomina
{
    public class SeccionCuentaDAL
    {
        public bool CreateSeleccionCuenta(SeleccionCuenta nuevoSeleccion)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(nuevoSeleccion.CODIGO))
            {
                throw new ArgumentNullException("CODIGO");
            }
            if (String.IsNullOrEmpty(nuevoSeleccion.TIPOCUENTA))
            {
                throw new ArgumentNullException("TIPOCUENTA");
            }

            //el codigo debe ser un numero 

            var codigo = 0;
            var esNumerico = Int32.TryParse(nuevoSeleccion.CODIGO, out codigo);

            if (esNumerico)
            {
                //debe ser subcuenta
                var digitosCuenta = codigo.ToString().Count();
                if (digitosCuenta <= 9) //una cuenta mayor es de maximo 6 digitos
                {

                    //es ok. pero esta validacion va en el modelo
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

                throw new ArgumentOutOfRangeException("CODIGO", "El codigo debe de ser de 9 digitos");
            }
            throw new ArgumentOutOfRangeException("CODIGO", "El codigo debe ser númerico ");

        }
        public bool UpdateSeleccionCuenta(SeleccionCuenta updateobjeto)
        {
            //el nombre debe existir

            if (String.IsNullOrEmpty(updateobjeto.CODIGO))
            {
                throw new ArgumentNullException("CODIGO");
            }
            if (String.IsNullOrEmpty(updateobjeto.TIPOCUENTA))
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
                throw new ArgumentOutOfRangeException("CODIGO", "El codigo no es correcto");
            }
            throw new ArgumentOutOfRangeException("CODIGO", "El codigo no es correcto");

        }

        public bool DeleteSeleccionCuenta(SeleccionCuenta descuentoDelete)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    //si existe
                    if (ctx.SeleccionCuenta.Find(descuentoDelete.CODIGO) != null)
                    {
                        ctx.SeleccionCuenta.Remove(descuentoDelete);
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
