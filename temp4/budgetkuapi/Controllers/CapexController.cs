using budgetkuapi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace budgetkuapi.Controllers
{
    public class CapexController : ApiController
    {
        DataTable res, resToken = null;
        string token, id = string.Empty;

        public object Post(HttpRequestMessage request)
        {
            var model = new Capex.CapexList();
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
                            res = DataAccess.GetCapex(id);
                            if (res.Rows.Count > 1 && res != null)
                            {
                                List<Capex.CapexDetail> list = new List<Capex.CapexDetail>();
                                for (int i = 0; i <= res.Rows.Count - 1; i++)
                                {
                                    decimal cera = 0;
                                    decimal tempCera = 0;
                                    if (res.Rows[i]["CERA"].ToString() == "")
                                    {
                                        cera = 0;
                                        tempCera = 0;
                                    }
                                    else
                                    {
                                        cera = Convert.ToDecimal(res.Rows[i]["CERA"]) + DataAccess.GetOtherAsset(id, res.Rows[i]["AssetClass"].ToString());
                                        tempCera = Convert.ToDecimal(res.Rows[i]["CERA"]);
                                    }

                                    decimal remainingaop = 0;
                                    if (res.Rows[i]["AssetClass"].ToString() == "")
                                        cera = tempCera + DataAccess.GetOtherAsset(id, "40") + DataAccess.GetOtherAsset(id, "50");

                                    string color = string.Empty;
                                    if (res.Rows[i]["Asset"].ToString() == "Total Capex")
                                        color = "Bold";
                                    else
                                        color = "None";

                                    Capex.CapexDetail c = new Capex.CapexDetail();
                                    c.assetcode = res.Rows[i]["AssetClass"].ToString();
                                    c.asset = res.Rows[i]["Asset"].ToString();
                                    c.originalaop = DataAccess.getNumberFormat(res.Rows[i]["OriAOP"].ToString());
                                    c.adjustment = DataAccess.getNumberFormat(res.Rows[i]["Adjustment"].ToString());
                                    c.cf = DataAccess.getNumberFormat(res.Rows[i]["CF"].ToString());
                                    c.netaop = DataAccess.getNumberFormat((Convert.ToDecimal(res.Rows[i]["OriAOP"]) - Convert.ToDecimal(res.Rows[i]["Adjustment"])).ToString());
                                    c.ytd = DataAccess.getNumberFormat((Convert.ToDecimal(res.Rows[i]["CF"]) + cera).ToString());
                                    remainingaop = (Convert.ToDecimal(res.Rows[i]["OriAOP"]) - Convert.ToDecimal(res.Rows[i]["CF"]) - cera);
                                    c.remainingaop = DataAccess.getNumberFormat(remainingaop.ToString());
                                    if (Convert.ToDecimal(res.Rows[i]["OriAOP"]) == 0)
                                        c.remainingaoppercent = "0%";
                                    else
                                        c.remainingaoppercent = DataAccess.getNumberFormat(((remainingaop / Convert.ToDecimal(res.Rows[i]["OriAOP"])) * 100).ToString()) + "%";
                                    c.color = color;
                                    list.Add(c);
                                }

                                DateTime dat = Convert.ToDateTime(DataAccess.GetLastUpdate("CAPEX"));
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
    }
}
