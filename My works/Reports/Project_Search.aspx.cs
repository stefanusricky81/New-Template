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
using BitByBit;
using SD.LLBLGen.Pro.ORMSupportClasses;
using DesktopShared;

public partial class Reports_Project_Search : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        iFrameMaster master = Page.Master as iFrameMaster;
        master.iFrameSource = "https://legacy.desktop.bitxbit.com/reports/projecthourlyanalysisdefault.asp?rpttype=ALL";

    }
}
