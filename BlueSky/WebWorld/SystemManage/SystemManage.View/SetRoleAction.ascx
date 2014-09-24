<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetRoleAction.ascx.cs" Inherits="WebWorld.SystemManage.SetRoleAction" %>
<table  cellpadding="0" cellspacing="0" height="100%" width="100%">
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td width="50%" height="30" align="left">
            &nbsp;<input id="btnSave" type="button" value="保 存" class="btn-normal" runat="server" onserverclick="btnSave_Click" />
        </td>
        <td width="50%" align="right"><input type="button" value="返 回" class="btn-normal" onclick="top.layout.closeActiveWindow();"  />&nbsp;</td>
    </tr>
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td colspan="2" valign="top" style="padding:5px;">
            <asp:CheckBoxList ID="cbl_Actions" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList>
        </td>
    </tr>
</table>