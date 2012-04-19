<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/CmsSubMenu.master" Inherits="Suteki.Shop.ViewPage<CmsViewData>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<h1>Menu</h1>

<%= Html.ValidationSummary() %>

<% if (Model.Content.Id > 0) { %>
	<p><%= ViewData.Model.Content.Link(Html)%></p>
<% } %>

<% using(Html.BeginForm()) { %>
	<%= this.Hidden(x => x.Content.Id) %>
	<%= this.Hidden(x => x.Content.Position) %>
	<%= this.TextBox(x => x.Content.Name).Label("Name") %>
    <label>Parent Menu</label>
    <%= Html.ComboFor(x => x.Content.ParentContent, x => x.Id != Model.Content.Id) %>
	<%= this.CheckBox(x => x.Content.IsActive).Label("Active") %>
	
	<input type="submit" value="Save Changes" />
<% } %>
</asp:Content>
