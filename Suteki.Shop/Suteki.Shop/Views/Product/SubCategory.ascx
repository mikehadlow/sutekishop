<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<Category>" %>
<div onclick="location.href='<%= Url.Action<ProductController>(c=>c.Index(Model.Id)) %>'" class="product">
    <div class="cat-label"><%= Model.Name %></div>

    <% if(Model.HasMainImage) { %>
        <%= Html.Image("~/ProductPhotos/" + Model.MainImage.ThumbFileName) %>
    <% } %>
    
    <% if (Context.User.IsAdministrator()) { %>
		<br />
		Active: <%= Html.Tick(Model.IsActive) %>
    <% } %>
</div>
