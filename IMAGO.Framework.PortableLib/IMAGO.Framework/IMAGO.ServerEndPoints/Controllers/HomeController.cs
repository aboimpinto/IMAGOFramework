using IMAGO.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMAGO.ServerEndPoints.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public JsonResult Index()
        {
            List<EndPointDefinition> lstRET = new List<EndPointDefinition>();
            lstRET.Add(new EndPointDefinition() { Contract = "IAuthenticationService", Service = "AuthenticationService.svc" });

            return Json(lstRET, "text/plain", JsonRequestBehavior.AllowGet);
        }

    }
}
