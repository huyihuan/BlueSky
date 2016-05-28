//Menu的显示与隐藏
var layout = {
    nMargin_TopBottom: 2, //上下布局之间的间距
    $: function(_id) {
        return document.getElementById(_id);
    },

    initLayout: function() {
        document.body.scroll = "no";
        //this.menuFrame = this.$("MenuFrame");
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
				            key: "tabCalendar" + Math.random(),
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
        this.menuTabs = Bluesky.component.Tabs.create({
            renderTo: this.leftPane,
            width: this.leftWidth - 1,
            height: this.height(this.leftPane),
            activeIndex: 0,
            items: [
	                {
	                    title: "",
	                    tip: "系统功能",
	                    sliding: true,
	                    closeable: false,
	                    showIcon: true,
	                    iconURL: "include/image/icons/house.png",
	                    html: ""
	                },
	                {
	                    title: "",
	                    tip: "日常办公",
	                    sliding: true,
	                    closeable: false,
	                    showIcon: true,
	                    iconURL: "include/image/icons/clock.png",
	                    html: "日常办公"
	                },
	                {
	                    title: "",
	                    tip: "邮箱",
	                    sliding: true,
	                    closeable: false,
	                    showIcon: true,
	                    iconURL: "include/image/icons/email.png",
	                    html: "邮箱"
	                }
            ]
        });
        this.menuTree = new Bluesky.component.Tree({
            id: "SystemMunu",
            renderTo: this.menuTabs.tab(0).contentNode,
            width: "100%",
            height: "100%",
            showCheckBox: false,
            showRootNode: false,
            loader: {
                url: "/Server/ServerRouting.ashx",
                params: { action: "GetFunctions" }
            },
            onNodeSelected: function(_node) {
                if (_node.childrenCount() == 0 && _node.data) {
                    var args = {
                        title: _node.data.name,
                        windowKey: _node.value,
                        url: "Window.aspx?fn=" + _node.value + "&r=" + Math.random()
                    };
                    top.layout.goWindow(args);
                }
            }
        });
        this.menuTree.init();
        this.initHeight();

        //加载任务栏
        //        if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true) {
        //            top.bs_event_ShowTaskBar(top.window, this.footer);
        //            this.height(this.footer, top.bs_field_TaskBarHeight);
        //        }
        //setTimeout(function() {
        //获取主菜单window引用
        //top.windowFactory.menuWindow = layout.menuFrame.contentWindow;
        //获取主工作区的window引用
        //top.windowFactory.mainWindow = layout.mainFrame.contentWindow;
        //}, 0);
        setTimeout(function() { Bluesky.component.Masklayer.remove(); }, 100);
    },

    initHeight: function() {
        this.docWidth = document.body.clientWidth;
        var rightWidth = this.docWidth - this.leftWidth - 5;
        this.docHeight = document.body.clientHeight;
        this.bottomHeight = this.docHeight - this.topHeight - this.nMargin_TopBottom;
        //        if (bs_field_IsShowTaskBar && bs_field_IsShowTaskBar == true)
        //            this.bottomHeight = this.bottomHeight - top.bs_field_TaskBarHeight - 1;
        this.height(this.bottom, this.bottomHeight);
        this.width(this.rightPane, rightWidth);
        this.tabs.resize({ width: rightWidth, height: this.bottomHeight });
        this.menuTabs.resize({ width: this.leftWidth, height: this.bottomHeight });
        //this.menuTree.node.wrapper.width(this.leftWidth);
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
        Bluesky.component.create("Window", {
            title: "模块配置",
            width: 600,
            height: 400,
            renderTo: document.body,
            mask: true,
            loader: {
                url: "ModuleConfig.aspx"
            },
            icon: {
                show: true,
                url: "/Include/image/ActionImages/role.png"
            }
        });
    },

    userView: function(url) {
        Bluesky.component.create("Window", {
            title: "用户信息",
            width: 500,
            height: 450,
            renderTo: document.body,
            mask: true,
            loader: {
                url: url
            },
            icon: {
                show: true,
                url: "/Include/image/ActionImages/role.png"
            }
        });
    },
    closeActiveWindow: function() {
        Bluesky.component.Window.activeWindow.close();
    },
    goWindow: function(_windowArguments) {
        this.tabs.add({
            title: _windowArguments.title,
            tip: _windowArguments.title,
            key: _windowArguments.windowKey,
            isRunMutil: false,
            showIcon: true,
            loader: {
                url: _windowArguments.url,
                autoLoad: true
            }
        });
    },

    refreshActiveWindow: function() {
        setTimeout(function() { layout.tabs.activeTab.refresh(); }, 0);
    },

    logout: function() {
        Bluesky.MessageBox.confirm({
            title: "退出系统",
            message: "确认退出系统？",
            callback: function() {
                Bluesky.Ajax({
                    type: "get",
                    url: "Server/SystemManage/Login.ashx",
                    data: { action: "Logout" },
                    success: function(response) {
                        if (response.text == "true") {
                            window.location.href = "Default.html";
                        }
                    }
                });
            }
        });
    },

    myMusic: function() {
        if (!layout.musicPlayer) {
            var target = Bluesky("#link_MyMusic");
            var pos = target.position();
            pos.y = pos.y + target.height() + 10;
            layout.musicPlayer = new Bluesky.component.MusicPlayer({ renderTo: document.body });
            pos.x = pos.x + target.width() - layout.musicPlayer.width;
            layout.musicPlayer.position = pos;
            layout.musicPlayer.init();

            layout.musicPlayer.add({ title: "", url: "SystemUpload/MyMusic/1/Song Of The Lonely Mountain.mp3", imageURL: "" });
        }
        else {
            if (layout.musicPlayer.isHidden()) {
                layout.musicPlayer.show();
            }
            else {
                layout.musicPlayer.hide();
            }
        }
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