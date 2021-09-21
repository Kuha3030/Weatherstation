using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using MVC_Sasema_test.Models;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVC_Sasema_test.Controllers
{
    public class ForecaSearchController : Controller
    {
        //Connect to database entity
        private static weatherstationEntities db = new weatherstationEntities();

        //Gets current apikeyinformation from database, and updates it if nescessary
        public static string GetForecaAccesstoken()
        {
            //Get current tokendata from database.
            var tokendata = db.providers.First(P => P.provider_name == "foreca");

            //Place the tokendata to variables
            string accesstoken = tokendata.api_key; //Current accesstoken in db
            var tokenExpiryTime_db = tokendata.token_expiry_time; //Datetime of token expirytime
            string apiUsername = tokendata.username; //Username and password to get new token if necessary
            string apiPassword = tokendata.password;


            //Declare few variables
            string apiResponseJson = ""; //string to store the response from foreca apikey query.
            int tokenExpiresIn = 0; //INT to store the validity time of the key in seconds (to set the token_expiry_time in db)

            //If current token expirytime is expired, get new token. Otherwise use the token from db.
            if (tokenExpiryTime_db <= DateTime.UtcNow)
            {
                //'expire hours=1' in the token url sets the time how long the token is alive.
                string url = "https://pfa.foreca.com/authorize/token?expire_hours=1";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Set the Method property of the request to POST.
                request.Method = "POST";

                // Set content type
                request.ContentType = "application/x-www-form-urlencoded";

                //Create bytebuffer for storing credentials for the token retrieval.
                byte[] buffer = Encoding.GetEncoding("UTF-8").GetBytes("{\"user\": \"" + apiUsername + "\", \"password\": \"" + apiPassword + "\"}");

                //Request stream. Not sure how this works, but this is how it is now.
                Stream reqstr = request.GetRequestStream();
                reqstr.Write(buffer, 0, buffer.Length);
                reqstr.Close();


                //Get the response from foreca and except JSON with token data.
                try
                {
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponseJson = reader.ReadToEnd();
                }
                catch (Exception e)
                {
                    apiResponseJson = e.Message;
                }

                //Convert the JSON string to JObject for easy accessibility
                var accesstokenJobj = JObject.Parse(apiResponseJson);
                //Get the accesstoken from the JSON object.
                accesstoken = accesstokenJobj["access_token"].ToString();
                //Get the expirytime from the JSON object.
                tokenExpiresIn = int.Parse(accesstokenJobj["expires_in"].ToString());

                //Call SaveForecaAccesstoken for storing the new token with expirytime in database.
                SaveForecaAccesstoken(accesstoken, tokenExpiresIn);

            }
            //Return the accesstoken.
            return accesstoken;
        }
        //Called for saving new accesstoken to database. Gets new token and expirytime as parameters.
        public static string SaveForecaAccesstoken(string accesstoken, int expirytime)
        {
            //Open the foreca provider db row to tokendata.
            var tokendata = db.providers.First(P => P.provider_name == "foreca");
            tokendata.api_key = accesstoken; //Replace the old token with new one.
            tokendata.token_expiry_time = DateTime.UtcNow.AddSeconds(expirytime); //Replace old expiry time with new one. Gets current time and adds the seconds retrieved from api to it.
            db.SaveChanges(); //Saves the changes to db.

            //Just some debugging stuff we return. Not used in any of the actual functionality
            string result = tokendata.api_key.ToString() + tokendata.token_expiry_time.ToString();
            return result;
        }

        //Get Hourly forecast. Gets accesstoken as parameter.
        public static Rootobject GetForecaWeatherData(string accesstoken)
        {
            //Get hourly forecast for next 48 hours. Default is 24h and maximum is 168h
            string url = "https://pfa.foreca.com//api/v1/forecast/hourly/:24.29,67.60?periods=48";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Set the Method property of the request to POST.
            request.Method = "GET";
            //Set token in header
            request.Headers["Authorization"] = "Bearer " + accesstoken;

            //Declare variables outside try/catch to be able to read them outside try.
            string responseText = "";

            //Get the response. TRY is disabled for now.
            //try
            //{
                //Get the forecast response from api
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseText = reader.ReadToEnd();

                //Deserialize the response straight to Class objects.
                var deserialised = JsonConvert.DeserializeObject<Rootobject>(responseText);
                return deserialised;

            //}
            //catch (Exception e)
            //{
            //    return null;
            //}




            ////DEBUG JSON test string to prevent un-necessary api usage when testing json deserialization
            ////
            //string testJson = "{ \"forecast\": [ { \"time\": \"2021 - 08 - 07T15: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 21, \"feelsLikeTemp\": 21, \"windSpeed\": 3, \"windGust\": 9, \"windDir\": 175, \"windDirString\": \"S\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 07T16: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 21, \"feelsLikeTemp\": 21, \"windSpeed\": 3, \"windGust\": 9, \"windDir\": 165, \"windDirString\": \"S\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 07T17: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 21, \"feelsLikeTemp\": 20, \"windSpeed\": 3, \"windGust\": 9, \"windDir\": 153, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 07T18: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 20, \"feelsLikeTemp\": 20, \"windSpeed\": 3, \"windGust\": 9, \"windDir\": 141, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 07T19: 00 + 03:00\", \"symbol\": \"d100\", \"temperature\": 20, \"feelsLikeTemp\": 20, \"windSpeed\": 3, \"windGust\": 8, \"windDir\": 136, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 07T20: 00 + 03:00\", \"symbol\": \"d100\", \"temperature\": 19, \"feelsLikeTemp\": 19, \"windSpeed\": 3, \"windGust\": 8, \"windDir\": 131, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 07T21: 00 + 03:00\", \"symbol\": \"d100\", \"temperature\": 18, \"feelsLikeTemp\": 18, \"windSpeed\": 2, \"windGust\": 8, \"windDir\": 124, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 07T22: 00 + 03:00\", \"symbol\": \"d100\", \"temperature\": 16, \"feelsLikeTemp\": 16, \"windSpeed\": 3, \"windGust\": 7, \"windDir\": 125, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 07T23: 00 + 03:00\", \"symbol\": \"n200\", \"temperature\": 15, \"feelsLikeTemp\": 15, \"windSpeed\": 3, \"windGust\": 7, \"windDir\": 126, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T00: 00 + 03:00\", \"symbol\": \"n200\", \"temperature\": 14, \"feelsLikeTemp\": 14, \"windSpeed\": 3, \"windGust\": 6, \"windDir\": 127, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T01: 00 + 03:00\", \"symbol\": \"n200\", \"temperature\": 13, \"feelsLikeTemp\": 13, \"windSpeed\": 3, \"windGust\": 7, \"windDir\": 120, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T02: 00 + 03:00\", \"symbol\": \"n200\", \"temperature\": 12, \"feelsLikeTemp\": 12, \"windSpeed\": 3, \"windGust\": 7, \"windDir\": 113, \"windDirString\": \"SE\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T03: 00 + 03:00\", \"symbol\": \"n200\", \"temperature\": 11, \"feelsLikeTemp\": 11, \"windSpeed\": 3, \"windGust\": 7, \"windDir\": 105, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T04: 00 + 03:00\", \"symbol\": \"n100\", \"temperature\": 11, \"feelsLikeTemp\": 11, \"windSpeed\": 3, \"windGust\": 7, \"windDir\": 98, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T05: 00 + 03:00\", \"symbol\": \"d100\", \"temperature\": 10, \"feelsLikeTemp\": 8, \"windSpeed\": 3, \"windGust\": 6, \"windDir\": 92, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T06: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 10, \"feelsLikeTemp\": 10, \"windSpeed\": 3, \"windGust\": 6, \"windDir\": 85, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T07: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 11, \"feelsLikeTemp\": 11, \"windSpeed\": 3, \"windGust\": 7, \"windDir\": 89, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T08: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 13, \"feelsLikeTemp\": 13, \"windSpeed\": 3, \"windGust\": 8, \"windDir\": 92, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T09: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 14, \"feelsLikeTemp\": 14, \"windSpeed\": 4, \"windGust\": 9, \"windDir\": 95, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T10: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 15, \"feelsLikeTemp\": 15, \"windSpeed\": 4, \"windGust\": 10, \"windDir\": 100, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T11: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 16, \"feelsLikeTemp\": 16, \"windSpeed\": 4, \"windGust\": 11, \"windDir\": 103, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T12: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 17, \"feelsLikeTemp\": 17, \"windSpeed\": 5, \"windGust\": 12, \"windDir\": 107, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T13: 00 + 03:00\", \"symbol\": \"d000\", \"temperature\": 18, \"feelsLikeTemp\": 18, \"windSpeed\": 5, \"windGust\": 12, \"windDir\": 107, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 }, { \"time\": \"2021 - 08 - 08T14: 00 + 03:00\", \"symbol\": \"d100\", \"temperature\": 19, \"feelsLikeTemp\": 19, \"windSpeed\": 5, \"windGust\": 12, \"windDir\": 108, \"windDirString\": \"E\", \"precipProb\": 1, \"precipAccum\": 0 } ] }";
            ////Use Json to deserialize the Json string to class objects.
            //var deserialised = JsonConvert.DeserializeObject<Rootobject>(testJson);
            ////Return the debugdata
            //return deserialised;

        }

        public class Rootobject
        {
            public Forecast[] forecast { get; set; }
        }

        public class Forecast
        {
            public string time { get; set; }
            public string symbol { get; set; }
            public int temperature { get; set; }
            public int feelsLikeTemp { get; set; }
            public int windSpeed { get; set; }
            public int windGust { get; set; }
            public int windDir { get; set; }
            public string windDirString { get; set; }
            public int precipProb { get; set; }
            public decimal precipAccum { get; set; }
        }
        // GET: ForecaSearch
        public ActionResult Index()
    {

            string accesstoken = GetForecaAccesstoken(); //Get accesstoken
            ViewData.Model = GetForecaWeatherData(accesstoken); //Get forecast
            ViewData["Accesstoken"] = accesstoken; //Pass the accesstoken just for debugging.


            return View();
    }
}
}