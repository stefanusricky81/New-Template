using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_ListBox_TicketDesignation : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region public properties

    /// <summary>
    /// get/set selected designation values
    /// use comma as seperator
    /// </summary>
    public string Designation
    {
        get
        {
            string returnValue = "";
            foreach (ListItem li in lbTicketDesignation.Items)
            {
                if (li.Selected)
                {
                    if (returnValue.Trim().Length > 0)
                        returnValue += ",";

                    returnValue += li.Value.Trim();
                }
            }
            return returnValue;
        
        }
        set
        {
            lbTicketDesignation.ClearSelection();

            string[] allValues = value.Trim().Split(',');
            foreach (string currentValue in allValues)
            {
                ListItem li = lbTicketDesignation.Items.FindByValue(currentValue);
                if (li != null)
                    li.Selected = true;
            }
        }
    }

    /// <summary>
    /// set listbox width
    /// </summary>
    public Unit Width
    {
        set { lbTicketDesignation.Width = value; }
    }

    /// <summary>
    /// get listbox contrl
    /// </summary>
    /// <returns></returns>
    public ListBox GetListBox()
    {
        return lbTicketDesignation;
    }

    /// <summary>
    /// set validation group for required field validator
    /// </summary>
    public string ValidationGroup
    {
        set { rfvDesignation.ValidationGroup = value; }
    }

    /// <summary>
    /// set selection is required
    /// </summary>
    public bool IsRequired
    {
        set { rfvDesignation.Visible = value; }
    }

    /// <summary>
    /// set css class for listbox control
    /// </summary>
    public string CssClass
    {
        set { lbTicketDesignation.CssClass = value; }
    }

    #endregion
}
