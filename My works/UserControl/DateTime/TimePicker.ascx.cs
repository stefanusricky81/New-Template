using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DateTime_TimePicker : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        /*if (ps.shared.Utility.BrowserHelper.IsTabletOrSmartPhone(this.Request))
        {
            txtTime.Attributes.Add("type", "time");
            phButton.Visible = false;
            litJs.Visible = false;
        }
        else*/
        DisplayTimePickerPluginJs();
    }

    #region public properties

    /// <summary>
    /// get/set selected date time
    /// </summary>
    public DateTime? SelectedDate
    {
        get
        {
            string _selectedDate = txtTime.Text.Trim();
            if (String.IsNullOrWhiteSpace(_selectedDate))
                return null;
            DateTime _returnDate = DateTime.MinValue;
            if (DateTime.TryParseExact(_selectedDate, "h:mm tt", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out _returnDate))
                return _returnDate;
            else
                return null;
        }
        set
        {
            if (!value.HasValue)
                return;
            txtTime.Text = value.Value.ToString("h:mmtt");

        }
    }

    /// <summary>
    /// set place holder text
    /// </summary>
    public string PlaceHolderText
    {
        set { txtTime.Attributes.Add("placeholder", value); }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { txtTime.TabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set { rfvTime.ValidationGroup = value; }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvTime.Visible = value; }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set { rfvTime.ErrorMessage = value; }
    }

    /// <summary>
    /// set validator display type
    /// </summary>
    public ValidatorDisplay validatorDisplay
    {
        set { rfvTime.Display = value; }
    }

    /// <summary>
    /// set validator fore color
    /// </summary>
    public System.Drawing.Color ValidatorForeColor
    {
        set { rfvTime.ForeColor = value; }
    }

    /// <summary>
    /// set control is enabled
    /// </summary>
    public bool Enabled
    {
        set { txtTime.Enabled = value; }
    }

    /// <summary>
    /// set control width
    /// </summary>
    public int ControlWidth
    {
        set { txtTime.Width = value; }
    }

    /// <summary>
    /// get/set minute step interval
    /// </summary>
    public int MinuteStepInterval
    {
        get
        {
            object obj = this.ViewState[String.Format("msi_{0}", this.ClientID)];
            return (obj == null) ? 15 : (int)obj;
        }
        set
        {
            this.ViewState[String.Format("msi_{0}", this.ClientID)] = value;
            DisplayTimePickerPluginJs();
        }
    }

    /// <summary>
    /// get text box controol
    /// </summary>
    public TextBox ControlTextBox
    {
        get { return txtTime; }
    }

    #endregion

    #region public methods

    /// <summary>
    /// clear selection
    /// </summary>
    public void Clear()
    {
        txtTime.Text = "";
    }

    #endregion

    #region private methods

    /// <summary>
    /// display script used by TimePicker plugin    
    /// </summary>
    private void DisplayTimePickerPluginJs()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoadedTimePicker_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function PageLoadedTimePicker_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').timepicker({{", txtTime.ClientID));
        sb.Append("defaultTime: false,");
        sb.Append("showSeconds: false,");
        //sb.Append("showMeridian: false,");
        sb.Append(String.Format("minuteStep: {0},", MinuteStepInterval));
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
        /*
        <script type="text/javascript">
            $(document).ready(function () {
                SetTimePickerEvents();
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                SetTimePickerEvents();
            });

            function SetTimePickerEvents() {
                $("#<%= txtTime.ClientID %>").timepicker().on('show.timepicker', function (e) {
                    /*console.log('The time is ' + e.time.value);
                    console.log('The hour is ' + e.time.hours);
                    console.log('The minute is ' + e.time.minutes);
                    console.log('The meridian is ' + e.time.meridian);
                    //alert(e.time.value);
                    //alert('here');
                    $("#<%= txtTime.ClientID %>").val('08:00 AM');
            });
            }
        </script>
        */
    }

    #endregion
}