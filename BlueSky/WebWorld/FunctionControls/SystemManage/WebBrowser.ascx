<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebBrowser.ascx.cs" Inherits="WebWorld.FunctionControls.SystemManage.WebBrowser" %>
<script type="text/javascript">
    var webFrame = null;
    $(document).ready(function() {
        webFrame = document.getElementById("frameContent");
        //var docWidth = document.body.clientWidth;
        //var docHeight = document.body.clientHeight;
        //webFrame.height = docHeight - 30;
    });

function goURI() {
    var strURI = document.getElementById("txt_URI").value;
    if ("" == strURI || undefined == strURI || null == strURI)
        return;
    webFrame.src = strURI;
}
</script>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    
    <tr height="100%"><td height="100%"><iframe id="frameContent" frameborder="0" src="http://www.baidu.com" scrolling="auto" height="100%" width="100%"></iframe></td></tr>
</table>