<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="_Default" %>

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
        Smart Trader
    </h2>
                   
    <table cellpadding="1" cellspacing="1" border="0" width="100%">
       <tr>
            <td width="20%" colspan="1"; style="vertical-align:top">
                <table cellpadding="1" cellspacing="1" border="0" width="100%">
                    <tr>
                        <td style="vertical-align:top"><h4>Date From</h4><asp:TextBox ID="txtdtFrm" runat="server" ClientIdMode="static" /><img src="calender.png" /></td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top"><h4>Date To</h4><asp:TextBox ID="txtdtTo" runat="server" ClientIdMode="static" /><img src="calender.png" /></td>
                    </tr>
                    <tr>
                    <td><asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" /></td>
                    </tr>
                    <tr>
                        <td >
                        <div style="font-family:Arial">
                            <asp:TreeView
                                id="TreeView1"                                                               
                                NodeStyle-CssClass="ChildNodeStyle"
                                HoverNodeStyle-CssClass="TreeViewHoverStyle"
                                SelectedNodeStyle-BackColor="Red"
                                Runat="server">
                                <Nodes>
                                <asp:TreeNode
                                    Text="Home">
                                    <asp:TreeNode Text="Products">
                                        <asp:TreeNode Text="First Product" />
                                        <asp:TreeNode Text="Second Product" />
                                    </asp:TreeNode>
                                    <asp:TreeNode Text="Services">
                                        <asp:TreeNode Text="First Service" />
                                        <asp:TreeNode Text="Second Service" />
                                    </asp:TreeNode>    
                                </asp:TreeNode>    
                                </Nodes>
                            </asp:TreeView>          
                        </div>
                    </td>
                </tr>
                   
                </table>
                </td>
                 <td width="80%" colspan="1" >
                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" />
                </td>                 
             </tr>
        </table>                  
    </asp:Content>