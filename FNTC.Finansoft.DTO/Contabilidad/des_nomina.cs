namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.des_nomina")]
    public partial class des_nomina
    {
        public int Id { get; set; }
        public string Nit { get; set; }
        public string Nombres { get; set; }
        public int? Numero_Cuota { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public int? Valor_Total { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public int? Capital { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public int? Interes_Corriente { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public int? Interes_Mora { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public int? Otros_Ingresos { get; set; }

        public string Documento { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Fecha { get; set; }
    }
}
