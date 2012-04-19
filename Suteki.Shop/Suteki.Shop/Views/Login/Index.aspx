<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ShopViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h1>Login</h1>
    <%= Html.ErrorBox(ViewData.Model)%>

	<% using(Html.BeginForm()) { %>
        <label for="username">Email</label>
        <%= Html.TextBox("email") %>
        
        <label for="password">Password</label>
        <%= Html.Password("password") %>

        <%= Html.SubmitButton() %>
	<% } %>
</asp:Content>
