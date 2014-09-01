if (Bluesky && Bluesky.component) {
    Bluesky.extend(false, Bluesky.component, { Window: function() {
            var args = arguments[0];
            return Bluesky.extend(true, {}, this, args);
        }
    });
    Bluesky.extend(true, Bluesky.component.Window, {
        items: [],
        status: {
            Normal: 1,
            Min: 2,
            Max: 3
        },
        display: {
            Show: 1,
            Hide: 2
        },
        direction: {
            North: "n-resize",
            Sourth: "s-resize",
            West: "w-resize",
            East: "e-resize",
            NorthWest: "nw-resize",
            NorthEast: "ne-resize",
            SourthEast: "se-resize",
            SourthWest: "sw-resize"
        },
        dialogResult: {
            OK : 1,
            Cancel : 0
        },
        configs : {
            defaultIconURL: "Include/image/application.png",
            titleHeight: 25,
            minWidth : 150,
            minHeight : 150
        }
        
    });

    Bluesky.extend(false, Bluesky.component.Window.prototype, {
        id: "",
        key: "",
        width: 300,
        height: 200,
        title: "未命名",
        static: Bluesky.component.Window,
        status: this.static.status.Normal,
        //sizestatus : bs_field_windowStatus.Normal,
        position: {
            x: 0,
            y: 0
        },
        nodes: {
            title: {},
            main: {},
            frame: {}
        },
        loader: {
            url: "",
            autoRefresh: false,
            tick: 0
        },
        titleid: "",
        mainid: "",
        url: "",
        frameid: "",
        //status : bs_field_Status_Selected,
        focus: true,
        display: this.static.display.Show,
        resizeable: false,
        minimum: false,
        maximum: false,
        closeable: true,
        inTaskBar: false,
        moveable: false,
        target: window,
        html: null,
        callback: null,                     //窗体关闭毁回调函数
        zIndex: 0,
        returnValue: new Object(), //窗口返回值，用于回调函数返回窗体值
        arguments: {}, //传入窗口参数
        //windowResult : windowFactory.windowResult.cancel,
        icon: {
            show: true,
            url: ""
        },
        create: function() {
            if ("" == this.id) {
                this.id = "window" + (this.static.items.length + 1);
            }
            if (this.minimum) {
                this.inTaskBar = true;
            }
            if (this.inTaskBar && false) {
                //todo.... 添加任务栏组件
            }
            //创建title节点
            var titleContent = "";
            if (this.icon.show) {
                if ("" == this.url) {
                    this.icon.url = this.static.configs.defaultIconURL;
                }
                titleContent = "<img class='window-icon' src='" + this.icon.url + "' align='absMiddle' />";
            }
            titleContent = "<nobr>" + titleContent + this.title + "</nobr>";
            this.nodes.title = Bluesky.create("div", { className: "window-title", html: "" }).height(this.static.configs.titleHeight);
            this.nodes.title.append(Bluesky.create("div", { className: "window-title-text", html: titleContent }));

        }
    });
}