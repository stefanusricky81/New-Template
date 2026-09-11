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
    public class FFBDetailController : ApiController
    {
        DataTable res, resToken = null;
        string token, id = string.Empty;

        public object Post(HttpRequestMessage request)
        {
            var model = new Crop.CropList();
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
                        res = DataAccess.GetFFBDetail(id);
                        if (res.Rows.Count > 0 && res != null)
                        {
                            decimal remainingaop = 0;
                            decimal act = 0;
                            string remainingaoppercent = string.Empty;
                            List<Crop.CropDetail> list = new List<Crop.CropDetail>();
                            for (int i = 0; i <= res.Rows.Count - 1; i++)
                            {
                                Crop.CropDetail c = new Crop.CropDetail();
                                c.category = CultureInfo.CreateSpecificCulture("id").DateTimeFormat.GetMonthName(Convert.ToInt32(res.Rows[i]["Period"]));
                                c.aop = DataAccess.getNumberFormat(res.Rows[i]["AOP"].ToString());
                                if (res.Rows[i]["ACT"].ToString() == "")
                                {
                                    c.act = "-";
                                    c.remainingaop = "-";
                                    c.remainingaoppercent = "-";
                                }
                                else
                                {
                                    if (res.Rows[i]["ACT"].ToString() == "0.000000")
                                        act = 0;
                                    else
                                        act = Convert.ToDecimal(res.Rows[i]["ACT"].ToString());

                                    c.act = DataAccess.getNumberFormat((Convert.ToDecimal(res.Rows[i]["ACT"])).ToString());
                                    remainingaop = act - Convert.ToDecimal(res.Rows[i]["AOP"]);
                                    c.remainingaop = DataAccess.getNumberFormat(remainingaop.ToString());
                                    remainingaoppercent = DataAccess.getNumberFormat(((remainingaop / Convert.ToDecimal(res.Rows[i]["AOP"])) * 100).ToString()) + "%";
                                    if (remainingaoppercent == "0%" || remainingaoppercent == "-0%")
                                        c.remainingaoppercent = DataAccess.getNumberFormat(((remainingaop / Convert.ToDecimal(res.Rows[i]["AOP"])) * 100).ToString()) + "%";
                                    else
                                        c.remainingaoppercent = remainingaoppercent;
                                }
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
