<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetUserPassword.ascx.cs" Inherits="WebWorld.SystemManage.SetUserPassword" %>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr><td colspan="2"><asp:Label ID="lbl_Message" runat="server"></asp:Label></td></tr>
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>密 码：</td>
        <td><input type="password" class="txt-normal" id="txt_Password" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>确认密码：</td>
        <td><input type="password" class="txt-normal" id="txt_PasswordSecond" runat="server" size="30" /></td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="center" colspan="2">
            <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.windowFactory.closeTopFocusForm();" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    function save() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Password.ClientID %>", message: "密码不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_PasswordSecond.ClientID %>", message: "确认密码不能为空！", ishint: true }))
            return false;
        if (document.getElementById("<%=txt_Password.ClientID %>").value != document.getElementById("<%=txt_PasswordSecond.ClientID %>").value) {
            alert("密码和确认密码不同，请重新输入！");
            return false;
        }
        return true;
    }
</script>