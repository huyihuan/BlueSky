<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleList.ascx.cs" Inherits="WebWorld.SystemManage.ModuleList" %>
<%@ Register TagPrefix="BS" Namespace="WebBase.UserControls" Assembly="WebBase" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 58, resize: true });
        window.formUtil.initCheckBox({ listElement: ".table-list", rememberValue: true, rowHover: true, rowClick: true });
    });

    function moduleDelete() {
        if (!confirm("确认删除选中的模块？"))
            return;
        document.getElementById("<%=btn_HiddenDelete.ClientID %>").click();
    }
</script>
<input type="button" id="btn_HiddenDelete" runat="server" value="删除" onserverclick="btn_HiddenDelete_Click" style="display:none;" />
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
                        <td align="center" class="td-header">模块名称</td>
                        <td align="center" class="td-header">Key</td>
                        <td align="center" class="td-header">Controller</td>
                        <td class="td-header-last">描述</td>
                    </tr>
                    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td align="center" class="td-content td-content-select"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                            <td align="center" class="td-content td-content-order"><asp:Label ID="lbl_OrderId" runat="server"></asp:Label></td>
                            <td class="td-content"><asp:Label ID="lbl_Name" runat="server"></asp:Label></td>
                            <td class="td-content"><asp:Label ID="lbl_Key" runat="server"></asp:Label></td>
                            <td class="td-content"><asp:Label ID="lbl_Controller" runat="server"></asp:Label></td>
                            <td class="td-content-last"><asp:Label ID="lbl_Description" runat="server"></asp:Label></td>
                        </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <BS:PagerNavication ID="PagerNavication" PageSizeList="10,15,30" PageSize="10" runat="server" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
        </td>
    </tr>
</table>

