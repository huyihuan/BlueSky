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
            splitPanel: [{ title: "系统功能列表", width: "200", content: "#treeContainer" }, { title: "功能操作", content: "请选择左边系统功能！"}]
        });
    });

    function nodeClick(parentNode) {
        if (null == parentNode || undefined == parentNode)
            return;
        var nodeType = parentNode.getAttribute("_type");
        var title = parentNode.getAttribute("_name");
        var nId = parentNode.getAttribute("_tag");
        if (nodeType == "son") {
            var strReUrl = strBaseUrl + "&setfn=" + nId;
            if (panel.splitPanel[1].url == strReUrl)
                return;
            panel.splitPanel[1].title = title + "-功能操作";
            panel.splitPanel[1].url = strReUrl;
            panel.splitPanel[1].content = null;
            panel.setSplitPanel(panel.splitPanel[1]);
        }
    }
</script>
<div id="treeContainer" style="width:100%;height:100%;"><%=strTree %></div>