<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketEmployee.aspx.cs" Inherits="JimReport" %>

<%@ Register assembly="DundasWebChart" namespace="Dundas.Charting.WebControl" tagprefix="dcwc" %>

<HTML>
    <body>
        <dcwc:CHART id="Chart1" Width="1024px" Height="700px" runat="server"  ImageUrl="..\..\TempImages\ChartPic_#SEQ(300,3)" Palette="Pastel">
            <ChartAreas>
                <dcwc:ChartArea Name="Default"></dcwc:ChartArea>
            </ChartAreas>
        </dcwc:CHART>
    </body>
</HTML>

