using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.SIAR
{
    public class ViewModelPerdidaAcumulada
    {
        public int id { get; set; }
        public string rango { get; set; }

        public string calificacion { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0%}")]
        public string PI { get; set; }
        public string EA { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0%}")]
        public string PDI { get; set; }
        public string PE { get; set; }
        public string PEA { get; set; }

    }
}
