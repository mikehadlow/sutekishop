<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master"  Inherits="Suteki.Shop.ViewPage<IEnumerable<Outfit>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<div id="product-list">

<h1>Outfits</h1>

<% if(User.IsAdministrator()) { %>
    <p><%= Html.ActionLink<OutfitController>(c => c.New(), "New Outift") %></p>
<% } %>

<% foreach (var outfit in ViewData.Model) { %>
    <div onclick="location.href='<%= Url.Action<OutfitController>(c=>c.Item(outfit.Id)) %>'" class="product">
        <div><%= Html.Encode(outfit.Name) %></div>

        <% if(outfit.HasMainImage) { %>
            <%= Html.Image("~/ProductPhotos/" + outfit.MainImage.ThumbFileName) %>
        <% } %>
        
        <% if(Context.User.IsAdministrator()) { %>
            <br />
            <%= Html.Tick(outfit.IsActive) %>
        <% } %>
    </div>
<% } %>

</div>
</asp:Content>
