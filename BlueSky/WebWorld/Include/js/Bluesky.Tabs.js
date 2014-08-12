/**********************************************
* Bluesky框架组件库 Tabs组件
* @copyright huyihuan 2013
* Date: 2013-11-12 20:31:11
**********************************************/
if (Bluesky && Bluesky.component) {
    Bluesky.extend(false, Bluesky.component, { Tabs: function() {
			var args = arguments[0];
			return Bluesky.extend(true, {}, this, args);
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
        _viewNode: {},
        _wrapper: {},
        _tabsWrapper: {},
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
            this._tabsWrapper.append(Bluesky([this._tabsNode[0], Bluesky.create("div", { className: "bluesky-tabs-bottom" })[0]]));
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
            tab.remove();
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
            this._contentsNode.height(this.height - 35).width(this.width - 2);
            this._wrapper.width(this.width).height(this.height);
        }

    });

    Bluesky.extend(false, Bluesky.component.Tabs, {
        Tab: function() {
            var args = arguments[0];
            return Bluesky.extend(true, {}, this, args);
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
        }
    });

    Bluesky.extend(false, Bluesky.component.Tabs.Tab, {
        create: function() {
            var tab = new Bluesky.component.Tabs.Tab(arguments[0]);
            return tab.create();
        }
    });
}