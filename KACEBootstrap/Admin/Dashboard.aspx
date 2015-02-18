<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Dashboard.aspx.vb" Inherits="KACEBootstrap.Dashboard1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <form>
        <div class="form-group">
            <label for="TxtChartTicketsClosedByQueue">Chart Title:</label>
            <asp:TextBox ID="TxtChartTicketsClosedByQueue" runat="server" class="form-control" placeholder="Chart Title"></asp:TextBox>
        </div>
        <div class="form-group form-inline">
            <label for="TxtChartTicketsClosedByQueue_XTitle" class="control-label">Chart X-Axis Title:</label>
            <asp:TextBox ID="TxtChartTicketsClosedByQueue_XTitle" runat="server" class="form-control" placeholder="Chart X-Axis Title"></asp:TextBox>
            <br />
            <label for="TxtChartTicketsClosedByQueue_YTitle" class="control-label">Chart Y-Axis Title:</label>
            <asp:TextBox ID="TxtChartTicketsClosedByQueue_YTitle" runat="server" class="form-control" placeholder="Chart Y-Axis Title:"></asp:TextBox>
        </div>
    </form>

</asp:Content>
