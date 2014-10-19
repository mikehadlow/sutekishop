<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<ShopViewData>" %>
<div onclick="location.href='<%= Url.Action<ProductController>(c=>c.Item(Model.ProductCategory.Product.UrlName)) %>'" class="product">
    <div><%= Html.Encode(Model.ProductCategory.Product.Name) %></div>

    <% if (Model.ProductCategory.Product.HasMainImage)
       { %>
        <%= Html.Image("~/ProductPhotos/" + Model.ProductCategory.Product.MainImage.ThumbFileName) %>
    <% } %>
    
    <% if(Context.User.IsAdministrator()) { %>
        <br />
        <%= Html.UpArrowLink<ProductController>(c => c.MoveUp(Model.Category.Id, Model.ProductCategory.Position)) %>
        <%= Html.DownArrowLink<ProductController>(c => c.MoveDown(Model.Category.Id, Model.ProductCategory.Position)) %>
        <%= Html.Tick(Model.ProductCategory.Product.IsActive) %>
    <% } %>
</div>
