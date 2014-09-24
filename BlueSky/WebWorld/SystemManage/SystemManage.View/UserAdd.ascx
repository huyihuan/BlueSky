<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAdd.ascx.cs" Inherits="WebWorld.SystemManage.UserAdd" %>
<script type="text/javascript">
    var tabsUserInformation, wrapper, tabWrapper, remark;
    Bluesky.ready(function() {
        wrapper = Bluesky("#tabs");
        var bIsPermission = "<%=bIsPermission %>".toLowerCase() == "true";
        var sizeW = { width: wrapper.width(), height: wrapper.height() };
        tabWrapper = Bluesky("#tabs_node");
        tabsUserInformation = Bluesky.component.Tabs.create({
            renderTo: "#tabs_node",
            width: sizeW.width - 10,
            height: sizeW.height - 50,
            activeIndex: 0,
            sliding: true,
            viewstate: "#<%=hidden_TabsViewState.ClientID %>",
            iconFolder: "include\\image\\icons",
            items:  [
				{
				    title: "基本信息",
				    sliding: true,
				    showIcon: true,
				    iconURL: "information.png",
				    contentElement: "#<%=tab_baseInformation.ClientID %>",
				    closeable: false
				},
				{
				    title: "密码设置",
				    sliding: true,
				    showIcon: true,
				    iconURL: "information.png",
				    contentElement: "#<%=tab_password.ClientID %>",
				    closeable: false
				},
				{
				    title: "角色设置",
				    sliding: true,
				    showIcon: true,
				    iconURL: "group.png",
				    contentElement: "#tab_role",
				    closeable: false,
				    disabled: !bIsPermission
				},
				{
				    title: "权限",
				    showIcon: true,
				    iconURL: "key.png",
				    closeable: false,
				    disabled: !bIsPermission,
				    loader: {
				        url: "<%=strPermissionURL %>",
				        autoLoad: false
				    }
				},
				{
				    title: "备注",
				    tip: "备注",
				    sliding: true,
				    showIcon: true,
				    closeable: false,
				    iconURL: "page_white_edit.png",
				    contentElement:"#<%=txt_Remark.ClientID %>"
				}
			]
        });
        remark = Bluesky("#<%=txt_Remark.ClientID %>").css("height", sizeW.height - 93).css("width", sizeW.width - 20);
    });

    Bluesky(window).addEvent("resize", function() {
        var size = { width: wrapper.width() - 10, height: wrapper.height() - 50 };
        tabsUserInformation.resize(size);
        tabWrapper.css("height", size.height).css("width", size.width);
        remark.css("height", size.height - 43).css("width", size.width - 10);
    });
</script>
<input type="hidden" id="hidden_TabsViewState" runat="server" />
<div id="tabs" style="width:100%;height:100%;">
    <div id="tabs_node" style="padding:5px;"></div>
    <div style="width:100%;height:28px;margin-bottom:3px;line-height:28px;text-align:right;">
        <input type="button" id="btn_Save" class="btn-normal" runat="server" value=" 保 存 " onclick="if(!save()) return false;" onserverclick="btnSave_ServerClick" />
        <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" 返 回 " onclick="top.layout.closeActiveWindow();" />&nbsp;
    </div>
</div>
<table id="tab_baseInformation" cellpadding="5" cellspacing="5" border="0" width="100%" runat="server">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" nowrap><font class="font-hint">*</font>账户名称：</td>
        <td><input type="text" id="txt_UserName" runat="server" class="txt-normal" /><asp:Label ID="lbl_UserName" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td align="right"><font class="font-hint">*</font>真实姓名：</td>
        <td><input type="text" class="txt-normal" id="txt_NickName" runat="server" /></td>
    </tr>
    <tr>
        <td align="right">性别：</td>
        <td>
            <input type="radio" name="rb_Gender" id="rb_GenderMale" runat="server" value="男" checked="true"><label for="<%=rb_GenderMale.ClientID %>">&nbsp;男</label>&nbsp;
            <input type="radio" name="rb_Gender" id="rb_GenderFemale" runat="server" value="女"><label for="<%=rb_GenderFemale.ClientID %>">&nbsp;女</label>
        </td>
    </tr>
    <tr>
        <td align="right">QQ：</td>
        <td><input type="text" class="txt-normal" id="txt_QQ" runat="server" size="20" /></td>
    </tr>
    <tr>
        <td align="right">Email：</td>
        <td><input type="text" class="txt-normal" id="txt_Email" runat="server" size="25" /></td>
    </tr>
    <tr>
        <td align="right">身份证号：</td>
        <td><input type="text" class="txt-normal" id="txt_CardID" runat="server" size="25" /></td>
    </tr>
    <tr>
        <td align="right">邮编：</td>
        <td><input type="text" class="txt-normal" id="txt_PostCode" runat="server" size="6" /></td>
    </tr>
    <tr>
        <td align="right">MSN：</td>
        <td><input type="text" class="txt-normal" id="txt_MSN" runat="server" size="25" /></td>
    </tr>
    <tr>
        <td align="right">Tel：</td>
        <td><input type="text" class="txt-normal" id="txt_Tel" runat="server" size="25" /></td>
    </tr>
    <tr>
        <td align="right">住址：</td>
        <td><input type="text" class="txt-normal" id="txt_Address" runat="server" size="25" /></td>
    </tr>
</table>
<table id="tab_password" runat="server" cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr>
        <td colspan="2">
            <fieldset class="fieldset-normal">
                <legend>&nbsp;密码修改说明&nbsp;</legend>
                <br />
                <p>1、如果新增用户，不填写密码则系统默认为“<b>123456</b>”。</p>
                <p>2、如果需要修改密码，请填写密码、确认密码。</p>
                <br />
            </fieldset>
        </td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right">密 码：</td>
        <td><input type="text" id="txt_Password" runat="server" size="35" class="txt-normal" /></td>
    </tr>
    <tr><td colspan="2" height="15"></td></tr>
    <tr>
        <td align="right" width="20%">确认密码：</td>
        <td><input type="text" id="txt_Password2" runat="server" size="35" class="txt-normal" /></td>
    </tr>
</table>
<table id="tab_role" cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td colspan="5" height="10"></td></tr>
    <tr>
        <td><asp:ListBox ID="lb_RoleList" runat="server" Width="100" Height="150" ToolTip="可选择角色列表"></asp:ListBox></td>
        <td width="3"></td>
        <td valign="middle">
            <input type="button" id="btnSelectRole" class="btn-normal" runat="server" value=">" onserverclick="btnSelectRole_Click" /><br /><br />
            <input type="button" id="btnRemoveRole" class="btn-normal" runat="server" value="<" onserverclick="btnRemoveRole_Click" />
        </td>
        <td width="3"></td>
        <td><asp:ListBox ID="lb_RoleSelect" runat="server" Width="100" Height="150" ToolTip="已选择角色列表"></asp:ListBox></td>
    </tr>
</table>
<textarea class="txt-mutil" id="txt_Remark" runat="server" cols="10" rows="10"></textarea>
<script type="text/javascript">
    function save() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_UserName.ClientID %>", message: "账户名称不能为空！", ishint: true })) {
            tabsUserInformation.setActiveTab(0);
            return false;
        }
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_NickName.ClientID %>", message: "真实姓名不能为空！", ishint: true })) {
            tabsUserInformation.setActiveTab(0);
            return false;
        }
        return true;
    }
</script>