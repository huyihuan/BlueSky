//Menu的显示与隐藏
var layoutUtils = new Object();
layoutUtils.menuStatus = true;
layoutUtils.bottomMarginTop = 2; //上下布局之间的间距
layoutUtils.menuWidthHide = 20;
layoutUtils.menuWidthShow = 0;
layoutUtils.doc = top.window.document;
layoutUtils.docWidth = 0;
layoutUtils.docHeight = 0;
layoutUtils.menuFrame = null;
layoutUtils.mainFrame = null;
layoutUtils.leftPane = null;
layoutUtils.rightPane = null;
layoutUtils.rightPaneWidth = 0;
layoutUtils.bottom = null;
layoutUtils.bottomHeight = 0;
//layoutUtils.menuBarHide = null;
layoutUtils.top = null;
layoutUtils.topHeight = null;
layoutUtils.menuTitle = null;
layoutUtils.menuTitleHeight = null;
layoutUtils.footer = null;
layoutUtils.resizeCount = 0;
layoutUtils.menuPane = null;
layoutUtils.tabTarget = null;
layoutUtils.tabFrameTarget = null;
layoutUtils.tabContainer = new Object();
//layoutUtils.htTagToTabId = new Object();

function $(_id) {
    return layoutUtils.doc.getElementById(_id.toString());
}

window.onload = function() {
    document.body.scroll = "no";
    layoutUtils.doc = window.document;
    layoutUtils.docWidth = layoutUtils.doc.body.clientWidth;
    layoutUtils.docHeight = layoutUtils.doc.body.clientHeight;

    layoutUtils.menuFrame = $("MenuFrame");
    layoutUtils.mainFrame = $("MainFrame");
    layoutUtils.leftPane = $("divLeft");
    layoutUtils.rightPane = $("divRight");
    //layoutUtils.menuBarHide = $("divShowBar");
    layoutUtils.top = $("divTop");
    layoutUtils.menuTitle = $("divMenuTitle");
    layoutUtils.bottom = $("divButtom");
    layoutUtils.footer = $("divFooter");
    layoutUtils.menuPane = $("divMenu");
    layoutUtils.tabTarget = $("bs_tabs");
    layoutUtils.tabFrameTarget = $("bs_tabFrames");
    setWidth(layoutUtils.tabTarget, 25);

    //setWidth(layoutUtils.menuBarHide, layoutUtils.menuWidthHide);
    layoutUtils.menuWidthShow = getWidth(layoutUtils.leftPane);
    layoutUtils.setFrameWidth();

    layoutUtils.topHeight = getHeight(layoutUtils.top);
    layoutUtils.menuTitleHeight = getHeight(layoutUtils.menuTitle); //菜单标题高度
    layoutUtils.setFrameHeight();

    //加载功能树
    setTimeout(function() { layoutUtils.menuFrame.src = "Menu.aspx"; }, 0);

    //加载任务栏
    if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true) {
        top.bs_event_ShowTaskBar(top.window, layoutUtils.footer);
        setHeight(layoutUtils.footer, top.bs_field_TaskBarHeight);
    }

    setTimeout(function() {
        //获取主菜单window引用
        top.windowFactory.menuWindow = layoutUtils.menuFrame.contentWindow;
        //获取主工作区的window引用
        top.windowFactory.mainWindow = layoutUtils.mainFrame.contentWindow;
    }, 0);

    setTimeout(function() { bs_event_hideLoadingLayer(top.window.document); }, 100);

    //初始化tab
    layoutUtils.tabContainer = new BlueSky.TabContainer();
    layoutUtils.tabContainer.init({ tWindow: window, parent: "bs_tabs" });

    var tab = new BlueSky.Tab();
    tab.init({ tabContent: "工作及日程区", frameTarget: "bs_tabFrames", frameUrl: "WorkPage.aspx", isClose: false, key: "home" });
    if (layoutUtils.tabContainer && null != layoutUtils.tabContainer)
        layoutUtils.tabContainer.addTab(tab);
}

window.onresize = function() {//防止重复执行
    layoutUtils.resizeCount += 1;
    if (layoutUtils.resizeCount > 1) {
        layoutUtils.resizeCount = 0;
        return;
    }
    setTimeout(function() { layoutUtils.resizeCount = 0; }, 200);
    layoutUtils.setFrameHeight();
    layoutUtils.setFrameWidth();
}
//layoutUtils.toggleMenu = function() {
//    layoutUtils.menuStatus = !layoutUtils.menuStatus;
//    layoutUtils.setMenuDisplay();
//}
//layoutUtils.setMenuDisplay = function() {
//    if (!layoutUtils.menuStatus) {
//        hide(layoutUtils.menuTitle);
//        hide(layoutUtils.menuPane);
//        //show(layoutUtils.menuBarHide);
//    }
//    else {
//        show(layoutUtils.menuTitle);
//        show(layoutUtils.menuPane);
//        //hide(layoutUtils.menuBarHide);
//    }
//    layoutUtils.setFrameWidth();
//}
layoutUtils.setFrameWidth = function() {
    layoutUtils.docWidth = layoutUtils.doc.body.clientWidth;
    var leftWidth = layoutUtils.menuStatus ? layoutUtils.menuWidthShow : layoutUtils.menuWidthHide;
    setWidth(layoutUtils.leftPane, leftWidth);
    var rightWidth = layoutUtils.docWidth - leftWidth - 2 - 2 - 5;
    setWidth(layoutUtils.rightPane, rightWidth);
    setWidth(layoutUtils.menuFrame, leftWidth);
    setWidth(layoutUtils.mainFrame, rightWidth);
    setWidth(layoutUtils.tabTarget, rightWidth);
    setWidth(layoutUtils.tabFrameTarget, rightWidth);
}
layoutUtils.setFrameHeight = function() {
    layoutUtils.docHeight = layoutUtils.doc.body.clientHeight;
    layoutUtils.bottomHeight = layoutUtils.docHeight - layoutUtils.topHeight - layoutUtils.bottomMarginTop;
    if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true)
        layoutUtils.bottomHeight = layoutUtils.bottomHeight - top.bs_field_TaskBarHeight - 1;
    setHeight(layoutUtils.menuFrame, layoutUtils.bottomHeight - layoutUtils.menuTitleHeight - 1 - 2);
    setHeight(layoutUtils.mainFrame, layoutUtils.bottomHeight - 2);
    setHeight(layoutUtils.leftPane, layoutUtils.bottomHeight - 2);
    setHeight(layoutUtils.rightPane, layoutUtils.bottomHeight);
    setHeight(layoutUtils.bottom, layoutUtils.bottomHeight);
    setHeight(layoutUtils.tabFrameTarget, layoutUtils.bottomHeight - 27 - 1);
}

function setWidth(_o, _w) {
    _o.style.width = _w.toString().replace("px", "") + "px";
}

function getWidth(_o) {
    var w = getDocStyle(_o, "width");
    return parseInt(w.toString().replace("px", ""));
}

function setHeight(_o, _h) {
    _o.style.height = _h.toString().replace("px", "") + "px";
}

function getHeight(_o) {
    var h = getDocStyle(_o, "height");
    return parseInt(h.toString().replace("px", ""));
}

function hide(_o) {
    _o.style.display = "none";
}
function show(_o) {
    _o.style.display = "block";
}

function getDocStyle(o, styleName) {
    if (o.currentStyle) {
        return o.currentStyle[styleName];
    }
    else{
        var oStyles = o.ownerDocument.defaultView.getComputedStyle(o, null);
        return oStyles[styleName];
    }
}

function goWindow(_windowArguments) {
    //显示loading
    //bs_event_showLoadingLayer(top.window.document);
    //创建窗体
    //top.bs_event_createWorkWindow(_windowArguments);
    //加载控件
    //layoutUtils.mainFrame.src = _windowArguments.url;
//    if (null != layoutUtils.htTagToTabId[_windowArguments.windowKey] && undefined != layoutUtils.htTagToTabId[_windowArguments.windowKey]) {
//        layoutUtils.tabContainer.setTabActive(layoutUtils.htTagToTabId[_windowArguments.windowKey]);
//        return;
    //    }
    var tab = new BlueSky.Tab();
    tab.init({ tabContent: _windowArguments.title, frameTarget: "bs_tabFrames", frameUrl: _windowArguments.url, key: _windowArguments.windowKey });
    layoutUtils.tabContainer.addTab(tab);
    //layoutUtils.htTagToTabId[_windowArguments.windowKey] = tab.id;
    //隐藏loading
    //setTimeout(function() { bs_event_hideLoadingLayer(top.window.document); }, 200);
}

function setModule() {
    var windowArguments = new Object();
    windowArguments.width = 600;
    windowArguments.height = 400;
    windowArguments.title = "模块配置";
    windowArguments.url = "ModuleConfig.aspx";
    top.windowFactory.topFocusForm(windowArguments);
}

function refreshActiveWindow() {
    setTimeout(function() { layoutUtils.tabContainer.refreshTab(layoutUtils.tabContainer.activeTab); }, 0);
}