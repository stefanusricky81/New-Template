using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;

public partial class UserControl_ListBox_ClientLocation : System.Web.UI.UserControl
{
    private string _defaultText = "";
    private string _cssClass = "form-control select-chosen";

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayChosenPluginJs();
        if (lbClientLocation.Items.Count == 0)
            Populate();
    }

    #region public methods

    /// <summary>
    /// populate list box
    /// </summary>
    /// <param name="selectedIds"></param>
    public void Populate(List<int> selectedIds = null)
    {
        if (ClientIdForLocation.HasValue)
        {
            lbClientLocation.DataSource = DesktopShared.Client.Location.GetActive(ClientIdForLocation.Value);
            lbClientLocation.DataTextField = "Name";
            lbClientLocation.DataValueField = "Id";
            lbClientLocation.DataBind();
        }

        if (selectedIds != null)
        {
            foreach (int _id in selectedIds)
            {
                ListItem li = lbClientLocation.Items.FindByValue(_id.ToString());
                if (li != null)
                    li.Selected = true;
            }
        }
      
        //css class
        if (!String.IsNullOrEmpty(_cssClass))
            lbClientLocation.CssClass = _cssClass;

    }

    /// <summary>
    /// remove specified item
    /// </summary>
    /// <param name="id"></param>
    public void RemoveItem(int id)
    {
        foreach (ListItem _li in lbClientLocation.Items)
        {
            if (Convert.ToInt32(_li.Value) == id)
            {
                lbClientLocation.Items.Remove(_li);
                return;
            }   
        }
    }

    #endregion

    #region public properties

    /// <summary>
    /// get selected values
    /// </summary>
    public List<int> SelectedValues
    {
        get
        {
            List<int> _vals = new List<int>();
            foreach (ListItem _li in lbClientLocation.Items)
            {
                if (_li.Selected)
                {
                    int _id = 0;
                    if (int.TryParse(_li.Value.Trim(), out _id))
                        _vals.Add(_id);
                }
            }
            return _vals;
        }
    }

    /// <summary>
    /// get/set client id
    /// </summary>
    public int? ClientIdForLocation
    {
        get
        {
            object obj = this.ViewState["cid_cl"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.ViewState["cid_cl"] = value; }
    }

    /// <summary>
    /// set tab index for control
    /// </summary>
    public short TabIndex
    {
        set { lbClientLocation.TabIndex = value; }
    }

    /// <summary>
    /// set css class for drop down list
    /// </summary>
    public string CssClass
    {
        set { _cssClass = value; }
    }

    /// <summary>
    /// get list box
    /// </summary>
    /// <returns></returns>
    public ListBox GetListBox()
    {
        return lbClientLocation;
    }

    /// <summary>
    /// set show chosen script
    /// </summary>
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
        sb.Append(String.Format("SetChosenClientLocation_{0}();", this.ClientID));
        sb.Append(String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetChosenClientLocation_{0})", this.ClientID));
        sb.Append("});");
        sb.Append(String.Format("function SetChosenClientLocation_{0}(sender, args) {{", this.ClientID));
        sb.Append(String.Format("$('#{0}').chosen({{", lbClientLocation.ClientID));
        sb.Append("allow_single_deselect: true,");
        sb.Append("placeholder_text_multiple: \"Select Locations ...\",");
        sb.Append("width: \"100%\"");
        sb.Append("});");
        sb.Append("}");
        sb.Append("</script>");
        litJs.Text = sb.ToString();
    }

    #endregion
}