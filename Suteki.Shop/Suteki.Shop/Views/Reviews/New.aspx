<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<Review>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Add a Review for <%= Html.Encode(Model.Product.Name) %></h2>
	<%= Html.ValidationSummary() %>
	<% using (Html.BeginForm<ReviewsController>(c => c.New(0))) { %>
		<p>Your Name:</p>
		<p>
			<%= this.TextBox(x => x.Reviewer).MaxLength(250) %>
		</p>
		<p>Product Review:</p>
		<p><%= this.TextArea(x => x.Text).Rows(10).Columns(40) %></p>
		<%= Html.HiddenFor(x => x.Product.Id) %>
		<input type="submit" value="Submit Review" />
	<% } %>

</asp:Content>
