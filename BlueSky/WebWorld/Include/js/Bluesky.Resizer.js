/*
*
* Bluesky Components Resizer Library v1.0
* 
* Copyright 2014, Yihuan Hu
*
*/
(function(Bluesky) {
    var resizing = function(e, context, action) {
        this.resizeStart(e);
        document.ondragstart = function() { return false; };
        document.onselectstart = function() { return false; };
        document.onselect = function() { document.selection.empty(); };
        document.onmousemove = function(e) {
            if (!context.isResizing) {
                context.masklayer = Bluesky.create("div", { className: "resizer-mask" });
                context.movelayer = Bluesky.create("div", { className: "resizer-panel" }).css({ width: context.startSize.width + "px", height: context.startSize.height + "px", top: context.startPosition.y + "px", left: context.startPosition.x + "px" });
                context.target.parent().append(context.masklayer).append(context.movelayer);
                context.isResizing = true;
            }
            action.call(context, e);
        };
        document.onmouseup = function(e) {
            context.resizeEnd(e);
            document.onmousemove = null;
            document.onmouseup = null;
            document.ondragstart = null;
            document.onselectstart = null;
            document.onselect = null;
        };
    }
    if (Bluesky && Bluesky.component) {
        Bluesky.extend(false, Bluesky.component, { Resizer: function() {
            var args = arguments[0];
            return Bluesky.extend(true, {}, this, args);
        }
        });

        Bluesky.extend(true, Bluesky.component.Resizer.prototype, {
            context: null,
            target: null,
            parentSize: null,
            startSize: null,
            endSize: null,
            startPosition: null,
            endPosition: null,
            ePosition: null,
            masklayer: null,
            movelayer: null,
            isResizing: false,
            init: function() {
                if (null == this.target || undefined == this.target) {
                    return;
                }
                var closure = this;
                this.target.append(Bluesky.create("div", { className: "resizer-border resizer-w" }).addEvent("mousedown", function(e) { resizing.call(closure, e, closure, closure.toWest); }))
                           .append(Bluesky.create("div", { className: "resizer-border resizer-n" }).addEvent("mousedown", function(e) { resizing.call(closure, e, closure, closure.toNorth); }))
                           .append(Bluesky.create("div", { className: "resizer-border resizer-e" }).addEvent("mousedown", function(e) { resizing.call(closure, e, closure, closure.toEast); }))
                           .append(Bluesky.create("div", { className: "resizer-border resizer-s" }).addEvent("mousedown", function(e) { resizing.call(closure, e, closure, closure.toSourth); }))
                           .append(Bluesky.create("div", { className: "resizer-corner resizer-nw" }).addEvent("mousedown", function(e) { resizing.call(closure, e, closure, closure.toNorthWest); }))
                           .append(Bluesky.create("div", { className: "resizer-corner resizer-ne" }).addEvent("mousedown", function(e) { resizing.call(closure, e, closure, closure.toNorthEast); }))
                           .append(Bluesky.create("div", { className: "resizer-corner resizer-se" }).addEvent("mousedown", function(e) { resizing.call(closure, e, closure, closure.toSourthEast); }))
                           .append(Bluesky.create("div", { className: "resizer-corner resizer-sw" }).addEvent("mousedown", function(e) { resizing.call(closure, e, closure, closure.toSourthWest); }));

            },
            onResizeStart: null,
            onResizeEnd: null,
            resizeStart: function(e) {
                if (this.onResizeStart) {
                    var result = this.onResizeStart.call(this.context, { e: e });
                    if (result == false) {
                        return false;
                    }
                };
                e = Bluesky.getEventArg(e);
                var parent = this.target.parent();
                this.parentSize = { width: parent.width(), height: parent.height() };
                this.startSize = { width: this.target.width(), height: this.target.height() };
                this.ePosition = Bluesky.getPosition(e);
                this.startPosition = { x: this.target.css("left"), y: this.target.css("top") };
                this.startPosition = {
                    x: Bluesky.util.parseInt(this.startPosition.x.replace("px", ""), 0),
                    y: Bluesky.util.parseInt(this.startPosition.y.replace("px", ""), 0)
                };

                this.endSize = { width: this.startSize.width, height: this.startSize.height };
                this.endPosition = { x: this.startPosition.x, y: this.startPosition.y };
            },
            resizeEnd: function(e) {
                if (this.isResizing) {
                    this.masklayer.remove();
                    this.movelayer.remove();
                    var position = Bluesky.getPosition(e);
                    var distance = {
                        x: (position.x - this.ePosition.x),
                        y: (position.y - this.ePosition.y)
                    };
                    var eventArgs = {
                        e: e,
                        endSize: this.endSize,
                        endPosition: this.endPosition
                    };
                    if (this.endSize.width != this.startSize.width) {
                        this.target.css("width", this.endSize.width + "px");
                    }
                    if (this.endSize.height != this.startSize.height) {
                        this.target.css("height", this.endSize.height + "px");
                    }
                    if (this.endPosition.x != this.startPosition.x) {
                        this.target.css("left", this.endPosition.x + "px");
                    }
                    if (this.endPosition.y != this.startPosition.y) {
                        this.target.css("top", this.endPosition.y + "px");
                    }
                    this.isResizing = false;
                    if (this.onResizeEnd) {
                        this.onResizeEnd.call(this.context, eventArgs);
                    }
                }
            },
            toWest: function(e) {
                var position = Bluesky.getPosition(e);
                this.endSize.width = this.startSize.width + (position.x - this.ePosition.x) * (-1);
                this.endPosition.x = this.startPosition.x + (position.x - this.ePosition.x);
                this.movelayer.css({ width: this.endSize.width + "px" });
                this.movelayer.css({ left: this.endPosition.x + "px" });
            },
            toNorth: function(e) {
                var position = Bluesky.getPosition(e);
                this.endSize.height = this.startSize.height + (position.y - this.ePosition.y) * (-1);
                this.endPosition.y = this.startPosition.y + (position.y - this.ePosition.y);
                this.movelayer.css({ height: this.endSize.height + "px" });
                this.movelayer.css({ top: this.endPosition.y + "px" });
            },
            toEast: function(e) {
                var position = Bluesky.getPosition(e);
                this.endSize.width = this.startSize.width + (position.x - this.ePosition.x);
                this.movelayer.css({ width: this.endSize.width + "px" });
            },
            toSourth: function(e) {
                var position = Bluesky.getPosition(e);
                this.endSize.height = this.startSize.height + (position.y - this.ePosition.y);
                this.movelayer.css({ height: this.endSize.height + "px" });
            },
            toNorthWest: function(e) {
                this.toNorth(e);
                this.toWest(e);
            },
            toNorthEast: function(e) {
                this.toNorth(e);
                this.toEast(e);
            },
            toSourthEast: function(e) {
                this.toSourth(e);
                this.toEast(e);
            },
            toSourthWest: function(e) {
                this.toSourth(e);
                this.toWest(e);
            }
        });
    }
})(Bluesky);