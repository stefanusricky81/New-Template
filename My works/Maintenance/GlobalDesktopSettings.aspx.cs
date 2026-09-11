using DesktopShared.CollectionClasses;
using DesktopShared.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Maintenance_GlobalDesktopSettings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        litMessage.Text = "";
        if (!IsPostBack)
            loadData();
    }

    protected void loadData()
    {
        LoadDataAutoClose();
        LoadCreditCardFee();

        txtSurveyId.Text = DesktopShared.ReportSettings.SurveyId();
        chkSendMailSurveyID.Checked = DesktopShared.ReportSettings.SendEmail(txtSurveyId.Text.Trim());
    }

    private void DisplayMessage(string message, Bootstrap.Alert.AlertType alertType, bool dismissable = true, bool hideMainPanel = false)
    {
        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
        litMessage.Visible = true;
        litMessage.Text = Bootstrap.Alert.GetDivText(alertType, message, dismissable, true, "");
        //pnlMain.Visible = !hideMainPanel;
    }

    #region Auto Close
    private void LoadDataAutoClose()
    {
        try
        {
            GlobalDesktopSettingCollection _globalsetting = new GlobalDesktopSettingCollection();
            var filter = new PredicateExpression { DesktopShared.HelperClasses.GlobalDesktopSettingFields.Availablefor == "autoclose" };
            _globalsetting.GetMulti(filter);

            if (_globalsetting.Count > 0)
            {
                txtFirst.Text = _globalsetting[0].FirstNotificationHours.ToString().Trim();
                txtFirstNotification.Content = _globalsetting[0].FirstNotificationEmail;
                txtSecond.Text = _globalsetting[0].SecondNotificationHours.ToString().Trim();
                txtSecondNotification.Content = _globalsetting[0].SecondNotifcationEmail;
                txtAuto.Text = _globalsetting[0].AutoClosedHours.ToString().Trim();
                txtAutoNotification.Content = _globalsetting[0].AutoClosedEmail;
            }
        }
        catch (Exception ex)
        {
            litMessage.Text = ex.Message.Trim();
        }
    }
    protected void lbSave_Click(object sender, EventArgs e)
    {
        try
        {
            GlobalDesktopSettingEntity _gs = null;

            GlobalDesktopSettingCollection _globalsetting = new GlobalDesktopSettingCollection();
            var filter = new PredicateExpression { DesktopShared.HelperClasses.GlobalDesktopSettingFields.Availablefor == "autoclose" };
            _globalsetting.GetMulti(filter);

            if (_globalsetting.Count == 0)
            {
                _gs = new GlobalDesktopSettingEntity();
                _gs.Createddate = DateTime.Now;
                _gs.Createdby = DesktopShared.User.UserID;
            }
            else if (_globalsetting.Count > 0)
            {
                _gs = new GlobalDesktopSettingEntity(_globalsetting[0].Id);
                _gs.Updatedate = DateTime.Now;
                _gs.Updateby = DesktopShared.User.UserID;
            }
            if (!String.IsNullOrEmpty(txtFirst.Text))
                _gs.FirstNotificationHours = Convert.ToInt16(txtFirst.Text);
            if (!String.IsNullOrEmpty(txtFirstNotification.Text))
                _gs.FirstNotificationEmail = txtFirstNotification.Content;

            if (!String.IsNullOrEmpty(txtSecond.Text))
                _gs.SecondNotificationHours = Convert.ToInt16(txtSecond.Text);
            if (!String.IsNullOrEmpty(txtSecondNotification.Text))
                _gs.SecondNotifcationEmail = txtSecondNotification.Content;

            if (!String.IsNullOrEmpty(txtAuto.Text))
                _gs.AutoClosedHours = Convert.ToInt16(txtAuto.Text);
            if (!String.IsNullOrEmpty(txtAutoNotification.Text))
                _gs.AutoClosedEmail = txtAutoNotification.Content;

            _gs.Availablefor = "autoclose";

            _gs.Save();

            loadData();

        }
        catch (Exception ex)
        {
            litMessage.Text = ex.Message.Trim();
        }
    }
    #endregion

    #region Credit card Fee
    private void LoadCreditCardFee()
    {
        try
        {
            PaymentFeeCollection _paymentfee = new PaymentFeeCollection();
            var filter = new PredicateExpression { DesktopShared.HelperClasses.PaymentFeeFields.PaymentType == 1 };
            _paymentfee.GetMulti(filter);

            if (_paymentfee.Count > 0)
            {
                txtFee.Text = (_paymentfee[0].Fee * 100).ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            litMessage.Text = ex.Message.Trim();
        }
    }

    protected void lbSaveCreditCardFees_Click(object sender, EventArgs e)
    {
        try {
            decimal _fee = Convert.ToDecimal(txtFee.Text);

            if (_fee < 0)
            {
                DisplayMessage("Do not allow negative numbers", Bootstrap.Alert.AlertType.Danger);
                return;
            }

            if (_fee >= 10 && _fee >= 0)
            {
                DisplayMessage("Only allow 0 to 10 ", Bootstrap.Alert.AlertType.Danger);
                return;
            }

            PaymentFeeEntity _pf = null;

            PaymentFeeCollection _paymentfee = new PaymentFeeCollection();
            var filter = new PredicateExpression { DesktopShared.HelperClasses.PaymentFeeFields.PaymentType == 1 };
            _paymentfee.GetMulti(filter);

            if (_paymentfee.Count > 0)
            {
                _pf = new PaymentFeeEntity(_paymentfee[0].Id);
                _pf.Lastupdated = DateTime.Now;
                _pf.LastupdatedBy = DesktopShared.User.UserID;
                _pf.Fee = Convert.ToDecimal(txtFee.Text) / 100;
            }

            _pf.Save();
            LoadCreditCardFee();
        }
        catch (Exception ex)
        {
            litMessage.Text = ex.Message.Trim();
        }
    }

    #endregion

    protected void lbSurveyId_Click(object sender, EventArgs e)
    {
        try
        {
            ReportSettingEntity _rs;
            ReportSettingCollection _rsc = new ReportSettingCollection();
            IPredicateExpression _rscFilter = new PredicateExpression();
            _rscFilter.AddWithAnd(DesktopShared.HelperClasses.ReportSettingFields.ForReport == BitByBit.Configuration.GetConfigString("SurveyID"));

            //fetch
            _rsc.GetMulti(_rscFilter, 0, null);

            if (_rsc.Count == 0)
            {
                _rs = new ReportSettingEntity();
                _rs.CreatedDate = DateTime.Now;
                _rs.CreatedBy = DesktopShared.User.UserID;
            }
            else
            {
                _rs = new ReportSettingEntity(_rsc[0].Id);
                _rs.LastUpdateDate = DateTime.Now;
                _rs.LastUpdateby = DesktopShared.User.UserID;
            }

            _rs.ForReport = BitByBit.Configuration.GetConfigString("SurveyID");
            _rs.Description = txtSurveyId.Text.Trim();
            _rs.Feature = chkSendMailSurveyID.Checked;
            _rs.Save();
            _rs.Refetch();

            DisplayMessage("Survey Id succesfully added", Bootstrap.Alert.AlertType.Success);
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString().Trim(), Bootstrap.Alert.AlertType.Danger);
        }
    }
}
