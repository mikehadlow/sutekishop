<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HeaderViewData>" %>

    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.01)" />
    <link href="<%= Url.Content("~/Content/" + Model.SiteCss) %>" rel="stylesheet" type="text/css" />
    <link rel="EditURI" type="application/rsd+xml" title="RSD" href="<%= Url.Content("~/rsd.xml") %>" />
    <link rel="wlwmanifest" type="application/wlwmanifest+xml" href="<%= Url.Content("~/wlwmanifest.xml") %>" />
    <meta name="verify-v1" content="yT3b2nHIk4a8/T+tW3p8zQ30vgio2ELuZc/9qk//JBw=" />
    <link rel="shortcut icon" href="<%= Url.Content("~/favicon.ico") %>" type="image/x-icon" />
    <script type="text/javascript" src="<%= Url.Content("~/Content/Script/jquery-1.2.6.min.js") %>"></script>
