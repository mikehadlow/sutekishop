<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<ShopViewData>" %>
<h3>Note</h3>
<% if (Model.IsPrint)
   { %>
    <%= Model.Order.Note %>    
<% }
   else
   { %>
    <% using (Html.BeginForm<OrderController>(c => c.UpdateNote(null)))
       { %>
	    <%= this.Hidden(x => x.Order.Id)%>
	    <%= this.TextArea(x => x.Order.Note).Rows(2).Columns(40)%>
	    <input type="submit" value="Update Note" />
    <% } %>
<% } %>