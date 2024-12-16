namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ViewModelPrestaTerceros
    {
        public Prestamos Prestamos { get; set; }
        public int id { get; set; }
        public string Pagare { get; set; }
        public long Capital { get; set; }
        public string Fecha_Prestamo { get; set; }
        public decimal Plazo { get; set; }
        public decimal Interes { get; set; }
        //public TercerosPRB Terceros { get; set; }
        public string NIT { get; set; }
        public string NOMBRE { get; set; }
        public bool estado { get; set; }
    }
}