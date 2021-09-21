using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Sasema_test.Models
{



    public class GeoObject
    {
        public Address address { get; set; }
    }

    public class Address
    {
        public string adminCode2 { get; set; }
        public string adminCode3 { get; set; }
        public string adminCode1 { get; set; }
        public string lng { get; set; }
        public string houseNumber { get; set; }
        public string locality { get; set; }
        public string adminCode4 { get; set; }
        public string adminName3 { get; set; }
        public string adminName2 { get; set; }
        public string street { get; set; }
        public string postalcode { get; set; }
        public string countryCode { get; set; }
        public string adminName1 { get; set; }
        public string lat { get; set; }
    }

}