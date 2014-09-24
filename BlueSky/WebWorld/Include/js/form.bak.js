/*
************************************************************************************
* 包含：action操作的执行、列表的初始化、以及一些列表页通用方法。
* formUtil.initCheckBox(_listArguments)：列表复选框的初始化以及列表事件的初始化。
* formUtil.initList(_listArguments):初始化主区域的高度宽度，并且自动绑定window.onresize事件时的高度自动计算。
* formUtil.refreshWindow():页面刷新。
* formUtil.toggleSearch():页面查询panel的现实与隐藏。
* formUtil.actionDone(_actionArguments):action操作的执行方法。
************************************************************************************
*/
(function(_argument) {
    var _ = _argument;
    _.initCheckBox = function(_listArguments) {
        if (undefined == _listArguments || null == _listArguments)
            return;
        var listElement = $(_listArguments.listElement);
        if (!listElement)
            return;
        var cbSelect = listElement.find("input.cbSelect");
        var cbSelectAll = listElement.find("input.cbSelectAll");
        $(cbSelectAll).bind("click", function() {
            var cbChecked = $(this).attr("checked") == undefined ? false : true;
            $(cbSelect).each(function() {
                if (undefined == $(this).attr("disabled")) {
                    $(this).attr("checked", cbChecked);
                    if (_listArguments.rowHover == true) {
                        var rowSelect = $(this).parent().parent();
                        rowSelect.toggleClass("tr-row-selected", cbChecked);
                        if (_listArguments.rowColor) {
                            if (cbChecked)
                                rowSelect.attr("style", "background-color:" + _listArguments.rowColor);
                            else
                                rowSelect.removeAttr("style");
                        }
                    }
                }
                if (_listArguments.rememberValue == true && cbChecked)
                    _.addSelectedItem($(this).val());
            });
            if (_listArguments.rememberValue == true && !cbChecked)
                _.setSelectedItems("");
        });

        $(cbSelect).bind("click", function(event) {
            if (undefined != $(this).attr("disabled")) {
                event.stopPropagation();
                return;
            }
            var bChecked = $(this).attr("checked") == undefined ? false : true;
            if (_listArguments.rowHover == true) {
                var rowSelect = $(this).parent().parent();
                rowSelect.toggleClass("tr-row-selected", bChecked);
                if (_listArguments.rowColor) {
                    if (bChecked)
                        rowSelect.attr("style", "background-color:" + _listArguments.rowColor);
                    else
                        rowSelect.removeAttr("style");
                }
            }
            if (_listArguments.rememberValue == true) {
                if (bChecked)
                    _.addSelectedItem($(this).val());
                else
                    _.removeSelectedItem($(this).val());
            }
            event.stopPropagation();
        });

        if (_listArguments.rowHover == true && cbSelect.length > 0) {
            $(cbSelect).parent().parent().bind("mouseover mouseout", function() {
                $(this).toggleClass("tr-row-active");
            });
        }

        if (_listArguments.rowClick = true && cbSelect.length > 0) {
            $(cbSelect).parent().parent().bind("click", function(event) {
                var cbSelectSingle = $(this).find("input.cbSelect");
                if (undefined != $(cbSelectSingle).attr("disabled")) {
                    event.stopPropagation();
                    return;
                }
                var bChecked = !($(cbSelectSingle).attr("checked") == undefined ? false : true);
                $(cbSelectSingle).attr("checked", bChecked);
                if (_listArguments.rowHover == true) {
                    $(this).toggleClass("tr-row-selected", bChecked);
                    if (_listArguments.rowColor) {
                        if (bChecked)
                            $(this).attr("style", "background-color:" + _listArguments.rowColor);
                        else
                            $(this).removeAttr("style");
                    }
                }
                if (_listArguments.rememberValue == true) {
                    if (bChecked)
                        _.addSelectedItem($(cbSelectSingle).val());
                    else
                        _.removeSelectedItem($(cbSelectSingle).val());
                }
                event.stopPropagation();
            });
        }

        if (_listArguments.initStatus == true && (_listArguments.rowHover == true || _listArguments.rememberValue == true)) {
            $(cbSelect).each(function() {
                var bChecked = $(this).attr("checked") == undefined ? false : true;
                if (_listArguments.rowHover == true) {
                    var rowSelect = $(this).parent().parent();
                    rowSelect.toggleClass("tr-row-selected", bChecked);
                    if (_listArguments.rowColor) {
                        if (bChecked)
                            rowSelect.attr("style", "background-color:" + _listArguments.rowColor);
                        else
                            rowSelect.removeAttr("style");
                    }
                }
                if (_listArguments.rememberValue == true) {
                    if (bChecked)
                        _.addSelectedItem($(this).val());
                    else
                        _.removeSelectedItem($(this).val());
                }
            });
        }
    }
    _.hiddenSelect = null;
    _.addSelectedItem = function(_item) {
        if (!_item || "" == _item)
            return;
        if (!this.hiddenSelect) {
            this.hiddenSelect = document.getElementById("hiddenSelectedValue");
        }
        var itemExist = this.hiddenSelect.value;
        if ((";" + itemExist + ";").indexOf(";" + _item + ";") == -1) {
            this.hiddenSelect.value = "" == itemExist ? _item : (itemExist + ";" + _item);
        }
    }

    _.removeSelectedItem = function(_item) {
        if (!_item || "" == _item)
            return;
        if (!this.hiddenSelect) {
            this.hiddenSelect = document.getElementById("hiddenSelectedValue");
        }
        if ("" == this.hiddenSelect.value)
            return;
        var itemExist = ";" + this.hiddenSelect.value + ";";
        _item = ";" + _item + ";";
        if (itemExist.indexOf(_item) != -1) {
            itemExist = itemExist.replace(_item, ";");
            var nLength = itemExist.length;
            if (1 == nLength) {
                this.hiddenSelect.value = "";
            }
            else {
                if (';' == itemExist.charAt(0)) {
                    itemExist = itemExist.substring(1);
                    nLength--;
                }
                if (';' == itemExist.charAt(nLength - 1))
                    itemExist = itemExist.substring(0, nLength - 1);
                this.hiddenSelect.value = itemExist;
            }
        }
    }

    _.setSelectedItems = function(_items) {
        if (!this.hiddenSelect) {
            this.hiddenSelect = document.getElementById("hiddenSelectedValue");
        }
        this.hiddenSelect.value = _items;
    }

    _.listObject = null;
    _.initList = function(_listArguments) {
        _.listObject = $(_listArguments.listObject);
        if (!_.listObject)
            return;
        if (undefined != _listArguments.height) {
            _.listObject.height(_listArguments.height);
        }
        else if (undefined != _listArguments.minusHeight && _listArguments.minusHeight >= 0) {
            _.listObject.height($(window).height() - _listArguments.minusHeight);
        }
        if (undefined != _listArguments.width) {
            _.listObject.width(_listArguments.width);
        }
        else if (undefined != _listArguments.minusWidth && _listArguments.minusWidth >= 0) {
            _.listObject.width($(window).width() - _listArguments.minusWidth);
        }

        if (_listArguments.resize == true) {
            $(window).bind("resize", function() {
                if (undefined != _listArguments.minusHeight && _listArguments.minusHeight >= 0) {
                    _.listObject.height($(window).height() - _listArguments.minusHeight);
                }
                if (undefined != _listArguments.minusWidth && _listArguments.minusWidth >= 0) {
                    _.listObject.width($(window).width() - _listArguments.minusWidth);
                }
            });
        }
    }

    _.refreshWindow = function() {
        window.location.href = window.location.href;
    }

    //存在bug需要完善：显示查询panel之后，如果改变浏览器窗口大小，那么列表尺寸就又会发生变化
    _.toggleSearch = function(event) {
        var searchPanel = $(".action-search-panel");
        if (!searchPanel)
            return;
        var bDisplay = $(".action-search-panel").attr("display");
        bDisplay = bDisplay ? bDisplay : "hidden";
        var minusObject = $(searchPanel.attr("minusObject"));
        if (minusObject) {
            var minusHeight = searchPanel.attr("minusHeight");
            minusObject.height(bDisplay == "hidden" ? (minusObject.height() - parseInt(minusHeight)) : (minusObject.height() + parseInt(minusHeight)));
        }
        $(".action-search-panel").toggle();
        $(".action-search-panel").attr("display", bDisplay == "hidden" ? "visible" : "hidden");
        //如果事件源是Action，那么改变Action的状态
        event = event ? event : window.event;
        var _eventSender = document.attachEvent ? event.srcElement : event.target;
        if (_eventSender) {
            var cName = _eventSender.className;
            if (cName.indexOf("action") == -1) {
                _eventSender = _eventSender.parentNode;
                if (!_eventSender)
                    return;
                cName = _eventSender.className;
            }
            if (cName.indexOf("action") != -1) {
                _eventSender.className = bDisplay == "hidden" ? cName.replace("normal", "active") : cName.replace("active", "normal");
            }
        }
    }

    _.actionDone = function(_actionArguments) {
        if (!_actionArguments)
            return;
        if (undefined == _actionArguments.actionType || null == _actionArguments.actionKey) {
            alert("操作无效！");
            return;
        }
        var selectValue = "";
        if (_actionArguments.entityCount != 0) {
            selectValue = $("#hiddenSelectedValue").val() + "";
            var ncount = "" == selectValue ? 0 : selectValue.split(';').length;
            if (_actionArguments.entityCount == -1 && ncount == 0) {
                alert("请选择要操作的记录！");
                return;
            }
            if (_actionArguments.entityCount > 0 && ncount != _actionArguments.entityCount) {
                alert("请选择 " + _actionArguments.entityCount + " 条记录！");
                return;
            }
        }
        //如果是弹出窗体，则存储窗体相关变量
        var oArg = new Object();
        if (_actionArguments.popup == true) {
            oArg.width = _actionArguments.width;
            oArg.height = _actionArguments.height;
            oArg.title = _actionArguments.title;
            oArg.target = "top";
            oArg.resize = _actionArguments.resize;
            oArg.maxbox = _actionArguments.maxbox;
            oArg.minbox = _actionArguments.minbox;
            oArg.move = _actionArguments.move;
            oArg.iconURL = _actionArguments.iconURL;
        }

        if (_actionArguments.actionType == 'normal') {
            var strUrl = "Window.aspx?fn=" + _actionArguments.fn + "&akey=" + _actionArguments.actionKey;
            if (undefined != _actionArguments.addinParameters && "" != _actionArguments.addinParameters)
                strUrl += "&" + _actionArguments.addinParameters;
            if (_actionArguments.entityCount != 0) {
                strUrl += (_actionArguments.entityCount == 1 ? "&id=" : "&ids=") + selectValue;
            }
            var strExtraParaeters = $("#hiddenExtraParameters").val();
            if (undefined != strExtraParaeters && null != strExtraParaeters && "" != strExtraParaeters)
                strUrl += "&" + strExtraParaeters;
            if (_actionArguments.popup == true) {
                oArg.url = strUrl;
                top.windowFactory.topFocusForm(oArg);
            }
            else {
                window.location.href = strUrl;
            }
        }
        else if (_actionArguments.actionType == 'javascript') {
            eval(_actionArguments.actionValue);
        }
        else if (_actionArguments.actionType == 'http') {
            if (_actionArguments.popup == true) {
                oArg.url = _actionArguments.actionValue;
                top.bs_event_createWorkWindow(oArg);
            }
            else {
                window.location.href = _actionArguments.actionValue; ;
            }
        }
        else {
            alert("未识别的action类型！");
            return;
        }
    }
})(window.formUtil = window.formUtil || {});