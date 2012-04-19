<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ProductViewData>" %>
<%@ Import Namespace="Suteki.Common.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h1>Product</h1>
    
    <%= Html.ErrorBox(ViewData.Model) %>
    <%= Html.MessageBox(ViewData.Model) %>
	<%= Html.ValidationSummary() %>
    
    <% if(ViewData.Model.ProductId > 0) { %>
        <%= Html.ActionLink<ProductController>(c => c.Item(ViewData.Model.UrlName), "Preview") %>
    <% } %>

    <% using (Html.MultipartForm()) { %>
		<%= this.Hidden(x => x.ProductId) %>
		<%= this.Hidden(x => x.Position) %>

		<%= this.TextBox(x => x.Name).Label("Name") %>
        <label for="CategoryIds">Categories (ctrl+click to select more than one)</label>
        <%= Html.MutipleSelectComboFor<ProductViewData, Category>("CategoryIds", Model.CategoryIds, category => category.Id != 1)%>
        <%= this.TextBox(x => x.Weight).Label("Weight") %>
        <%= this.TextBox(x => x.Price).Label("Price " + Money.Symbol) %>
        <%= this.CheckBox(x => x.IsActive).Label("Active") %>
        
        <%= this.TextArea(x => x.Description).Label("Description") %>
        
        <h3>Sizes</h3>
        
        <p>
        <% foreach(var size in ViewData.Model.Sizes) { %>
            <%= size %>&nbsp;
        <% } %>
        
        <%= Html.ActionLink<ProductController>(c => c.ClearSizes(ViewData.Model.ProductId), "Clear all sizes")%>
        </p>
        <div class="sizeInput">
        <% for(int i=0; i<10; i++) { %>
            <%= Html.TextBox("Sizes[" + i + "]")%>
        <% } %>
        </div>
        
        <h3>Photos</h3>
        
        <div class="imageList">
        <% foreach(var productImage in ViewData.Model.ProductImages.InOrder()) { %>
            <div class="imageEdit">
            <%= Html.Image("~/ProductPhotos/" + productImage.Image.ThumbFileName) %><br />
            <%= Html.UpArrowLink<ProductImageController>(c => c.MoveImageUp(ViewData.Model.ProductId, productImage.Position)) %>
            <%= Html.DownArrowLink<ProductImageController>(c => c.MoveImageDown(ViewData.Model.ProductId, productImage.Position)) %> &nbsp;&nbsp;
            <%= Html.CrossLink<ProductImageController>(c => c.DeleteImage(ViewData.Model.ProductId, productImage.Id)) %>
            </div>
        <% } %>
        </div>
        
        <div class="clear" />
        
        <% for (int i = 0; i < 5; i++) { %>
            <input type="file" id="image_<%= i.ToString() %>" name="image_<%= i.ToString() %>" />
        <% } %>
        
        <input type="submit" value="Save Changes" />
    <% } %>
    
    <% Html.InitialiseRichTextEditor(); %>
    
</asp:Content>