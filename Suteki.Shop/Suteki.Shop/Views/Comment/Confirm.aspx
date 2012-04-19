<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Shop.Master" %>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContentPlaceHolder">
<h2>Comment Submitted</h2>
<p>Thank you for your comment, it will be reviewed before being visible on the comments page</p>
<p><%= Html.ActionLink<ReviewsController>(c => c.AllApproved(), "Return to comments") %></p>
</asp:Content>