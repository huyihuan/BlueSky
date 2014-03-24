/**********************************************
* bluesky.js BlueSky核心库
* copyright huyihuan 2013
* date: 2013-11-12
**********************************************/
(function() {
    //系统基类原型扩展
    if (typeof Array.prototype.indexOf !== "function") {
        Array.prototype.indexOf = function(_item) {
            var length = this.length;
            for (var i = 0; i < length; i++) {
                if (this[i] === _item) {
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


    //bluesky框架对象
    function BlueSkyElement(_els) {
        this.length = _els && _els.length ? _els.length : 0;
        for (var i = 0; i < this.length; i++) {
            this[i] = _els[i];
        }
    }

    var root = window.BlueSky = BlueSkyElement;

    //浏览器版本检测
    root.browserInfo = {};
    root.browserInfo.isIE = document.attachEvent ? true : false;
    root.browserInfo.isW3C = document.addEventListener ? true : false;

    //bluesky框架选择器，需要不断完善
    root.get = function(_selector) {
        var els;
        if (typeof _selector === "string") {
            els = document.querySelectorAll(_selector);
        } else if (_selector.length) {
            els = _selector;
        } else {
            els = [_selector];
        }
        return new BlueSkyElement(els);
    };

    root.create = function(_tagName, _attrArgs) {
        var cElement = this.get([document.createElement(_tagName)]);
        if (_attrArgs) {
            if (_attrArgs.className) {
                cElement.addClass(_attrArgs.className);
                delete _attrArgs.className;
            }
            if (_attrArgs.text) {
                cElement.text(_attrArgs.text);
                delete _attrArgs.text;
            }
            if (_attrArgs.html) {
                cElement.html(_attrArgs.html);
                delete _attrArgs.html;
            }
            for (var attrName in _attrArgs) {
                if (_attrArgs.hasOwnProperty(attrName)) {
                    cElement.attr(attrName, _attrArgs[attrName]);
                }
            }
        }
        return cElement;
    }

    root.prototype.first = function() {
        return root.get(this[0]);
    }

    root.prototype.last = function() {
        return root.get(this[this.length - 1]);
    }

    //循环批量执行函数，并返回函数的执行结果数组
    root.prototype.map = function(_callback) {
        var results = [], length = this.length;
        for (var i = 0; i < length; i++) {
            results.push(_callback.call(this, this[i], i));
        }
        return results;
    };

    root.prototype.mapOne = function(_callback) {
        var results = this.map(_callback);
        return results.length > 1 ? results : results[0];
    };

    root.prototype.foreach = function(_callback) {
        this.map(_callback);
        return this;
    };

    root.prototype.attr = function(_attrName, _attrValue) {
        if (typeof _attrValue !== "undefined") {
            return this.foreach(function(_el) {
                _el.setAttribute(_attrName, _attrValue);
            });
        }
        else {
            return this.mapOne(function(_el) {
                return _el.getAttribute(_attrName);
            });
        }
    };

    root.prototype.text = function(_text) {
        if (typeof _text !== "undefined") {
            return this.foreach(function(_el) {
                _el.innerText = _text;
            });
        }
        else {
            return this.mapOne(function(_el) {
                return _el.innerText;
            });
        }
    };

    root.prototype.html = function(_html) {
        if (typeof _html !== "undefined") {
            return this.foreach(function(_el) {
                _el.innerHTML = _html;
            });
        }
        else {
            return this.mapOne(function(_el) {
                return _el.innerHTML;
            });
        }
    };

    root.prototype.value = function(_value) {
        if (typeof _value !== "undefined") {
            return this.foreach(function(_el) {
                _el.value = _value;
            });
        }
        else {
            return this.mapOne(function(_el) {
                return _el.value;
            });
        }
    };

    //样式操作
    root.prototype.hasClass = function(_className) {
        return this.attr("class").split(" ").indexOf(_className) != -1;
    }

    root.prototype.addClass = function(_classNames) {
        if (typeof _classNames === "string") {
            _classNames = " " + _classNames;
        }
        else if (typeof _classNames === "array") {
            _classNames = " " + _classNames.join(" ");
        }
        return this.foreach(function(_el) {
            _el.className += _classNames;
        });
    };

    root.prototype.removeClass = function(_classNames) {
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
    };

    root.prototype.replaceClass = function(_sourceClassName, _targetClassName) {
        return this.foreach(function(_el) {
            var aClassName = _el.className.split(" ");
            for (var i = 0; i < aClassName.length; i++) {
                if (aClassName[i] === _sourceClassName) {
                    aClassName[i] = _targetClassName;
                }
            }
            _el.className = aClassName.join(" ");
        });
    };

    root.prototype.css = function(_cssArgs, _cssValue) {
        var aComputedStyle = ["width", "height"];
        if (typeof _cssValue !== "undefined") {
            return this.foreach(function(_el) {
                if (aComputedStyle.indexOf(_cssArgs) != -1 && _cssValue.toString().indexOf("px") == -1) {
                    _cssValue += "px";
                }
                _el["style"][_cssArgs] = _cssValue;
            });
        }
        else {
            if (typeof _cssArgs === "string") {
                return this.mapOne(function(_el) {
                    var styleVlaue;
                    if (_el.currentStyle) {
                        styleVlaue = _el.currentStyle[_cssArgs];
                    }
                    else if (window.getComputedStyle) {
                        styleVlaue = _el.ownerDocument.defaultView.getComputedStyle(_el, null)[_cssArgs];
                    }
                    else {
                        styleVlaue = _el["style"][_cssArgs];
                    }
                    if (aComputedStyle.indexOf(_cssArgs) != -1)
                        styleVlaue = root.util.parseInt(styleVlaue.replace("px", ""), 0);
                    return styleVlaue;
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
    };

    root.prototype.width = function(_value) {
        return this.css("width", _value);
    };

    root.prototype.height = function(_value) {
        return this.css("height", _value);
    };

    //移除元素
    root.prototype.remove = function() {
        return this.foreach(function(_el) {
            _el.parentNode.removeChild(_el);
        });
    };

    //添加元素
    root.prototype.append = function(_els) {
        return this.foreach(function(_parent, _index) {
            _els.foreach(function(_child) {
                if (_index > 0) {
                    _child = _child.cloneNode(true);
                }
                _parent.appendChild(_child);
            });
        });
    };

    root.prototype.prepend = function(_els) {
        return this.foreach(function(_parent, _index) {
            var length = _els.length;
            var bFirst = _index > 0;
            for (var i = length - 1; i >= 0; i--) {
                _parent.insertBefore(bFirst ? _els[i] : _els[i].cloneNode(true), _parent.firstChid);
            }
        });
    };

    //事件绑定
    root.prototype.addEvent = (function() {
        if (root.browserInfo.isIE) {
            return function(_eventName, _fn) {
                return this.foreach(function(_el) {
                    _el.attachEvent("on" + _eventName, _fn);
                });
            };
        }
        else if (root.browserInfo.isW3C) {
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
    })();

    root.prototype.removeEvent = (function() {
        if (root.browserInfo.isIE) {
            return function(_eventName, _fn) {
                return this.foreach(function(_el) {
                    _el.detachEvent("on" + _eventName, _fn);
                });
            };
        }
        else if (root.browserInfo.isW3C) {
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
    })();


    //domready
    root.isReady = false;
    root.readyList = [];
    var readyBound = false;
    function bindReady() {
        if (readyBound) {
            return;
        }
        readyBound = true;
        if (root.browserInfo.isW3C) {
            document.addEventListener("DOMContentLoaded", function() { root.isReady = true; root.ready(); }, false);
        }
        else if (root.browserInfo.isIE) {
            (function() {
                if (root.isReady)
                    return;
                try {
                    document.documentElement.doScroll("left");
                    root.isReady = true;
                    root.ready();
                }
                catch (ee) {
                    setTimeout(arguments.callee, 0);
                }
            })();
        }
        else {
            root.get(window).addEvent("load", root.ready);
        }
    };

    root.ready = function(_callback) {
        bindReady();
        if (root.isReady) {
            for (var i = 0; i < root.readyList.length; i++) {
                root.readyList[i].call(document, root);
            }
        }
        else {
            root.readyList.push(_callback);
        }
        return this;
    };

    //bluesky框架动画控制
    root.prototype.show = function() {
        return this.foreach(function(_el) {
            _el.style.display = "visble";
        });
    };

    root.prototype.hide = function() {
        return this.foreach(function(_el) {
            _el.style.display = "none";
        });
    };

    //bluesky框架静态方法
    root.util = {
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
        }
    }

    //创建别名
    Bluesky = BlueSky.get;
    Bluesky.create = BlueSky.create;
    Bluesky.ready = BlueSky.ready;
    Bluesky.util = BlueSky.util;
})();