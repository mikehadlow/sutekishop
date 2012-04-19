<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/CmsSubMenu.master" Inherits="Suteki.Shop.ViewPage<CmsViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<div id="content">
    <%= ViewData.Model.TextContent.Text %>

    <% if (ViewData.Model.Content.CanEdit((User)User)) { %>
        <p><%= Html.ActionLink<CmsController>(c => c.EditText(ViewData.Model.Content.Id), "Edit")%></p>
    <% } %>

</div>
</asp:Content>
