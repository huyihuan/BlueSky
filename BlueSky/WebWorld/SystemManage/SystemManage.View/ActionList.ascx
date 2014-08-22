<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActionList.ascx.cs" Inherits="WebWorld.SystemManage.ActionList" %>
<%@ Register TagPrefix="BS" Namespace="WebBase.UserControls" Assembly="WebBase" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 58, minusWidth: 0, resize: true });
        window.formUtil.initCheckBox({ listElement: ".table-list", rememberValue: true, rowHover: true, rowClick: true });
    });

    function selectFunction() {
        var windowArguments = new Object();
        windowArguments.width = 400;
        windowArguments.height = 400;
        windowArguments.title = "选择系统功能";
        windowArguments.url = "<%=strFunctionSelectUrl %>";
        windowArguments.resize = false;
        windowArguments.move = true;
        top.windowFactory.topFocusForm(windowArguments);
    }
</script>
<table  cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td valign="top"><div id="toolBar" runat="server"></div></td>
    </tr>
    <tr>
        <td valign="top">
            <div class="action-search-panel" minusHeight="34" minusObject=".list-container">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td nowrap>
                            操作名称：<input type="text" class="txt-normal" id="txt_Name" runat="server" />&nbsp;&nbsp;
                            Key：<input type="text" class="txt-normal"  id="txt_Key" runat="server" />&nbsp;&nbsp;
                            所属功能：<input type="text" class="txt-normal" id="txt_FunctionName" runat="server" /><input type="button" class="btn-normal btn-select-noleft" id="btn_SelectFunction" value="..." onclick="selectFunction();" />&nbsp;&nbsp;
                            <input type="button" id="btn_Search" runat="server" class="btn-normal" onserverclick="btn_Search_Click" value="查 询" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="list-container">
                <table class="table-list table-list-topline" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" class="td-header td-header-select"><input type="checkbox" class="cbSelectAll" /></td>
                        <td align="center" class="td-header td-header-order">序号</td>
                        <td align="center" class="td-header" width="25">&nbsp;</td>
                        <td align="center" class="td-header">名称</td>
                        <td align="center" class="td-header">Key</td>
                        <td align="center" class="td-header">控件名称</td>
                        <td align="center" class="td-header">操作数量</td>
                        <td align="center" class="td-header">类型</td>
                        <td class="td-header-last">描述</td>
                    </tr>
                    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td align="center" class="td-content td-content-select"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                            <td align="center" class="td-content td-content-order"><asp:Literal ID="lit_OrderId" runat="server"></asp:Literal></td>
                            <td class="td-content" align="center"><asp:Literal ID="lit_IconName" runat="server"></asp:Literal></td>
                            <td class="td-content"><asp:Literal ID="lit_Name" runat="server"></asp:Literal></td>
                            <td class="td-content"><asp:Literal ID="lit_Key" runat="server"></asp:Literal></td>
                            <td class="td-content"><asp:Literal ID="lit_ControlName" runat="server"></asp:Literal></td>
                            <td class="td-content"><asp:Literal ID="lit_EntityCount" runat="server"></asp:Literal></td>
                            <td class="td-content"><asp:Literal ID="lit_ActionType" runat="server"></asp:Literal></td>
                            <td class="td-content-last"><asp:Literal ID="lit_Description" runat="server"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <BS:PagerNavication ID="PagerNavication" PageSizeList="10,30,50" PageSize="10" runat="server" Refreshable="true" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
        </td>
    </tr>
</table>