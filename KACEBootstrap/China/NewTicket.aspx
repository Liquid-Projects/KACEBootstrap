<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="NewTicket.aspx.vb" Inherits="KACEBootstrap._default2" EnableEventValidation="false"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<form class="form-horizontal">
<fieldset>

<!-- Form Name -->
<legend>China CraneBrain Service Desk</legend>

<!-- Text input-->
<div class="form-group">
  <label class="col-md-10 control-label" for="text_from_email">From</label>  
  <div class="col-sm-10">
  <asp:TextBox ID="text_from_email" runat="server" placeholder="Email Address" class="form-control input-md" required="" type="email"></asp:TextBox>
  <span class="help-block">Please enter your current email address</span>  
  </div>
</div>

<!-- Text input-->
<div class="form-group">
  <label class="col-md-10 control-label" for="text_Issue_Title">Issue Title</label>  
  <div class="col-sm-10">
  <asp:TextBox ID="text_Issue_Title" runat="server" placeholder="Issue Title" class="form-control input-md" required=""></asp:TextBox>
  <span class="help-block">Please create a Title for your request. Example: "Error is displayed when opeing quote"</span>  
  </div>
</div>

<!-- Textarea -->
<div class="form-group">
  <label class="col-md-10 control-label" for="text_Issue_description">Issue Description</label>
  <div class="col-sm-10">
     <asp:TextBox ID="text_Issue_description" runat="server" placeholder="Issue Title" class="form-control input-md" required="" TextMode="MultiLine" Rows="10"></asp:TextBox>                     
  </div>
</div>

<!-- Select Basic -->
<div class="form-group">
  <label class="col-md-10 control-label" for="select_cata">Select Catagory</label>
  <div class="col-sm-10">
      <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource_Cata" DataTextField="Short_Name" DataValueField="Long_Name"  class="form-control"></asp:DropDownList>
  </div>
</div>

<!-- File Button --> 
<div class="form-group">
  <label class="col-md-10 control-label" for="attach_File">Attach File</label>
  <div class="col-sm-10">
      <asp:FileUpload ID="FileUpload1" CssClass="input-file" runat="server" />
  </div>
</div>

<!-- Button (Double) -->
<div class="form-group">
  <label class="col-md-10 control-label" for="button1id"></label>
  <div class="col-md-10">
      <asp:Button ID="Button1" runat="server" Text="Submit" CssClass="btn btn-default" /><asp:Button ID="Button2" runat="server" Text="Clear Form" CssClass="btn btn-default"  />
  </div>
</div>

</fieldset>
</form>

    <asp:SqlDataSource ID="SqlDataSource_Cata" runat="server" ConnectionString="<%$ ConnectionStrings:ORG1ConnectionString %>" ProviderName="<%$ ConnectionStrings:ORG1ConnectionString.ProviderName %>" SelectCommand="SELECT SUBSTRING_INDEX(NAME, '::', - 1) AS Short_Name, NAME AS Long_Name FROM hd_category WHERE (HD_QUEUE_ID = 5) AND (NAME LIKE '%China::%')"></asp:SqlDataSource>

</asp:Content>