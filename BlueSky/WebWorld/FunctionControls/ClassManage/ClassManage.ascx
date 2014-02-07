<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClassManage.ascx.cs" Inherits="WebWorld.FunctionControls.ClassManage.ClassManage" %>
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
            <td><div id="toolBar" runat="server">&nbsp;<input type="button" id="btnSetFunctions" value=" 添加班级 " class="btn-normal" onclick='window.location.href = "Window.aspx?value=FunctionControls/ClassManage/ClassAdd.ascx";' /></div></td>
        </tr>
        <tr>
            <td valign="top">
                <div id="listContainer" style="overflow:auto;">
                    <table class="table-list-top" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" class="td-header">序号</td>
                            <td align="center" class="td-header">班级名称</td>
                            <td align="center" class="td-header-last">人数</td>
                        </tr>
                        <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td align="center" class="td-content"><asp:Label ID="lbl_OrderId" runat="server"></asp:Label></td>
                                <td align="center" class="td-content"><asp:Label ID="lbl_ClassName" runat="server"></asp:Label></td>
                                <td align="center" class="td-content-last"><asp:Label ID="lbl_StudentNumber" runat="server"></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <BS:PagerNavication ID="PagerNavication" PageSizeList="10,20,50" PageSize="10" runat="server" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
            </td>
        </tr>
    </table>
</div>