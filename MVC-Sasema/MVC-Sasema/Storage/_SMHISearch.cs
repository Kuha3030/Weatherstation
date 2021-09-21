using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MVC_Sasema_test.Controllers
{
    public class SMHISearch
    {

        public static string SMHIFetch(Tuple<string, string> coordinates)
        {
            //string searchDate = DateTime.Now.ToString("yyyy-MM-dd");
            //string searchTime = DateTime.Now.ToString("HH:mm:ss");
            //searchTime = searchTime.Replace(".", ":");

            //string searchDateEnd = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd");
            //Basic url for getting latest data for coordinates lat=60.1&lon=24.9 (Helsinki)
            string url = $"https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/{coordinates.Item2}/lat/{coordinates.Item1}/data.json";//Paste ur url here  
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //Create WebRequest handler.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //UserAgent identification (needed by the response server)
            //request.UserAgent = "Weatherstation";

            //Declare variables outside try/catch to be able to read them outside try.
            string responseText = "";
            string responseText_deserialized = "";

            //Get the response (data hopefully).
            try
            {

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseText = reader.ReadToEnd();


                //Deserialize the output (from singleline to multiline)
                responseText_deserialized = JsonConvert.DeserializeObject(responseText).ToString();
                //StreamWriter writer = new StreamWriter("/json/yrnodump.json");
                //writer.Write(responseText_deserialized);


            }
            catch (Exception e)
            {
                throw e;
            }

            //Print out the response

            return responseText;

        }
    }
}