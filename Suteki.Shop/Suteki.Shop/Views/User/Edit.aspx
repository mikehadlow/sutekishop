<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Suteki.Shop.ViewPage<ShopViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>User</h1>
    
	<%= Html.ValidationSummary() %>
    <%= Html.MessageBox(ViewData.Model) %>

    <% using(Html.BeginForm()) { %>
		<%= this.Hidden(x => x.User.Id) %>
		<%= this.TextBox(x => x.User.Email).Label("Email") %>
        <%--NOTE: We lose modelstate support for password as we're overriding the name--%>
		<%= this.Password(x=> x.User.Password).Name("password").Value("").Label("Password (leave blank if you don't want to change)") %>
		<%= this.Select(x => x.User.Role.Id).Options(Model.Roles, x => x.Id, x => x.Name).Label("Role") %>
        <%= this.CheckBox(x => x.User.IsEnabled).Label("User can log on") %>

		<input type="submit" value="Save" />
    <% } %>

</asp:Content>
