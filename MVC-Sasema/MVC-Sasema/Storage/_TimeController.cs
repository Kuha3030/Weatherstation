using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Sasema_test.Controllers
{
    public class TimeController : Controller
    {
        // GET: Time
        public ActionResult Index()
        {

            string timeExample = "Wed, 21 Jul 2021 07:54:50 GMT";
            ViewBag.Time = TimeTools.GMTStringToDateTime(timeExample);
            ViewBag.Time2 = TimeTools.DateTimeToRFC1123(ViewBag.Time);

            return View();
        }
    }
}