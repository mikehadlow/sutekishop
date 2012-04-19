<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ShopViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<%= Html.MessageBox(ViewData.Model)%>
	
	<p><%= Html.ActionLink<CategoryController>(c => c.New(1), "New Category")%></p>
	
	<%= Html.WriteCategories(ViewData.Model.CategoryViewData, CategoryDisplay.Edit)%>
</asp:Content>
