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
    public class CapexDetailController : ApiController
    {
        DataTable res, resToken = null;
        string token, id, asset = string.Empty;

        public object Post(HttpRequestMessage request)
        {
            var model = new Capex.CapexAssetList();
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
                    asset= (string)objects["asset"];

                    resToken = DataAccess.Level(token);
                    if (resToken.Rows.Count > 0)
                    {
                        res = DataAccess.GetCapexDetail(id, asset);
                        if (res.Rows.Count > 0 && res != null)
                        {
                            List<Capex.CapexAssetDetail> list = new List<Capex.CapexAssetDetail>();
                            for (int i = 0; i <= res.Rows.Count - 1; i++)
                            {
                                Capex.CapexAssetDetail c = new Capex.CapexAssetDetail();
                                c.asset = res.Rows[i]["Asset"].ToString();
                                if (res.Rows[i]["Value"].ToString() != "")
                                    c.value = DataAccess.getNumberFormat(res.Rows[i]["Value"].ToString());
                                else
                                    c.value = "0";
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
