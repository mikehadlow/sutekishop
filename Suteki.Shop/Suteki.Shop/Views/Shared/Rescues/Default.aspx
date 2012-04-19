<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Suteki.Shop.Views.Shared.Rescues.Default" %>
<%@ Import Namespace="System.Security"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<% if (ViewData.Model.Exception.InnerException != null && ViewData.Model.Exception.InnerException.GetType() == typeof(SecurityException))
   { %>

<h1>You do not have permission to view this page</h1>

<% }
   else
 {%>

<h1>An Error has occured in the application</h1>

<p>A record of the problem has been made. Please check back soon</p>

<%
 }%>

<% if (Context.IsDebuggingEnabled) { %>
<pre>

<%= ViewData.Model.GetType().Name%>

<%= ViewData.Model.Exception.Message%>

<%= ViewData.Model.Exception.StackTrace%>

<% if (ViewData.Model.Exception.InnerException != null) { %>

    <%= ViewData.Model.Exception.InnerException.GetType().Name%>

    <%= ViewData.Model.Exception.InnerException.Message%>

    <%= ViewData.Model.Exception.InnerException.StackTrace%>

    <% if (ViewData.Model.Exception.InnerException.InnerException != null) { %>

        <%= ViewData.Model.Exception.InnerException.InnerException.GetType().Name%>

        <%= ViewData.Model.Exception.InnerException.InnerException.Message%>

        <%= ViewData.Model.Exception.InnerException.InnerException.StackTrace%>

    <% } %>


<% } %>
<% } %>
</pre>
</asp:Content>
