using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;

using System.Net;
using System.Text;
using System.Threading.Tasks;
using MVC_Sasema_test.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System.Xml;
using MVC_Sasema_test.Models2;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using MVC_Sasema_test.Models3;

namespace MVC_Sasema_test.Controllers
{
    public class YrnoController : Controller
    {

        // # Set location as string
        public static string searchString = "Jämsä";
        // GET: yrno
        private static weatherstationEntities db = new weatherstationEntities();




        public ActionResult Index()
        {
            // ### Testing different approaches to control Json data: 

            //----------------------------------------------------------------------------------------
            //StreamWriter sw = new StreamWriter(Server.MapPath("/txt/datadump.txt"));
            //sw.Write(yrnoFetch());
            //sw.Close();
            //string[] texts = System.IO.File.ReadAllLines(Server.MapPath("/txt/datadump.txt"));
            //ViewBag.Data = (texts);
            //var dataFile = Server.MapPath("~/txt/datadump.txt");
            //var juttu = JsonConvert.DeserializeObject(yrnoFetch());
            //StreamWriter writer = new StreamWriter(Server.MapPath("~/json/yrnodump2.json"));
            //writer.Write(juttu);
            //writer.Close();
            //var juttu = Server.MapPath("~/json/yrnodump.json");
            //ViewBag.Data = JsonConvert.SerializeObject(juttu, Formatting.Indented);
            //var jsonDeserialized = JsonConvert.DeserializeObject<Models.jsonFetched>(yrnoFetch());
            //StreamWriter writer = new StreamWriter(Server.MapPath("~/json/yrnodump.json"));
            //writer.Write(dumperino);
            //writer.Close();
            //----------------------------------------------------------------------------------------
            //StreamWriter sw = new StreamWriter(Server.MapPath("/json/yrnoDumpNew.json"));
            //writer.Write(GetGeolocation("Jämsänkoski"));
            //writer.Close();

            // Creating user ID by hashing users IP. This should maybe be a cookie? 


            // # Fetch coordinates from database using getGeo method

            // NEEDS TO BE CHANGED TO NEW VERSION OF GEOLOC (returns list)
            //Tuple<string, string> coordinates = GeoLocations.GetGeo(searchString);
            
            // # (only for the View)
            ViewBag.showLocation = searchString;

            // # Fetch JSON file from yrno API using yrnoFetch() method


            //var yrnoDump = yrnoSearch.yrnoFetch(coordinates);




            //StreamReader r = new StreamReader(Server.MapPath("/json/yrnoComplete.json"));
            //var jsonString = r.ReadToEnd();
            //var YrnoDetails = JsonConvert.DeserializeObject<Models3.YrnoObjectComplete>(jsonString);


            // # Transfer data from JSON to object
            //Models.YrnoObject jsonConverted = JsonConvert.DeserializeObject<Models.YrnoObject>(yrnoDump);


            // # Transfer data from JSON to object
            //Models3.YrnoObjectComplete jsonConverted = JsonConvert.DeserializeObject<Models3.YrnoObjectComplete>(yrnoDump);
            //dynamic weatherModel = new ExpandoObject();



            //var YrnoObject = JsonConvert.DeserializeObject<Models3.YrnoObjectComplete>(a);
            //JArray YrnoDetails = new JArray();

            //foreach (JObject item in jsonString)
            //{
            //    YrnoDetails.Add(item);
            //}


            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(yrnoSearch.yrnoFetch(coordinates));
            //doc.Save(Server.MapPath("/json/YrnoXMLtest.xml"));
            Models2.weatherdata yrnoConverted = YrnoSearch.ConvertYrnoXML(Server.MapPath("/json/YrnoXMLtest.xml"));
            // # Creating XML serializer based on Model and saving xml memorystream
            //XmlSerializer serializer = new XmlSerializer(typeof(Models2.Weatherdata));
            //MemoryStream xmlStream = new MemoryStream();
            //doc.Save(xmlStream);
            //xmlStream.Flush();
            //xmlStream.Position = 0;

            // # Reading and Deserializing memorystream to readable Object for the View.
            //StreamReader reader = new StreamReader(xmlStream);
            //var yrnoToView = (Models2.Weatherdata)serializer.Deserialize(reader);
            //reader.Close();

            dynamic weatherModel = new ExpandoObject();
            //weatherModel.YrnoTime = yrnoConverted.product.time;
            //weatherModel.Yrno = yrnoConverted;
            //weatherModel.YrnoProduct = yrnoConverted.product;

            // # Return view
            return View(weatherModel);
        }

        //public ActionResult SaveToDB(string location)
        //{

        //    // # Stopwatch and StreamWriter for logging DB access times.
        //    //Stopwatch stopwatch = new Stopwatch();
        //    //Stopwatch stopwatch2 = new Stopwatch();
        //    //StreamWriter sw = new StreamWriter(Server.MapPath("/txt/Errorlog.txt"), true);


        //    // # Session UserID needs to be converted to string because LINQ cannot use dynamic variables.  
        //    // # User IP is hashed to Session["UserID"] in Index @ HomeController.
        //    string userID = Convert.ToString(Session["UserID"]);

        //    // # For View to show location.
        //    searchString = location;
        //    Tuple<string, string> coordinates = new Tuple<string, string>("","");
        //    // # Getting coordinates using GetGeo method.

        

        //    coordinates = GeoLocations.GetGeo(location);

            

        //    var yrnoDump = yrnoSearch.yrnoFetch(coordinates);

        //    Models3.YrnoObjectComplete jsonConverted = JsonConvert.DeserializeObject<Models3.YrnoObjectComplete>(yrnoDump);

        //    // # Create data object from Model's [data] table and save it to database.
        //    // # Uses time and air temp data from yrno. search.hash_id from Session[UserID].

        //    //stopwatch.Start();

        //    // #Insert search info into dbo.[searches] 
        //    searches search = new searches();
        //    search.hash_id = userID;
        //    search.input_location = location;
        //    search.timestamp = DateTime.Now;
        //    db.searches.Add(search);
        //    db.SaveChanges();
            
        //    // #Get search_id from [searches]
        //    ViewBag.searchID = (
        //            from id in db.searches
        //            where id.hash_id.Equals(userID)
        //            orderby id.search_id descending
        //            select id.search_id
        //            ).Take(1).SingleOrDefault();

        //    //stopwatch2.Start();

        //    // # Create DataTable
        //    using (DataTable dt = new DataTable("dataInsert"))
        //    {
        //        dt.Columns.Add("search_id", typeof(int));
        //        dt.Columns.Add("provider_id", typeof(int));
        //        dt.Columns.Add("datatype_id", typeof(int));
        //        dt.Columns.Add("data_timestamp", typeof(DateTime));
        //        dt.Columns.Add("data_value", typeof(string));
            
        //        // # Insert jsonClass into DataTable
        //        // # FMI and foreca classes can be added as 2 additional foreach loops later on
        //        // # Data can be separated by provider_id
        //        foreach (var dataEntry in jsonConverted.properties.timeseries)
        //        {
        //            dt.Rows.Add(ViewBag.searchID, 2, 1, dataEntry.time, Convert.ToString(dataEntry.data.instant.details.air_temperature));
        //        }


        //    // # Create DB connection and insert DataTable into dbo.data
        //        using (var databaseConnection2 = new weatherstationEntities())
        //        {
        //            // # For optimizing access time to DB.
        //            databaseConnection2.Configuration.AutoDetectChangesEnabled = false;
        //            databaseConnection2.Configuration.ValidateOnSaveEnabled = false;

        //            using (var copy = new SqlBulkCopy(databaseConnection2.Database.Connection.ConnectionString))
        //            {

        //                copy.DestinationTableName = "dbo.data";

        //                // Add mappings so that the column order doesn't matter
        //                copy.ColumnMappings.Add(nameof(data.search_id), "search_id");
        //                copy.ColumnMappings.Add(nameof(data.provider_id), "provider_id");
        //                copy.ColumnMappings.Add(nameof(data.datatype_id), "datatype_id");
        //                copy.ColumnMappings.Add(nameof(data.data_timestamp), "data_timestamp");
        //                copy.ColumnMappings.Add(nameof(data.data_value), "data_value");


        //                copy.WriteToServer(dt);

        //            }

        //        }
        //    }
        //    //sw.Write("\nBULK INSERT: Elapsed time for location called " + location + " foreach loop is: " + stopwatch2.ElapsedMilliseconds);
        //    //stopwatch2.Stop();
        //    //sw.Close();



        //    // # Access time atm ~ 2-3 seconds.

        //    // Solution #1: Upload DB and Web app to Azure. TESTED: Didn't work.

        //    // Solution #2: Is it possible to keep DB connection open permanently as in remove hand shakes? (Juha)

        //    // Solution #3: Look into SQL bulk inserts. This needs more coding but should be very effective. 

        //    // Solution #4: Create DataTable and add data from jsonClass into it. After that use SqlBulkCopy method to insert data into db.
        //    // ###########: This brings down access time from 5 seconds to under 0.2 seconds.
            
            
        //    // # Redirect back to Index. This method doesn't have a View.
        //    return RedirectToAction("Index");

        //    // ###########################################################################################################################
        //    // DEVELOPMENT PHASES OF DATABASE INSERT
        //    /*
        //    stopwatch.Stop();
        //    sw.Write("\n Elapsed time for creating the search to database is: " + stopwatch.ElapsedMilliseconds);


        //    stopwatch.Start();

        //    stopwatch.Stop();
        //    sw.Write("\n Elapsed time for finding the searchID is: " + stopwatch.ElapsedMilliseconds);

        //    int counter = 0;

        //    stopwatch.Start();
        //    using (var db3 = new weatherstationEntities())
        //    {
        //        // # For optimizing access time to DB.
        //        db3.Configuration.AutoDetectChangesEnabled = false;
        //        db3.Configuration.ValidateOnSaveEnabled = false;
        //        //# Going through the data class created from yr.no API json and inserting it into dbo.[data]
        //        foreach (var dataEntry in jsonConverted.properties.timeseries)
        //        {
        //            data dt = new data();
        //            dt.search_id = searchID;
        //            dt.provider_id = 2;
        //            dt.datatype_id = 1;
        //            dt.data_timestamp = dataEntry.time;
        //            dt.data_value = Convert.ToString(dataEntry.data.instant.details.air_temperature);
        //            db3.data.Add(dt);
        //            counter++;

        //            // # If SaveChanges() is used in this loop it pumps up the access time atleast three times.
        //            //db.SaveChanges();

        //            // # Limiting rows to 24.
        //            if (counter > 81)
        //            {
        //                break;
        //            }

        //        }
        //        db3.SaveChanges();
        //    }
        //    sw.Write("\nENTITY FRAMEWORK: Elapsed time for location called " + location + " foreach loop is: " + stopwatch.ElapsedMilliseconds);
        //    stopwatch.Stop();

        //    stopwatch.Start();
        //    db2.SaveChanges();
        //    sw.Write("\n Elapsed time for location called " + location + " SaveChanges() is: " + stopwatch.ElapsedMilliseconds);
        //    stopwatch.Stop();
        //    int counter2 = 0;

        //    stopwatch2.Start();
        //    using (var databaseConnection = new weatherstationEntities())
        //    {
        //        // # For optimizing access time to DB.
        //        databaseConnection.Configuration.AutoDetectChangesEnabled = false;
        //        databaseConnection.Configuration.ValidateOnSaveEnabled = false;
        //        using (SqlConnection connection = new SqlConnection(databaseConnection.Database.Connection.ConnectionString))
        //        {
        //            //sw.Close();


        //            String query = "INSERT INTO dbo.data (search_id,provider_id,datatype_id,data_timestamp,data_value) VALUES (@search_id,@provider_id,@datatype_id, @data_timestamp, @data_value)";
        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                connection.Open();


        //                foreach (var dataEntry in jsonConverted.properties.timeseries)
        //                {

        //                    counter2++;

        //                    command.Parameters.Add("@search_id", searchID);
        //                    command.Parameters.Add("@provider_id", 2);
        //                    command.Parameters.Add("@datatype_id", 1);
        //                    command.Parameters.Add("@data_timestamp", dataEntry.time);
        //                    command.Parameters.Add("@data_value", Convert.ToString(dataEntry.data.instant.details.air_temperature));
        //                    command.ExecuteNonQuery();
        //                    command.Parameters.Clear();


        //                    // Check Error
        //                    //    if (result < 0)
        //                    //    Console.WriteLine("Error inserting data into Database!");
        //                    if (counter2 > 81)
        //                    {
        //                        connection.Close();
        //                        break;
        //                    }


        //                }
        //                // # If SaveChanges() is used in this loop it pumps up the access time atleast three times.
        //                //db.SaveChanges();
        //            }
        //            // # Limiting rows to 24.


        //        }

        //    stopwatch2.Start();

        //    using (var databaseConnection2 = new weatherstationEntities())
        //    {
        //            // # For optimizing access time to DB.
        //            databaseConnection2.Configuration.AutoDetectChangesEnabled = false;
        //            databaseConnection2.Configuration.ValidateOnSaveEnabled = false;

        //            foreach (var dataEntry in jsonConverted.properties.timeseries)
        //            {
        //                var cmdText = db.data.Aggregate(
        //                    new StringBuilder(),
        //                    (sb, data) => sb.AppendLine($@"insert into dbo.data (search_id, provider_id, datatype_id, data_timestamp, data_value)
        //                        values('{searchID}', '{2}', '{1}', '{dataEntry.time}', '{Convert.ToString(dataEntry.data.instant.details.air_temperature)}')")
        //                    );


        //                using (SqlConnection connection2 = new SqlConnection(databaseConnection2.Database.Connection.ConnectionString))
        //                {
        //                    var command2 = new SqlCommand(cmdText.ToString(), connection2);
        //                    connection2.Open();
        //                    command2.ExecuteNonQuery();

        //                }
        //            }
        //    */

        //}

        public ActionResult EraseFromDB()
        {
            db.Database.ExecuteSqlCommand("DELETE FROM data");

            db.SaveChanges();

            return RedirectToAction("Index");

        }


       

                
    }
}

