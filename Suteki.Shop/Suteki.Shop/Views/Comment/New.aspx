<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Suteki.Shop.Comment>" MasterPageFile="~/Views/Shared/Shop.Master" %>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContentPlaceHolder">
<h2>Leave a Comment</h2>
<%= Html.ValidationSummary() %>
<% using(Html.BeginForm<CommentController>(c => c.New())) { %>
	<p><label for="Reviewer">Your Name:</label></p>
	<p><%= Html.TextBoxFor(x => x.Reviewer) %></p>
	<p><label for="Text">Comment:</label></p>
	<p><%= Html.TextAreaFor(x => x.Text) %></p>
    <input id="Id" name="Id" type="hidden" value="0" />
	<input type="submit" value="Submit Comment" />
<% }%>
</asp:Content>