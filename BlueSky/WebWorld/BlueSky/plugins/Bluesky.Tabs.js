/*
*
* Bluesky Components Tabs Library v1.0
* 
* Copyright 2014, Yihuan Hu
*
*/
(function(Bluesky) {
    if (Bluesky && Bluesky.component) {
        Bluesky.extend(false, Bluesky.component, { Tabs: function() {
            return Bluesky.extend(true, {}, this, arguments[0], Bluesky.component.prototype);
        }
        });

        Bluesky.extend(false, Bluesky.component.Tabs.prototype, {
            renderTo: " ",
            width: 2,
            height: 35,
            items: [],
            list: [],
            activeIndex: 1,
            sliding: false,
            component: Bluesky.component.Tabs,
            viewstate: "",
            iconFolder: "",
            moveDistance: 0,
            moveStep: 29,
            moverWidth: 20,
            nodesWidth: 0,
            leftEnd: function() { return (this.width - this.moveDistance) == (this.nodesWidth + this.moverWidth + 4); },
            rightEnd: function() { return (this.moveDistance == this.moverWidth); },
            hasMover: false,
            _viewNode: {},
            _wrapper: {},
            _tabsWrapper: {},
            node: {
                tabsBarWrapper: null,
                leftMover: null,
                rightMover: null
            },
            _tabsNode: {},
            _contentsNode: {},
            _setWidth: function() {
                var width = arguments[0] || 2;
                if (typeof width === "number" && width >= 2) {
                    this.width = width;
                }
                else if (this.width < 2) {
                    this.width = 2;
                }
                return this;
            },
            _setHeight: function() {
                var height = arguments[0] || 35;
                if (typeof height === "number" && height >= 35) {
                    this.height = height;
                } else if (this.height < 35) {
                    this.height = 35;
                }
                return this;
            },
            create: function() {
                this._setWidth(this.width)._setHeight(this.height);
                this._wrapper = Bluesky.create("div", { className: "bluesky-wrapper" }).width(this.width).height(this.height);
                this._tabsWrapper = Bluesky.create("div", { className: "bluesky-tabs-wrapper" }).width(this.width - 2);
                this.node.tabsBarWrapper = Bluesky.create("div", { className: "bluesky-tabs-barwrapper" });


                this._tabsNode = Bluesky.create("div", { className: "bluesky-tabs-bar" });


                this._contentsNode = Bluesky.create("div", { className: "bluesky-tabs-content-wrapper" });
                this._contentsNode.height(this.height - 35).width(this.width - 2);
                if (this.viewstate !== "") {
                    this._viewNode = Bluesky(this.viewstate);
                    var aIndex = this._viewNode.value();
                    if (this._viewNode && aIndex != undefined && aIndex !== "")
                        this.activeIndex = parseInt(aIndex, 10);
                }
                var activeIndex = this.activeIndex,
				tabs = this;
                Bluesky.foreach(this.items, function(obj, i) {
                    this.isActive = i == activeIndex;
                    if (this.showIcon === true && tabs.iconFolder !== "") {
                        this.iconURL = tabs.iconFolder + "\\" + this.iconURL;
                    }
                    tabs.add(this);
                });
                this.node.tabsBarWrapper.append(this._tabsNode);
                this._tabsWrapper.append(Bluesky([this.node.tabsBarWrapper.element(), Bluesky.create("div", { className: "bluesky-tabs-bottom" }).element()]));
                this._wrapper.append(this._tabsWrapper).append(this._contentsNode);
                Bluesky(this.renderTo).append(this._wrapper);
                return this;
            },
            add: function() {
                var tabArgs = arguments[0];
                if (tabArgs.key) {
                    var nIndex = this.list.indexOfProperty(tabArgs.key, "key");
                    if (nIndex != -1 && (this.list[nIndex].isRunMutil != true)) {
                        this.setActiveTab(nIndex);
                        return;
                    }
                }
                var tab = this.component.Tab.create(tabArgs),
                tabs = this;
                tab.tabNode.addEvent("click", function() {
                    if (tab.disabled == true)
                        return;
                    tabs.setActiveTab(tab);
                });
                if (tab.isActive === true) {
                    if (this.activeTab.title) {
                        this.activeTab.toNormal();
                    }
                    this.activeTab = tab;
                    if (tab.loader.url != " " && tab.loader.autoLoad === false && tab.loader._loaded === false) {
                        try { tab.contentNode.attr("src", tab.loader.url); } catch (ee) { }
                        tab.loader._loaded = true;
                    }
                }
                if (tab.closeable === true) {
                    tab.closeNode.addEvent("click", function(e) {
                        if (typeof tab.closingFn === "function") {
                            if (tab.closingFn() === false)
                                return;
                        }
                        tabs.remove(tab);
                        e = e ? e : window.event;
                        if (e && e.stopPropagation) {
                            e.stopPropagation();
                        }
                        else {
                            window.event.cancelBubble = true;
                        }
                        if (e && e.preventDefault) {
                            e.preventDefault();
                        }
                    });
                }
                this._tabsNode.append(tab.tabNode);
                this._contentsNode.append(tab.contentNode);
                if (tab.sliding === true && tab.isActive === false) {
                    tab.contentNode.css("top", (35 - this.height) + "px");
                }
                this.list.push(tab);
                setTimeout(function() {
                    tabs.nodesWidth += tab.tabNode.width() + 3;
                    if (!tabs.hasMover && tabs.nodesWidth >= tabs.width) {
                        tabs.showMover();
                    }
                    if (tabs.hasMover) {
                        tabs.moveLeftEnd();
                    }
                }, 0);
            },
            remove: function() {
                var tab = arguments[0];
                if (!tab) {
                    return;
                }
                this.list.remove(tab);
                if (tab.isActive === true && this.list.length >= 1) {
                    this.setActiveTab(this.list.length - 1);
                }
                this.nodesWidth -= (tab.tabNode.width() + 3);
                tab.remove();
                if (this.hasMover) {
                    if (this.nodesWidth <= this.width) {
                        this.hiddenMover();
                    }
                    else {
                        this.moveLeftEnd();
                    }
                }
            },
            activeTab: {},
            setActiveTab: function() {
                var tab = arguments[0];
                if (typeof tab === "number") {
                    this.activeIndex = tab;
                    tab = this.list[tab];
                }
                if (undefined === tab || tab.isActive === true)
                    return;
                if (typeof tab !== "number") {
                    this.activeIndex = this.list.indexOf(tab);
                }
                if (this.viewstate !== "" && this._viewNode) {
                    this._viewNode.value(this.activeIndex);
                }
                this.activeTab.toNormal();
                this.activeTab = tab.toActive();
            },
            resize: function() {
                var size = arguments[0];
                if (undefined !== size && null != size) {
                    if (this.width === size.width && this.height === size.height) {
                        return;
                    }
                    size.width && (this._setWidth(size.width));
                    size.height && (this._setHeight(size.height));
                }
                this._tabsWrapper.width(this.width - 2);
                if (this.nodesWidth >= this.width && !this.hasMover) {
                    this.showMover();
                }
                else if (this.nodesWidth < this.width && this.hasMover) {
                    this.hiddenMover();
                }
                this._contentsNode.height(this.height - 35).width(this.width - 2);
                this._wrapper.width(this.width).height(this.height);
            },
            showMover: function() {
                if (!this.hasMover) {
                    this.node.leftMover = Bluesky.create("div", { className: "bluesky-tabs-mover bluesky-tabs-mover-left-normal" });
                    this.node.rightMover = Bluesky.create("div", { className: "bluesky-tabs-mover bluesky-tabs-mover-right-normal" });
                    var closure = this, leftInterval, rightInteral;
                    this.node.leftMover.addEvent("mousedown", function(e) {
                        leftInterval = window.setInterval(function() { closure.moveLeft(leftInterval); }, 100);
                    });
                    this.node.leftMover.addEvent("mouseup", function(e) { clearInterval(leftInterval); });
                    this.node.leftMover.addEvent("mouseout", function(e) { clearInterval(leftInterval); });
                    this.node.leftMover.addEvent("click", function(e) { closure.moveLeft(); });
                    this.node.rightMover.addEvent("mousedown", function(e) {
                        rightInteral = window.setInterval(function() { closure.moveRight(rightInteral); }, 100);
                    });
                    this.node.rightMover.addEvent("mouseup", function(e) { clearInterval(rightInteral); });
                    this.node.rightMover.addEvent("mouseout", function(e) { clearInterval(rightInteral); });
                    this.node.rightMover.addEvent("click", function(e) { closure.moveRight(); });
                    this.hasMover = true;
                }
                this.node.tabsBarWrapper.prepend(this.node.leftMover).append(this.node.rightMover);

            },
            hiddenMover: function() {
                this.node.leftMover.remove();
                this.node.rightMover.remove();
                this.node.leftMover = null;
                this.node.rightMover = null;
                this.hasMover = false;
                this._tabsNode.css("left", (this.moveDistance = 0) + "px");
            },
            leftEndDistance: function() {
                return this.width - this.nodesWidth - this.moverWidth - 4;
            },
            moveLeft: function(_moveInteral) {
                if (this.leftEnd()) {
                    if (undefined != _moveInteral) {
                        clearInterval(_moveInteral);
                    }
                    return;
                }
                if (this.rightEnd()) {
                    this.node.rightMover.disabled(false).replaceClass("bluesky-tabs-mover-right-disabled", "bluesky-tabs-mover-right-normal");
                }
                if (!this.leftEnd()) {
                    var dn = this.moveDistance - this.moveStep, ds = this.leftEndDistance();
                    this.moveDistance = dn <= ds ? ds : dn;
                    this._tabsNode.css("left", this.moveDistance + "px");
                }
                if (this.leftEnd()) {
                    this.node.leftMover.disabled(true).replaceClass("bluesky-tabs-mover-left-normal", "bluesky-tabs-mover-left-disabled");
                }
            },
            moveRight: function(_moveInteral) {
                if (this.rightEnd()) {
                    if (undefined != _moveInteral) {
                        clearInterval(_moveInteral);
                    }
                    return;
                }
                if (this.leftEnd()) {
                    this.node.leftMover.disabled(false).replaceClass("bluesky-tabs-mover-left-disabled", "bluesky-tabs-mover-left-normal");
                }
                if (!this.rightEnd()) {
                    var distance = this.moveDistance + this.moveStep;
                    this.moveDistance = distance >= this.moverWidth ? this.moverWidth : distance;
                    this._tabsNode.css("left", this.moveDistance + "px");
                }
                if (this.rightEnd()) {
                    this.node.rightMover.disabled(true).replaceClass("bluesky-tabs-mover-right-normal", "bluesky-tabs-mover-right-disabled");
                }
            },
            moveLeftEnd: function() {
                if (this.leftEnd()) {
                    return;
                }
                var closure = this,
                    leftInterval = window.setInterval(function() { closure.moveLeft(leftInterval); }, 50);
            },
            moveRightEnd: function() {
                if (this.rightEnd()) {
                    return;
                }
                var closure = this,
                    rightInterval = window.setInterval(function() { closure.moveRight(rightInterval); }, 50);
            },
            tab: function(index) {
                return this.list[index];
            }
        });

        Bluesky.extend(false, Bluesky.component.Tabs, {
            Tab: function() {
                return Bluesky.extend(true, {}, this, arguments[0], Bluesky.component.prototype);
            },
            create: function() {
                var tabs = new this(arguments[0]);
                return tabs.create();
            }
        });

        Bluesky.extend(true, Bluesky.component.Tabs.Tab.prototype, {
            title: "Unkown Title",
            tip: "",
            html: " ",
            key: "",
            isRunMutil: false,
            disabled: false,
            showIcon: false,
            iconURL: "include/image/application.png",
            closeable: true,
            closingFn: {},
            isActive: true,
            sliding: false,
            contentElement: " ",
            tabNode: {},
            closeNode: {},
            contentNode: {},
            create: function() {
                this.tabNode = Bluesky.create("a", {
                    className: "bluesky-tab " + (this.isActive ? "bluesky-tab-active" : "bluesky-tab-normal"),
                    html: this.title,
                    title: this.tip
                });
                if (this.showIcon === true) {
                    var bIcon = this.title === "";
                    this.tabNode.html("<img class='bluesky-tab-icon" + (bIcon ? " bluesky-tab-icon-only" : "") + "' src='" + this.iconURL + "' align='absmiddle' />" + (bIcon ? "" : ("&nbsp;" + this.title)));
                }
                if (this.disabled === true) {
                    this.tabNode.attr("disabled", true);
                    this.tabNode.addClass("bluesky-tab-disabled");
                }
                if (this.closeable === true) {
                    this.closeNode = Bluesky.create("a", { className: "bluesky-tab-close", html: "x", title: "关闭" });
                    this.tabNode.append(this.closeNode);
                }
                if (this.contentElement !== " ") {
                    this.contentNode = Bluesky.create("div", { className: "bluesky-tabs-content-node" });
                    this.contentNode.append(Bluesky(this.contentElement));
                }
                else if (this.html !== " ") {
                    this.contentNode = Bluesky.create("div", { className: "bluesky-tabs-content-node", html: this.html });
                }
                else if (this.loader.url !== " ") {
                    this.contentNode = Bluesky.create("iframe", { frameborder: 0, width: "100%", height: "100%" });
                    if (this.loader.autoLoad === true) {
                        this.contentNode.attr("src", this.loader.url);
                        this.loader._loaded = true;
                    }
                }
                else {
                    this.contentNode = Bluesky.create("div", { className: "bluesky-tabs-content-node", html: "Unkown Html!" });
                }
                if (this.isActive === false) {
                    this.contentNode.hide();
                }
                if (this.sliding === true) {
                    this.contentNode.addClass("bluesky-tabs-content-slider");
                }
                return this;
            },
            remove: function() {
                this.contentNode.remove();
                this.tabNode.remove();
                this.dispose();
            },
            toActive: function() {
                this.tabNode.replaceClass("bluesky-tab-normal", "bluesky-tab-active");
                this.contentNode.show();
                if (this.loader._loaded === false && this.loader.autoLoad === false) {
                    this.contentNode.attr("src", this.loader.url);
                    this.loader._loaded = true;
                }
                if (this.sliding === true) {
                    var top = parseFloat(this.contentNode.css("top").toString());
                    if (typeof top === "number" && !isNaN(top) && top != 0) {
                        this.contentNode.animate({ "top": 0 }, 200);
                    }
                }
                this.isActive = true;
                return this;
            },
            toNormal: function() {
                this.tabNode.replaceClass("bluesky-tab-active", "bluesky-tab-normal");
                this.contentNode.hide();
                this.isActive = false;
                return this;
            },
            loader: {
                url: " ",
                autoLoad: true,
                _loaded: false,
                params: " "
            },
            refresh: function() {
                this.contentNode.attr("src", this.loader.url);
            },
            add: function(element) {
                if (!element)
                    return;
                if (element.node && element.node.wrapper) {
                    this.contentNode.append(element.node.wrapper);
                }
                else {
                    this.contentNode.append(element);
                }
            }

        });

        Bluesky.extend(false, Bluesky.component.Tabs.Tab, {
            create: function() {
                var tab = new Bluesky.component.Tabs.Tab(arguments[0]);
                return tab.create();
            }
        });
    }
})(Bluesky);