<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Basket>" %>
<table>
    <tr>
        <th class="wide">Product</th>
        <th class="thin">Size</th>
        <th class="thin number">Quantity</th>
        <th class="thin number">Unit Price</th>
        <th class="thin number">Total Price</th>
    </tr>
    
    <% foreach (var basketItem in Model.BasketItems) { %>
    
    <tr>
        <td><%= Html.ActionLink<ProductController>(c => c.Item(basketItem.Size.Product.UrlName), basketItem.Size.Product.Name)%></td>
        <td><%= basketItem.Size.Name%></td>
        <td class="number"><%= basketItem.Quantity%></td>
        <td class="number"><%= basketItem.Size.Product.Price.ToStringWithSymbol()%></td>
        <td class="number"><%= basketItem.Total.ToStringWithSymbol()%></td>
    </tr>
    
    <% } %>
    
    <tr class="total">
        <td>Total</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="number"><%= Model.Total.ToStringWithSymbol()%></td>
    </tr>

    <% Html.RenderAction<PostageDetailController>(c => c.ReadOnly(Model.Id)); %>
    
</table>