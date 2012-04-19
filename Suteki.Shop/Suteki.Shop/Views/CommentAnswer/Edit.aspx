<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Suteki.Shop.ViewData.CommentAnswerViewData>" MasterPageFile="~/Views/Shared/Shop.Master" %>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContentPlaceHolder"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContentPlaceHolder">
<h2>Edit Comment Answer</h2>
	<p>
        <strong><%= Html.Encode(Model.Reviewer) %></strong> 
	</p>
	<p>
		<%= Html.Encode(Model.Text)%>
	</p>
    <% using(Html.BeginForm<CommentAnswerController>(c => c.Edit(null))) { %>
	    <p><label for="Answer">Answer:</label></p>
	    <p><%= Html.TextAreaFor(x => x.Answer) %></p>
        <%= Html.HiddenFor(x => x.CommentId) %>
	    <input type="submit" value="Submit Reply" />
    <% }%>
</asp:Content>
