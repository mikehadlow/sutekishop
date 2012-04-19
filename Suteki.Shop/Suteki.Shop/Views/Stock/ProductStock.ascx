<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Product>" %>
<div class="productOptions">
<% using (Html.BeginForm("ProductStockUpdate", "Stock"))
   { %>
    <%= Html.HiddenFor(p => p.Id) %>
    <table>
    <% for (var i=0; i<ViewData.Model.Sizes.Count; i++){ 
        if(!Model.Sizes[i].IsActive) continue; %>
        <tr>
            <td><%= Html.CheckBoxFor(p => p.Sizes[i].IsInStock) %></td>
            <td><%= Model.Sizes[i].Name %></td>
        </tr>
    <%} %>
    </table>
    <input type="submit" value="Update Stock" />
<%} %>
</div>