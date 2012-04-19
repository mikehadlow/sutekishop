<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Suteki.Shop.StockControl.AddIn.Models" %>
<%@ Import Namespace="Suteki.Shop.StockControl.AddIn.ViewData" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<% var viewData = (StockItemHistoryViewData)Model; %>
<% var stockItem = viewData.StockItem; %>

    <h1>History for <%= stockItem.ProductName %> - <%= stockItem.SizeName %></h1>
    <% Html.BeginForm("HistoryQuery", "StockControl"); %>
        <%= Html.Hidden("StockItemId", viewData.StockItem.Id) %>
        Show stock history 
        From &nbsp;&nbsp; <%= Html.TextBox("Start", viewData.Start.ToShortDateString(), new { @class = "inline-date" })%> &nbsp;&nbsp;
        To   &nbsp;&nbsp; <%= Html.TextBox("End",   viewData.End.ToShortDateString(),   new { @class = "inline-date" })%>
        &nbsp;&nbsp; DD/MM/YYYY
        <input type="submit" value="Update List" class="inline" />
    <% Html.EndForm(); %>

    <table>
        <tr>
            <th class="thin">When</th>
            <th class="thin">What</th>
            <th class="thin">Who</th>
            <th class="thin">Stock Level</th>
        </tr>
    <% foreach (var stockItemHistory in viewData.History) {%>
        <tr class="line-above">
           <td class="thin"><%= stockItemHistory.DateTime %></td> 
           <td class="thin"><%= stockItemHistory.Description %></td> 
           <td class="thin"><%= stockItemHistory.User %></td> 
           <td class="thin"><%= stockItemHistory.Level %></td> 
        </tr>
        <% if (!string.IsNullOrEmpty(stockItemHistory.Comment))
           { %>
        <tr>
            <td colspan="4"><%= stockItemHistory.Comment%></td>
        </tr>
        <% } %>
    <% } %>
    </table>

</asp:Content>