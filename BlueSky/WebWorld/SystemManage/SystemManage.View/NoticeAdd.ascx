<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NoticeAdd.ascx.cs" Inherits="WebWorld.SystemManage.NoticeAdd" %>

<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>公告标题：</td>
        <td><input type="text" id="txt_Title" class="txt-normal" runat="server" size="50" /></td>
    </tr>
    <tr>
        <td align="right">开放时间：</td>
        <td><input type="text" class="txt-normal" id="txt_BeginTime" runat="server" /></td>
    </tr>
    <tr>
        <td align="right">结束时间：</td>
        <td><input type="text" class="txt-normal" id="txt_EndTime" runat="server" /></td>
    </tr>
    <tr>
        <td align="right">公告内容：</td>
        <td><textarea class="txt-mutil" id="txt_Content" runat="server" cols="70" rows="15"></textarea></td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td colspan="2" align="center">
            <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.layout.closeActiveWindow();"/>
        </td>
    </tr>
</table>
<script type="text/javascript">
    function save() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Title.ClientID %>", message: "公告标题不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>