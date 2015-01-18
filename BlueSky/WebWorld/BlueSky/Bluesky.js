/**********************************************
* BlueSky核心库
* copyright huyihuan 2013
* date: 2013-11-12
**********************************************/
(function() {
    //Array类原型扩展
    if (typeof Array.prototype.indexOf !== "function") {
        Array.prototype.indexOf = function(_item) {
            var length = this.length;
            for (var i = 0; i < length; i++) {
                if (this[i] == _item) {
                    return i;
                }
            }
            return -1;
        }
    }

    if (typeof Array.prototype.indexOfProperty !== "function") {
        Array.prototype.indexOfProperty = function(_item, _attrName) {
            var length = this.length;
            var bSame = false;
            for (var i = 0; i < length; i++) {
                bSame = _attrName ? (this[i][_attrName] == _item) : (this[i] == _item);
                if (bSame == true) {
                    return i;
                }
            }
            return -1;
        }
    }

    if (typeof Array.prototype.removeAt !== "function") {
        Array.prototype.removeAt = function(_index) {
            this.splice(_index, 1);
        }
    }

    if (typeof Array.prototype.remove !== "function") {
        Array.prototype.remove = function(_item) {
            for (var i = 0; i < this.length; i++) {
                if (this[i] === _item) {
                    this.removeAt(i);
                    i--;
                }
            }
        }
    }

    //String类型原型扩展
    if (typeof String.prototype.trim !== "function") {
        return this.toString().replace(" ", "");
    }

})();

(function() {
    //暂存常用的函数
    var hasOwn = Object.prototype.hasOwnProperty,
        toString = Object.prototype.toString;

    //Bluesky框架对象
    var BlueSky = function(_els) {
        this.length = _els && _els.length ? _els.length : 0;
        for (var i = 0; i < this.length; i++) {
            this[i] = _els[i];
        }
    }

    BlueSky.instance = BlueSky.prototype;
    BlueSky.extend = BlueSky.instance.extend = function() {
        var options, key, src, copy, copyIsArray, clone,
		    target = arguments[0],
		    i = 1,
		    length = arguments.length,
		    deep = false;

        if (undefined == target)
            target = {};

        if (typeof target === "boolean") {
            deep = target;
            target = arguments[1] || {};
            i = 2;
        }

        if (typeof target !== "object" && typeof target !== "function") {
            target = {};
        }

        if (i === arguments.length) {
            target = this;
            i--;
        }

        for (; i < arguments.length; i++) {
            if ((options = arguments[i]) != null) {
                for (key in options) {
                    copy = options[key];
                    if (deep && copy && BlueSky.util.isPureObject(copy)) {
                        src = target[key]; //如果对象不包含此属性Key，原型中包含，则此扩展将修改实例的原型结构影响所有的实例
                        src = src && BlueSky.util.isPureObject(src) ? src : {};

                        target[key] = BlueSky.extend(deep, src, copy);
                    }
                    else if (copy !== undefined) {
                        target[key] = copy;
                    }
                }
            }
        }
        return target;
    };

    BlueSky.extend({
        String: {
            trim: function() {
                if (arguments[0]) {
                    return arguments[0].replace(" ", "");
                }
            },
            toUpwer: function() {

            }
        }
    });

    BlueSky.extend({
        Bluesky: "Bluesky1.0",
        browser: {
            isIE: document.attachEvent ? true : false,
            isW3C: document.addEventListener ? true : false
        },

        //Bluesky框架选择器，需要不断完善
        G: function(selector) {
            var els;
            if (typeof selector === "string") {
                if (document.querySelectorAll) {
                    els = document.querySelectorAll(selector);
                }
                else {
                    //IE7一下浏览器支持
                    if (selector.indexOf("#") == 0) {
                        els = document.getElementById(selector);
                    }
                    else if (selector.indexOf(".") == 0) {
                        var a = document.all, len = a.length, c, ct = selector.replace(".", "");
                        for (var i = 0; i < len; i++) {
                            if (a[i].className.indexOf(ct) >= 0) {
                                //todo...
                            }
                        }
                    }
                }
            } else if (Bluesky.isArray(selector)) {
                els = selector;
            } else if (typeof selector.Bluesky === "string") {
                return selector;
            }
            else {
                els = [selector];
            }
            return (new BlueSky(els));
        },
        create: function(_tagName, _attrArgs) {
            var elm = this.G([document.createElement(_tagName)]);
            if (_attrArgs) {
                if (_attrArgs.className) {
                    elm.addClass(_attrArgs.className);
                    delete _attrArgs.className;
                }
                if (_attrArgs.text) {
                    elm.text(_attrArgs.text);
                    delete _attrArgs.text;
                }
                if (_attrArgs.html) {
                    elm.html(_attrArgs.html);
                    delete _attrArgs.html;
                }
                for (var attrName in _attrArgs) {
                    if (_attrArgs.hasOwnProperty(attrName)) {
                        elm.attr(attrName, _attrArgs[attrName]);
                    }
                }
            }
            return elm;
        },

        foreach: function(_object, _callback, _args) {
            var i = 0, key,
                length = _object.length,
                isObj = length === undefined;
            if (_args) {
                if (isObj) {
                    for (key in _object) {
                        if (_callback.apply(_object[key], args) === false) {
                            break;
                        }
                    }
                }
                else {
                    for (; i < length; i++) {
                        if (_callback.apply(_object[i], _args) === false) {
                            break;
                        }
                    }
                }
            }
            else {
                if (isObj) {
                    for (key in _object) {
                        if (_callback.call(_object[key], _object[key], key) === false) {
                            break;
                        }
                    }
                }
                else {
                    for (; i < length; i++) {
                        if (_callback.call(_object[i], _object[i], i) === false) {
                            break;
                        }
                    }
                }
            }
            return _object;
        }
    });

    //常用工具模块
    BlueSky.extend({
        class2type: (function() {
            var class2type = {};
            BlueSky.foreach("Boolean Number String Function Array Date RegExp Object".split(" "), function(_className) {
                class2type["[object " + _className + "]"] = _className.toLowerCase();
            });
            return class2type;
        })(),
        isArray: function() {
            return toString.call(arguments[0]) === "[object Array]";
        },
        util: {
            parseInt: function(_strValue, _defaultValue) {
                var parseValue = parseInt(_strValue);
                if (isNaN(parseValue) || parseValue == undefined || parseValue == null)
                    return _defaultValue;
                return parseValue;
            },

            parseDouble: function(_strValue, _defaultValue) {
                var parseValue = parseFloat(_strValue);
                if (isNaN(parseValue) || parseValue == undefined || parseValue == null)
                    return _defaultValue;
                return parseValue;
            },

            type: function(_object) {
                return null == _object ? "" : (BlueSky.class2type[toString.call(_object)] || "object");
            },

            isPureObject: function(_object) {
                if (!_object || BlueSky.util.type(_object) !== "object" || _object.nodeType) {
                    return false;
                }

                try {
                    if (_object.constructor &&
						!hasOwn.call(_object, "constructor") &&
						!hasOwn.call(_object.constructor.prototype, "isPrototypeOf")) {
                        return false;
                    }
                } catch (e) {
                    return false;
                }

                var key;
                for (key in _object) { }

                return key === undefined || hasOwn.call(_object, key);
            },

            isFunction: function(_object) {
                return typeof _object === "function";
            }
        },
        getEventArg: function(e) {
            return e ? e : window.event;
        },
        getPosition: function(e) {
            e = Bluesky.getEventArg(e);
            if (e.PageX || e.PageY) {
                return { x: e.PageX, y: e.PageY };
            }
            else {
                return {
                    x: e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft,
                    y: e.clientY + document.body.scrollTop + document.documentElement.scrollTop
                };
            }
        },
        stopPropagation: function(e) {
            e = Bluesky.getEventArg(e);
            if (e && e.stopPropagation) {
                //因此它支持W3C的stopPropagation()方法　　
                e.stopPropagation();
            }
            else {
                e.cancelBubble = true;
            }

        },
        stopDefault: function(e) {
            e = Bluesky.getEventArg(e);
            if (e && e.preventDefault) {
                e.preventDefault();
            }
            else {
                e.returnValue = false;
            }
        },
        parseJson: function(toJson) {
            if (JSON) {
                return JSON.parse(toJson);
            }
            else {
                return eval("(" + toJson + ")");
            }
        }
    });

    BlueSky.instance.extend({
        Bluesky: "Bluesky1.0",
        count: function() {
            return this.length;
        },
        element: function(index) {
            if (undefined == index)
                return this[0];
            return this[index];
        },
        first: function() {
            return BlueSky.G(this.element());
        },
        last: function() {
            return BlueSky.G(this.element(this.length - 1));
        },
        eIndex: function(index) {
            return BlueSky.G(this.element(index));
        },
        //循环批量执行函数，并返回函数的执行结果数组
        action: function(_callback) {
            var results = [], length = this.length;
            for (var i = 0; i < length; i++) {
                results.push(_callback.call(this, this[i], i));
            }
            return results;
        },

        actionOne: function(_callback) {
            var results = this.action(_callback);
            return results.length > 1 ? results : results[0];
        },

        foreach: function(_callback) {
            this.action(_callback);
            return this;
        },

        attr: function(_attrName, _attrValue) {
            if (typeof _attrValue !== "undefined") {
                return this.foreach(function(_el) {
                    _el.setAttribute(_attrName, _attrValue);
                });
            }
            else {
                return this.actionOne(function(_el) {
                    return _el.getAttribute(_attrName);
                });
            }
        },
        removeAttr: function(_attrName) {
            if (_attrName && typeof _attrName == "string") {
                return this.foreach(function(_el) {
                    _el.removeAttribute(_attrName);
                });
            }
            return this;
        },
        text: function(_text) {
            if (typeof _text !== "undefined") {
                return this.foreach(function(_el) {
                    _el.innerText = _text;
                });
            }
            else {
                return this.actionOne(function(_el) {
                    return _el.innerText;
                });
            }
        },

        html: function(_html) {
            if (typeof _html !== "undefined") {
                return this.foreach(function(_el) {
                    _el.innerHTML = _html;
                });
            }
            else {
                return this.actionOne(function(_el) {
                    return _el.innerHTML;
                });
            }
        },

        value: function(_value) {
            if (typeof _value !== "undefined") {
                return this.foreach(function(_el) {
                    _el.value = _value;
                });
            }
            else {
                return this.actionOne(function(_el) {
                    return _el.value;
                });
            }
        },

        checked: function(_value) {
            if (typeof _value !== "undefined") {
                return this.foreach(function(_el) {
                    _el.checked = _value;
                });
            }
            else {
                return this.actionOne(function(_el) {
                    return _el.checked;
                });
            }
        },

        disabled: function(_value) {
            if (typeof _value !== "undefined") {
                return this.foreach(function(_el) {
                    _el.disabled = _value;
                });
            }
            else {
                return this.actionOne(function(_el) {
                    return _el.disabled;
                });
            }
        },

        focus: function() {
            return this.foreach(function(_el) {
                return _el.focus();
            });
        },

        select: function() {
            return this.foreach(function(_el) {
                return _el.select();
            });
        }
    });
    //数据缓存
    var uniqueCode = BlueSky.Bluesky + Math.random().toString();
    BlueSky.extend({
        data: {},
        cache: function(_obj, _key, _value) {
            if (null == _obj || undefined === _obj)
                return;
            _key = _key.toString();
            if (_value === undefined) {
                if (_obj.nodeType) {
                    var GUID = _obj.getAttribute(uniqueCode);
                    if (undefined === GUID || null === GUID) {
                        return;
                    }
                    return this.data[GUID][_key];
                } else {
                    if (undefined === _obj[uniqueCode]) {
                        return;
                    }
                    return _obj[uniqueCode][_key];
                }
            }
            else {
                if (_obj.nodeType) {
                    var GUID = _obj.getAttribute(uniqueCode);
                    if (undefined === GUID || null === GUID) {
                        GUID = Math.random().toString();
                        _obj.setAttribute(uniqueCode, GUID);
                        this.data[GUID] = {};
                    }
                    this.data[GUID][_key] = _value;
                }
                else {
                    if (_obj[uniqueCode] === undefined) {
                        _obj[uniqueCode] = {};
                    }
                    _obj[uniqueCode][_key] = _value;
                }
            }
        }
    });

    BlueSky.instance.extend({
        cache: function(_key, _value) {
            if (undefined === _value) {
                return this.foreach(function(_elm) {
                    BlueSky.cache(_elm, _key, _value);
                });
            }
            else {
                return this.actionOne(function(_elm) {
                    BlueSky.cache(_elm, _key);
                });
            }
        }
    });

    //样式操作模块
    BlueSky.instance.extend({
        hasClass: function(_className) {
            return this.attr("class").split(" ").indexOf(_className) != -1;
        },

        addClass: function(_classNames) {
            if (typeof _classNames === "string") {
                _classNames = _classNames;
            }
            else if (Bluesky.isArray(_classNames)) {
                _classNames = _classNames.join(" ");
            }
            return this.foreach(function(_el) {
                if ("" == _el.className)
                    _el.className = _classNames;
                else
                    _el.className += " " + _classNames;
            });
        },

        removeClass: function(_classNames) {
            if ("" === _classNames)
                return;
            var length = Bluesky.isArray(_classNames) ? _classNames.length : 1;
            if (typeof _classNames === "string") {
                _classNames = [_classNames];
            }
            return this.foreach(function(_el) {
                var aClassName = _el.className.split(" ");
                for (var i = 0; i < length; i++) {
                    aClassName.remove(_classNames[i]);
                }
                _el.className = aClassName.join(" ");
            });
        },

        replaceClass: function(_sourceClassName, _targetClassName) {
            return this.foreach(function(_el) {
                var aClassName = _el.className.split(" ");
                for (var i = 0; i < aClassName.length; i++) {
                    if (aClassName[i] === _sourceClassName) {
                        aClassName[i] = _targetClassName;
                    }
                }
                _el.className = aClassName.join(" ");
            });
        },

        toggleClass: function(_className, _flag) {
            if (_flag == true || _flag == false) {
                return _flag ? this.addClass(_className) : this.removeClass(_className);
            }
            return this.foreach(function(_el) {
                _el.className.split(" ").indexOf(_className) != -1 ? BlueSky.G(_el).removeClass(_className) : BlueSky.G(_el).addClass(_className);
            });
        },

        css: function(_cssArgs, _cssValue) {
            var aComputedStyle = ["width", "height"];
            if (typeof _cssValue !== "undefined") {
                return this.foreach(function(_el) {
                    if (aComputedStyle.indexOf(_cssArgs) != -1 && _cssValue.toString().indexOf("px") == -1 && _cssValue.toString().indexOf("%") == -1) {
                        _cssValue += "px";
                    }
                    _el["style"][_cssArgs] = _cssValue;
                });
            }
            else {
                if (typeof _cssArgs === "string") {
                    return this.actionOne(function(_el) {
                        var styleValue;
                        if (_el.currentStyle) {
                            styleValue = _el.currentStyle[_cssArgs];
                        }
                        else if (window.getComputedStyle) {
                            styleValue = _el.ownerDocument.defaultView.getComputedStyle(_el, null)[_cssArgs];
                        }
                        else {
                            styleValue = _el["style"][_cssArgs];
                        }
                        if (aComputedStyle.indexOf(_cssArgs) != -1 && typeof styleValue !== "undefined") {
                            styleValue = styleValue.replace("px", "");
                            var isPersent = styleValue.indexOf("%") !== -1;
                            if (isPersent || styleValue === "auto") {
                                styleValue = _el["offset" + (_cssArgs === "width" ? "Width" : "Height")];
                            }
                            else if (!isPersent && styleValue === "0") {
                                styleValue = _el["offset" + (_cssArgs === "width" ? "Width" : "Height")];
                                styleValue = styleValue != 0 ? styleValue : 0;
                            }
                            else if (!isPersent && styleValue !== "auto") {
                                styleValue = BlueSky.util.parseInt(styleValue, 0);
                            }
                        }
                        return styleValue;
                    });
                }
                else {
                    return this.foreach(function(_el) {
                        for (var cssName in _cssArgs) {
                            var cssValue = _cssArgs[cssName];
                            if (aComputedStyle.indexOf(cssName) != -1 && cssValue.toString().indexOf("px") == -1) {
                                cssValue += "px";
                            }
                            _el["style"][cssName] = cssValue;
                        }
                    });
                }
            }
        },

        width: function(_value) {
            return this.css("width", _value);
        },

        height: function(_value) {
            return this.css("height", _value);
        },
        position: function() {
            return this.actionOne(function(_el) {
                return {
                    x: _el.offsetParent ? (_el.offsetLeft + BlueSky.G(_el.offsetParent).position().x) : _el.offsetLeft,
                    y: _el.offsetParent ? (_el.offsetTop + BlueSky.G(_el.offsetParent).position().y) : _el.offsetTop
                };
            });
        },
        isHidden: function() {
            return this.css("display") == "none";
        }
    });


    //DOM操作模块
    BlueSky.instance.extend({
        remove: function() {
            return this.foreach(function(_el) {
                _el.parentNode.removeChild(_el);
            });
        },

        append: function(_els) {
            return this.foreach(function(_parent, _index) {
                _els.foreach(function(_child) {
                    if (_index > 0) {
                        _child = _child.cloneNode(true);
                    }
                    _parent.appendChild(_child);
                });
            });
        },

        prepend: function(_els) {
            return this.foreach(function(_parent, _index) {
                var length = _els.length;
                var bFirst = _index == 0;
                for (var i = length - 1; i >= 0; i--) {
                    _parent.insertBefore(bFirst ? _els[i] : _els[i].cloneNode(true), _parent.firstChild);
                }
            });
        },

        parent: function() {
            return this.actionOne(function(_el) {
                return Bluesky(_el.parentNode);
            });
        },

        children: function() {
            return this.actionOne(function(_el) {
                return Bluesky(_el.children);
            });
        }
    });

    //事件绑定模块
    BlueSky.instance.extend({
        addEvent: (function() {
            if (BlueSky.browser.isIE) {
                return function(_eventName, _fn) {
                    return this.foreach(function(_el) {
                        _el.attachEvent("on" + _eventName, _fn);
                    });
                };
            }
            else if (BlueSky.browser.isW3C) {
                return function(_eventName, _fn) {
                    return this.foreach(function(_el) {
                        _el.addEventListener(_eventName, _fn, false);
                    });
                };
            }
            else {
                return function(_eventName, _fn) {
                    return this.foreach(function(_el) {
                        _el["on" + _eventName] = _fn;
                    });
                };
            }
        })(),

        removeEvent: (function() {
            if (BlueSky.browser.isIE) {
                return function(_eventName, _fn) {
                    return this.foreach(function(_el) {
                        _el.detachEvent("on" + _eventName, _fn);
                    });
                };
            }
            else if (BlueSky.browser.isW3C) {
                return function(_eventName, _fn) {
                    return this.foreach(function(_el) {
                        _el.removeEventListener(_eventName, _fn, false);
                    });
                };
            }
            else {
                return function(_eventName, _fn) {
                    return this.foreach(function(_el) {
                        _el["on" + _eventName] = null;
                    });
                };
            }
        })(),

        onceEvent: function(_eventName, _fn) {
            var closure = this;
            var once = function() { _fn(); closure.removeEvent(_eventName, once); }
            this.addEvent(_eventName, once);
        }
    });

    //domready
    function doScrollCheck() {
        if (BlueSky.isReady) {
            return;
        }
        try {
            document.documentElement.doScroll("left");
        }
        catch (ee) {
            setTimeout(doScrollCheck, 1);
            return;
        }
        BlueSky.isReady = true;
        BlueSky.ready();
    }

    BlueSky.extend({
        isReady: false,
        readyList: [],
        done: false,
        readyBound: false,
        loaded: (function() {
            if (BlueSky.browser.isW3C) {
                return function() {
                    document.removeEventListener("DOMContentLoaded", BlueSky.loaded, false);
                    BlueSky.isReady = true;
                    BlueSky.ready();
                }
            }
            else {
                return function() {
                    if (document.readyState === "complete") {
                        document.detachEvent("onreadystatechange", BlueSky.loaded);
                        BlueSky.isReady = true;
                        BlueSky.ready();
                    }
                }
            }
        })(),
        bindReady: function() {
            if (BlueSky.readyBound) {
                return;
            }
            BlueSky.readyBound = true;
            if (BlueSky.browser.isW3C) {
                document.addEventListener("DOMContentLoaded", BlueSky.loaded, false);
                window.addEventListener("load", BlueSky.ready, false);
            }
            else if (BlueSky.browser.isIE) {
                document.attachEvent("onreadystatechange", BlueSky.loaded);
                window.attachEvent("onload", BlueSky.ready);
                setTimeout(doScrollCheck, 0);
            }
        },

        ready: function(_callback) {
            if (!BlueSky.readyBound) {
                BlueSky.bindReady();
            }
            if (BlueSky.isReady && !BlueSky.done) {
                BlueSky.done = true;
                for (var i = 0; i < BlueSky.readyList.length; i++) {
                    BlueSky.readyList[i].call(document, BlueSky);
                }
            }
            else if (typeof _callback === "function") {
                BlueSky.readyList.push(_callback);
            }
            return this;
        }
    });
    //异步请求模块
    function getXmlHttpRequest() {
        if (typeof XMLHttpRequest != "undefined") {
            return new XMLHttpRequest();
        }
        else {
            var xNames = { "MSXML2.XMLHTTP.6.0": 0, "MSXML2.XMLHTTP.3.0": 0, "MSXML2.XMLHTTP": 0, "Microsoft.XMLHTTP": 0 };
            var request;
            for (var name in xNames) {
                try {
                    request = new ActiveXObject(name);
                    break;
                }
                catch (ee) { }
            }
            if (typeof request != "undefined") {
                return request;
            }
            else {
                alert("Ajax not supported!");
            }
        }
    }
    function getKeyValueEncode() {
        return encodeURIComponent(arguments[0]) + "=" + encodeURIComponent(arguments[1]);
    }
    BlueSky.extend({ Ajax: function(args) {
        var defaultArgs = {
            type: "get",
            url: "",
            async: true,
            data: {},
            success: null,
            fail: null,
            complete: null,
            beforeSend: null,
            onSended: null,
            context: null,
            dataType: "",
            contentType: ""
        }
        args = BlueSky.extend(true, {}, defaultArgs, args);

        var req = getXmlHttpRequest();
        args.context = args.context || req;

        function readyChange() {
            if (req.readyState == 4 && req.status == 200) {
                if (args.success) {
                    var rObject = req.responseText;
                    if (args.dataType === "json") {
                        rObject = { json: Bluesky.parseJson(rObject) };
                    }
                    else {
                        rObject = { text: rObject };
                    }
                    args.success.call(args.context, rObject);
                }
            }
            else if (req.readyState == 4 && req.status != 200) {
                if (args.fail) {
                    //status:301、304、401、403、404、500
                    args.fail.call(args.context, { code: req.status });
                }
            }
            if (req.readyState === 4 && args.complete) {
                args.complete.call(args.context, req);
            }
        }
        for (var key in args.data) {
            if (args.url.indexOf("?") == -1)
                args.url += "?";
            args.url += "&" + getKeyValueEncode(key, args.data[key]);
        }
        req.onreadystatechange = readyChange;
        req.open(args.type, args.url, args.async);
        if (args.beforeSend) {
            if (args.beforeSend.call(args.context, req) === false) {
                return req;
            }
        }
        req.send(args.type.toLowerCase() === "get" ? null : "");
        if (args.onSended) {
            args.onSended.call(args.context, req);
        }
        return req;
    }
    });

    //动画模块
    BlueSky.instance.extend({
        show: function() {
            return this.foreach(function(_el) {
                var cache = BlueSky.cache(_el, "priv_display");
                if (cache === undefined) {
                    cache = "block";
                }
                _el.style.display = cache;
            });
        },

        hide: function() {
            return this.foreach(function(_el) {
                BlueSky.cache(_el, "priv_display", _el.style.display);
                _el.style.display = "none";
            });
        },

        animate: function() {
            var args = arguments[0] || {},
				step = {},
				start = {},
				unit = {},
				next, stepZero, direct,
				ts = typeof arguments[1] === "number" ? arguments[1] : 1000,
				_callback = arguments[2], forwardValue,
				node = this.first();

            for (var name in args) {
                if (typeof args[name] !== "number")
                    args[name] = parseFloat(args[name].toString());
                start[name] = node.css(name).toString();
                unit[name] = start[name].indexOf("px") === -1 ? "" : "px";
                start[name] = parseFloat(start[name].replace("px", ""));
                if (typeof start[name] !== "number" || isNaN(start[name])) {
                    start[name] = 0;
                }
                step[name] = (args[name] - start[name]) / (ts / 10);
            }

            var animation = function() {
                next = false;
                for (var name in args) {
                    start[name] = start[name] + step[name];
                    stepZero = step[name] > 0;
                    direct = start[name] < args[name];
                    if ((stepZero && direct) || (!stepZero && !direct)) {
                        next = true;
                    }
                    forwardValue = ((stepZero && !direct) || (!stepZero && direct)) ? args[name] : start[name];
                    node.css(name, (forwardValue + unit[name]));
                    if (BlueSky.browser.isIE && name === "opacity") {
                        node.css("filter", "alpha(opacity:" + (forwardValue * 100) + ")");
                    }
                }
                if (next)
                    setTimeout(function() { animation(); }, 10);
                if (!next && typeof _callback === "function") {
                    setTimeout(function() { _callback(node, node); }, 0);
                }
            };
            animation();
        }
    });
    //组件库命名空间
    BlueSky.extend({
        component: {
            create: function() {
                var comName = arguments.length >= 1 ? arguments[0] : undefined;
                if (!comName)
                    return;
                var com = new Bluesky.component[comName](arguments.length >= 2 ? arguments[1] : undefined);
                if (com.show) {
                    com.show();
                }
                return com;
            }
        },
        model: {}
    });

    //创建别名
    window.Bluesky = BlueSky.G;
    for (var key in BlueSky) {
        Bluesky[key] = BlueSky[key];
    }

})();