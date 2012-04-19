<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ShopViewData>" %>
<h3>Payment Details</h3>
<div class="columnContainer">
    <div class="contentLeftColumn">

        <% if(ViewData.Model.Order.PayByTelephone) { %>
        
        <p>Pay By Telephone</p>
        
        <% } else { %>

        <dl>
            <dt>Card Type</dt><dd><%= ViewData.Model.Order.Card.CardType.Name %>&nbsp;</dd>
            <dt>Card Holder</dt><dd><%= ViewData.Model.Order.Card.Holder %>&nbsp;</dd>
        </dl>
        
            <% if(ViewContext.HttpContext.User.IsAdministrator()) { %>

                <%= Html.ErrorBox(ViewData.Model) %>

                <% if (ViewData.Model.Card == null) { %>

                    <% using (Html.BeginForm("ShowCard", "Order", FormMethod.Post,
                           new Dictionary<string, object> { { "onsubmit", "submitHandler();" } }))
                       { %>
                        
                        <%= Html.Hidden("orderId", ViewData.Model.Order.Id.ToString()) %>
                        
                        <label for="privateKey">Private Key</label>
                        <%= Html.TextBox("privateKey")%>
                        
                        <%= Html.SubmitButton("cardDetailsSubmit", "Get Card Details")%>

                    <% } %>
                
                <% } else { %>
                
                    <dl>
                        <dt>Card Number</dt><dd><%= ViewData.Model.Card.CardNumberAsString %></dd>
                        <dt>Issue Number</dt><dd><%= ViewData.Model.Card.IssueNumber %></dd>
                        <dt>Security Code</dt><dd><%= ViewData.Model.Card.SecurityCode %></dd>
                        <dt>Start Date</dt><dd><%= ViewData.Model.Card.StartDateAsString %></dd>
                        <dt>Expiry Date</dt><dd><%= ViewData.Model.Card.ExpiryDateAsString %></dd>
                    </dl>
                
                <% } %>

            <% } %>        
        
        <% } %>
        
    </div>
    <div class="contentRightColumn">

    </div>
</div>

