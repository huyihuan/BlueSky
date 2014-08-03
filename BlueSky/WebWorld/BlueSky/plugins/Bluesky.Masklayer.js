//各个层的垂直高度值z-index
//loadinglayer:9999;
//popWindowSelf-normal:9990;
//popWindowSelf-active:9991;
//TaskBar:9993;
//popWindowParent-normal:9995;
//popWindowParent-active:9996;
//popWindowTop-normal:9997;
//popWindowTop-active:9998;
//var bs_field_loadingLayerId = "loadingLayer";
//var bs_field_loadingLayerClass = "graybox-loading";
//var bs_field_loadingImageUrl = "Include/image/loading.gif";
//var bs_field_loadingImageWidth = 20;
//var bs_field_loadingImageHeight = 20;
//var bs_field_grayBoxClassName = "grayBox";
//var bs_field_grayBoxId = "grayBox";
//var bs_filed_grayBoxArray = new Array();

////show loading layer
//function bs_event_showLoadingLayer(_targetDoc) {
//    if (!_targetDoc || undefined == _targetDoc || null == _targetDoc)
//        return false;
//    var oldLayer = _targetDoc.getElementById(bs_field_loadingLayerId);
//    if (oldLayer)
//        return false;
//    var loadLayer = _targetDoc.createElement("div");
//    loadLayer.id = bs_field_loadingLayerId;
//    loadLayer.className = bs_field_loadingLayerClass;
//    
//    var offsetwidth = _targetDoc.body.offsetWidth;
//    var offsetheight = _targetDoc.body.offsetHeight;
//    var loadImage = _targetDoc.createElement("img");
//    loadImage.setAttribute("src", bs_field_loadingImageUrl);
//    loadImage.style.marginLeft = (offsetwidth - bs_field_loadingImageWidth) / 2 + "px";
//    loadImage.style.marginTop = (offsetheight - bs_field_loadingImageHeight) / 2 + "px";
//    loadLayer.appendChild(loadImage);
//    if (_targetDoc.body.childNodes.length == 0)
//        _targetDoc.body.appendChild(grayBox);
//    else
//        _targetDoc.body.insertBefore(loadLayer, _targetDoc.body.childNodes[0]);
//}
//function bs_event_hideLoadingLayer(_targetDoc) {
//    if (!_targetDoc || undefined == _targetDoc || null == _targetDoc)
//        return false;
//    var loadLayer = _targetDoc.getElementById(bs_field_loadingLayerId);
//    if (loadLayer)
//        _targetDoc.body.removeChild(loadLayer);
//}


//function bs_event_showGrayBox(_targetDoc) {
//    if (!_targetDoc || undefined == _targetDoc || null == _targetDoc)
//        return false;
//    var grayBox = _targetDoc.createElement("div");
//    grayBox.id = "grayBox" + (bs_filed_grayBoxArray.length + 1);
//    grayBox.className = bs_field_grayBoxClassName;
//    bs_filed_grayBoxArray[bs_filed_grayBoxArray.length] = grayBox.id;
//    grayBox.style.zIndex = 9998 + (bs_filed_grayBoxArray.length - 1) * 2;
//    if (_targetDoc.body.childNodes.length == 0)
//        _targetDoc.body.appendChild(grayBox);
//    else
//        _targetDoc.body.insertBefore(grayBox, _targetDoc.body.childNodes[0]);
//}
//function bs_event_hideGrayBox(_targetDoc) {
//    if (!_targetDoc || undefined == _targetDoc || null == _targetDoc)
//        return false;
//    var grayBox = _targetDoc.getElementById(bs_filed_grayBoxArray[bs_filed_grayBoxArray.length - 1]);
//    if (grayBox) {
//        _targetDoc.body.removeChild(grayBox);
//        bs_filed_grayBoxArray.splice(bs_filed_grayBoxArray.length - 1, 1);
//    }
//}
if (Bluesky && Bluesky.component) {
    Bluesky.extend(false, Bluesky.component, { Masklayer: function() {
        var args = arguments[0];
        if (args) {
            for (var name in args) {
                if (this[name] != undefined)
                    this[name] = args[name];
            }
        }
        this.component.aList.push(this);
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
        component: Bluesky.component.Masklayer,
        show: function() {
            if (!this.parent)
                return;
            this.node = Bluesky.create("div", { className: this.className, html: this.html });
            this.node.css("zIndex", 9998 + (this.component.aList.length - 1) * 2);
            if (this.width > 1) {
                this.node.css("width", this.width);
            }
            if (this.height > 1) {
                this.node.css("height", this.height);
            }
            Bluesky(this.parent.body).prepend(this.node);
            this.node.animate({ "opacity": 0.4 }, 100);
            return this;

        },
        remove: function() {
            this.node.remove();
            this.component.aList.pop(this);
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
    

