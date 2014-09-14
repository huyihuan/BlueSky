﻿if (Bluesky && Bluesky.component) {
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
        },
        focusWindow : null
    });

    Bluesky.extend(false, Bluesky.component.Window.prototype, {
        id: "",
        key: "",
        width: 300,
        height: 200,
        title: "未命名",
        renderTo: null,
        static: null,
        status: null,
        position: {
            x: -1,
            y: -1
        },
        nodes: {
            wrapper: null,
            title: null,
            main: null,
            frame: null,
            close: null,
            max: null,
            min: null
        },
        loader: {
            url: "",
            autoRefresh: false,
            tick: 0
        },
        focus: false,
        display: null,
        resizeable: false,
        minimum: false,
        maximum: false,
        closeable: true,
        inTaskBar: false,
        moveable: false,
        drag: null,
        resizer: null,
        target: null,
        html: null,
        callback: null, //窗体关闭回调函数
        zIndex: 0,
        returnValue: null, //窗口返回值，用于回调函数返回窗体值
        arguments: null, //传入窗口参数
        //windowResult : windowFactory.windowResult.cancel,
        icon: {
            show: true,
            url: ""
        },
        show: function() {
            //相关参数初始化
            this.static = Bluesky.component.Window;
            this.status = this.static.status.Normal;
            this.display = this.static.display.Show;
            this.target = Bluesky(this.renderTo);
            if ("" == this.id) {
                this.id = "Bluesky_Window" + (this.static.items.length + 1);
            }
            if (this.minimum) {
                this.inTaskBar = true;
            }
            if (this.inTaskBar && false) {
                //todo.... 添加任务栏组件
            }
            var closure = this;
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
            this.nodes.title = Bluesky.create("div", { className: "window-title window-title-normal" }).height(this.static.configs.titleHeight);
            delete this.title;
            //添加窗体Icon和title
            this.nodes.title.append(Bluesky.create("div", { className: "window-title-text", html: titleContent }));
            //添加窗体操作按钮
            if (this.closeable) {
                this.nodes.close = Bluesky.create("a", { className: "window-button window-button-close" }).addEvent("click", function() { closure.close(); });
                this.nodes.title.append(this.nodes.close);
            }
            if (this.maximum) {
                this.nodes.max = Bluesky.create("a", { className: "window-button window-button-max" }).addEvent("click", function() { closure.toggleStatus(); });
                this.nodes.title.append(this.nodes.max);
                this.nodes.title.addEvent("dblclick", function() { closure.toggleStatus(); });
            }
            if (this.minimum) {
                this.nodes.min = Bluesky.create("a", { className: "window-button window-button-min", html: "" }).addEvent("click", function() { closure.toMin(); });
                this.nodes.title.append(this.nodes.min);
            }
            //创建窗体主体部分
            this.nodes.main = Bluesky.create("div", { className: "window-main" }).height(this.height - this.static.configs.titleHeight - 1);
            if (this.loader && this.loader.url) {
                this.nodes.frame = Bluesky.create("iframe", { width: "100%", height: "100%", frameborder: "0" })
                this.nodes.main.append(this.nodes.frame);
                setTimeout(function() { closure.nodes.frame.attr("src", closure.loader.url); }, 0);
            }
            //计算窗体的位置(默认出现在窗体的中心)
            if (this.position.x == -1 && this.position.y == -1) {
                this.position.x = (this.target.width() - this.width) / 2;
                this.position.y = (this.target.height() - this.height) / 2;
            }
            //创建主窗体
            this.nodes.wrapper = Bluesky.create("div", { className: "window-popup window-normal", id: this.id }).height(this.height).width(this.width).css({ left: this.position.x + "px", top: this.position.y + "px" });
            this.nodes.wrapper.append(this.nodes.title).append(this.nodes.main).addEvent("mousedown", function(e) { closure.toFocus(); Bluesky.stopPropagation(e); });
            //创建窗体resize组件
            if (this.resizeable) {
                this.resizer = new Bluesky.component.Resizer({
                    context: this,
                    target: this.nodes.wrapper,
                    onResizeEnd: function(eventArgs) {
                        var endSize = eventArgs.endSize;
                        var endPos = eventArgs.endPosition;
                        if (endSize.height != this.height) {
                            this.nodes.main.height(endSize.height - this.static.configs.titleHeight - 1);
                        }
                        closure.height = endSize.height;
                        closure.width = endSize.width;
                        closure.position = {
                            x: endPos.x,
                            y: endPos.y
                        }
                    }
                });
                this.resizer.init();
            }
            if (this.moveable) {
                this.drag = new Bluesky.component.Drag({
                    context: this,
                    mover: this.nodes.wrapper,
                    operater: this.nodes.title,
                    onDragStart: function(eventArgs) {
                        if (closure.status == closure.static.status.Max) {
                            return false;
                        }
                    },
                    onDragEnd: function(eventArgs) {
                        closure.position.x = eventArgs.endPosition.x;
                        closure.position.y = eventArgs.endPosition.y;
                    }
                });
                this.drag.init();
            }
            this.target.append(this.nodes.wrapper);
            this.static.items.push(this.static.focusWindow = this.toFocus());
        },
        close: function() {
            this.nodes.wrapper.remove();
            var closure = this;
            setTimeout(function() { closure = null; }, 0);
        },
        toggleStatus: function() {
            var s = this.static.status;
            if (this.status == s.Normal) {
                this.toMax();
            }
            else if (this.status == s.Max) {
                this.toNormal();
            }
            return this;
        },
        toMax: function() {
            var b = this.target;
            this.setPosition({ x: 0, y: 0 });
            this.setSize({ width: b.width() - 2, height: b.height() - 2 });
            this.nodes.max.replaceClass("window-button-max", "window-button-normal");
            this.status = this.static.status.Max;
            return this;
        },
        toNormal: function() {
            this.setPosition(this.position);
            this.setSize({ width: this.width, height: this.height });
            this.nodes.max.replaceClass("window-button-normal", "window-button-max");
            this.status = this.static.status.Normal;
            return this;
        },
        toMin: function() {
            this.setVisible(false);
            return this;
        },
        toFocus: function() {
            if (this.focus) {
                return this;
            }
            if (this.static.focusWindow) {
                this.static.focusWindow.toBlur();
            }
            this.nodes.wrapper.replaceClass("window-normal", "window-active");
            this.nodes.title.replaceClass("window-title-normal", "window-title-active");
            this.focus = true;
            this.static.focusWindow = this;
            return this;
        },
        toBlur: function() {
            if (!this.focus) {
                return this;
            }
            this.nodes.wrapper.replaceClass("window-active", "window-normal");
            this.nodes.title.replaceClass("window-title-active", "window-title-normal");
            this.focus = false;
            return this;
        },
        setVisible: function(_bVisible) {
            _bVisible == true ? this.nodes.wrapper.show() : this.nodes.wrapper.hide();
            return this;
        },
        setSize: function() {
            var size = arguments[0];
            if (!size) {
                return this;
            }
            if (size.width >= 0) {
                this.nodes.wrapper.width(size.width);
            }
            if (size.height >= 0) {
                this.nodes.wrapper.height(size.height);
                this.nodes.main.height(size.height);
            }
            return this;
        },
        setPosition: function() {
            var p = arguments[0];
            if (!p) {
                return;
            }
            this.nodes.wrapper.css({ left: p.x + "px", top: p.y + "px" });
            return this;
        }
    });
}