using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class UserControl_DateTime_DateAndTime : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region public properties

    /// <summary>
    /// get/set selected date & time
    /// </summary>
    public DateTime SelctedDateTime
    {
        get
        {
            if (rdpMain.SelectedDate.HasValue)
            {
                int _hour = 0;
                int _minute = 0;

                if (ucTime.Hour>0)
                    _hour = ucTime.Hour;

                if (ucTime.Minute>0)
                    _minute = ucTime.Minute;

                DateTime _selectedDateTime = new DateTime(
                    rdpMain.SelectedDate.Value.Year,
                    rdpMain.SelectedDate.Value.Month,
                    rdpMain.SelectedDate.Value.Day, 
                    _hour, 
                    _minute, 
                    0);
                return _selectedDateTime;
            }
            else
                return DateTime.MinValue;
        }
        set
        { 
            rdpMain.SelectedDate = value;
            ucTime.SelectedDateTime = value;
        }
    }

    /// <summary>
    /// set css clss for hour & minute drop down list controls
    /// </summary>
    public string TimeCssClass
    {
        set { ucTime.CssClass = value; }
    }

    /// <summary>
    /// set date required error message
    /// </summary>
    public string DateRequiredErrorMessage
    {
        set { rfvDate.ErrorMessage = value; }
    }

    /// <summary>
    /// set hour required error message
    /// </summary>
    public string HourRequiredErrorMessage
    {
        set { ucTime.HourRequiredErrorMessage = value; }
    }

    /// <summary>
    /// set minute required error message
    /// </summary>
    public string MinuteRequiredErrorMessage
    {
        set { ucTime.MinuteRequiredErrorMessage = value; }
    }

    /// <summary>
    /// set validation group fro required validators
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            ucTime.ValidationGroup = value;
            rfvDate.ValidationGroup = value;
            cvTime.ValidationGroup = value;
        }
    }

    /// <summary>
    /// set date & time is required
    /// </summary>
    public bool IsRequired
    {
        set
        {
            ucTime.IsRequired = value;
            rfvDate.Visible = value;
        }
    }

    public bool DisableTimeRequired
    {
        set
        {
            if (value)
                ucTime.IsRequired = false;
        }
    }

    /// <summary>
    /// set time is required if date is selected
    /// </summary>
    public bool TimeRequiredIfDateSelected
    {
        set { cvTime.Visible = value; }
    }

    /// <summary>
    /// set time is required if date is selected error message
    /// </summary>
    public string TimeRequiredIfDateSelectedErrorMessage
    {
        set { cvTime.ErrorMessage = value; }
    }

    #endregion

    #region public methods / events

    /// <summary>
    /// clear selected date
    /// </summary>
    public void ClearAll()
    {
        rdpMain.Clear();
    }

    #endregion

    #region protected methods / events

    /// <summary>
    /// rad date picker on children created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdpMain_ChildrenCreated(object sender, System.EventArgs e)
    {
        RadDatePicker picker = (RadDatePicker)sender;

        DropDownList ddlHour = ucTime.GetHourDropDownList();
        DropDownList ddlMinute = ucTime.GetMinuteDropDownList();

        HyperLink clearLink = new HyperLink();
        clearLink.NavigateUrl = string.Format("javascript:$find('{0}').clear();javascript:SelectItemInDDL('{1}','{3}');javascript:SelectItemInDDL('{2}','{3}')", 
            picker.ClientID, ddlHour.ClientID, ddlMinute.ClientID, "");
        clearLink.Text = "Clear";

        picker.Controls.Add(clearLink);
    }

    #region custom validator

    /// <summary>
    /// validate time is entered if date is entered
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvTime_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (!rdpMain.SelectedDate.HasValue) || (ucTime.Hour > 0) || (ucTime.Minute > 0);
    }

    #endregion

    #endregion


}
