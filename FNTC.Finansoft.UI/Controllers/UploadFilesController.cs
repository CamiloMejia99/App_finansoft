﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FNTC.Finansoft.UI.Controllers
{
    public class UploadFilesController : ApiController
    {
        // GET: api/UploadFiles
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UploadFiles/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UploadFiles
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UploadFiles/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UploadFiles/5
        public void Delete(int id)
        {
        }
    }
}
