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

    Dim ChartSQL As String = "SELECT Chart.Id, Chart.Title, Chart.SubTitle, Chart.Colors, Chart.Height, Chart.Width, Chart.Margin_Top, Chart.Margin_Bottom, Chart.Margin_Left, Chart.Margin_Right, Chart.Chart_Type, Chart_Legend.Legend_Layout, Chart_Legend.Legend_Align, Chart_Legend.Legend_VerticalAligns, Chart_Legend.Legend_X, Chart_Legend.Legend_Y, Chart_Legend.Legend_BorderWidth FROM Chart LEFT OUTER JOIN Chart_Legend ON Chart.Id = Chart_Legend.Chart_ID WHERE (Chart.Id = @ChartID)"
    Dim ChartSQLData As String = "Select Chart.Id, Chart_Data_Series.Chart_Data_Set, Chart_Data_Series.KACE_QUEUE, Chart_Data_Series.Chart_Data_Name FROM Chart INNER JOIN Chart_Data_Series ON Chart.Id = Chart_Data_Series.Chart_id WHERE (Chart.Id = @ChartID)"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call SetupChartTicketsClosedByQueue()
        Call SetupChartTicketsClosedByCategory()
        Call SetupChartTicketsClosedByDepartment()
        Call SetupChartTicketsClosedByOwner()
    End Sub

    Sub SetupChartTicketsClosedByCategory()
        SqlDataSourceChart.SelectParameters.Clear()
        SqlDataSourceChart.SelectParameters.Add("ChartID", 1)
        SqlDataSourceChart.SelectCommand = ChartSQL
        Dim dvSqlChart As DataView = SqlDataSourceChart.Select(DataSourceSelectArguments.Empty)

        Dim Chart = New DotNet.Highcharts.Highcharts("Chart_Category")

        Dim InitChart As New Chart
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Top")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Top").ToString) Then InitChart.MarginTop = Integer.Parse(dvSqlChart(0).Item("Margin_Top").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Left")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Left").ToString) Then InitChart.MarginLeft = Integer.Parse(dvSqlChart(0).Item("Margin_Left").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Right")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Right").ToString) Then InitChart.MarginRight = Integer.Parse(dvSqlChart(0).Item("Margin_Right").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Bottom")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Bottom").ToString) Then InitChart.MarginBottom = Integer.Parse(dvSqlChart(0).Item("Margin_Bottom").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Height")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Height").ToString) Then InitChart.Height = Integer.Parse(dvSqlChart(0).Item("Height").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Width")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Width").ToString) Then InitChart.Width = Integer.Parse(dvSqlChart(0).Item("Width").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Chart_Type")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Chart_Type").ToString) Then InitChart.Type = dvSqlChart(0).Item("Chart_Type")
        Chart.InitChart(InitChart)

        Chart.SetTitle(New Title With {.Text = dvSqlChart(0).Item("Title").ToString()})
        Chart.SetPlotOptions(New PlotOptions With {.Pie = (New PlotOptionsPie With {.AllowPointSelect = True})})

        Dim chartLegend As New Legend
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Layout")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Layout").ToString) Then chartLegend.Layout = Integer.Parse(dvSqlChart(0).Item("Legend_Layout").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Align")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Align").ToString) Then chartLegend.Align = Integer.Parse(dvSqlChart(0).Item("Legend_Align").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_VerticalAligns")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_VerticalAligns").ToString) Then chartLegend.VerticalAlign = Integer.Parse(dvSqlChart(0).Item("Legend_VerticalAligns").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_X")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_X").ToString) Then chartLegend.X = Integer.Parse(dvSqlChart(0).Item("Legend_X").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Y")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Y").ToString) Then chartLegend.Y = Integer.Parse(dvSqlChart(0).Item("Legend_Y").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_BorderWidth")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_BorderWidth").ToString) Then chartLegend.BorderWidth = Integer.Parse(dvSqlChart(0).Item("Legend_BorderWidth").ToString)
        Chart.SetLegend(chartLegend)


        Dim ChartData As New List(Of Array)()

        Dim sqlChartData As New SqlDataSource
        sqlChartData.SelectParameters.Add("ChartID", 1)
        sqlChartData.SelectCommand = ChartSQLData
        sqlChartData.ConnectionString = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim dvSqlChartData As DataView = sqlChartData.Select(DataSourceSelectArguments.Empty)

        For Each drvSqlChatData As DataRowView In dvSqlChartData
            SqlDataSourceKACE.SelectParameters.Clear()
            SqlDataSourceKACE.SelectParameters.Add("QUEUE_ID", drvSqlChatData("KACE_QUEUE"))
            SqlDataSourceKACE.SelectCommand = drvSqlChatData("Chart_Data_Set")
            Dim drvSqlKACE As DataView = SqlDataSourceKACE.Select(DataSourceSelectArguments.Empty)
            For Each drvSql As DataRowView In drvSqlKACE
                ChartData.Add({drvSql("CatName"), drvSql("total")})
            Next
        Next

        If Not IsDBNull(dvSqlChart(0).Item("Colors")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Colors")) Then
            Dim palette_colors_list As New List(Of System.Drawing.Color)
            For Each Color As String In dvSqlChart(0).Item("Colors").ToString.Split(",").ToArray
                palette_colors_list.Add(System.Drawing.ColorTranslator.FromHtml(Color))
            Next
            Chart.SetColors(New Options.Colors With {.ChartColors = palette_colors_list.ToArray})
        End If

        Chart.SetSeries(New Series With {.Type = ChartTypes.Pie, .Name = "Tickets", .Data = New Data(ChartData.ToArray)})
        Chart.SetSubtitle(New Subtitle With {.Text = dvSqlChart(0).Item("SubTitle").ToString(), .X = -20})

        Dim litchart As New Literal With {.Text = Chart.ToHtmlString}
        Dim mpContentPlaceHolder As ContentPlaceHolder
        mpContentPlaceHolder = CType(Master.FindControl("MainContent"), ContentPlaceHolder)
        mpContentPlaceHolder.Controls.Add(litchart)
    End Sub

    Sub SetupChartTicketsClosedByQueue()
        SqlDataSourceChart.SelectParameters.Clear()
        SqlDataSourceChart.SelectParameters.Add("ChartID", 2)
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
        Dim litchart As New Literal With {.Text = Chart.ToHtmlString}
        Dim mpContentPlaceHolder As ContentPlaceHolder = CType(Master.FindControl("MainContent"), ContentPlaceHolder)
        mpContentPlaceHolder.Controls.Add(litchart)

    End Sub

    Sub SetupChartTicketsClosedByDepartment()
        SqlDataSourceChart.SelectParameters.Clear()
        SqlDataSourceChart.SelectParameters.Add("ChartID", 3)
        SqlDataSourceChart.SelectCommand = ChartSQL
        Dim dvSqlChart As DataView = SqlDataSourceChart.Select(DataSourceSelectArguments.Empty)

        Dim Chart = New DotNet.Highcharts.Highcharts("Chart_Department")

        Dim InitChart As New Chart
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Top")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Top").ToString) Then InitChart.MarginTop = Integer.Parse(dvSqlChart(0).Item("Margin_Top").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Left")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Left").ToString) Then InitChart.MarginLeft = Integer.Parse(dvSqlChart(0).Item("Margin_Left").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Right")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Right").ToString) Then InitChart.MarginRight = Integer.Parse(dvSqlChart(0).Item("Margin_Right").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Bottom")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Bottom").ToString) Then InitChart.MarginBottom = Integer.Parse(dvSqlChart(0).Item("Margin_Bottom").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Height")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Height").ToString) Then InitChart.Height = Integer.Parse(dvSqlChart(0).Item("Height").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Width")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Width").ToString) Then InitChart.Width = Integer.Parse(dvSqlChart(0).Item("Width").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Chart_Type")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Chart_Type").ToString) Then InitChart.Type = dvSqlChart(0).Item("Chart_Type")
        Chart.InitChart(InitChart)

        Chart.SetTitle(New Title With {.Text = dvSqlChart(0).Item("Title").ToString()})
        Chart.SetPlotOptions(New PlotOptions With {.Pie = (New PlotOptionsPie With {.AllowPointSelect = True})})

        Dim chartLegend As New Legend
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Layout")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Layout").ToString) Then chartLegend.Layout = Integer.Parse(dvSqlChart(0).Item("Legend_Layout").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Align")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Align").ToString) Then chartLegend.Align = Integer.Parse(dvSqlChart(0).Item("Legend_Align").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_VerticalAligns")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_VerticalAligns").ToString) Then chartLegend.VerticalAlign = Integer.Parse(dvSqlChart(0).Item("Legend_VerticalAligns").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_X")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_X").ToString) Then chartLegend.X = Integer.Parse(dvSqlChart(0).Item("Legend_X").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Y")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Y").ToString) Then chartLegend.Y = Integer.Parse(dvSqlChart(0).Item("Legend_Y").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_BorderWidth")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_BorderWidth").ToString) Then chartLegend.BorderWidth = Integer.Parse(dvSqlChart(0).Item("Legend_BorderWidth").ToString)
        Chart.SetLegend(chartLegend)


        Dim ChartData As New List(Of Array)()

        Dim sqlChartData As New SqlDataSource
        sqlChartData.SelectParameters.Add("ChartID", 3)
        sqlChartData.SelectCommand = ChartSQLData
        sqlChartData.ConnectionString = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim dvSqlChartData As DataView = sqlChartData.Select(DataSourceSelectArguments.Empty)

        For Each drvSqlChatData As DataRowView In dvSqlChartData
            SqlDataSourceKACE.SelectParameters.Clear()
            SqlDataSourceKACE.SelectCommand = drvSqlChatData("Chart_Data_Set")
            Dim drvSqlKACE As DataView = SqlDataSourceKACE.Select(DataSourceSelectArguments.Empty)
            For Each drvSql As DataRowView In drvSqlKACE
                ChartData.Add({drvSql("department"), drvSql("total")})
            Next
        Next

        If Not IsDBNull(dvSqlChart(0).Item("Colors")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Colors").ToString) Then
            Dim palette_colors_list As New List(Of System.Drawing.Color)
            For Each Color As String In dvSqlChart(0).Item("Colors").ToString.Split(",").ToArray
                palette_colors_list.Add(System.Drawing.ColorTranslator.FromHtml(Color))
            Next
            Chart.SetColors(New Options.Colors With {.ChartColors = palette_colors_list.ToArray})
        End If

        Chart.SetSeries(New Series With {.Type = ChartTypes.Pie, .Name = "Tickets", .Data = New Data(ChartData.ToArray)})
        Chart.SetSubtitle(New Subtitle With {.Text = dvSqlChart(0).Item("SubTitle").ToString(), .X = -20})

        Dim litchart As New Literal With {.Text = Chart.ToHtmlString}
        Dim mpContentPlaceHolder As ContentPlaceHolder
        mpContentPlaceHolder = CType(Master.FindControl("MainContent"), ContentPlaceHolder)
        mpContentPlaceHolder.Controls.Add(litchart)
    End Sub

    Structure monthData
        Dim name As String
        Dim data As List(Of String)
    End Structure

    Sub SetupChartTicketsClosedByOwner()
        'Clears any select Parameter on the sql quary
        SqlDataSourceChart.SelectParameters.Clear()
        'Add a Parameter of ChartID to find the configuation data in "Chart"
        SqlDataSourceChart.SelectParameters.Add("ChartID", 4)
        'execute sql select
        SqlDataSourceChart.SelectCommand = ChartSQL
        'Pass Sql Select to a DataView
        Dim dvSqlChart As DataView = SqlDataSourceChart.Select(DataSourceSelectArguments.Empty)
        'Create a HighCharts Object
        Dim Chart = New DotNet.Highcharts.Highcharts("chart_Owners")
        'Set Initial Chart Configuration, by passing in data from Table "Chart"
        Dim InitChart As New Chart
        InitChart.Type = ChartTypes.Column
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Top")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Top").ToString) Then InitChart.MarginTop = Integer.Parse(dvSqlChart(0).Item("Margin_Top").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Left")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Left").ToString) Then InitChart.MarginLeft = Integer.Parse(dvSqlChart(0).Item("Margin_Left").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Right")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Right").ToString) Then InitChart.MarginRight = Integer.Parse(dvSqlChart(0).Item("Margin_Right").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Margin_Bottom")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Margin_Bottom").ToString) Then InitChart.MarginBottom = Integer.Parse(dvSqlChart(0).Item("Margin_Bottom").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Height")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Height").ToString) Then InitChart.Height = Integer.Parse(dvSqlChart(0).Item("Height").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Width")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Width").ToString) Then InitChart.Width = Integer.Parse(dvSqlChart(0).Item("Width").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Chart_Type")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Chart_Type").ToString) Then InitChart.Type = dvSqlChart(0).Item("Chart_Type")
        'Pass Intital Chart Configuation back to HighChart
        Chart.InitChart(InitChart)

        'Defind the charts Ploting Options, and incude custom colors if need
        Dim ChartPlotOptions As New PlotOptions
        If Not IsDBNull(dvSqlChart(0).Item("Colors")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Colors").ToString) Then
            Dim palette_colors_list As New List(Of System.Drawing.Color)
            For Each Color As String In dvSqlChart(0).Item("Colors").ToString.Split(",").ToArray
                palette_colors_list.Add(System.Drawing.ColorTranslator.FromHtml(Color))
            Next
            Chart.SetColors(New Options.Colors With {.ChartColors = palette_colors_list.ToArray})
        End If

        'Set the HighCharts Title, by passing it from table "Chart"
        Chart.SetTitle(New Title With {.Text = dvSqlChart(0).Item("Title").ToString(), .X = 20})

        'Set Legend from sql table "Chart_Legend"
        Dim chartLegend As New Legend
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Layout")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Layout").ToString) Then chartLegend.Layout = Integer.Parse(dvSqlChart(0).Item("Legend_Layout").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Align")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Align").ToString) Then chartLegend.Align = Integer.Parse(dvSqlChart(0).Item("Legend_Align").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_VerticalAligns")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_VerticalAligns").ToString) Then chartLegend.VerticalAlign = Integer.Parse(dvSqlChart(0).Item("Legend_VerticalAligns").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_X")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_X").ToString) Then chartLegend.X = Integer.Parse(dvSqlChart(0).Item("Legend_X").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_Y")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_Y").ToString) Then chartLegend.Y = Integer.Parse(dvSqlChart(0).Item("Legend_Y").ToString)
        If Not IsDBNull(dvSqlChart(0).Item("Legend_BorderWidth")) And Not String.IsNullOrWhiteSpace(dvSqlChart(0).Item("Legend_BorderWidth").ToString) Then chartLegend.BorderWidth = Integer.Parse(dvSqlChart(0).Item("Legend_BorderWidth").ToString)
        Chart.SetLegend(chartLegend)

        'Set YAxis information, including the name
        Chart.SetYAxis(New YAxis With {.Min = 0, .Title = New YAxisTitle With {.Text = "Tickets"}})
        'Set SubTitle information
        Chart.SetSubtitle(New Subtitle With {.Text = dvSqlChart(0).Item("SubTitle").ToString(), .X = -20})

        'Create series of HighChart.Series to store the data
        Dim series As New List(Of Series)()
        'Creates a new sql datasource and retive sql select stamtments to access data from K1000
        Dim sqlChartData As New SqlDataSource
        'Pass in chartID=4 to retreave this charts data.
        sqlChartData.SelectParameters.Add("ChartID", 4)
        sqlChartData.SelectCommand = ChartSQLData
        sqlChartData.ConnectionString = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Dim dvSqlChartData As DataView = sqlChartData.Select(DataSourceSelectArguments.Empty)

        'Store Users who are Owners With in the Queue
        Dim Users As New Dictionary(Of String, String)
        'Holds Key Value Pairs of Open and Closed Tickets for each month for each user.
        Dim seriesOpen As New Dictionary(Of Integer, Dictionary(Of String, Integer))
        Dim seriesClosed As New Dictionary(Of Integer, Dictionary(Of String, Integer))
        'Hold Key Value Pairs for KACE UserID and Index Value
        Dim userid_xref As New Dictionary(Of Integer, Integer)
        'Set Max Number of months
        'Note: This needs to be converted to SQL Lookup
        Const maxMonths As Integer = 3
        'Get todays date so we can caluate the number of months to count
        Dim dtToday As DateTime = Date.Today



        For Each drvSqlChatData As DataRowView In dvSqlChartData

            'Gets the users who are only the Owners of the Current Queue
            'Note: This needs to be converted to SQL Lookup
            'Note:
            Dim sql_Users As String = "SELECT USER.ID,USER.USER_NAME FROM USER LEFT JOIN USER_LABEL_JT ON (USER_LABEL_JT.USER_ID=USER.ID) LEFT JOIN LABEL ON (USER_LABEL_JT.LABEL_ID=LABEL.ID) Left JOIN hd_queue_owner_label_jt ON (hd_queue_owner_label_jt.LABEL_ID=label.ID) WHERE hd_queue_owner_label_jt.HD_QUEUE_ID=@QUEUE_ID GROUP BY USER.ID ORDER BY USER.USER_NAME"
            SqlDataSourceKACE.SelectParameters.Clear()
            SqlDataSourceKACE.SelectParameters.Add("QUEUE_ID", drvSqlChatData("KACE_QUEUE"))
            SqlDataSourceKACE.SelectCommand = sql_Users

            Dim drvSqlKACE As DataView = SqlDataSourceKACE.Select(DataSourceSelectArguments.Empty)
            For i = 0 To drvSqlKACE.Count - 1
                Dim drvSql As DataRowView = drvSqlKACE(i)
                'Users(i) = {drvSql("ID"), drvSql("USER_NAME")}
                If Not userid_xref.ContainsKey(drvSql("ID")) Then
                    Dim Month As New Dictionary(Of String, Integer)
                    For i_m = (maxMonths - 1) To 0 Step -1
                        Month(dtToday.AddMonths(-i_m).Month.ToString + "-" + dtToday.AddMonths(-i_m).Year.ToString) = 0
                    Next
                    seriesOpen(i) = Month
                    seriesClosed(i) = Month
                    Users(i) = drvSql("USER_NAME")
                    userid_xref(drvSql("ID")) = i
                End If
            Next


            SqlDataSourceKACE.SelectParameters.Clear()
            'SqlDataSourceKACE.SelectParameters.Add("QUEUE_ID", drvSqlChatData("KACE_QUEUE"))
            SqlDataSourceKACE.SelectCommand = drvSqlChatData("Chart_Data_Set")
            drvSqlKACE = SqlDataSourceKACE.Select(DataSourceSelectArguments.Empty)

            For i = 0 To drvSqlKACE.Count - 1
                Dim drvSql As DataRowView = drvSqlKACE(i)
                Dim total As Integer = drvSql("total")
                Dim month As Integer = drvSql("month")
                Dim year As Integer = drvSql("year")

                'If (Not IsNothing(userid_xref(drvSql("owner")))) Then
                'Continue For
                'End If

                Dim user As Integer = userid_xref.Item(drvSql("owner"))
                seriesOpen(user)(drvSql("month").ToString + "-" + drvSql("year").ToString) = total
            Next
            For i = (maxMonths - 1) To 0 Step -1
                Dim setData As New List(Of Object)
                For iu = 0 To userid_xref.Count - 1 Step 1
                    If drvSqlChatData("Chart_Data_Name").ToString = "Open Tickets" Then
                        setData.Add(seriesOpen(iu)(dtToday.AddMonths(-i).Month.ToString + "-" + dtToday.AddMonths(-i).Year.ToString))
                    Else
                        setData.Add(seriesClosed(iu)(dtToday.AddMonths(-i).Month.ToString + "-" + dtToday.AddMonths(-i).Year.ToString))
                    End If
                Next
                series.Add(New Series With {.Name = dtToday.AddMonths(-i).Month.ToString + "-" + dtToday.AddMonths(-i).Year.ToString + " - " + drvSqlChatData("Chart_Data_Name").ToString, .Data = New Data(setData.ToArray)})
            Next
        Next


        'series.Add(New Series With {.Name = drvSql("month").ToString + "-" + drvSql("year").ToString + " - " + drvSqlChatData("Chart_Data_Name"), .Data = New Data(data.ToArray)})
        Dim Categories As New List(Of String)(Users.Values)
        Chart.SetXAxis(New XAxis With {.Type = AxisTypes.Category, .Categories = Categories.ToArray})

        Chart.SetSeries(series.ToArray)
        Dim litchart As New Literal With {.Text = Chart.ToHtmlString}
        Dim mpContentPlaceHolder As ContentPlaceHolder = CType(Master.FindControl("MainContent"), ContentPlaceHolder)
        mpContentPlaceHolder.Controls.Add(litchart)

    End Sub

End Class