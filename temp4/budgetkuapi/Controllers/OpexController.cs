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
    public class OpexController : ApiController
    {
        DataTable res, resToken = null;
        string token, id = string.Empty;

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

                    resToken = DataAccess.Level(token);
                    if (resToken.Rows.Count > 0)
                    {
                        string[] strid = id.Split('#');
                        if (Convert.ToInt32(strid[0].ToString().Trim()) == 1)
                        {
                            res = DataAccess.GetOpex(id);
                            if (res.Rows.Count > 1 && res != null)
                            {
                                List<Opex.OpexDetail> list = new List<Opex.OpexDetail>();
                                for (int i = 0; i <= res.Rows.Count - 1; i++)
                                {
                                    decimal remainingaop = 0;
                                    string color = string.Empty;
                                    if (res.Rows[i]["Category"].ToString() == "Total")
                                        color = "Bold";
                                    else
                                        color = "None";

                                    Opex.OpexDetail c = new Opex.OpexDetail();
                                    c.category = getOpexLabel(res.Rows[i]["Category"].ToString());
                                    c.act = DataAccess.getNumberFormat(res.Rows[i]["ACT"].ToString());
                                    c.aop = DataAccess.getNumberFormat(res.Rows[i]["AOP"].ToString());
                                    remainingaop = Convert.ToDecimal(res.Rows[i]["AOP"].ToString()) - Convert.ToDecimal(res.Rows[i]["ACT"].ToString());
                                    c.remainingaop = DataAccess.getNumberFormat((remainingaop).ToString());
                                    if (Convert.ToDecimal(res.Rows[i]["AOP"]) == 0)
                                        c.remainingaoppercent = "-100%";
                                    else
                                        c.remainingaoppercent = DataAccess.getNumberFormat(((remainingaop / Convert.ToDecimal(res.Rows[i]["AOP"])) * 100).ToString()) + "%";
                                    c.color = color;
                                    list.Add(c);
                                }

                                DateTime dat = Convert.ToDateTime(DataAccess.GetLastUpdate("BIAYA"));
                                model.code = 200;
                                model.msg = "Success";
                                model.lastupdate = "Report generated at " + dat.ToString("dd MMM yyyy HH:mm:ss");
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

        private string getOpexLabel(string lbl)
        {
            string label = string.Empty;

            if (lbl == "DEPRESIASI")
                label = "Depresiasi";
            else if (lbl == "FERTILIZING")
                label = "Pemupukan";
            else if (lbl == "UPKEEP")
                label = "Pemeliharaan";
            else if (lbl == "GENERALCOST")
                label = "General Charges";
            else if (lbl == "HARVESTING")
                label = "Panen";
            else if (lbl == "TRANSPORT")
                label = "Transport";
            else if (lbl == "Total")
                label = "Total Biaya Produksi";

            return label;
        }
    }
}
