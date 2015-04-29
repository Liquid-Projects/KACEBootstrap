Imports DotNet.Highcharts
Imports DotNet.Highcharts.Options
Imports System.Data
Imports System.Data.SqlClient
Imports DotNet.Highcharts.Enums
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Linq
Imports DotNet.Highcharts.Helpers
Imports System.Data.OleDb
Public Class DashboardCatagories
    Inherits System.Web.UI.Page

    Dim ChartSQL As String = "SELECT Chart.Id, Chart.Title, Chart.SubTitle, Chart.Colors, Chart.Height, Chart.Width, Chart.Margin_Top, Chart.Margin_Bottom, Chart.Margin_Left, Chart.Margin_Right, Chart.Chart_Type, Chart_Legend.Legend_Layout, Chart_Legend.Legend_Align, Chart_Legend.Legend_VerticalAligns, Chart_Legend.Legend_X, Chart_Legend.Legend_Y, Chart_Legend.Legend_BorderWidth FROM Chart LEFT OUTER JOIN Chart_Legend ON Chart.Id = Chart_Legend.Chart_ID WHERE (Chart.Id = @ChartID)"
    Dim ChartSQLData As String = "Select Chart.Id, Chart_Data_Series.Chart_Data_Set, Chart_Data_Series.KACE_QUEUE, Chart_Data_Series.Chart_Data_Name FROM Chart INNER JOIN Chart_Data_Series ON Chart.Id = Chart_Data_Series.Chart_id WHERE (Chart.Id = @ChartID)"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call ChartTicketsOpenClosedLast12Months()
        Dim DataSet1 As New DataTable

        DataSet1.Columns.Add("Catagory")
        DataSet1.Columns.Add("Currently Open")
        DataSet1.Columns.Add("Opened Last 30 Days")
        DataSet1.Columns.Add("Closed Last 30 Days")
        DataSet1.Columns.Add("30 Day Avg Closure Time")
        DataSet1.Columns.Add("Opened Last 12 Months")
        DataSet1.Columns.Add("Closed Last 12 Months")
        DataSet1.Columns.Add("12 Month Avg Closure Time")

        SqlDataSourceKACE.SelectParameters.Clear()
        SqlDataSourceKACE.SelectParameters.Add("QUEUE_ID", 4)
        SqlDataSourceKACE.SelectCommand = "SELECT COUNT(HD_TICKET.ID) AS TOTAL,HD_TICKET.HD_CATEGORY_ID AS CATID,HD_CATEGORY.NAME AS CATNAME, SUM(IF(HD_STATUS.STATE NOT LIKE '%CLOSED%',1,0)) AS CURRENTLY_OPEN, SUM(IF(HD_TICKET.CREATED >= DATE_SUB(CURDATE(), INTERVAL 30 DAY),1,0)) AS OPENEDLAST30, SUM(IF(HD_TICKET.TIME_CLOSED >= DATE_SUB(CURDATE(), INTERVAL 30 DAY),1,0)) AS CLOSEDLAST30, SUM(IF(HD_TICKET.TIME_CLOSED >= DATE_SUB(CURDATE(), INTERVAL 30 DAY), TIMESTAMPDIFF(SECOND,HD_TICKET.CREATED,HD_TICKET.TIME_CLOSED),0))/ SUM(IF(HD_TICKET.TIME_CLOSED >= DATE_SUB(CURDATE(), INTERVAL 30 DAY),1,0)) AS AVG30_S, SUM(IF(HD_TICKET.CREATED >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH),1,0)) AS OPENEDLAST12, SUM(IF(HD_TICKET.TIME_CLOSED >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH),1,0)) AS CLOSEDLAST12, SUM(IF(HD_TICKET.TIME_CLOSED >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH), TIMESTAMPDIFF(SECOND,HD_TICKET.CREATED,HD_TICKET.TIME_CLOSED),0))/ SUM(IF(HD_TICKET.TIME_CLOSED >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH),1,0)) AS AVG12M_S FROM HD_TICKET JOIN HD_STATUS ON (HD_STATUS.ID = HD_TICKET.HD_STATUS_ID) LEFT JOIN HD_CATEGORY ON (HD_TICKET.HD_CATEGORY_ID=HD_CATEGORY.ID) LEFT JOIN USER O ON (O.ID = HD_TICKET.OWNER_ID) WHERE (HD_STATUS.NAME NOT LIKE '%SERVER STATUS REPORT%') AND (HD_STATUS.NAME NOT LIKE '%SPAM%') AND (HD_TICKET.HD_QUEUE_ID = @QUEUE_ID) AND (((HD_STATUS.STATE NOT LIKE '%CLOSED%') AND HD_TICKET.CREATED >= DATE_SUB(DATE_ADD(LAST_DAY(CURDATE()), INTERVAL 1 DAY), INTERVAL 12 MONTH)) OR (HD_TICKET.TIME_CLOSED >= DATE_SUB(DATE_ADD(LAST_DAY(CURDATE()), INTERVAL 1 DAY), INTERVAL 12 MONTH))) GROUP BY HD_TICKET.HD_CATEGORY_ID ORDER BY CATNAME, SUM(IF(HD_STATUS.STATE NOT LIKE '%CLOSED%',1,0)) DESC;"
        Dim drvSqlKACE As DataView = SqlDataSourceKACE.Select(DataSourceSelectArguments.Empty)
        For Each drvSql As DataRowView In drvSqlKACE
            Dim DataRow As System.Data.DataRow = DataSet1.NewRow
            DataRow.Item(0) = drvSql("CATNAME")
            DataRow.Item(1) = drvSql("CURRENTLY_OPEN")
            DataRow.Item(2) = drvSql("OPENEDLAST30")
            DataRow.Item(3) = drvSql("CLOSEDLAST30")
            DataRow.Item(4) = drvSql("AVG30_S")
            DataRow.Item(5) = drvSql("OPENEDLAST12")
            DataRow.Item(6) = drvSql("CLOSEDLAST12")
            DataRow.Item(7) = drvSql("AVG12M_S")
            DataSet1.Rows.Add(DataRow)
        Next

        GridView1.DataSource = DataSet1
        GridView1.DataBind()
    End Sub

    Sub ChartTicketsOpenClosedLast12Months()
        SqlDataSourceChart.SelectParameters.Clear()
        SqlDataSourceChart.SelectParameters.Add("ChartID", 5)
        SqlDataSourceChart.SelectCommand = ChartSQL
        Dim dvSqlChart As DataView = SqlDataSourceChart.Select(DataSourceSelectArguments.Empty)

        Dim Chart = New DotNet.Highcharts.Highcharts("chart")
        Dim InitChart As New Chart
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Top")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Top").ToString) Then InitChart.MarginTop = Integer.Parse(dvSqlChart(0).Item("Margin_Top").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Left")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Left").ToString) Then InitChart.MarginLeft = Integer.Parse(dvSqlChart(0).Item("Margin_Left").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Right")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Right").ToString) Then InitChart.MarginRight = Integer.Parse(dvSqlChart(0).Item("Margin_Right").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Bottom")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Bottom").ToString) Then InitChart.MarginBottom = Integer.Parse(dvSqlChart(0).Item("Margin_Bottom").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Height")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Height").ToString) Then InitChart.Height = Integer.Parse(dvSqlChart(0).Item("Height").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Width")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Width").ToString) Then InitChart.Width = Integer.Parse(dvSqlChart(0).Item("Width").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Chart_Type")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Chart_Type").ToString) Then InitChart.Type = dvSqlChart(0).Item("Chart_Type")
        Chart.InitChart(InitChart)

        Dim ChartPlotOptions As New PlotOptions

        If Not IsDBNull(dvSqlChart(0).Item("Colors")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Colors")) Then
            Dim palette_colors_list As New List(Of System.Drawing.Color)
            For Each Color As String In dvSqlChart(0).Item("Colors").ToString.Split(",").ToArray
                palette_colors_list.Add(System.Drawing.ColorTranslator.FromHtml(Color))
            Next
            Chart.SetColors(New Options.Colors With {.ChartColors = palette_colors_list.ToArray})
        End If

        Chart.SetTitle(New Title With {.Text = dvSqlChart(0).Item("Title").ToString(), .X = 20})

        Dim chartLegend As New Legend
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Layout")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Layout").ToString) Then chartLegend.Layout = Integer.Parse(dvSqlChart(0).Item("Legend_Layout").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Align")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Align").ToString) Then chartLegend.Align = Integer.Parse(dvSqlChart(0).Item("Legend_Align").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_VerticalAligns")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_VerticalAligns").ToString) Then chartLegend.VerticalAlign = Integer.Parse(dvSqlChart(0).Item("Legend_VerticalAligns").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_X")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_X").ToString) Then chartLegend.X = Integer.Parse(dvSqlChart(0).Item("Legend_X").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Y")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Y").ToString) Then chartLegend.Y = Integer.Parse(dvSqlChart(0).Item("Legend_Y").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_BorderWidth")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_BorderWidth").ToString) Then chartLegend.BorderWidth = Integer.Parse(dvSqlChart(0).Item("Legend_BorderWidth").ToString)
        Chart.SetLegend(chartLegend)

        Chart.SetXAxis(New XAxis With {.Type = AxisTypes.Category})
        Chart.SetYAxis(New YAxis With {.Min = 0, .Title = New YAxisTitle With {.Text = "Tickets"}})
        Chart.SetSubtitle(New Subtitle With {.Text = dvSqlChart(0).Item("SubTitle").ToString(), .X = -20})


        Dim series As New List(Of Series)()
        Dim sqlChartData As New SqlDataSource
        sqlChartData.SelectParameters.Add("ChartID", 2)
        sqlChartData.SelectCommand = ChartSQLData
        sqlChartData.ConnectionString = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim dvSqlChartData As DataView = sqlChartData.Select(DataSourceSelectArguments.Empty)

        For Each drvSqlChatData As DataRowView In dvSqlChartData
            SqlDataSourceKACE.SelectParameters.Clear()
            SqlDataSourceKACE.SelectParameters.Add("QUEUE_ID", drvSqlChatData("KACE_QUEUE"))
            SqlDataSourceKACE.SelectCommand = drvSqlChatData("Chart_Data_Set")

            Dim drvSqlKACE As DataView = SqlDataSourceKACE.Select(DataSourceSelectArguments.Empty)
            Dim sqlList As New List(Of Array)()
            For Each drvSql As DataRowView In drvSqlKACE
                sqlList.Add({drvSql("month").ToString + "-" + drvSql("year").ToString, Integer.Parse(drvSql("total").ToString)})
            Next
            series.Add(New Series With {.Name = drvSqlChatData("Chart_Data_Name"), .Data = New Data(sqlList.ToArray)})
        Next

        Chart.SetSeries(series.ToArray)
        litchart.Text = Chart.ToHtmlString
        'Dim litchart As New Literal With {.Text = Chart.ToHtmlString}
        'Dim mpContentPlaceHolder As ContentPlaceHolder
        ' mpContentPlaceHolder = CType(Master.FindControl("MainContent"), ContentPlaceHolder)
        'mpContentPlaceHolder.Controls.Add(litchart)
    End Sub

End Class