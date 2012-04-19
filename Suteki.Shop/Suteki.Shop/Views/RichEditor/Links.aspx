<%@ Page ContentType="application/x-javascript" Language="C#" Inherits="System.Web.Mvc.ViewPage<ShopViewData>" %>
var tinyMCELinkList = new Array();
<% foreach (var product in Model.Products) { %>
	tinyMCELinkList.push(["Product - <%= product.Name %>", "<%= Url.Action<ProductController>(c => c.Item(product.UrlName))%>"]);
<% } %>
<% foreach (var category in Model.Categories) { %>
	tinyMCELinkList.push(["Category - <%= category.Name %>", "<%= Url.Action<ProductController>(c => c.Index(category.Id))%>"]);	
<% } %>
<% foreach (var content in Model.Contents) { %>
	tinyMCELinkList.push(["Content - <%= content.Name %>", "<%= content.Url(Url) %>"]);	
<% } %>