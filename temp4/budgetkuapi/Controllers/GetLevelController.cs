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
    public class GetLevelController : ApiController
    {
        DataTable res, resToken = null;
        string token = string.Empty;
        public object Post(HttpRequestMessage request)
        {
            var model = new GetLevel.GetLevelList();
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
                        List<GetLevel.GetLevelDetail> list = new List<GetLevel.GetLevelDetail>();
                        for (int i = 0; i <= res.Rows.Count - 1; i++)
                        {
                            GetLevel.GetLevelDetail lvl = new GetLevel.GetLevelDetail();
                            lvl.level = Convert.ToInt16(res.Rows[i]["LevelId"]);
                            list.Add(lvl);
                        }
                        model.code = 200;
                        model.msg = "Success";
                        model.data = list;
                        return model;
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
