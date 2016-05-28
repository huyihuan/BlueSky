<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetRoleFunction.ascx.cs" Inherits="WebWorld.Modules.CommonSystemManage.SetRoleFunction" %>
<script type="text/javascript">
    var listContainer = null;
    $(document).ready(function() {
        var docSize = Utils.documentSize(document);
        listContainer = document.getElementById("listContainer");
        listContainer.style.height = docSize.height - 26 + "px";
    });

    window.onresize = function() {
        var docSize = Utils.documentSize(document);
        listContainer.style.height = docSize.height - 26 + "px";
    }
</script>
<div class="content">
    <table  cellpadding="0" cellspacing="0" height="100%" width="100%">
        <tr>
            <td><div class="toolBar"><input type="button" value="返 回" class="btn-normal" onclick="window.history.back(-1);" /><input type="button" value="确定" class="btn-normal" runat="server" onserverclick="btnSave_Click" /></div></td>
        </tr>
        <tr>
            <td valign="top">
                <div id="listContainer" style="overflow:auto;">
                    <table class="table-list-top" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" class="td-header"><input type="checkbox" class="cbSelectAll" /></td>
                            <td align="center" class="td-header">序号</td>
                            <td align="center" class="td-header-last">权限名称</td>
                        </tr>
                        <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td align="center" class="td-content"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                                <td align="center" class="td-content"><asp:Label ID="lbl_OrderId" runat="server"></asp:Label></td>
                                <td class="td-content-last"><asp:Label ID="lbl_FunctionName" runat="server"></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>