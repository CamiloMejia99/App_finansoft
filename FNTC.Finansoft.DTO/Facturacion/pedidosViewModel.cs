namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    public class pedidosViewModel
    {
        public int cod { get; set; }
        public int cantidad { get; set; }
        public string nombre { get; set; }
        public string iva { get; set; }
        public decimal unidad { get; set; }
        public decimal total { get; set; }
        public int operatioId { get; set; }
    }
}