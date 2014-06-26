<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master"  Inherits="System.Web.Mvc.ViewPage<ShopViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<% if(User.IsAdministrator()) { %>
<h1>Order</h1>
<% } else { %>
<h1>Order Confirmation</h1>
<% } %>

<%= Html.ValidationSummary() %>
<%= Html.MessageBox(Model) %>

<h3>Order Details</h3>
<% Html.RenderPartial("~/Views/Order/Status.ascx"); %>
<% Html.RenderPartial("~/Views/Order/OrderDetails.ascx"); %>
<hr />

<% if(User.IsAdministrator()) { %>
    <% Html.RenderAction<OrderAdjustmentController>(c => c.Add(Model.Order)); %>
	<% Html.RenderPartial("Note"); %>
    <% Html.RenderPartial("TrackingNumber"); %>
<% } %>
    
<hr />

<% Html.RenderPartial("~/Views/Order/CustomerDetails.ascx"); %>

<% if(User.IsAdministrator()) { %>
    <% Html.RenderPartial("Problem"); %>
	<hr />
<% } %>

<div class="clear"></div>         

<% Html.RenderPartial("~/Views/Order/PaymentDetails.ascx"); %>

<% if (User.IsAdministrator()) { %>
    <script type="text/javascript">
    
        init();
    
        function submitHandler()
        {
            var text = document.getElementById("privateKey");
            setCookie("privateKey", text.value);
        }
        
        function init()
        {
            var text = document.getElementById("privateKey");
            if(text)
            {
                var value = getCookie("privateKey");
                if(value)
                {
                    text.value = value;
                }
            }
        }
        
        function setCookie(name, value, expires, path, domain, secure) 
        {
            var curCookie = name + "=" + escape(value) +
                ((expires) ? "; expires=" + expires.toGMTString() : "") +
                ((path) ? "; path=" + path : "") +
                ((domain) ? "; domain=" + domain : "") +
                ((secure) ? "; secure" : "");
            document.cookie = curCookie;
        }        
        
        function getCookie(name) 
        {
            var dc = document.cookie;
            var prefix = name + "=";
            var begin = dc.indexOf("; " + prefix);
            if (begin == -1) 
            {
                begin = dc.indexOf(prefix);
                if (begin != 0) return null;
            } 
            else
            {
                begin += 2;
            }
            
            var end = document.cookie.indexOf(";", begin);
            if (end == -1)
            end = dc.length;
            return unescape(dc.substring(begin + prefix.length, end));
        }        

    </script>

<% } %>

</asp:Content>
