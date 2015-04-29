<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Default.aspx.vb" Inherits="KACEBootstrap._Default3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>All Open Tickets for
        <asp:Label ID="lblQueueTitle" runat="server" Text="lblQueueTitle"></asp:Label>
    </h2>
    <p>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceITHelpDesk" DataKeyNames="ID"  CssClass="table table-hover table-bordered table-condensed" AllowPaging="True" PageSize="50" Width="100%">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="http://k1000.internal.gorbel.com/adminui/ticket.php?ID={0}" DataTextField="ID" HeaderText="ID" Target="_blank" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Priority") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Priority") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
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
                    SelectCommand="SELECT HD_TICKET.ID AS ID, HD_TICKET.TITLE AS TITLE, HD_STATUS.NAME AS STATUS, HD_PRIORITY.NAME AS PRIORITY, HD_TICKET.CREATED AS CREATED, HD_TICKET.MODIFIED AS MODIFIED, S.FULL_NAME AS SUBMITTER, O.FULL_NAME AS OWNER, HD_CATEGORY.NAME FROM HD_TICKET JOIN HD_CATEGORY ON (HD_TICKET.HD_CATEGORY_ID = HD_CATEGORY.ID) JOIN HD_STATUS ON (HD_STATUS.ID = HD_TICKET.HD_STATUS_ID) JOIN HD_PRIORITY ON (HD_PRIORITY.ID = HD_TICKET.HD_PRIORITY_ID) LEFT JOIN USER S ON (S.ID = HD_TICKET.SUBMITTER_ID) LEFT JOIN USER O ON (O.ID = HD_TICKET.OWNER_ID) WHERE (HD_TICKET.HD_QUEUE_ID = 5) AND (HD_STATUS.STATE NOT LIKE '%CLOSED%') AND HD_CATEGORY.NAME LIKE '%CHINA::%' ORDER BY OWNER, CREATED DESC">
                </asp:SqlDataSource>
    </p>

</asp:Content>
