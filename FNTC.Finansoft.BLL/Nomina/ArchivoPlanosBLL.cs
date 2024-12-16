using FNTC.Finansoft.Accounting.DTO.Nomina;
using System.Collections.Generic;
using System.Linq;



namespace FNTC.Finansoft.Accounting.BLL.ArchivoPlanos
{
    public class ArchivoPlanosBLL
    {
        private DAL.Nomina.ArchivoPlanosDAL dal;
        //crud de clase de plano

        public ArchivoPlanosBLL()
        {
            dal = new DAL.Nomina.ArchivoPlanosDAL();
        }

        public List<ArchivoPlano> GetArchivoPlanos()
        {
            using (var ctx = new FNTC.Finansoft.Accounting.DTO.AccountingContext())
            {
                List<ArchivoPlano> lista = ctx.ArchivoPlano.ToList();
                return lista;

                //filtrado
                /* var comprobantes = ctx.ClaseDePlano.ToList();
                 return comprobantes;*/
            }
        }

    }
}
