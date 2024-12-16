using System;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{

    public class AdministradoraDTO
    {
        public string Nombre { get; set; }
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
