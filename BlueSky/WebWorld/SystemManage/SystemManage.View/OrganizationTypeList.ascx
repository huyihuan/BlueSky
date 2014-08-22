<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationTypeList.ascx.cs" Inherits="WebWorld.SystemManage.OrganizationTypeList" %>
<%@ Register TagPrefix="BS" Namespace="WebBase.UserControls" Assembly="WebBase" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 58, resize: true });
        window.formUtil.initCheckBox({ listElement: ".table-list", rememberValue: true, rowHover: true, rowClick: true });
    });
</script>
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td><div id="toolBar" runat="server"></div></td>
    </tr>
    <tr>
        <td valign="top">
            <div class="list-container">
                <table class="table-list table-list-topline" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" class="td-header td-header-select"><input type="checkbox" class="cbSelectAll" /></td>
                        <td align="center" class="td-header td-header-order">序号</td>
                        <td align="center" class="td-header">类型名称</td>
                        <td class="td-header-last">备注</td>
                    </tr>
                    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td align="center" class="td-content td-content-select"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                            <td align="center" class="td-content td-content-order"><asp:Literal ID="lit_OrderId" runat="server"></asp:Literal></td>
                            <td class="td-content"><asp:Literal ID="lit_Name" runat="server"></asp:Literal></td>
                            <td class="td-content-last"><asp:Literal ID="lit_Remark" runat="server"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <BS:PagerNavication ID="PagerNavication" PageSizeList="10,15,30" PageSize="10" runat="server" Refreshable="true" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
        </td>
    </tr>
</table>