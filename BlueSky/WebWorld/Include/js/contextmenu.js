/*
 * 右击菜单使用例子
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
            var strAddUrl = "Window.aspx?value=SystemManage/FunctionAdd.ascx&parentid=" + strFuncId;
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
            var strEditUrl = "Window.aspx?value=SystemManage/FunctionAdd.ascx&id=" + strFuncId;
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
            var strDeleteUrl = "Window.aspx?value=SystemManage/FunctionDelete.ascx&id=" + strFuncId;
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
 */

var bs_field_contextMenuClassName = "contextMenu";
var bs_field_contextMenuId = "contextMenu";
var bs_field_contextMenuItemClassName = "contextMenuItem";
function bs_contextMenu() {
    this.id = bs_field_contextMenuId;
    this.menuItems = new Array();
    this.height = 100;
    this.width = 80;
    this.targetWindow = window;
}
bs_contextMenu.prototype.addItem = function(_contextItem) {
    this.menuItems[this.menuItems.length] = _contextItem;
}
bs_contextMenu.prototype.showMenu = function(e) {
    this.hideMenu();
    var mPos = top.Utils.mousePosition(window, e);
    var x = mPos.X;
    var y = mPos.Y;
    var contextMenu = this.targetWindow.document.createElement("div");
    contextMenu.className = bs_field_contextMenuClassName;
    contextMenu.id = bs_field_contextMenuId;
    for (var i = 0; i < this.menuItems.length; i++) {
        contextMenu.appendChild(this.menuItems[i]);
    }

    contextMenu.style.width = this.width + "px";
    contextMenu.style.left = x + 2 + "px";
    contextMenu.style.height = "auto";
    contextMenu.style.top = y + 2 + "px";

    contextMenu.tabIndex = 1;
    contextMenu.hideFocus = "true";
    bs_event_AddEvent(contextMenu, "focus", focusContextMenu);
    setTimeout(function() { contextMenu.focus(); }, 0);
    this.targetWindow.document.body.appendChild(contextMenu);
}

function bs_event_AddEvent(_target, _eventType, _eventHandler) {
    if (_target.addEventListener) {
        _target.addEventListener(_eventType, _eventHandler, true);
    }
    else {
        _target.attachEvent("on" + _eventType, _eventHandler);
    }
}

function focusContextMenu(e) {
    var menuRef = document.getElementById(bs_field_contextMenuId);
    bs_event_AddEvent(menuRef, "blur",
        function() {
            var curDocument = document.activeElement;
            if (curDocument && curDocument != null) {
                if (curDocument == menuRef || curDocument.parentNode == menuRef || curDocument.parentNode.parentNode == menuRef)
                    return;
            }
            setTimeout(function() {
            menuRef = document.getElementById(bs_field_contextMenuId);
                if (null != menuRef && undefined != menuRef) {
                    menuRef.parentNode.removeChild(menuRef);
                }
            }, 1);
        }
    );
}

bs_contextMenu.prototype.hideMenu = function() {
    var contxtMenu = this.targetWindow.document.getElementById(this.id);
    if (null == contxtMenu || undefined == contxtMenu)
        return;
    contxtMenu.parentNode.removeChild(contxtMenu);
}


function bs_contextMenuItem() {
    this.id = "";
    this.text = "";
    this.clickFunction = function() { };
}
bs_contextMenuItem.prototype.createItem = function() {
    var contextMenuItem = document.createElement("div");
    contextMenuItem.className = bs_field_contextMenuItemClassName;
    contextMenuItem.innerHTML = "<a>" + this.text + "</a>";
    contextMenuItem.style.height = "auto";
    var clickFunc = this.clickFunction;
    if (null != clickFunc && undefined != clickFunc)
        contextMenuItem.onclick = function() {
            clickFunc();
            var contextMenu = document.getElementById(bs_field_contextMenuId);
            contextMenu.parentNode.removeChild(contextMenu);
        };
    return contextMenuItem;
}
function bs_contextSplitLine() { }
bs_contextSplitLine.prototype.createLine = function() {
    var contextSplitLine = document.createElement("div");
    contextSplitLine.className = "contextSplitLine";
    return contextSplitLine;
}