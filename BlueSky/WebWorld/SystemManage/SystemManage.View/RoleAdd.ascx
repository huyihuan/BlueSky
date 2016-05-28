<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleAdd.ascx.cs" Inherits="WebWorld.SystemManage.RoleAdd" %>

<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>角色名称：</td>
        <td><input type="text" id="txt_Name" class="txt-normal" runat="server" size="35" /></td>
    </tr>
    <tr>
        <td align="right">描述：</td>
        <td><textarea class="txt-mutil" id="txt_Description" runat="server" cols="40" rows="8"></textarea></td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td colspan="2" align="center">
            <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.layout.closeActiveWindow();"/>
        </td>
    </tr>
</table>
<script type="text/javascript">
    function save() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Name.ClientID %>", message: "角色名称不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>