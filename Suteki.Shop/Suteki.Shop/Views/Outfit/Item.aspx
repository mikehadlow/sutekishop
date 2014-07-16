<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master"  Inherits="Suteki.Shop.ViewPage<Outfit>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<div style="overflow: hidden; zoom: 1;">    <h1><%= Model.Name %></h1>

    <% if(User.IsAdministrator()) { %>
        <%= Html.ActionLink<OutfitController>(c => c.Edit(ViewData.Model.Id), "Edit", new { @class = "linkButton" })%>
    <% } %>

    <div>
        
        <div class="mainImage">
            <% if(Model.HasMainImage) { %>
                <% if (Model.HasOriginalImages) { %>
                <a href="javascript:onMainPictureClick()">
                    <%= Html.Image("~/ProductPhotos/" + Model.MainImage.MainFileName,new { id = "mainImage" })%><br/>
                    Click to enlarge
                </a>
                <% } else { %>
                    <%= Html.Image("~/ProductPhotos/" + Model.MainImage.MainFileName,new { id = "mainImage" })%><br/>
                <% } %>
            <% } %>
            
        </div>

        <div class="imageList">
        <% foreach(var productImage in Model.OutfitImages.InOrder()) { %>
            <%= Html.Image("~/ProductPhotos/" + productImage.Image.ThumbFileName, new { onclick = "onThumbnailClick(this)" })%>
        <% } %>
        </div>

    </div>
    <div class="clear"></div>

    <div><%= Model.Description %></div>
    
    <h3>Get the look:</h3>

    <% foreach (var outfitProduct in ViewData.Model.OutfitProducts) { %>
        <% Html.RenderPartial("Product", outfitProduct.Product); %>
    <% } %>
    
    <fb:like width="450" height="80" colorscheme="dark"/>
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
    
<script type="text/javascript">

	var photoUrls = [<%= string.Join(",", Model.OutfitImages.InOrder().Select(x => "\"" + Url.Content("~/ProductPhotos/" + x.Image.FileNameAsString) + "\"").ToArray()) %>];

    function onMainPictureClick() {
        $.fancybox.open(photoUrls, {
                openEffect: 'none',
                closeEffect: 'none',
                nextEffect: 'none',
                prevEffect: 'none'});
    }

    function onThumbnailClick(img) {
        var mainImage = document.getElementById("mainImage");
        var imgUrl = img.src.replace("thumb", "main");
        var targetUrl = img.src.replace(".thumb", "");
        mainImage.src = imgUrl;
        reorderPhotoUrls(targetUrl);
    }
    
    function reorderPhotoUrls(target) {
        var reorderedUrls = [];
        var i;
        for(i = 0; i < photoUrls.length; i++) {
            if(photoUrls[i].slice(-30) === target.slice(-30)) {
                var j;
                for(j = 0; j < photoUrls.length; j++) {
                    reorderedUrls[j] = photoUrls[i];
                    i++;
                    if(i === photoUrls.length) {
                        i = 0;
                    }
                }
                photoUrls = reorderedUrls;
            }
        }
    }

</script>
</asp:Content>
