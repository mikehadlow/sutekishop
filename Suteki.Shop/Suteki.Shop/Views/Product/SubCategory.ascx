<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<Category>" %>
<div onclick="location.href='<%= Url.Action<ProductController>(c=>c.Category(Model.UrlName)) %>'" class="product">
    <div><%= Model.Name %></div>

    <% if(Model.HasMainImage) { %>
        <%= Html.Image("~/ProductPhotos/" + Model.MainImage.ThumbFileName) %>
    <% } %>
    
    <% if (Context.User.IsAdministrator()) { %>
		<br />
		Active: <%= Html.Tick(Model.IsActive) %>
    <% } %>
</div>
