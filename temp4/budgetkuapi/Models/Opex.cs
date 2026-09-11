using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgetkuapi.Models
{
    public class Opex
    {
        public class OpexList
        {
            public int code { get; set; }
            public string msg { get; set; }
            public string lastupdate { get; set; }
            public List<OpexDetail> data { get; set; }
        }

        public class OpexDetail
        {
            public string category { get; set; }
            public string act { get; set; }
            public string aop { get; set; }
            public string color { get; set; }
            public string remainingaop { get; set; }
            public string remainingaoppercent { get; set; }
        }
    }
}