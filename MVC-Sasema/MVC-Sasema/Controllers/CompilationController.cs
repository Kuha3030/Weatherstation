using System;
using System.Linq;
using System.Web.Mvc;
using MVC_Sasema_test.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;

namespace MVC_Sasema_test.Controllers
{
    public class CompilationController : Controller
    {


        public static bool checkExpiredYrno = false;
        public static bool checkExpiredFMI = false;
        public static bool checkExpiredForeca = false;


        public static string StopwatchMethod, StopwatchSearch, StopwatchInsert, StopwatchDBoperations, StopwatchAccessToken;
        public static string searchLocation = "";

        private static weatherstationEntities db = new weatherstationEntities();


        public ActionResult Results()
        {
            // # Get UserID
            string userIP = Request.UserHostAddress;
            string userIDforSession = IPHashing.GetHashString(userIP);

            Session["UserID"] = userIDforSession;
            ViewBag.userID = userIDforSession;


            // # OBS! Server DateTime when uploaded to Azure is UTC!
            ViewBag.DateTime = DateTime.UtcNow.AddHours(-1);

            // # Passing Stopwatch information to View:
            ViewBag.StopwatchMethod = StopwatchMethod;
            ViewBag.StopwatchSearch = StopwatchSearch;
            ViewBag.StopwatchInsert = StopwatchInsert;
            ViewBag.StopwatchAccessToken = StopwatchAccessToken;

            ViewBag.StopwatchDBoperations = StopwatchDBoperations;
            ViewBag.StopwatchCreatesearch = (Convert.ToInt32(StopwatchDBoperations) - Convert.ToInt32(StopwatchInsert)).ToString();

            // # Passing expired booleans to View (Not used atm)
            ViewBag.YrnoCheck = checkExpiredYrno;
            ViewBag.FMICheck = checkExpiredFMI;
            ViewBag.ForecaCheck = checkExpiredForeca;
            // Get UserID from Session (hashed IP)
            string userID = Convert.ToString(Session["UserID"]);

            // # Get user input location

            ViewBag.searchLocation = searchLocation;
            Stopwatch stopwatchGatheringDataTable = new Stopwatch();


            // # Checking for user changed now. If searchLocation is "" there will not be a previous result in View.
            // ## searchLocation is always set back to "" after page has loaded. This is to avoid confusion between users.
            // ## There's room for improvement here. We could show the last search the user made by using userID as identifier.
            // ## DBTools.CheckForUser(userID) needs to be developed further for this to work.

            if (searchLocation != "")
            {
                // # Get the latest searchID from user to fetch from DB for yrno
                if (checkExpiredYrno == true)
                {
                    // # If the search IS expired get the last search_ID
                    // # This means there's a fresh fetch from the API
                    ViewBag.searchIDYrno = DBTools.GetSearchID("latest_for_user", userID, searchLocation);
                }
                else
                {
                    // # If the search is still valid get the last search_ID from corresponding API
                    ViewBag.searchIDYrno = DBTools.GetSearchID("yrno_latest_for_location", userID, searchLocation);
                }

                // # Same as above for FMI
                if (checkExpiredFMI == true)
                {
                    ViewBag.searchIDFMI = DBTools.GetSearchID("latest_for_user", userID, searchLocation);
                }
                else
                {
                    ViewBag.searchIDFMI = DBTools.GetSearchID("FMI_latest_for_location", userID, searchLocation);
                }
                // # Same as above for Foreca
                if (checkExpiredForeca == true)
                {
                    ViewBag.searchIDForeca = DBTools.GetSearchID("latest_for_user", userID, searchLocation);
                }
                else
                {
                    ViewBag.searchIDForeca = DBTools.GetSearchID("foreca_latest_for_location", userID, searchLocation);
                }

                // # A ViewBag boolean for showing development data in View
                if (checkExpiredYrno == true | checkExpiredFMI == true | checkExpiredForeca == true)
                {
                    ViewBag.Fresh = "fresh";
                }
                else
                {
                    ViewBag.Fresh = "old";
                }
            }


            // ## Using DataTable instead of class is due to "system.null_reference_exception" error when calling classes in View made from nested JSON.
            // ## yrno's class based on XML needs to be reconstructed as it cannot be called upon direclty in View either.
            // ## This may not be the most elegant solution, but atleast all the data is now in the same format for the View!

            // # DataTable for showing search results in View
            DataTable dtForView = new DataTable("WeatherTable");

            // # Adding columns. When creating new columns pay attention to the order they are made. This is the same order in which data will be inserted and extracted.
            dtForView.Columns.Add("search_id", typeof(int));
            dtForView.Columns.Add("provider_id", typeof(int));
            dtForView.Columns.Add("data_timestamp", typeof(DateTime));
            dtForView.Columns.Add("data_temp", typeof(string));
            dtForView.Columns.Add("data_wsms", typeof(string));
            dtForView.Columns.Add("data_precA", typeof(string));
            dtForView.Columns.Add("data_tcc", typeof(string));

            // # Declaring lists for extracting data from DB
            // # Using foreach loops that add rows straight into DataTable would be better. 
            // # However yrno doesn't always return same amount of precipitation data vs other data. This results null reference exception in foreach loop.
            // # There is no reason not to change DataTable insertion method to foreach loop for FMI. Go for it!

            List<DateTime> TimestampsFMI = new List<DateTime>();
            List<DateTime> TimestampsYrno = new List<DateTime>();
            List<DateTime> TimestampsForeca = new List<DateTime>();

            List<string> FMITempList = new List<string>();
            List<string> FMIWsmsList = new List<string>();
            List<string> FMIPrecAList = new List<string>();
            List<string> FMITccList = new List<string>();

            List<string> yrnoTempList = new List<string>();
            List<string> yrnoWsmsList = new List<string>();
            List<string> yrnoPrecAList = new List<string>();
            List<string> yrnoTccList = new List<string>();

            List<string> forecaTempList = new List<string>();
            List<string> forecaWsmsList = new List<string>();
            List<string> forecaPrecAList = new List<string>();
            List<string> forecaTccList = new List<string>();

            // #Casting ViewBags into ints. Linq cannot work with dynamic variables such as ViewBag. 


            stopwatchGatheringDataTable.Start();

            // # Check if searcLocation is empty (when loading the page first time) or if there's a Geolocation error.
            if (searchLocation != "" & searchLocation != "Geolocation error")
            {
                int search_idFMI = ViewBag.searchIDFMI;
                int search_idYrno = ViewBag.searchIDYrno;
                int search_idForeca = ViewBag.searchIDForeca;
                // # Get data rows from FMI with previously determined searchID and add them to corresponding lists.
                // # OrderBy arranges rows by their timestamp
                foreach (var dataEntry in db.data.Where(a => a.search_id == search_idFMI).OrderBy(a => a.data_timestamp))
                {
                    if (dataEntry.provider_id == 1)
                    {
                        // # DateTime gets only added from temperature entry. 
                        if (dataEntry.datatype_id == 1)
                        {
                            FMITempList.Add(dataEntry.data_value);
                            TimestampsFMI.Add((DateTime)dataEntry.data_timestamp);
                        }
                        if (dataEntry.datatype_id == 2)
                        {
                            FMIWsmsList.Add(dataEntry.data_value);
                        }
                        if (dataEntry.datatype_id == 3)
                        {
                            FMIPrecAList.Add(dataEntry.data_value);
                        }
                        if (dataEntry.datatype_id == 4)
                        {
                            FMITccList.Add(dataEntry.data_value);
                        }
                    }

                }

                foreach (var dataEntry in db.data.Where(a => a.search_id == search_idYrno).OrderBy(a => a.data_timestamp))
                {

                        if (dataEntry.provider_id == 2)
                        {
                            if (dataEntry.datatype_id == 1)
                            {
                                yrnoTempList.Add(dataEntry.data_value);
                                TimestampsYrno.Add((DateTime)dataEntry.data_timestamp);
                            }
                            if (dataEntry.datatype_id == 2)
                            {
                                yrnoWsmsList.Add(dataEntry.data_value);
                            }
                            if (dataEntry.datatype_id == 3)
                            {
                                yrnoPrecAList.Add(dataEntry.data_value);
                            }
                            if (dataEntry.datatype_id == 5)
                            {
                                yrnoTccList.Add(dataEntry.data_value);
                            }
                        }
                }

                // Foreca data
                foreach (var dataEntry in db.data.Where(a => a.search_id == search_idForeca).OrderBy(a => a.data_timestamp))
                {

                    if (dataEntry.provider_id == 3)
                    {
                        if (dataEntry.datatype_id == 1)
                        {
                            forecaTempList.Add(dataEntry.data_value);
                            TimestampsForeca.Add((DateTime)dataEntry.data_timestamp);
                        }
                        if (dataEntry.datatype_id == 2)
                        {
                            forecaWsmsList.Add(dataEntry.data_value);
                        }
                        if (dataEntry.datatype_id == 3)
                        {
                            forecaPrecAList.Add(dataEntry.data_value);
                        }
                        if (dataEntry.datatype_id == 4)
                        {
                            forecaTccList.Add(dataEntry.data_value);
                        }
                    }


                }

                // # If yrno doesn't deliver enough precipitation data the while loop inserts "NaN" to the list to avoid null reference exception.
                while (yrnoPrecAList.Count < yrnoTempList.Count)
                {
                    yrnoPrecAList.Add("NaN");
                    yrnoTccList.Add("NaN");
                }

            }
            else if (searchLocation == "Geolocation error")
            {
                ViewBag.ErrorMessage = "Geolocation error!";
            }

            stopwatchGatheringDataTable.Stop();
            // # Inserting data to DataTable
            for (int i = 0; i < FMITempList.Count; i++)
            {
                dtForView.Rows.Add(ViewBag.searchIDFMI, 1, TimestampsFMI[i], FMITempList[i], FMIWsmsList[i], FMIPrecAList[i], FMITccList[i]);
            }

            for (int i = 0; i < yrnoTempList.Count; i++)
            {
                dtForView.Rows.Add(ViewBag.searchIDYrno, 2, (TimestampsYrno[i]), yrnoTempList[i], yrnoWsmsList[i], yrnoPrecAList[i], yrnoTccList[i]);
            }

            for (int i = 0; i < forecaTempList.Count; i++)
            {
                dtForView.Rows.Add(ViewBag.searchIDForeca, 3, (TimestampsForeca[i]), forecaTempList[i], forecaWsmsList[i], forecaPrecAList[i], forecaTccList[i]);
            }

            // Insert DataTable to a ViewData Model.
            ViewData.Model = dtForView.AsEnumerable();
            ViewBag.StopwatchGather = stopwatchGatheringDataTable.ElapsedMilliseconds;

            searchLocation = "";
            return View();
        }
        public ActionResult SaveToDB(string location)
        {   
            // ### This method saves search into Database

            // # Handles javaScript error. This shouldn't happen anymore as RegEx should catch false inputs.
            if (location == "thisValueGetsReplaced")
            {
                searchLocation = "Geolocation error";
                return RedirectToAction("Results");
            }

            // # Capitalizes the search location and puts the rest of it in lowercase.
            location = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(location);

            // # Stopwatches for clocking method's operations.
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatchInsert = new Stopwatch();
            Stopwatch stopwatchSearch = new Stopwatch();
            Stopwatch stopwatchDBoperations = new Stopwatch();
            Stopwatch stopwatchAccessToken = new Stopwatch();

            // # Fetch "Expires" DateTime from DB based on location. Reducing spam to APIs.
            checkExpiredYrno = TimeTools.CheckExpiredYrno(location);
            checkExpiredFMI = TimeTools.CheckExpiredFMI(location);
            checkExpiredForeca = TimeTools.CheckExpiredForeca(location);

            // # Executing search routine if any of the old searces has expired
            if (checkExpiredYrno == true | checkExpiredFMI == true | checkExpiredForeca == true)
            {
                stopwatch.Start();

                // ## PART 1: Fetch data from API into objects: ################################################################

                // # Casting UserID (User's hashed IP address) into string.
                string userID = Convert.ToString(Session["UserID"]);

                // # After the fetch the reseponse lists contain: Response string[0], Expires[1], and Last-modified[2]      
                List<string> yrnoResponse = new List<string>();
                List<string> FMIResponse = new List<string>();
                List<string> ForecaResponse = new List<string>();
                List<string> coordinateResponseList = new List<string>();

                Tuple<string, string> coordinates = new Tuple<string, string>("", "");

                // # Fetch coordinates
                coordinateResponseList = GeoLocations.GetGeo(location);

                // # Check for geolocation error
                if (coordinateResponseList[0] == "error")
                {
                     searchLocation = "Geolocation error";
                }
                else
                {
                    coordinates = Tuple.Create(coordinateResponseList[0], coordinateResponseList[1]);
                    location = coordinateResponseList[2];
                    searchLocation = location;

                    // # FMISearch uses string instead of Tuple.
                    string FMICoordinates = coordinates.Item1 + "," + coordinates.Item2;
                    string ForecaCoordinates = coordinates.Item2.Replace(",", ".") + "," + coordinates.Item1.Replace(",", ".");
                    // # Get "Last modified" DateTime from DB.
                    DateTime sendLastModifiedYrno = TimeTools.GetLastModifiedYrno(location);
                    DateTime sendLastModifiedFMI = TimeTools.GetLastModifiedFMI(location);
                    DateTime sendLastModifiedForeca = TimeTools.GetLastModifiedFMI(location);

                    // # Fetching from API.
                    // # List contains: search results[0], (Header) Expired[1], (Header) Last modified[2]

                    stopwatchSearch.Start();

                    if (checkExpiredYrno == true)
                    {
                        // # Fetching data from API.
                        yrnoResponse = YrnoSearch.yrnoFetch2(coordinates, sendLastModifiedYrno);
                    }
                    if (checkExpiredFMI == true)
                    {
                        FMIResponse = FMISearch.GetFMI2(FMICoordinates, sendLastModifiedFMI);
                    }
                    if (checkExpiredForeca == true)
                    {
                        stopwatchAccessToken.Start();
                        string accessToken = ForecaSearch.GetForecaAccesstoken();
                        stopwatchAccessToken.Stop();
                        StopwatchAccessToken = stopwatchAccessToken.ElapsedMilliseconds.ToString();

                        ForecaResponse = ForecaSearch.forecaFetch(ForecaCoordinates, sendLastModifiedForeca, accessToken);
                    }



                    // # Handling the response
                    // ## If response list has more than 1 entries -> insert fetched data into DB.
                    // ## If list has only first value it's an HTTP status code (error).
                    // ## Logic below only handles response lists that have more than 1 value (when list actuallys contains insertable data)
                    if (yrnoResponse.Count > 1 | FMIResponse.Count > 1 | ForecaResponse.Count > 1)
                    {
                        string yrnoLoad = "";
                        string yrnoExpires = "";
                        string yrnoLastModified = "";
                        string FMILoad = "";
                        string FMIExpires = "";
                        string FMILastModified = "";
                        string forecaLoad = "";
                        string forecaExpires = "";
                        string forecaLastModified = "";

                        Models3.FeatureCollection FMIxmlConverted = new Models3.FeatureCollection();
                        Models2.weatherdata yrnoXmlConverted = new Models2.weatherdata();
                        Models.ForecaModelJSON forecaJsonConverted = new Models.ForecaModelJSON();
                        //StreamWriter sw = new StreamWriter(Server.MapPath("/json/YrnoXMLResult.xml"));
                        if (yrnoResponse.Count > 1)
                        {
                            yrnoLoad = yrnoResponse[0];
                            //sw.Write(yrnoLoad);
                            //sw.Close();
                            yrnoExpires = yrnoResponse[1];
                            yrnoLastModified = yrnoResponse[2];
                        }
                        if (FMIResponse.Count > 1)
                        {
                            FMILoad = FMIResponse[0];
                            FMIExpires = FMIResponse[1];
                            FMILastModified = FMIResponse[2];
                        }
                        if (ForecaResponse.Count > 1)
                        {
                            forecaLoad = ForecaResponse[0];
                            forecaExpires = ForecaResponse[1];
                            forecaLastModified = ForecaResponse[2];
                        }


                        // # Creating C# object from yr.no XML
                        if (yrnoResponse.Count > 1)
                        {
                            yrnoXmlConverted = YrnoSearch.ConvertYrnoXML(yrnoLoad);
                        }

                        // # Creating C# object from FMI XML
                        if (FMIResponse.Count > 1)
                        {
                            FMIxmlConverted = FMISearch.ConvertXML2(FMILoad);
                        }
                        if (ForecaResponse.Count > 1)
                        {
                            forecaJsonConverted = JsonConvert.DeserializeObject<ForecaModelJSON>(forecaLoad);
                        }
                        stopwatchSearch.Stop();


                        // ## PART 2: Insert objects into database: ################################################################
                        stopwatchDBoperations.Start();

                        // # Insert search info into dbo.[searches] (Create a search)
                        searches search = new searches();
                        search.hash_id = userID;
                        search.input_location = location;
                        search.timestamp = DateTime.UtcNow;
                        if (yrnoResponse.Count > 1)
                        {
                            search.expires_yrno = TimeTools.GMTStringToDateTime(yrnoExpires);
                            search.last_modified_yrno = TimeTools.GMTStringToDateTime(yrnoLastModified);
                        }
                        if (FMIResponse.Count > 1)
                        {
                            search.expires_FMI = TimeTools.GMTStringToDateTime(FMIExpires);
                            search.last_modified_FMI = TimeTools.GMTStringToDateTime(FMILastModified);
                        }
                        if (ForecaResponse.Count > 1)
                        {
                            search.expires_foreca = TimeTools.GMTStringToDateTime(forecaExpires);
                            search.last_modified_foreca = TimeTools.GMTStringToDateTime(forecaLastModified);
                        }
                        db.searches.Add(search);
                        db.SaveChanges();

                        // #Get search_id from [searches] (This is required as we cannot know what the previously created search_id is!)
                        var searchID = DBTools.GetSearchID("latest_for_user", userID, location);

                        // # Create DataTable based on Models dbo.[data] table
                        // # Note! Data can be separated by provider_id later on

                        stopwatchInsert.Start();
                        using (DataTable dt = new DataTable("dataInsert"))
                        {
                            dt.Columns.Add("search_id", typeof(int));
                            dt.Columns.Add("provider_id", typeof(int));
                            dt.Columns.Add("datatype_id", typeof(int));
                            dt.Columns.Add("data_timestamp", typeof(DateTime));
                            dt.Columns.Add("data_value", typeof(string));
                            //int yrnocounter = 0;

                            // # Insert yrno json class into DataTable
                            if (yrnoResponse.Count > 1)
                            {
                                List<DateTime> checker = new List<DateTime>();

                                foreach (var dataEntry in yrnoXmlConverted.product.time)
                                {
                                    if (dataEntry.location.temperature != null)
                                    {
                                        dt.Rows.Add(searchID, 2, 1, dataEntry.from, Convert.ToString(Math.Round(Convert.ToDecimal(dataEntry.location.temperature.value),0)));
                                        dt.Rows.Add(searchID, 2, 2, dataEntry.from, Convert.ToString(Math.Round(Convert.ToDecimal(dataEntry.location.windSpeed.mps), 0)));
                                        dt.Rows.Add(searchID, 2, 4, dataEntry.from, Convert.ToString(dataEntry.location.cloudiness.percent));
                                    }
                                    else
                                    {
                                        if (!checker.Contains(dataEntry.to))
                                        {
                                            // #This needs checking! Check the yrno XML for actual data. 
                                            dt.Rows.Add(searchID, 2, 3, dataEntry.to, Convert.ToString(dataEntry.location.precipitation.value));
                                            dt.Rows.Add(searchID, 2, 5, dataEntry.to, Convert.ToString(dataEntry.location.symbol.code));
                                            checker.Add(dataEntry.to);
                                        }
                                    }
                                }
                            }

                            // # Insert FMIs XML based object into DataTable
                            if (FMIResponse.Count > 1)
                            {
                                foreach (var dataEntry in FMIxmlConverted.Member2[0].PointTimeSeriesObservation.Result.MeasurementTimeseries.Point)
                                {
                                    if (dataEntry.MeasurementTVP.Value2 != "NaN")
                                    {
                                        //string convertThis = dataEntry.MeasurementTVP.Value2.Replace(".", ",");
                                        double convertThis = double.Parse(dataEntry.MeasurementTVP.Value2, CultureInfo.InvariantCulture);
                                        string addThis = Convert.ToString(Math.Round(convertThis, 0), CultureInfo.InvariantCulture);
                                        dt.Rows.Add(searchID, 1, 1, dataEntry.MeasurementTVP.Time, addThis);
                                    }
                                    else
                                    {
                                        dt.Rows.Add(searchID, 1, 1, dataEntry.MeasurementTVP.Time, Convert.ToString(dataEntry.MeasurementTVP.Value2));
                                    }

                                }
                                foreach (var dataEntry in FMIxmlConverted.Member2[1].PointTimeSeriesObservation.Result.MeasurementTimeseries.Point)
                                {
                                    if (dataEntry.MeasurementTVP.Value2 != "NaN")
                                    {
                                        double convertThis = double.Parse(dataEntry.MeasurementTVP.Value2, CultureInfo.InvariantCulture);
                                        string addThis = Convert.ToString(Math.Round(convertThis, 0), CultureInfo.InvariantCulture);
                                        dt.Rows.Add(searchID, 1, 2, dataEntry.MeasurementTVP.Time, addThis);
                                    }
                                    else
                                    {
                                        dt.Rows.Add(searchID, 1, 2, dataEntry.MeasurementTVP.Time, Convert.ToString(dataEntry.MeasurementTVP.Value2));

                                    }
                                }
                                foreach (var dataEntry in FMIxmlConverted.Member2[2].PointTimeSeriesObservation.Result.MeasurementTimeseries.Point)
                                {
                                    dt.Rows.Add(searchID, 1, 3, dataEntry.MeasurementTVP.Time, Convert.ToString(dataEntry.MeasurementTVP.Value2));
                                }
                                foreach (var dataEntry in FMIxmlConverted.Member2[3].PointTimeSeriesObservation.Result.MeasurementTimeseries.Point)
                                {
                                    dt.Rows.Add(searchID, 1, 4, dataEntry.MeasurementTVP.Time, Convert.ToString(dataEntry.MeasurementTVP.Value2));
                                }
                            }

                            // # Insert Foreca's JSON based object into DataTable
                            if (ForecaResponse.Count > 1)
                            {
                                foreach (var dataEntry in forecaJsonConverted.forecast)
                                {
                                    ////string convertThis = dataEntry.MeasurementTVP.Value2.Replace(".", ",");
                                    //double convertThis = double.Parse(dataEntry.MeasurementTVP.Value2, CultureInfo.InvariantCulture);
                                    //string addThis = Convert.ToString(Math.Round(convertThis, 0), CultureInfo.InvariantCulture);
                                        DateTime addThisTime = dataEntry.time.ToUniversalTime();

                                        dt.Rows.Add(searchID, 3, 1, addThisTime, dataEntry.temperature);
                                        dt.Rows.Add(searchID, 3, 2, addThisTime, dataEntry.windSpeed);

                                        double convertThis = decimal.ToDouble(dataEntry.precipAccum);
                                        string addThis = Convert.ToString(Math.Round(convertThis, 1), CultureInfo.InvariantCulture);
                                        dt.Rows.Add(searchID, 3, 3, addThisTime, addThis);
                                        dt.Rows.Add(searchID, 3, 4, addThisTime, dataEntry.symbol);
                                }
                            }

                            

                            // # Create DB connection and insert DataTable into dbo.data
                            using (var databaseConnection = new weatherstationEntities())
                            {
                                // # For optimizing access time to DB.
                                databaseConnection.Configuration.AutoDetectChangesEnabled = false;
                                databaseConnection.Configuration.ValidateOnSaveEnabled = false;

                                // # Inserting the DataTable with SqlBulkCopy method
                                using (var copy = new SqlBulkCopy(databaseConnection.Database.Connection.ConnectionString))
                                {

                                    copy.DestinationTableName = "dbo.data";

                                    // Add mappings so that the column order doesn't matter
                                    copy.ColumnMappings.Add(nameof(data.search_id), "search_id");
                                    copy.ColumnMappings.Add(nameof(data.provider_id), "provider_id");
                                    copy.ColumnMappings.Add(nameof(data.datatype_id), "datatype_id");
                                    copy.ColumnMappings.Add(nameof(data.data_timestamp), "data_timestamp");
                                    copy.ColumnMappings.Add(nameof(data.data_value), "data_value");

                                    copy.WriteToServer(dt);
                                }

                                // ## This is just for search and access times:
                                stopwatchDBoperations.Stop();
                                stopwatch.Stop();
                                stopwatchInsert.Stop();
                                StopwatchMethod = stopwatch.ElapsedMilliseconds.ToString();
                                StopwatchSearch = stopwatchSearch.ElapsedMilliseconds.ToString();
                                StopwatchInsert = stopwatchInsert.ElapsedMilliseconds.ToString();
                                StopwatchDBoperations = stopwatchDBoperations.ElapsedMilliseconds.ToString();
                            }
                        }
                    }
                    else
                    {
                        searchLocation = location;
                    }
                }
            }
            else
            {
                searchLocation = location;
            }

            return RedirectToAction("Results");

        }
        public ActionResult EraseFromDB()
            {
                db.Database.ExecuteSqlCommand("DELETE FROM data");

                db.SaveChanges();

                return RedirectToAction("Results");
            }



        public ActionResult Index()
        {
            return RedirectToAction("Results");
        }
    }

}


