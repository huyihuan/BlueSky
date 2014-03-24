<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebWorld.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<html>
<head>
    <title></title>
    <script src="Include/js/Bluesky.js" type="text/javascript"></script>
    <link href="Include/css/main.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/graybox.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/graybox.js" type="text/javascript"></script>
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
        setTimeout(function() { bs_event_showLoadingLayer(top.window.document); }, 0);
    </script>
    <div id="divTop"><font class="font-title float-left">Bule Sky Of My Dream</font><a href="javascript:setModule();" class="float-right">【系统模块配置】</a></div>
    <div id="divButtom">
        <div id="divLeft">
            <div id="divMenuTitle">系统功能</div>
            <div id="divMenu"><iframe id="MenuFrame" frameborder="0" scrolling="no"></iframe></div>
            <!--<div id="divShowBar" class="ShowMenuBtn" onclick="layoutUtils.toggleMenu();" title="显示功能菜单"><div class="ShowMenuBtn SmallShowMenuBtn">>></div></div>-->
        </div>
        <div id="divRight">
            <div id="bs_tabs"></div>
            <div id="bs_tabFrames"><iframe id="MainFrame" style="display:none;" name="MainFrame" frameborder="0" scrolling="no"></iframe></div>
        </div>
    </div>
    <div id="divFooter"></div>
    </form>
</body>
</html>
