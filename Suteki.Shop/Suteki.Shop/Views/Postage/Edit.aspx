<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master"   Inherits="Suteki.Shop.ViewPage<ScaffoldViewData<Suteki.Shop.Postage>>" %>
<%@ Import Namespace="Suteki.Common.ViewData"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>Postage Band</h1>
    <%= Html.ValidationSummary() %>
    <%= Html.MessageBox(ViewData.Model) %>

    <% using (Html.BeginForm()) { %>
		<%= this.Hidden(x=>x.Item.Id) %>
		<%= this.Hidden(x => x.Item.Position) %>
		<%= this.TextBox(x => x.Item.Name).Label("Name") %>
		<%= this.TextBox(x => x.Item.MaxWeight).Label("Max Weight (grams)") %>
        <%= this.TextBox(x => x.Item.Price).Label("Price") %>
        <%= this.CheckBox(x=>x.Item.IsActive).Label("Active") %>
        <input type="submit" value="Save Changes" />
    <% } %>
</asp:Content>
