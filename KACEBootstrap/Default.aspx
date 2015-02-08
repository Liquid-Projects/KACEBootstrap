<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="KACEBootstrap._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Unassigned Support Tickets for
        <asp:Label ID="lblQueueTitle" runat="server" Text="lblQueueTitle"></asp:Label>
    </h2>
    <p>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </p>

</asp:Content>
