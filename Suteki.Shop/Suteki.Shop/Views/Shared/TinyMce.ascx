<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript" src="<%= Url.Content("~/Content/tiny_mce/tiny_mce.js") %>"></script>
<script type="text/javascript">
	tinyMCE.init({
		mode: "textareas",
		theme: "advanced",
		
		relative_urls : false,
		external_link_list_url: '<%= Url.Action<RichEditorController>(c=>c.Links()) %>',

		theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,|bullist,numlist,|,outdent,indent,|,undo,redo",
		theme_advanced_buttons2: "cut,copy,paste,pastetext,|,link,unlink,anchor,image,cleanup,code",
		theme_advanced_buttons3: "",
		
		theme_advanced_toolbar_location : "top",
		theme_advanced_toolbar_align : "left",
		theme_advanced_statusbar_location : "bottom",
		theme_advanced_resizing : true
	});   	
</script>