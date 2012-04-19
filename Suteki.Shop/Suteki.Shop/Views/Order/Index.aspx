<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ShopViewData>" %>
<%@ Import Namespace="MvcContrib"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1>Orders</h1>

<div class="columnContainer">
	<% Html.RenderPartial("OrderSearchForm"); %>
</div>

<div class="clearRight">&nbsp;</div>

<div class="columnContainer">
    <div class="pager">
		<%= Html.Pager("Order", "Index", ViewData.Model.Orders)%>
    </div>

	<%= Html.Grid(Model.Orders).Columns(column => {
			column.For(x => Html.ActionLink<OrderController>(c => c.Item(x.Id), x.Id.ToString()))
				.Encode(false).Named("Number").HeaderAttributes(@class => "thin");
			column.For(x => x.CardContact.Fullname).Named("Customer").HeaderAttributes(@class => "wide");
			column.For(x => x.CreatedDate.ToShortDateString()).Named("Created").HeaderAttributes(@class => "thin");
			column.For(x => x.DispatchedDateAsString).Named("Dispatched").HeaderAttributes(@class => "thin").Encode(false);
			column.For(x => x.OrderStatus.Name).Named("Status").HeaderAttributes(@class => "thin");
			column.For(x => x.UserAsString).Named("Updated by").HeaderAttributes(@class => "thin").Encode(false);
	}).RowAttributes(row => new Hash(@class => row.Item.OrderStatus.Name)) %>

    <p>Total orders: <%= ViewData.Model.Orders.TotalCount %></p>
</div>
</asp:Content>
