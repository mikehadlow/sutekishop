<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<Product>" %>
<div onclick="location.href='<%= Url.Action<ProductController>(c=>c.Item(Model.UrlName)) %>'" class="product">
    <div><%= Html.Encode(Model.Name) %></div>

    <% if(Model.HasMainImage) { %>
        <%= Html.Image("~/ProductPhotos/" + Model.MainImage.ThumbFileName) %>
    <% } %>
</div>
