using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MVC_Sasema_test.Controllers
{
    public class YrnoSearch
    {
        //public static List<string> yrnoFetch(Tuple<string, string> coordinates, string lastModified)
        //{
        //    List<string> responseList = new List<string>();
        //    //Basic url for getting latest data for coordinates lat=60.1&lon=24.9 (Helsinki)
        //    string url = "https://api.met.no/weatherapi/locationforecast/2.0/complete.json?lat=" + coordinates.Item1 + "&lon=" + coordinates.Item2;//Paste ur url here  
        //    //string url = "https://api.met.no/weatherapi/locationforecast/2.0/classic.xml?lat=" + coordinates.Item1 + "&lon=" + coordinates.Item2;//Paste ur url here  
        //    var sitename = "Weatherstation / School project @ Careeria (https://www.careeria.fi/) / weatherstationcareeria@gmail.com";


        //    //Create WebRequest handler.
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        //    if (lastModified != null)
        //    {
        //        DateTime IfModified = Convert.ToDateTime(lastModified);
        //        IfModified.AddHours(-3);
        //        request.IfModifiedSince = IfModified;
        //    }

        //    //UserAgent identification (needed by the response server)
        //    request.UserAgent = sitename;

        //    //Declare variables outside try/catch to be able to read them outside try.
        //    //string responseText_deserialized = "";
        //    //var response2 = "";


        //    //StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("/txt/HttpRequestinfo.txt"), true);

        //    Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
        //    //Get the response (data hopefully).
        //    try
        //    {
        //        string responseText = "";
        //        WebResponse response = request.GetResponse();
        //        StreamReader reader = new StreamReader(response.GetResponseStream());
        //        responseText = reader.ReadToEnd();
        //        responseList.Add(responseText);


        //        WebHeaderCollection header = response.Headers;
        //        for (int i = 0; i < header.Count; i++)
        //        {
        //            requestHeaders.Add(header.GetKey(i), header[i]);
        //            if (header.GetKey(i) == "Expires")
        //            {
        //                responseList.Add(header[i]);
        //            }
        //            if (header.GetKey(i) == "Last-Modified")
        //            {
        //                responseList.Add(header[i]);
        //            }
        //        }
        //        //sw.Write("\n----------------------------------------------");
        //        //foreach (var item in requestHeaders)
        //        //{
        //        //    sw.Write("\n" + item.Key + ": " + item.Value);
        //        //}
        //        //Deserialize the output(from singleline to multiline)
        //        //responseText_deserialized = JsonConvert.DeserializeObject(responseText).ToString();
        //        //response2 = new WebClient().DownloadString(url);




        //    }
        //    catch (Exception e)
        //    {
        //        responseList.Add(e.ToString());
        //    }


        //    // # Get all response headers! This needs to be implemented into class



        //    //sw.Close();

        //    //return responseText;
        //    //Print out the response
        //    return responseList;

        //}


        public static List<string> yrnoFetch2(Tuple<string, string> coordinates, DateTime lastModified)
        {
            List<string> responseList = new List<string>();
            //Basic url for getting latest data for coordinates lat=60.1&lon=24.9 (Helsinki)
            string url = "https://api.met.no/weatherapi/locationforecast/2.0/classic.xml?lat=" + coordinates.Item1 + "&lon=" + coordinates.Item2;//Paste ur url here  
            var sitename = "Weatherstation / School project @ Careeria (https://www.careeria.fi/) / weatherstationcareeria@gmail.com";


            //Create WebRequest handler.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (lastModified != null)
            {
                request.IfModifiedSince = lastModified;
            }

            //UserAgent identification (needed by the response server)
            request.UserAgent = sitename;


            //Get the response (data hopefully).
            try
            {
                string responseText = "";
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseText = reader.ReadToEnd();
                responseList.Add(responseText);


                WebHeaderCollection header = response.Headers;
                for (int i = 0; i < header.Count; i++)
                {
                    if (header.GetKey(i) == "Expires")
                    {
                        responseList.Add(header[i]);
                    }
                    if (header.GetKey(i) == "Last-Modified")
                    {
                        responseList.Add(header[i]);
                    }
                }
                //sw.Write("\n----------------------------------------------");
                //foreach (var item in requestHeaders)
                //{
                //    sw.Write("\n" + item.Key + ": " + item.Value);
                //}
                //Deserialize the output(from singleline to multiline)
                //responseText_deserialized = JsonConvert.DeserializeObject(responseText).ToString();
                //response2 = new WebClient().DownloadString(url);




            }
            catch (Exception e)
            {
                responseList.Add(e.ToString());
            }


            return responseList;

        }

        public static Models2.weatherdata ConvertYrnoXML(string YrnoLoad)
        {

            // # Handle FMIs XML response transfer it to Object

            Models2.weatherdata YrnoXMLConverted = new Models2.weatherdata();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(YrnoLoad);
            XmlSerializer serializer = new XmlSerializer(typeof(Models2.weatherdata));
            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);
            xmlStream.Flush();
            xmlStream.Position = 0;
            StreamReader YrnoDump = new StreamReader(xmlStream);
            YrnoXMLConverted = (Models2.weatherdata)serializer.Deserialize(YrnoDump);

            return YrnoXMLConverted;
        }
    }
}

