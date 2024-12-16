using System.Collections.Generic;
using System.Web.Http;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.Api.Movimientos
{
    public class MovimientosController : ApiController
    {
        
        // GET: api/MovimientosApi
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MovimientosApi/5
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MovimientosApi
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/MovimientosApi/5
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MovimientosApi/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
