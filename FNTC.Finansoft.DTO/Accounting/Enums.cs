namespace FNTC.Finansoft.Accounting.DTO.Accounting
{
    public static class NaturalezaCuenta
    {
        public static string Credito { get { return "C"; } }
        public static string Debito { get { return "D"; } }
    }

    public enum ClaseComprobante
    {
        NC, //nota contabilidad
        CE, //comrpbante egreso
        RC, //recibo de caja
        TC, //traslado cuentas
        SI  //saldos iniciales
    }
}
