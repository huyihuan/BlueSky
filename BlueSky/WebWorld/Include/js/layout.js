//Menu的显示与隐藏
var layout = new Object();
layout.menuStatus = true;
layout.bottomMarginTop = 2; //上下布局之间的间距
layout.menuWidthHide = 20;
layout.menuWidthShow = 0;
layout.doc = top.window.document;
layout.docWidth = 0;
layout.docHeight = 0;
layout.menuFrame = null;
layout.mainFrame = null;
layout.leftPane = null;
layout.rightPane = null;
layout.rightPaneWidth = 0;
layout.bottom = null;
layout.bottomHeight = 0;
layout.top = null;
layout.topHeight = null;
layout.menuTitle = null;
layout.menuTitleHeight = null;
layout.footer = null;
layout.menuPane = null;
layout.tabTarget = null;
layout.tabFrameTarget = null;
layout.tabContainer = new Object();

function $(_id) {
    return layout.doc.getElementById(_id.toString());
}

Bluesky.ready(function() {
    document.body.scroll = "no";
    layout.doc = window.document;
    layout.docWidth = layout.doc.body.clientWidth;
    layout.docHeight = layout.doc.body.clientHeight;

    layout.menuFrame = $("MenuFrame");
    layout.mainFrame = $("MainFrame");
    layout.leftPane = $("divLeft");
    layout.rightPane = $("divRight");
    layout.top = $("divTop");
    layout.menuTitle = $("divMenuTitle");
    layout.bottom = $("divButtom");
    layout.footer = $("divFooter");
    layout.menuPane = $("divMenu");
    layout.tabTarget = $("bs_tabs");
    layout.tabFrameTarget = $("bs_tabFrames");
    setWidth(layout.tabTarget, 25);
    layout.menuWidthShow = getWidth(layout.leftPane);
    layout.setFrameWidth();

    layout.topHeight = getHeight(layout.top);
    layout.menuTitleHeight = getHeight(layout.menuTitle); //菜单标题高度
    layout.setFrameHeight();

    //加载功能树
    setTimeout(function() { layout.menuFrame.src = "Menu.aspx"; }, 0);

    //加载任务栏
    if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true) {
        top.bs_event_ShowTaskBar(top.window, layout.footer);
        setHeight(layout.footer, top.bs_field_TaskBarHeight);
    }

    setTimeout(function() {
        //获取主菜单window引用
        top.windowFactory.menuWindow = layout.menuFrame.contentWindow;
        //获取主工作区的window引用
        top.windowFactory.mainWindow = layout.mainFrame.contentWindow;
    }, 0);

    setTimeout(function() { bs_event_hideLoadingLayer(top.window.document); }, 100);

    //初始化tab
    layout.tabContainer = new BlueSky.TabContainer();
    layout.tabContainer.init({ tWindow: window, parent: "bs_tabs" });

    var tab = new BlueSky.Tab();
    tab.init({ tabContent: "工作及日程区", frameTarget: "bs_tabFrames", frameUrl: "WorkPage.aspx", isClose: false, key: "home" });
    if (layout.tabContainer && null != layout.tabContainer)
        layout.tabContainer.addTab(tab);
});

Bluesky(window).addEvent("resize", (function() {
    //防止重复执行
    var rCount = 0;
    return function() {
        rCount++;
        if (rCount >= 2) {
            rCount = 0;
            return;
        }
        setTimeout(function() { rCount = 0; }, 200);
        layout.setFrameHeight();
        layout.setFrameWidth();
    }
})());

layout.setFrameWidth = function() {
    this.docWidth = this.doc.body.clientWidth;
    var leftWidth = this.menuStatus ? this.menuWidthShow : this.menuWidthHide;
    //setWidth([this.leftPane, this.menuFrame], leftWidth);
    var rightWidth = this.docWidth - leftWidth - 2 - 2 - 5;
    setWidth([this.rightPane, this.mainFrame, this.tabTarget, this.tabFrameTarget], rightWidth);
}

layout.setFrameHeight = function() {
    layout.docHeight = layout.doc.body.clientHeight;
    layout.bottomHeight = layout.docHeight - layout.topHeight - layout.bottomMarginTop;
    if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true)
        layout.bottomHeight = layout.bottomHeight - top.bs_field_TaskBarHeight - 1;
    setHeight(layout.menuFrame, layout.bottomHeight - layout.menuTitleHeight - 1 - 2);
    setHeight(layout.mainFrame, layout.bottomHeight - 2);
    setHeight(layout.leftPane, layout.bottomHeight - 2);
    setHeight(layout.rightPane, layout.bottomHeight);
    setHeight(layout.bottom, layout.bottomHeight);
    setHeight(layout.tabFrameTarget, layout.bottomHeight - 27 - 1);
}

function setWidth(_elm, _width) {
    Bluesky(_elm).width(_width);
}

function getWidth(_elm) {
    return Bluesky(_elm).width();
}

function setHeight(_elm, _height) {
    Bluesky(_elm).height(_height);
}

function getHeight(_elm) {
    return Bluesky(_elm).height();
}

function goWindow(_windowArguments) {
    //显示loading
    //bs_event_showLoadingLayer(top.window.document);
    //创建窗体
    //top.bs_event_createWorkWindow(_windowArguments);
    //加载控件
    //layout.mainFrame.src = _windowArguments.url;
//    if (null != layout.htTagToTabId[_windowArguments.windowKey] && undefined != layout.htTagToTabId[_windowArguments.windowKey]) {
//        layout.tabContainer.setTabActive(layout.htTagToTabId[_windowArguments.windowKey]);
//        return;
    //    }
    var tab = new BlueSky.Tab();
    tab.init({ tabContent: _windowArguments.title, frameTarget: "bs_tabFrames", frameUrl: _windowArguments.url, key: _windowArguments.windowKey });
    layout.tabContainer.addTab(tab);
    //layout.htTagToTabId[_windowArguments.windowKey] = tab.id;
    //隐藏loading
    //setTimeout(function() { bs_event_hideLoadingLayer(top.window.document); }, 200);
}

function setModule() {
    var windowArguments = { width: 600, height: 400, title: "模块配置", url: "ModuleConfig.aspx" };
    top.windowFactory.topFocusForm(windowArguments);
}

function refreshActiveWindow() {
    setTimeout(function() { layout.tabContainer.refreshTab(layout.tabContainer.activeTab); }, 0);
}