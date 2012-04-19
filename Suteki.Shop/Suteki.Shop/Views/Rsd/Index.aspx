<%@ Page Language="C#" ContentType="text/xml" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Suteki.Shop.Views.Rsd.Index" %><?xml version="1.0" encoding="utf-8" ?>
<rsd version="1.0" xmlns="http://archipelago.phrasewise.com/rsd" >
  <service>
    <engineName>SutekiShop</engineName>
    <engineLink>http://sutekishop.co.uk/</engineLink>
    <homePageLink><%= ViewData.Model.SiteUrl %></homePageLink>
    <apis>
      <api name="MetaWeblog" preferred="true" apiLink="<%= ViewData.Model.SiteUrl %>metablogapi.aspx" blogID="1" />
    </apis>
  </service>
</rsd>

