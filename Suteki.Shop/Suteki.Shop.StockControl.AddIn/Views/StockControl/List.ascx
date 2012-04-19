<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Suteki.Shop.StockControl.AddIn.Controllers" %>
<%@ Import Namespace="Suteki.Shop.StockControl.AddIn.Models" %>
<%@ Import Namespace="Microsoft.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<hr />
<h1>Stock</h1>

<% using(Html.BeginForm("Update", "StockControl")){ %>
    <%= Html.Hidden("ReturnUrl", Request.RawUrl) %>
    <table>
      <tr>
        <th class="thin">Size</th>
        <th class="thin">Stock Level</th>
        <th class="thin">In Stock</th>
        <th class="thin">Received</th>
        <th class="thin">Adjustment</th>
      </tr>
    <% var i = 0; %>
    <% foreach (Suteki.Shop.StockControl.AddIn.Models.StockItem stockItem in (IEnumerable)Model) { %>
      <tr>
        <td class="thin"><%= Html.ActionLink<StockControlController>(x => x.History(stockItem.Id), stockItem.SizeName) %></td>
        <td class="thin"><%= stockItem.Level %></td>
        <td class="thin"><%= Html.CheckBox("UpdateItems[" + i + "].IsInStock", stockItem.IsInStock)%></td>
        <td class="thin"><%= Html.TextBox("UpdateItems[" + i + "].Received", "", new { @class = "table" })%></td>
        <td class="thin"><%= Html.TextBox("UpdateItems[" + i + "].Adjustment", "", new { @class = "table" })%></td>
        <%= Html.Hidden("UpdateItems[" + i + "].StockItemId", stockItem.Id)%>
      </tr>
      <% i++; %>
    <% } %>
    </table>
    <label for="Comment">Comment</label>
    <%= Html.TextArea("Comment")%>
    <input type="submit" value="Update" />
<% } %>
