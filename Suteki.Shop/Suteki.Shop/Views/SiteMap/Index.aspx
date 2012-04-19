<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ShopViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1>Site Map</h1>

<div class="siteMap">

    <h3>Products</h3>

    <ul>
    <% foreach(var product in ViewData.Model.Products.InOrder()) {%>
        <li><%= Html.ActionLink<ProductController>(c => c.Item(product.UrlName), product.Name) %></li>
    <% } %>
    </ul>

    <h3>Contents</h3>

    <ul>
    <% foreach(var content in ViewData.Model.Contents.InOrder()) {%>
        <li><%= Html.ActionLink<CmsController>(c => c.Index(content.UrlName), content.Name) %></li>
    <% } %>
    </ul>

</div>

</asp:Content>
