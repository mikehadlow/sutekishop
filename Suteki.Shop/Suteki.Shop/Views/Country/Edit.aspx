<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ScaffoldViewData<Suteki.Shop.Country>>" %>
<%@ Import Namespace="Suteki.Common.ViewData"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>Country</h1>
    <%= Html.ValidationSummary() %>
    <%= Html.MessageBox(ViewData.Model)%>

    <% using (Html.BeginForm()) { %>
		<%= this.Hidden(x => x.Item.Id) %>
		<%= this.Hidden(x => x.Item.Position) %>
		<%= this.TextBox(x => x.Item.Name).Label("Name") %>
		<%= this.CheckBox(x => x.Item.IsActive).Label("Active") %>
		<%= this.Select(x => x.Item.PostZone.Id).Options(Model.GetLookupList<PostZone>(), x => x.Id, x => x.Name).Label("Post Zone")%>
		<input type="submit" value="Save" />
    <% } %>
</asp:Content>
