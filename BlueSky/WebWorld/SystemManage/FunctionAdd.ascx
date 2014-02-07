<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionAdd.ascx.cs" Inherits="WebWorld.SystemManage.FunctionAdd" %>
<script type="text/javascript">
    function selectActionImage() {
        var windowArguments = new Object();
        windowArguments.width = 400;
        windowArguments.height = 400;
        windowArguments.title = "选择功能图标";
        windowArguments.url = "<%=strImageFormUrl %>";
        windowArguments.resize = false;
        windowArguments.move = true;
        var imgSelect = document.getElementById("<%=img_IconName.ClientID %>");
        if (imgSelect.src && "" != imgSelect.src) {
            windowArguments.windowArguments = { src: imgSelect.src, imageName: imgSelect.title }
        }
        windowArguments.closeFunction = function(_windowResult, _windowReturnValue) {
            if (_windowResult == top.windowFactory.windowResult.cancel)
                return;
            document.getElementById("<%=txt_IconName.ClientID %>").value = _windowReturnValue.imageName;
            imgSelect.src = _windowReturnValue.src;
            imgSelect.title = _windowReturnValue.imageName;
        }
        top.windowFactory.topFocusForm(windowArguments);
    }
</script>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap>父级功能名称：</td>
        <td><asp:Label ID="lbl_ParentNmae" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>功能名称：</td>
        <td><input type="text" class="txt-normal" id="txt_Name" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>Key：</td>
        <td><input type="text" class="txt-normal" id="txt_Key" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right">功能图标：</td>
        <td>
            <input type="text" class="txt-normal" id="txt_IconName" runat="server" size="20" />
            <input type="button" id="btn_SelectActionImage" class="btn-normal" onclick="selectActionImage();" title="点击选择功能图标" value="..." />
            <img runat="server" id="img_IconName" align="absMiddle" style="width:16px;height:16px;" />
        </td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>功能排序：</td>
        <td><input type="text" class="txt-normal" id="txt_OrderId" runat="server" size="5" /></td>
    </tr>
    <tr>
        <td align="right">功能描述：</td>
        <td><textarea class="txt-mutil" id="txt_Description" runat="server" style="width:200px;height:100px;"></textarea></td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td colspan="2" align="center">
            <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.windowFactory.closeTopFocusForm();" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    function save() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Name.ClientID %>", message: "功能名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Key.ClientID %>", message: "Key不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_OrderId.ClientID %>", message: "排序不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>