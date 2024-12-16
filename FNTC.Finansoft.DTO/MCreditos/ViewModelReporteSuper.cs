namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ViewModelReporteSuper
    {
        //public Terceros Terceros { get; set; }
        public string NIT { get; set; }
        public Prestamos Prestamos { get; set; }
        public int id { get; set; }
        public string Pagare { get; set; }
        public string Fecha_Prestamo { get; set; }
        public long Capital { get; set; }
        public decimal Interes { get; set; }
        public decimal Plazo { get; set; }
        public Forma_Pago Forma_Pago { get; set; }
        public int Forma_Pago_Id { get; set; }
        public Lineas Lineas { get; set; }
        public string Lineas_Codigo { get; set; }
        public Destinos Destinos { get; set; }
        public string Destino_Codigo { get; set; }
        public SubDestinos Subdestinos { get; set; }
        public string Subdestino_Codigo { get; set; }
        public Real Real { get; set; }
        public int Real_Id { get; set; }
        public long Real_Valor { get; set; }
        public Codigo_Operador Codigo_Operador { get; set; }
        public string Codigo_Operador_Descripcion { get; set; }
    }
}
