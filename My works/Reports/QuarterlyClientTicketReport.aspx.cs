using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using eis = Telerik.Web.UI.ExportInfrastructure;

public partial class Reports_QuarterlyClientTicketReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            binddl();
        }
    }

    protected void lbexport_Click(object sender, EventArgs e)
    {
        try
        {
            int month = DateTime.ParseExact(ddlMonths.SelectedValue, "MM/dd/yyyy", null).Month;
            System.Data.DataTable dtall = GetAllData();
            int _nom = int.Parse(ddlNoM.SelectedValue);

            Exporttocsv(dtall, month, _nom);

        }
        catch (Exception ex)
        {
            return;
        }
    }
    protected static string getFullName(int month, int lasts)
    {
        DateTime date = new DateTime(DateTime.Now.Year, month, 1);

        return date.AddMonths(lasts).ToString("MMMM");
    }

    protected static int getMonth(int month, int lasts)
    {
        DateTime date = new DateTime(DateTime.Now.Year, month, 1);

        return date.AddMonths(lasts).Month;
    }

    protected static string getheaderTotalRequest(string monthno, int month, int nom, int crit, int high, int normal, int low)
    {
        int _crit = 0;
        int _high = 0;
        int _normal = 0;
        int _low = 0;
        string _allTotalRequest = string.Empty;
        for (int x = nom; x > 1; x--)
        {
            if (monthno == getMonth(month, -(nom - x)).ToString())
            {
                _crit += crit;
                _high += high;
                _normal += normal;
                _low += low;
            }
        }
        _allTotalRequest += "," + _crit + "," + _high + "," + _normal + "," + _low;
        return _allTotalRequest;
    }

    public void Exporttocsv(DataTable dt, int month, int nom)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            string header1 = string.Empty;
            string header2 = string.Empty;
            string headerTotalRequest = "Total Helpdesk Requests";
            string headerViprequest = "VIP Requests";
            string fieldVIPRequest = string.Empty;
            string headerResponseTime = "Response Times (minutes)";
            string fieldResposeTime = string.Empty;
            string headerResolution = "Resolution";
            string fieldResolution = string.Empty;
            string headerUsercategory = "User Ticket Category";
            string fieldUsercategory = string.Empty;

            int col = nom * 4;
            #region header
            for (int i = 0; i <= col; i++)
            {
                if (i == 0)
                    header1 = "";
                else if (i > 0 && i < 5)
                    header1 += "," + getFullName(month, -(nom - 1));
                else if (i > 4 && i < 9)
                    header1 += "," + getFullName(month, -(nom - 2));
                else if (i > 8 && i < 13)
                    header1 += "," + getFullName(month, -(nom - 3));
                else if (i > 12 && i < 17)
                    header1 += "," + getFullName(month, -(nom - 4));
                else if (i > 16 && i < 21)
                    header1 += "," + getFullName(month, -(nom - 5));
                else if (i > 20 && i < 25)
                    header1 += "," + getFullName(month, -(nom - 6));
                else if (i > 24 && i < 29)
                    header1 += "," + getFullName(month, -(nom - 7));
                else if (i > 28 && i < 33)
                    header1 += "," + getFullName(month, -(nom - 8));
                else if (i > 32 && i < 37)
                    header1 += "," + getFullName(month, -(nom - 9));
                else if (i > 36 && i < 41)
                    header1 += "," + getFullName(month, -(nom - 10));
                else if (i > 40 && i < 45)
                    header1 += "," + getFullName(month, -(nom - 11));
                else if (i > 44 && i < 49)
                    header1 += "," + getFullName(month, -(nom - 12));

                if (i == 0)
                    header2 = "";
                else if (i == 1 || i == 5 || i == 9 || i == 13 || i == 17 || i == 21 || i == 25 || i == 29 || i == 33 || i == 37 || i == 41 || i == 45)
                    header2 += "," + "Critical";
                else if (i == 2 || i == 6 || i == 10 || i == 14 || i == 18 || i == 22 || i == 26 || i == 30 || i == 34 || i == 38 || i == 42 || i == 46)
                    header2 += "," + "High";
                else if (i == 3 || i == 7 || i == 11 || i == 15 || i == 19 || i == 23 || i == 27 || i == 31 || i == 35 || i == 39 || i == 43 || i == 47)
                    header2 += "," + "Normal";
                else if (i == 4 || i == 8 || i == 12 || i == 16 || i == 20 || i == 24 || i == 28 || i == 32 || i == 36 || i == 40 || i == 44 || i == 48)
                    header2 += "," + "Low";
            }
            sb.AppendLine(header1);
            sb.AppendLine(header2);
            #endregion

            #region field
            headerTotalRequest += AppendHeaders(dt, month, "1", nom);
            sb.AppendLine(headerTotalRequest);
            sb.AppendLine("Email" + AppendDetail(dt, month, "1", "email", nom));
            sb.AppendLine("Phone" + AppendDetail(dt, month, "1", "phone", nom));
            sb.AppendLine("Voice" + AppendDetail(dt, month, "1", "voice", nom));
            sb.AppendLine("Web" + AppendDetail(dt, month, "1", "web", nom));            

            headerResolution += AppendHeaders(dt, month, "2", nom);
            sb.AppendLine(headerResolution);
            sb.AppendLine("Closed" + AppendDetail(dt, month, "2", "closed", nom));
            sb.AppendLine("Open" + AppendDetail(dt, month, "2", "open", nom));

            //headerResponseTime += AppendHeaders(dt, month, "3", nom);
            sb.AppendLine(headerResponseTime);
            sb.AppendLine("AVG Time/issue" + AppendDetail(dt, month, "3", "avg response time", nom));
            sb.AppendLine("Longest Time/Issue" + AppendDetail(dt, month, "3", "longest response time", nom));

            //headerViprequest += AppendHeaders(dt, month, "4", nom);
            sb.AppendLine(headerViprequest);
            sb.AppendLine("VIP Tickets" + AppendDetail(dt, month, "4", "vip", nom));
            
            headerUsercategory += AppendHeaders(dt, month, "5", nom);
            sb.AppendLine(headerUsercategory);
            List<UserCategory> usercategoryList = new List<UserCategory>();
            UserCategory usercategory = new UserCategory();
            bool exist = false;

            for (int x = 0; x <= dt.Rows.Count - 1; x++)
            {
                if (dt.Rows[x]["OrderId1"].ToString().Trim() == "5")
                {
                    if (usercategoryList.Count == 0)
                    {
                        usercategoryList.Add(new UserCategory { Detail = dt.Rows[x]["Detail"].ToString() });
                    }
                    else
                    {
                        for (int z = 0; z <= usercategoryList.Count - 1; z++)
                        {
                            if (dt.Rows[x]["Detail"].ToString() == usercategoryList[z].Detail)
                            {
                                exist = true;
                                break;
                            }
                            else
                                exist = false;
                        }

                        if (exist == false)
                        {
                            usercategoryList.Add(new UserCategory { Detail = dt.Rows[x]["Detail"].ToString() });
                        }
                    }
                }
            }
            List<UserCategory> SortedCategoryList = usercategoryList.OrderBy(o => o.Detail).ToList();

            for (int y = 0; y <= SortedCategoryList.Count - 1; y++)
            {
                sb.AppendLine(SortedCategoryList[y].Detail + AppendDetail(dt, month, "5", SortedCategoryList[y].Detail.ToString().Trim().ToLower(),nom));
            }
            #endregion

            Response.Clear();
            Response.ContentType = "application/CSV";
            Response.AddHeader("content-disposition", "attachment; filename=\"" + "QuarterlyReport" + ".csv\"");
            Response.Write(sb.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            litMessage.Text = ex.Message;
            return;
        }
    }
    protected string AppendHeaders(DataTable dt, int month, string orderid1, int nom)
    {
        int _critmonth11 = 0; int _highmonth11 = 0; int _normalmonth11 = 0; int _lowmonth11 = 0;
        int _critmonth10 = 0; int _highmonth10 = 0; int _normalmonth10 = 0; int _lowmonth10 = 0;
        int _critmonth9 = 0; int _highmonth9 = 0; int _normalmonth9 = 0; int _lowmonth9 = 0;
        int _critmonth8 = 0; int _highmonth8 = 0; int _normalmonth8 = 0; int _lowmonth8 = 0;
        int _critmonth7 = 0; int _highmonth7 = 0; int _normalmonth7 = 0; int _lowmonth7 = 0;
        int _critmonth6 = 0; int _highmonth6 = 0; int _normalmonth6 = 0; int _lowmonth6 = 0;
        int _critmonth5 = 0; int _highmonth5 = 0; int _normalmonth5 = 0; int _lowmonth5 = 0;
        int _critmonth4 = 0; int _highmonth4 = 0; int _normalmonth4 = 0; int _lowmonth4 = 0;
        int _critmonth3 = 0; int _highmonth3 = 0; int _normalmonth3 = 0; int _lowmonth3 = 0;
        int _critmonth2 = 0; int _highmonth2 = 0; int _normalmonth2 = 0; int _lowmonth2 = 0;
        int _critmonth1 = 0; int _highmonth1 = 0; int _normalmonth1 = 0; int _lowmonth1 = 0;
        int _critmonth = 0; int _highmonth = 0; int _normalmonth = 0; int _lowmonth = 0;
        string headers = string.Empty;
        try
        {
            for (int a = 0; a <= dt.Rows.Count - 1; a++)
            {
                if (dt.Rows[a]["OrderId1"].ToString() == orderid1)
                {
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 12)).ToString())
                    {
                        _critmonth11 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth11 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth11 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth11 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 12)).ToString())
                    {
                        _critmonth11 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth11 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth11 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth11 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 11)).ToString())//(month - (nom - 2)).ToString())
                    {
                        _critmonth10 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth10 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth10 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth10 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 10)).ToString())
                    {
                        _critmonth9 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth9 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth9 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth9 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 9)).ToString())
                    {
                        _critmonth8 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth8 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth8 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth8 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 8)).ToString())
                    {
                        _critmonth7 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth7 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth7 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth7 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 7)).ToString())
                    {
                        _critmonth6 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth6 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth6 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth6 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 6)).ToString())
                    {
                        _critmonth5 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth5 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth5 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth5 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 5)).ToString())
                    {
                        _critmonth4 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth4 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth4 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth4 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 4)).ToString())
                    {
                        _critmonth3 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth3 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth3 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth3 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 3)).ToString())
                    {
                        _critmonth2 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth2 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth2 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth2 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 2)).ToString())
                    {
                        _critmonth1 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth1 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth1 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth1 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 1)).ToString())
                    {
                        _critmonth += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                }
            }

            if (nom == 12)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                     "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                     "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                     "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7 +
                                     "," + _critmonth8 + "," + _highmonth8 + "," + _normalmonth8 + "," + _lowmonth8 + "," + _critmonth9 + "," + _highmonth9 + "," + _normalmonth9 + "," + _lowmonth9 +
                                     "," + _critmonth10 + "," + _highmonth10 + "," + _normalmonth10 + "," + _lowmonth10 + "," + _critmonth11 + "," + _highmonth11 + "," + _normalmonth11 + "," + _lowmonth11;
            else if (nom == 11)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                    "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                    "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                    "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7 +
                                    "," + _critmonth8 + "," + _highmonth8 + "," + _normalmonth8 + "," + _lowmonth8 + "," + _critmonth9 + "," + _highmonth9 + "," + _normalmonth9 + "," + _lowmonth9 +
                                    "," + _critmonth10 + "," + _highmonth10 + "," + _normalmonth10 + "," + _lowmonth10;
            else if (nom == 10)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                    "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                    "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                    "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7 +
                                    "," + _critmonth8 + "," + _highmonth8 + "," + _normalmonth8 + "," + _lowmonth8 + "," + _critmonth9 + "," + _highmonth9 + "," + _normalmonth9 + "," + _lowmonth9;
            else if (nom == 9)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                    "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                    "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                    "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7 +
                                    "," + _critmonth8 + "," + _highmonth8 + "," + _normalmonth8 + "," + _lowmonth8;
            else if (nom == 8)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                    "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                    "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                    "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7;
            else if (nom == 7)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                    "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                    "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                    "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6;
            else if (nom == 6)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                    "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                    "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5;
            else if (nom == 5)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                    "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                    "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4;
            else if (nom == 4)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1
                                    + "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3;
            else if (nom == 3)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1
                                    + "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2;
            else if (nom == 2)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1;
            else if (nom == 1)
                headers += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth;

            return headers;
        }
        catch
        { return string.Empty; }
    }
    protected string AppendDetail(DataTable dt, int month, string orderid1, string detail, int nom)
    {
        int _critmonth11 = 0; int _highmonth11 = 0; int _normalmonth11 = 0; int _lowmonth11 = 0;
        int _critmonth10 = 0; int _highmonth10 = 0; int _normalmonth10 = 0; int _lowmonth10 = 0;
        int _critmonth9 = 0; int _highmonth9 = 0; int _normalmonth9 = 0; int _lowmonth9 = 0;
        int _critmonth8 = 0; int _highmonth8 = 0; int _normalmonth8 = 0; int _lowmonth8 = 0;
        int _critmonth7 = 0; int _highmonth7 = 0; int _normalmonth7 = 0; int _lowmonth7 = 0;
        int _critmonth6 = 0; int _highmonth6 = 0; int _normalmonth6 = 0; int _lowmonth6 = 0;
        int _critmonth5 = 0; int _highmonth5 = 0; int _normalmonth5 = 0; int _lowmonth5 = 0;
        int _critmonth4 = 0; int _highmonth4 = 0; int _normalmonth4 = 0; int _lowmonth4 = 0;
        int _critmonth3 = 0; int _highmonth3 = 0; int _normalmonth3 = 0; int _lowmonth3 = 0;
        int _critmonth2 = 0; int _critmonth1 = 0; int _critmonth = 0;
        int _highmonth2 = 0; int _highmonth1 = 0; int _highmonth = 0;
        int _normalmonth2 = 0; int _normalmonth1 = 0; int _normalmonth = 0;
        int _lowmonth2 = 0; int _lowmonth1 = 0; int _lowmonth = 0;
        string fieldetail = string.Empty;

        for (int a = 0; a <= dt.Rows.Count - 1; a++)
        {
            if (dt.Rows[a]["OrderId1"].ToString() == orderid1)
            {
                if (dt.Rows[a]["Detail"].ToString().ToLower() == detail)
                {
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 12)).ToString())//(month - (nom - 1)).ToString())
                    {
                        _critmonth11 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth11 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth11 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth11 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 11)).ToString())
                    {
                        _critmonth10 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth10 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth10 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth10 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 10)).ToString())
                    {
                        _critmonth9 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth9 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth9 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth9 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 9)).ToString())
                    {
                        _critmonth8 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth8 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth8 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth8 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 8)).ToString())
                    {
                        _critmonth7 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth7 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth7 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth7 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 7)).ToString())
                    {
                        _critmonth6 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth6 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth6 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth6 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 6)).ToString())
                    {
                        _critmonth5 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth5 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth5 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth5 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 5)).ToString())
                    {
                        _critmonth4 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth4 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth4 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth4 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 4)).ToString())
                    {
                        _critmonth3 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth3 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth3 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth3 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 3)).ToString())
                    {
                        _critmonth2 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth2 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth2 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth2 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 2)).ToString())
                    {
                        _critmonth1 += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth1 += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth1 += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth1 += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                    if (dt.Rows[a]["monthno"].ToString() == getMonth(month, -(nom - 1)).ToString())
                    {
                        _critmonth += Convert.ToInt32(dt.Rows[a]["Critical"]);
                        _highmonth += Convert.ToInt32(dt.Rows[a]["High"]);
                        _normalmonth += Convert.ToInt32(dt.Rows[a]["Normal"]);
                        _lowmonth += Convert.ToInt32(dt.Rows[a]["Low"]);
                    }
                }
            }
        }
        if (nom == 12)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                 "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                 "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                 "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7 +
                                 "," + _critmonth8 + "," + _highmonth8 + "," + _normalmonth8 + "," + _lowmonth8 + "," + _critmonth9 + "," + _highmonth9 + "," + _normalmonth9 + "," + _lowmonth9 +
                                 "," + _critmonth10 + "," + _highmonth10 + "," + _normalmonth10 + "," + _lowmonth10 + "," + _critmonth11 + "," + _highmonth11 + "," + _normalmonth11 + "," + _lowmonth11;
        else if (nom == 11)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7 +
                                "," + _critmonth8 + "," + _highmonth8 + "," + _normalmonth8 + "," + _lowmonth8 + "," + _critmonth9 + "," + _highmonth9 + "," + _normalmonth9 + "," + _lowmonth9 +
                                "," + _critmonth10 + "," + _highmonth10 + "," + _normalmonth10 + "," + _lowmonth10;
        else if (nom == 10)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7 +
                                "," + _critmonth8 + "," + _highmonth8 + "," + _normalmonth8 + "," + _lowmonth8 + "," + _critmonth9 + "," + _highmonth9 + "," + _normalmonth9 + "," + _lowmonth9;
        else if (nom == 9)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7 +
                                "," + _critmonth8 + "," + _highmonth8 + "," + _normalmonth8 + "," + _lowmonth8;
        else if (nom == 8)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6 + "," + _critmonth7 + "," + _highmonth7 + "," + _normalmonth7 + "," + _lowmonth7;
        else if (nom == 7)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5 +
                                "," + _critmonth6 + "," + _highmonth6 + "," + _normalmonth6 + "," + _lowmonth6;
        else if (nom == 6)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4 + "," + _critmonth5 + "," + _highmonth5 + "," + _normalmonth5 + "," + _lowmonth5;
        else if (nom == 5)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1 +
                                "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3 +
                                "," + _critmonth4 + "," + _highmonth4 + "," + _normalmonth4 + "," + _lowmonth4;
        else if (nom == 4)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1
                                + "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2 + "," + _critmonth3 + "," + _highmonth3 + "," + _normalmonth3 + "," + _lowmonth3;
        else if (nom == 3)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1
                                + "," + _critmonth2 + "," + _highmonth2 + "," + _normalmonth2 + "," + _lowmonth2;
        else if (nom == 2)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth + "," + _critmonth1 + "," + _highmonth1 + "," + _normalmonth1 + "," + _lowmonth1;
        else if (nom == 1)
            fieldetail += "," + _critmonth + "," + _highmonth + "," + _normalmonth + "," + _lowmonth;
        return fieldetail;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        phSearchResults.Visible = true;

        rpgExport.DataSource = GetAllData();

        Telerik.Web.UI.PivotGridSortExpression expressionmonth = new Telerik.Web.UI.PivotGridSortExpression();
        expressionmonth.FieldName = "monthno";
        expressionmonth.SortOrder = ddlSort.SelectedValue == "asc" ? Telerik.Web.UI.PivotGridSortOrder.Ascending : Telerik.Web.UI.PivotGridSortOrder.Descending;
        rpgExport.Sort(expressionmonth);

        Telerik.Web.UI.PivotGridSortExpression expressionyears = new Telerik.Web.UI.PivotGridSortExpression();
        expressionyears.FieldName = "years";
        expressionyears.SortOrder = ddlSort.SelectedValue == "asc" ? Telerik.Web.UI.PivotGridSortOrder.Ascending : Telerik.Web.UI.PivotGridSortOrder.Descending;
        rpgExport.Sort(expressionyears);

        rpgExport.Rebind();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("QuarterlyClientTicketReport.aspx");
    }

    #region Binddata
    protected void bindGrid()
    {
        rpgExport.DataSource = GetAllData();
    }
    protected void binddl()
    {
        DateTime currentDate = DateTime.Now;
        List<ListItem> items = new List<ListItem>();
        DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
        items.Add(new ListItem
        {
            Text = currentDate.AddMonths(-6).ToString("MMMM"),
            Value = currentDate.AddMonths(-6).Date.ToString("MM/dd/yyyy")
        });
        items.Add(new ListItem
        {
            Text = currentDate.AddMonths(-5).ToString("MMMM"),
            Value = currentDate.AddMonths(-5).Date.ToString("MM/dd/yyyy")
        });
        items.Add(new ListItem
        {
            Text = currentDate.AddMonths(-4).ToString("MMMM"),
            Value = currentDate.AddMonths(-4).Date.ToString("MM/dd/yyyy")
        });
        items.Add(new ListItem
        {
            Text = currentDate.AddMonths(-3).ToString("MMMM"),
            Value = currentDate.AddMonths(-3).Date.ToString("MM/dd/yyyy")
        });
        items.Add(new ListItem
        {
            Text = currentDate.AddMonths(-2).ToString("MMMM"),
            Value = currentDate.AddMonths(-2).Date.ToString("MM/dd/yyyy")
        });
        items.Add(new ListItem
        {
            Text = currentDate.AddMonths(-1).ToString("MMMM"),
            Value = currentDate.AddMonths(-1).Date.ToString("MM/dd/yyyy")
        });
        items.Add(new ListItem
        {
            Text = currentDate.AddMonths(0).ToString("MMMM"),
            Value = currentDate.Date.ToString("MM/dd/yyyy")
        });
        ddlMonths.DataSource = items;
        ddlMonths.DataTextField = "Text";
        ddlMonths.DataValueField = "Value";
        ddlMonths.DataBind();
    }
    private DataTable GetAllData()
    {
        string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString.SQL Server (SqlClient)"];
        System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
        DataTable dt = new DataTable();
        string _tickettype = string.Empty;

        for (int i = 0; i <= ddlTicketType.SelectedValues.Count - 1; i++)
        {
            if (_tickettype == string.Empty)
                _tickettype = ddlTicketType.SelectedValues[i];
            else
                _tickettype += "," + ddlTicketType.SelectedValues[i];
        }
        try
        {
            using (connection)
            {
                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adp = new System.Data.SqlClient.SqlDataAdapter("proc_getAllDataQuarterly_Desktop", connection);

                adp.SelectCommand.Parameters.AddWithValue("@client", ddlClient.ClientId == null ? -1 : ddlClient.ClientId);
                adp.SelectCommand.Parameters.AddWithValue("@dateofmonth", ddlMonths.SelectedValue);
                adp.SelectCommand.Parameters.AddWithValue("@nom", ddlNoM.SelectedValue);
                adp.SelectCommand.Parameters.AddWithValue("@tickettype", _tickettype);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;

                dt.Reset();
                adp.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally { connection.Close(); }

        return dt;
    }
    #endregion

    public class UserCategory
    {
        public string Detail { get; set; }
    }

    protected void rpgExport_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
    {
        bindGrid();
    }

    protected void rpgExport_Sorting(object sender, Telerik.Web.UI.PivotGridSortEventArgs e)
    {

    }
}