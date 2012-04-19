<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ShopViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1>Stock</h1>

<div class="columnContainer">

<% using(Html.BeginForm()) { %>

    <%= Html.WriteStock(Model.Category) %>

    <%= Html.SubmitButton() %>

<% } %>

</div>

<script type="text/javascript">
	$(function() {
		$('input[type=checkbox]').click(updateTickboxesWithSameName);
	});

	function updateTickboxesWithSameName() {
		var name = $(this).attr('name');
		var checked = this.checked;
		$('input[name=' + name + ']').attr('checked', checked);
	}
</script>

</asp:Content>
