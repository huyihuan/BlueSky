/**********************************************
* Bluesky.js BlueSky核心库
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
    //暂存常用的函数与变量中，提高性能
    var hasOwn = Object.prototype.hasOwnProperty,
        toString = Object.prototype.toString;

    //bluesky框架对象
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
        Bluesky: "Bluesky1.0",
        browser: {
            isIE: document.attachEvent ? true : false,
            isW3C: document.addEventListener ? true : false
        },

        //bluesky框架选择器，需要不断完善
        get: function(_selector) {
            var els;
            if (typeof _selector === "string") {
                els = document.querySelectorAll(_selector);
            } else if (_selector.length) {
                els = _selector;
            } else {
                els = [_selector];
            }
            return new BlueSky(els);
        },
        create: function(_tagName, _attrArgs) {
            var elm = this.get([document.createElement(_tagName)]);
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
        }

    });

    BlueSky.instance.extend({
        Bluesky: "Bluesky1.0",
        first: function() {
            return BlueSky.get(this[0]);
        },

        last: function() {
            return BlueSky.get(this[this.length - 1]);
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
            else if (typeof _classNames === "array") {
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
            var length = typeof _classNames === "array" ? _classNames.length : 1;
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
                            if (!isPersent) {
                                styleValue = BlueSky.util.parseInt(styleValue, 0);
                            }
                            if (styleValue === 0 || isPersent) {
                                styleValue = _el["offset" + (_cssArgs === "width" ? "Width" : "Height")];
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
        })()
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

    //组件库
    BlueSky.extend({ component: {} });

    //创建别名
    window.Bluesky = BlueSky.get;
    for (var key in BlueSky)
        Bluesky[key] = BlueSky[key];

})();