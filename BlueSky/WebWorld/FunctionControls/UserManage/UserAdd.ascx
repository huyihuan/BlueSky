<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAdd.ascx.cs" Inherits="WebWorld.FunctionControls.UserManage.UserAdd" %>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>账户名称：</td>
        <td><input type="text" id="txt_UserName" runat="server" class="txt-normal" /><asp:Label ID="lbl_UserName" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>真实姓名：</td>
        <td><input type="text" class="txt-normal" id="txt_NickName" runat="server" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>密码：</td>
        <td><input type="password" class="txt-normal" id="txt_Password" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>确认密码：</td>
        <td><input type="password" class="txt-normal" id="txt_PasswordSecond" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right">性别：</td>
        <td>
            <input type="radio" name="rb_Gender" id="rb_GenderMale" runat="server" value="男" checked="true"><label for="<%=rb_GenderMale.ClientID %>">&nbsp;男</label>&nbsp;
            <input type="radio" name="rb_Gender" id="rb_GenderFemale" runat="server" value="女"><label for="<%=rb_GenderFemale.ClientID %>">&nbsp;女</label>
        </td>
    </tr>
    <tr>
        <td align="right">联系电话：</td>
        <td><input type="text" class="txt-normal" id="txt_Tel" runat="server" size="20" /></td>
    </tr>
    <tr>
        <td align="right">Email：</td>
        <td><input type="text" class="txt-normal" id="txt_Email" runat="server" size="25" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>角色：</td>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td><asp:ListBox ID="lb_RoleList" runat="server" Width="100" Height="150" ToolTip="可选择角色列表"></asp:ListBox></td>
                    <td valign="middle">
                        <input type="button" id="btnSelectRole" class="btn-normal" runat="server" value=">" onserverclick="btnSelectRole_Click" /><br /><br />
                        <input type="button" id="btnRemoveRole" class="btn-normal" runat="server" value="<" onserverclick="btnRemoveRole_Click" />
                    </td>
                    <td><asp:ListBox ID="lb_RoleSelect" runat="server" Width="100" Height="150" ToolTip="已选择角色列表"></asp:ListBox></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td colspan="2" align="center">
            <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.windowFactory.closeTopFocusForm();" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    function save() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_UserName.ClientID %>", message: "账户名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_NickName.ClientID %>", message: "真实姓名不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Password.ClientID %>", message: "密码不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_PasswordSecond.ClientID %>", message: "确认密码不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>