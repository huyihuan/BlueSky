<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleAdd.ascx.cs" Inherits="WebWorld.SystemManage.ModuleAdd" %>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>模块名称：</td>
        <td><input type="text" id="txt_Name" runat="server" class="txt-normal" /><asp:Label ID="lbl_Name" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>Key：</td>
        <td><input type="text" class="txt-normal" id="txt_Key" runat="server" /><asp:Label ID="lbl_Key" runat="server"></asp:Label>（请填写英文，必须唯一）</td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>Controller：</td>
        <td><input type="text" class="txt-normal" id="txt_Controller" runat="server" size="40" /><asp:Label ID="lbl_Controller" runat="server"></asp:Label>（请填写英文）</td>
    </tr>
    <tr>
        <td align="right">描述：</td>
        <td><asp:TextBox ID="txt_Description" runat="server" CssClass="txt-mutil" TextMode="MultiLine" Rows="10" Columns="60"></asp:TextBox></td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td colspan="2" align="center">
            <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.layout.closeActiveWindow();" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    function save() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Name.ClientID %>", message: "模块名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Key.ClientID %>", message: "Key不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Controller.ClientID %>", message: "Controller不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>
