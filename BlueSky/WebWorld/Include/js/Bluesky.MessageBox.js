/*
*
* Bluesky Components MessageBox Library v1.0
* 
* Copyright 2014, Yihuan Hu
*
*/
(function(Bluesky) {
    if (Bluesky && Bluesky.component) {
        Bluesky.extend({ MessageBox: {
            alert: function(_args) {
                var title = _args.title ? _args.title : "系统提示";
                var message = _args.message ? _args.message : (typeof (_args) == "string" ? _args : "");
                //计算内容的高度
                var autoSize = Bluesky.create("div", { html: message }).width(270).css("position", "absolute").css("left", "-10000px").css("top", "-10000px");
                Bluesky(document.body).append(autoSize);
                var theight = autoSize.height() + 25 + 35 + 30;
                autoSize.remove();
                var height = theight <= 150 ? 150 : theight;

                //                var bodySize = Bluesky(document.body);
                //                var mask = Bluesky.component.Masklayer.create({
                //                    width: bodySize.width(),
                //                    height: bodySize.height(),
                //                    onClick: function() {
                //                        var flag = 1, title = alertWindow.nodes.title, wrap = alertWindow.nodes.wrapper, isActive = title.hasClass("window-title-active"),
                //                        tick = function() {
                //                            title.replaceClass("window-title-" + (isActive ? "active" : "normal"), "window-title-" + (isActive ? "normal" : "active"));
                //                            wrap.toggleClass("window-flicker", isActive);
                //                            flag++ <= 5 || clearInterval(fire);
                //                            isActive = !isActive;
                //                        },
                //                        fire = setInterval(tick, 90);
                //                    }
                //                });
                //var alertWindow = Bluesky.component.create("Window", { width: 300, height: height, title: title, renderTo: document.body, icon: { show: false }, zIndex: mask.zIndex + 1 });
                var alertWindow = Bluesky.component.create("Window", { width: 300, height: height, title: title, renderTo: document.body, icon: { show: false }, mask: true, flicker: true });
                //alertWindow.onClosed = function() { mask.remove(); }
                alertWindow.add(Bluesky.create("div", { html: message, className: "messagebox-content" }).width(270).css("margin", "15px 0 0 15px"));
                //添加确定按钮
                var btnOk = Bluesky.create("input", { type: "button", value: "确定", className: "btn-normal btn-messagebox-ok" });
                btnOk.addEvent("click", function() {
                    if (_args.callback) {
                        _args.callback();
                    }
                    alertWindow.close();
                });
                alertWindow.add(Bluesky.create("div", { className: "messagebox-footer" }).append(btnOk));
                btnOk.focus();
            },
            confirm: function(_args) {
                var title = _args.title ? _args.title : "系统提示";
                var message = _args.message ? _args.message : (typeof (_args) == "string" ? _args : "");

                //计算内容的高度
                var autoSize = Bluesky.create("div", { html: message }).width(270).css("position", "absolute").css("left", "-10000px").css("top", "-10000px");
                Bluesky(document.body).append(autoSize);
                var theight = autoSize.height() + 25 + 35 + 30;
                autoSize.remove();
                var height = theight <= 150 ? 150 : theight;

                var bodySize = Bluesky(document.body);
                var mask = Bluesky.component.Masklayer.create({
                    width: bodySize.width(),
                    height: bodySize.height(),
                    zIndex: 9990,
                    onClick: function() {
                        var flag = 1, title = alertWindow.nodes.title, wrap = alertWindow.nodes.wrapper, isActive = title.hasClass("window-title-active"),
                        tick = function() {
                            title.replaceClass("window-title-" + (isActive ? "active" : "normal"), "window-title-" + (isActive ? "normal" : "active"));
                            wrap.toggleClass("window-flicker", isActive);
                            flag++ <= 5 || clearInterval(fire);
                            isActive = !isActive;
                        },
                        fire = setInterval(tick, 90);
                    }
                });
                var alertWindow = Bluesky.component.create("Window", { width: 300, height: height, title: title, renderTo: document.body, icon: { show: false }, zIndex: mask.zIndex + 1 });
                alertWindow.add(Bluesky.create("div", { html: message, className: "messagebox-content" }).width(270).css("margin", "15px 0 0 15px"));
                alertWindow.onClosed = function() { mask.remove(); }
                //添加确定按钮
                var btnOk = Bluesky.create("input", { type: "button", value: "确定", className: "btn-normal btn-messagebox-ok" });
                btnOk.addEvent("click", function() {
                    if (_args.callback) {
                        _args.callback();
                    }
                    alertWindow.close();
                });
                //添加取消按钮
                var btnCancel = Bluesky.create("input", { type: "button", value: "取消", className: "btn-normal btn-messagebox-cancel" });
                btnCancel.addEvent("click", function() {
                    alertWindow.close();
                });
                //添加窗体footer
                var footer = Bluesky.create("div", { className: "messagebox-footer" }).append(btnOk).append(btnCancel);
                alertWindow.add(footer);
                btnOk.focus();
            }
        }
        });
    }
})(Bluesky);