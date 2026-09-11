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

public partial class UserControl_DropDownList_TimesheetTime : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _defaultValue = "";
    private bool _displayDefaultValue = true;
    private int _selectedHour = -1;
    private int _selectedMinute = -1;
    private int _hourStart = 0;
    private string _cssClass = "form-control select-chosen";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();

        if (ddlHour.Items.Count == 0)
            PopulateHour();

        if (ddlMinute.Items.Count == 0)
            PopulateMinute();
    }

    #region public methods

    /// <summary>
    /// populate hour drop down list
    /// </summary>
    public void PopulateHour()
    {
        ddlHour.Items.Clear();
        for (int i = _hourStart; i < 25; i++)
        {
            string display = i.ToString().PadLeft(2, '0'); ;
            ddlHour.Items.Add(new ListItem(display, i.ToString()));
        }

        if (_selectedHour >= 0)
        {
            ListItem li = ddlHour.Items.FindByValue(_selectedHour.ToString());
            if (li != null)
                li.Selected = true;
        }

        if (_displayDefaultValue)
            ddlHour.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

        if (!String.IsNullOrEmpty(_cssClass))
            ddlHour.CssClass = _cssClass;
    }

    /// <summary>
    /// populate minute drop down list
    /// </summary>
    public void PopulateMinute()
    {
        ddlMinute.Items.Clear();

        if (UseMinuteIncrement)
        {
            for (int i = 0; i < 61; i++)
            {
                string display = i.ToString().PadLeft(2, '0'); ;
                ddlMinute.Items.Add(new ListItem(display, i.ToString()));
            }
        }
        else
        {
            ddlMinute.Items.Insert(0, new ListItem("00", "00"));
            ddlMinute.Items.Insert(1, new ListItem("15", "15"));
            ddlMinute.Items.Insert(2, new ListItem("30", "30"));
            ddlMinute.Items.Insert(3, new ListItem("45", "45"));
        }
        
        if (_selectedMinute > -1)
        {
            ListItem li = ddlMinute.Items.FindByValue(_selectedMinute.ToString().PadLeft(2, '0'));
            if (li != null)
                li.Selected = true;
        }

        if (_displayDefaultValue)
            ddlMinute.Items.Insert(0, new ListItem(_defaultText, _defaultValue));

        if (!String.IsNullOrEmpty(_cssClass))
            ddlMinute.CssClass = _cssClass;
    }

    /// <summary>
    /// get hour drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetHourDropDownList()
    {
        return ddlHour;
    }

    /// <summary>
    /// get minute drop down list
    /// </summary>
    /// <returns></returns>
    public DropDownList GetMinuteDropDownList()
    {
        return ddlMinute;
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set hour
    /// </summary>
    public int Hour
    {
        get
        { 
            if (!String.IsNullOrEmpty(ddlHour.SelectedValue))
                return Convert.ToInt32(ddlHour.SelectedValue); 
            else
                return -1;
        }
        set
        {
            _selectedHour = value;
            PopulateHour();
        }
    }

    /// <summary>
    /// get/set minute
    /// </summary>
    public int Minute
    {
        get
        { 
            if (!String.IsNullOrEmpty(ddlMinute.SelectedValue))
                return Convert.ToInt32(ddlMinute.SelectedValue); 
            else
                return -1;
        }
        set
        {
            _selectedMinute = value;
            PopulateMinute();
        }
    }

    /// <summary>
    /// get/set use minute increment rather than 15 
    /// </summary>
    public bool UseMinuteIncrement
    {
        get
        {
            object obj = this.ViewState["umi_tt"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["umi_tt"] = value; }
    }

    /// <summary>
    /// set display headers
    /// </summary>
    public bool DisplayHeaders
    {
        set
        {
            phHourHeader.Visible = value;
            phMinuteHeader.Visible = value;
        }
    }

    /// <summary>
    /// set selected time via date time
    /// </summary>
    public DateTime SelectedDateTime
    {
        set
        {
            Minute = value.Minute;
            Hour = value.Hour;
        }
    }

    /// <summary>
    /// set text for default list item
    /// </summary>
    public string DefaultText
    {
        set { _defaultText = value; }
    }

    /// <summary>
    /// set value for default list item
    /// </summary>
    public string DefaultValue
    {
        set { _defaultValue = value; }
    }

    /// <summary>
    /// set display default list item
    /// </summary>
    public bool DisplayDefaultValue
    {
        set { _displayDefaultValue = value; }
    }

    /// <summary>
    /// set start hour at 0
    /// </summary>
    public bool StartHourAtZero
    {
        set
        {
            if (value)
                _hourStart = 0;
        }
    }

    /// <summary>
    /// set is required
    /// </summary>
    public bool IsRequired
    {
        set
        {
            rfvHour.Visible = value;
            rfvMinute.Visible = value;
        }
    }

    /// <summary>
    /// set validation group for required field validators
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            rfvHour.ValidationGroup = value;
            rfvMinute.ValidationGroup = value;
        }
    }

    /// <summary>
    /// set error message for required hour
    /// </summary>
    public string HourRequiredErrorMessage
    {
        set { rfvHour.ErrorMessage = value; }
    }

    /// <summary>
    /// set error message for required minute
    /// </summary>
    public string MinuteRequiredErrorMessage
    {
        set { rfvMinute.ErrorMessage = value; }
    }

    /// <summary>
    /// set tab index for hour control
    /// </summary>
    public short HourTabIndex
    {
        set { ddlHour.TabIndex = value; }
    }

    /// <summary>
    /// set tab index for minute control
    /// </summary>
    public short MinuteTabIndex
    {
        set { ddlMinute.TabIndex = value; }
    }

    /// <summary>
    /// set css class for drop down lists
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; }
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
    }

    /// <summary>
    /// set header text for hours
    /// </summary>
    public string HourHeader
    {
        set { litHourHeader.Text = value;  }
    }

    /// <summary>
    /// set header text for minutes
    /// </summary>
    public string MinuteHeader
    {
        set { litMinuteHeader.Text = value; }
    }

    #endregion

    #region private methods

    /// <summary>
    /// display script used by chosen plugin
    /// </summary>
    private void DisplayChosenPluginJs()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenHour_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenHour_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenHour_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlHour.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select Hour ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("SetChosenMinute_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenMinute_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenMinute_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", ddlMinute.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_single: \"Select Minute ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}
