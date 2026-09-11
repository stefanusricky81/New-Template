using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgetkuapi.Models
{
    public class CapexOpex
    {
        public class CapexOpexList
        {
            public int code { get; set; }
            public string msg { get; set; }
            public List<CapexOpexDetail> data { get; set; }
        }

        public class CapexOpexDetail
        {
            public List<Capex_Opex> capex { get; set; }
            public List<Capex_Opex> opex { get; set; }
            public string manager { get; set; }
            public string division { get; set; }
            public string block { get; set; }
            public string rut { get; set; }
        }

        public class Capex_Opex
        {
            public string act { get; set; }
            public string aop { get; set; }
            public string lastused { get; set; }
        }
    }
}