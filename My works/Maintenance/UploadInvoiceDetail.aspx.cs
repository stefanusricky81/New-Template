using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Text;

public partial class Maintenance_UploadInvoiceDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int _id = -1;
            if (int.TryParse(BitByBit.Web.Request.GetString("UploadId").Trim(), out _id))
            {
                UploadId = _id;
                BindGrid();
                //rgUploadInvoice.Rebind();
            }
        }
    }
    protected void BindGrid()
    {
        DataTable dt = null;
        //rgUploadInvoice.DataSource = DesktopShared.UploadInvoice.Get(ref dt, _lastimport);
        rgUploadInvoice.DataSource = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetLastImportUploadInvoice(UploadId);
    }
    

    protected void rgUploadInvoice_GridExporting(object sender, Telerik.Web.UI.GridExportingArgs e)
    {
        if (e.ExportType == ExportType.Excel)
        {
            string css = "<style> body { border:solid 0.1pt #CCCCCC; }</style>";
            e.ExportOutput = e.ExportOutput.Replace("</head>", css + "</head>");
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = null;
        dt = DesktopShared.StoredProcedureCallerClasses.RetrievalProcedures.ProcGetLastImportUploadInvoice(UploadId);
        if (dt != null)
        {
            dt.Columns.Remove("Id");
            dt.Columns.Remove("LastUpdateDate");
            dt.Columns.Remove("CreatedDate");
            dt.Columns.Remove("CreatedBy");
            dt.Columns.Remove("LastUpdateBy");
            dt.Columns.Remove("fkUploadInvoiceId");
            dt.Columns.Remove("flagOldNew");
            dt.Columns.Remove("MSRP");
            Exporttocsv(dt);
        }
    }

    #region public
    public void Exporttocsv(DataTable dt)
    {
        string headers = string.Empty;
        string err = string.Empty;
        string billrollup = string.Empty;
        string categoryname = string.Empty;
        string summessage = string.Empty;
        string errmessage = string.Empty;
        bool categoryexist = false;
        List<SumDetail> sumdetailList = new List<SumDetail>();
        SumDetail _sumdetail = new SumDetail();
        decimal _extendedmsrp;

        try
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder str = new StringBuilder();
            str.Append("<table  align='center' border='1' bordercolor='#00aeef' width='99%' class='reporttable1' cellspacing='0' cellpadding='0' style='font-size:15;'>");
            str.Append("<tr>");
            #region for header
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                if (dt.Columns[i].ColumnName != "EndUserName")
                {
                    str.Append("<td>");
                    str.Append("<b>" + title(dt.Columns[i].ColumnName) + "</b>");
                    str.Append("</td>");
                }
            }
            #endregion
            str.Append("</tr>");

            #region for Detail
            for (int j = 0; j <= dt.Rows.Count - 1; j++)
            {
                if (String.IsNullOrEmpty(dt.Rows[j]["ExtendedMSRP"].ToString().Trim()))
                    _extendedmsrp = 0;
                else
                    _extendedmsrp = Convert.ToDecimal(dt.Rows[j]["ExtendedMSRP"].ToString().Trim());

                categoryname = string.Empty;
                DesktopShared.UploadInvoice.checkRollup(Convert.ToInt32(dt.Rows[j]["Fk_Sku"]), ref billrollup, ref categoryname, ref errmessage);

                if (sumdetailList.Count > 0)
                {
                    if (dt.Rows[j]["ItemCustomerFullName"].ToString().Trim() == sumdetailList[0].client.ToString().Trim())
                    {
                        if (billrollup == "N" || billrollup == "")
                        {
                            nonrolluprow(dt, j, ref str);
                        }
                        else if (billrollup == "Y")
                        {
                            for (int _list = 0; _list <= sumdetailList.Count - 1; _list++)
                            {
                                if (categoryname == sumdetailList[_list].skucategoryname)
                                {
                                    sumdetail(sumdetailList, _list, Convert.ToDecimal(dt.Rows[j]["Cost"]), Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), _extendedmsrp);

                                    categoryexist = true;
                                    break;
                                }
                                else
                                    categoryexist = false;
                            }
                            if (!categoryexist)
                            {
                                addList(sumdetailList, dt.Rows[j]["VendorFullName"].ToString().Trim(), dt.Rows[j]["APAccountFullName"].ToString().Trim(), dt.Rows[j]["txnDate"].ToString().Trim(),
                                    dt.Rows[j]["DueDate"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["AmountDue"].ToString().Trim()), dt.Rows[j]["CIN"].ToString().Trim(), dt.Rows[j]["Terms"].ToString().Trim(),
                                    dt.Rows[j]["Memo"].ToString().Trim(), dt.Rows[j]["IsPaid"].ToString().Trim(), dt.Rows[j]["OpenAccount"].ToString().Trim(), dt.Rows[j]["ExpAccountFullName"].ToString().Trim(),
                                    categoryname.Trim(), categoryname.Trim(), Convert.ToDecimal(dt.Rows[j]["Cost"]),
                                    Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), dt.Rows[j]["ItemCustomerFullName"].ToString().Trim(), dt.Rows[j]["ItemBillableStatus"].ToString().Trim(),
                                    dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname, dt.Rows[j]["EndUserName"].ToString().Trim(),
                                    _extendedmsrp);
                            }
                        }
                    }
                    else
                    {
                        for (int a = 0; a <= sumdetailList.Count - 1; a++)
                        {
                            summary(sumdetailList[a].vendorname, sumdetailList[a].apaccfullname, sumdetailList[a].txndate, sumdetailList[a].duedate, sumdetailList[a].amountdue, sumdetailList[a].refnum,
                                sumdetailList[a].terms, sumdetailList[a].memo, sumdetailList[a].ispaid, sumdetailList[a].openaccount, sumdetailList[a].expaccfullname, sumdetailList[a].sku, sumdetailList[a].desc,
                                sumdetailList[a].cost, sumdetailList[a].itemamount, sumdetailList[a].client, sumdetailList[a].itembillablestatus, sumdetailList[a].ItemSalesRepRefFullName,
                                sumdetailList[a].extmsrp, ref summessage, ref errmessage);
                            str.Append(summessage);
                        }
                        sumdetailList.Clear();

                        if (billrollup == "N" || billrollup == "")
                        {
                            nonrolluprow(dt, j, ref str);
                        }
                        else if (billrollup == "Y")
                        {
                            addList(sumdetailList, dt.Rows[j]["VendorFullName"].ToString().Trim(), dt.Rows[j]["APAccountFullName"].ToString().Trim(), dt.Rows[j]["txnDate"].ToString().Trim(),
                                    dt.Rows[j]["DueDate"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["AmountDue"].ToString().Trim()), dt.Rows[j]["CIN"].ToString().Trim(), dt.Rows[j]["Terms"].ToString().Trim(),
                                    dt.Rows[j]["Memo"].ToString().Trim(), dt.Rows[j]["IsPaid"].ToString().Trim(), dt.Rows[j]["OpenAccount"].ToString().Trim(), dt.Rows[j]["ExpAccountFullName"].ToString().Trim(),
                                    categoryname.Trim(), categoryname.Trim(), Convert.ToDecimal(dt.Rows[j]["Cost"]),
                                    Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), dt.Rows[j]["ItemCustomerFullName"].ToString().Trim(), dt.Rows[j]["ItemBillableStatus"].ToString().Trim(),
                                    dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname, dt.Rows[j]["EndUserName"].ToString().Trim(),
                                    _extendedmsrp);
                        }
                    }
                }
                else
                {
                    if (billrollup == "N" || billrollup == "")
                    {
                        nonrolluprow(dt, j, ref str);
                    }
                    else if (billrollup == "Y")
                    {
                        //addList(sumdetailList, dt.Rows[j]["VendorFullName"].ToString().Trim(), dt.Rows[j]["APAccountFullName"].ToString().Trim(), dt.Rows[j]["txnDate"].ToString().Trim(),
                        //            dt.Rows[j]["DueDate"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["AmountDue"].ToString().Trim()), dt.Rows[j]["CIN"].ToString().Trim(), dt.Rows[j]["Terms"].ToString().Trim(),
                        //            dt.Rows[j]["Memo"].ToString().Trim(), dt.Rows[j]["IsPaid"].ToString().Trim(), dt.Rows[j]["OpenAccount"].ToString().Trim(), dt.Rows[j]["ExpAccountFullName"].ToString().Trim(),
                        //            dt.Rows[j]["FkSku"].ToString().Trim(), dt.Rows[j]["Subscription"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["Cost"]),
                        //            Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), dt.Rows[j]["ItemCustomerFullName"].ToString().Trim(), dt.Rows[j]["ItemBillableStatus"].ToString().Trim(),
                        //            dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname);
                        addList(sumdetailList, dt.Rows[j]["VendorFullName"].ToString().Trim(), dt.Rows[j]["APAccountFullName"].ToString().Trim(), dt.Rows[j]["txnDate"].ToString().Trim(),
                                    dt.Rows[j]["DueDate"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["AmountDue"].ToString().Trim()), dt.Rows[j]["CIN"].ToString().Trim(), dt.Rows[j]["Terms"].ToString().Trim(),
                                    dt.Rows[j]["Memo"].ToString().Trim(), dt.Rows[j]["IsPaid"].ToString().Trim(), dt.Rows[j]["OpenAccount"].ToString().Trim(), dt.Rows[j]["ExpAccountFullName"].ToString().Trim(),
                                    categoryname.Trim(), categoryname.Trim(), Convert.ToDecimal(dt.Rows[j]["Cost"]),
                                    Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), dt.Rows[j]["ItemCustomerFullName"].ToString().Trim(), dt.Rows[j]["ItemBillableStatus"].ToString().Trim(),
                                    dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname, dt.Rows[j]["EndUserName"].ToString().Trim(),
                                    _extendedmsrp);
                    }
                }
            }
            for (int a = 0; a <= sumdetailList.Count - 1; a++)
            {
                //summary(sumdetailList[a].client, sumdetailList[a].skucategoryname, sumdetailList[a].skuid, sumdetailList[a].cost, sumdetailList[a].itemamount, ref summessage, ref errmessage);
                summary(sumdetailList[a].vendorname, sumdetailList[a].apaccfullname, sumdetailList[a].txndate, sumdetailList[a].duedate, sumdetailList[a].amountdue, sumdetailList[a].refnum,
                                sumdetailList[a].terms, sumdetailList[a].memo, sumdetailList[a].ispaid, sumdetailList[a].openaccount, sumdetailList[a].expaccfullname, sumdetailList[a].sku, sumdetailList[a].desc,
                                sumdetailList[a].cost, sumdetailList[a].itemamount, sumdetailList[a].client, sumdetailList[a].itembillablestatus, sumdetailList[a].ItemSalesRepRefFullName,
                                sumdetailList[a].extmsrp, ref summessage, ref errmessage);
                str.Append(summessage);
            }
            sumdetailList.Clear();

            #endregion
            str.Append("</table>".ToString());

            sb.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            sb.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            sb.Append("<DIV style='font-size:12px;'>");
            sb.Append(str.ToString());
            sb.Append("</div></body></html>");
            string strFile = "Billing -" + DateTime.Now.ToString() + ".xls";
            string strcontentType = "application/excel";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.BufferOutput = true;
            Response.ContentType = strcontentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
            Response.Write(sb.ToString());
            Response.Flush();
            Response.Close();
            Response.End();

            litMessage.Text = "";
        }
        catch (Exception ex)
        {
            litMessage.Text = ex.Message;
            return;
        }
    }

    protected string title(string columnname)
    {
        if (columnname.ToLower() == "fk_sku")
            return "Item Full Name";
        else
            return columnname;
    }

    protected void addList(List<SumDetail> detail, string vendor, string apaccfullname, string txndate, string duedate, decimal amountdue, string cin,
        string terms, string memo, string ispaid, string openaccount, string expaccfullname, string skuid, string desc, decimal cost, decimal itemamount,
        string itemclienfullname, string ItemBillableStatus, string ItemSalesRepRefFullName, string categoryname, string endusername, decimal extendedmsrp)
    {
        detail.Add(new SumDetail
        {
            vendorname = vendor,
            apaccfullname = apaccfullname,
            txndate = txndate,
            duedate = duedate,
            amountdue = amountdue,
            refnum = cin,
            terms = terms,
            memo = memo,
            ispaid = ispaid,
            openaccount = openaccount,
            expaccfullname = expaccfullname,
            sku = skuid,
            desc = desc,
            cost = cost,
            itemamount = itemamount,
            client = itemclienfullname,
            itembillablestatus = ItemBillableStatus,
            ItemSalesRepRefFullName = ItemSalesRepRefFullName,
            skucategoryname = categoryname,
            EndUsername = endusername,
            extmsrp=extendedmsrp
        });
    }

    protected bool summary(string vendor, string apaccfullname, string txndate, string duedate, decimal amountdue, string cin,
        string terms, string memo, string ispaid, string openaccount, string expaccfullname, string skuid, string desc, decimal cost, decimal itemamount,
        string endusername, string ItemBillableStatus, string ItemSalesRepRefFullName, decimal extendedmsrp, ref string message, ref string errmessage)
    {
        try
        {
            StringBuilder strsummary = new StringBuilder();

            strsummary.Append("<tr bgcolor='Yellow'>");

            strsummary.Append("<td>");
            strsummary.Append(vendor);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(apaccfullname);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(Convert.ToDateTime(txndate).ToString("MM/dd/yyyy"));
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(Convert.ToDateTime(duedate).ToString("MM/dd/yyyy"));
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(amountdue);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(cin);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(terms);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(memo);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(ispaid);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(openaccount);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(expaccfullname);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(skuid);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(desc);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(cost);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(itemamount);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            string _client = endusername;
            var parts = _client.Split(':');

            if (parts[0].ToString().Trim() == string.Empty)
                endusername = parts[1].ToString().Trim();
            else if (parts[1].ToString().Trim() == string.Empty)
                endusername = parts[0].ToString().Trim();
            else
                endusername = _client;
            strsummary.Append(endusername);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(ItemBillableStatus);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(ItemSalesRepRefFullName);
            strsummary.Append("</td>");

            strsummary.Append("<td>");
            strsummary.Append(extendedmsrp);
            strsummary.Append("</td>");

            strsummary.Append("</tr>");

            message = strsummary.ToString();
            errmessage = string.Empty;

            return true;
        }
        catch (Exception ex)
        {
            message = string.Empty;
            errmessage = ex.Message.ToString().Trim();
            return false;
        }
    }

    protected void nonrolluprow(DataTable dt, int counter, ref StringBuilder str)
    {
        str.Append("<tr>");
        for (int col = 0; col <= dt.Columns.Count - 1; col++)
        {
            if (dt.Columns[col].ColumnName.ToString().Trim() != "EndUserName")
            {
                str.Append("<td>");
                if (dt.Columns[col].ColumnName.ToString().Trim() == "txnDate" || dt.Columns[col].ColumnName.ToString().Trim() == "DueDate")
                    str.Append(Convert.ToDateTime(dt.Rows[counter][dt.Columns[col].ColumnName].ToString().Trim()).ToString("MM/dd/yyyy"));
                else
                {
                    if (dt.Columns[col].ColumnName.ToString().Trim() == "ItemCustomerFullName")
                    {
                        string _client = dt.Rows[counter][dt.Columns[col].ColumnName].ToString().Trim();
                        string newcustomerfullname = string.Empty;
                        var parts = _client.Split(':');

                        if (parts[0].ToString().Trim() == string.Empty)
                            newcustomerfullname = parts[1].ToString().Trim();
                        else if (parts[1].ToString().Trim() == string.Empty)
                            newcustomerfullname = parts[0].ToString().Trim();
                        else
                            newcustomerfullname = _client;

                        str.Append(newcustomerfullname);
                    }
                    else
                        str.Append(dt.Rows[counter][dt.Columns[col].ColumnName].ToString().Trim());
                }
                
                str.Append("</td>");
            }
        }
        str.Append("</tr>");
    }

    protected void sumdetail(List<SumDetail> detailList, int counter, decimal cost, decimal itemamount, decimal extendedmsrp)
    {
        detailList[counter].cost += cost;
        detailList[counter].itemamount += itemamount;
        detailList[counter].extmsrp += extendedmsrp;
    }

    protected string caption(string dataneedtochanges, string originalcaption, string newcaption)
    {
        if (dataneedtochanges == originalcaption)
            return newcaption;
        else
            return originalcaption;
    }


    public class SumDetail
    {
        public string vendorname { get; set; }
        public string apaccfullname { get; set; }
        public string txndate { get; set; }
        public string duedate { get; set; }
        public decimal amountdue { get; set; }
        public string refnum { get; set; }
        public string terms { get; set; }
        public string memo { get; set; }
        public string ispaid { get; set; }
        public string openaccount { get; set; }
        public string expaccfullname { get; set; }
        public string sku { get; set; }
        public string desc { get; set; }
        public decimal cost { get; set; }
        public decimal itemamount { get; set; }
        public string client { get; set; }
        public string itembillablestatus { get; set; }
        public string ItemSalesRepRefFullName { get; set; }
        public string skucategoryname { get; set; }
        public string EndUsername { get; set; }
        public decimal extmsrp { get; set; }
        //public decimal tax { get; set; }
    }
    #endregion

    #region private
    private int? UploadId
    {
        get
        {
            object obj = this.ViewState["scid"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["scid"] = value; }
    }
    #endregion

    protected void rgUploadInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void rgUploadInvoice_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            DataRowView _row = (DataRowView)e.Item.DataItem;
            GridDataItem _gdi = e.Item as GridDataItem;

            string _client = _row["ItemCustomerFullName"].ToString().Trim();
            var parts = _client.Split(':');

            if (parts[0].ToString().Trim() == string.Empty)
                _gdi["ItemCustomerFullName"].Text = parts[1].ToString().Trim();
            else if (parts[1].ToString().Trim() == string.Empty)
                _gdi["ItemCustomerFullName"].Text = parts[0].ToString().Trim();
            else
                _gdi["ItemCustomerFullName"].Text = _client;
        }
    }

    protected void btnBackUploadInvoice_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Maintenance/UploadInvoice.aspx");
    }
}