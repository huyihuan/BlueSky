/*
*
* Bluesky Components Tree Library v1.0
* 
* Copyright 15/4/2015, Yihuan Hu
*
*/
(function(Bluesky) {
    if (Bluesky && Bluesky.component) {
        Bluesky.extend(false, Bluesky.component, {
            Tree: function() {
                return Bluesky.extend(true, {}, this, arguments[0], Bluesky.component.prototype);
            }
        });
        Bluesky.component.Tree.selectMode = { single: 1, multiple: 2 };
        Bluesky.extend(false, Bluesky.component.Tree.prototype, {
            id: "",
            renderTo: "",
            width: 0,
            height: 0,
            rootNode: {},
            childNodes: [],
            childrens: [],
            showCheckBox: false,
            selectedNode: null,
            selectMode: Bluesky.component.Tree.selectMode.single,
            node: {
                wrapper: null
            },
            loader: {
                url: "",
                autoLoad: true,
                _loaded: false,
                params: ""
            },
            init: function() {
                if (this.id === "") {
                    this.id = "BlueskyTree_" + Math.random();
                }
                this.node.wrapper = Bluesky.create("div", { className: "treelist" }).width(this.width).height(this.height);
                var colsure = this;
                if (this.loader && "" != this.loader.url) {
                    Bluesky.Ajax({
                        type: "get",
                        url: this.loader.url,
                        data: this.loader.params,
                        dataType: "json",
                        success: function(_children) {
                            var nodes = _children.json, length = nodes.length;
                            if (nodes && length) {
                                colsure.childrens = nodes;
                            }
                        },
                        fail: function() {
                            alert("获取失败");
                        }
                    });
                }
                if (this.childrens && this.childrens.length) {
                    this.initNode();
                }
                Bluesky(this.renderTo).append(this.node.wrapper);
                debugger;
                Bluesky.cache(null, this.id, this);
                delete this.renderTo;
                return this;
            },
            initNode: function() {
                var nodes = this.childrens, length = nodes.length;
                if (nodes && length) {
                    for (var i = 0; i < length; i++) {
                        var cNode = new Bluesky.component.TreeNode({
                            text: nodes[i].text,
                            value: nodes[i].value,
                            childrens: nodes[i].childrens,
                            index: i + 1,
                            siblingCount: length,
                            showCheckBox: true,
                            parentNode: null,
                            tree: this.id
                        });
                        cNode.init();
                        this.childNodes[i] = cNode;
                        this.node.wrapper.append(cNode.node.wrapper);
                        cNode.initChildren();
                    }
                    delete this.childrens;
                    this.childrens = null;
                }
            },
            addNode: function(_node) {

            },
            removeNode: function(_node) {

            },
            select: function(_node) {
                if (this.selectedNode && _node != this.selectedNode) {
                    this.selectedNode.toNormal();
                }
                if (_node != this.selectedNode) {
                    _node.toSelected();
                    this.selectedNode = _node;
                }
            }
        });

        Bluesky.extend(false, Bluesky.component, {
            TreeNode: function() {
                var static = {}, key;
                if (arguments[0] && arguments[0].static) {
                    static = arguments[0].static;
                    arguments[0].static = null;
                    delete arguments[0].static;
                }
                var node = Bluesky.extend(true, {}, this, arguments[0], Bluesky.component.prototype);
                //同一类型的所有实体，静态共享同一属性
                for (key in static)
                    node[key] = static[key];
                return node;
            }
        });

        Bluesky.extend(true, Bluesky.component.TreeNode.prototype, {
            text: "",
            value: {},
            html: "",
            key: {},
            childNodes: [],
            childrens: [],
            parentNode: null,
            isSelected: false,
            index: 1,
            siblingCount: 1,
            showCheckBox: false,
            showIcon: true,
            iconURL: "include/image/application.png",
            tree: "",
            onSelected: null,
            onBlur: null,
            node: {
                wrapper: {},
                childrenWrapper: {},
                head: {},
                operator: {},
                checkbox: {}
            },
            init: function() {
                this.node.wrapper = Bluesky.create("table", { className: "tree-node", width: "100%", height: "20px", border: "0", cellspacing: "0", cellpadding: "0" });
                var tr = Bluesky.create("tr"), childrenCount = this.childrens.length, parentNode = this.parentNode, coulsure = this, spanClass;
                while (parentNode) {
                    //添加虚线
                    spanClass = "tree-node-branch-common tree-node-branch-" + (parentNode.index == parentNode.siblingCount ? "empty" : "line");
                    tr.prepend(Bluesky.create("td", { nowrap: "nowrap", className: "tree-node-branch-wrapper", html: "<span class='" + spanClass + "'></span>" }));
                    parentNode = parentNode.parentNode;
                }
                //添加TreeNode头部内容

                if (childrenCount == 0) {
                    spanClass = "tree-node-branch-common tree-node-branch-";
                    if (null == this.parentNode) {
                        spanClass += (this.siblingCount == 1 ? "one" : (this.index == 1 ? "first" : (this.index == this.siblingCount ? "last" : "middle")));
                    }
                    else {
                        spanClass += (this.index == this.siblingCount ? "last" : "middle");
                    }
                    this.node.head = Bluesky.create("span", { className: "tree-node-head-normal", html: "<span class='tree-node-head-icon-leaf'>" + this.text + "</span>" });
                    this.node.head.addEvent("click", function() {
                        var tree = Bluesky.cache(null, coulsure.tree);
                        if (tree) {
                            tree.select(coulsure);
                        }
                        if (coulsure.onSelected) {
                            var result = coulsure.onSelected.call(coulsure, coulsure);
                            if (result === false)
                                return false;
                        }
                    });
                    tr.append(Bluesky.create("td", { nowrap: "nowrap", className: "tree-node-branch-wrapper", html: "<span class='" + spanClass + "'></span>" }));
                }
                else {
                    spanClass = "tree-node-branch-common tree-node-dot-";
                    if (null == this.parentNode) {
                        spanClass += (this.siblingCount == 1 ? "one-" : (this.index == 1 ? "first-" : (this.index == this.siblingCount ? "last-" : ""))) + "plus";
                    }
                    else {
                        spanClass += (this.index == this.siblingCount ? "last-" : "") + "plus";
                    }
                    this.node.operator = Bluesky.create("span", { className: spanClass });
                    this.node.head = Bluesky.create("span", { className: "tree-node-head-normal", html: "<span class='tree-node-folder-normal'>" + this.text + "</span>" });

                    var toggleStatus = function() { coulsure.node.childrenWrapper.isHidden() ? coulsure.plus() : coulsure.minus(); };
                    this.node.head.addEvent("click", function() {
                        toggleStatus();
                        var tree = Bluesky.cache(null, coulsure.tree);
                        if (tree) {
                            tree.select(coulsure);
                        }
                        if (coulsure.onSelected) {
                            var result = coulsure.onSelected.call(coulsure, coulsure);
                            if (result === false)
                                return false;
                        }
                    });
                    this.node.operator.addEvent("click", toggleStatus);
                    tr.append(Bluesky.create("td", { nowrap: "nowrap", className: "tree-node-branch-wrapper" }).append(this.node.operator));
                }
                if (this.showCheckBox) {
                    this.node.checkbox = Bluesky.create("input", { type: "checkbox" });
                    this.node.checkbox.addEvent("click", function(e) {
                        Bluesky.stopPropagation(e);
                        var arr = coulsure.childNodes, len = arr.length;
                        if (len >= 1) {
                            var cb = coulsure.node.checkbox.checked(), i = 0;
                            for (; i < len; i++) {
                                arr[i].check(cb);
                            }
                        }

                    });
                    this.node.head.children().first().prepend(this.node.checkbox);
                }
                tr.append(Bluesky.create("td", { nowrap: "nowrap" }).append(this.node.head));
                this.node.wrapper.append(tr);
                return this;
            },
            initChildren: function() {
                var nodes = this.childrens, length = nodes.length;
                if (nodes && length) {
                    this.node.childrenWrapper = Bluesky.create("div", { className: "tree-nodes-box" });
                    for (var i = 0; i < length; i++) {
                        var cNode = new Bluesky.component.TreeNode({
                            text: nodes[i].text,
                            value: nodes[i].value,
                            childrens: nodes[i].childrens,
                            //childNodes: [], //防止所有的Node指向同一个原型链属性?
                            index: i + 1,
                            siblingCount: length,
                            showCheckBox: true,
                            parentNode: this,
                            tree: this.tree
                        });
                        cNode.init();
                        this.childNodes[i] = cNode;
                        this.node.childrenWrapper.append(cNode.node.wrapper);
                        cNode.initChildren();
                    }
                    this.childrens = null;
                    delete this.childrens;
                    this.node.wrapper.parent().append(this.node.childrenWrapper);
                }
                return this;
            },
            remove: function() {
                this.node.wrapper.remove();
                this.dispose();
            },
            plus: function() {
                var folder = this.node.head.children().first(), fClass = folder.attr("class"),
                    operator = this.node.operator, oClass = operator.attr("class");
                this.node.childrenWrapper.show();
                folder.attr("class", fClass.replace("normal", "active"));
                operator.attr("class", oClass.replace("plus", "minus"));
                return this;
            },
            minus: function() {
                var folder = this.node.head.children().first(), fClass = folder.attr("class"),
                    operator = this.node.operator, oClass = operator.attr("class");
                this.node.childrenWrapper.hide();
                folder.attr("class", fClass.replace("active", "normal"));
                operator.attr("class", oClass.replace("minus", "plus"));
                return this;
            },
            check: function(cb) {
                var i = 0, len = this.childNodes.length;
                this.node.checkbox.checked(cb);
                for (; i < len; i++) {
                    (this.childNodes[i]).node.checkbox.checked(cb);
                    (this.childNodes[i]).check(cb);
                }
            },
            toSelected: function() {
                this.node.head.replaceClass("tree-node-head-normal", "tree-node-head-active");
                return this;
            },
            toNormal: function() {
                this.node.head.replaceClass("tree-node-head-active", "tree-node-head-normal");
                return this;
            },
            addNode: function(_node) {

            }
        });
    }
})(Bluesky);