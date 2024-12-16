using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MediosMagneticos
{
    public class InformacionExogena
    {
        public static int CommandTimeout { get; set; }
        [Key]
        public string CUENTA { get; set; }
        public string NOMBRECUENTA { get; set; }
        public string Nit { get; set; }
        public string TIPODOCUMENTO { get; set; }
        public string NOMBRE1 { get; set; }
        public string NOMBRE2 { get; set; }
        public string APELLIDO1 { get; set; }
        public string APELLIDO2 { get; set; }
        public decimal DEBITO { get; set; }
        public decimal CREDITO { get; set; }
        public decimal BASE { get; set; }
        public DateTime FECHAMOVIMIENTO { get; set; }
        public string EMAIL { get; set; }
        public int Id_pais { get; set; }
        public int Dep_muni { get; set; }
        public int MUNICIPIO { get; set; }
        public string  Dir { get; set; }
        public string NATURALEZA { get; set; }
        public string  DIGVER { get; set; }
        public string NombreComercial { get; set; }



    }
}




