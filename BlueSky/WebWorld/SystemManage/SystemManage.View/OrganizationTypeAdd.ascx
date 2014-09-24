<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationTypeAdd.ascx.cs" Inherits="WebWorld.SystemManage.OrganizationTypeAdd" %>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>类型名称：</td>
        <td><input type="text" id="txt_Name" runat="server" class="txt-normal" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>父级类型：</td>
        <td><asp:DropDownList ID="sel_OrganizationTypeId" runat="server" CssClass="select-normal"></asp:DropDownList></td>
    </tr>
    <tr>
        <td align="right">描述：</td>
        <td><asp:TextBox ID="txt_Remark" runat="server" CssClass="txt-mutil" TextMode="MultiLine" Rows="5" Columns="30"></asp:TextBox></td>
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
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Name.ClientID %>", message: "类型名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=sel_OrganizationTypeId.ClientID %>", message: "父级类型不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>