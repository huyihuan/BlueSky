if (Bluesky && Bluesky.component) {
    Bluesky.extend({ MessageBox: {
        alert: function(_args) {
            var title = _args.title ? _args.title : "系统提示";
            var message = _args.message ? _args.message : (typeof (_args) == "string" ? _args : "");

            var autoSize = Bluesky.create("div", { html: message }).width(300).css("position", "absolute").css("left", "-10000px").css("top", "-10000px");
            Bluesky(document.body).append(autoSize);
            var theight = autoSize.height() + 25 + 35 + 20;
            var height = theight <= 150 ? 150 : theight;

            var alertWindow = Bluesky.component.create("Window", { width: 300, height: height, title: title, renderTo: document.body, icon: { show: false} });
            alertWindow.add(Bluesky.create("div", { html: message, className: "messagebox-content" }));

            var btnOk = Bluesky.create("input", { type: "button", value: "确定", className: "btn-normal btn-messagebox-ok" });
            btnOk.addEvent("click", function() {
                if (_args.callback) {
                    _args.callback();
                }
                alertWindow.close();
            });
            alertWindow.add(Bluesky.create("div", { className: "messagebox-footer" }).append(btnOk));
        },
        confirm: function(_args) {
            var title = _args.title ? _args.title : "系统提示";
            var message = _args.message ? _args.message : (typeof (_args) == "string" ? _args : "");
            var alertWindow = Bluesky.component.create("Window", { width: 300, height: 150, title: title, renderTo: document.body, icon: { show: false} });
            alertWindow.add(Bluesky.create("div", { html: message, className: "messagebox-content" }));

            var btnOk = Bluesky.create("input", { type: "button", value: "确定", className: "btn-normal btn-messagebox-ok" });
            btnOk.addEvent("click", function() {
                if (_args.callback) {
                    _args.callback();
                }
                alertWindow.close();
            });

            var btnCancel = Bluesky.create("input", { type: "button", value: "取消", className: "btn-normal btn-messagebox-cancel" });
            btnCancel.addEvent("click", function() {
                alertWindow.close();
            });

            var footer = Bluesky.create("div", { className: "messagebox-footer" }).append(btnOk).append(btnCancel);
            alertWindow.add(footer);
        }
    }
    });
}