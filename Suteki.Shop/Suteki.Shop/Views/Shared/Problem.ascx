<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<ShopViewData>" %>
<% if (!Model.IsPrint)
   { %>
    <% using (Html.BeginForm<OrderController>(c => c.UpdateProblemCustomer(null)))
       { %>
	   <%= this.Hidden(x => x.Order.Id)%>
       <table>
            <tr>
                <td class="label"><label for="Problem">Problem Customer</label></td>
                <td class="field"><%= Html.CheckBoxFor(m => m.Order.Problem,
                        new { onChange = "this.form.submit()" })%></td>
            </tr>
        </table>
    <% } %>
<% } %>