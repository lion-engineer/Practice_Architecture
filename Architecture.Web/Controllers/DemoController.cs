using Architecture.Common.Model;
using Architecture.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Architecture.Web.Controllers
{
    public class DemoController : Controller
    {
        public DemoController()
        {
            this._httpClientSample = new HttpClientSample();
        }

        private HttpClientSample _httpClientSample;

        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DemoHttpClient() 
        {
            var result = new Adapter();
            var taskResult = this._httpClientSample.testhttpclient();
            if (!taskResult.IsFaulted)
            {
                result = taskResult.Result;
            }
            else 
            {
                result.ResCode = "99999";
                result.ResMsg = taskResult.Exception.Message.ToString();
            }
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}