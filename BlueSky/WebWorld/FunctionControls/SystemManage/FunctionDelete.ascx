﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionDelete.ascx.cs" Inherits="WebWorld.FunctionControls.SystemManage.FunctionDelete" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr><td align="center" valign="middle"><asp:Label ID="lbl_DeleteMessage" runat="server"></asp:Label></td></tr>
    <tr><td height="30"></td></tr>
    <tr>
        <td align="center" valign="middle">
            <input type="button" class="btn-normal" value=" 删 除 " runat="server" id="btnDelete" onserverclick="btnDelete_Click" />
            <input type="button" class="btn-normal" value=" 返 回 " onclick="top.windowFactory.closeTopFocusForm();" />
        </td>
    </tr>
</table>