<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MusicUpload.ascx.cs" Inherits="WebWorld.Modules.MyMusic.View.MusicUpload" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr><td height="15"></td></tr>
    <tr><td align="center" valign="middle"><asp:Label ID="lbl_DeleteMessage" runat="server"></asp:Label></td></tr>
    <tr><td align="center" valign="middle"><input type="file" runat="server" id="file_MusicFullName" style="width:200px;" /></td></tr>
    <tr><td height="15"></td></tr>
    <tr>
        <td align="center" valign="middle">
            <input type="button" class="btn-normal" value=" 上 传 " runat="server" id="btnUpload" onserverclick="btnUpload_Click" />
            <input type="button" class="btn-normal" value=" 返 回 " onclick="top.layout.closeActiveWindow();" />
        </td>
    </tr>
</table>