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
    <link href="Include/css/BlueSky.Window.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/Bluesky.Window.js" type="text/javascript"></script>
    <link href="BlueSky/themes/gray/Bluesky.Button.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/layout.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/layout.js" type="text/javascript"></script>
    <script src="Include/js/utils.js" type="text/javascript"></script>
</head>
<script type="text/javascript">
    function userView() {
        top.windowFactory.targetFocusForm({
            title: "用户信息",
            url: "<%=strUserInfomationURL %>",
            width: 500,
            height: 450,
            target: window
        });
    }
</script>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        setTimeout(function() { Bluesky.component.Masklayer.loading(); }, 0);
    </script>
    <div id="divTop">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" height="100%">
            <tr>
                <td><font class="font-title float-left">Bule Sky</font></td>
                <td align="right" valign="middle">
                    <a class="bluesky-button buttonMode-plain buttonCombination-both icon-user iconPosition-left buttonHighten-5px buttonPlainBorder-dashed" onclick="userView();">当前用户：<asp:Literal ID="lt_CurrentUserName" runat="server"></asp:Literal></a>
                    <a class="bluesky-button buttonMode-normal buttonCombination-both icon-exist iconPosition-left buttonHighten-5px buttonNormalBorder-single" onclick="layout.logout();" href="javascript:void(0);">退出</a>
                    <a class="bluesky-button buttonMode-normal buttonCombination-both icon-refresh iconPosition-left buttonHighten-5px buttonNormalBorder-3D" runat="server" onserverclick="btnRefresh_Click" href="javascript:void(0);">重新登陆</a>
                    <a class="bluesky-button buttonMode-normal buttonCombination-both icon-set iconPosition-left buttonHighten-5px buttonNormalBorder-3D" onclick="layout.moduleSetting();" href="javascript:void(0);">配置</a>&nbsp;
                    <%--<a class="bluesky-button buttonMode-plain buttonCombination-image icon-exist buttonHighten-3px" onclick="layout.logout();" href="javascript:void(0);"></a>
                    <a class="bluesky-button buttonMode-plain buttonCombination-image icon-set buttonHighten-3px" onclick="layout.moduleSetting();" href="javascript:void(0);"></a>&nbsp;--%>
                </td>
            </tr>
        </table>
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
