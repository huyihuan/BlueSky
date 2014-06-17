<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActionAdd.ascx.cs" Inherits="WebWorld.SystemManage.ActionAdd" %>
<script type="text/javascript">
    function selectActionImage() {
        var windowArguments = new Object();
        windowArguments.width = 400;
        windowArguments.height = 400;
        windowArguments.title = "选择操作图标";
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
        <td align="right" nowrap>所属功能名称：</td>
        <td><asp:Label ID="lbl_FunctionNmae" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>操作名称：</td>
        <td><input type="text" class="txt-normal" id="txt_Name" runat="server" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>Key：</td>
        <td><input type="text" class="txt-normal" id="txt_Key" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>控件名称：</td>
        <td><input type="text" class="txt-normal" id="txt_ControlName" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>操作类型：</td>
        <td>
            <asp:DropDownList runat="server" ID="sel_ActionType" AutoPostBack="true" OnSelectedIndexChanged="sel_ActionType_SelectedIndexChanaged"></asp:DropDownList>
            <input type="text" class="txt-normal" id="txt_ActionValue" runat="server" size="15" />
        </td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>操作实体数量：</td>
        <td><input type="text" class="txt-normal" id="txt_EntityCount" runat="server" size="10" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>操作图标：</td>
        <td>
            <input type="text" class="txt-normal" id="txt_IconName" runat="server" size="15" /><input type="button" id="btn_SelectActionImage" class="btn-normal btn-select-noleft" onclick="selectActionImage();" title="点击选择操作图片" value="..." />
            <img runat="server" id="img_IconName" align="absMiddle" style="width:16px;height:16px;" />
        </td>
    </tr>
    <tr>
        <td align="right">操作提示：</td>
        <td><input type="text" class="txt-normal" id="txt_Tip" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right">操作描述：</td>
        <td><input type="text" class="txt-normal" id="txt_Description" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right">是否默认操作？</td>
        <td><input type="checkbox" runat="server" id="cb_IsDefault" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>排序：</td>
        <td><input type="text" class="txt-normal" id="txt_OrderId" runat="server" size="10" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <fieldset class="fieldset-normal">
                <legend>&nbsp;是否弹出窗口？&nbsp;<input type="checkbox" runat="server" id="cb_IsPopup" />&nbsp;</legend>
                <table cellpadding="0" cellspacing="5" border="0" width="100%">
                    <tr>
                        <td colspan="2">宽：<input type="text" class="txt-normal" id="txt_Width" runat="server" size="10" />（单位：像素，最小值150）</td>
                    </tr>
                    <tr>
                        <td colspan="2">高：<input type="text" class="txt-normal" id="txt_Height" runat="server" size="10" />（单位：像素，最小值150）</td>
                    </tr>
                    <tr>
                        <td width="50%">是否可改变大小？<input type="checkbox" runat="server" id="cb_IsResize" /></td>
                        <td width="50%">是否可移动？<input type="checkbox" runat="server" id="cb_IsMove" /></td>
                    </tr>
                    <tr>
                        <td>是否最小化？<input type="checkbox" runat="server" id="cb_IsIncludeMinBox" /></td>
                        <td>是否最大化？<input type="checkbox" runat="server" id="cb_IsIncludeMaxBox" /></td>
                    </tr>
                    <tr>
                        <td>最否显示在任务栏？<input type="checkbox" runat="server" id="cb_IsShowInTaskBar" /></td>
                        <td></td>
                    </tr>
                </table>
            </fieldset>
        </td>
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
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Name.ClientID %>", message: "操作名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Key.ClientID %>", message: "Key不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_ControlName.ClientID %>", message: "控件名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_EntityCount.ClientID %>", message: "操作实体数量不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_IconName.ClientID %>", message: "图标不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=sel_ActionType.ClientID %>", message: "操作类型不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>