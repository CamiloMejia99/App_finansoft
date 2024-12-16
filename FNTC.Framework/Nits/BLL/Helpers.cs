using System;

namespace FNTC.Framework.NITS
{
    public static class Helpers
    {
        /// <summary>
        /// Calcula el Digito de Verificacion, basado en el nit introducido. 
        /// Orden Administrativa # 4 del 27 de Octubre de 1989 DIAN - COLOMBIA
        /// </summary>
        /// <param name="nit">El NIT como String</param>
        /// <returns>el digito de verificacion</returns>
        public static string CalcularDigitoVerificacion_NIT(string Nit = "")
        {
            if (Nit.Length == 0) return String.Empty;

            string Temp;
            int Contador;
            int Residuo;
            int Acumulador;
            int[] Vector = new int[15];

            Vector[0] = 3;
            Vector[1] = 7;
            Vector[2] = 13;
            Vector[3] = 17;
            Vector[4] = 19;
            Vector[5] = 23;
            Vector[6] = 29;
            Vector[7] = 37;
            Vector[8] = 41;
            Vector[9] = 43;
            Vector[10] = 47;
            Vector[11] = 53;
            Vector[12] = 59;
            Vector[13] = 67;
            Vector[14] = 71;

            Acumulador = 0;

            Residuo = 0;

            for (Contador = 0; Contador < Nit.Length; Contador++)
            {
                Temp = Nit[(Nit.Length - 1) - Contador].ToString();
                Acumulador = Acumulador + (Convert.ToInt32(Temp) * Vector[Contador]);
            }

            Residuo = Acumulador % 11;

            return Residuo > 1 ? Convert.ToString(11 - Residuo) : Residuo.ToString();

        }

        /// <summary>
        /// Obtiene la razon social, basada en el numero de NIT, sin digito de verificacion
        /// </summary>
        /// <param name="nit"></param>
        /// <remarks >La Razon Social</remarks>
        public static String RazonSocialby_NIT(String nit)
        {
            //https://muisca.dian.gov.co/WebRutMuisca/DefConsultaEstadoRUT.faces
            return String.Empty;
        }
    }
}
