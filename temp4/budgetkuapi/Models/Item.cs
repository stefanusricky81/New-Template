using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgetkuapi.Models
{
    public class Item
    {
        public class ItemList
        {
            public int code { get; set; }
            public string msg { get; set; }
            public List<ItemDetail> data { get; set; }
        }

        public class ItemDetail
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}