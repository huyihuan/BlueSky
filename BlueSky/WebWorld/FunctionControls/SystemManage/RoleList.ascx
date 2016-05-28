<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleList.ascx.cs" Inherits="WebWorld.FunctionControls.SystemManage.RoleList" %>
<%@ Register TagPrefix="BS" Namespace="DataBase.UserControls" Assembly="DataBase" %>
<script type="text/javascript">
    var listContainer = null;
    $(document).ready(function() {
        var docSize = Utils.documentSize(document);
        listContainer = document.getElementById("listContainer");
        listContainer.style.height = docSize.height - 58 + "px";
    });

    window.onresize = function() {
        var docSize = Utils.documentSize(document);
        listContainer.style.height = docSize.height - 58 + "px";
    }

    function setFunctions() {
        var roleId = 2; //一般用户测试
        window.location.href = "Window.aspx?value=FunctionControls/SystemManage/SetRoleFunction.ascx&roleid=" + roleId;
    }
</script>
<div class="content">
    <table  cellpadding="0" cellspacing="0" height="100%" width="100%">
        <tr>
            <td><div id="toolBar" runat="server"><input type="button" id="btnSetFunctions" value="设置角色权限" onclick="setFunctions();" /></div></td>
        </tr>
        <tr>
            <td valign="top">
                <div id="listContainer" style="overflow:auto;">
                    <table class="table-list-top" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" class="td-header"><input type="checkbox" class="cbSelectAll" /></td>
                            <td align="center" class="td-header">序号</td>
                            <td align="center" class="td-header">角色名称</td>
                            <td align="center" class="td-header-last">角色描述</td>
                        </tr>
                        <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td align="center" class="td-content"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                                <td align="center" class="td-content"><asp:Label ID="lbl_OrderId" runat="server"></asp:Label></td>
                                <td class="td-content"><asp:Label ID="lbl_RoleName" runat="server"></asp:Label></td>
                                <td class="td-content-last"><asp:Label ID="lbl_Remark" runat="server"></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <BS:PagerNavication ID="PagerNavication" PageSizeList="2,3,10" PageSize="2" runat="server" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
            </td>
        </tr>
    </table>
</div>