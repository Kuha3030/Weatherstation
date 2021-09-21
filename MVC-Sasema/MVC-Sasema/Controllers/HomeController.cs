using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVC_Sasema_test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string userIP = Request.UserHostAddress;
            string userID = IPHashing.GetHashString(userIP);
            
            Session["UserID"] = userID;
            ViewBag.userID = userID;
            ViewBag.userIP = userIP;

            return View();
        }

        public ActionResult Info()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



    }
}
