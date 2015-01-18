/**********************************************
* BlueSky框架综合类库 如数据转换，表单验证等
* copyright huyihuan 2013
* date: 2013-11-12
**********************************************/
(function(_util) {
    _util.mousePosition = function(_targetWindow, e) {
        var xPos, yPos;
        e = e || _targetWindow.event;
        if (e.pageX) {
            xPos = e.pageX;
            yPos = e.pageY;
        } else {
            xPos = e.clientX + _targetWindow.document.body.scrollLeft - _targetWindow.document.body.clientLeft;
            yPos = e.clientY + _targetWindow.document.body.scrollTop - _targetWindow.document.body.clientTop;
        }
        return { X: xPos, Y: yPos };
    }
    _util.documentSize = function(_documentRef) {
        var size = new Object;
        size.width = document.body.clientWidth;
        size.height = document.body.clientHeight;
        return size;
    }
    _util.parseInt = function(_strValue, _defaultValue) {
        var parseValue = parseInt(_strValue);
        if (isNaN(parseValue) || parseValue == undefined || parseValue == null)
            return _defaultValue;
        return parseValue;
    }
    _util.parseDouble = function(_strValue, _defaultValue) {
        var parseValue = parseFloat(_strValue);
        if (isNaN(parseValue) || parseValue == undefined || parseValue == null)
            return _defaultValue;
        return parseValue;
    }

    /*表单验证*/
    _util.vType = {
        Empty: "Empty",
        Email: "Email",
        Post: "Post",
        IDCard: "IDCard",
        Chinese: "Chinese",
        English: "English",
        Integer: "Integer"
    }
    _util.vText = function(_vArguments) {
        _vArguments.vtarget = _vArguments.vtarget || window;
        var txtReference = _vArguments.vtarget.document.getElementById(_vArguments.vid);
        var strValue = txtReference.value;
        if (_vArguments.vtype == _util.vType.Empty) {
            if ("" == strValue || !strValue) {
                txtReference.focus();
                if (_vArguments.ishint == true) {
                    txtReference.className = txtReference.className + " txt-hint";
                }
                if (_vArguments.message) {
                    top.Bluesky.MessageBox.alert({
                        message: _vArguments.message,
                        callback: function() {
                            txtReference.focus();
                        }
                    });
                }
                return false;
            }
            if (_vArguments.ishint == true) {
                txtReference.className = txtReference.className.replace(" txt-hint", "");
            }
            return true;
        }
        else if (_vArguments.vtype == _util.vType.Integer) {
            var reg = /^\d*$/;
            if ("" == strValue || !strValue || null == strValue.match(reg)) {
                txtReference.focus();
                if (_vArguments.ishint == true) {
                    txtReference.className = txtReference.className + " txt-hint";
                }
                if (_vArguments.message) {
                    top.Bluesky.MessageBox.alert({
                        message: _vArguments.message,
                        callback: function() {
                            txtReference.focus();
                        }
                    });
                }
                return false;
            }
            txtReference.className = txtReference.className.replace(" txt-hint", "");
            return true;
        }
        //如果未说明验证类型则返回false
        return false;
    }

    _util.xmlHttpRequest = function() {
        if (XMLHttpRequest)
            return new XMLHttpRequest();
        var arrXHRS = ["MSXML2.XMLHTTP.6.0", "MSXML2.XMLHTTP.3.0", "MSXML2.XMLHTTP", "Microsoft.XMLHTTP"];
        var nXHRCount = arrXHRS.length;
        var xhrObject = new Object();
        for (var i = 0; i < nXHRCount; i++) {
            try {
                xhrObject = new ActiveXObject(arrXHRS[i]);
                break;
            }
            catch (e) { }
        }
        if (typeof xhrObject != undefined)
            return xhrObject;
        top.Bluesky.MessageBox.alert("Ajax not supported!");
    }

    _util.eventPrevent = function() {
        if (e && e.preventDefault)
            e.preventDefault();

        if (e && e.stopPropagation)
            e.stopPropagation();
        else
            window.event.cancelBubble = true;
    }
})(window.Utils = window.Utils || {});