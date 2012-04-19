<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<Basket>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1>Basket</h1>

<%= Html.ValidationSummary() %>

<% if(Model.IsEmpty) { %>

    <p>Your basket is empty</p>

<% } else { %>
<form method="post" action="<%= Url.Action<BasketController>(c => c.GoToCheckout(null)).ToSslUrl() %>" id="basketForm">
    <table>
        <tr>
            <th class="wide">Product</th>
            <th class="thin">Size</th>
            <th class="thin number">Quantity</th>
            <th class="thin number">Unit Price</th>
            <th class="thin number">Total Price</th>
            <th class="thin number">Delete</th>
        </tr>
        
        <% foreach (var basketItem in Model.BasketItems) { %>
        
        <tr>
            <td><%= Html.ActionLink<ProductController>(c => c.Item(basketItem.Size.Product.UrlName), basketItem.Size.Product.Name)%></td>
            <td><%= basketItem.Size.Name%></td>
            <td class="number"><%= basketItem.Quantity%></td>
            <td class="number"><%= basketItem.Size.Product.Price.ToStringWithSymbol()%></td>
            <td class="number"><%= basketItem.Total.ToStringWithSymbol()%></td>
            <td class="number"><%= Html.ActionLink<BasketController>(c => c.Remove(basketItem.Id), "X")%></td>
        </tr>
        
        <% } %>
        
        <tr class="total">
            <td>Total</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="number"><%= Model.Total.ToStringWithSymbol()%></td>
            <td>&nbsp;</td>
        </tr>

        <% Html.RenderAction<PostageDetailController>(c => c.Index(Model.Id)); %>

    </table>

    <p>The default postage & package charge displayed is for UK postal deliveries. If you select a delivery address outside the UK please check this price again.</p>

	<input type="submit" value="Checkout" />
</form>
<% } %>

<script type="text/javascript">
	$(function() {
	    $('#Country_Id').change(function () {
			$('#basketForm').attr('action', '<%= Url.Action<BasketController>(c => c.UpdateCountry(null)) %>').submit();
		});
	});
</script>

</asp:Content>
