using MVC_Sasema_test.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MVC_Sasema_test.Controllers
{
    public class FMISearch
    {

                       
        public static List<string> GetFMI2(string location, DateTime lastModified)

        {

            List<string> responseList = new List<string>();

            // Get timevalues for search string. 4 hours deducted from Datetime to match results from FMI.
            // Why doesn't the value "&timezone=Europe/Helsinki&" in search string work?
            string searchDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string searchTime = DateTime.UtcNow.AddHours(-1).ToString("HH:mm:ss");
            searchTime = searchTime.Replace(".", ":");

            string searchDateEnd = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            string url = $"http://opendata.fmi.fi/wfs/fin?service=WFS&version=2.0.0&request=GetFeature&storedquery_id=fmi::forecast::harmonie::surface::point::timevaluepair&parameters=temperature,windspeedms,Precipitation1h,totalcloudcover&latlon={location}&starttime={searchDate}T{searchTime}Z&endtime={searchDateEnd}T{searchTime}Z&maxlocations=1&";
            //WebRequest request = WebRequest.Create(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (lastModified != null)
            {
                 request.IfModifiedSince = lastModified;
            }

            //Get the response (data hopefully).
            string responseText = "";

            //StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("/txt/HttpRequestinfoFMI.txt"), true);

            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();

            try
            {

                // # Get response using request
                WebResponse response = request.GetResponse();
                // # Create HTTP header collection from response
                WebHeaderCollection header = response.Headers;
                
                // # Read response and write it to responseList[0]
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseText = reader.ReadToEnd();
                responseList.Add(responseText);

                // # Go through headers and get Expired & Last modified values
                for (int i = 0; i < header.Count; i++)
                {
                    // # For logging only
                    requestHeaders.Add(header.GetKey(i), header[i]);

                    if (header.GetKey(i) == "Expires")
                    {
                        responseList.Add(header[i]);
                    }
                    if (header.GetKey(i) == "Last-Modified")
                    {
                        responseList.Add(header[i]);
                    }
                }
            }

            // # Catch exception and write result to responseList[0]
            catch (Exception e)
            {
                responseList.Add(e.ToString());
            }


            return responseList;

        }
        public static Models3.FeatureCollection ConvertXML2(string FMILoad)
        {

            // # Handle FMIs XML response transfer it to Object

            Models3.FeatureCollection FMIxmlConverted = new Models3.FeatureCollection();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(FMILoad);
            XmlSerializer serializer = new XmlSerializer(typeof(Models3.FeatureCollection));
            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);
            xmlStream.Flush();
            xmlStream.Position = 0;
            StreamReader FMIDump = new StreamReader(xmlStream);
            FMIxmlConverted = (Models3.FeatureCollection)serializer.Deserialize(FMIDump);

            return FMIxmlConverted;
        }
    }
}