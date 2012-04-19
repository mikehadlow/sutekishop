<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<ShopViewData>" %>
<% using(Html.BeginForm()) { %>

    <div class="contentLeftColumn">
		<%= this.TextBox(x => x.OrderSearchCriteria.OrderId).Label("Order Number") %>
		<%= this.TextBox(x => x.OrderSearchCriteria.Email).Label("Email") %>
		<%= this.TextBox(x => x.OrderSearchCriteria.Lastname).Label("Last Name") %>

		<input type="submit" value="Search" />
    </div>

    <div class="contentRightColumn">
		<%= this.TextBox(x => x.OrderSearchCriteria.Postcode).Label("Postcode") %>
		<%= this.Select(x => x.OrderSearchCriteria.OrderStatusId).Label("Status").Options(Model.OrderStatuses, x => x.Id, x => x.Name) %>
        <br /><br /><br /><br />
    </div>

<% } %>