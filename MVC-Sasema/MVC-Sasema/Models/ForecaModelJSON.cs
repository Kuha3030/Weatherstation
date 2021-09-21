using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Sasema_test.Models
{
    public class ForecaModelJSON
    {
        public Forecast[] forecast { get; set; }
    }

    public class Forecast
    {
        public DateTime time { get; set; }
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
}