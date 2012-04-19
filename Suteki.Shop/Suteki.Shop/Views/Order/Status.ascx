<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ShopViewData>" %>
    <p class="hideForPrint"><%= Html.ActionLink<OrderController>(c => c.Print(ViewData.Model.Order.Id), "Printable version of this page") %></p>
<dl>
    <dt>Order Number</dt><dd><%= ViewData.Model.Order.Id.ToString()%>&nbsp;</dd>
    <dt>Date</dt><dd><%= ViewData.Model.Order.CreatedDate.ToShortDateString()%></dd>

<% if(ViewContext.HttpContext.User.IsAdministrator()) { %>
    <dt>Status</dt><dd><%= ViewData.Model.Order.OrderStatus.Name %> <%= ViewData.Model.Order.DispatchedDateAsString %></dd>
    <dt>Updated by</dt><dd><%= ViewData.Model.Order.UserAsString %></dd>
<% } %>
    
</dl>

<div class="orderAction hideForPrint">
<% if(ViewContext.HttpContext.User.IsAdministrator()) { %>
    <% if(ViewData.Model.Order.IsCreated) { %>
        <% Html.PostAction<OrderStatusController>(c => c.Dispatch(Model.Order), "Dispatch"); %>
        <% Html.PostAction<OrderStatusController>(c => c.Reject(Model.Order), "Reject"); %>
    <% } else { %>
        <% Html.PostAction<OrderStatusController>(c => c.UndoStatus(Model.Order), "Reset Status"); %>
    <% } %>
    <%= Html.ActionLink<InvoiceController>(c => c.Show(ViewData.Model.Order.Id), "Print Invoice", new { @class = "linkButton" }) %>
<% } %>
</div>    

