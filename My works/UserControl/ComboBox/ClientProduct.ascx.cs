using System;
using System.Collections;
using System.Collections.Generic;
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
using SD.LLBLGen.Pro.ORMSupportClasses;

public partial class UserControl_ComboBox_ClientProduct : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
            PopulateProducts();
    }
    
    #region public methods

    /// <summary>
    /// clear all items from combo box
    /// </summary>
    public void ClearItems()
    {
        _rcbProduct.ClearSelection();
        _rcbProduct.Items.Clear();
        PopulateProducts();
    }

    public void PopulateProducts()
    {
        DesktopShared.CollectionClasses.ProductsCollection products = DesktopShared.Product.Legacy.GetProducts();

        #region bind drop down list

        _rcbProduct.DataSource = products;
        _rcbProduct.Filter = RadComboBoxFilter.Contains;
        _rcbProduct.DataTextField = "Product";
        _rcbProduct.DataValueField = "ProductId";
        _rcbProduct.DataBind();

        #endregion
    }

    #endregion

    #region public properties

    /// <summary>
    /// set/set combo control
    /// </summary>
    public RadComboBox rcbProduct
    {
        get
        {
            return _rcbProduct;
        }
    }
    
    #endregion

}