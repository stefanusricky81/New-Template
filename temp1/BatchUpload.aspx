<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BatchUpload.aspx.cs" Inherits="cs_forest.BatchUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script>
          function isNumber(evt) {
              evt = (evt) ? evt : window.event;
              var charCode = (evt.which) ? evt.which : evt.keyCode;
              if (charCode > 31 && (charCode < 46 || charCode > 57))
                  return false;
              else
                  return true;
          }

          function goBack() {
              window.history.back();
          }

    </script>
    <style>
        .Absolute-Center {
            margin: auto;
            position: absolute;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
