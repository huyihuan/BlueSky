<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MusicAdd.ascx.cs" Inherits="WebWorld.Modules.MyMusic.View.MusicAdd" %>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>音乐名称：</td>
        <td><input type="text" id="txt_MusicName" runat="server" class="txt-normal" /><asp:Label ID="lbl_UserName" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>文件目录：</td>
        <td><input type="text" class="txt-normal" id="txt_MusicURL" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right">音乐类型：</td>
        <td><input type="text" class="txt-normal" id="txt_MusicType" runat="server" size="20" /></td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.layout.closeActiveWindow();" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    function save() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_MusicName.ClientID %>", message: "音乐名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_MusicURL.ClientID %>", message: "音乐目录不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_MusicType.ClientID %>", message: "音乐类型不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>