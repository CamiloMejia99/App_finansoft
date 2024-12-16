using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ViewModelCreditos
    {
        public Prestamos Prestamos { get; set; }

        public int id { get; set; }
        [Display(Name = "Capital")]
        public long Capital { get; set; }
        [Display(Name = "Interes")]
        public decimal Interes { get; set; }
        [Display(Name = "Plazo")]
        public decimal Plazo { get; set; }
        [Display(Name = "Fecha Prestamo")]
        public string Fecha_Prestamo { get; set; }

        public Tipo_Periodo Tipo_Periodo { get; set; }
        [Display(Name = "Valor Periodo")]
        public decimal Tipo_Periodo_Valor { get; set; }

        public Costos_Adicionales Costos_Adicionales { get; set; }
        [Display(Name = "Valor")]
        public string CA_Valor { get; set; }
        [Display(Name = "Porcentaje")]
        public string CA_Porcentaje { get; set; }

        public int myselect { get; set; }
        public decimal ValorPeriodo { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorSeguroPorcentaje { get; set; }
        public int ValDiasInt { get; set; }
        public int difdias { get; set; }
        public string fechadesembolso { get; set; }
        public string nombreEmpresa { get; set; }
        public string nitEmpresa { get; set; }
        public string nitTercero { get; set; }
        public string nombreTercero { get; set; }
        public string celularTercero { get; set; }
        public string linea { get; set; }
        public string destino { get; set; }

        public string garatiaId { get; set; }
        public int realValor { get; set; }
        public string pagare { get; set; }
        public int codeudorNit { get; set; }
        public string nombreCodeudor { get; set; }

        public decimal costoAdicionalEnEltiempo { get; set; }
        public decimal costoAdicionalAnticipado { get; set; }
        public decimal costoAdicionalPrimeraCuota { get; set; }
        public decimal costoAdicionalDividoEnElTiempo { get; set; }
        public decimal ValorPorcentajeCostoAnticipado { get; set; }
        public decimal ValorPorcentajeCostoEnCadaCuota { get; set; }
    }
}