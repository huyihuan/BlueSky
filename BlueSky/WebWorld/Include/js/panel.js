/**********************************************
* BlueSky框架组件库 Panel组件
* @copyright huyihuan 2013
* date: 2013-11-18
**********************************************/
if (undefined == BlueSky || null == BlueSky)
    var BlueSky = new Object();
BlueSky.panelCount = 0;
BlueSky.Panel = function(_arg) {
    this.id = "";
    this.width = "100";
    this.height = "100";
    this.parent = null;
    this.splitPanel = new Array();
    if (undefined != _arg && null != _arg) {
        this._initArgument(_arg);
    }
}
BlueSky.Panel.prototype._initArgument = function(_arg) {
    if (undefined == _arg && null == _arg)
        return;
    this.width = _arg.width ? _arg.width : "200";
    this.height = _arg.height ? _arg.height : "300";
    if (_arg.parent && null != _arg.parent) {
        if (typeof (_arg.parent) == "string")
            this.parent = document.getElementById(_arg.parent);
        else if (typeof (_arg.parent) == "object")
            this.parent = _arg.parent;
    }
    else if (undefined == _arg.parent || null == _arg.parent) {
        this.parent = document;
    }
    if (_arg.splitPanel && _arg.splitPanel.length > 0) {
        this.splitPanel = _arg.splitPanel;
    }
}
BlueSky.Panel.prototype.setSplitPanel = function(_splitPanel) {
    if (undefined == _splitPanel || null == _splitPanel || undefined == _splitPanel.key)
        return;
    var tbPanelContainer = document.getElementById(this.id);
    if (undefined == tbPanelContainer)
        return;
    var trTitle = tbPanelContainer.firstChild;
    if (trTitle && _splitPanel.title) {
        trTitle.childNodes[_splitPanel.key * 2].innerHTML = "&nbsp;" + _splitPanel.title;
    }
    var trContent = tbPanelContainer.lastChild;
    var divContent = trContent.childNodes[_splitPanel.key].firstChild;
    if (_splitPanel.content) {
        divContent.innerHTML = "";
        if (typeof (_splitPanel.content) == "string") {
            if ("" == _splitPanel.content || "#" != _splitPanel.content.substr(0, 1)) {
                divContent.innerHTML = _splitPanel.content;
            }
            else {
                var content = document.getElementById(_splitPanel.content.substring(1));
                if (content && null != content)
                    divContent.appendChild(content);
                else
                    divContent.innerHTML = _splitPanel.content;
            }
        }
        else if (typeof (_splitPanel.content) == "object") {
            divContent.appendChild(_splitPanel.content);
        }
    }
    else if (_splitPanel.url) {
        var tFrame = divContent.firstChild;
        if (undefined == tFrame || null == tFrame || undefined == tFrame.tagName || null == tFrame.tagName || tFrame.tagName.toLowerCase() != "iframe") {
            divContent.innerHTML = "";
            tFrame = document.createElement("iframe");
            tFrame.style.width = "100%";
            tFrame.style.height = "100%";
            tFrame.setAttribute("frameborder", "0");
            tFrame.setAttribute("scrolling", "no");
            divContent.appendChild(tFrame);
        }
        setTimeout(function() { tFrame.src = _splitPanel.url }, 1);
    }
}
BlueSky.Panel.prototype.init = function(_arg) {
    this._initArgument(_arg);
    BlueSky.panelCount++;
    this.id = "panel" + BlueSky.panelCount;
    var panelTable = document.createElement("table");
    var panelReference = this;
    panelTable.id = this.id;
    panelTable.className = "panel-container";
    panelTable.setAttribute("cellspacing", "0");
    panelTable.setAttribute("cellpadding", "0");
    panelTable.setAttribute("width", this.width);
    panelTable.setAttribute("height", this.height);
    if (this.splitPanel && this.splitPanel.length > 0) {
        var trTitle = document.createElement("tr");
        panelTable.appendChild(trTitle);
        var trContent = document.createElement("tr");
        panelTable.appendChild(trContent);
        var nCount = this.splitPanel.length;
        for (var i = 0; i < nCount; i++) {
            if (i > 0) {
                var tdSplitLine = document.createElement("td");
                tdSplitLine.className = "panel-splitline-h";
                tdSplitLine.innerHTML = "&nbsp;";
                tdSplitLine.setAttribute("_index", i);
                tdSplitLine.setAttribute("rowspan", nCount);
                trTitle.appendChild(tdSplitLine);
                if (!(true == this.splitPanel[i - 1].fixable || true == this.splitPanel[i].fixable))
                    tdSplitLine.onmousedown = panelReference.splitMove;
            }
            this.splitPanel[i].key = i;
            var panelArg = this.splitPanel[i];

            var tdPanelTitle = document.createElement("td");
            tdPanelTitle.className = "panel-title";
            tdPanelTitle.setAttribute("nowrap", "nowrap");
            tdPanelTitle.innerHTML = "&nbsp;" + panelArg.title;
            if (panelArg.width)
                tdPanelTitle.setAttribute("width", panelArg.width);
            trTitle.appendChild(tdPanelTitle);

            var tdPanelContent = document.createElement("td");
            var divContent = document.createElement("div");
            divContent.className = "panel-content";
            //设置divContent的宽度和高度，目前设置为100%但是无法出现滚动条
//            if (panelArg.width && panelArg.width.indexOf("%") == -1) {
//                divContent.style.width = parseInt(panelArg.width) + "px";
//            }
//            else if (panelArg.width && panelArg.width.indexOf("%") > -1) {
//                divContent.style.width = parseInt(panelArg.width.replace("%", "")) * this.width + "px";
//            }
            if (this.height && this.height.indexOf("%") == -1) {
                divContent.style.height = (parseInt(this.height) - 30) + "px";
            }
            else if (this.height && this.height.indexOf("%") != -1) {
                var parentHeight = this.parent == document ? this.parent.body.clientHeight : this.parent.clientHeight;
                divContent.style.height = (parentHeight - 30) + "px";
            }
            tdPanelContent.appendChild(divContent);
            if (panelArg.url && null != panelArg.url) {
                var tFrame = document.createElement("iframe");
                tFrame.style.width = "100%";
                tFrame.style.height = "100%";
                tFrame.frameBorder = "0";
                tFrame.setAttribute("scrolling", "yes");
                divContent.appendChild(tFrame);
                setTimeout(function() { tFrame.setAttribute("src", panelArg.url); }, 100);
            }
            else if (panelArg.content && null != panelArg.content) {
                if (typeof (panelArg.content) == "string") {
                    if ("" == panelArg.content || "#" != panelArg.content.substr(0, 1)) {
                        divContent.innerHTML = panelArg.content;
                    }
                    else {
                        var content = document.getElementById(panelArg.content.substring(1));
                        if (content && null != content)
                            divContent.appendChild(content);
                        else
                            divContent.innerHTML = panelArg.content;
                    }
                }
                else if (typeof (panelArg.content) == "object") {
                    divContent.appendChild(panelArg.content);
                }
            }
            trContent.appendChild(tdPanelContent);
        }
    }
    if (this.parent == document) {
        if (document.forms)
            document.forms[0].appendChild(panelTable);
        else
            document.body.appendChild(panelTable);
    }
    else if (typeof (this.parent) == "object") {
        this.parent.appendChild(panelTable);
    }
    else if (typeof (this.parent) == "string") {
        document.getElementById(this.parent).appendChild(panelTable);
    }
}

BlueSky.Panel.prototype.splitMove = function(e) {
    var _index = this.getAttribute("_index");
    var tbPanelContainer = this.parentNode.parentNode;
    if (!tbPanelContainer || undefined == tbPanelContainer)
        return;
    var trTitle = tbPanelContainer.firstChild;
    if (undefined == trTitle || null == trTitle)
        return;
    var tdLeftSplit = trTitle.childNodes[(_index - 1) * 2];
    var tdRightSplit = trTitle.childNodes[_index * 2];
    if (undefined == tdLeftSplit || null == tdLeftSplit || undefined == tdRightSplit || null == tdRightSplit)
        return;
    var leftWidth = parseInt(tdLeftSplit.getAttribute("width"));
    var rightWidth = parseInt(tdRightSplit.getAttribute("width"));

    var movePanel = document.createElement("div");
    movePanel.className = "window-move-panel";
    document.body.appendChild(movePanel);


    document.ondragstart = "return false;";
    document.onselectstart = "return false;";
    document.onselect = "document.selection.empty();";
    //记录异动前的坐标位置
    var e = e ? e : window.event;
    var tempX = e.clientX;
    document.onmousemove = function(e) {
        var e = e ? e : window.event;
        var moveX = e.clientX - tempX;
        if (!isNaN(leftWidth)) {
            tdLeftSplit.setAttribute("width", leftWidth + moveX);
        }
        if (!isNaN(rightWidth)) {
            tdRightSplit.setAttribute("width", rightWidth - moveX);
        }
    }

    document.onmouseup = function(e) {
        if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);

        document.body.removeChild(movePanel);
        document.onmousemove = null;
        document.onmouseup = null;
        document.ondragstart = null;
        document.onselectstart = null;
        document.onselect = null;
    }

    //如果提供了事件对象，则这是一个非IE浏览器
    if (e && e.stopPropagation)// 因此它支持W3C的stopPropagation()方法　　
        e.stopPropagation();
    else//否则，我们需要使用IE的方式来取消事件冒泡　　
        window.event.cancelBubble = true;
    if (e && e.preventDefault)
        e.preventDefault();
}