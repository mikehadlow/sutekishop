<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master"  Inherits="Suteki.Shop.ViewPage<Outfit>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
<div style="overflow: hidden; zoom: 1;"><h1>Outfit Edit</h1>
<%= Html.ValidationSummary() %>

    <% using (Html.MultipartForm()) { %>
		<%= this.Hidden(x => x.Id) %>
		<%= this.Hidden(x => x.Position) %>
		<%= this.TextBox(x => x.Name).Label("Name") %>
        <%= this.TextArea(x => x.Description).Label("Description") %>

        <h3>Products</h3>
        <% foreach (var outfitProduct in Model.OutfitProducts) { %>
            <%= Html.CrossLink<OutfitController>(c => c.DeleteProduct(Model.Id, outfitProduct.Id)) %>
            <%= Html.ActionLink<ProductController>(x => x.Item(outfitProduct.Product.UrlName), outfitProduct.Product.Name) %>
            <br/>
        <% } %>
    
        <%= this.CheckBox(x => x.IsActive).Label("Active") %>
    
        <h3>Images</h3>

        <div class="imageList">
        <% foreach(var outfitImage in ViewData.Model.OutfitImages.InOrder()) { %>
            <div class="imageEdit">
            <%= Html.Image("~/ProductPhotos/" + outfitImage.Image.ThumbFileName) %><br />
            <%= Html.UpArrowLink<OutfitController>(c => c.MoveImageUp(Model.Id, outfitImage.Position)) %>
            <%= Html.DownArrowLink<OutfitController>(c => c.MoveImageDown(Model.Id, outfitImage.Position)) %> &nbsp;&nbsp;
            <%= Html.CrossLink<OutfitController>(c => c.DeleteImage(Model.Id, outfitImage.Id)) %>
            </div>
        <% } %>
        </div>
        
        <div class="clear"></div>

        <div>
            <% for (int i = 0; i < 5; i++) { %>
                <input type="file" id="image_<%= i.ToString() %>" name="image_<%= i.ToString() %>" />
            <% } %>
        </div>

        <input type="submit" value="Save Changes" />
    <% } %>

</div>
<% Html.InitialiseRichTextEditor(); %>
</asp:Content>
