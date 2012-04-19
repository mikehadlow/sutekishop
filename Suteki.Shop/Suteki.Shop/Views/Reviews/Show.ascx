<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Product>" %>

<%= Html.ActionLink<ReviewsController>(c => c.New(Model.Id), "Leave a Review") %>&nbsp;&nbsp;

<% if (Model.Reviews.Count() > 0) { %>

<a id="show-reviews" href="javascript:void(0)">Show Reviews (<%= Model.ActiveReviews.Count()%>)</a>

<div id="reviews" style="display: none;">
	<p>&nbsp;</p>
	<% foreach (var review in Model.ActiveReviews) { %>
		<div>
			<!--<%= Html.Stars(review.Rating) %> -->
			<strong><%= Html.Encode(review.Reviewer) %></strong>
			<% if(Context.User.IsAdministrator()) { %>
				<% using(Html.BeginForm<ReviewsController>(c => c.Delete(review.Id), FormMethod.Post, new{ style="display:inline;" })) { %>
					<img src="<%= Url.Content("~/Content/Images/cross.png") %>" alt="delete" class="delete-review pointer" />
				<% } %>
			<% } %>
		</div>
		<p>
			<%= Html.Encode(review.Text) %>
		</p>
	<% } %>
</div>

<script type="text/javascript">
	$(function() {
		$('#show-reviews').click(function() {
			$('#reviews').toggle('slide');
		});
		$('.delete-review').click(function() {
			if (confirm('Are you sure you want to delete this review?')) {
				$(this).parent().submit();
			}
		});
	});
</script>

<% } %>