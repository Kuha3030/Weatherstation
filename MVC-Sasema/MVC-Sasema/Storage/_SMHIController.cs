using MVC_Sasema_test.Models;
using MVC_Sasema_test.Models2;
using MVC_Sasema_test.Models3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MVC_Sasema_test.Controllers
{
    public class SMHIController : Controller
    {
        public static string searchString = "Tampere";
        private static weatherstationEntities db = new weatherstationEntities();

        // GET: SMHI
        public ActionResult Index()
        {

            StreamWriter sw = new StreamWriter(Server.MapPath("/json/SMHIdump.json"));




            // # Fetch coordinates from database using getGeo method
            //Tuple<string, string> coordinates = GeoLocations.GetGeo(searchString);

            // # (only for the View)
            ViewBag.showLocation = searchString;

            // # Fetch JSON file from yrno API using yrnoFetch() method
            //var SMHIDump = SMHISearch.SMHIFetch(coordinates);

            //sw.Write(SMHIDump);
            //sw.Close();
            // # Transfer data from JSON to object
            //Models.YrnoObject jsonConverted = JsonConvert.DeserializeObject<Models.YrnoObject>(yrnoDump);
            //dynamic weatherModel = new ExpandoObject();
            //weatherModel.SMHIObject = JsonConvert.DeserializeObject<Models2.SMHIObject>(SMHIDump);

            //List<string> tempList = new List<string>();
            //foreach (var item in SMHIDump)
            //{
            //    tempList.Add(Convert.ToString(item));

            //}

            //ViewBag.tempList = tempList;
            //sw.Write(SMHIDump);
            //sw.Close();
            //weatherModel.SMHIParameters = parameters.ToList();
            //Models2.SMHIObject jsonConverted = JsonConvert.DeserializeObject<Models2.SMHIObject>(SMHIDump);

            // # Return view
            return View();
        }
    }
}