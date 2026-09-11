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
    public class CropController : ApiController
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
                        res = DataAccess.GetCrop(resToken.Rows[0]["ID"].ToString().Trim(), id);
                        if (res.Rows.Count > 0 && res != null)
                        {
                            decimal act = 0;
                            decimal aop = 0;
                            decimal remainingaop = 0;

                            List<Crop.CropDetail> list = new List<Crop.CropDetail>();
                            for (int i = 0; i <= res.Rows.Count - 1; i++)
                            {
                                Crop.CropDetail c = new Crop.CropDetail();

                                if (res.Rows[i]["Category"].ToString() == "C1")
                                    c.bgbolor = "#c1beea";
                                else
                                    c.bgbolor = "Transparent";

                                if (res.Rows[i]["AOP"].ToString() == "")
                                    aop = 0;
                                else
                                    aop = Convert.ToDecimal(res.Rows[i]["AOP"]);

                                if (res.Rows[i]["ACT"].ToString() == "")
                                {
                                    act = 0;
                                    remainingaop = 0 - aop;
                                }
                                else
                                {
                                    act = Convert.ToDecimal(res.Rows[i]["ACT"]);
                                    remainingaop = act - aop;
                                }

                                c.categorytype = res.Rows[i]["Category"].ToString();
                                c.category = getCropLabel(res.Rows[i]["Category"].ToString());
                                c.aop = DataAccess.getNumberFormat(aop.ToString());
                                c.act = DataAccess.getNumberFormat(act.ToString());
                                c.remainingaop = DataAccess.getNumberFormat(remainingaop.ToString());
                                if (aop == 0)
                                    c.remainingaoppercent = "100%";
                                else
                                    c.remainingaoppercent = DataAccess.getNumberFormat(Math.Round((remainingaop / aop) * 100).ToString()) + "%";
                                
                                list.Add(c);
                            }
                            

                            DateTime dat = Convert.ToDateTime(DataAccess.GetLastUpdate("PRODUKSI"));
                            model.code = 200;
                            model.msg = "Success";
                            model.lastupdate = "Report generated at "+ dat.ToString("dd MMM yyyy HH:mm:ss");
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

        private string getCropLabel(string code)
        {
            string label = string.Empty;

            if (code == "C1")
                label = "FFB Production (Ton)";
            else if (code == "C2")
                label = "Total Bunches (.000 Unit)";
            else if (code == "C3")
                label = "Round Panen";
            else if (code == "C4")
                label = "BJR (Kg)";
            else if (code == "C5")
                label = "Yield (Ton/Ha)";
            else if (code == "C6")
                label = "JPP";
            else if (code == "C7")
                label = "Total Loose Fruit (Ton)";
            else if (code == "C8")
                label = "Loose Fruit (%)";

            return label;
        }
    }
}
