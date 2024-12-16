using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Respuesta
{
    public class DTORespuestaCuentasByProducto
    {
        public bool Correcto { get; set; }
        public int IdProducto { get; set; }

        public DTORespuestaCuentasByProducto GenerarRespuestaCorrecta(int IdProducto)
        {
            var respuesta = new DTORespuestaCuentasByProducto
            {
                Correcto = true,
                IdProducto = IdProducto
            };
            return respuesta;
        }

        public DTORespuestaCuentasByProducto GenerarRespuestaIncorrecta()
        {
            var respuesta = new DTORespuestaCuentasByProducto
            {
                Correcto = false,
                IdProducto=0
            };
            return respuesta;
        }
    }
}
