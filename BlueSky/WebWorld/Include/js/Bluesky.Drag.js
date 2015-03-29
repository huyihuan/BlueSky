/*
*
* Bluesky Components Drag Library v1.0
* 
* Copyright 2014, Yihuan Hu
*
*/
(function(Bluesky) {
    if (Bluesky && Bluesky.component) {
            Bluesky.extend(false, Bluesky.component, { Drag: function() {
                return Bluesky.extend(true, {}, this, arguments[0], Bluesky.component.prototype);
            }
        });

        Bluesky.extend(true, Bluesky.component.Drag.prototype, {
            context: null,
            mover: null,
            operater: null,
            parentSize: null,
            startPosition: null,
            ePosition: null,
            masklayer: null,
            movelayer: null,
            isMoving: false,
            init: function() {
                if (null == this.operater) {
                    this.operater = this.mover;
                }
                var closure = this;
                Bluesky(this.operater).addEvent("mousedown", function(e) {
                    var result = closure.dragStart(e);
                    if (result == false) {
                        return false;
                    }
                    document.ondragstart = function() { return false; };
                    document.onselectstart = function() { return false; };
                    document.onselect = function() { document.selection.empty(); };
                    document.onmousemove = function(e) { closure.draging(e) };
                    document.onmouseup = function(e) {
                        closure.dragEnd(e);
                        document.onmousemove = null;
                        document.onmouseup = null;
                        document.ondragstart = null;
                        document.onselectstart = null;
                        document.onselect = null;
                    };
                });
            },
            onDragStart: null,
            onDragEnd: null,
            draging: function(e) {
                if (!this.isMoving) {
                    this.masklayer = Bluesky.create("div", { className: "drag-mask" });
                    this.mover.parent().append(this.masklayer);
                    var size = { width: this.mover.width(), height: this.mover.height() };
                    this.movelayer = Bluesky.create("div", { className: "drag-panel" }).css({ width: size.width + "px", height: size.height + "px", top: this.startPosition.y + "px", left: this.startPosition.x + "px" });
                    this.mover.parent().append(this.movelayer);
                    this.isMoving = true;
                }
                var position = Bluesky.getPosition(e);
                this.movelayer.css({ left: this.startPosition.x + (position.x - this.ePosition.x) + "px", top: this.startPosition.y + (position.y - this.ePosition.y) + "px" });
            },
            dragStart: function(e) {
                if (this.onDragStart) {
                    var result = this.onDragStart.call(this.context, { e: e });
                    if (result == false) {
                        return false;
                    }
                }
                e = Bluesky.getEventArg(e);
                if (e.preventDefault) {
                    e.preventDefault();
                }
                var parent = this.mover.parent();
                this.parentSize = { width: parent.width(), height: parent.height() };
                this.ePosition = Bluesky.getPosition(e);
                this.startPosition = { x: this.mover.css("left"), y: this.mover.css("top") };
                this.startPosition = {
                    x: Bluesky.util.parseInt(this.startPosition.x.replace("px", ""), 0),
                    y: Bluesky.util.parseInt(this.startPosition.y.replace("px", ""), 0)
                };
            },
            dragEnd: function(e) {
                if (this.isMoving) {
                    this.masklayer.remove();
                    this.movelayer.remove();
                    var position = Bluesky.getPosition(e);
                    var distance = {
                        x: (position.x - this.ePosition.x),
                        y: (position.y - this.ePosition.y)
                    };
                    var endPos = {
                        x: this.startPosition.x + distance.x,
                        y: this.startPosition.y + distance.y
                    };
                    var eventArgs = {
                        e: e,
                        distance: distance,
                        endPosition: endPos
                    };
                    this.mover.css("left", endPos.x + "px");
                    this.mover.css("top", endPos.y + "px");
                    this.isMoving = false;
                    if (this.onDragEnd) {
                        this.onDragEnd.apply(this.context, [eventArgs]);
                    }
                }
            }
        });
    }
})(Bluesky);