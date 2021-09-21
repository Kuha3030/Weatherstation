using MVC_Sasema_test.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MVC_Sasema_test.Controllers
{
    public class ForecaSearch
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
            WebHeaderCollection header = new WebHeaderCollection();

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
        public static List<string> forecaFetch(string location, DateTime lastModified, string accesstoken)
        {
            List<string> responseList = new List<string>();
            //Get hourly forecast for next 48 hours. Default is 24h and maximum is 168h
            string url = $"https://pfa.foreca.com//api/v1/forecast/hourly/:{location}?periods=168";
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
            responseList.Add(responseText);


            // # This doesn't work yet, because Foreca doesn't return Expires & Last Modified headers(?)
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


            // # Creating artificial response headers imitating Yrno's response. (Expires = DateTime.UtcNow + 30minutes, Last Modified = about the time search was made)
            // # This reduces spam to Foreca if same location is entered multiple times.
            responseList.Add(DateTime.UtcNow.AddMinutes(360).ToString("r"));
            responseList.Add(DateTime.UtcNow.ToString("r"));

            //Deserialize the response straight to Class objects.
            //var deserialised = JsonConvert.DeserializeObject<Rootobject>(responseText);
            return responseList;

            //}
            //catch (Exception e)
            //{
            //    return null;
            //}




        }

   
        // GET: ForecaSearch
        //public ActionResult Index()
        //{

        //    string accesstoken = GetForecaAccesstoken(); //Get accesstoken
        //    ViewData.Model = GetForecaWeatherData(accesstoken); //Get forecast
        //    ViewData["Accesstoken"] = accesstoken; //Pass the accesstoken just for debugging.


        //    return View();
        //}

    }
}