Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Web.CrystalReportViewer
Imports CrystalDecisions.Shared
Partial Class Invoiz
    Inherits System.Web.UI.Page
    Dim GstrDbKey As String = "GoldenGVR2"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Request.QueryString("id") <> Nothing) Then
            Dim li_id As Integer = Request.QueryString("id")
            Dim ls_euid As String = Request.QueryString("euid")
            Dim li_companyid As Integer
            Dim ld_grand As Double
            Dim ls_Format As String

            li_companyid = getCompanyid(ls_euid)
            ld_grand = SNLVal(GetQueryValue("select grand from appsalesmaster where id=" & li_id, GstrDbKey))
            ls_Format = SNL(GetQueryValue("select invFormat from autosettings where companyid=" & li_companyid, GstrDbKey))

            Dim crystalReport As New ReportDocument()
            If ls_Format = "F1" Then
                crystalReport.Load(Server.MapPath("~/rpt_Retail_Bill_Format1.rpt"))
            Else
                crystalReport.Load(Server.MapPath("~/rpt_Retail_Bill_" & SNL(ls_Format) & ".rpt"))
            End If

            Dim dsCustomers As DataTable = GetData("exec SpRptGvrInvoice " & li_companyid & "," & li_id)
            crystalReport.SetDataSource(dsCustomers)
            CRAddParameter(crystalReport, ToWords("Rs", Val(ld_grand), True, , True), "ToWords")

            CrystalReportViewer1.GroupTreeStyle.Dispose()
            'CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf
            'CrystalDecisions.Web.PrintMode.Pdf = CrystalDecisions.Web.PrintMode.ActiveX
            'crystalReport.PrintToPrinter(1, True, 0, 0)
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
            CrystalReportViewer1.ReportSource = crystalReport
        End If
    End Sub

    Private Sub CRAddParameter(ByVal crReportDocument As ReportDocument, ByVal ls_ParaValue As String, ByVal ls_ParaField As String)
        Dim pvCollection As New ParameterValues
        Dim pdv As New ParameterDiscreteValue
        Try

            pdv.Value = ls_ParaValue
            pvCollection.Add(pdv)
            crReportDocument.DataDefinition.ParameterFields(ls_ParaField).ApplyCurrentValues(pvCollection)

        Catch ex As Exception
            'InsertLogFile(ex.Message, "ModGeneral", "CRAddParameter", "E")
        End Try
    End Sub

    Private Function GetData(ByVal query As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings(GstrDbKey).ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con

                sda.SelectCommand = cmd
                Using dsCustomers As New DataSet()
                    sda.Fill(dsCustomers, "DataTable1")
                    Return dsCustomers.Tables(0)
                End Using
            End Using
        End Using
    End Function

    Private Function getCompanyid(ByVal ls_euid As String) As Integer
        getCompanyid = SNLVal(GetQueryValue("select companyid from employee where euid='" & ls_euid & "'", GstrDbKey))
    End Function
    Public Function SNL(ByVal vExpr As Object) As String
        If IsDBNull(vExpr) Then
            SNL = ""
        Else
            vExpr = Replace(vExpr, "'", "`")
            SNL = Trim(vExpr & "")
        End If
    End Function

    Public Function GetQueryValue(ByVal strSQL As String, ByVal strConnectionStringKey As String) As String
        Dim conGlobal As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings(strConnectionStringKey).ConnectionString)
        Dim dt As New DataTable()
        Dim a(0) As String
        Try
            If conGlobal.State = ConnectionState.Closed Then conGlobal.Open()
            Dim cmd As SqlCommand = New SqlCommand(strSQL, conGlobal)
            Dim DA As SqlDataReader = cmd.ExecuteReader
            While DA.Read()
                GetQueryValue = DA.GetValue(0).ToString
            End While

        Catch ex As Exception
            GetQueryValue = ex.Message.Trim
            conGlobal.Close()
        Finally
            If conGlobal.State = ConnectionState.Open Then conGlobal.Close()
        End Try
    End Function

    Public Function SNLVal(ByVal vExpr As Object) As Double
        If IsDBNull(vExpr) Then
            SNLVal = 0
        Else

            SNLVal = Val(SNL(vExpr))
        End If
    End Function


    Public Function ToWords(ByVal CurCode As String, ByVal Number As Double, ByVal RupeesLeft As Boolean, Optional ByVal NoRupees As Boolean = False, Optional ByVal WantOnly As Boolean = False, Optional ByVal DcimalPlace As Integer = 2) As String
        '''*** Function To Change A Number To String ***''' 
        '''*** Functions Used ***'''
        '''*** ToWordsChange           - Change Into Words From 1 To 19 ***'''
        '''*** ToWordsChangemor        - Change Into Words From 20,30,.. To 90 ***'''
        '''*** ToWordsNum              - Call The Functions ToWordsNumSub ***'''
        '''*** ToWordsNumSub           - Call The Functions ToWordsChange And ToWordsChangemor ***'''
        Dim X As Double
        Dim pscheck As Boolean
        Dim Num As Long
        Dim Remain As Long
        Dim InWords As String
        Dim CurName As String
        Dim PaiseName As String
        InWords = ""

        Num = Int(Number)
        If CurCode = "Rs" Then
            CurName = "Indian Rupees"
            PaiseName = "Piasa"
        ElseIf CurCode = "RO" Then
            CurName = "Rial Omani"
            PaiseName = "Biasa"
        End If

        If Num > 0 Then
            If NoRupees = False Then
                If RupeesLeft = False Then
                    InWords = InWords & ToWordsNum(Num) & CurName & " "
                    pscheck = True
                Else
                    InWords = CurName & " " + InWords & ToWordsNum(Num)
                    pscheck = True
                End If
            Else
                InWords = InWords & ToWordsNum(Num)
                pscheck = True
            End If
        End If

        If DcimalPlace = 2 Then
            Remain = Math.Round(Number - Int(Number), DcimalPlace) * 100
        ElseIf DcimalPlace = 3 Then
            Remain = Math.Round(Number - Int(Number), DcimalPlace) * 1000
        ElseIf DcimalPlace = 4 Then
            Remain = Math.Round(Number - Int(Number), DcimalPlace) * 10000
        End If
        If Remain > 0 Then
            If pscheck = True Then
                InWords = InWords & "And " & ToWordsNum(Remain) & PaiseName & " "
            Else
                InWords = InWords & ToWordsNum(Remain) & PaiseName & " "
            End If
        End If

        If WantOnly = True Then
            InWords = InWords & " Only"
        End If
        ToWords = InWords
    End Function



    Public Function ToWordsNum(ByVal X As Long) As String
        '''*** Call The Functions ToWordsNumSub To Find Number Of ***'''
        '''*** Hundreds, Thousands, Lakhs, Crores... ***'''
        Dim b As Integer
        Dim a(10), res, TempResNew As String
        Dim Check As Boolean
        b = 0
        res = ""
        TempResNew = ""
        Check = False
        a(1) = "Hundred "
        a(2) = "Thousand "
        a(3) = "Lakh "
        a(4) = "Crore "
        While Int(X) > 0
            If Int(X / 10000000) > 0 Then
                b = Int(X / 10000000)
                res = res & ToWordsNumSub(b) & a(4)
                X = X Mod 10000000
                Check = True
            ElseIf Int(X / 100000) > 0 Then
                b = Int(X / 100000)
                res = res & ToWordsNumSub(b) & a(3)
                X = X Mod 100000
                Check = True
            ElseIf Int(X / 1000) > 0 Then
                b = Int(X / 1000)
                res = res & ToWordsNumSub(b) & a(2)
                X = X Mod 1000
                Check = True
            ElseIf Int(X / 100) > 0 Then
                b = Int(X / 100)
                TempResNew = ToWordsChange(b)
                res = res & TempResNew & a(1)
                X = X Mod 100
                Check = True
            ElseIf Int(X) > 0 Then
                If Check = True Then
                    res = res & "And "
                End If
                b = Int(X)
                res = res & ToWordsNumSub(b)
                X = 0
            End If
        End While
        ToWordsNum = res
    End Function
    Private Function ToWordsNumSub(ByVal SNum As Integer) As String
        '''*** Call The Functions ToWordsChange And ToWordsChangemor ***'''
        '''*** To Change Into String ***'''
        Dim SubRes As String
        Dim TempRes As String
        Dim C As Integer
        If SNum >= 20 Then
            C = SNum - (SNum Mod 10)
            TempRes = ToWordsChangemor(C)
            SubRes = SubRes & TempRes
        Else
            TempRes = ToWordsChange(SNum)
            SubRes = SubRes & TempRes
        End If
        If (SNum > 20 And (SNum Mod 10 > 0)) Then
            C = SNum Mod 10
            TempRes = ToWordsChange(C)
            SubRes = SubRes & TempRes
        End If
        ToWordsNumSub = SubRes
    End Function
    Private Function ToWordsChange(ByVal Cno As Integer) As String
        '''*** Change Into Words From 1 To 19 ***'''
        Dim ret As String
        Select Case Cno
            Case 1
                ret = "One "
            Case 2
                ret = "Two "
            Case 3
                ret = "Three "
            Case 4
                ret = "Four "
            Case 5
                ret = "Five "
            Case 6
                ret = "Six "
            Case 7
                ret = "Seven "
            Case 8
                ret = "Eight "
            Case 9
                ret = "Nine "
            Case 10
                ret = "Ten "
            Case 11
                ret = "Eleven "
            Case 12
                ret = "Twelve "
            Case 13
                ret = "Thirteen "
            Case 14
                ret = "Fourteen "
            Case 15
                ret = "Fifteen "
            Case 16
                ret = "Sixteen "
            Case 17
                ret = "Seventeen "
            Case 18
                ret = "Eighteen "
            Case 19
                ret = "Ninteen "
        End Select
        ToWordsChange = ret
    End Function
    Private Function ToWordsChangemor(ByVal Cno As Integer) As String
        '''*** Change Into Words From 20,30,.. To 90 ***'''
        Dim ret As String
        Select Case Cno
            Case 20
                ret = "Twenty "
            Case 30
                ret = "Thirty "
            Case 40
                ret = "Fourty "
            Case 50
                ret = "Fifty "
            Case 60
                ret = "Sixty "
            Case 70
                ret = "Seventy "
            Case 80
                ret = "Eighty "
            Case 90
                ret = "Ninty "
        End Select
        ToWordsChangemor = ret
    End Function
End Class
