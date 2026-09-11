using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class UserControl_Grid_EditForm_TicketFileEdit : AbstractTicketFileEdit
{
    protected void Page_Load(object sender, EventArgs e)
    {
        litAllowedFileExtensions.Text = string.Join(",", DesktopShared.Utility.ValidFileUploadExtensions().ToArray());
    }

    #region public methods

    #region overrride

    /// <summary>
    /// save values
    /// </summary>
    /// <param name="ticketId"></param>
    public override void SaveValues(int ticketId)
    {
        DesktopShared.EntityClasses.CscDefectsEntity _ticket =
            new DesktopShared.EntityClasses.CscDefectsEntity(ticketId);

        SaveFile(fuOne, _ticket);
        SaveFile(fuTwo, _ticket);
        SaveFile(fuThree, _ticket);
        SaveFile(fuFour, _ticket);
    }

    #endregion

    #endregion

    #region private methods

    /// <summary>
    /// save file and add history ote
    /// </summary>
    /// <param name="fileUpload"></param>
    /// <param name="ticket"></param>
    private void SaveFile(FileUpload fileUpload, DesktopShared.EntityClasses.CscDefectsEntity ticket)
    {
        if (fileUpload.PostedFile != null && fileUpload.PostedFile.ContentLength > 0)
        {
            DateTime _dtNow = DateTime.Now;

            string _postedFileName = fileUpload.FileName.Trim();
            int _nPos = _postedFileName.LastIndexOf(".");

            #region create file name

            string _fileName = _postedFileName.Substring(0, _nPos);
            _fileName += "_";
            _fileName += _dtNow.Year.ToString();
            _fileName += _dtNow.Month.ToString().PadLeft(2, '0');
            _fileName += _dtNow.Day.ToString().PadLeft(2, '0');
            _fileName += "_";
            _fileName += _dtNow.Hour.ToString().PadLeft(2, '0');
            _fileName += _dtNow.Minute.ToString().PadLeft(2, '0');
            _fileName += _dtNow.Second.ToString().PadLeft(2, '0');
            _fileName += _postedFileName.Substring(_nPos);

            _fileName = _fileName.Replace(" ", "_");

            #endregion

            #region create new CscdefectfileEntity

            DesktopShared.EntityClasses.CscdefectfileEntity _file =
                new DesktopShared.EntityClasses.CscdefectfileEntity();

            _file.Name = _fileName;
            _file.FkCscdefects = ticket.Pcscdefects;
            _file.UploadedBy = DesktopShared.User.UserID.ToString();
            _file.FkProject = ticket.CscProjects.FkProject.Value;
            _file.FkCscprojects = ticket.FkCscprojects.Value;
            _file.FkClient = ticket.FkClient.Value;

            _file.Save();

            #endregion

            #region save file

            string uploadPath = BitByBit.Configuration.GetConfigString("TicketUploadPath");
            uploadPath += ticket.FkClient.ToString() + "\\";

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            fileUpload.SaveAs(uploadPath + _fileName);

            #endregion

            #region create history note

            string _notes = String.Format("{0}{1}", DesktopShared.SiteHelper.Message.FileUploaded, _fileName);

            DesktopShared.Ticket.History.Add(
                ticket.Pcscdefects, //ticket id
                DesktopShared.User.UserID, //user id, 
                _notes.Trim(), //notes
                "", //internal notes
                false, // is new ticket
                "", // client email addresses
                "", // employee email addresses
                false, // send email out
                DesktopShared.Ticket.History.Type.Id.FileUpload, //history type
                DateTime.Now //date/time stamp to user for history note created
            );

            #endregion
        }

    }

    #endregion

    #region protected events

    #region 

    /// <summary>
    /// validate file upload
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvFu_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;

        var _cv = (CustomValidator)source;
        var _id = _cv.ControlToValidate;
        var _fu = _cv.NamingContainer.FindControl(_id) as FileUpload;

        if (!_fu.HasFile)
            return;

        List<string> _allowedExtensions = DesktopShared.Utility.ValidFileUploadExtensions();

        _cv.ErrorMessage = String.Format("Invalid File Upload. Allowed extensions: {0}", string.Join(",", _allowedExtensions.ToArray()));
        args.IsValid = _allowedExtensions.Contains(System.IO.Path.GetExtension(_fu.PostedFile.FileName).Replace(".", "").ToLower());
    }

    #endregion

    #endregion
}
