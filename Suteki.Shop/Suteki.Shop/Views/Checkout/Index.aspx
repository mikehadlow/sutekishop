<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master"  Inherits="Suteki.Shop.ViewPage<CheckoutViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<script type="text/javascript" language="javascript">

var first = true;

function toggleCardHolderDetails()
{
    var useCardholderContactCheck = document.getElementsByName("UseCardholderContact")[0];
    var deliveryAddress = document.getElementById("deliveryAddress");
    toggleVisibilityWithCheckbox(useCardholderContactCheck, deliveryAddress);
}

function toggleCard()
{
    var paybytelephone = document.getElementsByName("PayByTelephone")[0];
    var cardDetails = document.getElementById("cardDetails");
    toggleVisibilityWithCheckbox(paybytelephone, cardDetails);
}

function toggleVisibilityWithCheckbox(checkbox, div)
{
    if(checkbox.checked)
    {
        div.style.visibility = "hidden";
    }
    else
    {
        div.style.visibility = "visible";
    }
}

function updatePostageOnUseCardholderDetailsChange(checkbox)
{
    if(first)
    {
        first = false;
        return;
    }

    var select;
    if(checkbox.checked)
    {
        select = document.getElementById("CardContactCountry_Id");
    }
    else
    {
        select = document.getElementById("DeliveryContactCountry_Id");
    }
    updateSelectedCountry(select);
}

function updateSelectedCountry(select)
{
    var useCardholderContactCheck = document.getElementsByName("UseCardholderContact")[0];
    
    if((!useCardholderContactCheck.checked && select.id) == "CardContactCountry_Id") return;
    
    for(var i = 0; i < select.options.length; i++)
    {
        if(select.options[i].selected)
        {
            alert("Postage will be updated for " + select.options[i].text);
            
            var form = document.getElementById("mainForm");
            
            var url = <%= "\"" + Url.RouteUrl(new { Controller = "Checkout", Action = "UpdateCountry" }) + "\"" %>
                 
            form.action = url;
            form.submit();
        }
    }
}

function addHandlers()
{
    var cardcontactCountryid = document.getElementById("CardContactCountry_Id");
    cardcontactCountryid.onchange = function() { updateSelectedCountry(this); }
    
    var deliverycontactCountryid = document.getElementById("DeliveryContactCountry_Id");
    deliverycontactCountryid.onchange = function() { updateSelectedCountry(this); }
}

</script>

    <h1>Checkout</h1>
	<%= Html.ValidationSummary() %>
    
    <div class="inline_image">
    <%= Html.Image("~/Content/Images/SSL_Secured.png", "SSL Secured")%>
    </div>
    <div>
    <p>Welcome to our secure payment page. Please check your order and fill in the information below to place your order. For security puposes your information will be encrypted and once your order has been processed any credit card information will be destroyed.</p>
    <p>The default postage & package charge displayed is for UK postal deliveries. If you select a delivery address outside the UK please check this price again.</p>
    <p>&nbsp;</p>
    </div>
    
    <%= Html.ErrorBox(ViewData.Model)%>
    <%= Html.MessageBox(ViewData.Model)%>

<!-- basket view -->

    <h3>Order Details</h3>
	<% Html.RenderAction<BasketController>(c => c.Readonly(ViewData.Model.BasketId)); %>

<!-- addresses -->
	
	<% using (Html.BeginForm<CheckoutController>(c => c.Index(null), FormMethod.Post, new { name = "mainForm", id = "mainForm" })) { %>
	
	<h3>Customer Details</h3>
	<% Html.RenderPartial("CustomerDetails"); %>

	<h3>Payment Details</h3>     
	<% Html.RenderPartial("PaymentDetails"); %>

	<%= Html.SubmitButton("submitButton", "Continue")%>
	
	<% } %>

<script type="text/javascript">

toggleCardHolderDetails();
toggleCard();
addHandlers();

</script>

</asp:Content>
