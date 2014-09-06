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
        static: {},
        status: {},
        position: {
            x: -1,
            y: -1
        },
        nodes: {
            self: {},
            title: {},
            main: {},
            frame: {},
            close: {},
            max: {},
            min: {}
        },
        loader: {
            url: "",
            autoRefresh: false,
            tick: 0
        },
        focus: true,
        display: {},
        resizeable: false,
        minimum: false,
        maximum: false,
        closeable: true,
        inTaskBar: false,
        moveable: false,
        target: {},
        html: null,
        callback: null,                     //窗体关闭毁回调函数
        zIndex: 0,
        returnValue: {}, //窗口返回值，用于回调函数返回窗体值
        arguments: {}, //传入窗口参数
        //windowResult : windowFactory.windowResult.cancel,
        icon: {
            show: true,
            url: ""
        },
        create: function() {
            //相关参数初始化
            this.static = Bluesky.component.Window;
            this.status = this.static.status.Normal;
            this.display = this.static.display.Show;
            this.target = window;
            if ("" == this.id) {
                this.id = "Bluesky_Window" + (this.static.items.length + 1);
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
                delete this.icon.url;
            }
            titleContent = "<nobr>" + titleContent + this.title + "</nobr>";
            this.nodes.title = Bluesky.create("div", { className: "window-title", html: "" }).height(this.static.configs.titleHeight);
            delete this.title;
            //添加窗体Icon和title
            this.nodes.title.append(Bluesky.create("div", { className: "window-title-text", html: titleContent }));
            //添加窗体操作按钮
            var dd = this;
            if (this.closeable) {
                this.nodes.close = Bluesky.create("a", { className: "window-button window-button-close" }).addEvent("click", function() { dd.close(); });
                this.nodes.title.append(this.nodes.close);
            }
            if (this.maximum) {
                this.nodes.max = Bluesky.create("a", { className: "window-button window-button-max" }).addEvent("click", function() { dd.toggleStatus(); });
                this.nodes.title.append(this.nodes.max);
            }
            if (this.minimum) {
                this.nodes.min = Bluesky.create("a", { className: "window-button window-button-min", html: "" }).addEvent("click", function() { dd.toMin(); });
                this.nodes.title.append(this.nodes.min);
            }
            //创建窗体主体主体部分
            this.nodes.main = Bluesky.create("div", { className: "window-main" }).height(this.height - this.static.configs.titleHeight - 1);

            //计算窗体的位置(默认出现在窗体的中心)
            if (this.position.x == -1 && this.position.y == -1) {
                var _body = Bluesky(this.target.document.body);
                this.position.x = (_body.width() - this.width) / 2;
                this.position.y = (_body.height() - this.height) / 2;
            }
            //创建主窗体
            this.nodes.self = Bluesky.create("div", { className: "window-popup window-normal" }).height(this.height).width(this.width).css("left", this.position.x + "px").css("top", this.position.y + "px");
            this.nodes.self.append(this.nodes.title).append(this.nodes.main);
            //创建窗体resize组件
            if (this.resizeable) {
                this.nodes.self.append(Bluesky.create("div", { className: "window-resize-border window-resize-w" }).addEvent("mousedown"));
            }



            Bluesky(this.target.document.body).append(this.nodes.self);
        },
        close: function() {
            this.nodes.self.remove();
            var dd = this;
            setTimeout(function() { dd = null; }, 0);
        },
        toggleStatus: function() {
            var s = this.static.status;
            if (this.status == s.Normal) {
                this.toMax();
            }
            else if (this.status == s.Max) {
                this.toNormal();
            }
        },
        toMax: function() {
            var b = Bluesky(this.target.document.body);
            this.setPosition({ x: 0, y: 0 });
            this.setSize({ width: b.width() - 2, height: b.height() - 2 });
            this.nodes.max.replaceClass("window-button-max", "window-button-normal");
            this.status = this.static.status.Max;
        },
        toNormal: function() {
            this.setPosition(this.position);
            this.setSize({ width: this.width, height: this.height });
            this.nodes.max.replaceClass("window-button-normal", "window-button-max");
            this.status = this.static.status.Normal;
        },
        toMin: function() {
            this.setVisible(false);
        },
        setVisible: function(_bVisible) {
            _bVisible == true ? this.nodes.self.show() : this.nodes.self.hide();
        },
        setSize: function() {
            var size = arguments[0];
            if (!size) {
                return;
            }
            if (size.width >= 0) {
                this.nodes.self.width(size.width);
            }
            if (size.height >= 0) {
                this.nodes.self.height(size.height);
            }
        },
        setPosition: function() {
            var p = arguments[0];
            if (!p) {
                return;
            }
            this.nodes.self.css("left", p.x + "px");
            this.nodes.self.css("top", p.y + "px");
        }
    });
}