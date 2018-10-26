<%@ Page Title="Invoiz.in" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeFile="Invoiz.aspx.vb" Inherits="Invoiz" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>



<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <style type="text/css">
        .treeNode
        {
            color:blue;
            font:14px Arial, Sans-Serif;
        }
        .rootNode
        {
            font-size:18px;
            width:100%;
            border-bottom:Solid 1px black;
        }
        .leafNode
        {
            border:Dotted 2px black;
            padding:4px;
            background-color:#eeeeee;
            font-weight:bold;
        }
    </style>

     <style type="text/css">
        .ChildNodeStyle {
            font-size: small;
            font-family: Arial;
            color: Yellow;
        }
        .TreeViewHoverStyle {
            cursor: hand;
            text-decoration: underline;
            font-style: italic;
            color: Black;
            font-size: medium;
            border-bottom-color: White;
        }
    </style>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css" />

    <script type="text/javascript">
        $(document).ready(function () {
            $(function () {
                $("#txtdtFrm").datepicker(
                {  
                dateFormat: "dd/mm/yy",
                yearRange: "c-80:c+40",
                inline: true,
                showAnim: 'fadeIn',
                changeMonth: true,
                changeYear: true,
                minDate: "-120y",
                maxDate: "+18y",
                });
                $("#txtdtTo").datepicker(
                { 
                dateFormat: "dd/mm/yy",
                yearRange: "c-80:c+40",
                inline: true,
                showAnim: 'fadeIn',
                changeMonth: true,
                changeYear: true,
                minDate: "-120y",
                maxDate: "+18y",
                });
            });
        });
    </script>


    <h2>
         
    </h2>
                   
    <table cellpadding="1" cellspacing="1" border="0" width="100%">
       <tr>             
            <td width="100%" colspan="1" >
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
                    AutoDataBind="true" ToolPanelView="None" EnableDatabaseLogonPrompt="False" 
                    HasCrystalLogo="False" PrintMode="ActiveX" Width="350px" />
            </td>                 
        </tr>
    </table>                  
</asp:Content>