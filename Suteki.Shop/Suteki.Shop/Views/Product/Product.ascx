<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<ShopViewData>" %>
<div onclick="location.href='<%= Url.Action<ProductController>(c=>c.Item(Model.Product.UrlName)) %>'" class="product">
    <div class="cat-label"><%= Html.Encode(Model.Product.Name) %></div>

    <% if(Model.Product.HasMainImage) { %>
        <%= Html.Image("~/ProductPhotos/" + Model.Product.MainImage.ThumbFileName) %>
    <% } %>
    
    <% if(Context.User.IsAdministrator()) { %>
        <br />
        <%= Html.UpArrowLink<ProductController>(c => c.MoveUp(Model.Category.Id, Model.Product.Position)) %>
        <%= Html.DownArrowLink<ProductController>(c => c.MoveDown(Model.Category.Id, Model.Product.Position)) %>
        <%= Html.Tick(Model.Product.IsActive) %>
    <% } %>
</div>
