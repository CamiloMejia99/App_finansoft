namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    public static class NaturalezaCuenta
    {
        public static string Credito { get { return "C"; } }
        public static string Debito { get { return "D"; } }
    }

#warning Cuando empieze a crear otros comprobantes esto debe migrar a una tabla
    //public enum ClaseComprobante
    //{
    //    NC, //nota contabilidad
    //    CE, //comrpbante egreso
    //    RC, //recibo de caja
    //    TC, //traslado cuentas
    //    SI  //saldos iniciales
    //}
}
