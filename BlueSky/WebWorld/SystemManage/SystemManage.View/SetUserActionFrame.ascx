<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetUserActionFrame.ascx.cs" Inherits="WebWorld.SystemManage.SetUserActionFrame" %>
<script type="text/javascript">
    var panel = null;
    var strBaseUrl = "";
    $(document).ready(function() {
        strBaseUrl = "<%=strActionUrl %>";
        panel = new BlueSky.Panel();
        panel.init({
            parent: document,
            width: "100%",
            height: "100%",
            splitPanel: [{ title: "系统功能列表", width: "200", content: "" }, { title: "功能操作", content: "请选择左边系统功能！"}]
        });

        var menuTree = new Bluesky.component.Tree({
            id: "SetUserActionMunu",
            renderTo: panel.getPanel(0),
            width: "100%",
            height: "100%",
            showCheckBox: false,
            showRootNode: false,
            loader: {
                url: "/Server/ServerRouting.ashx",
                params: { action: "GetFunctions" }
            },
            onNodeSelected: function(_node) {
                if (_node.childrenCount() == 0 && _node.data) {
                    var strReUrl = strBaseUrl + "&setfn=" + _node.value + "&r=" + Math.random()
                    if (panel.splitPanel[1].url == strReUrl)
                        return;
                    panel.splitPanel[1].title = _node.text + "-功能操作";
                    panel.splitPanel[1].url = strReUrl;
                    panel.splitPanel[1].content = null;
                    panel.setSplitPanel(panel.splitPanel[1]);
                }
            }
        });
        menuTree.init();
        
    });
</script>