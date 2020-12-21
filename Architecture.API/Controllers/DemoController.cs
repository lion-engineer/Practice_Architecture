using Architecture.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Architecture.API.Controllers
{
    [RoutePrefix("Demo")]
    public class DemoController : ApiController
    {
        [Route("TestDemo")]
        [HttpGet]
        public Adapter TestDemo() 
        {
            var result = new Adapter();
            result.ResCode = "00000";
            result.ResMsg = "Successful";
            return result;
        }
    }
}
