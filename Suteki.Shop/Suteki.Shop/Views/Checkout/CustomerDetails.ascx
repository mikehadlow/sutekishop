<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CheckoutViewData>" %>

        <%= Html.HiddenFor(m => m.OrderId)%>
        <%= Html.HiddenFor(m => m.BasketId)%>
        
        <!-- card contact -->

<div class="columnContainer">
    <div class="contentLeftColumn">
        
        <h3>Card Holder</h3>
        
        <table>
            <tr>
                <td class="label"><label for="CardContactFirstName">First Name</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactFirstName)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactLastName">Last Name</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactLastName)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactAddress1">Address</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactAddress1)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactAddress2">&nbsp;</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactAddress2)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactAddress3">&nbsp;</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactAddress3)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactTown">Town / City</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactTown)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactCounty">County</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactCounty)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactPostcode">Postcode</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactPostcode)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactCountry.Id">Country*</label></td>
                <td class="field"><%= Html.ComboFor(m => m.CardContactCountry)%></td>
            </tr>
            <tr>
                <td class="label"><label for="CardContactTelephone">Telephone</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.CardContactTelephone)%></td>
            </tr>
            <tr>
                <td class="label"><label for="Email">Email</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.Email)%></td>
            </tr>
            <tr>
                <td class="label"><label for="EmailConfirm">Confirm Email</label></td>
                <td class="field"><%= Html.TextBoxFor(m => m.EmailConfirm)%></td>
            </tr>
        </table>
    </div>
    <div class="contentRightColumn">
 
        <!-- deliver contact -->
        
        <h3>Delivery Address</h3>
        
        <table>
            <tr>
                <td class="label"><label for="UseCardholderContact">Use Cardholder Details</label></td>
                <td class="field"><%= Html.CheckBoxFor(m => m.UseCardholderContact,
                        new { onclick = "javascript:toggleCardHolderDetails();" })%></td>
            </tr>
        </table>
        
        <div id="deliveryAddress">

            <table>
                <tr>
                    <td class="label"><label for="DeliveryContactFirstName">First Name</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactFirstName)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactLastName">Last Name</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactLastName)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactAddress1">Address</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactAddress1)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactAddress2">&nbsp;</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactAddress2)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactAddress3">&nbsp;</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactAddress3)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactTown">Town / City</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactTown)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactCounty">County</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactCounty)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactPostcode">Postcode</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactPostcode)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactCountry.Id">Country</label></td>
                    <td class="field"><%= Html.ComboFor(m => m.DeliveryContactCountry)%></td>
                </tr>
                <tr>
                    <td class="label"><label for="DeliveryContactTelephone">Telephone</label></td>
                    <td class="field"><%= Html.TextBoxFor(m => m.DeliveryContactTelephone)%></td>
                </tr>
            </table>
        
        </div>
        
        <!-- additional information -->  
        
        <table>
            <tr>
                <td class="label"><label for="AdditionalInformation">Additional Information</label></td>
                <td class="field"><span><%= Html.TextAreaFor(m => m.AdditionalInformation)%></span></td>
            </tr>
        </table>
        
    </div>      
</div>        
* Selecting a different country may result in a different postage charge. You will be able to review this on the next page.