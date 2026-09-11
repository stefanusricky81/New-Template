using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControl_Grid_EditForm_ProductEdit : AbstractProductEdit
{
    private EditMode _editMode = EditMode.Grid;

    /// <summary>
    /// page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        SetUpControl();
    }

    #region private methods

    /// <summary>
    /// set up control for display
    /// </summary>
    private void SetUpControl()
    {
        if (_editMode == EditMode.Grid)
        {
            mvButton.SetActiveView(viewGrid);
            mvButtonTop.SetActiveView(viewGridTop);
        }
        else
        {
            mvButton.SetActiveView(viewPage);
            mvButtonTop.SetActiveView(viewPageTop);
            divMain.Style.Add("border", "1px solid #3b5a82");
        }
        chkActive.InputAttributes.Add("class", "checkbox");
    }

    #endregion

    #region protected  events

    #region custom validators

    /// <summary>
    /// validate product name is unique
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = DesktopShared.Product.IsUniqueName(tbName.Text.Trim(), ProductId);
    }

    #endregion
    /// <summary>
    /// update page button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdatePage_Click(object sender, EventArgs e)
    {
        evButtonClick(this, e);
    }

    #endregion

    #region public override methods

    /// <summary>
    /// load prodcut by id
    /// </summary>
    /// <param name="productID"></param>
    public override void LoadValues(int productID)
    {
        if (productID > 0)
        {
            DesktopShared.TypedListClasses.ProductRow objProduct = DesktopShared.Product.GetTypedListRow(productID);
            LoadValues(objProduct);
        }
        else
            LoadValues(null);
    }

    /// <summary>
    /// load product by typed list row
    /// </summary>
    /// <param name="objProduct"></param>
    public override void LoadValues(DesktopShared.TypedListClasses.ProductRow objProduct)
    {
        btnInsert.Visible = (objProduct == null);
        btnInsertTop.Visible = (objProduct == null);

        btnUpdate.Visible = (objProduct != null);
        btnUpdateTop.Visible = (objProduct != null);

        phExistingFields.Visible = (objProduct != null);        
        
        #region existing record

        if (objProduct != null)
        {
            ProductId = objProduct.Id;
            //hlProductId.Text = objProduct.Id.ToString();
            //hlProductId.NavigateUrl = String.Format("/Product/Detail.aspx?Id={0}", objProduct.Id);
            phProductId.Visible = false;
            Name = objProduct.Name.Trim();
            Description = objProduct.Description.Trim();
            Active = objProduct.Active;
            Created = String.Format("{0}{1}", objProduct.Created, (objProduct.IsCreatedEmployeeIdNull() ? "" : String.Format(" - {0} {1}", objProduct.CreatedEmployeeFirst.Trim(), objProduct.CreatedEmployeeLast.Trim())));
            LastUpdated = String.Format("{0}{1}", objProduct.LastUpdated, (objProduct.IsLastUpdatedEmployeeIdNull() ? "" : String.Format(" - {0} {1}", objProduct.LastUpdatedEmployeeFirst.Trim(), objProduct.LastUpdatedEmployeeLast.Trim())));

            if (_editMode == EditMode.Page)
                btnUpdatePage.Text = "UPDATE";
        }

        #endregion

        #region new record

        else
        {
            phProductId.Visible = false; //hide id link
            chkActive.Checked = true;

            if (_editMode == EditMode.Page)
                btnUpdatePage.Text = "ADD";
        }

        #endregion

    }

    /// <summary>
    /// save product entity
    /// </summary>
    /// <param name="authenticationInfoId"></param>
    /// <returns></returns>
    public override int SaveValues(int? productID)
    {
        DesktopShared.EntityClasses.ProductEntity objProduct = null;
        DateTime _now = DateTime.Now;
        int _employeeId = DesktopShared.User.EmployeeID;

        if (productID.HasValue)
            objProduct = new DesktopShared.EntityClasses.ProductEntity(productID.Value);
        else 
        {
            objProduct = new DesktopShared.EntityClasses.ProductEntity();
            objProduct.Created = _now;
            objProduct.CreatedEmployeeId = _employeeId;
        }
        objProduct.Name = Name.Trim();
        objProduct.Description = Description.Trim();
        objProduct.Active = Active;
        objProduct.LastUpdated = _now;
        objProduct.LastUpdatedEmployeeId = _employeeId;
        objProduct.Save();

        return objProduct.Id;
    }

    #endregion

    #region public properties

    /// <summary>
    /// get/set name
    /// </summary>
    public string Name
    {
        get { return tbName.Text; }
        set { tbName.Text = value; }
    }

    /// <summary>
    /// get/set description
    /// </summary>
    public string Description
    {
        get { return txtDescription.Text; }
        set { txtDescription.Text = value; }
    }

    /// <summary>
    /// get/set active
    /// </summary>
    public bool Active
    {
        get { return chkActive.Checked; } 
        set { chkActive.Checked = value; }
    }

    /// <summary>
    /// set created 
    /// </summary>
    public string Created
    {
        set { litCreated.Text = value.ToString(); }
    }

    /// <summary>
    /// set last updated 
    /// </summary>
    public string LastUpdated
    {
        set { litLastUpdated.Text = value.ToString(); }
    }

    /// <summary>
    /// set control edit mode
    /// </summary>
    public EditMode FormEditMode
    {
        set { _editMode = value; }
    }

    #endregion

    #region public event

    /// <summary>
    /// event handler for parent page to use on update/add button click
    /// </summary>
    public event EventHandler evButtonClick;

    #endregion

    #region public enum

    /// <summary>
    /// enum type for edit mode
    /// </summary>
    public enum EditMode
    {
        Grid,
        Page
    }

    #endregion

    #region private properties

    /// <summary>
    /// get/set product id
    /// </summary>
    private int? ProductId
    {
        get
        {
            object obj = this.Session["pid_pe"];
            return (obj == null) ? (int?)null : (int)obj;
        }
        set { this.Session["pid_pe"] = value; }
    }

    #endregion

}
