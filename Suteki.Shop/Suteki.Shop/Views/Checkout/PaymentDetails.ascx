<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CheckoutViewData>" %>
<%@ Import Namespace="Suteki.Common.Models"%>
        
<div class="columnContainer">
    <div class="contentLeftColumn">
        
        <div id="cardDetails">
			<%= Html.Image("~/content/images/creditcards.gif", "Credit Cards") %>
			
            <table>
                <tr>
                    <td class="label"><label for="CardCardType.Id">Card Type</label></td>
                    <td colspan="2" class="field"><%= Html.ComboFor(m => m.CardCardType)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="CardHolder">Card Holder</label></td>
                    <td colspan="2" class="field"><%= Html.TextBoxFor(m => m.CardHolder)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="CardNumber">Card Number</label></td>
                    <td colspan="2" class="field"><%= Html.TextBox("CardNumber", "", new { autocomplete = "off" })%></td>
                </tr>
                <tr>
                    <td class="label"><label for="CardExpiryMonth">Expire Date</label></td>
                    <td class="field small"><%= Html.DropDownList("CardExpiryMonth", Card.Months.ToStringValues().AsSelectList(ViewData.Model.CardExpiryMonth))%></td>
                    <td class="field small"><%= Html.DropDownList("CardExpiryYear", Card.ExpiryYears.ToStringValues().AsSelectList(ViewData.Model.CardExpiryYear))%></td>
                </tr>
                <tr>
                    <td class="label"><label for="CardStartMonth">Start Date</label><br />If present on your card</td>
                    <td class="field small"><%= Html.DropDownList("CardStartMonth", Card.Months.ToStringValues().AddBlankFirstValue().AsSelectList(ViewData.Model.CardStartMonth))%></td>
                    <td class="field small"><%= Html.DropDownList("CardStartYear", Card.StartYears.ToStringValues().AddBlankFirstValue().AsSelectList(ViewData.Model.CardStartYear))%></td>
                </tr>
                <tr>
                    <td class="label"><label for="CardIssueNumber">Issue Number</label><br />If present on your card</td>
                    <td colspan="2" class="field small"><%= Html.TextBoxFor(m => m.CardIssueNumber, new { maxlength = "1" })%></td>
                </tr>
                <tr>
                    <td class="label"><label for="CardSecurityCode">Security Code</label></td>
                    <td colspan="2" class="field small"><%= Html.TextBox("CardSecurityCode", "", new { maxlength = "4" })%></td>
                </tr>
                <tr>
                    <td colspan="3"><p>The last three digits printed on the signature strip of your credit/debit card. Or for Amex. the four digits printed on the front of the card above the embossed card number.</p></td>
                </tr>
            </table>
        </div>
    </div>
    <div class="contentRightColumn">
    
        <label for="PayByTelephone"><strong>I prefer to pay by cheque or telephone</strong></label>
        <%= Html.CheckBoxFor(m => m.PayByTelephone,
                        new { onclick = "javascript:toggleCard();" })%>
    
        <p>If you tick this option you will receive an order number. You should quote this number when you contact us with your payment. We accept most major credit and debit cards including Amex. We also accept cheques( in pounds sterling from a UK bank only) and postal orders made payable to ‘Jump the Gun’ Please note that cheques will have to await clearance before we can dispatch goods- this can take six working days.</p>
        <p>All credit card details are collected using an SSL secure server which means that any sensitive information is securely encrypted and cannot be read by any unauthorised party. We do not store any of your credit card details once payment has been taken. We will not take payment until your goods are ready for dispatch.</p>
    </div>
</div> 
<div class="clear" />       

<label style="display:inline" for="ContactMe">Would you like to be added to our mailing list? We will not share your contact information with 3rd parties.</label>
<%= Html.CheckBoxFor(m => m.ContactMe)%>