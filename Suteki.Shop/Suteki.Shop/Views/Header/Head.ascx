<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HeaderViewData>" %>

    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.01)" />
    <link href="<%= Url.Content("~/Content/" + Model.SiteCss) %>" rel="stylesheet" type="text/css" />
    <link rel="EditURI" type="application/rsd+xml" title="RSD" href="<%= Url.Content("~/rsd.xml") %>" />
    <link rel="wlwmanifest" type="application/wlwmanifest+xml" href="<%= Url.Content("~/wlwmanifest.xml") %>" />
    <meta name="verify-v1" content="yT3b2nHIk4a8/T+tW3p8zQ30vgio2ELuZc/9qk//JBw=" />
    <link rel="shortcut icon" href="<%= Url.Content("~/favicon.ico") %>" type="image/x-icon" />
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.min.js"></script>
    
    <!-- FancyBox -->
    <link rel="stylesheet" href="<%= Url.Content("~/Content/" + "fancybox/source/jquery.fancybox.css?v=2.0.6") %>" type="text/css" media="screen" />
    <script type="text/javascript" src="<%= Url.Content("~/Content/" + "fancybox/source/jquery.fancybox.pack.js?v=2.0.6") %>"></script>
    
    <link rel="stylesheet" href="<%= Url.Content("~/Content/" + "bjqs/bjqs.css") %>" type="text/css" />
    <script type="text/javascript" src="<%= Url.Content("~/Content/" + "bjqs/bjqs-1.3.min.js") %>"></script>
