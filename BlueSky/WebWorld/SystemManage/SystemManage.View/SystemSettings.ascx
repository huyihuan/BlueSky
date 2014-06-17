<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemSettings.ascx.cs" Inherits="WebWorld.SystemManage.SystemSettings" %>
<script type="text/javascript">
    Bluesky.ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 28, resize: true });
    });

    var tabsSettings, wrapper, tabsWrapper;
    Bluesky.ready(function() {
        tabsSettings = Bluesky.component.Tabs.create({
            renderTo: "#tabs_Wrapper",
            width: 600,
            height: 450,
            activeIndex: 0,
            sliding: false,
            viewstate: "#<%=hidden_TabsViewState.ClientID %>",
            iconFolder: "include\\image\\icons",
            items: [
				{
				    title: "Database Operate",
				    showIcon: true,
				    iconURL: "information.png",
				    contentElement: "#tab_DatabaseOper",
				    closeable: false
				},
				{
				    title: "Database Settings",
				    sliding: true,
				    showIcon: true,
				    iconURL: "group.png",
				    contentElement: "#tab_Settings",
				    closeable: false
				}
			]
        });
    });
</script>
<input type="hidden" id="hidden_TabsViewState" runat="server" />
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td><div id="toolBar" runat="server"></div></td>
    </tr>
    <tr>
        <td align="center" valign="top">
            <div class="list-container">
                <div  id="tabs_Wrapper" style="margin:10px;"></div>
            </div>
        </td>
    </tr>
</table>
<table id="tab_DatabaseOper" cellpadding="5" cellspacing="5" border="0" width="100%">
    <tr><td height="5"></td></tr>
    <tr>
        <td>&nbsp;Please Input The Command Of Operating Database：<font class="font-hint"><asp:Literal ID="lit_Message" runat="server"></asp:Literal></font></td>
    </tr>
    <tr>
        <td><textarea id="txt_OperSql" runat="server" style="width:500px;height:300px;" class="txt-mutil"></textarea></td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            <input type="button" id="btn_Go" class="btn-normal" runat="server" value=" GO " onclick="if(!go()) return false;" onserverclick="btnGo_ServerClick" />
            <input type="button" id="btn_Cancel" class="btn-normal" runat="server" value=" Cancel " onserverclick="btn_Cancel_ServerClick"/>&nbsp;
        </td>
    </tr>
</table>
<table id="tab_Settings" cellpadding="5" cellspacing="5" border="0">
    <tr><td height="5"></td></tr>
    <tr>
        <td>DatabaseSettings</td>
    </tr>
    <tr>
        <td><input type="button" value="Settings" onclick="if(!go()) return false;"</td>
    </tr>
</table>
<script type="text/javascript">
    function go() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_OperSql.ClientID %>", message: "Please Input The Command！", ishint: true })) {
            tabsSettings.setActiveTab(0);
            return false;
        }
        return true;
    }
</script>