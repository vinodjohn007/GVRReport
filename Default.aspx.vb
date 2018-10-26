Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Web.CrystalReportViewer

Partial Class _Default
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim crystalReport As New ReportDocument()
        crystalReport.Load(Server.MapPath("~/CustomerReport.rpt"))
        Dim dsCustomers As customers = GetData("select top 1000 id,cusname as name,cusadd as address,mob as place  from autocustomer")
        crystalReport.SetDataSource(dsCustomers)
        CrystalReportViewer1.GroupTreeStyle.Dispose()
        CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf
        'CrystalDecisions.Web.PrintMode.Pdf = CrystalDecisions.Web.PrintMode.ActiveX
        'crystalReport.PrintToPrinter(1, True, 0, 0)
        CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
        CrystalReportViewer1.ReportSource = crystalReport
    End Sub

    Private Function GetData(ByVal query As String) As Customers
        Dim conString As String = ConfigurationManager.ConnectionStrings("GoldenGVR2").ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con

                sda.SelectCommand = cmd
                Using dsCustomers As New Customers()
                    sda.Fill(dsCustomers, "DataTable1")
                    Return dsCustomers
                End Using
            End Using
        End Using
    End Function

    Protected Sub btnShow_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Dim dob As DateTime = DateTime.Parse(Request.Form(txtdtFrm.UniqueID))
        'Response.Write("<script type=""text/javascript"">alert(" & dob & ");</script")
        Dim crystalReport As New ReportDocument()
        crystalReport.Load(Server.MapPath("~/CustomerReport.rpt"))
        Dim dsCustomers As customers = GetData("select top 1000 id,cusname as name,cusadd as address,mob as place  from autocustomer")
        crystalReport.SetDataSource(dsCustomers)
        CrystalReportViewer1.GroupTreeStyle.Dispose()
        CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf
        'CrystalDecisions.Web.PrintMode.Pdf = CrystalDecisions.Web.PrintMode.ActiveX
        'crystalReport.PrintToPrinter(1, True, 0, 0)
        CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
        CrystalReportViewer1.ReportSource = crystalReport
    End Sub
End Class
