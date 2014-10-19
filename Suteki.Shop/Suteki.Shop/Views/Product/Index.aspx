<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master"  Inherits="Suteki.Shop.ViewPage<ShopViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<div id="product-list">
<h1><%= Model.Category.Name %></h1>

<% if(User.IsAdministrator()) { %>
    <p><%= Html.ActionLink<CategoryController>(c => c.New(ViewData.Model.Category.Id), "New Category")%></p>
    <p><%= Html.ActionLink<ProductController>(c => c.New(ViewData.Model.Category.Id), "New Product") %></p>
<% } %>
<% if(Model.Category.Image != null) { %>
    <div class="categoryImage">
	 <%= Html.Image("~/ProductPhotos/" + Model.Category.Image.CategoryFileName, "Category Image") %>
	 </div>
<% } %>

<% foreach (var category in ViewData.Model.Category.Categories.ActiveFor((User)User)) { %>
	<% Html.RenderPartial("SubCategory", category); %>
<% } %>

<% foreach (var productCategory in ViewData.Model.ProductCategories) { %>
	<% Html.RenderPartial("Product", ShopView.Data.WithProductCategory(productCategory).WithCategory(Model.Category)); %>
<% } %>

</div>
</asp:Content>
