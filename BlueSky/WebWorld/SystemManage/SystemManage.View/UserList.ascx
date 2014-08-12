<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserList.ascx.cs" Inherits="WebWorld.SystemManage.UserList" %>
<%@ Register TagPrefix="BS" Namespace="WebBase.UserControls" Assembly="WebBase" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 58, resize: true, minusWidth: 0 });
        window.formUtil.initCheckBox({ listElement: ".table-list", rememberValue: true, rowHover: true, rowClick: true });
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
                        <td align="center" class="td-header" width="100">帐号</td>
                        <td align="center" class="td-header" width="100">姓名</td>
                        <td align="center" class="td-header" width="50"><div style="width:50px;">性别</td>
                        <td align="center" class="td-header" width="100">QQ</td>
                        <td align="center" class="td-header" width="100">角色</td>
                        <td align="center" class="td-header" width="100">Email</td>
                        <td align="center" class="td-header" width="100">联系电话</td>
                        <td align="center" class="td-header" width="100">身份证号</td>
                        <td class="td-header-last"></td>
                    </tr>
                    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="td-content td-content-select" align="center"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                            <td class="td-content td-content-order td-align-center"><div><asp:Literal ID="lit_OrderId" runat="server"></asp:Literal></div></td>
                            <td class="td-content td-align-center"><div style="width:100px;"><asp:Literal ID="lit_UserName" runat="server"></asp:Literal></div></td>
                            <td class="td-content td-align-center"><div style="width:100px;"><asp:Literal ID="lit_NickName" runat="server"></asp:Literal></div></td>
                            <td class="td-content text-center td-align-center"><div style="width:50px;"><asp:Literal ID="lit_PropertyGender" runat="server"></asp:Literal></div></td>
                            <td class="td-content td-align-center"><div style="width:100px;"><asp:Literal ID="lit_QQ" runat="server"></asp:Literal></div></td>
                            <td class="td-content td-align-center"><div style="width:100px;"><asp:Literal ID="lit_PropertyRoleName" runat="server"></asp:Literal></div></td>
                            <td class="td-content td-align-center"><div style="width:100px;"><asp:Literal ID="lit_Email" runat="server"></asp:Literal></div></td>
                            <td class="td-content td-align-center"><div style="width:100px;"><asp:Literal ID="lit_Tel" runat="server"></asp:Literal></div></td>
                            <td class="td-content td-align-center"><div style="width:100px;"><asp:Literal ID="lit_IDCard" runat="server"></asp:Literal></div></td>
                            <td class="td-content-last td-content-empty"></td>
                        </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <BS:PagerNavication ID="PagerNavication" PageSizeList="10,15,20,30,50,100" PageSize="10" runat="server" Refreshable="true" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
        </td>
    </tr>
</table>