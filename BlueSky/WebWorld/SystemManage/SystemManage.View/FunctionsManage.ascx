<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionsManage.ascx.cs" Inherits="WebWorld.SystemManage.FunctionsManage" %>
<link href="Include/css/menu.css" rel="stylesheet" type="text/css" />
<script src="Include/js/menu.js" type="text/javascript"></script>
<script type="text/javascript">
    //    var listContainer = null;
    var panel = null;
    var strBaseUrl = "";
    $(document).ready(function() {
//        var docSize = Utils.documentSize(document);
//        listContainer = document.getElementById("listContainer");
//        listContainer.style.overflow = "auto";
//        listContainer.style.height = docSize.height - 30 + "px";
        strBaseUrl = "<%=strActionUrl %>";
        panel = new BlueSky.Panel();
        panel.init({
            parent: "panelContainer",
            width: "100%",
            height: "100%",
            splitPanel: [{ title: "系统功能管理", width: "400", content: "#tbFunctionManage" }, { title: "功能操作管理", content: "请选择左边的功能树！"}]
        });
    });

//    window.onresize = function() {
//        var docSize = Utils.documentSize(document);
//        listContainer.style.height = docSize.height - 30 + "px";
//    }

    function nodeClick(_nodeRef) {
        if (null == _nodeRef || undefined == _nodeRef)
            return;
        var strId = _nodeRef.getAttribute("_tag");
        if (undefined == strId || "" == strId || null == strId)
            return;
        $("#hiddenExtraParameters").val("parentid=" + strId);
        formUtil.setSelectedItems(strId);
        var nodeType = _nodeRef.getAttribute("_type");
        if (nodeType == "son") {
            var title = _nodeRef.getAttribute("_name");
            var strReUrl = strBaseUrl + "&setfn=" + strId;
            if (panel.splitPanel[1].url == strReUrl)
                return;
            panel.splitPanel[1].title = title + "-功能操作管理";
            panel.splitPanel[1].url = strReUrl;
            panel.splitPanel[1].content = null;
            panel.setSplitPanel(panel.splitPanel[1]);
        }
    }
</script>
<div id="panelContainer" class="content">
    <table id="tbFunctionManage"  cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td><div id="toolBar" runat="server"></div></td>
        </tr>
        <tr>
            <td valign="top">
                    <%=strTree %>
            </td>
        </tr>
    </table>
</div>

