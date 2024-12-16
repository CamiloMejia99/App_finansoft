using System;

namespace FNTC.Finansoft.DTO.Aportes
{
    public class DTOAfiliadosAportes
    {
        public string nit { get; set; }
        public string nombres { get; set; }
        public string empresa { get; set; }
        public string dependencia { get; set; }

        //cuenta Aportes
        public int id { get; set; }
        public string numeroCuenta { get; set; }
        public string idPersona { get; set; }
        public string tipoPago { get; set; }
        public string porcentaje { get; set; }
        public string valor { get; set; }
        public string valorCuota { get; set; }
        public string totalAportes { get; set; }
        public Nullable<System.DateTime> fechaApertura { get; set; }
        public Nullable<bool> activa { get; set; }

        //configuracion Aportes
        public Nullable<int> idTipoCuotaCalculo { get; set; }
    }
}
