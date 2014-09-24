<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionSelect.ascx.cs" Inherits="WebWorld.SystemManage.FunctionSelect" %>
<%@ Register TagPrefix="BS" Namespace="WebBase.UserControls" Assembly="WebBase" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 62, resize: false });
    });
</script>
<table  cellpadding="0" cellspacing="0" height="100%" width="100%">
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td width="50%" valign="middle">
            &nbsp;<input type="button" value="返 回" class="btn-normal" onclick="top.layout.closeActiveWindow();"  />
        </td>
        <td width="50%" align="right"><input id="btn_OK" type="button" value="确 定" onclick="save();" class="btn-normal" />&nbsp;</td>
    </tr>
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td valign="top" colspan="2">
        <div class="list-container"></div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <BS:PagerNavication ID="PagerNavication" PageSizeList="10,30,50" PageSize="10" runat="server"></BS:PagerNavication>
        </td>
    </tr>
</table>
<input type="hidden" runat="server" id="hiddenFunctionSelected" />