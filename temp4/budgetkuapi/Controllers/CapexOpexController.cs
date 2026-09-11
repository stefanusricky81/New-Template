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
    public class CapexOpexController : ApiController
    {
        DataTable res, resHome, resToken = null;
        string token, id = string.Empty;

        public object Post(HttpRequestMessage request)
        {
            var model = new CapexOpex.CapexOpexList();
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
                        res = DataAccess.GetCapexOpex(resToken.Rows[0]["ID"].ToString().Trim(),id);
                        if (res.Rows.Count > 0 && res != null)
                        {
                            List<CapexOpex.CapexOpexDetail> obj = new List<CapexOpex.CapexOpexDetail>();
                            CapexOpex.CapexOpexDetail objList = new CapexOpex.CapexOpexDetail();

                            List<CapexOpex.Capex_Opex> capex = new List<CapexOpex.Capex_Opex>();
                            for (int i = 0; i <= res.Rows.Count - 1; i++)
                            {
                                decimal cera = 0;
                                if (res.Rows[i]["CERA"].ToString() == "")
                                    cera = 0;
                                else
                                    cera = Convert.ToDecimal(res.Rows[i]["CERA"].ToString());

                                CapexOpex.Capex_Opex c = new CapexOpex.Capex_Opex();
                                c.aop = DataAccess.getMoneyFormat((Convert.ToDecimal(res.Rows[i]["ValueOriCapex"]) - Convert.ToDecimal(res.Rows[i]["ValueAdjusment"])
                                    - Convert.ToDecimal(res.Rows[i]["ValueCarryForward"]) - cera).ToString());
                                c.act = DataAccess.getMoneyFormat((Convert.ToDecimal(res.Rows[i]["ValueCarryForward"]) + cera).ToString());
                                if (res.Rows[i]["LastUsedCERA"].ToString() != "")
                                {
                                    DateTime dat = Convert.ToDateTime(res.Rows[i]["LastUsedCERA"].ToString());
                                    c.lastused = "(terakhir tgl " + dat.ToString("dd MMM yyyy") + ")";
                                }
                                else
                                    c.lastused = "(terakhir tgl N/A)";
                                
                                capex.Add(c);
                            }

                            List<CapexOpex.Capex_Opex> opex = new List<CapexOpex.Capex_Opex>();
                            for (int i = 0; i <= res.Rows.Count - 1; i++)
                            {
                                CapexOpex.Capex_Opex c = new CapexOpex.Capex_Opex();
                                if (res.Rows[i]["OPEXAOP"].ToString() == "")
                                    c.aop = "N/A";
                                else
                                    c.aop = DataAccess.getMoneyFormat(res.Rows[i]["OPEXAOP"].ToString());

                                if (res.Rows[i]["OPEXACT"].ToString() == "")
                                    c.act = "N/A";
                                else
                                    c.act = DataAccess.getMoneyFormat(res.Rows[i]["OPEXACT"].ToString());
                                if (res.Rows[i]["LastUsedOPEX"].ToString() != "")
                                {
                                    c.lastused = "(terakhir " + CultureInfo.CreateSpecificCulture("id").DateTimeFormat.GetMonthName(Convert.ToInt32(res.Rows[i]["LastUsedOPEX"])) +" "+ DateTime.Now.Year.ToString()+")";
                                }
                                else
                                    c.lastused = "(terakhir tgl N/A)";

                                opex.Add(c);
                            }

                            string[] strid = id.Split('#');
                            int level = Convert.ToInt32(strid[0].ToString().Trim());
                            if (level == 1)
                            {
                                resHome = DataAccess.GetOthersInfoHome(id);
                                if (resHome.Rows.Count == 1) objList.manager = resHome.Rows[0]["Dt"].ToString();
                                else if (resHome.Rows.Count == 4) objList.manager = resHome.Rows[3]["Dt"].ToString();
                                else objList.manager = "N/A";
                                objList.division = (resHome.Rows.Count == 1) ? "-" : resHome.Rows[1]["Dt"].ToString();
                                objList.block = (resHome.Rows.Count == 1) ? "-" : resHome.Rows[0]["Dt"].ToString();
                                objList.rut = (resHome.Rows.Count == 1) ? "-" : resHome.Rows[2]["Dt"].ToString();
                            }

                            objList.capex = capex;
                            objList.opex = opex;
                            obj.Add(objList);

                            model.code = 200;
                            model.msg = "Success";
                            model.data = obj;
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
