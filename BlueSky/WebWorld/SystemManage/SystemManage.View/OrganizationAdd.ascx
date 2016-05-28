<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationAdd.ascx.cs" Inherits="WebWorld.SystemManage.OrganizationAdd" %>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>组织机构名称：</td>
        <td><input type="text" id="txt_Name" runat="server" class="txt-normal" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>组织机构类型：</td>
        <td><asp:DropDownList ID="sel_OrganizationTypeId" runat="server" CssClass="select-normal"></asp:DropDownList>&nbsp;<input type="button" runat="server" id="btnRefresh" value="刷新" title="刷新组织机构类型列表" class="btn-normal" onserverclick="btnRefresh_Click" />&nbsp;<input type="button" id="btnTypeManage" value="..." class="btn-normal" onclick="typeManage();" title="管理组织机构类型" /></td>
    </tr>
    <tr>
        <td align="right">描述：</td>
        <td><asp:TextBox ID="txt_Remark" runat="server" CssClass="txt-mutil" TextMode="MultiLine" Rows="10" Columns="50"></asp:TextBox></td>
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
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Name.ClientID %>", message: "组织机构名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=sel_OrganizationTypeId.ClientID %>", message: "组织机构类型不能为空！", ishint: true }))
            return false;
        return true;
    }

    function typeManage() {
        var args = {
            width: 450,
            height: 400,
            renderTo: top.document.body,
            title: "组织机构类型管理",
            loader: { url: "<%=strOrganizationTypeUrl %>" },
            icon : { show : false },
            resizeable: false,
            moveable: true,
            mask: true,
            flicker:true
        }
        top.Bluesky.component.create("Window", args);
    }
</script>