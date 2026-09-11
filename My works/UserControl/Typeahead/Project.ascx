<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Project.ascx.cs" Inherits="UserControl_Typeahead_Project" %>
<input type="text" id="txtProject" runat="server" class="form-control input-typeahead" autocomplete="off" placeholder="Enter Project #, Project name, or Client name ..." maxlength="50" />
<asp:HiddenField ID="hdnProjectId" runat="server" />
<asp:HiddenField ID="hdnClientId" runat="server" />
<div style="float:left; display:inline-block">
    <asp:CustomValidator ID="cvProject" runat="server" ControlToValidate="txtProject" ValidateEmptyText="true" ErrorMessage="Project is required" Visible="false" OnServerValidate="cvProject_ServerValidate">
        <span class="error">*</span>
    </asp:CustomValidator>
</div>

<script>
    $(function () {
        setUpTypeahead();
    });
    var prmInstance = Sys.WebForms.PageRequestManager.getInstance();
    prmInstance.add_endRequest(function () {
        setUpTypeahead();
    });
    function setUpTypeahead() {
        var projects;
        $('#<%=txtProject.ClientID %>').typeahead({
            minLength: 2,
            hint: true,
            cancelButton: true,
            source: function (query, process) {
                return $.ajax({
                    type: "POST",
                    url: '/WebService/Projects.asmx/GetProjects',
                    data: JSON.stringify({ query: query, clientId: $('#<%=hdnClientId.ClientID %>').val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                        function (data) {
                            projects = data.d;
                            var projectIds = _.map(projects, function (c) {
                                return c.Id;
                            });;
                            return process(projectIds);
                            
                        },
                    error: function (response) {
                        console.log('error:' + response.responseText); 
                    },
                    failure: function (response) {
                        console.log('failure: ' + response.responseText); 
                    }
                });
            },
            matcher: function (item) {
                return true;
            },
            sorter: function (items) {
                return items;
            },
            highlighter: function (id) {
                var project = _.find(projects, function (p) {
                    return p.Id == id;
                });
                return project.Name;
            },
            updater: function (id) {
                $('#<%=hdnProjectId.ClientID %>').val(id);
                var project = _.find(projects, function (p) {
                    return p.Id == id;
                });
                return project.Name;
            }
        });
    }
</script>
