<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Suteki.Shop.Menu>" %>
<%= Html.WriteMenu(Model, new { _class = "mainMenu" })%>