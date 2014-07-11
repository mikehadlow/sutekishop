<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<Product>" %>
<div class="productOptions">
<% using (Html.BeginForm<BasketController>(c=>c.Update(null, null))) { %>
    <% if(Model.HasSize) { %>
        <label for="basketItem_size_Id">Size</label>
        <%= Html.DropDownList("basketItem.size.Id", new SelectList(Model.Sizes.Active(), "Id", "NameAndStock" ))%>
    <% } else { %>
        <%= Html.Hidden("basketItem.size.Id", Model.DefaultSize.Id.ToString()) %>
        <label><%= Model.DefaultSize.NameAndStock %></label>
    <% } %>

    <label for="quantity">Quantity</label>
    <%= Html.DropDownList("basketItem.Quantity", new SelectList(1.To(50).Select(i => new { Value = i }), "Value", "Value")) %>
    <%= Html.SubmitButton("addToBasket", "Add to basket")%>
<% } %>
</div>