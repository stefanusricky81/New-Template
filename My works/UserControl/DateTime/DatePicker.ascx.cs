using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_DateTime_DatePicker : System.Web.UI.UserControl
{
    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!TextBoxOnly)
        {
            if (IsTabletOrSmartPhone(this.Request))
            {
                txtDate.Attributes.Add("type", "date");
                phButton.Visible = false;
            }
            else
                DisplayDatepickerPluginJs();
        }
    }   

    #region public properties

    /// <summary>
    /// get/set selected date time
    /// </summary>
    public DateTime? SelectedDate
    {
        get
        {
            string _selectedDate = txtDate.Text.Trim();
            if (String.IsNullOrWhiteSpace(_selectedDate))
                return null;

            DateTime _returnDate = DateTime.MinValue;
            if (DateTime.TryParse(_selectedDate, out _returnDate))
            {
                DateTime minValue = DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString());
                DateTime maxValue = DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MaxValue.ToString());
                if (minValue > _returnDate || maxValue < _returnDate)
                    return null;

                return _returnDate;

            }
            else
                return null;
        }
        set
        {
            if (!value.HasValue)
                return;
            if (IsTabletOrSmartPhone(this.Request) && !TextBoxOnly)
                txtDate.Text = value.Value.ToString("yyyy-MM-dd");
            else
                txtDate.Text = value.Value.ToString("MM/dd/yyyy");
        }
    }

    /// <summary>
    /// set text box type
    /// </summary>
    public string TextBoxType
    {
        set
        {
            txtDate.Attributes.Remove("type");
            txtDate.Attributes.Add("type", value);
        }
    }

    /// <summary>
    /// set place holder text
    /// </summary>
    public string PlaceHolderText
    {
        set { txtDate.Attributes.Add("placeholder", value); }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { txtDate.TabIndex = value; }
    }

    /// <summary>
    /// set validation group
    /// </summary>
    public string ValidationGroup
    {
        set
        {
            rfvDate.ValidationGroup = value;
            cvDate.ValidationGroup = value;
            rvDate.ValidationGroup = value;
        }
    }

    /// <summary>
    /// set drop down selection is required
    /// </summary>
    public bool IsRequired
    {
        set
        {
            rfvDate.Visible = value;
            cvDate.Visible = value;
            rvDate.Visible = value;
        }
    }

    /// <summary>
    /// set validation error message
    /// </summary>
    public string ErrorMessage
    {
        set
        {
            rfvDate.ErrorMessage = value;
            cvDate.ErrorMessage = value;
            rvDate.ErrorMessage = value;
        }
    }

    /// <summary>
    /// set display asterisk for error
    /// </summary>
    public bool DisplayErrorAsterisk
    {
        set { litError.Visible = value; }
    }

    /// <summary>
    /// set validator display type
    /// </summary>
    public ValidatorDisplay validatorDisplay
    {
        set { rfvDate.Display = value; }
    }

    /// <summary>
    /// set validator fore color
    /// </summary>
    public System.Drawing.Color ValidatorForeColor
    {
        set { rfvDate.ForeColor = value; }
    }

    /// <summary>
    /// set control is enabled
    /// </summary>
    public bool Enabled
    {
        set
        {
            txtDate.Enabled = value;
            txtDate.ReadOnly = value;
            litJs.Visible = value;
        }
    }

    /// <summary>
    /// set control width
    /// </summary>
    public int ControlWidth
    {
        set { txtDate.Width = value;  }
    }

    /// <summary>
    /// get text box control
    /// </summary>
    public TextBox TextBoxControl
    {
        get { return txtDate;  }
    }

    #endregion

    #region public methods

    /// <summary>
    /// clear selection
    /// </summary>
    public void Clear()
    {
        txtDate.Text = "";
    }

    public void SetAsTextBoxOnly()
    {
        litJs.Visible = false;
        phButton.Visible = false;
        txtDate.Attributes.Remove("type");
        txtDate.Attributes.Add("placeholder", "mm/dd/yyyy");
        TextBoxOnly = true;
        txtDate.CssClass = "form-control";
        litDivStart.Visible = false;
        litDivEnd.Visible = false;
    }
    public static bool IsTabletOrSmartPhone(HttpRequest request)
    {
        if (IsTablet(request))
            return true;
        return IsSmartPhone(request);
    }
    public static bool IsTablet(HttpRequest request)
    {
        string _userAgent = request.UserAgent;
        if (_userAgent == null)
            return false;

        Regex r = new Regex("ipad|android|android 3.0|xoom|sch-i800|playbook|tablet|kindle|nexus", RegexOptions.IgnoreCase);
        return r.IsMatch(_userAgent);
    }
    public static bool IsSmartPhone(HttpRequest request)
    {
        if (IsTablet(request))
            return false;

        string _userAgent = request.UserAgent;
        if (_userAgent == null)
            return false;

        return request.Browser.IsMobileDevice;
    }
    #endregion

    #region protected events

    #region custom validator

    /// <summary>
    /// validate date
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string _selectedDate = txtDate.Text.Trim();
        if (String.IsNullOrWhiteSpace(_selectedDate))
        {
            args.IsValid = false;
            return;
        }

        DateTime _returnDate = DateTime.MinValue;
        if (DateTime.TryParse(_selectedDate, out _returnDate))
        {
            DateTime minValue = DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString());
            DateTime maxValue = DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MaxValue.ToString());
            if (minValue > _returnDate || maxValue < _returnDate)
            {
                args.IsValid = false;
                return;
            }
        }
        else
        {
            args.IsValid = false;
            return;

        }

        args.IsValid = true;
    }

    #endregion

    #endregion

    #region private methods

        /// <summary>
        /// display script used by datepicker plugin    
        /// </summary>
    private void DisplayDatepickerPluginJs()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("$(document).ready(function() {");
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoadedDatePicker_{0});", this.ClientID));
        sb.Append(String.Format("PageLoadedDatePicker_{0}();", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function PageLoadedDatePicker_{0}(sender, args) {{", this.ClientID));
        //sb.Append(String.Format("$('#{0}').datepicker({{", txtDate.ClientID));
        sb.Append("$('.input-group.date').datepicker({");
        sb.Append("format: 'mm/dd/yyyy',");
        sb.Append("todayHighlight: true,");
        sb.Append("clearBtn: true,");
        sb.Append("showOnFocus: false,");
        sb.Append("assumeNearbyYear: true,");
        sb.Append("autoclose: true,");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set employee id
    /// </summary>
    private bool TextBoxOnly
    {
        get
        {
            object obj = this.ViewState["tbo_dp"];
            return (obj == null) ? false : (bool)obj;
        }
        set { this.ViewState["tbo_dp"] = value; }
    }

    #endregion

    
}