<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Cms.master" Inherits="Suteki.Shop.ViewPage<CmsViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<div id="content">
    <%= ViewData.Model.TextContent.Text%>

    <% if(User.IsAdministrator()) { %>
        <p><%= Html.ActionLink<CmsController>(c => c.EditTop(ViewData.Model.Content.Id), "Edit")%></p>
    <% } %>

</div>
</asp:Content>
