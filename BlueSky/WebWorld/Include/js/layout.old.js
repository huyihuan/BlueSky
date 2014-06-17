//Menu的显示与隐藏
var layout = {
    nMargin_TopBottom: 2, //上下布局之间的间距
    $: function(_id) {
        return document.getElementById(_id);
    },

    initLayout: function() {
        document.body.scroll = "no";
        this.menuFrame = this.$("MenuFrame");
        this.mainFrame = this.$("MainFrame");
        this.leftPane = this.$("divLeft");
        this.rightPane = this.$("divRight");
        this.top = this.$("divTop");
        this.menuTitle = this.$("divMenuTitle");
        this.bottom = this.$("divButtom");
        this.footer = this.$("divFooter");
        this.menuPane = this.$("divMenu");
        this.tabTarget = this.$("bs_tabs");
        this.tabFrameTarget = this.$("bs_tabFrames");
        this.width(this.tabTarget, 25);
        this.menuWidth = this.width(this.leftPane);
        this.initWidth();
        this.topHeight = this.height(this.top);
        this.menuTitleHeight = this.height(this.menuTitle); //菜单标题高度
        this.initHeight();

        //加载功能树
        setTimeout(function() { layout.menuFrame.src = "Menu.aspx"; }, 0);

        //加载任务栏
        if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true) {
            top.bs_event_ShowTaskBar(top.window, this.footer);
            this.height(this.footer, top.bs_field_TaskBarHeight);
        }

        setTimeout(function() {
            //获取主菜单window引用
            top.windowFactory.menuWindow = layout.menuFrame.contentWindow;
            //获取主工作区的window引用
            top.windowFactory.mainWindow = layout.mainFrame.contentWindow;
        }, 0);

        setTimeout(function() { Bluesky.component.Masklayer.remove(); }, 100);

        //初始化tab
        this.tabContainer = new BlueSky.TabContainer();
        this.tabContainer.init({ tWindow: window, parent: "bs_tabs" });

        var tab = new BlueSky.Tab();
        tab.init({ tabContent: "工作及日程区", frameTarget: "bs_tabFrames", frameUrl: "WorkPage.aspx", isClose: false, key: "home" });
        if (this.tabContainer && null != this.tabContainer)
            this.tabContainer.addTab(tab);
    },

    initWidth: function() {
        this.docWidth = document.body.clientWidth;
        var rightWidth = this.docWidth - this.menuWidth - 2 - 2 - 5;
        this.width([this.rightPane, this.mainFrame, this.tabTarget, this.tabFrameTarget], rightWidth);
    },

    initHeight: function() {
        this.docHeight = document.body.clientHeight;
        this.bottomHeight = this.docHeight - this.topHeight - this.nMargin_TopBottom;
        if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true)
            this.bottomHeight = this.bottomHeight - top.bs_field_TaskBarHeight - 1;
        this.height(this.menuFrame, this.bottomHeight - this.menuTitleHeight - 1 - 2);
        this.height(this.mainFrame, this.bottomHeight - 2);
        this.height(this.leftPane, this.bottomHeight - 2);
        this.height(this.rightPane, this.bottomHeight);
        this.height(this.bottom, this.bottomHeight);
        this.height(this.tabFrameTarget, this.bottomHeight - 27 - 1);
    },

    width: function(_elm, _width) {
        if (typeof _width === "undefined") {
            return Bluesky(_elm).width();
        }
        Bluesky(_elm).width(_width);
    },

    height: function(_elm, _height) {
        if (typeof _height === "undefined") {
            return Bluesky(_elm).height();
        }
        Bluesky(_elm).height(_height);
    },

    moduleSetting: function() {
        var windowArguments = { width: 600, height: 400, title: "模块配置", url: "ModuleConfig.aspx" };
        top.windowFactory.topFocusForm(windowArguments);
    },

    goWindow: function(_windowArguments) {
        var tab = new BlueSky.Tab();
        tab.init({ tabContent: _windowArguments.title, frameTarget: "bs_tabFrames", frameUrl: _windowArguments.url, key: _windowArguments.windowKey });
        this.tabContainer.addTab(tab);
    },

    refreshActiveWindow: function() {
        setTimeout(function() { layout.tabContainer.refreshTab(layout.tabContainer.activeTab); }, 0);
    },

    logout: function() {
        var strLoginUrl = "Window.aspx?fn=1&fm=Logout";
        top.windowFactory.targetFocusForm({
            title: "退出系统",
            url: strLoginUrl,
            width: 300,
            height: 150,
            target: window
        });
    }
}
Bluesky.ready(function() { layout.initLayout(); });

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
        layout.initHeight();
        layout.initWidth();
    }
})());