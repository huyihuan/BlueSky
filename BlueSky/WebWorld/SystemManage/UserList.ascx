﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserList.ascx.cs" Inherits="WebWorld.SystemManage.UserList" %>
<%@ Register TagPrefix="BS" Namespace="WebSystemBase.UserControls" Assembly="WebSystemBase" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 58, resize: true });
        window.formUtil.initCheckBox({ listElement: ".table-list", rememberValue:true, rowHover: true, rowClick: true });
    });
</script>

<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td><div id="toolBar" runat="server"></div></td>
        <td nowrap class="td-search-single" width="200">
            <input type="text" id="txt_Filter" runat="server" class="txt-normal" size="20" />
            <input type="button" id="btn_Search" runat="server" class="btn-normal" value="查 询" onserverclick="btn_Search_Click" />
        </td>
    </tr>
    <tr>
        <td valign="top" colspan="2">
            <div class="list-container">
                <table class="table-list table-list-topline" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" class="td-header td-header-select"><input type="checkbox" class="cbSelectAll" /></td>
                        <td align="center" class="td-header td-header-order">序号</td>
                        <td align="center" class="td-header">帐号</td>
                        <td align="center" class="td-header">姓名</td>
                        <td align="center" class="td-header">性别</td>
                        <td align="center" class="td-header">QQ</td>
                        <td align="center" class="td-header">角色</td>
                        <td align="center" class="td-header-last">Email</td>
                    </tr>
                    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td align="center" class="td-content td-content-select"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                            <td align="center" class="td-content td-content-order"><asp:Label ID="lbl_OrderId" runat="server"></asp:Label></td>
                            <td class="td-content"><asp:Label ID="lbl_UserName" runat="server"></asp:Label></td>
                            <td class="td-content"><asp:Label ID="lbl_NickName" runat="server"></asp:Label></td>
                            <td align="center" class="td-content"><asp:Label ID="lbl_Gender" runat="server"></asp:Label></td>
                            <td align="center" class="td-content"><asp:Label ID="lbl_QQ" runat="server"></asp:Label></td>
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
        <td colspan="2">
            <BS:PagerNavication ID="PagerNavication" PageSizeList="10,15,20,30,50,100" PageSize="10" runat="server" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
        </td>
    </tr>
</table>