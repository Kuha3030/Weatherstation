using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MVC_Sasema_test.Models;
using System.Data;
using MVC_Sasema_test.Models3;
using System.Net;

namespace MVC_Sasema_test.Controllers
{
    public class GeoLocations
    {
        private static weatherstationEntities db = new weatherstationEntities();

        public static List<string> GetGeo(string location)
        {
            List<string> methodReponseList = new List<string>();
            string locality = "";

            if (location == "thisValueGetsReplaced")
            {
                methodReponseList.Add("error");
                return methodReponseList;
            }
            else
            {

                // # Declaring variables which are used in either if or else: 
                string latitude = "";
                string longitude = "";

                // # Try to get coordinates from database [geocodes] table
                var la = (from G in db.geocodes
                          where G.location_name == location
                          select G.latitude.Value);

                var lo = (from G in db.geocodes
                          where G.location_name == location
                          select G.longitude.Value);

                locality = location;


                // # This step is required to transfer IQueryable object into a list
                List<double> latitudes = la.ToList();
                List<double> longitudes = lo.ToList();

                // # Checking if there was a result for geolocation from own DB. If not, fetches it from geocodes.org using GetGeolocation.
                if (latitudes.Count() == 0)
                {
                    string lat = "";
                    string lng = "";

                    List<string> responseList = new List<string>();

                    responseList = GeoLocations.GetGeolocation(location);

                    // # Checking for error. This means both fetch methods failed
                    if (responseList[1] == "error")
                    {
                        methodReponseList.Add("error");
                        return methodReponseList;
                    }
                    // # If response 1 is success Deserialize response into object and get coordinates from it. 
                    else if (responseList[0] == "succeeded")
                    {
                        Models.GeoObject jsonConverted = JsonConvert.DeserializeObject<Models.GeoObject>(responseList[1]);
                        lat = jsonConverted.address.lat;
                        lng = jsonConverted.address.lng;
                        // # This is the actual location that API found
                        locality = jsonConverted.address.locality;
                    }

                    // # If response 2 succeeds then geoobject has different structure so this handling is required.
                    else
                    {                        
                        Models.GeoObject2 jsonConverted = JsonConvert.DeserializeObject<Models.GeoObject2>(responseList[1]);
                        if (jsonConverted.totalResultsCount == 0)
                        {
                            methodReponseList.Add("error");
                            return methodReponseList;
                        }
                        else 
                        { 
                        lat = jsonConverted.geonames[0].lat;
                        lng = jsonConverted.geonames[0].lng;
                        locality = jsonConverted.geonames[0].name;
                        }
                    }


                    // # Changing coordinates . -> ,
                    string latAdd = lat.Replace('.', ',');
                    string lngAdd = lng.Replace('.', ',');

                    // # Adding new location to database
                    geocodes gc = new geocodes();
                    gc.location_name = locality;
                    gc.latitude = float.Parse(latAdd);
                    gc.longitude = float.Parse(lngAdd);
                    db.geocodes.Add(gc);
                    db.SaveChanges();

                    latitude = string.Format("{00:000}", lat);
                    longitude = string.Format("{00:000}", lng);

                }
                // ## Rounding up and converting to string (using always the first found location)
                // # This happens only when there is a successful search of geolocation from own DB.
                else
                {
                    latitude = Convert.ToString(Math.Round(latitudes[0], 3));
                    longitude = Convert.ToString(Math.Round(longitudes[0], 3));
                }
                latitude = latitude.Replace(',', '.');
                longitude = longitude.Replace(',', '.');


                // Returning found coordinates as Tuple
                methodReponseList.Add(latitude);
                methodReponseList.Add(longitude);
                methodReponseList.Add(locality);
                return methodReponseList;
            }

        }

        public static List<string> GetGeolocation(string location)
        {
            //StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("/txt/Errorlog.txt"), true);

            //Basic url for getting latest data for coordinates lat=60.1&lon=24.9 (Helsinki)
            string url = $"http://api.geonames.org/geoCodeAddressJSON?q={location}&countryBias=FI&username=velinteemu";

            //Create WebRequest handler.
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

            //UserAgent identification (needed by the response server)
            request.UserAgent = "Weatherstation";

            //Declare variables outside try/catch to be able to read them outside try.
            string responseText = "";
            string responseText_deserialized;
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            List<string> responseList = new List<string>();

            //StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("/txt/HttpRequestsFromGeocodes.txt"), true);


            System.Net.WebResponse response = request.GetResponse();

            //Get the response (data hopefully).
            try
            {

                // # Read response and write it to responseList[0]
                StreamReader readerForRequest = new StreamReader(response.GetResponseStream());
                responseText = readerForRequest.ReadToEnd();
                WebHeaderCollection header = request.Headers;

                // # Go through headers and get Expired & Last modified values
                for (int i = 0; i < header.Count; i++)
                {
                    // # For logging only
                    requestHeaders.Add(header.GetKey(i), header[i]);
                }
            }
            catch (Exception e)
            {

            }


            try
            {
                if (responseText == "{}" | responseText == "")
                {
                    responseList.Add("First request failed");
                    string responseText_deserialized2 = GetGeolocation2(location);
                    if (responseText_deserialized2 == "error")
                    {
                        responseList.Add("error");
                    }
                    else 
                    { 
                    responseList.Add(responseText_deserialized2);
                    }
                }
                else
                {
                    responseList.Add("succeeded");

                    //Deserialize the output (from singleline to multiline)
                    responseText_deserialized = JsonConvert.DeserializeObject(responseText).ToString();


                    responseList.Add(responseText_deserialized);
                }
            }


            catch (Exception e)
            {
                //sw.Write("\n" + e.Message);
            }
            return responseList;

        }
        //sw.Close();


        public static string GetGeolocation2(string location)
        {

            // # Alternative search if GetGeolocation() returns null:
            // ## Uses different search url, "searchJSON" instead of "getGeoAddress".
            // ## Finds places like "Häijää"

            //Basic url for getting latest data for coordinates lat=60.1&lon=24.9 (Helsinki)
            string url = $"http://api.geonames.org/searchJSON?q={location}&country=FI&username=velinteemu";

            //Create WebRequest handler.
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

            //UserAgent identification (needed by the response server)
            request.UserAgent = "Weatherstation";
            
            //Declare variables outside try/catch to be able to read them outside try.
            string responseText = "";
            string responseText_deserialized;
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            System.Net.WebResponse response = request.GetResponse();
            try
            {

                // # Read response and write it to responseList[0]
                StreamReader readerForRequest = new StreamReader(response.GetResponseStream());
                responseText = readerForRequest.ReadToEnd();
                WebHeaderCollection header = request.Headers;
                // # Go through headers and get Expired & Last modified values
                for (int i = 0; i < header.Count; i++)
                {
                    // # For logging only
                    requestHeaders.Add(header.GetKey(i), header[i]);

                }
            }
            catch (Exception e)
            {

            }

            //Get the response (data hopefully).
            if (string.IsNullOrEmpty(responseText) == true)
            {
                responseText_deserialized = "error";
                return responseText_deserialized;

            }
            else
            {

                //Deserialize the output (from singleline to multiline)
                if (string.IsNullOrEmpty(responseText) != true)
                { 
                    responseText_deserialized = JsonConvert.DeserializeObject(responseText).ToString();
                }
                else
                {
                    responseText_deserialized = "error";
                }
                return responseText_deserialized;

            }

            //sw.Close();
        }



    }


}