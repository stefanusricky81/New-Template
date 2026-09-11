<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Client.ascx.cs" Inherits="UserControl_Typeahead_Client" %>
<input type="text" id="txtClient" runat="server" class="form-control input-typeahead" autocomplete="off" placeholder="Enter Client name or code ..." maxlength="50" width="50" />
<div style="display:none;"><asp:TextBox ID="txtClientHidden" runat="server" Width="0"/></div>
<div style="float:left; display:inline-block">
    <asp:CustomValidator ID="cvClient" runat="server" ControlToValidate="txtClient" ValidateEmptyText="true" ErrorMessage="Client is required" Visible="false" OnServerValidate="cvClient_ServerValidate">
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
        var clients;
        $('#<%=txtClient.ClientID %>').typeahead({
            minLength: 2,
            items: 30,
            hint: true,
            cancelButton: true,
            source: function (query, process) {
                return $.ajax({
                    type: "POST",
                    url: '/WebService/Clients.asmx/GetClients3',
                    data: JSON.stringify({ query: query }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                        function (data) {
                            clients = data.d;
                            var clientIds = _.map(clients, function (c) {
                                return c.Id;
                            });;
                            return process(clientIds);
                            
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
                var client = _.find(clients, function (c) {
                    return c.Id == id;
                });
                return client.Name;
            },
            updater: function (id) {
                $('#<%=txtClientHidden.ClientID %>').val(id);
                var client = _.find(clients, function (c) {
                    return c.Id == id;
                });
                return client.Name;
            }
        });
    }
</script>
