<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ShopViewData>" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    
    <meta property="og:title" content="<%= Model.Product.Name %>"/>
    <meta property="og:type" content="product"/>
    <meta property="og:url" content="<%= Url.ActionAbsolute<ProductController>(c => c.Item(Model.Product.UrlName)) %>"/>
    <% if (Model.Product.HasMainImage) { %>
    <meta property="og:image" content="<%= Url.ContentAbsolute("~/ProductPhotos/" + Model.Product.MainImage.MainFileName) %>"/>
    <%} %>
    <meta property="og:site_name" content="<%= ((Suteki.Shop.Controllers.ControllerBase)ViewContext.Controller).BaseControllerService.ShopName %>"/>
    <meta property="fb:admins" content="<%= ((Suteki.Shop.Controllers.ControllerBase)ViewContext.Controller).BaseControllerService.FacebookUserId %>"/>
    <meta property="og:description" content="<%= Model.Product.PlainTextDescription %>"/>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<div id="product">

<div class="error"><%= TempData["message"] %></div>

<h1><%= Html.Encode(ViewData.Model.Product.Name) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%= ViewData.Model.Product.Price.ToStringWithSymbol()%><%= ViewData.Model.Product.IsActiveAsString %></h1>

<% if(User.IsAdministrator()) { %>
    <%= Html.ActionLink<ProductController>(c => c.Edit(ViewData.Model.Product.Id), "Edit", new { @class = "linkButton" })%>
    <% Html.PostAction<ProductCopyController>(c => c.Copy(ViewData.Model.Product), "Copy");%>
<%
} %>

<% Html.RenderPartial("ProductDescription", Model.Product); %>
<% Html.RenderPartial("BasketOptions", Model.Product); %>

<p>If an item is out of stock, please email us at 
<a href="mailto:<%= ((Suteki.Shop.Controllers.ControllerBase)this.ViewContext.Controller).BaseControllerService.EmailAddress %>">
<%= ((Suteki.Shop.Controllers.ControllerBase)this.ViewContext.Controller).BaseControllerService.EmailAddress %>
</a>
 so that we can let you know when it will be available.</p>

<% Html.RenderAction<ReviewsController>(c => c.Show(Model.Product.Id)); %>

<% if(User.IsAdministrator()) Html.StockControlUi(Model.Product.UrlName); %>

<div id="fb-root"></div>
<script type="text/javascript">
    window.fbAsyncInit = function () {
        FB.init({ appId: '<%= ((Suteki.Shop.Controllers.ControllerBase)ViewContext.Controller).BaseControllerService.FacebookUserId %>', status: true, cookie: true,
            xfbml: true
        });
    };
    (function () {
        var e = document.createElement('script'); e.async = true;
        e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
        document.getElementById('fb-root').appendChild(e);
    } ());
</script>
</div> 
</asp:Content>
