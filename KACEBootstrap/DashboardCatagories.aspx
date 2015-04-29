<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DashboardCatagories.aspx.vb" Inherits="KACEBootstrap.DashboardCatagories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Literal ID="litchart" runat="server"></asp:Literal>
    <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered table-head-bordered-bottom table-condensed">
    </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceKACE" runat="server" ConnectionString="<%$ ConnectionStrings:ORG1ConnectionString %>" ProviderName="<%$ ConnectionStrings:ORG1ConnectionString.ProviderName %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceChart" runat="server"  ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="">
    </asp:SqlDataSource>
</asp:Content>
