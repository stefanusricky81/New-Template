using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgetkuapi.Models
{
    public class Estate
    {
        public class EstateList
        {
            public int code { get; set; }
            public string msg { get; set; }
            public List<EstateDetail> data { get; set; }
        }

        public class EstateDetail
        {
            public string bacode { get; set; }

            public string estatename { get; set; }
        }
    }
}