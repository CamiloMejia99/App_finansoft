using FNTC.Finansoft.Accounting.DTO.Parametros;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.FactOpcaja")]
    public class FactOpcaja
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string factura { get; set; }
        public string operacion { get; set; }
        [ForeignKey("Caja")]
        public string codigo_caja { get; set; }
        [ForeignKey("Terceros")]
        public string nit_cajero { get; set; }

        [ForeignKey("ProductoFK")]
        public int IdProducto { get; set; }
        public string numero_cuenta { get; set; }
        [ForeignKey("terceroFK")]
        public string nit_propietario_cuenta { get; set; }
        public string nombre_propietario_cuenta { get; set; }
        public decimal valor_recibido { get; set; }
        public decimal valor_efectivo { get; set; }
        public decimal vueltas { get; set; }
        public decimal valor_cheque { get; set; }
        public string numero_cheque { get; set; }
        public decimal consignacion { get; set; }
        public string observacion { get; set; }
        public decimal saldo_total_cuenta { get; set; }
        public decimal total { get; set; }
        [ForeignKey("CodigosBanco")]
        public string nit_consignacion { get; set; }
        public decimal valor_cheque1 { get; set; }
        public string numero_cheque1 { get; set; }
        [ForeignKey("CodigosBanco1")]
        public string nit_consignacion1 { get; set; }
        public decimal valor_cheque2 { get; set; }
        public string numero_cheque2 { get; set; }
        [ForeignKey("CodigosBanco2")]
        public string nit_consignacion2 { get; set; }
        public decimal valor_cheque3 { get; set; }
        public string numero_cheque3 { get; set; }
        [ForeignKey("CodigosBanco3")]
        public string nit_consignacion3 { get; set; }
        public decimal valor_cheque4 { get; set; }
        public string numero_cheque4 { get; set; }
        [ForeignKey("CodigosBanco4")]
        public string nit_consignacion4 { get; set; }
        public decimal valor_cheque5 { get; set; }
        public string numero_cheque5 { get; set; }
        [ForeignKey("CodigosBanco5")]
        public string nit_consignacion5 { get; set; }
        public decimal total_cheques { get; set; }

        public string TIPO { get; set; }
        public string NUMERO { get; set; }

        public virtual Caja Caja { get; set; }
        public virtual CodigosBanco CodigosBanco { get; set; }
        public virtual CodigosBanco CodigosBanco1 { get; set; }
        public virtual CodigosBanco CodigosBanco2 { get; set; }
        public virtual CodigosBanco CodigosBanco3 { get; set; }
        public virtual CodigosBanco CodigosBanco4 { get; set; }
        public virtual CodigosBanco CodigosBanco5 { get; set; }
        public virtual configCajero configCajero { get; set; }

        public virtual Tercero Terceros { get; set; }
        public virtual Tercero terceroFK { get; set; }
        public virtual TipoProducto ProductoFK { get; set; }
    }
}