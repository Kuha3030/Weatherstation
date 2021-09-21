using MVC_Sasema_test.Models;
using MVC_Sasema_test.Models3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Sasema_test.Controllers
{
    public class DBTools
    {

        // ## Methods for LINQ search statements 

        private static weatherstationEntities db = new weatherstationEntities();


        public static int GetSearchID(string label, string userID, string searchLocation)
        {

            int searchID = 0;

            // # Finds the last searchID made for user
            if (label == "latest_for_user")
            {
                searchID = (
                           from id in db.searches
                           where id.hash_id.Equals(userID)
                           orderby id.search_id descending
                           select id.search_id
                           ).Take(1).SingleOrDefault();
            }

            // # Finds the last searchID for searchLocation by "id_expires_[API]" cell. 
            // # It skips the searches where "id_expired_[API] tag is null. These searches are only valid for some other APIs.
            if (label == "yrno_latest_for_location")
            { 
                searchID = (
                    from id in db.searches
                    where id.input_location.Equals(searchLocation) & id.expires_yrno != null
                    orderby id.search_id descending
                    select id.search_id
                    ).Take(1).SingleOrDefault();
            }

            // # Explained above.
            if (label == "FMI_latest_for_location")
            {
                searchID = (
                    from id in db.searches
                    where id.input_location.Equals(searchLocation) & id.expires_FMI != null
                    orderby id.search_id descending
                    select id.search_id
                    ).Take(1).SingleOrDefault();
            }

            if (label == "foreca_latest_for_location")
            {
                searchID = (
                    from id in db.searches
                    where id.input_location.Equals(searchLocation) & id.expires_foreca != null
                    orderby id.search_id descending
                    select id.search_id
                    ).Take(1).SingleOrDefault();
            }

            if (label == "check_for_user")
            {
                searchID = (
                    from id in db.searches
                    where id.hash_id == userID & id.search_id != 0
                    select id.search_id
                    ).Take(1).SingleOrDefault();                    
            }

                return searchID;

        }

        public static DateTime CheckForUser(string userID)
        {

                var searchedTime = (
                    from id in db.searches
                    where id.hash_id == userID
                    orderby id.search_id descending
                    select id.timestamp
                    ).Take(1).SingleOrDefault();
            if (searchedTime != null)
            {
                DateTime returnThisDate = (DateTime)searchedTime;

                return returnThisDate;
            }
            else
            {
                return DateTime.UtcNow.AddMinutes(-6);
            }

        }

    }
}