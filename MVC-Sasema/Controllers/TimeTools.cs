using MVC_Sasema_test.Models;
using MVC_Sasema_test.Models3;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MVC_Sasema_test.Controllers
{

    public class TimeTools
    {
        private static weatherstationEntities db = new weatherstationEntities();

        public static DateTime GMTStringToDateTime(string expiredTime)
        {
            // # Parse yr.no HTTP Status header "Expired" string to DateTime for DB:
            // # Example: "Wed, 21 Jul 2021 07:54:50 GMT";

            // # GMT String isn't modified when using ParseExact and InvariantCulture! Needs RFC1123Pattern to work.
            DateTime time = DateTime.ParseExact(expiredTime, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.InvariantCulture);    
            //DateTime.UtcNow.ToString("R")

            return time;
        
        }



        public static DateTime DateTimeToRFC1123(DateTime timeFromDB)
        {
            // # Parse DateTime (From DB) to RFC 1123 format.
            DateTime time = timeFromDB.ToUniversalTime();
            return time;
        }
        public static bool CheckExpiredYrno(string location)
        {

            bool isExpired = false;

            var expires = (
                from id in db.searches
                where id.input_location.Equals(location) & id.expires_yrno != null
                orderby id.search_id descending
                select id.expires_yrno
                ).Take(1).SingleOrDefault();

            if (expires == null)
            {
                isExpired = true;
            }
            // # Checks against UTCnow.
            else if (expires > DateTime.UtcNow)
            {
                isExpired = false;
            }
            else
            {
                isExpired = true;
            }
            return isExpired;
        }

        public static DateTime GetLastModifiedYrno(string location)
        {

            var lastModifiedFromDB = (
                from id in db.searches
                where id.input_location.Equals(location)
                orderby id.search_id descending
                select id.last_modified_yrno
                ).Take(1).SingleOrDefault();

            if (lastModifiedFromDB != null)
            {
                // # Doesn't modify the timecode. Only turns it into string
                return (DateTime)lastModifiedFromDB;
            }
            else
            {
                return DateTime.UtcNow.AddDays(-1);
            }
        }

        public static bool CheckExpiredFMI(string location)
        {

            bool isExpired = false;

            var expires = (
                from id in db.searches
                where id.input_location.Equals(location) & id.expires_FMI != null
                orderby id.search_id descending
                select id.expires_FMI
                ).Take(1).SingleOrDefault();

            if (expires == null)
            {
                isExpired = true;
            }
            else if (expires > DateTime.UtcNow)
            {
                isExpired = false;
            }
            else
            {
                isExpired = true;
            }
            return isExpired;
        }
        public static DateTime GetLastModifiedFMI(string location)
        {

            var lastModifiedFromDB = (
                from id in db.searches
                where id.input_location.Equals(location)
                orderby id.search_id descending
                select id.last_modified_FMI
                ).Take(1).SingleOrDefault();

            if (lastModifiedFromDB != null)
            {
                // # Doesn't modify the timecode. Only turns it into string
                return (DateTime)lastModifiedFromDB;
            }
            else
            {
                return DateTime.UtcNow.AddDays(-1);
            }

        }

        public static bool CheckExpiredForeca(string location)
        {

            bool isExpired = false;

            var expires = (
                from id in db.searches
                where id.input_location.Equals(location) & id.expires_foreca != null
                orderby id.search_id descending
                select id.expires_foreca
                ).Take(1).SingleOrDefault();

            if (expires == null)
            {
                isExpired = true;
            }
            else if (expires > DateTime.UtcNow)
            {
                isExpired = false;
            }
            else
            {
                isExpired = true;
            }
            return isExpired;
        }

        public static DateTime GetLastModifiedForeca(string location)
        {

            var lastModifiedFromDB = (
                from id in db.searches
                where id.input_location.Equals(location)
                orderby id.search_id descending
                select id.last_modified_foreca
                ).Take(1).SingleOrDefault();

            if (lastModifiedFromDB != null)
            {
                // # Doesn't modify the timecode. Only turns it into string
                return (DateTime)lastModifiedFromDB;
            }
            else
            {
                return DateTime.UtcNow.AddDays(-1);
            }

        }
    }
}