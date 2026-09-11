<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Styles/IS_style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="Styles/nivo-slider.css" type="text/css" media="screen" />
    <link rel="stylesheet" type="text/css" href="Styles/ddsmoothmenu.css" />

    <script language="javascript" type="text/javascript">
        function clearText(field) {
            if (field.defaultValue == field.value) field.value = '';
            else if (field.value == '') field.value = field.defaultValue;
        }
    </script>

    <script type="text/javascript" src="Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="Scripts/ddsmoothmenu.js">
    </script>

    <script type="text/javascript">

        ddsmoothmenu.init({
            mainmenuid: "IS_menu", //menu DIV id
            orientation: 'h', //Horizontal or vertical menu: Set to "h" or "v"
            classname: 'ddsmoothmenu', //class added to menu's outer DIV
            //customtheme: ["#1c5a80", "#18374a"],
            contentsource: "markup" //"markup" or ["container_id", "path_to_menu_file"]
        })

    </script>

    <script type="text/javascript" src="Scripts/jquery-1-4-2.min.js"></script> 
    <link rel="stylesheet" href="Styles/slimbox2.css" type="text/css" media="screen" /> 
    <script type="text/JavaScript" src="Scripts/slimbox2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.4.3.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.nivo.slider.pack.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <div id="IS_slider">
            <div id="slider-wrapper">
                <div id="slider" class="nivoSlider" >
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                                <img src='<%# DataBinder.Eval(Container.DataItem,"Value") %>' title='<%# (DataBinder.Eval(Container.DataItem,"Text").ToString()).Split('.')[0].ToString() %>' alt="">
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div id="htmlcaption" class="nivo-html-caption">
                    <strong>This</strong> is an example of a <em>Image</em> Slider using <a href="#">Javascript</a>.
                </div>
            </div>

            <script type="text/javascript">
                $(window).load(function () {
                    $('#slider').nivoSlider();
                });
            </script>
        </div>
        <div class="home-left">
            <img src="images/1.jpg" />
            <img src="images/2.jpg" />
            <img src="images/3.jpg" />
        </div>
        <br />
    </div>
</asp:Content>
