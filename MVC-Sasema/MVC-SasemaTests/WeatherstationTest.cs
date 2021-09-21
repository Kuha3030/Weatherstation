using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Sasema_test.Controllers;
using MVC_Sasema_test.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;

namespace MVC_SasemaTests
{
    [TestClass]
    public class GeneralToolsTest
    {

        // # Variables for TimeTools
        private const string RFC1123TimeSample = "Wed, 21 Jul 2021 07:54:50 GMT";
        private DateTime DateTimeSample = new DateTime(1984, 08, 29, 22, 35, 5);

        // # Variables for fetch methods:
        // This should be changed between tests so the APIs won't get spammed

       
        // # IP Address for hashing test
        //private const string IPAddressForHashing = "62.248.149.235";
        private const string IPAddressForHashing = "::1";


        [TestMethod]
        public void TimeToolsTest()
        {
            // # Test 1: Convert RFC1123 string into DateTime format.
            DateTime test = TimeTools.GMTStringToDateTime(RFC1123TimeSample);

            if (test.Hour != 7)
            {
                Assert.Fail("Failed to convert RFC1123 string into DateTime format.");
            }

            // # Test 2: Convert DateTime object to UTC.

            DateTime test2 = TimeTools.DateTimeToRFC1123(DateTimeSample);

            if (test2.Hour == DateTimeSample.Hour - 3 || test2.Hour == DateTimeSample.Hour - 2)
            {
                // Success
            }
            else
            {
                Assert.Fail("Failed to convert DateTime into UTC." + " Sample hours: " + DateTimeSample.Hour + " | Converted hours: " + test2.Hour);
            }
        }
        [TestMethod]

        public void HashTest()
        {
            string HashedIP = IPHashing.GetHashString(IPAddressForHashing);

            if (HashedIP.Length != 64)
            {
                Assert.Fail("Hashed string length wasn't 64.");
            }

        }
    }
    [TestClass]

    public class APITests
    {

        private const string location = "Karigasniemi";
        private Tuple<string, string> coordinates = new Tuple<string, string>("62.48", "24.39");
        [TestMethod]
        public void YrnoTest()
        {
            List<string> responseList = new List<string>();
            responseList = YrnoSearch.yrnoFetch2(coordinates, DateTime.Now);


            if (!(responseList.Count > 1))
            {
                if (responseList.Count == 0)
                {
                    Assert.Fail("Failed to get any response from method.");
                }
                if (responseList.Count == 1)
                {
                    Assert.Fail("Response from Yrno resulted in HTTP error: " + responseList[0]);
                }
            }
        }

        [TestMethod]
        public void FMITest()
        {
            List<string> responseList = new List<string>();
            string FMICoordinates = coordinates.Item1 + "," + coordinates.Item2;

            responseList = FMISearch.GetFMI2(FMICoordinates, DateTime.Now);


            if (!(responseList.Count > 1))
            {
                if (responseList.Count == 0)
                {
                    Assert.Fail("Failed to get any response from method.");
                }
                if (responseList.Count == 1)
                {
                    Assert.Fail("Response from FMI resulted in HTTP error: " + responseList[0]);
                }
            }
        }




        // # This failed at first because TestMethod couldn't find DB entity.
        // ## Testing project needed <connectionString> in app.config file AND Entityframework installataion

        [TestMethod]
        public void ForecaTest()
        {
            List<string> responseList = new List<string>();
            string ForecaCoordinates = coordinates.Item2.Replace(",", ".") + "," + coordinates.Item1.Replace(",", ".");


            string accessToken = ForecaSearch.GetForecaAccesstoken();
            responseList = ForecaSearch.forecaFetch(ForecaCoordinates, DateTime.Now, accessToken);


            if (!(responseList.Count > 1))
            {
                if (responseList.Count == 0)
                {
                    Assert.Fail("Failed to get any response from Foreca.");
                }
                if (responseList.Count == 1)
                {
                    Assert.Fail("Response from Foreca resulted in HTTP error: " + responseList[0]);
                }
            }
        }

        [TestMethod]

        public void GeoLocationTest()
        {
            List<string> responseList = new List<string>();
            responseList = GeoLocations.GetGeo(location);


            if (responseList.Count == 0)
            {
                Assert.Fail("Failed to get any response from method.");
            }
            if (responseList[0] == "error")
            {
                Assert.Fail("Response from method resulted in error: " + responseList[0]);
            }


        }


    }

    [TestClass]
    public class DatabaseTests
    {

        private const string IPAddressForHashing = "::1";
        private const string location = "Jämsä";
        private string userID = IPHashing.GetHashString(IPAddressForHashing);


        // # This requires search data from DB => Doesn't work on empty DB.
        // # Run the program and input your IP-address and used search location to TestClass variables.
        [TestMethod]

        public void DatabaseToolsTestCheckForUser()
        {

            var searchID = -1;
 

            try
            { 
            searchID = DBTools.GetSearchID("check_for_user", userID, location); 
            }

            catch (Exception e)
            {
                Assert.Fail("One of the methods threw exception:" + e);
            }
            if (searchID == 0)
            {
                Assert.Inconclusive("'check for user' method worked, but failed to find match from DB.");
            }
     
        }

        [TestMethod]

        public void DatabaseToolsTest()
        {
            var searchID2 = -1;

            try
            {
                searchID2 = DBTools.GetSearchID("latest_for_user", userID, location);
            }

            catch (Exception e)
            {
                Assert.Fail("One of the methods threw exception:" + e);
            }

            if (searchID2 == 0)
            {
                Assert.Inconclusive("'latest for user' method worked, but failed to find match from DB.");
            }
            if (searchID2 == -1)
            {
                Assert.Fail("'latest for user' method failed. Given searchID value didn't change.");
            }

          
        }
        [TestMethod]

        public void DatabaseToolsTestYrno()
        {
            string userID = IPHashing.GetHashString(IPAddressForHashing);

            var searchID3 = -1;

            try
            {
                searchID3 = DBTools.GetSearchID("yrno_latest_for_location", userID, location);
            }

            catch (Exception e)
            {
                Assert.Fail("One of the methods threw exception:" + e);
            }
    
            if (searchID3 == 0)
            {
                Assert.Inconclusive("'yrno_latest_for_location' method worked, but failed to find match from DB.");
            }
            if (searchID3 == -1)
            {
                Assert.Fail("'yrno_latest_for_location' method failed. Given searchID value didn't change.");
            }

        }
        [TestMethod]

        public void DatabaseToolsTestFMI()
        {
            string userID = IPHashing.GetHashString(IPAddressForHashing);


            var searchID4 = -1;

            try
            {
                searchID4 = DBTools.GetSearchID("FMI_latest_for_location", userID, location);
            }

            catch (Exception e)
            {
                Assert.Fail("One of the methods threw exception:" + e);
            }
 

            if (searchID4 == 0)
            {
                Assert.Inconclusive("'fmi_latest_for_location' method worked, but failed to find match from DB.");
            }
            if (searchID4 == -1)
            {
                Assert.Fail("'fmi_latest_for_location' method failed. Given searchID value didn't change.");
            }

  
        }
        [TestMethod]

        public void DatabaseToolsTestForeca()
        {
            var searchID5 = -1;

            try
            {
                searchID5 = DBTools.GetSearchID("foreca_latest_for_location", userID, location);
            }

            catch (Exception e)
            {
                Assert.Fail("One of the methods threw exception:" + e);
            }

            if (searchID5 == 0)
            {
                Assert.Inconclusive("'foreca_latest_for_location' method worked, but failed to find match from DB.");
            }
            if (searchID5 == -1)
            {
                Assert.Fail("'frno_latest_for_location' method failed. Given searchID value didn't change.");
            }
        }

    }


}


