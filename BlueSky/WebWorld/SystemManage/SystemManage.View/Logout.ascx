<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Logout.ascx.cs" Inherits="WebWorld.SystemManage.Logout" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr><td height="15"></td></tr>
    <tr><td align="center" valign="middle">确定要退出系统吗？</td></tr>
    <tr><td height="15"></td></tr>
    <tr>
        <td align="center" valign="middle">
            <input type="button" class="btn-normal" value=" 安全退出 " runat="server" id="btnLogout" onserverclick="btnLogout_Click" />
            <input type="button" class="btn-normal" value=" 取 消 " onclick="top.windowFactory.closeTopFocusForm();" />
        </td>
    </tr>
</table>