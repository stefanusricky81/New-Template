using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgetkuapi.Models
{
    public class Capex
    {
        public class CapexList
        {
            public int code { get; set; }
            public string msg { get; set; }
            public string lastupdate { get; set; }
            public List<CapexDetail> data { get; set; }
        }

        public class CapexDetail
        {
            public string assetcode { get; set; }
            public string asset { get; set; }
            public string originalaop { get; set; }
            public string adjustment { get; set; }
            public string cf { get; set; }
            public string netaop { get; set; }
            public string ytd { get; set; }
            public string remainingaop { get; set; }
            public string remainingaoppercent { get; set; }
            public string color { get; set; }
        }

        public class CapexAssetList
        {
            public int code { get; set; }
            public string msg { get; set; }
            public List<CapexAssetDetail> data { get; set; }
        }

        public class CapexAssetDetail
        {
            public string asset { get; set; }
            public string value { get; set; }
        }
    }
}