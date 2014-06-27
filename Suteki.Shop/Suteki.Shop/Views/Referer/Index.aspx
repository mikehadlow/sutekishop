<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ScaffoldViewData<Suteki.Shop.Referer>>" %>
<%@ Import Namespace="MvcContrib.Pagination"%>
<%@ Import Namespace="Suteki.Common.ViewData"%>
<%@ Import Namespace="MvcContrib.UI.Pager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1>How Did You Hear of Us?</h1>

<div class="message"><%= TempData["message"] %></div>

<p><%= Html.ActionLink<RefererController>(c => c.New(), "New Reason") %></p>

<%= Html.Grid(Model.Items).Columns(column => {
		column.For(x => Html.ActionLink<RefererController>(c => c.Edit(x.Id), x.Name)).Encode(false).Named("Name").HeaderAttributes(@class => "wide");
		column.For(x => Html.Tick(x.IsActive)).Encode(false).Named("Active").HeaderAttributes(@class => "thin");
		column.For(x => Html.UpArrowLink<RefererController>(c => c.MoveUp(x.Position, 1))).Encode(false).Named("&nbsp;");
		column.For(x => Html.DownArrowLink<RefererController>(c => c.MoveDown(x.Position, 1))).Encode(false).Named("&nbsp;");
    }) %>
    
 <%= Html.Pager((IPagination)Model.Items) %>
</asp:Content>
