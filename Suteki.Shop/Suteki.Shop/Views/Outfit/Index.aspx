<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master"  Inherits="Suteki.Shop.ViewPage<IEnumerable<Outfit>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<div id="product-list">
<h1>Outfits</h1>

    
<% if(User.IsAdministrator()) { %>
    <p><%= Html.ActionLink<OutfitController>(c => c.New(), "New Outift")%></p>
<% } %>
    
    <div>
        
        <p>List of outfits goes here.</p>
    </div>

</div>
</asp:Content>
