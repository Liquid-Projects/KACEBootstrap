<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Dashboard.aspx.vb" MasterPageFile="~/Site.Master" Inherits="KACEBootstrap.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="SqlDataSourceKACE" runat="server" ConnectionString="<%$ ConnectionStrings:ORG1ConnectionString %>" ProviderName="<%$ ConnectionStrings:ORG1ConnectionString.ProviderName %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceListQueues" runat="server"  ConnectionString="<%$ ConnectionStrings:ORG1ConnectionString %>" ProviderName="<%$ ConnectionStrings:ORG1ConnectionString.ProviderName %>" SelectCommand="SELECT ID, NAME FROM hd_queue"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceChart" runat="server"  ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceChart0" runat="server"  ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT Chart.Id, Chart.Title, Chart.SubTitle, Chart.Colors, Chart.Height, Chart.Width, Chart.Margin_Top, Chart.Margin_Bottom, Chart.Margin_Left, Chart.Margin_Right, Chart.Chart_Type, Chart_Legend.Legend_Layout, Chart_Legend.Legend_Align, Chart_Legend.Legend_VerticalAligns, Chart_Legend.Legend_X, Chart_Legend.Legend_Y, Chart_Legend.Legend_BorderWidth FROM Chart LEFT OUTER JOIN Chart_Legend ON Chart.Id = Chart_Legend.Chart_ID WHERE (Chart.Id = @ChartID)">
        <SelectParameters>
            <asp:Parameter Name="ChartID" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
</asp:Content>
