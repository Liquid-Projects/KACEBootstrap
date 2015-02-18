<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Dashboard.aspx.vb" MasterPageFile="~/Site.Master" Inherits="KACEBootstrap.Dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="SqlDataSourceClosedTickets" runat="server" ConnectionString="<%$ ConnectionStrings:ORG1ConnectionString %>" ProviderName="<%$ ConnectionStrings:ORG1ConnectionString.ProviderName %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceListQueues" runat="server"  ConnectionString="<%$ ConnectionStrings:ORG1ConnectionString %>" ProviderName="<%$ ConnectionStrings:ORG1ConnectionString.ProviderName %>" SelectCommand="SELECT ID, NAME FROM hd_queue"></asp:SqlDataSource>
    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSourceListQueues" DataTextField="NAME" DataValueField="ID">
    </asp:DropDownList>
    <br />
    <asp:Literal ID="ltrChart" runat="server"></asp:Literal>
    <br />
    <div style="float:left"><asp:Literal ID="ltrChart2" runat="server"></asp:Literal></div>
    <div style="float:right"><asp:Literal ID="ltrChart3" runat="server"></asp:Literal></div>
</asp:Content>
