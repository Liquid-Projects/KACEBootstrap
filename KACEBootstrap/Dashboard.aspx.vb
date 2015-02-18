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

Public Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call SetupChartTicketsClosedByQueue()
        Call SetupChartTicketsClosedByCategory()
        Call SetupChartTicketsClosedByCategory2()
    End Sub

    Sub SetupChartTicketsClosedByCategory2()
        Dim SAG As String = "SELECT HD_CATEGORY.NAME as CatName, count(HD_TICKET.ID) as total FROM HD_TICKET JOIN HD_STATUS ON (HD_STATUS.ID = HD_TICKET.HD_STATUS_ID) LEFT JOIN HD_CATEGORY ON (HD_TICKET.HD_CATEGORY_ID=HD_CATEGORY.ID) WHERE (HD_STATUS.NAME not like '%Server Status Report%') AND ((HD_STATUS.STATE like '%closed%') AND (HD_STATUS.NAME not like '%spam%') AND (HD_TICKET.HD_QUEUE_ID = 4) AND (HD_TICKET.TIME_CLOSED > utc_timestamp() - interval 30 day)) GROUP BY HD_TICKET.HD_CATEGORY_ID order by total"

        Dim Chart = New DotNet.Highcharts.Highcharts("Chart_Category_2")
        Chart.InitChart(New Chart With {.Height = 500, .Width = 800, .BorderWidth = 2, .BorderColor = Color.Gray, .Margin = {0, 0, 0, 0}, .SpacingBottom = 1000})

        Chart.SetTitle(New Title With {.Text = "Tickets Closed by Category For The Last 30 Days", .X = 20})
        Dim palette_colors() As System.Drawing.Color = {System.Drawing.ColorTranslator.FromHtml("#058DC7"), System.Drawing.ColorTranslator.FromHtml("#50B432"), System.Drawing.ColorTranslator.FromHtml("#ED561B"), System.Drawing.ColorTranslator.FromHtml("#DDDF00"), System.Drawing.ColorTranslator.FromHtml("#24CBE5"), System.Drawing.ColorTranslator.FromHtml("#64E572"), System.Drawing.ColorTranslator.FromHtml("#FF9655"), System.Drawing.ColorTranslator.FromHtml("#FFF263"), System.Drawing.ColorTranslator.FromHtml("#6AF9C4")}
        Chart.SetOptions(New Helpers.GlobalOptions() With {.Colors = palette_colors})
        Chart.SetPlotOptions(New PlotOptions With {.Pie = (New PlotOptionsPie With {.AllowPointSelect = True})})
        ' Chart.SetLegend(New Legend With {.Layout = Layouts.Vertical, .Align = HorizontalAligns.Right, .VerticalAlign = VerticalAligns.Top, .X = -10, .Y = 100, .BorderWidth = 0})


        SqlDataSourceClosedTickets.SelectCommand = SAG
        Dim dvSql As DataView
        dvSql = DirectCast(SqlDataSourceClosedTickets.Select(DataSourceSelectArguments.Empty), DataView)
        Dim ArrayData As New List(Of Array)()
        For Each drvSql As DataRowView In dvSql
            ArrayData.Add({drvSql("CatName"), drvSql("total")})
        Next

        Chart.SetSeries(New Series With {.Type = ChartTypes.Pie, .Name = "Tickets", .Data = New Data(ArrayData.ToArray)})
        Chart.SetSubtitle(New Subtitle With {.Text = "Source: Kace", .X = -20})

        ltrChart3.Text = Chart.ToHtmlString
    End Sub

    Sub SetupChartTicketsClosedByCategory()
        Dim SAG As String = "SELECT HD_CATEGORY.NAME as CatName, count(HD_TICKET.ID) as total FROM HD_TICKET JOIN HD_STATUS ON (HD_STATUS.ID = HD_TICKET.HD_STATUS_ID) LEFT JOIN HD_CATEGORY ON (HD_TICKET.HD_CATEGORY_ID=HD_CATEGORY.ID) WHERE (HD_STATUS.NAME not like '%Server Status Report%') AND ((HD_STATUS.STATE like '%closed%') AND (HD_STATUS.NAME not like '%spam%') AND (HD_TICKET.HD_QUEUE_ID = 4) AND (HD_TICKET.TIME_CLOSED > utc_timestamp() - interval 30 day)) GROUP BY HD_TICKET.HD_CATEGORY_ID order by total"

        Dim Chart = New DotNet.Highcharts.Highcharts("Chart_Category").InitChart(New Chart With {.Width = 800})

        Chart.SetTitle(New Title With {.Text = "Tickets Closed by Category For The Last 30 Days", .X = 20})
        Dim palette_colors() As System.Drawing.Color = {System.Drawing.ColorTranslator.FromHtml("#058DC7"), System.Drawing.ColorTranslator.FromHtml("#50B432"), System.Drawing.ColorTranslator.FromHtml("#ED561B"), System.Drawing.ColorTranslator.FromHtml("#DDDF00"), System.Drawing.ColorTranslator.FromHtml("#24CBE5"), System.Drawing.ColorTranslator.FromHtml("#64E572"), System.Drawing.ColorTranslator.FromHtml("#FF9655"), System.Drawing.ColorTranslator.FromHtml("#FFF263"), System.Drawing.ColorTranslator.FromHtml("#6AF9C4")}
        Chart.SetOptions(New Helpers.GlobalOptions() With {.Colors = palette_colors})
        Chart.SetPlotOptions(New PlotOptions With {.Pie = (New PlotOptionsPie With {.AllowPointSelect = True})})
        ' Chart.SetLegend(New Legend With {.Layout = Layouts.Vertical, .Align = HorizontalAligns.Right, .VerticalAlign = VerticalAligns.Top, .X = -10, .Y = 100, .BorderWidth = 0})


        SqlDataSourceClosedTickets.SelectCommand = SAG
        Dim dvSql As DataView
        dvSql = DirectCast(SqlDataSourceClosedTickets.Select(DataSourceSelectArguments.Empty), DataView)
        Dim ArrayData As New List(Of Array)()
        For Each drvSql As DataRowView In dvSql
            ArrayData.Add({drvSql("CatName"), drvSql("total")})
        Next

        Chart.SetSeries(New Series With {.Type = ChartTypes.Pie, .Name = "Tickets", .Data = New Data(ArrayData.ToArray)})
        Chart.SetSubtitle(New Subtitle With {.Text = "Source: Kace", .X = -20})

        ltrChart2.Text = Chart.ToHtmlString
    End Sub

    Sub SetupChartTicketsClosedByQueue()
        Dim SAG As String = "SELECT Y AS YEAR, m AS MONTH, COUNT(hd_ticket.ID) AS total, hd_status.NAME AS STATUS FROM ( SELECT Y, m FROM ( SELECT YEAR(CURDATE()) Y UNION ALL SELECT YEAR(CURDATE())-1) years, ( SELECT 1 m UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12) months) ym LEFT JOIN hd_ticket ON ym.y = YEAR(hd_ticket.TIME_CLOSED) AND ym.m = MONTH(hd_ticket.TIME_CLOSED) AND (hd_ticket.HD_QUEUE_ID = 4) LEFT JOIN hd_status ON hd_status.ID = hd_ticket.HD_STATUS_ID AND (hd_status.STATE LIKE '%closed%') AND (hd_status.NAME NOT LIKE '%Spam%') AND (hd_status.NAME NOT LIKE '%Server Status Report%') WHERE ((Y= YEAR(CURDATE()) AND m<= MONTH(CURDATE())) OR (Y< YEAR(CURDATE()) AND m> MONTH(CURDATE()))) GROUP BY Y, m;"
        Dim SEG As String = "SELECT Y AS YEAR, m AS MONTH, COUNT(hd_ticket.ID) AS total, hd_status.NAME AS STATUS FROM ( SELECT Y, m FROM ( SELECT YEAR(CURDATE()) Y UNION ALL SELECT YEAR(CURDATE())-1) years, ( SELECT 1 m UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12) months) ym LEFT JOIN hd_ticket ON ym.y = YEAR(hd_ticket.TIME_CLOSED) AND ym.m = MONTH(hd_ticket.TIME_CLOSED) AND (hd_ticket.HD_QUEUE_ID = 5) LEFT JOIN hd_status ON hd_status.ID = hd_ticket.HD_STATUS_ID AND (hd_status.STATE LIKE '%closed%') AND (hd_status.NAME NOT LIKE '%Spam%') AND (hd_status.NAME NOT LIKE '%Server Status Report%') WHERE ((Y= YEAR(CURDATE()) AND m<= MONTH(CURDATE())) OR (Y< YEAR(CURDATE()) AND m> MONTH(CURDATE()))) GROUP BY Y, m;"

        Dim Chart = New DotNet.Highcharts.Highcharts("chart")


        Chart.SetTitle(New Title With {.Text = "Tickets Closed by Category For The Last 30 Days", .X = 20})
        Dim palette_colors() As System.Drawing.Color = {System.Drawing.ColorTranslator.FromHtml("#058DC7"), System.Drawing.ColorTranslator.FromHtml("#50B432"), System.Drawing.ColorTranslator.FromHtml("#ED561B"), System.Drawing.ColorTranslator.FromHtml("#DDDF00"), System.Drawing.ColorTranslator.FromHtml("#24CBE5"), System.Drawing.ColorTranslator.FromHtml("#64E572"), System.Drawing.ColorTranslator.FromHtml("#FF9655"), System.Drawing.ColorTranslator.FromHtml("#FFF263"), System.Drawing.ColorTranslator.FromHtml("#6AF9C4")}
        Chart.SetOptions(New Helpers.GlobalOptions() With {.Colors = palette_colors})
        Chart.SetPlotOptions(New PlotOptions With {.Pie = (New PlotOptionsPie With {.AllowPointSelect = True})})
        Chart.SetLegend(New Legend With {.Layout = Layouts.Vertical, .Align = HorizontalAligns.Right, .VerticalAlign = VerticalAligns.Top, .X = -10, .Y = 100, .BorderWidth = 0})
        Chart.SetXAxis(New XAxis With {.Type = AxisTypes.Category})
        Chart.SetYAxis(New YAxis With {.Min = 0, .Title = New YAxisTitle With {.Text = "Tickets"}})
        Chart.SetSubtitle(New Subtitle With {.Text = "Source: Kace", .X = -20})


        Dim series As New List(Of Series)()


        SqlDataSourceClosedTickets.SelectCommand = SAG
        Dim dvSql As DataView
        dvSql = DirectCast(SqlDataSourceClosedTickets.Select(DataSourceSelectArguments.Empty), DataView)
        Dim sqlList As New List(Of Array)()

        For Each drvSql As DataRowView In dvSql
            sqlList.Add({drvSql("month").ToString + "-" + drvSql("year").ToString, Integer.Parse(drvSql("total").ToString)})
        Next
        series.Add(New Series With {.Type = ChartTypes.Line, .Name = "Information Technology - SAG", .Data = New Data(sqlList.ToArray)})


        SqlDataSourceClosedTickets.SelectCommand = SEG
        dvSql = DirectCast(SqlDataSourceClosedTickets.Select(DataSourceSelectArguments.Empty), DataView)
        sqlList.Clear()
        For Each drvSql As DataRowView In dvSql
            sqlList.Add({drvSql("month").ToString + "-" + drvSql("year").ToString, Integer.Parse(drvSql("total").ToString)})
        Next
        series.Add(New Series With {.Type = ChartTypes.Line, .Name = "Information Technology - SEG", .Data = New Data(sqlList.ToArray)})

        Chart.SetSeries(series.ToArray)

        ltrChart.Text = Chart.ToHtmlString
    End Sub

End Class