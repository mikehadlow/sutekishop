<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<Product>" %>
<div class="productDescription">
	<div class="mainImage">
	<% if(Model.HasMainImage) { %>
		<%= Html.Image("~/ProductPhotos/" + Model.MainImage.MainFileName, new { id = "mainImage" })%>
	<% } %>
	</div>

	<div class="imageList">
	<% foreach(var productImage in Model.ProductImages.InOrder()) { %>
		<%= Html.Image("~/ProductPhotos/" + productImage.Image.ThumbFileName, new { onclick = "onThumbnailClick(this)" })%>
	<% } %>
	</div>

	<p><%= Model.Description %></p>
    <p><a href="mailto:?subject=<%= Model.Name %>&body=<%= "http://" + Request.Url.Host + Url.Action("Item", new{ urlName = Model.UrlName }) %>">Email to a friend</a></p>

<%--    <div class="clear"></div>--%>

    <fb:like width="450" height="80" colorscheme="dark"/>
</div>