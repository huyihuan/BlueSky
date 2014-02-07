<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserList.ascx.cs" Inherits="WebWorld.FunctionControls.UserManage.UserList" %>
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
</script>
<div class="content">
    <table  cellpadding="0" cellspacing="0" height="100%" width="100%">
        <tr>
            <td><div id="toolBar" runat="server"></div></td>
        </tr>
        <tr>
            <td valign="top">
                <div id="listContainer" style="overflow:auto;">
                    <table class="table-list-top" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" class="td-header"><input type="checkbox" class="cbSelectAll" /></td>
                            <td align="center" class="td-header">序号</td>
                            <td align="center" class="td-header">帐号</td>
                            <td align="center" class="td-header">姓名</td>
                            <td align="center" class="td-header">性别</td>
                            <td align="center" class="td-header">联系电话</td>
                            <td align="center" class="td-header">角色</td>
                            <td align="center" class="td-header-last">Email</td>
                        </tr>
                        <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td align="center" class="td-content"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                                <td align="center" class="td-content"><asp:Label ID="lbl_OrderId" runat="server"></asp:Label></td>
                                <td class="td-content"><asp:Label ID="lbl_UserName" runat="server"></asp:Label></td>
                                <td class="td-content"><asp:Label ID="lbl_NickName" runat="server"></asp:Label></td>
                                <td align="center" class="td-content"><asp:Label ID="lbl_Gender" runat="server"></asp:Label></td>
                                <td align="center" class="td-content"><asp:Label ID="lbl_Tel" runat="server"></asp:Label></td>
                                <td align="center" class="td-content"><asp:Label ID="lbl_PropertyRoleName" runat="server"></asp:Label></td>
                                <td class="td-content-last"><asp:Label ID="lbl_Email" runat="server"></asp:Label></td>
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