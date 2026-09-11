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
    public class EstateController : ApiController
    {
        DataTable res, resToken = null;
        string token = string.Empty;

        public object Post(HttpRequestMessage request)
        {
            var model = new Estate.EstateList();
            if (request.Headers != null && request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Basic")
            {
                string auth_scheme = request.Headers.Authorization.Scheme;
                string auth_parameter = request.Headers.Authorization.Parameter;
                string content = request.Content.ReadAsStringAsync().Result; // body

                if (auth_parameter == "admin")
                {
                    var objects = JObject.Parse(content);
                    token = (string)objects["token"];

                    resToken = DataAccess.Level(token);
                    if (resToken.Rows.Count > 0)
                    {
                        res = DataAccess.GetEstateList();
                        if (res.Rows.Count > 0 && res != null)
                        {
                            List<Estate.EstateDetail> list = new List<Estate.EstateDetail>();
                            for (int i = 0; i <= res.Rows.Count - 1; i++)
                            {
                                Estate.EstateDetail ed = new Estate.EstateDetail();
                                ed.bacode = res.Rows[i]["BACode"].ToString();
                                ed.estatename = res.Rows[i]["EstateName"].ToString();
                                list.Add(ed);
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
