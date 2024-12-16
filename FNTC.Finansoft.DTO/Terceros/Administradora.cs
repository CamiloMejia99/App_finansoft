using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    public class Administradora
    {

        [Required]
        public bool EsAdministradora { get; set; }
        public string CodigoAdministradora { get; set; }

        public Nullable<bool> EsEPS { get; set; }
        public Nullable<bool> EsAFP { get; set; }
        public Nullable<bool> EsARP { get; set; }
        public Nullable<bool> EsCesantia { get; set; }
        public Nullable<bool> EsCCF { get; set; }
        public Nullable<bool> EsParafiscal { get; set; }
    }
}
