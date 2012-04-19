<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1>Reports</h1>

<ul>
	<li><%= Html.ActionLink<ReportController>(c => c.Orders(), "Orders") %></li>
	<li><%= Html.ActionLink<ReportController>(c => c.MailingListSubscriptions(), "Mailing List Subscriptions") %></li>
	<li><%= Html.ActionLink<ReportController>(c => c.MailingListEmails(), "Mailing List Subscriptions (Emails Only)") %></li>
</ul>


</asp:Content>
