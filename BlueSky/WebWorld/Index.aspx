<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebWorld.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<html>
<head>
    <title></title>
    <script src="Include/js/Bluesky.js" type="text/javascript"></script>
    <link href="Include/css/main.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/Bluesky.Masklayer.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/Bluesky.Masklayer.js" type="text/javascript"></script>
    <link href="Include/css/Bluesky.Tabs.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/Bluesky.Tabs.js" type="text/javascript"></script>
    <link href="Include/css/layout.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/layout.js" type="text/javascript"></script>
    <link href="Include/css/window.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/Bluesky.window.js" type="text/javascript"></script>
    <script src="Include/js/utils.js" type="text/javascript"></script>
    <link href="Include/css/tab.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/tabs.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        setTimeout(function() { Bluesky.component.Masklayer.loading(); }, 0);
    </script>
    <div id="divTop">
        <font class="font-title float-left">Bule Sky</font>
        <a href="javascript:layout.logout();"  class="float-right">【退出】</a>
        <a href="javascript:layout.moduleSetting();" class="float-right">【系统模块配置】</a>
    </div>
    <div id="divButtom">
        <div id="divLeft">
            <iframe id="MenuFrame" frameborder="0" src="Menu.aspx" scrolling="no" width="100%" height="100%"></iframe>
        </div>
        <div id="divRight"></div>
    </div>
    <div id="divFooter"></div>
    </form>
</body>
</html>
