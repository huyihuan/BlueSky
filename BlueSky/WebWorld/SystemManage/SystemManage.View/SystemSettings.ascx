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
                    title: "Debugging",
                    showIcon: true,
                    iconURL: "information.png",
                    contentElement: "#tab_Debugging",
                    closeable: false
                },
				{
				    title: "Database Operate",
				    showIcon: true,
				    iconURL: "information.png",
				    contentElement: "#tab_DatabaseOper",
				    closeable: false
				},
				{
				    title: "Cache",
				    sliding: true,
				    showIcon: true,
				    iconURL: "group.png",
				    contentElement: "#tab_CacheSettings",
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
<table id="tab_Debugging" cellpadding="10" cellspacing="10" border="0" width="100%">
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td align="right" width="40%">Enable Debugging On：</td>
        <td><input type="checkbox" id="cb_EnableDebuggingOn" runat="server" onchange="fnEnableChanged();"/></td>
    </tr>
    <tr>
        <td align="right">Login UserName：</td>
        <td><input type="text" id="txt_DebuggUserName" runat="server" class="txt-normal" /></td>
    </tr>
    <tr>
        <td align="right">Login Password：</td>
        <td><input type="text" id="txt_DebuggPassword" runat="server" class="txt-normal" /></td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td colspan="2" align="center">
            <input type="button" id="btn_DebuggingOn" class="btn-normal" runat="server" value=" LoginTest" />
            <input type="button" id="btn_DebuggingSave" class="btn-normal" runat="server" value=" DebuggingSave " onclick="if(!fnDebuggingSave(this)) return false;" onserverclick="btn_DebuggingSave_Click"/>
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
<table id="tab_CacheSettings" cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr><td height="5"></td></tr>
    <tr>
        <td>
            <input type="button" id="btn_ClearCache" runat="server" class="btn-normal" value="Clear Cache" onserverclick="btn_ClearCache_Click" />
            <input type="button" id="btn_CacheMoniter" runat="server" class="btn-normal" value="CacheMoniter" onserverclick="btn_CacheMoniter_Click" />
            <input type="button" id="btn_ClearEntity" class="btn-normal" runat="server" value="Clear EntityCache" onserverclick="btn_ClearEntity_ServerClick" />
            <input type="button" id="btn_ClearList" class="btn-normal" runat="server" value="Clear EntityCache" onserverclick="btn_ClearList_ServerClick" />
        </td>
    </tr>
    <tr>
        <td><asp:Literal ID="lt_CacheInformation" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td>
            <table class="table-list table-list-topline" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="center" class="td-header td-header-select"><input type="checkbox" class="cbSelectAll" /></td>
                    <td align="center" class="td-header td-header-order">序号</td>
                    <td align="center" class="td-header">缓存类型</td>
                    <td align="center" class="td-header">缓存名称</td>
                    <td align="center" class="td-header-last">数量</td>
                </tr>
                <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td align="center" class="td-content td-content-select"><input type="checkbox" id="cbSelect" runat="server" class="cbSelect" /></td>
                        <td align="center" class="td-content td-content-order"><asp:Literal ID="lt_OrderId" runat="server"></asp:Literal></td>
                        <td align="center" class="td-content"><asp:Literal ID="lt_Type" runat="server"></asp:Literal></td>
                        <td align="center" class="td-content"><asp:Literal ID="lt_EntityName" runat="server"></asp:Literal></td>
                        <td class="td-content-last"><asp:Literal ID="lt_CacheCount" runat="server"></asp:Literal></td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
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

    function fnEnableChanged() {
        var isEnabled = Bluesky("#<%=cb_EnableDebuggingOn.ClientID %>").checked();
        Bluesky("#<%=txt_DebuggUserName.ClientID %>").disabled(!isEnabled);
        Bluesky("#<%=txt_DebuggPassword.ClientID %>").disabled(!isEnabled);
    }

    function fnDebuggingSave(_sender) {
        if (Bluesky("#<%=cb_EnableDebuggingOn.ClientID %>").checked() == true) {
            if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_DebuggUserName.ClientID %>", message: "Please Input The Login UserName！", ishint: true })) {
                return false;
            }
            if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_DebuggPassword.ClientID %>", message: "Please Input The Login Password！", ishint: true })) {
                return false;
            }
        }
        if (_sender) {
            Bluesky(_sender).value("Waitting...");
            Bluesky(_sender).disabled(true);
        }
        return true;
    }
</script>