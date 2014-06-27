<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<Order>>" %>
<%@ Import Namespace="MvcContrib"%>
<% if (Model.Count > 0)
   { %>
<div>
    <p>Potentially related problem orders</p>
    <table>
	<%= Html.Grid(Model).Columns(column =>
	    {
	        column.For(x => Html.ActionLink<OrderController>(c => c.Item(x.Id), x.Id.ToString()))
	            .Encode(false).Named("Number").HeaderAttributes(@class => "thin");
	        column.For(x => x.CardContact.Fullname).Named("Customer").HeaderAttributes(@class => "wide");
	        column.For(x => x.CreatedDate.ToShortDateString()).Named("Created").HeaderAttributes(@class => "thin");
	        column.For(x => x.DispatchedDateAsString).Named("Dispatched").HeaderAttributes(@class => "thin").Encode(false);
	        column.For(x => x.OrderStatus.Name).Named("Status").HeaderAttributes(@class => "thin");
	        column.For(x => x.UserAsString).Named("Updated by").HeaderAttributes(@class => "thin").Encode(false);
	    }).RowAttributes(row => new Hash(@class => row.Item.OrderStatus.Name)) %>

    </table>
</div>
<% } %>