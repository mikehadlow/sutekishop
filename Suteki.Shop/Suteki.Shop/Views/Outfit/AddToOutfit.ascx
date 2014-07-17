<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AddToOutfitViewModel>" %>

<div class="productOptions">
    <% using (Html.BeginForm<OutfitController>(x => x.AddToOutfit(null))) { %>
        <%= Html.HiddenFor(x => x.Product.Id) %>
        <%= Html.ComboFor(x => x.Outfit) %>
        <%= Html.SubmitButton("outfitSubmit", "Add to outfit") %>
    <% } %>
</div>