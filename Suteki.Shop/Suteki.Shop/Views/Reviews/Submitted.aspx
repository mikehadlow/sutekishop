<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="System.Web.Mvc.ViewPage<Product>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Review Submitted</h2>
    <p>Thank you for submitting your review. It is now awaiting approval and should appear on the product page shortly.</p>
    <p><%= Html.ActionLink<ProductController>(c=>c.Item(Model.UrlName), "Return to product page.") %></p>
</asp:Content>
