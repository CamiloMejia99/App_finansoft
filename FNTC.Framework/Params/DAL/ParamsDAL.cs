using FNTC.Framework.Paras.DAL;
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Framework.Params.DAL
{
    public class ParamsDAL
    {

        ParametersModel ctx;
        public ParamsDAL()
        {

        }


        public List<Parameter> GetParamValues(string paramName)
        {
            using (ctx = new ParametersModel())
            {
                var values = ctx.Parametros
                    .Where(x => x.NombreParametro.Equals(paramName))
                    .ToList();
                return values;
            }
        }

        public List<Parameter> GetParameters(string paramName = "")
        {
            using (ctx = new ParametersModel())
            {
                var values = ctx.Parametros
                    .Where(x => x.NombreParametro.Equals(paramName))
                    .ToList();
                return values;
            }

        }
    }
}
