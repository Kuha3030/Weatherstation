using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MVC_Sasema_test.Controllers
{
    public class FMIController : Controller
    {
        // GET: FMI
        public static string searchString = "Jämsä";

        public ActionResult Index()
        {

            // ### FMISearch has changed to use <List> instead of string! Below code is now commented.
            ViewBag.showLocation = searchString;
            List<string> FMIResponse = new List<string>();
            //Tuple<string, string> coordinates = GeoLocations.GetGeo(searchString);
            //string FMIcoordinates = coordinates.Item1 + "," + coordinates.Item2;
            //// # FMI Fetch (to xml document)
            DateTime sendLastModifiedFMI = TimeTools.GetLastModifiedFMI(searchString);

            //FMIResponse = FMISearch.GetFMI2(FMIcoordinates, sendLastModifiedFMI);

            Models3.FeatureCollection FMIxmlConverted = new Models3.FeatureCollection();

            string FMILoad = "";
            string FMIExpires = "";
            string FMILastModified = "";

            if (FMIResponse.Count > 1)
            {
                FMILoad = FMIResponse[0];
                FMIExpires = FMIResponse[1];
                FMILastModified = FMIResponse[2];
                FMIxmlConverted = FMISearch.ConvertXML2(FMILoad);

            }
            Models3.FeatureCollection FMIxmlConverted1 = new Models3.FeatureCollection();

            dynamic weatherModel = new ExpandoObject();
            weatherModel.FMIList1 = FMIxmlConverted.Member2[0];
            weatherModel.FMIList2 = FMIxmlConverted.Member2[1];
            weatherModel.FMIList3 = FMIxmlConverted.Member2[2];
            weatherModel.FMIList4 = FMIxmlConverted.Member2[3];


            //for (int i = 0; i < FMIxmlConverted.Member2.Count; i++)
            //{
            //    ViewBag.List[i]= FMIxmlConverted.Member2[i];
            //}

            //return View(myObject);
            return View(weatherModel);
        }
    }
}