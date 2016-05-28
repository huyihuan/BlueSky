<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetUserRole.ascx.cs" Inherits="WebWorld.SystemManage.SetUserRole" %>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr><td colspan="2"><asp:Label ID="lbl_Message" runat="server"></asp:Label></td></tr>
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>角色：</td>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td><asp:ListBox ID="lb_RoleList" runat="server" Width="100" Height="150" ToolTip="可选择角色列表"></asp:ListBox></td>
                    <td width="3"></td>
                    <td valign="middle">
                        <input type="button" id="btnSelectRole" class="btn-normal" runat="server" value=">" onserverclick="btnSelectRole_Click" /><br /><br />
                        <input type="button" id="btnRemoveRole" class="btn-normal" runat="server" value="<" onserverclick="btnRemoveRole_Click" />
                    </td>
                    <td width="3"></td>
                    <td><asp:ListBox ID="lb_RoleSelect" runat="server" Width="100" Height="150" ToolTip="已选择角色列表"></asp:ListBox></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="center" colspan="2">
            <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.layout.closeActiveWindow();" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    function save() {
        return true;
    }
</script>