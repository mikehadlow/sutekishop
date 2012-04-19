<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/CmsSubMenu.master" Inherits="Suteki.Shop.ViewPage<CmsViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1><%= ViewData.Model.Menu.Name%></h1>
<div class="message"><%= TempData["message"] %></div>
<p>
    <%= Html.ActionLink<CmsController>(c => c.Add(ViewData.Model.Menu.Id), "New Page")%>&nbsp;
    <%= Html.ActionLink<MenuController>(c => c.New(ViewData.Model.Menu.Id), "New Menu")%>
</p>

<%= Html.Grid(Model.Menu.Contents.InOrder()).Columns(column => {
		column.For(x => x.Type).Named("&nbsp;").HeaderAttributes(@class => "thin");
		column.For(x => x.Link(Html)).Encode(false).Named("&nbsp;").HeaderAttributes(@class => "thin");
		column.For(x => x.EditLink(Html)).Encode(false).Named("&nbsp;").HeaderAttributes(@class => "thin");
		column.For(x => Html.Tick(x.IsActive)).Encode(false).Named("&nbsp;").HeaderAttributes(@class => "thin");
		column.For(x => Html.Partial("UpDown", x)).HeaderAttributes(@class => "thin"); //Re-ordering arrows
		column.For(x => x.IsMenu ? Html.ActionLink<CmsController>(c => c.Add(x.Id), "New Page").ToString() : "&nbsp;").Encode(false).Named("&nbsp;").HeaderAttributes(@class => "thin");
        column.For(x => x.IsMenu ? Html.ActionLink<MenuController>(c => c.New(x.Id), "New Menu").ToString() : "&nbsp;").Encode(false).Named("&nbsp;").HeaderAttributes(@class => "thin");
			
	}) %>

</asp:Content>
