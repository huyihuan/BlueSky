<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyMusicManage.ascx.cs" Inherits="WebWorld.Modules.MyMusic.View.MyMusicManage" %>
<%@ Register TagPrefix="BS" Namespace="WebBase.UserControls" Assembly="WebBase" %>
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
                        <td align="center" class="td-header" width="150">音乐名称</td>
                        <td class="td-header" width="200">存储位置</td>
                        <td align="center" class="td-header" width="150">音乐类型</td>
                        <td class="td-header-last"></td>
                    </tr>
                    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td align="center" class="td-content td-content-select"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                            <td align="center" class="td-content td-content-order"><asp:Label ID="lbl_OrderId" runat="server"></asp:Label></td>
                            <td class="td-content"><div style="width:150px;"><asp:Label ID="lbl_MusicName" runat="server"></asp:Label></div></td>
                            <td class="td-content"><div style="width:200px;"><asp:Label ID="lbl_MusicURL" runat="server"></asp:Label></div></td>
                            <td class="td-content"><div style="width:150px;"><asp:Label ID="lbl_MusicType" runat="server"></asp:Label></div></td>
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
            <BS:PagerNavication ID="PagerNavication" PageSizeList="10,15,20,30,50,100" PageSize="10" Refreshable="true" runat="server" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
        </td>
    </tr>
</table>