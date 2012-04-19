<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<IComment>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Unapproved Reviews</h2>
    <% foreach(var comment in Model) { %>
    <div>
		<p>
        <%
            var review = comment.CastAs<Review>();
            if (review != null) { %>
			<strong><%= Html.ActionLink<ProductController>(c => c.Item(review.Product.UrlName), review.Product.Name)%></strong> reviewed by 
        <%} %>
            <strong><%= Html.Encode(comment.Reviewer) %></strong> 
		</p>
		<p>
			<%= Html.Encode(comment.Text)%>
		</p>
        <% if (comment.HasAnswer) { %>
        <p>
            <strong>Our Reply: </strong><%= Html.Encode(comment.Answer) %>
        </p>
        <% } %>
		
		<% using (Html.BeginForm<CommentAnswerController>(c => c.Edit(comment.Id), FormMethod.Get)) { %>
			<input type="submit" value="Reply" />
		<% } %>
		
		<% using (Html.BeginForm<ReviewsController>(c => c.Approve(comment.Id))) { %>
			<input type="submit" value="Approve" />
		<% } %>
		
		<% using (Html.BeginForm<ReviewsController>(c => c.Delete(comment.Id))) { %>
			<input type="submit" value="Delete" />
		<% } %>
		
		<hr />
	</div>
    <% } %>
    
    <% if (Model.Count() == 0) { %>
		<p>There are no outstanding reviews.</p>
    <% } %>
</asp:Content>