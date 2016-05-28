/*
*
* Bluesky Components Masklayer Library v1.0
* 
* Copyright 2014, Yihuan Hu
*
*/
(function(Bluesky) {
    if (Bluesky && Bluesky.component) {
        Bluesky.extend(false, Bluesky.component, { Masklayer: function() {
            var com = Bluesky.extend(true, {}, this, arguments[0], Bluesky.component.prototype);
            this.component.aList.push(com);
            return com;
        }
        });

        Bluesky.extend(false, Bluesky.component.Masklayer.prototype, {
            parent: document,
            className: "bluesky-masklayer",
            width: 1,
            height: 1,
            background: "#000000",
            html: " ",
            node: {},
            zIndex: -1,
            onClick: null,
            component: Bluesky.component.Masklayer,
            show: function() {
                if (!this.parent)
                    return;
                this.node = Bluesky.create("div", { className: this.className, html: this.html });

                if (this.width > 1) {
                    this.node.css("width", this.width);
                }
                if (this.height > 1) {
                    this.node.css("height", this.height);
                }
                if (this.zIndex != -1) {
                    this.node.css("zIndex", this.zIndex);
                } else {
                    this.node.css("zIndex", this.zIndex = (9989 + (this.component.aList.length - 1) * 2));
                }
                Bluesky(this.parent.body).prepend(this.node);
                if (this.onClick) {
                    this.node.addEvent("click", this.onClick);
                }
                this.node.animate({ "opacity": 0.4 }, 150);
                return this;
            },
            remove: function() {
                this.node.remove();
                this.component.aList.pop(this);
                this.dispose();
            }
        });

        Bluesky.extend(false, Bluesky.component.Masklayer, {
            aList: [],
            create: function() {
                var instance = new Bluesky.component.Masklayer(arguments[0]);
                return instance.show();
            },
            remove: function() {
                if (this.aList.length == 0)
                    return;
                var index = this.aList.length - 1, i = arguments[0];
                if (Bluesky.util.type(i) == "number" && i >= 0 && i <= index)
                    index = i;
                this.aList[index].remove();
            },
            removeAll: function() {
                while (this.aList.length > 0) {
                    this.aList[this.aList.length - 1].remove();
                }
            }
        });

        Bluesky.extend(false, Bluesky.component.Masklayer, {
            loading: function() {
                var parent = arguments[0] ? arguments[0] : document,
            w = parent.body.offsetWidth,
            h = parent.body.offsetHeight,
            src = "Include/image/loading.gif",
            ml = (w - 20) / 2,
            mt = (h - 20) / 2,
            html = "<img src='" + src + "' style='margin-left:" + ml + "px;margin-top:" + mt + "px;' />";
                return this.create({ className: "bluesky-masklayer-loading", html: html });
            }
        });
    }
})(Bluesky);
    

