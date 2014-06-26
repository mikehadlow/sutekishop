<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<ShopViewData>" %>
<h3>Tracking Number</h3>
<% if (Model.IsPrint)
   { %>
    <%= Model.Order.TrackingNumber %>    
<% }
   else
   { %>
    <% using (Html.BeginForm<OrderController>(c => c.UpdateTrackingNumber(null)))
       { %>
	    <%= this.Hidden(x => x.Order.Id)%>
	    <%= this.TextArea(x => x.Order.TrackingNumber).Rows(1).Columns(40)%>
	    <input type="submit" value="Update Tracking Number" />
    <% } %>
<% } %>