<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="System.Web.Mvc.ViewPage<ShopViewData>" %>
<%@ Import Namespace="MvcContrib.UI.Pager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Mailing List</h2>
	<%= Html.MessageBox(Model) %>


	<ul>
		<li><%= Html.ActionLink<ReportController>(c => c.MailingListSubscriptions(), "Mailing List Subscriptions") %></li>
		<li><%= Html.ActionLink<ReportController>(c => c.MailingListEmails(), "Mailing List Subscriptions (Emails Only)") %></li>
	</ul>
	
	<%= Html.Grid(Model.MailingListSubscriptions).Columns(column => {
			column.For(x => x.Contact.Firstname);
			column.For(x => x.Contact.Lastname);
			column.For(x => x.DateSubscribed).Format("{0:d}");
			column.For(x => x.Email);
			column.For(x => x.Contact.Postcode);
			column.For(x => Html.ActionLink<MailingListController>(c => c.Edit(x.Id), "Edit")).Encode(false).Named("");	
		}) %>
	
	<%= Html.Pager(Model.MailingListSubscriptions) %>
</asp:Content>
