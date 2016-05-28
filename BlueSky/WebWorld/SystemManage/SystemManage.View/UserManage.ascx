<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserManage.ascx.cs" Inherits="WebWorld.SystemManage.UserManage" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 28, resize: true });
    });

    function add() {
        if (!Utils.vText({ vtype: "Integer", vid: "<%=txt_AddNumber.ClientID %>", message: "请填写新增的用户数量！", ishint: true }))
            return false;
        return true;
    }
</script>
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td><div id="toolBar" runat="server"></div></td>
    </tr>
    <tr>
        <td>
            <div class="list-container">
                新增用户数量：<input type="text" id="txt_AddNumber" runat="server" class="txt-normal" size="8" />
                &nbsp;初始化用户角色：<asp:DropDownList ID="sel_Role" runat="server"></asp:DropDownList>
                <input type="button" id="btn_Add" runat="server" class="btn-normal" value="新 增" onclick="if(!add()) return false;" onserverclick="btn_Add_ServerClick" />
                <label id="lbl_Message" runat="server"></label>
            </div>
        </td>
    </tr>
</table>