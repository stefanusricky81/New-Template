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
    public class DdlController : ApiController
    {
        DataTable resCompany, resRegion, resAma, resEstate, resToken, resZone, resVP = null;
        string token, user = string.Empty;

        public object Post(HttpRequestMessage request)
        {
            var model = new Item.ItemList();
            if (request.Headers != null && request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Basic")
            {
                string auth_scheme = request.Headers.Authorization.Scheme;
                string auth_parameter = request.Headers.Authorization.Parameter;
                string content = request.Content.ReadAsStringAsync().Result; // body

                if (auth_parameter == "admin")
                {
                    var objects = JObject.Parse(content);
                    token = (string)objects["token"];
                    user = (string)objects["user"];

                    resToken = DataAccess.Level(token);
                    if (resToken.Rows.Count > 0)
                    {
                        if (resToken.Rows[0]["LevelId"].ToString().Trim() == "5")
                        {
                            //get company
                            resCompany = DataAccess.GetCompanyList();
                            if (resCompany.Rows.Count > 0)
                            {
                                List<Item.ItemDetail> list = new List<Item.ItemDetail>();
                                for (int i = 0; i <= resCompany.Rows.Count - 1; i++)
                                {
                                    Item.ItemDetail co = new Item.ItemDetail();
                                    co.id = "4#" + resCompany.Rows[i]["ID"].ToString().Trim();
                                    co.name = resCompany.Rows[i]["Name"].ToString();
                                    list.Add(co);

                                    //get region
                                    resRegion = DataAccess.GetRegionList(resCompany.Rows[i]["ID"].ToString().Trim());
                                    if (resRegion.Rows.Count > 0)
                                    {
                                        for (int j = 0; j <= resRegion.Rows.Count - 1; j++)
                                        {
                                            Item.ItemDetail re = new Item.ItemDetail();
                                            re.id = "3#" + resRegion.Rows[j]["ID"].ToString().Trim();
                                            re.name = "> " + resRegion.Rows[j]["Name"].ToString();
                                            list.Add(re);

                                            //get ama
                                            resAma = DataAccess.GetAMAList(resCompany.Rows[i]["ID"].ToString().Trim(),
                                                resRegion.Rows[j]["ID"].ToString().Trim());
                                            if (resAma.Rows.Count > 0)
                                            {
                                                for (int k = 0; k <= resAma.Rows.Count - 1; k++)
                                                {
                                                    Item.ItemDetail am = new Item.ItemDetail();
                                                    am.id = "2#" + resAma.Rows[k]["ID"].ToString().Trim();
                                                    am.name = ">> " + resAma.Rows[k]["Name"].ToString();
                                                    list.Add(am);

                                                    //get estate
                                                    resEstate = DataAccess.GetBAcodeList(resCompany.Rows[i]["ID"].ToString().Trim(),
                                                        resRegion.Rows[j]["ID"].ToString().Trim(), resAma.Rows[k]["ID"].ToString().Trim());
                                                    if (resEstate.Rows.Count > 0)
                                                    {
                                                        for (int l = 0; l <= resEstate.Rows.Count - 1; l++)
                                                        {
                                                            Item.ItemDetail es = new Item.ItemDetail();
                                                            es.id = "1#" + resEstate.Rows[l]["ID"].ToString().Trim();
                                                            es.name = ">>> " + resEstate.Rows[l]["Name"].ToString();
                                                            list.Add(es);
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
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
                        else if (resToken.Rows[0]["LevelId"].ToString().Trim() == "2")
                        {
                            List<Item.ItemDetail> list = new List<Item.ItemDetail>();
                            //get zone
                            resZone = DataAccess.GetZone(resToken.Rows[0]["ID"].ToString().Trim(), user, resToken.Rows[0]["LevelId"].ToString().Trim());
                            if (resZone.Rows.Count > 0)
                            {
                                for (int m = 0; m <= resZone.Rows.Count - 1; m++)
                                {
                                    Item.ItemDetail zone = new Item.ItemDetail();
                                    zone.id = "2#" + resZone.Rows[m]["ID"].ToString().Trim();
                                    zone.name = resZone.Rows[m]["Name"].ToString();
                                    list.Add(zone);

                                    //get bacode
                                    resEstate = DataAccess.GetBAcodeList(resToken.Rows[0]["ID"].ToString().Trim(),
                                                                            resZone.Rows[m]["RegionID"].ToString().Trim(),
                                                                            resZone.Rows[m]["ID"].ToString().Trim());
                                    if (resEstate.Rows.Count > 0)
                                    {
                                        for (int k = 0; k <= resEstate.Rows.Count - 1; k++)
                                        {
                                            Item.ItemDetail estate = new Item.ItemDetail();
                                            estate.id = "1#" + resEstate.Rows[k]["ID"].ToString().Trim();
                                            estate.name = "> " + resEstate.Rows[k]["Name"].ToString();
                                            list.Add(estate);
                                        }
                                    }
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
                        else if (resToken.Rows[0]["LevelId"].ToString().Trim() == "3")
                        {
                            List<Item.ItemDetail> list = new List<Item.ItemDetail>();
                            //get zone
                            resZone = DataAccess.GetZone(resToken.Rows[0]["ID"].ToString().Trim(), user, resToken.Rows[0]["LevelId"].ToString().Trim());
                            if (resZone.Rows.Count > 0)
                            {
                                for (int m = 0; m <= resZone.Rows.Count - 1; m++)
                                {
                                    Item.ItemDetail zone = new Item.ItemDetail();
                                    zone.id = "5#" + resZone.Rows[m]["ID"].ToString().Trim();
                                    zone.name = resZone.Rows[m]["Name"].ToString();
                                    list.Add(zone);

                                    //get ama
                                    resAma = DataAccess.GetAMAByZone(resToken.Rows[0]["ID"].ToString().Trim(),
                                                                     resZone.Rows[m]["ID"].ToString().Trim());
                                    if (resAma.Rows.Count > 0)
                                    {
                                        for (int j = 0; j <= resAma.Rows.Count - 1; j++)
                                        {
                                            Item.ItemDetail ama = new Item.ItemDetail();
                                            ama.id = "2#" + resAma.Rows[j]["ID"].ToString().Trim();
                                            ama.name = "> " + resAma.Rows[j]["Name"].ToString();
                                            list.Add(ama);

                                            //get bacode
                                            resEstate = DataAccess.GetBAcodeList(resToken.Rows[0]["ID"].ToString().Trim(),
                                                                                    resAma.Rows[j]["RegionID"].ToString().Trim(),
                                                                                    resAma.Rows[j]["ID"].ToString().Trim());
                                            if (resEstate.Rows.Count > 0)
                                            {
                                                for (int k = 0; k <= resEstate.Rows.Count - 1; k++)
                                                {
                                                    Item.ItemDetail estate = new Item.ItemDetail();
                                                    estate.id = "1#" + resEstate.Rows[k]["ID"].ToString().Trim();
                                                    estate.name = ">> " + resEstate.Rows[k]["Name"].ToString();
                                                    list.Add(estate);
                                                }
                                            }
                                        }
                                    }
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
                        else if (resToken.Rows[0]["LevelId"].ToString().Trim() == "4")
                        {
                            List<Item.ItemDetail> list = new List<Item.ItemDetail>();
                            //get zone
                            resZone = DataAccess.GetZone(resToken.Rows[0]["ID"].ToString().Trim(), user, "3");
                            if (resZone.Rows.Count > 0)
                            {
                                for (int m = 0; m <= resZone.Rows.Count - 1; m++)
                                {
                                    Item.ItemDetail zone = new Item.ItemDetail();
                                    zone.id = "5#" + resZone.Rows[m]["ID"].ToString().Trim();
                                    zone.name = resZone.Rows[m]["Name"].ToString();
                                    list.Add(zone);

                                    //get VP
                                    resVP = DataAccess.GetZone(resToken.Rows[0]["ID"].ToString().Trim(), user, resToken.Rows[0]["LevelId"].ToString().Trim());
                                    if (resVP.Rows.Count > 0)
                                    {
                                        for (int o = 0; o <= resVP.Rows.Count - 1; o++)
                                        {
                                            Item.ItemDetail vp = new Item.ItemDetail();
                                            vp.id = "6#" + resVP.Rows[o]["ID"].ToString().Trim();
                                            vp.name = resVP.Rows[o]["Name"].ToString();
                                            list.Add(vp);

                                            //get ama
                                            resAma = DataAccess.GetAMAByZone(resToken.Rows[0]["ID"].ToString().Trim(),
                                                                             resVP.Rows[o]["ID"].ToString().Trim());
                                            if (resAma.Rows.Count > 0)
                                            {
                                                for (int j = 0; j <= resAma.Rows.Count - 1; j++)
                                                {
                                                    Item.ItemDetail ama = new Item.ItemDetail();
                                                    ama.id = "2#" + resAma.Rows[j]["ID"].ToString().Trim();
                                                    ama.name = "> " + resAma.Rows[j]["Name"].ToString();
                                                    list.Add(ama);

                                                    //get bacode
                                                    resEstate = DataAccess.GetBAcodeList(resToken.Rows[0]["ID"].ToString().Trim(),
                                                                                            resAma.Rows[j]["RegionID"].ToString().Trim(),
                                                                                            resAma.Rows[j]["ID"].ToString().Trim());
                                                    if (resEstate.Rows.Count > 0)
                                                    {
                                                        for (int k = 0; k <= resEstate.Rows.Count - 1; k++)
                                                        {
                                                            Item.ItemDetail estate = new Item.ItemDetail();
                                                            estate.id = "1#" + resEstate.Rows[k]["ID"].ToString().Trim();
                                                            estate.name = ">> " + resEstate.Rows[k]["Name"].ToString();
                                                            list.Add(estate);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
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
                            List<Item.ItemDetail> list = new List<Item.ItemDetail>();
                            //get zone
                            resZone = DataAccess.GetZone(resToken.Rows[0]["ID"].ToString().Trim(), user, resToken.Rows[0]["LevelId"].ToString().Trim());
                            if (resZone.Rows.Count > 0)
                            {
                                for (int m = 0; m <= resZone.Rows.Count - 1; m++)
                                {
                                    Item.ItemDetail zone = new Item.ItemDetail();
                                    zone.id = "1#" + resZone.Rows[m]["ID"].ToString().Trim();
                                    zone.name = resZone.Rows[m]["Name"].ToString();
                                    list.Add(zone);
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
