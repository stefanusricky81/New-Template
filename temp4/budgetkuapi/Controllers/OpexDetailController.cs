using budgetkuapi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace budgetkuapi.Controllers
{
    public class OpexDetailController : ApiController
    {
        DataTable res, resToken = null;
        string token, id, category = string.Empty;

        private string getOpexLabel(string lbl)
        {
            string label = string.Empty;

            if (lbl == "Depresiasi")
                label = "DEPRESIASI";
            else if (lbl == "Pemupukan")
                label = "FERTILIZING";
            else if (lbl == "Pemeliharaan")
                label = "UPKEEP";
            else if (lbl == "General Charges")
                label = "GENERALCOST";
            else if (lbl == "Panen")
                label = "HARVESTING";
            else if (lbl == "Transport")
                label = "TRANSPORT";
            else if (lbl == "Total Biaya Produksi")
                label = "Total";

            return label;
        }

        public object Post(HttpRequestMessage request)
        {
            var model = new Opex.OpexList();
            if (request.Headers != null && request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Basic")
            {
                string auth_scheme = request.Headers.Authorization.Scheme;
                string auth_parameter = request.Headers.Authorization.Parameter;
                string content = request.Content.ReadAsStringAsync().Result; // body

                if (auth_parameter == "admin")
                {
                    var objects = JObject.Parse(content);
                    token = (string)objects["token"];
                    id = (string)objects["id"];
                    category = (string)objects["category"];

                    resToken = DataAccess.Level(token);
                    if (resToken.Rows.Count > 0 )
                    {
                        res = DataAccess.GetOpexDetail(id, getOpexLabel(category));
                        if (res.Rows.Count > 1 && res != null)
                        {
                            decimal remainingaop = 0;
                            decimal act = 0;
                            string remainingaoppercent = string.Empty;
                            List<Opex.OpexDetail> list = new List<Opex.OpexDetail>();
                            for (int i = 0; i <= res.Rows.Count - 1; i++)
                            {
                                Opex.OpexDetail c = new Opex.OpexDetail();
                                c.category = CultureInfo.CreateSpecificCulture("id").DateTimeFormat.GetMonthName(Convert.ToInt32(res.Rows[i]["Period"]));
                                if (res.Rows[i]["ACT"].ToString() == "" || Convert.ToDecimal(res.Rows[i]["ACT"])  == 0)
                                {
                                    act = 0;
                                    c.act = act.ToString();
                                }
                                else
                                {
                                    act = Convert.ToDecimal(res.Rows[i]["ACT"]);
                                    c.act = DataAccess.getNumberFormat(act.ToString());
                                }
                                c.aop = DataAccess.getNumberFormat(res.Rows[i]["AOP"].ToString());
                                remainingaop =  Convert.ToDecimal(res.Rows[i]["AOP"]) - act ;
                                c.remainingaop = DataAccess.getNumberFormat(remainingaop.ToString());
                                remainingaoppercent = DataAccess.getNumberFormat(((remainingaop / Convert.ToDecimal(res.Rows[i]["AOP"])) * 100).ToString()) + "%";
                                if (remainingaoppercent == "0%" || remainingaoppercent == "-0%")
                                    c.remainingaoppercent = DataAccess.getNumberFormat(((remainingaop / Convert.ToDecimal(res.Rows[i]["AOP"])) * 100).ToString()) + "%";
                                else
                                    c.remainingaoppercent = remainingaoppercent;

                                list.Add(c);
                            }
                            
                            model.code = 200;
                            model.msg = "Success";
                            model.data = list;
                            return model;
                        }
                        else
                        {
                            model.code = 400;
                            model.msg = "Oops!!! No data found";
                            return model;
                        }
                    }
                    else
                    {
                        model.code = 403;
                        model.msg = "Gotcha!!! Your mobile not register yet";
                        return model;
                    }
                }
                else
                {
                    model.code = 401;
                    model.msg = "Oops!!! Invalid API Key";
                    return model;
                }
            }
            else
            {
                model.code = 401;
                model.msg = "Oops!!! API Key is missing";
                return model;
            }
        }
    }
}
