<%@ Control Language="C#" Inherits="Suteki.Shop.ViewUserControl<Product>" %>
<div class="productDescription">
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
	<% foreach(var productImage in Model.ProductImages.InOrder()) { %>
		<%= Html.Image("~/ProductPhotos/" + productImage.Image.ThumbFileName, new { onclick = "onThumbnailClick(this)" })%>
	<% } %>
	</div>

	<p><%= Model.Description %></p>
    <p><a href="mailto:?subject=<%= Model.Name %>&body=<%= "http://" + Request.Url.Host + Url.Action("Item", new{ urlName = Model.UrlName }) %>">Email to a friend</a></p>

<%--    <div class="clear"></div>--%>

    <fb:like width="450" height="80" colorscheme="dark"/>
</div>

<script type="text/javascript">

	var photoUrls = [<%= string.Join(",", Model.ProductImages.InOrder().Select(x => "\"" + Url.Content("~/ProductPhotos/" + x.Image.FileNameAsString) + "\"").ToArray()) %>];

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