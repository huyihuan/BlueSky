<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionAdd.ascx.cs" Inherits="WebWorld.FunctionControls.SystemManage.FunctionAdd" %>
<table cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap>父级功能名称：</td>
        <td><asp:Label ID="lbl_ParentNmae" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>功能名称：</td>
        <td><input type="text" class="txt-normal" id="txt_FunctionName" runat="server" /></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>功能地址：</td>
        <td><input type="text" class="txt-normal" id="txt_Value" runat="server" size="30" /></td>
    </tr>
    <tr>
        <td align="right">功能提示：</td>
        <td><input type="text" class="txt-normal" id="txt_Tip" runat="server" size="25" /></td>
    </tr>
    <tr>
        <td align="right">功能图标：</td>
        <td><input type="text" class="txt-normal" id="txt_Image" runat="server" size="15" /></td>
    </tr>
    <tr>
        <td align="right">打开目标：</td>
        <td><asp:DropDownList ID="sel_Target" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>宽：</td>
        <td valign="middle"><input type="text" class="txt-normal" id="txt_Width" runat="server" size="10" />（像素，最小值150）</td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>高：</td>
        <td valign="middle"><input type="text" class="txt-normal" id="txt_Height" runat="server" size="10" />（像素，最小值150）</td>
    </tr>
    <tr>
        <td align="right">可改变大小？</td>
        <td>
            <input type="radio" name="rb_IsResize" id="rb_IsResizeNo" runat="server" value="0" checked="true"><label for="<%=rb_IsResizeNo.ClientID %>">&nbsp;否</label>&nbsp;
            <input type="radio" name="rb_IsResize" id="rb_IsResizeYes" runat="server" value="1"><label for="<%=rb_IsResizeYes.ClientID %>">&nbsp;是</label>
        </td>
    </tr>
    <tr>
        <td align="right">可移动？</td>
        <td>
            <input type="radio" name="rb_IsToMove" id="rb_IsToMoveNo" runat="server" value="0" checked="true"><label for="<%=rb_IsToMoveNo.ClientID %>">&nbsp;否</label>&nbsp;
            <input type="radio" name="rb_IsToMove" id="rb_IsToMoveYes" runat="server" value="1"><label for="<%=rb_IsToMoveYes.ClientID %>">&nbsp;是</label>
        </td>
    </tr>
    <tr>
        <td align="right">显示在任务栏？</td>
        <td>
            <input type="radio" name="rb_IsShowInTaskBar" id="rb_IsShowInTaskBarNo" runat="server" value="0" checked="true"><label for="<%=rb_IsShowInTaskBarNo.ClientID %>">&nbsp;否</label>&nbsp;
            <input type="radio" name="rb_IsShowInTaskBar" id="rb_IsShowInTaskBarYes" runat="server" value="1"><label for="<%=rb_IsShowInTaskBarYes.ClientID %>">&nbsp;是</label>
        </td>
    </tr>
    <tr>
        <td align="right">最小化？</td>
        <td>
            <input type="radio" name="rb_IsIncludeMinBox" id="rb_IsIncludeMinBoxNo" runat="server" value="0" checked="true"><label for="<%=rb_IsIncludeMinBoxNo.ClientID %>">&nbsp;否</label>&nbsp;
            <input type="radio" name="rb_IsIncludeMinBox" id="rb_IsIncludeMinBoxYes" runat="server" value="1"><label for="<%=rb_IsIncludeMinBoxYes.ClientID %>">&nbsp;是</label>
        </td>
    </tr>
    <tr>
        <td align="right">最大化？</td>
        <td>
            <input type="radio" name="rb_IsIncludeMaxBox" id="rb_IsIncludeMaxBoxNo" runat="server" value="0" checked="true"><label for="<%=rb_IsIncludeMaxBoxNo.ClientID %>">&nbsp;否</label>&nbsp;
            <input type="radio" name="rb_IsIncludeMaxBox" id="rb_IsIncludeMaxBoxYes" runat="server" value="1"><label for="<%=rb_IsIncludeMaxBoxYes.ClientID %>">&nbsp;是</label>
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
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_FunctionName.ClientID %>", message: "功能名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Value.ClientID %>", message: "功能地址不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Width.ClientID %>", message: "宽不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Height.ClientID %>", message: "高不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>