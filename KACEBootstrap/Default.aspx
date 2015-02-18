<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="KACEBootstrap._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>All Open Tickets for
        <asp:Label ID="lblQueueTitle" runat="server" Text="lblQueueTitle"></asp:Label>
    </h2>
    <p>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceITHelpDesk" DataKeyNames="ID"  CssClass="table table-hover table-bordered table-condensed" AllowPaging="True" PageSize="50" Width="100%">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="http://k1000.internal.gorbel.com/adminui/ticket.php?ID={0}" DataTextField="ID" HeaderText="ID" Target="_blank" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority"  />
                <asp:BoundField DataField="Created" HeaderText="Created" SortExpression="Created" />
                <asp:BoundField DataField="Modified" HeaderText="Modified" SortExpression="Modified" />
                <asp:BoundField DataField="Submitter" HeaderText="Submitter" SortExpression="Submitter" />
                <asp:BoundField DataField="Owner" HeaderText="Owner" SortExpression="Owner" />
                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" Visible="False" />
            </Columns>
        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSourceITHelpDesk" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ORG1ConnectionString %>" 
                    ProviderName="<%$ ConnectionStrings:ORG1ConnectionString.ProviderName %>" 
                    SelectCommand="SELECT HD_TICKET.ID AS ID, HD_TICKET.TITLE AS Title, HD_STATUS.NAME AS Status, HD_PRIORITY.NAME AS Priority, HD_TICKET.CREATED AS Created, HD_TICKET.MODIFIED AS Modified, S.FULL_NAME AS Submitter, O.FULL_NAME AS Owner, HD_TICKET.CUSTOM_FIELD_VALUE0 AS Type FROM HD_TICKET JOIN HD_STATUS ON (HD_STATUS.ID = HD_TICKET.HD_STATUS_ID) JOIN HD_PRIORITY ON (HD_PRIORITY.ID = HD_TICKET.HD_PRIORITY_ID) LEFT JOIN USER S ON (S.ID = HD_TICKET.SUBMITTER_ID) LEFT JOIN USER O ON (O.ID = HD_TICKET.OWNER_ID) WHERE (HD_TICKET.HD_QUEUE_ID = 4) AND (HD_STATUS.STATE NOT LIKE '%Closed%') ORDER BY Owner, Created DESC">
                </asp:SqlDataSource>
    </p>

</asp:Content>
