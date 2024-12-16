using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ViewModelCostos
    {
        public Costos_Adicionales Costos_Adicionales { get; set; }
        public int CA_Id { get; set; }
        public long Cuenta_Cod { get; set; }
        public string CA_Nombre { get; set; }
        public string CA_Valor { get; set; }
        public string CA_Porcentaje { get; set; }
        public bool CA_estado { get; set; }

        public Destinos Destinos { get; set; }
        [Display(Name = "Destinos")]
        public string Destino_Descripcion { get; set; }

        public Lineas Lineas { get; set; }
        [Display(Name = "Lineas")]
        public string Lineas_Descripcion { get; set; }

        public Tipo_Costo Tipo_Costo { get; set; }

        public string Tipo_Costo_Descripcion { get; set; }
    }
}