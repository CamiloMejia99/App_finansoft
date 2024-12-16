using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DAL;
using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.BLL.Terceros
{
    public class TercerosBLL
    {
        TercerosDAL dalConfig = new TercerosDAL();
        /// <summary>
        /// Get All Terceros
        /// </summary>
        public List<TerceroDTO> Terceros { get { return new DAL.TercerosDAL().Terceros; } }

        public void Test()
        {
            //.Terceros.AsociadosContext ctx = new DAL.Model.Terceros.AsociadosContext();
            //ctx.Abogados.Add( new Abogado());

            //ModeloEntities ctx2 = new ModeloEntities();
            //var jugfado  = ctx2.Juzgados.Find(1);


        }
        public string ObtenerNivel(string nit)
        {
            return dalConfig.ObtenerNivel(nit);
        }
        public string ObtenerNivelNombre(string nivelSelecccionado)
        {
            return dalConfig.ObtenerNivelNombre(nivelSelecccionado);
        }
        public string ObtenerCargoNombre(int cargoNom)
        {
            return dalConfig.ObtenerCargoNombre(cargoNom);
        }
        public int ObtenerCargo(string nit)
        {
            return dalConfig.ObtenerCargo(nit);
        } 
        public TerceroDTO GetTerceroByNit(string nit)
        {

            //if nit empty
            return new DAL.TercerosDAL().GetTerceroByNIT(nit);
        }
    }
}
