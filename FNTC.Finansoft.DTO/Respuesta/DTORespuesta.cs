using System.Collections;

namespace FNTC.Finansoft.DTO.Respuestas
{
    public class DTORespuesta
    {
        public bool Correcto { get; set; }
        public string Mensaje { get; set; }
        public IDictionary Exepciones { get; set; }

        public DTORespuesta GenerarRespuestaBasica(bool tipo)
        {
            var respuesta = new DTORespuesta
            {
                Correcto = tipo,
                Mensaje = tipo ? "La operacion solicitada se realizo correctamente." : "Error al realizar la operacion solicitada. Si el problema persiste, comunícate con soporte técnico."
            };
            return respuesta;
        }


    }
}
