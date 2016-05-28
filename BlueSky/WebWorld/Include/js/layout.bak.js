//Menu的显示与隐藏
var layout = {
    nMargin_TopBottom: 2, //上下布局之间的间距
    $: function(_id) {
        return document.getElementById(_id);
    },

    initLayout: function() {
        document.body.scroll = "no";
        this.menuFrame = this.$("MenuFrame");
        this.leftPane = this.$("divLeft");
        this.rightPane = this.$("divRight");
        this.top = this.$("divTop");
        this.bottom = this.$("divButtom");
        this.footer = this.$("divFooter");
        this.tabs = Bluesky.component.Tabs.create({
            renderTo: "#divRight",
            width: 2,
            height: 35,
            activeIndex: 0,
            items: [
				        {
				            title: "工作及日程区",
				            key:"tabCalendar" + Math.random(),
				            sliding: true,
				            closeable: false,
				            showIcon: true,
				            iconURL: "include/image/icons/information.png",
				            loader: {
				                url: "WorkPage.aspx",
				                autoLoad: true
				            }
				        }
                    ]
        });
        this.leftWidth = this.width(this.leftPane);
        this.topHeight = this.height(this.top);
        this.initHeight();

        //加载任务栏
        if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true) {
            top.bs_event_ShowTaskBar(top.window, this.footer);
            this.height(this.footer, top.bs_field_TaskBarHeight);
        }
        setTimeout(function() {
            //获取主菜单window引用
            top.windowFactory.menuWindow = layout.menuFrame.contentWindow;
            //获取主工作区的window引用
            //top.windowFactory.mainWindow = layout.mainFrame.contentWindow;
        }, 0);
        setTimeout(function() { Bluesky.component.Masklayer.remove(); }, 100);
    },

    initHeight: function() {
        this.docWidth = document.body.clientWidth;
        var rightWidth = this.docWidth - this.leftWidth - 5;
        this.docHeight = document.body.clientHeight;
        this.bottomHeight = this.docHeight - this.topHeight - this.nMargin_TopBottom;
        if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true)
            this.bottomHeight = this.bottomHeight - top.bs_field_TaskBarHeight - 1;
        this.height(this.bottom, this.bottomHeight);
        this.width(this.rightPane, rightWidth);

        this.tabs.resize({ width: rightWidth, height: this.bottomHeight });
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
        this.tabs.add({
            title: _windowArguments.title,
            tip: _windowArguments.title,
            key: _windowArguments.windowKey,
            isRunMutil:false,
            showIcon: true,
            loader: {
                url: _windowArguments.url,
                autoLoad: true
            }
        });
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
    }
})());