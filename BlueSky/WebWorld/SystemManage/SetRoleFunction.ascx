<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetRoleFunction.ascx.cs" Inherits="WebWorld.SystemManage.SetRoleFunction" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 33, resize: true });
        window.formUtil.initCheckBox({ listElement: ".table-list", rememberValue: false, rowHover: true, rowClick: true, initStatus: true });
    });

    function selectAction(_url, _title) {
        var windowArguments = new Object();
        windowArguments.width = 400;
        windowArguments.height = 200;
        windowArguments.title =_title + "-操作权限管理";
        windowArguments.url = _url;
        windowArguments.resize = true;
        windowArguments.move = true;
        top.windowFactory.topFocusForm(windowArguments);
    }
</script>

<table  cellpadding="0" cellspacing="0" height="100%" width="100%">
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td width="50%" align="left">&nbsp;<input type="button" value="返 回" class="btn-normal" onclick="top.windowFactory.closeTopFocusForm();"  /></td>
        <td width="50%" align="right"><input id="btn_Save" type="button" value="保 存" class="btn-normal" runat="server" onserverclick="btnSave_Click" />&nbsp;</td>
    </tr>
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td colspan="2" valign="top">
            <div class="list-container">
                <table class="table-list table-list-topline" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" class="td-header td-header-select"><input type="checkbox" class="cbSelectAll" /></td>
                        <td align="center" class="td-header td-header-order">序号</td>
                        <td align="center" class="td-header-last">权限名称</td>
                    </tr>
                    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td align="center" class="td-content td-content-select"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                            <td align="center" class="td-content td-content-order"><asp:Label ID="lbl_OrderId" runat="server"></asp:Label></td>
                            <td class="td-content-last"><asp:Label ID="lbl_FunctionName" runat="server"></asp:Label></td>
                        </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </td>
    </tr>
</table>