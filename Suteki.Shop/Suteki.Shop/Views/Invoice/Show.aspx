<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Print.Master" Inherits="System.Web.Mvc.ViewPage<ShopViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<div class="invoiceHeader">
    <img src="http://static.jumpthegun.co.uk.s3.amazonaws.com/invoice_header.png" />
</div>

<table class="invoiceTable">
    <tr>
        <td>
            LB International Ltd,<br />
            28a North Road,<br />
            Brighton,<br />
            BN1 1YB,<br />
            UK.<br /><br />
            Tel: 01273 626 333,<br /><br />  
            info@jumpthegun.co.uk<br />
        </td>
        <td>  
            <span class="invoiceNumber">Invoice No. <%= ViewData.Model.Order.Id.ToString() %></span> <br /><br />
            <%= DateTime.Now.ToLongDateString() %><br /><br />
            <% foreach(var line in ViewData.Model.Order.CardContact.GetAddressLines()) { %>
                <%= line %><br />
            <% } %>
        </td>
    </tr>
</table>

<div class="invoiceBasket">
<table>
    <tr>
        <th class="wide">Product</th>
        <th class="wide number">Quantity</th>
        <th class="wide number">Unit Price</th>
        <th class="wide number">Total Price</th>
    </tr>
    
    <tr></tr>
    
    <% foreach (var orderLine in ViewData.Model.Order.OrderLines)
       { %>
    
    <tr>
        <td><%= orderLine.ProductName%></td>
        <td class="number"><%= orderLine.Quantity%></td>
        <td class="number"><%= orderLine.Price.ToStringWithSymbol()%></td>
        <td class="number"><%= orderLine.Total.ToStringWithSymbol()%></td>
    </tr>
    
    <% } %>
    
    <% foreach (var adjustment in ViewData.Model.Order.Adjustments)
       { %>
    
    <tr>
        <td colspan="3"><%= adjustment.Description %></td>
        <td class="number"><%= adjustment.Amount.ToStringWithSymbol()%></td>
    </tr>
    
    <% } %>

    <tr class="total">
        <td>Total</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="number"><%= ViewData.Model.Order.Total.ToStringWithSymbol()%></td>
    </tr>

    <tr>
        <td>Postage</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="number"><%= ViewData.Model.Order.PostageTotal%></td>
    </tr>

    <tr>
        <td colspan="4">(<%= ViewData.Model.Order.PostageDescription %>)</td>
    </tr>

    <tr class="total">
        <td>Total With Postage</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="number"><%= ViewData.Model.Order.TotalWithPostage%></td>
    </tr>
    
</table>
<div class="invoiceThanks">Paid with Thanks</div>
</div>

<p>Registered in England No. 2341339 Vat No: 505 0466 78</p>

<p>Please use the sticker below if you need to send us anything. (stamps are neccessary).</p>

<div class="invoiceLabelLeft">

    <ul>
        <td>            
            <% foreach(var line in ViewData.Model.Order.PostalContact.GetAddressLines()) { %>
                <li><%= line %>&nbsp;</li>
            <% } %>
        </td>
    </ul>

</div>

<div class="invoiceLabelRight">

    <ul>
        <li>Jump the Gun</li>
        <li>28a North Road,</li>
        <li>Brighton,</li>
        <li>BN1 1YB,</li>
        <li>UK.</li>
    </ul>

</div>

</asp:Content>
