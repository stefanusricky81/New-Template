using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_ListBox_Days : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _defaultValue = "-1";
    private string _cssClass = "";
    private bool _displayDefaultValue = true;
    private bool _setSize = true;
    private int _selectedId = 0;

    private List<string> Days = new List<string>
    {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (lbDays.Items.Count == 0)
            Populate();
    }

    public void Populate(List<string> selectedTextValues = null)
    {
        lbDays.DataSource = Days;
        lbDays.DataBind();
        if (_selectedId > 0)
        {
            bool delimiterCheck = true;
            ListItem liDays = lbDays.Items.FindByValue(_selectedId.ToString());
            if (liDays != null)
            {
                liDays.Selected = true;
                delimiterCheck = false;
            }

            if (delimiterCheck)
            {
                foreach (ListItem li in lbDays.Items)
                {
                    int nPos = li.Value.IndexOf("|");
                    if (nPos > 0)
                    {
                        int liValue = Convert.ToInt32(li.Value.Substring(0, nPos));
                        if (liValue == _selectedId)
                        {
                            li.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        if (selectedTextValues != null)
        {
            foreach (string tag in selectedTextValues)
            {
                ListItem li = lbDays.Items.FindByValue(tag.Trim());
                if (li != null)
                    li.Selected = true;
            }
        }

        if (_setSize)
        {
            //ddlEmployee.Width = Unit.Pixel(100);
            lbDays.Height = Unit.Pixel(20);
        }

        if (!String.IsNullOrEmpty(_cssClass))
            lbDays.CssClass = _cssClass;
    }

    #region public properties
    public ListBox GetListBox()
    {
        return lbDays;
    }

    public List<string> SelectedValues
    {
        get
        {
            List<string> _vals = new List<string>();
            foreach (ListItem _li in lbDays.Items)
            {
                if (_li.Selected)
                    _vals.Add(_li.Value.Trim());
            }
            return _vals;
        }
    }

    public List<string> SelectedText
    {
        get
        {
            List<string> _vals = new List<string>();
            foreach (ListItem _li in lbDays.Items)
            {
                if (_li.Selected)
                    _vals.Add(_li.Text.Trim());
            }
            return _vals;
        }
    }

    public string ValidationGroup
    {
        set
        {
            rfvDays.Visible = true;
            rfvDays.ValidationGroup = value;
        }
    }

    public bool Enabled
    {
        set
        {
            rfvDays.Enabled = value;
        }
    }

    public bool Visible
    {
        set
        {
            rfvDays.Visible = value;
        }
    }

    public bool ClearData
    {
        set 
        {
            if (value)
                lbDays.Items.Clear();
        }
    }

    /// <summary>
    /// set error message for required field validator
    /// </summary>
    public string RequiredErrorMessage
    {
        set { rfvDays.ErrorMessage = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set
        {
            lbDays.CssClass = value;
            _cssClass = value;
        }
    }

    public bool DisplayChosenScript
    {
        set { litJs.Visible = value; }
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
        sb.Append(String.Format("SetChosenTicketTag_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenTicketTag_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenTicketTag_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", lbDays.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_multiple: \"Select Days ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }



    #endregion
}