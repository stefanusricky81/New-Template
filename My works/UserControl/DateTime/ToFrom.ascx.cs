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

public partial class UserControl_DateTime_ToFrom : System.Web.UI.UserControl
{
    private int _maxDaysDifference = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region public properties

    /// <summary>
    /// get/set start date
    /// </summary>
    public DateTime StartDate
    {
        get
        {
            if (rdpStart.SelectedDate.HasValue)
                return rdpStart.SelectedDate.Value;
            else
                return DateTime.MinValue;
        }
        set { rdpStart.SelectedDate = value; }
    }

    /// <summary>
    /// get/set end date
    /// </summary>
    public DateTime EndDate
    {
        get
        {
            if (rdpEnd.SelectedDate.HasValue)
                return rdpEnd.SelectedDate.Value;
            else
                return DateTime.MinValue;
        }
        set { rdpEnd.SelectedDate = value; }
    }

    /// <summary>
    /// set compare error message
    /// </summary>
    public string ErrorMessageCompare
    {
        set { cvDates.ErrorMessage = value; }
    }

    /// <summary>
    /// validate end date is greater than start date
    /// </summary>
    public bool ValidateDates
    {
        set { cvDates.Visible = value; }
    }

    /// <summary>
    /// set start date is required
    /// </summary>
    public bool StartDateRequired
    {
        set { rfvStartDate.Visible = value; }
    }

    /// <summary>
    /// set end date is required
    /// </summary>
    public bool EndDateRequired
    {
        set { rfvEndDate.Visible = value; }
    }

    /// <summary>
    /// set maximum number of day difference for start vs end date
    /// </summary>
    public int MaxixumDayDifference
    {
        set
        {
            customvDates.Visible = true;
            _maxDaysDifference = value;
        }
    }

    /// <summary>
    /// set error message for maximum number of day difference for start vs end date
    /// </summary>
    public string MaxixumDayDifferenceErrorMessage
    {
        set { customvDates.ErrorMessage = value; }
    }

    /// <summary>
    /// set validation group for validators
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            customvDates.ValidationGroup = value;
            cvDates.ValidationGroup = value;
            rfvStartDate.ValidationGroup = value;
            rfvEndDate.ValidationGroup = value;
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// clear start & end date
    /// </summary>
    public void ClearAll()
    {
        rdpStart.Clear();
        rdpEnd.Clear();
    }

    /// <summary>
    /// clear start date
    /// </summary>
    public void ClearStartDate()
    {
        rdpStart.Clear();
    }

    /// <summary>
    /// clear end date
    /// </summary>
    public void ClearEndDate()
    {
        rdpEnd.Clear();
    }

    #endregion

    #region protected methods

    /// <summary>
    /// rad date picker on children created
    /// add clear date hyperlink
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdp_ChildrenCreated(object sender, System.EventArgs e)
    {
        RadDatePicker picker = (RadDatePicker)sender;

        HyperLink clearLink = new HyperLink();
        clearLink.NavigateUrl = string.Format("javascript:$find('{0}').clear()", picker.ClientID);
        clearLink.Text = "Clear";

        picker.Controls.Add(clearLink);
    }

    #endregion

    #region custom validators

    protected void customvDates_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (_maxDaysDifference > 0)
        {
            TimeSpan _ts = EndDate - StartDate;
            int _days = _ts.Days;

            if (_days > _maxDaysDifference)
            {
                args.IsValid = false;
                return;
            }
        }

        args.IsValid = true;
    }

    #endregion
}

