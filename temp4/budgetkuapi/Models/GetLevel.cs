using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgetkuapi.Models
{
    public class GetLevel
    {
        public class GetLevelList
        {
            public int code { get; set; }
            public string msg { get; set; }
            public List<GetLevelDetail> data { get; set; }
        }
        public class GetLevelDetail
        {
            public int level { get; set; }

        }
    }
}