/**********************************************
* BlueSky框架组件库 Tab组件
* @copyright huyihuan 2013
* date: 2013-11-12
**********************************************/
if(undefined == BlueSky || null == BlueSky) 
    var BlueSky = new Object();
BlueSky.tabContainers = new Object();
BlueSky.tabContainersArry = new Array();
BlueSky.tabType = { tab: "tab-normal", space: "tab-space-normal", last: "tab-space-last" }
BlueSky.tabStatus = { normal: 1, active: 2 }
BlueSky.TabContainer = function() {
    this.id = "";
    this.parent = null;
    this.tWindow = window;
    this.tDocument = window.document;
    this.activeTab = null;
    this.tabCount = 0;
    this.htTabs = new Object();
    this.htTabKey = new Object();
    this.tabArry = new Array();
}

BlueSky.TabContainer.prototype.init = function(_tabArguments) {
    if (_tabArguments.parent && undefined != _tabArguments.parent)
        this.parent = _tabArguments.parent;
    if (_tabArguments.tWindow && undefined != _tabArguments.tWindow) {
        this.tWindow = _tabArguments.tWindow;
        this.tDocument = _tabArguments.tWindow.document;
    }
    this.id = "tab-container" + (BlueSky.tabContainersArry.length + 1);

    var tbContainer = this.tDocument.createElement("table");
    tbContainer.id = this.id;
    tbContainer.className = "tab-container";
    tbContainer.setAttribute("cellpadding", "0");
    tbContainer.setAttribute("cellspacing", "0");

    var trTabs = this.tDocument.createElement("tr");

    var tdSpaceHead = this.tDocument.createElement("td");
    tdSpaceHead.className = "tab " + BlueSky.tabType.space;
    tdSpaceHead.innerHTML = "&nbsp;";
    trTabs.appendChild(tdSpaceHead);

    var tdSpaceLast = this.tDocument.createElement("td");
    tdSpaceLast.className = "tab " + BlueSky.tabType.last;
    tdSpaceLast.innerHTML = "&nbsp;";
    trTabs.appendChild(tdSpaceLast);

    tbContainer.appendChild(trTabs);
    this.tDocument.getElementById(this.parent).appendChild(tbContainer);
    BlueSky.tabContainers[this.id] = this;
    BlueSky.tabContainersArry[BlueSky.tabContainersArry.length] = this.id;
}

BlueSky.TabContainer.prototype.addTab = function(_tabItem) {
    if (!_tabItem || undefined == _tabItem)
        return;
    if (undefined != this.htTabKey[_tabItem.key] && null != this.htTabKey[_tabItem.key]) {
        if (null != this.activeTab && this.activeTab.key != _tabItem.key) {
            this.setTabActive(this.htTabKey[_tabItem.key]);
        }
        return;
    }

    var tbContainer = this.tDocument.getElementById(this.id);
    var trTabs = tbContainer.getElementsByTagName("tr")[0];
    this.tabCount += 1;

    var tdTab = this.tDocument.createElement("td");
    _tabItem.id = this.id + "-tab" + this.tabCount;
    _tabItem.tabContainerId = this.id;
    this.tabArry[this.tabArry.length] = _tabItem.id;
    tdTab.id = _tabItem.id;
    tdTab.className = "tab " + BlueSky.tabType.tab;
    tdTab.setAttribute("nowrap", "nowrap");
    tdTab.innerHTML = _tabItem.tabContent;
    tdTab.onclick = function() {
        if (_tabItem.status == BlueSky.tabStatus.normal) {
            BlueSky.tabContainers[tbContainer.id].setTabActive(_tabItem);
        }
    }

    //添加关闭按钮
    if (_tabItem.isClose) {
        var aClose = this.tDocument.createElement("a");
        aClose.innerHTML = "&nbsp;x";
        aClose.title = "关闭";
        aClose.className = "tabCloseBtn";
        aClose.onclick = function(e) {
            BlueSky.tabContainers[tbContainer.id].removeTab(_tabItem);
            //如果提供了事件对象，则这是一个非IE浏览器,因此它支持W3C的stopPropagation()方法。否则，我们需要使用IE的方式来取消事件冒泡
            e = e ? e : window.event;
            if (e && e.stopPropagation)
                e.stopPropagation();
            else
                this.tWindow.event.cancelBubble = true;
            if (e && e.preventDefault)
                e.preventDefault();
        }
        tdTab.appendChild(aClose);
    }
    trTabs.insertBefore(tdTab, trTabs.lastChild);
    this.htTabs[tdTab.id] = _tabItem;
    this.htTabKey[_tabItem.key] = _tabItem;

    var tdSpace = this.tDocument.createElement("td");
    tdSpace.className = "tab " + BlueSky.tabType.space;
    tdSpace.innerHTML = "&nbsp;";
    trTabs.insertBefore(tdSpace, trTabs.lastChild);

    //add the tab frame
    var tFrame = this.tDocument.createElement("iframe");
    _tabItem.frameId = this.id + "-tabFrame" + this.tabCount;
    tFrame.id = _tabItem.frameId;
    tFrame.style.display = "none";
    tFrame.style.width = "100%";
    tFrame.style.height = "100%";
    tFrame.frameBorder = "0";
    tFrame.setAttribute("scrolling", "no");
    _tabItem.frame = tFrame;
    var tFrameTarget = this.tDocument.getElementById(_tabItem.frameTarget);
    if (tFrameTarget && undefined != tFrameTarget) {
        tFrameTarget.appendChild(tFrame);
        setTimeout(function() { tFrame.src = _tabItem.frameUrl; }, 0);
    }

    if (_tabItem.initActive)
        this.setTabActive(_tabItem);
}

BlueSky.TabContainer.prototype.removeTab = function(_tabItem) {
    if (undefined == _tabItem || null == _tabItem)
        return;
    if (typeof (_tabItem) == "string")
        _tabItem = this.htTabs[_tabItem];
    var tabNormal = this.tDocument.getElementById(_tabItem.id);
    if (tabNormal && undefined != tabNormal) {
        tabNormal.parentNode.removeChild(tabNormal.nextSibling);
        tabNormal.parentNode.removeChild(tabNormal);
        delete this.htTabs[tabNormal.id];
        delete this.htTabKey[_tabItem.key];
        var tempContainer = BlueSky.tabContainers[this.id];
        for (var n = 0; n < tempContainer.tabArry.length; n++) {
            if (tempContainer.tabArry[n].toString() == _tabItem.id) {
                tempContainer.tabArry.splice(n, 1);
                break;
            }
        }
        //tempContainer.tabCount = tempContainer.tabCount - 1;
        if (_tabItem.status == BlueSky.tabStatus.active && BlueSky.tabContainers[this.id].tabArry.length >= 1) {
            var nextTab = this.htTabs[tempContainer.tabArry[tempContainer.tabArry.length - 1]];
            tempContainer.setTabActive(nextTab);
        }

        //var tFrame = this.tDocument.getElementById(_tabItem.frameId);
        var tFrame = _tabItem.frame;
        if (tFrame && undefined != tFrame) {
            tFrame.parentNode.removeChild(tFrame);
        }
    }
}

BlueSky.TabContainer.prototype.clear = function() {
    
}

BlueSky.TabContainer.prototype.setTabActive = function(_tabItem) {
    if (undefined == _tabItem || null == _tabItem)
        return;
    if (typeof (_tabItem) == "string")
        _tabItem = this.htTabs[_tabItem];
    if (this.activeTab == _tabItem)
        return;
    this.setTabNormal(this.activeTab);
    var tabNormal = this.tDocument.getElementById(_tabItem.id);
    if (tabNormal && undefined != tabNormal) {
        tabNormal.className = tabNormal.className.replace("normal", "active");
        _tabItem.status = BlueSky.tabStatus.active;
        this.activeTab = _tabItem;

        //var tFrame = this.tDocument.getElementById(_tabItem.frameId);
        var tFrame = _tabItem.frame;
        if (tFrame && undefined != tFrame) {
            tFrame.style.display = "block";
        }
    }
}

BlueSky.TabContainer.prototype.setTabNormal = function(_tabItem) {
    if (undefined == _tabItem || null == _tabItem)
        return;
    if (typeof (_tabItem) == "string")
        _tabItem = this.htTabs[_tabItem];
    var tabNormal = this.tDocument.getElementById(_tabItem.id);
    if (tabNormal && undefined != tabNormal) {
        tabNormal.className = tabNormal.className.replace("active", "normal");
        _tabItem.status = BlueSky.tabStatus.normal;

        //var tFrame = this.tDocument.getElementById(_tabItem.frameId);
        var tFrame = _tabItem.frame;
        if (tFrame && undefined != tFrame) {
            tFrame.style.display = "none";
        }
    }
}
BlueSky.TabContainer.prototype.refreshTab = function(_tabItem) {
    if (!_tabItem || !_tabItem.frame)
        return;
    _tabItem.frame.contentWindow.location.href = _tabItem.frame.contentWindow.location.href;
}

BlueSky.Tab = function() {
    this.id = "";
    this.tabContainerId = "";
    this.tabContent = "";
    this.frameId = "";
    this.frameId = null;
    this.frameTarget = "";
    this.frameUrl = "";
    this.type = "";
    this.isClose = true;
    this.initActive = true;
    this.key = "";
    this.status = BlueSky.tabStatus.normal;
}
BlueSky.Tab.prototype.init = function(_tabArguments) {
    if (_tabArguments.tabContent && undefined != _tabArguments.tabContent)
        this.tabContent = _tabArguments.tabContent;
    if (_tabArguments.frameUrl && undefined != _tabArguments.frameUrl)
        this.frameUrl = _tabArguments.frameUrl;
    if (_tabArguments.frameTarget && undefined != _tabArguments.frameTarget)
        this.frameTarget = _tabArguments.frameTarget;
    if (_tabArguments.key && undefined != _tabArguments.key)
        this.key = _tabArguments.key;
    if (undefined != _tabArguments.isClose)
        this.isClose = _tabArguments.isClose;
    if (undefined != _tabArguments.initActive)
        this.initActive = _tabArguments.initActive;
}