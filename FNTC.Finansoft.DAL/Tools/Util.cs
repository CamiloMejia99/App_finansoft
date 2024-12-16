using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DAL.Tools
{
    public class Util
    {
        public string ObtenerConsecutivoString(string consecutivo)
        {
            string cadena = "";
            var tamanio = 10-consecutivo.Length;
            for (int i = 1; i <= tamanio; i++)
            {
                cadena += "0";
            }
            cadena += consecutivo;

            return cadena;
        }
    }
}
