using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;


public partial class AdminHome : System.Web.UI.Page
{
    ClassConnectionDatabase conn = new ClassConnectionDatabase();
    DataTable dt = new DataTable();
    SqlCommand smd = new SqlCommand();
    SqlDataReader sdr = null;
    int vstatus = 0;
    string pstatus = string.Empty;
    string pstatusmsg = string.Empty;
    string VCacheKey = "";

    protected void CekSession()
    {
        if (Session["USER_ID"] != null)
        {
            if (Session["USER_ID"].ToString().ToUpper() != "ADMIN")
                Response.Redirect("Home.aspx");
        }
        else
            Response.Redirect("Home.aspx");
    }

    protected void GetData()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_HOMEPICTURE", conn.getconn());
        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.HasRows)
        {
            dt.Load(sdr);
        }

        if (dt.Rows.Count > 0)
        {
            GV.DataSource = dt;
            Cache.Insert(VCacheKey, GV.DataSource, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);
        }
        GV.DataBind();

        conn.CloseConnection();
        conn.Dispose();
    }

    protected string Validation()
    {
        //if (Request.Params["type"] != "edit")
        //{
            if (fileuploadimagesFirst.HasFile == false) return "First Image Is Empty";
            if (fileuploadimagesSecond.HasFile == false) return "Second Image Is Empty";
            if (fileuploadimagesThird.HasFile == false) return "Third Image Is Empty";
            if (fileuploadimagesFourth.HasFile == false) return "Fourth Image Is Empty";
        //}

        return string.Empty;
    }

    protected void loaddata()
    {
        conn.ConnectionDatabase();

        smd = new SqlCommand("GET_MASTERHOME", conn.getconn());
        smd.Parameters.AddWithValue("@ID", Request.Params["ID"].ToString());

        smd.CommandType = CommandType.StoredProcedure;
        sdr = smd.ExecuteReader();
        if (sdr.Read())
        {
            txtName.Text = sdr["HOME_NAME"].ToString();
        }

        conn.CloseConnection();
        conn.Dispose();
    }

    protected void myGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        CekSession();
        GV.PageIndex = e.NewPageIndex;
        GV.DataSource = Cache[VCacheKey];
        GV.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CekSession();

        if (!IsPostBack)
        {
            if (Request.Params["type"] == "edit")
            {
                loaddata();
            }

            GetData();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CekSession();
        Response.Redirect("AdminHome.aspx");
    }

    protected string cekImage(string f1,string f2,string f3, string f4)
    {
        int width1 = 0; int width2 = 0; int width3 = 0; int width4 = 0;
        int height1 = 0; int height2 = 0; int height3 = 0; int height4 = 0;

        System.Drawing.Image objImage1 = System.Drawing.Image.FromFile(Server.MapPath("ImageHome/" + f1));
        width1 = objImage1.Width;
        height1 = objImage1.Height;
        if (width1 != 1024 && height1 != 768)
        {
            return "Your Picture Widht Must 1024 and Height Must be 768, Your Picture is " + width1 + " and Height Is " + height1;
        }

        System.Drawing.Image objImage2 = System.Drawing.Image.FromFile(Server.MapPath("ImageHome/" + f2));
        width2 = objImage2.Width;
        height2 = objImage2.Height;
        if (width2 != 1024 && height2 != 768)
        {
            return "Your Picture Widht Must 1024 and Height Must be 768, Your Picture is " + width2 + " and Height Is " + height2;
        }

        System.Drawing.Image objImage3 = System.Drawing.Image.FromFile(Server.MapPath("ImageHome/" + f3));
        width3 = objImage3.Width;
        height3 = objImage3.Height;
        if (width3 != 1024 && height3 != 768)
        {
            return "Your Picture Widht Must 1024 and Height Must be 768, Your Picture is " + width3 + " and Height Is " + height3;
        }

        System.Drawing.Image objImage4 = System.Drawing.Image.FromFile(Server.MapPath("ImageHome/" + f4));
        width4 = objImage4.Width;
        height4 = objImage4.Height;
        if (width4 != 1024 && height4 != 768)
        {
            return "Your Picture Widht Must 1024 and Height Must be 768, Your Picture is " + width4 + " and Height Is " + height4;
        }

        return string.Empty;
    }

    protected void DeleteImage()
    {
        int jml = dt.Rows.Count;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string CekValidation = string.Empty;
        string _image1 = string.Empty;
        string _image2 = string.Empty;
        string _image3 = string.Empty;
        string _image4 = string.Empty;

        CekSession();

        try
        {

            CekValidation = Validation();

            ClassConnectionDatabase conn = new ClassConnectionDatabase();
            conn.ConnectionDatabase();
            if (CekValidation == string.Empty)
            {
                if (GV.Rows.Count > 0)
                {
                    if (fileuploadimagesFirst.HasFile)
                    {
                        _image1 = GV.DataKeys[GV.Rows[0].RowIndex].Values[1].ToString();
                        System.IO.File.Delete(Server.MapPath("~/" + _image1));
                    }

                    if (fileuploadimagesSecond.HasFile)
                    {
                        _image2 = GV.DataKeys[GV.Rows[0].RowIndex].Values[2].ToString();
                        System.IO.File.Delete(Server.MapPath("~/" + _image2));
                    }

                    if (fileuploadimagesThird.HasFile)
                    {
                        _image3 = GV.DataKeys[GV.Rows[0].RowIndex].Values[3].ToString();
                        System.IO.File.Delete(Server.MapPath("~/" + _image3));
                    }

                    if (fileuploadimagesFourth.HasFile)
                    {
                        _image4 = GV.DataKeys[GV.Rows[0].RowIndex].Values[4].ToString();
                        System.IO.File.Delete(Server.MapPath("~/" + _image4));
                    }
                }

                string filename1 = Path.GetFileName(fileuploadimagesFirst.PostedFile.FileName);
                string filename2 = Path.GetFileName(fileuploadimagesSecond.PostedFile.FileName);
                string filename3 = Path.GetFileName(fileuploadimagesThird.PostedFile.FileName);
                string filename4 = Path.GetFileName(fileuploadimagesFourth.PostedFile.FileName);

                fileuploadimagesFirst.SaveAs(Server.MapPath("ImageHome/" + filename1));
                fileuploadimagesSecond.SaveAs(Server.MapPath("ImageHome/" + filename2));
                fileuploadimagesThird.SaveAs(Server.MapPath("ImageHome/" + filename3));
                fileuploadimagesFourth.SaveAs(Server.MapPath("ImageHome/" + filename4));

                if (cekImage(filename1, filename2, filename3, filename4) != string.Empty)
                {
                    Master.Status.Text = cekImage(filename1, filename2, filename3, filename4);
                    Master.Status.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                    Master.Status.Text = string.Empty;

                if (Request.Params["type"] == "edit")
                {
                    smd = new SqlCommand("UPDATE_HOMEIMAGE", conn.getconn());
                    smd.Parameters.AddWithValue("@Id", Request.Params["ID"]);
                }
                else
                {
                    smd = new SqlCommand("SP_INSERTHOMEIMAGE", conn.getconn());
                }
                smd.Parameters.AddWithValue("@Name", txtName.Text);
                smd.Parameters.AddWithValue("@Image1", "ImageHome/" + filename1);
                smd.Parameters.AddWithValue("@Image2", "ImageHome/" + filename2);
                smd.Parameters.AddWithValue("@Image3", "ImageHome/" + filename3);
                smd.Parameters.AddWithValue("@Image4", "ImageHome/" + filename4);

                smd.Parameters.Add("@Pstatus", SqlDbType.VarChar, 255);
                smd.Parameters["@Pstatus"].Direction = ParameterDirection.Output;
                smd.Parameters.Add("@PstatusMsg", SqlDbType.VarChar, 255);
                smd.Parameters["@PstatusMsg"].Direction = ParameterDirection.Output;
                smd.CommandType = CommandType.StoredProcedure;
                smd.ExecuteNonQuery();
                pstatus = smd.Parameters["@Pstatus"].Value.ToString();
                pstatusmsg = smd.Parameters["@PstatusMsg"].Value.ToString();
                conn.CloseConnection();
                conn.Dispose();

                vstatus = Int32.Parse(pstatus);
                if (vstatus > 0)
                {
                    Master.Status.Text = "[" + pstatus + "] " + pstatusmsg;
                    Master.Status.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    Master.Status.Text = "[" + pstatus + "] " + pstatusmsg;
                    Master.Status.ForeColor = System.Drawing.Color.Blue;

                    Response.Redirect("AdminHome.aspx");
                }
            }
            else
            {
                Master.Status.Text = CekValidation;
                Master.Status.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }
        catch
        {
            Master.Status.Text = "System Error";
            Master.Status.ForeColor = System.Drawing.Color.Red;
            return;
        }
    }
}