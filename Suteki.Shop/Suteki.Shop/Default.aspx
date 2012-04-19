<%@ Page Language="C#" AutoEventWireup="true" %>
<script runat="server">
	protected void Page_Load(object sender, EventArgs e)
	{
		HttpContext.Current.RewritePath(Request.ApplicationPath);
		IHttpHandler httpHandler = new MvcHttpHandler();
		httpHandler.ProcessRequest(HttpContext.Current);
	}
</script>