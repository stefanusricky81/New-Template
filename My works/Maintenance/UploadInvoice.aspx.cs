using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using Telerik.Web.UI;

public partial class Maintenance_UploadInvoice : System.Web.UI.Page
{
    bool lastimport = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindGrid();
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        string errmessage = string.Empty;
        string Successmessage = string.Empty;
        string allerrmessage = string.Empty;
        string fqdn = BitByBit.Configuration.GetConfigString("DesktopFqdn");
        try
        {
            string excelpath = string.Empty;
            string sheetname = string.Empty;
            int? clientid = null;
            string synexid = "";
            int checksynnex = 0, countsku = 0, synnexerrcount = 0;
            bool uploadvalid = true;
            string listsku = string.Empty;
            List<string> listclient = new List<string>();
            string urleditsku = "https://" + fqdn + "/Maintenance/BillingSKU.aspx?skuId=";
            string txndate = string.Empty;
            DateTime? dueDate = null;
            decimal? amount = null;
            string memo = "";
            string jobnumber = "";
            int? pclient = null;
            int? uploadinvoiceid = null;
            string clientcode = "";
            string salescode = "";

            if (DesktopShared.UploadInvoice.checkFilename(txtFileName.Text.Trim(), ref errmessage) > 0)
            {
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, "Filename already existed", DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                return;
            }
            else if (DesktopShared.UploadInvoice.checkFilename(txtFileName.Text.Trim(), ref errmessage) < 0)
            {
                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, errmessage.Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                return;
            }
            else
            {
                
                #region Datatable for upload
                DataTable dtUpload = new DataTable();
                dtUpload.Columns.Add("VendorName", System.Type.GetType("System.String"));//0
                dtUpload.Columns.Add("APAccountFullName", System.Type.GetType("System.String"));//1
                dtUpload.Columns.Add("TxnDate", System.Type.GetType("System.String"));//2
                dtUpload.Columns.Add("DueDate", System.Type.GetType("System.String"));//3
                dtUpload.Columns.Add("AmountDue", System.Type.GetType("System.String"));//4
                dtUpload.Columns.Add("CIN", System.Type.GetType("System.String"));//5
                dtUpload.Columns.Add("Terms", System.Type.GetType("System.String"));//6
                dtUpload.Columns.Add("Memo", System.Type.GetType("System.String"));//7
                dtUpload.Columns.Add("IsPaid", System.Type.GetType("System.String"));//8
                dtUpload.Columns.Add("OpenAmount", System.Type.GetType("System.String"));//9
                dtUpload.Columns.Add("ExpAccountFullName", System.Type.GetType("System.String"));//10
                dtUpload.Columns.Add("SKUNo", System.Type.GetType("System.String"));//11
                dtUpload.Columns.Add("Subscription", System.Type.GetType("System.String"));//12
                dtUpload.Columns.Add("Cost", System.Type.GetType("System.String"));//13
                dtUpload.Columns.Add("ItemAmount", System.Type.GetType("System.String"));//14
                dtUpload.Columns.Add("ItemCustomerFullName", System.Type.GetType("System.String"));//15
                dtUpload.Columns.Add("ItemBillableStatus", System.Type.GetType("System.String"));//16
                dtUpload.Columns.Add("ItemSalesRepRefFullName", System.Type.GetType("System.String"));//17
                dtUpload.Columns.Add("EndUserName", System.Type.GetType("System.String"));//18
                dtUpload.Columns.Add("MSRP", System.Type.GetType("System.String"));//19
                dtUpload.Columns.Add("ExtendedMSRP", System.Type.GetType("System.String"));//20

                DataRow drUpload;
                #endregion

                foreach (UploadedFile file in rauFileUpload.UploadedFiles)
                {
                    UploadedFileInfo uploadedFileInfo = new UploadedFileInfo(file);
                    excelpath = Server.MapPath("~/Archieves/Imports/") + file.GetNameWithoutExtension() + "-" + DesktopShared.User.UserName + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + file.GetExtension();
                    file.SaveAs(excelpath);

                    string connectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + excelpath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
                    OleDbConnection ad = new OleDbConnection(connectionstring);
                    ad.Open();
                    //DataTable dtexcel = ad.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    sheetname = "Consolidated Invoicing Report$";// dtexcel.Rows[1]["TABLE_NAME"].ToString();
                    DataSet dsExcel = new DataSet();
                    OleDbCommand command = new OleDbCommand("select * from [" + sheetname + "]", ad);
                    string q = "select * from [" + sheetname + "]";

                    OleDbDataAdapter oda = new OleDbDataAdapter(q, ad);
                    oda.Fill(dsExcel);
                    ad.Close();

                    memo = dsExcel.Tables[0].Columns[1].ColumnName;
                    txndate = memo.Substring(memo.IndexOf("-") + 1, memo.IndexOf("-"));
                    dueDate = Convert.ToDateTime(txndate).AddDays(30);
                    amount = Convert.ToDecimal(dsExcel.Tables[0].Rows[3][1].ToString().Replace("$", ""));

                    for (int i = 0; i <= 7; i++)
                    {
                        dsExcel.Tables[0].Rows[i].Delete();
                    }
                    dsExcel.AcceptChanges();

                    #region for checking data
                    foreach (DataRow row in dsExcel.Tables[0].Rows)
                    {
                        if (row[11].ToString() != string.Empty)
                        {
                            DesktopShared.UploadInvoice.getJobNumber(row[3].ToString(), ref errmessage, ref jobnumber, ref pclient, ref clientcode);
                            //getJobNumber(row[3].ToString(), ref errmessage, ref jobnumber, ref pclient, ref clientcode);
                            if (pclient != null)
                                salescode = DesktopShared.UploadInvoice.getSalesPesonCode(pclient, ref errmessage);
                            drUpload = dtUpload.NewRow();
                            drUpload["VendorName"] = "SYNN";
                            drUpload["APAccountFullName"] = "Accounts Payable";
                            drUpload["TxnDate"] = txndate;
                            drUpload["DueDate"] = dueDate;
                            drUpload["AmountDue"] = amount;
                            drUpload["CIN"] = row[0].ToString();
                            drUpload["Terms"] = row[9].ToString();
                            drUpload["Memo"] = memo;
                            drUpload["IsPaid"] = false;
                            drUpload["OpenAmount"] = string.Empty;
                            drUpload["ExpAccountFullName"] = string.Empty;
                            drUpload["SKUNo"] = row[11].ToString();
                            drUpload["Subscription"] = row[10].ToString();
                            drUpload["Cost"] = row[16].ToString().Trim() == string.Empty ? "$0.00" : row[16].ToString();
                            drUpload["ItemAmount"] = row[16].ToString().Trim() == string.Empty ? "$0.00" : row[16].ToString();
                            drUpload["ItemCustomerFullName"] = clientcode + ":" + jobnumber;
                            drUpload["ItemBillableStatus"] = "Billable";
                            drUpload["ItemSalesRepRefFullName"] = salescode;
                            drUpload["EndUserName"] = row[3].ToString();
                            drUpload["MSRP"] = row[13].ToString();
                            drUpload["ExtendedMSRP"] = row[14].ToString();
                            dtUpload.Rows.Add(drUpload);
                        }
                    }
                    if (!DesktopShared.UploadInvoice.AddUploadHist(txtFileName.Text.Trim(), dtUpload.Rows.Count, DesktopShared.User.UserID, file.FileName.ToString().Trim(), ref uploadinvoiceid, ref errmessage))
                    {
                        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, errmessage.Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                        return;
                    }

                    for (int i = 0; i <= dtUpload.Rows.Count - 1; i++)
                    {
                        if (checkClient(listclient, dtUpload.Rows[i]["EndUserName"].ToString(), ref errmessage) == 0)
                        {
                            checksynnex = DesktopShared.UploadInvoice.checkSynexId(dtUpload.Rows[i]["EndUserName"].ToString(), ref errmessage, ref clientid, ref synexid);

                            if (errmessage != string.Empty)
                                allerrmessage += "<br/>" + errmessage;
                            if (checksynnex == 0)
                            {
                                listclient.Add(dtUpload.Rows[i]["EndUserName"].ToString());
                                uploadvalid = false;
                                synnexerrcount++;
                                if (allerrmessage == string.Empty)
                                    allerrmessage += dtUpload.Rows[i]["EndUserName"].ToString();
                                else
                                    allerrmessage += "<br/>" + dtUpload.Rows[i]["EndUserName"].ToString();
                            }
                        }

                        if (errmessage != string.Empty)
                            allerrmessage += "<br/>" + errmessage;
                    }
                    #endregion

                    #region for upload
                    if (uploadvalid)
                    {
                        for (int i = 0; i <= dtUpload.Rows.Count - 1; i++)
                        {
                            if (!DesktopShared.UploadInvoice.AddBillingInvoice(dtUpload.Rows[i]["VendorName"].ToString().Trim(), dtUpload.Rows[i]["APAccountFullName"].ToString().Trim(),
                                Convert.ToDateTime(dtUpload.Rows[i]["TxnDate"].ToString().Trim()), Convert.ToDateTime(dtUpload.Rows[i]["DueDate"].ToString().Trim()),
                                dtUpload.Rows[i]["AmountDue"].ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(dtUpload.Rows[i]["AmountDue"].ToString().Trim().Replace("$", "")),
                                dtUpload.Rows[i]["CIN"].ToString().Trim() == string.Empty ? 0 : Convert.ToInt32(dtUpload.Rows[i]["CIN"].ToString().Trim()), dtUpload.Rows[i]["Terms"].ToString().Trim(),
                                dtUpload.Rows[i]["Memo"].ToString().Trim(), Convert.ToInt32(dtUpload.Rows[i]["SKUNo"].ToString().Trim()),
                                dtUpload.Rows[i]["Subscription"].ToString().Trim(),
                                dtUpload.Rows[i]["Cost"].ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(dtUpload.Rows[i]["Cost"].ToString().Trim().Replace("$", "")),
                                dtUpload.Rows[i]["ItemAmount"].ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(dtUpload.Rows[i]["ItemAmount"].ToString().Trim().Replace("$", "")),
                                 dtUpload.Rows[i]["ItemCustomerFullName"].ToString().Trim(), dtUpload.Rows[i]["ItemBillableStatus"].ToString().Trim(), dtUpload.Rows[i]["ItemSalesRepRefFullName"].ToString().Trim(),
                                 dtUpload.Rows[i]["EndUserName"].ToString().Trim(), DesktopShared.User.UserID,uploadinvoiceid,
                                 dtUpload.Rows[i]["MSRP"].ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(dtUpload.Rows[i]["MSRP"].ToString().Trim().Replace("$", "")),
                                 dtUpload.Rows[i]["ExtendedMSRP"].ToString().Trim() == string.Empty ? 0 : Convert.ToDecimal(dtUpload.Rows[i]["MSRP"].ToString().Trim().Replace("$", "")),
                                 ref errmessage)
                                )
                            {
                                DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Error Add billing : {0}; SKU#:{1}", errmessage, dtUpload.Rows[i]["SKUNo"].ToString()), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                                return;
                            }
                            int? dbskuid = null;
                            int? id = null;

                            if (DesktopShared.UploadInvoice.checkSKU(Convert.ToInt32(dtUpload.Rows[i]["SKUNo"].ToString()), ref errmessage, ref dbskuid, ref id) == 0)
                            {
                                if (!DesktopShared.UploadInvoice.AddSKU(Convert.ToInt32(dtUpload.Rows[i]["SKUNo"].ToString()), DesktopShared.User.UserID, dtUpload.Rows[i]["Subscription"].ToString(), ref errmessage))
                                {
                                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Error Add SKU : {0}; SKU#:{1}", errmessage, dtUpload.Rows[i]["SKUNo"].ToString()), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                                    return;
                                }
                                else
                                {
                                    countsku++;
                                    listsku += String.Format("<a href=\"BillingSKU.aspx?skuId={0}\" target=\"_blank\" class=\"alert-link\">{0}</a>.<br/> ", dtUpload.Rows[i]["SKUNo"].ToString());
                                }
                            }
                            else
                            {
                                if (!DesktopShared.UploadInvoice.UpdateSKU((int)id, DesktopShared.User.UserID, dtUpload.Rows[i]["Subscription"].ToString(), ref errmessage))
                                {
                                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, String.Format("Error Add SKU : {0}; SKU#:{1}", errmessage, dtUpload.Rows[i]["SKUNo"].ToString()), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        string alerterrclientorsynnex = "";
                        alerterrclientorsynnex = "The Synnex file did not import. Please fix the errors below and reupload";

                        if (synnexerrcount > 0)
                        {
                            alerterrclientorsynnex += "<br/> Client Synnex ID Validation: " + synnexerrcount + " Errors ";
                            alerterrclientorsynnex += "<br/> The following Desktop Client -> Billing: Synnex ID field should match below";
                            allerrmessage = alerterrclientorsynnex + "<br/>" + allerrmessage;
                        }
                        else
                        {
                            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, allerrmessage, DesktopShared.Bootstrap.Alert.AlertType.Danger);
                            return;
                        }
                        DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, allerrmessage, DesktopShared.Bootstrap.Alert.AlertType.Danger);
                        return;
                    }
                    #endregion
                }

                if (listsku == string.Empty)
                {
                    Successmessage = "Upload Successful <br/>Client Account Synnex ID Validation: 0 Errors <br/>SKU Validation: 0 Errors";
                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, Successmessage, DesktopShared.Bootstrap.Alert.AlertType.Success);
                }
                else
                {
                    Successmessage = "SKU Validation: " + countsku + " Errors<br/> SKU # needs to be assigned a category. Click on the SKU to update.<br/>";
                    Successmessage += listsku;
                    DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, Successmessage, DesktopShared.Bootstrap.Alert.AlertType.Danger);
                }

                if (File.Exists(excelpath))
                    File.Delete(excelpath);

                pnlResults.Visible = true;
                lastimport = true;
                BindGrid();
                rgUploadInvoice.Rebind();
                ViewState["dtupload"] = dtUpload;
            }
        }
        catch (Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.Message.Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    protected void btnLastImport_Click(object sender, EventArgs e)
    {
        try 
        {
            litMessage.Text = "";
            pnlResults.Visible = true;
            lastimport = false;
            BindGrid();
            rgUploadInvoice.Rebind();
        }
        catch (Exception ex)
        {
            DesktopShared.Bootstrap.Alert.DisplayMessage(litMessage, ex.Message.Trim(), DesktopShared.Bootstrap.Alert.AlertType.Danger, false);
        }
    }

    protected int checkClient(List<string> _listclient, string clientname, ref string err)
    {
        try
        {
            if (_listclient.Count != 0)
            {
                for (int i = 0; i <= _listclient.Count - 1; i++)
                {
                    if (clientname == _listclient[i].ToString().Trim())
                        return 1;
                }
                err = string.Empty;
                return 0;
            }
            else
            {
                err = string.Empty;
                return 0;
            }
            
        }
        catch (Exception ex)
        {
            err = ex.Message.ToString().Trim();
            return -1;
        }
    }

    protected void rgUploadInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void BindGrid()
    {
        rgUploadInvoice.DataSource = DesktopShared.UploadInvoice.Get();
        //rgUploadInvoice.Rebind();
    }

    protected void rgUploadInvoice_GridExporting(object sender, GridExportingArgs e)
    {
        if (e.ExportType == ExportType.Excel)
        {
            string css = "<style> body { border:solid 0.1pt #CCCCCC; }</style>";
            e.ExportOutput = e.ExportOutput.Replace("</head>", css + "</head>");
        }
    }

    #region Export

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
                DesktopShared.UploadInvoice.checkRollup(Convert.ToInt32(dt.Rows[j]["Fk_Sku"]), ref billrollup, ref categoryname, ref errmessage);

                if (sumdetailList.Count > 0)
                {
                    if (dt.Rows[j]["EndUserName"].ToString().Trim() == sumdetailList[0].EndUsername)
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
                                    sumdetail(sumdetailList, _list, Convert.ToDecimal(dt.Rows[j]["Cost"]), Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), Convert.ToDecimal(dt.Rows[j]["ExtendedMSRP"]));

                                    categoryexist = true;
                                    break;
                                }
                                else
                                    categoryexist = false;
                            }
                            if (!categoryexist)
                            {
                                //addList(sumdetailList, dt.Rows[j]["VendorFullName"].ToString().Trim(), dt.Rows[j]["APAccountFullName"].ToString().Trim(), dt.Rows[j]["txnDate"].ToString().Trim(),
                                //    dt.Rows[j]["DueDate"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["AmountDue"].ToString().Trim()), dt.Rows[j]["CIN"].ToString().Trim(), dt.Rows[j]["Terms"].ToString().Trim(),
                                //    dt.Rows[j]["Memo"].ToString().Trim(), dt.Rows[j]["IsPaid"].ToString().Trim(), dt.Rows[j]["OpenAccount"].ToString().Trim(), dt.Rows[j]["ExpAccountFullName"].ToString().Trim(),
                                //    dt.Rows[j]["FkSku"].ToString().Trim(), dt.Rows[j]["Subscription"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["Cost"]),
                                //    Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), dt.Rows[j]["ItemCustomerFullName"].ToString().Trim(), dt.Rows[j]["ItemBillableStatus"].ToString().Trim(),
                                //    dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname);

                                addList(sumdetailList, dt.Rows[j]["VendorFullName"].ToString().Trim(), dt.Rows[j]["APAccountFullName"].ToString().Trim(), dt.Rows[j]["txnDate"].ToString().Trim(),
                                    dt.Rows[j]["DueDate"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["AmountDue"].ToString().Trim()), dt.Rows[j]["CIN"].ToString().Trim(), dt.Rows[j]["Terms"].ToString().Trim(),
                                    dt.Rows[j]["Memo"].ToString().Trim(), dt.Rows[j]["IsPaid"].ToString().Trim(), dt.Rows[j]["OpenAccount"].ToString().Trim(), dt.Rows[j]["ExpAccountFullName"].ToString().Trim(),
                                    categoryname.Trim(), categoryname.Trim(), Convert.ToDecimal(dt.Rows[j]["Cost"]),
                                    Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), dt.Rows[j]["ItemCustomerFullName"].ToString().Trim(), dt.Rows[j]["ItemBillableStatus"].ToString().Trim(),
                                    dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname, dt.Rows[j]["EndUserName"].ToString().Trim(),Convert.ToDecimal(dt.Rows[j]["ExtendedMSRP"].ToString().Trim()));
                            }
                        }
                    }
                    else
                    {
                        for (int a = 0; a <= sumdetailList.Count - 1; a++)
                        {
                            //summary(sumdetailList[a].client, sumdetailList[a].skucategoryname, sumdetailList[a].skuid, sumdetailList[a].cost, sumdetailList[a].itemamount, ref summessage, ref errmessage);
                            summary(sumdetailList[a].vendorname, sumdetailList[a].apaccfullname, sumdetailList[a].txndate, sumdetailList[a].duedate, sumdetailList[a].amountdue, sumdetailList[a].refnum,
                                sumdetailList[a].terms, sumdetailList[a].memo, sumdetailList[a].ispaid, sumdetailList[a].openaccount, sumdetailList[a].expaccfullname, sumdetailList[a].sku, sumdetailList[a].desc,
                                sumdetailList[a].cost, sumdetailList[a].itemamount, sumdetailList[a].client, sumdetailList[a].itembillablestatus, sumdetailList[a].ItemSalesRepRefFullName,
                                sumdetailList[a].extendedmsrp, ref summessage, ref errmessage);
                            str.Append(summessage);
                        }
                        sumdetailList.Clear();

                        if (billrollup == "N" || billrollup == "")
                        {
                            nonrolluprow(dt, j, ref str);
                        }
                        else if (billrollup == "Y")
                        {
                            //addList(sumdetailList, dt.Rows[j]["VendorFullName"].ToString().Trim(), dt.Rows[j]["APAccountFullName"].ToString().Trim(), dt.Rows[j]["txnDate"].ToString().Trim(),
                            //        dt.Rows[j]["DueDate"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["AmountDue"].ToString().Trim()), dt.Rows[j]["CIN"].ToString().Trim(), dt.Rows[j]["Terms"].ToString().Trim(),
                            //        dt.Rows[j]["Memo"].ToString().Trim(), dt.Rows[j]["IsPaid"].ToString().Trim(), dt.Rows[j]["OpenAccount"].ToString().Trim(), dt.Rows[j]["ExpAccountFullName"].ToString().Trim(),
                            //        dt.Rows[j]["FkSku"].ToString().Trim(), dt.Rows[j]["Subscription"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["Cost"]),
                            //        Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), dt.Rows[j]["ItemCustomerFullName"].ToString().Trim(), dt.Rows[j]["ItemBillableStatus"].ToString().Trim(),
                            //        dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname);
                            addList(sumdetailList, dt.Rows[j]["VendorFullName"].ToString().Trim(), dt.Rows[j]["APAccountFullName"].ToString().Trim(), dt.Rows[j]["txnDate"].ToString().Trim(),
                                    dt.Rows[j]["DueDate"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["AmountDue"].ToString().Trim()), dt.Rows[j]["CIN"].ToString().Trim(), dt.Rows[j]["Terms"].ToString().Trim(),
                                    dt.Rows[j]["Memo"].ToString().Trim(), dt.Rows[j]["IsPaid"].ToString().Trim(), dt.Rows[j]["OpenAccount"].ToString().Trim(), dt.Rows[j]["ExpAccountFullName"].ToString().Trim(),
                                    categoryname.Trim(), categoryname.Trim(), Convert.ToDecimal(dt.Rows[j]["Cost"]),
                                    Convert.ToDecimal(dt.Rows[j]["ItemAmount"]), dt.Rows[j]["ItemCustomerFullName"].ToString().Trim(), dt.Rows[j]["ItemBillableStatus"].ToString().Trim(),
                                    dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname, dt.Rows[j]["EndUserName"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["ExtendedMSRP"].ToString().Trim()));
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
                                    dt.Rows[j]["ItemSalesRepRefFullName"].ToString().Trim(), categoryname, dt.Rows[j]["EndUserName"].ToString().Trim(), Convert.ToDecimal(dt.Rows[j]["ExtendedMSRP"].ToString().Trim()));
                    }
                }
            }
            for (int a = 0; a <= sumdetailList.Count - 1; a++)
            {
                //summary(sumdetailList[a].client, sumdetailList[a].skucategoryname, sumdetailList[a].skuid, sumdetailList[a].cost, sumdetailList[a].itemamount, ref summessage, ref errmessage);
                summary(sumdetailList[a].vendorname, sumdetailList[a].apaccfullname, sumdetailList[a].txndate, sumdetailList[a].duedate, sumdetailList[a].amountdue, sumdetailList[a].refnum,
                                sumdetailList[a].terms, sumdetailList[a].memo, sumdetailList[a].ispaid, sumdetailList[a].openaccount, sumdetailList[a].expaccfullname, sumdetailList[a].sku, sumdetailList[a].desc,
                                sumdetailList[a].cost, sumdetailList[a].itemamount, sumdetailList[a].client, sumdetailList[a].itembillablestatus, sumdetailList[a].ItemSalesRepRefFullName,
                                sumdetailList[a].extendedmsrp, ref summessage, ref errmessage);
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
        }
        catch (Exception ex)
        {
            litMessage.Text = ex.Message;
            return;
        }
    }

    protected string title(string columnname)
    {
        if (columnname.ToLower() == "fksku")
            return "SKU#";
        else
            return columnname;
    }
    
    protected void addList(List<SumDetail> detail, string vendor, string apaccfullname, string txndate, string duedate, decimal amountdue,string cin,
        string terms, string memo, string ispaid,string openaccount, string expaccfullname, string skuid, string desc, decimal cost, decimal itemamount,
        string itemclienfullname, string ItemBillableStatus, string ItemSalesRepRefFullName, string categoryname, string endusername, decimal extendedMSRP)
    {
        detail.Add(new SumDetail
        {
            vendorname= vendor,
            apaccfullname= apaccfullname,
            txndate= txndate,
            duedate= duedate,
            amountdue= amountdue,
            refnum= cin,
            terms=terms,
            memo=memo,
            ispaid=ispaid,
            openaccount=openaccount,
            expaccfullname=expaccfullname,
            sku=skuid,
            desc=desc,
            cost = cost,
            itemamount = itemamount,
            client = itemclienfullname,
            itembillablestatus=ItemBillableStatus,
            ItemSalesRepRefFullName=ItemSalesRepRefFullName,
            skucategoryname = categoryname,
            EndUsername = endusername,
            extendedmsrp=extendedMSRP
        });
    }
    
    protected bool summary(string vendor, string apaccfullname, string txndate, string duedate, decimal amountdue, string cin,
        string terms, string memo, string ispaid, string openaccount, string expaccfullname, string skuid, string desc, decimal cost, decimal itemamount,
        string endusername, string ItemBillableStatus, string ItemSalesRepRefFullName, decimal extendedmsrp, ref string message, ref string errmessage)
    {
        try
        {
            StringBuilder strsummary= new StringBuilder();

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
                str.Append(dt.Rows[counter][dt.Columns[col].ColumnName].ToString().Trim());
                str.Append("</td>");
            }
        }
        str.Append("</tr>");
    }

    protected void sumdetail(List<SumDetail> detailList, int counter, decimal cost, decimal itemamount, decimal extendedMSRP)
    {
        detailList[counter].cost += cost;
        detailList[counter].itemamount += itemamount;
        detailList[counter].extendedmsrp += extendedMSRP;
    }

    protected string caption(string dataneedtochanges,string originalcaption,string newcaption)
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
        public decimal extendedmsrp { get; set; }
        //public decimal tax { get; set; }
    }
    #endregion


}