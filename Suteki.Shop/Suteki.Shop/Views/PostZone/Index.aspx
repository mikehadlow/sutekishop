<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<ScaffoldViewData<Suteki.Shop.PostZone>>" %>
<%@ Import Namespace="MvcContrib.Pagination"%>
<%@ Import Namespace="Suteki.Common.ViewData"%>
<%@ Import Namespace="Suteki.Common.ViewData"%>
<%@ Import Namespace="MvcContrib.UI.Pager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1>Postage Zones</h1>

<div class="message"><%= TempData["message"] %></div>

<p><%= Html.ActionLink<PostZoneController>(c => c.New(), "New Postage Zone") %></p>

<%= Html.Grid(Model.Items).Columns(column => {
		column.For(x => Html.ActionLink<PostZoneController>(c => c.Edit(x.Id), x.Name)).Named("Name").Encode(false).HeaderAttributes(@class => "thin");
		column.For(x => x.Multiplier).Format("{0:0.00}").Attributes(@class => "number").HeaderAttributes(@class => "thin number");
		column.For(x => Html.Tick(x.AskIfMaxWeight)).Encode(false).Named("Ask If Max Weight").HeaderAttributes(@class => "wide");
		column.For(x => x.FlatRate.ToStringWithSymbol()).Named("Flat Rate").HeaderAttributes(@class => "thin number").Attributes(@class => "number");
		column.For(x => Html.Tick(x.IsActive)).Named("Active").Encode(false).HeaderAttributes(@class => "thin");
		column.For(x => Html.UpArrowLink<PostZoneController>(c => c.MoveUp(x.Position, 1))).Encode(false).Named("&nbsp;");
		column.For(x => Html.DownArrowLink<PostZoneController>(c => c.MoveDown(x.Position, 1))).Encode(false).Named("&nbsp;");
   }) %>
   
<%= Html.Pager((IPagination)Model.Items) %>
</asp:Content>
