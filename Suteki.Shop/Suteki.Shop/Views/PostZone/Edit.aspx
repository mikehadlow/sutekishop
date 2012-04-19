<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ScaffoldViewData<Suteki.Shop.PostZone>>" %>
<%@ Import Namespace="Suteki.Common.ViewData"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h1>Post Zone</h1>
    
    <%= Html.ValidationSummary() %>
    <%= Html.MessageBox(ViewData.Model) %>

	<% using(Html.BeginForm()) { %>
		<%= this.Hidden(x => x.Item.Id) %>
		<%= this.Hidden(x => x.Item.Position) %>
		<%= this.TextBox(x => x.Item.Name).Label("Name") %>
		<%= this.TextBox(x => x.Item.Multiplier).Format("0.00").Label("Multiplier") %>
		<%= this.CheckBox(x => x.Item.AskIfMaxWeight).Label("Ask If Max Weight") %>
		<%= this.TextBox(x => x.Item.FlatRate).Label("Flat Rate") %>
		<%= this.CheckBox(x => x.Item.IsActive).Label("Active") %>
        
        <input type="submit" value="Save" />
    <% } %>
</asp:Content>
