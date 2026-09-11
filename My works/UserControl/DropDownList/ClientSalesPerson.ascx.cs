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
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_DropDownList_ClientSalesPerson : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    

    public System.Collections.Generic.IList<Telerik.Web.UI.RadListBoxItem> SelectedSalesPerson
    {
        get { return rlbClientSalesPerson.CheckedItems; }
        //set { rlbClientSalesPerson.CheckedItems = value; }

    }
}