/*
*
* Bluesky Components Window Library v1.0
* 
* Copyright 2014, Yihuan Hu
*
*/
(function(Bluesky) {
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
            dialogResult: {
                OK: 1,
                Cancel: 0
            },
            configs: {
                defaultIconURL: "Include/image/application.png",
                titleHeight: 25,
                minWidth: 150,
                minHeight: 150,
                borderWidth: 6
            },
            activeWindow: null
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
                tick: 0,
                onLoad: null
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
            zIndex: 0,
            returnValue: null, //窗口返回值，用于回调函数返回窗体值
            arguments: null, //传入窗口参数
            //windowResult : windowFactory.windowResult.cancel,
            mask: false,
            masklayer: null,
            flicker: false,
            icon: {
                show: true,
                url: ""
            },
            controls: [],
            onClosing: null,
            onClosed: null,
            show: function() {
                //相关参数初始化
                this.static = Bluesky.component.Window;
                this.status = this.static.status.Normal;
                this.display = this.static.display.Show;
                this.target = Bluesky(this.renderTo);
                if ("" == this.id) {
                    this.id = "BlueskyWindow" + (this.static.items.length + 1);
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
                    this.nodes.close = Bluesky.create("a", { className: "window-button window-button-close" }).addEvent("click", function() { closure.close(); setTimeout(function() { closure = null; }, 0); });
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
                this.nodes.main = Bluesky.create("div", { className: "window-main" }).height(this.height - this.static.configs.titleHeight - 1 - this.static.configs.borderWidth).width(this.width - 2 * this.static.configs.borderWidth).css("margin", "0px 0px 0px " + (this.static.configs.borderWidth - 1) + "px");
                if (this.loader && this.loader.url) {
                    this.nodes.frame = Bluesky.create("iframe", { width: "100%", height: "100%", frameborder: "0" });
                    this.nodes.frame.addEvent("focus", function() {
                        closure.toFocus();
                    });
                    if (typeof (this.loader.onLoad) == "function") {
                        this.nodes.frame.addEvent("load", function() {
                            if (closure.nodes.frame.window) {
                                closure.nodes.frame.window.arguments = closure.arguments;
                            }
                            closure.loader.onLoad.call(closure);
                        });
                    }
                    setTimeout(function() { closure.nodes.frame.attr("src", closure.loader.url); }, 0);
                    this.nodes.main.append(this.nodes.frame);
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
                                this.nodes.main.height(endSize.height - this.static.configs.titleHeight - 1 - this.static.configs.borderWidth);
                            }
                            if (endSize.width != this.width) {
                                this.nodes.main.width(endSize.width - 2 * this.static.configs.borderWidth);
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
                //创建窗体拖拽组件
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
                if (this.mask === true) {
                    this.masklayer = new Bluesky.component.Masklayer({
                        onClick: !this.flicker ? null : function() {
                            var flag = 1, title = closure.nodes.title, wrap = closure.nodes.wrapper, isActive = title.hasClass("window-title-active"),
                            tick = function() {
                                title.replaceClass("window-title-" + (isActive ? "active" : "normal"), "window-title-" + (isActive ? "normal" : "active"));
                                wrap.toggleClass("window-flicker", isActive);
                                flag++ <= 5 || clearInterval(fire);
                                isActive = !isActive;
                            },
                            fire = setInterval(tick, 90);
                        }
                    });
                    this.masklayer.show();
                    this.zIndex = this.masklayer.zIndex + 1;
                }
                if (this.zIndex > 0) {
                    this.nodes.wrapper.css("zIndex", this.zIndex);
                }
                this.target.append(this.nodes.wrapper);
                this.static.items.push(this.static.activeWindow = this.toFocus());
            },
            close: function() {
                this.dialogResult = this.static.dialogResult.Cancel;
                if (this.loader && this.loader.url != "") {
                    var cWindow = this.nodes.frame[0].contentWindow;
                    if (cWindow && cWindow.dialogResult) {
                        this.dialogResult = cWindow.dialogResult;
                    }
                }
                if (typeof (this.onClosing) == "function") {
                    var result = this.onClosing.call(this, this.dialogResult);
                    if (!result) {
                        return;
                    }
                }
                this.nodes.wrapper.remove();
                if (this.mask) {
                    this.masklayer.remove();
                }
                if (typeof (this.onClosed) === "function") {
                    this.onClosed.call(this, this.dialogResult);
                }
                //从数组中删除关闭的窗体对象，并且将数组中默认的第一个Focus
                var items = this.static.items;
                var nIndex = items.indexOfProperty(this.id, "id");
                if (nIndex != -1) {
                    items.splice(nIndex, 1);
                }
                if (items.length >= 1) {
                    items[items.length - 1].toFocus();
                }
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
                if (this.static.activeWindow) {
                    this.static.activeWindow.toBlur();
                }
                this.nodes.wrapper.replaceClass("window-normal", "window-active");
                this.nodes.title.replaceClass("window-title-normal", "window-title-active");
                this.focus = true;
                this.static.activeWindow = this;
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
                    this.nodes.main.width(size.width - 2 * this.static.configs.borderWidth);
                }
                if (size.height >= 0) {
                    this.nodes.wrapper.height(size.height);
                    this.nodes.main.height(size.height - this.static.configs.titleHeight - 1 - this.static.configs.borderWidth);
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
            },
            add: function() {
                var cs = arguments[0];
                if (!cs)
                    return;
                this.controls.push(cs);
                this.nodes.main.append(cs);
            }
        });
    }
})(Bluesky);