using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgetkuapi.Models
{
    public class Crop
    {
        public class CropList
        {
            public int code { get; set; }
            public string msg { get; set; }
            public string lastupdate { get; set; }
            public List<CropDetail> data { get; set; }
        }
        
        public class CropDetail
        {
            public string categorytype { get; set; }
            public string category { get; set; }
            public string act { get; set; }
            public string aop { get; set; }
            public string remainingaop { get; set; }
            public string remainingaoppercent { get; set; }
            public string bgbolor { get; set; }
        }
    }
}