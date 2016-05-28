<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionsManage.ascx.cs" Inherits="WebWorld.Modules.CommonSystemManage.FunctionsManage" %>
<link href="Include/css/menu.css" rel="stylesheet" type="text/css" />
<script src="Include/js/menu.js" type="text/javascript"></script>
<script src="Include/js/contextmenu.js" type="text/javascript"></script>
<link href="Include/css/contextmenu.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(document).ready(function(e) {
        document.oncontextmenu = function() { return false; }
        $("span.tree-node-head-normal").each(function() {
            var menuInstance = this;
            $(this).mousedown(function(e) {
                var e = e ? e : window.event;
                if (e.button == 2 || e.button == 3) {
                    buttonClick(menuInstance, e);
                }
            });
        });
    });
    function nodeClick(_nodeRef) {
        if (null == _nodeRef || undefined == _nodeRef)
            return;
        var nodeType = _nodeRef.getAttribute("_type");
        if (nodeType == "son")
            return;
        if (nodeType == "parent") {
            //父节点图片的切换
            var currentSelectFolder = $(_nodeRef).parent().find("span[class^='tree-node-folder']");
            if (null != currentSelectFolder && currentSelectFolder.length > 0) {
                __toggleFolder(currentSelectFolder[0]);
            }

            //切换展开和收缩图标
            var currentSelectPlus = $(_nodeRef).parent().find("span[class*='tree-node-dot']");
            if (null != currentSelectPlus && currentSelectPlus.length > 0) {
                __togglePlusMinus(currentSelectPlus[0]);
            }

            //子菜单的显示与隐藏
            var currentSelectSons = $(_nodeRef).parent().next();
            if (null != currentSelectSons && currentSelectSons.length > 0) {
                if (getClass(currentSelectSons[0]) == "tree-nodes-box") {
                    $(currentSelectSons[0]).toggle();
                }
            }
        }
    }
    
    function buttonClick(nodeRef, e) {
        if (null == nodeRef || undefined == nodeRef)
            return;
        var strFuncId = nodeRef.getAttribute("_tag");
        if (null == strFuncId || "" == strFuncId || undefined == strFuncId)
            return;
        var contextMenu = new bs_contextMenu();
        contextMenu.targetWindow = this.window;
        
        var contextItemAdd = new bs_contextMenuItem();
        contextItemAdd.text = "新增";
        contextItemAdd.clickFunction = function() {
            var strAddUrl = "Window.aspx?value=FunctionControls/SystemManage/FunctionAdd.ascx&parentid=" + strFuncId;
            top.windowFactory.topFocusForm({
                title: "新增功能",
                url: strAddUrl,
                width: 600,
                height: 500
            });
        }
        contextMenu.addItem(contextItemAdd.createItem());

        var contextSplitLine = new bs_contextSplitLine();
        contextMenu.addItem(contextSplitLine.createLine());

        var contextItemEdit = new bs_contextMenuItem();
        contextItemEdit.text = "编辑";
        contextItemEdit.clickFunction = function() {
            var strEditUrl = "Window.aspx?value=FunctionControls/SystemManage/FunctionAdd.ascx&id=" + strFuncId;
            top.windowFactory.topFocusForm({
                title: "编辑功能",
                url: strEditUrl,
                width: 600,
                height: 500
            });
        }
        contextMenu.addItem(contextItemEdit.createItem());

        var contextItemDelete = new bs_contextMenuItem();
        contextItemDelete.text = "删除";
        contextItemDelete.clickFunction = function() {
            var strDeleteUrl = "Window.aspx?value=FunctionControls/SystemManage/FunctionDelete.ascx&id=" + strFuncId;
            top.windowFactory.topFocusForm({
                title: "删除功能提示",
                url: strDeleteUrl,
                width: 300,
                height: 200
            });
        }
        contextMenu.addItem(contextItemDelete.createItem());
        contextMenu.showMenu(e);
    }

    function addRootNode() {
        var strAddUrl = "Window.aspx?value=FunctionControls/SystemManage/FunctionAdd.ascx";
        top.windowFactory.topFocusForm({
            title: "新增功能",
            url: strAddUrl,
            width: 600,
            height: 500
        });
    }
</script>
<div style="text-align:left;" class="box"><input type="button" class="btn-normal" value="新增根节点" onclick="addRootNode();" /></div>
<%=strTree %>