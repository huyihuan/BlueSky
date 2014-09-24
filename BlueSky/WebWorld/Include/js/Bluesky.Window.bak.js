//各个层的垂直高度值z-index
//loadinglayer:9999;
//popWindowSelf-normal:9990;
//popWindowSelf-active:9991;
//TaskBar:9993;
//popWindowParent-normal:9995;
//popWindowParent-active:9996;
//popWindowTop-normal:9997;
//popWindowTop-active:9998;
function bs_event_createWorkWindow(_windowArguments) {
    var appWindow = new bs_Window();
    
    //if the same window is opened, active it
    var windowKey = _windowArguments.windowKey;
    if (windowKey != "" && bs_field_ppWindows) {
        for (var windowId in bs_field_ppWindows) {
            var checkWindow = bs_field_ppWindows[windowId];
            if (checkWindow.windowKey == windowKey) {
                if(checkWindow.status == bs_field_Status_Selected)
                    return;
                var clickId = null == bs_field_ppWindowDocSelected ? "" : bs_field_ppWindowDocSelected.id;
                if (null != bs_field_ppWindowDocSelected && clickId != windowId) {
                    var curWindow = bs_field_ppWindows[clickId];
                    curWindow.blur(bs_field_ppWindowDocSelected);

                    if (bs_field_IsShowTaskBar && curWindow.isShowInTaskBar)
                        bs_field_TaskItems[curWindow.taskitemid].blur(null);
                    checkWindow.focus(null);

                    if (bs_field_IsShowTaskBar && checkWindow.isShowInTaskBar)
                        bs_field_TaskItems[checkWindow.taskitemid].focus(null);
                }
                return;
            }
        }
    }
    
    windowFactory.setWindowArguments(_windowArguments, appWindow);
    appWindow.createWindow();
}
//pop windows
var bs_field_ppWindowsIdList = new Array();
var bs_field_ppWindows = new Object();
var bs_field_ppWindowDocSelected = null;
var bs_field_windowStatus = { Max: 3, Normal: 1, Min: 2 };
var bs_field_windowDisplay = { Show: 1, Hide: 2 };
var bs_field_ppWindowTitle_Height = 25;
var bs_field_ppWindow_MinWidth = 150;
var bs_field_ppWindow_MinHeight = 150;
//dblclick
var bs_field_IsDbClick = 0; //双击标识
var bs_field_DbClickTime = 200; //双击时间间隔（微秒）

function bs_Window() {
    this.id = "";
    this.windowKey = "";
    this.taskitemid = "";
    this.width = 300;
    this.height = 200;
    this.sizestatus = bs_field_windowStatus.Normal;
    this.left = 0;
    this.top = 0;
    this.title = "未命名";
    this.titleid = "";
    this.mainid = "";
    this.url = "";
    this.frameid = "";
    this.status = bs_field_Status_Selected;
    this.display = bs_field_windowDisplay.Show;
    this.isResize = false;
    this.isIncludeMin = false;
    this.isIncludeMax = false;
    this.isIncludeClose = true;
    this.isShowInTaskBar = false;
    this.isMove = false;
    this.targetWindow = window;
    this.targetDocument = document;
    this.isHtmlContent = false;
    this.htmlContent = null;
    this.windowClosing = null; //窗体关闭执行函数
    this.zIndex = 0;
    this.windowReturnValue = new Object(); //窗口返回值，用于回调函数返回窗体值
    this.windowArguments = null; //传入窗口参数
    this.windowResult = windowFactory.windowResult.cancel;
    this.iconShow = true;
    this.iconURL = "";
    this.iconDefaultURL = "Include/image/application.png";

}
bs_Window.prototype.createWindow = function() {
    this.targetDocument = this.targetWindow.document;
    var windowCount = bs_field_ppWindowsIdList.length;

    this.id = "ppWindow" + (windowCount + 1);
    this.titleid = this.id + "_title";
    this.mainid = this.id + "_main";
    if (this.isIncludeMin)
        this.isShowInTaskBar = true;
    if (bs_field_IsShowTaskBar && this.isShowInTaskBar)
        this.taskitemid = this.id + "_taskitem";
    if (!this.isHtmlContent)
        this.frameid = this.id + "_frameid";

    var windowTitle = __createDocDiv(this.targetDocument, this.titleid, "window-title", "");
    var appImage = "";
    if (this.iconShow == true) {
        if ("" == this.iconURL)
            this.iconURL = this.iconDefaultURL;
        appImage = "<img class='window-icon' src='" + this.iconURL + "' align='absMiddle' />";
    }
    appImage += this.title;
    var titleText = __createDocDiv(this.targetDocument, "", "window-title-text", "<nobr>" + appImage + "</nobr>");
    windowTitle.appendChild(titleText);
    windowTitle.style.height = __UnitAddPx(bs_field_ppWindowTitle_Height);

    var btnTargetWindow = this.targetWindow;
    if (this.isIncludeClose) {
        var btnClose = new bs_WindowButton(this.id + "_btnclose", this.id, bs_field_buttonType.Close);
        btnClose.targetWindow = btnTargetWindow;
        windowTitle.appendChild(btnClose.createButton());
    }
    if (this.isIncludeMax) {
        var btnMax = new bs_WindowButton(this.id + "_btnmax", this.id, bs_field_buttonType.Max);
        btnMax.targetWindow = btnTargetWindow;
        windowTitle.appendChild(btnMax.createButton());
    }
    if (this.isIncludeMin) {
        var btnMin = new bs_WindowButton(this.id + "_btnmin", this.id, bs_field_buttonType.Min);
        btnMin.targetWindow = btnTargetWindow;
        windowTitle.appendChild(btnMin.createButton());
    }

    var moveDivId = this.id;
    var _isToMove = this.isMove;
    var _isIncludeMax = this.isIncludeMax;
    windowTitle.onmousedown = function() {
        var clickId = null == bs_field_ppWindowDocSelected ? "" : bs_field_ppWindowDocSelected.id;
        if (null != bs_field_ppWindowDocSelected && clickId != moveDivId) {
            var curWindow = bs_field_ppWindows[clickId];
            curWindow.blur(bs_field_ppWindowDocSelected);

            if (bs_field_IsShowTaskBar && curWindow.isShowInTaskBar)
                bs_field_TaskItems[curWindow.taskitemid].blur(null);

            var clickWindow = bs_field_ppWindows[moveDivId];
            clickWindow.focus(null);

            if (bs_field_IsShowTaskBar && clickWindow.isShowInTaskBar)
                bs_field_TaskItems[clickWindow.taskitemid].focus(null);
        }
        if (_isToMove) {
            //解决firefox的双击捕获
            bs_field_IsDbClick = bs_field_IsDbClick + 1;
            setTimeout(function() { bs_field_IsDbClick = 0; }, bs_field_DbClickTime);
            if (bs_field_IsDbClick == 2 && _isIncludeMax)
                this.ondblclick();
            else
                bs_field_ppWindows[moveDivId].move();
        }
    }
    //最大化的窗口，可以添加双击最大化事件
    if (this.isIncludeMax) {
        windowTitle.ondblclick = function() {
            bs_field_IsDbClick = 0;
            var normalId = moveDivId + "_btnnormal";
            var maxId = moveDivId + "_btnmax";
            var ppWindow = bs_field_ppWindows[moveDivId];
            if (null == ppWindow)
                return false;
            if (ppWindow.sizestatus == bs_field_windowStatus.Normal) {
                ppWindow.targetDocument.getElementById(maxId).click();
            }
            else if (ppWindow.sizestatus == bs_field_windowStatus.Max) {
                ppWindow.targetDocument.getElementById(normalId).click();
            }
        }
    }

    var windowMain = __createDocDiv(this.targetDocument, this.mainid, "window-main", "");
    windowMain.style.height = __UnitAddPx(this.height - bs_field_ppWindowTitle_Height - 1);

    var windowFrame = this.targetDocument.createElement("iframe");
    if (!this.isHtmlContent) {
        windowFrame.id = this.frameid;
        windowFrame.frameBorder = "0";
        windowFrame.scrolling = "no";
        windowFrame.src = this.url;
        var _windowArguments = this.windowArguments;
        //iframe.onload事件和window.onload事件执行顺序 window.onload 早于 iframe.onload，所以在调用window.windowArguments时需要setTimeout(loadfn,0);来延迟调用页面参数
        bs_event_AddEvent(windowFrame, "load", function() {
            if (null != windowFrame.window && undefined !== windowFrame.window) {
                windowFrame.window.windowArguments = _windowArguments;
            }
        });
        windowMain.appendChild(windowFrame);
    }

    var bodyWidth = this.targetDocument.body.clientWidth;
    var bodyHeight = this.targetDocument.body.clientHeight;
    if (bodyWidth - this.width > 0) {
        this.left = (bodyWidth - this.width) / 2;
    }
    if (bodyHeight - this.height > 0) {
        this.top = (bodyHeight - this.height) / 2;
    }
    //添加窗口标题和主体
    var popWindow = __createDocDiv(this.targetDocument, this.id, "window-popup window-normal", "");
    __setStyle(popWindow, this.width, this.height, this.left, this.top);
    popWindow.appendChild(windowTitle);
    popWindow.appendChild(windowMain);

    //可以resize的窗口，添加resize边框
    if (this.isResize) {
        var resizeWest = __createDiv(this.id + "west", "window-resize-border window-resize-w", "");
        var resizeEast = __createDiv(this.id + "east", "window-resize-border window-resize-e", "");
        var resizeNorth = __createDiv(this.id + "north", "window-resize-border window-resize-n", "");
        var resizeSourth = __createDiv(this.id + "sourth", "window-resize-border window-resize-s", "");

        resizeWest.onmousedown = function(e) { bs_field_ppWindows[moveDivId].resize(e, bs_field_ResizeType.West); }
        resizeEast.onmousedown = function(e) { bs_field_ppWindows[moveDivId].resize(e, bs_field_ResizeType.East); }
        resizeNorth.onmousedown = function(e) { bs_field_ppWindows[moveDivId].resize(e, bs_field_ResizeType.North); }
        resizeSourth.onmousedown = function(e) { bs_field_ppWindows[moveDivId].resize(e, bs_field_ResizeType.Sourth); }

        var resizeNorthEast = __createDiv(this.id + "_northeast", "window-resize-corner window-resize-ne", "");
        var resizeNorthWest = __createDiv(this.id + "_northwest", "window-resize-corner window-resize-nw", "");
        var resizeSourthEast = __createDiv(this.id + "_sourtheast", "window-resize-corner window-resize-se", "");
        var resizeSourthWest = __createDiv(this.id + "_sourthwest", "window-resize-corner window-resize-sw", "");

        resizeNorthEast.onmousedown = function(e) { bs_field_ppWindows[moveDivId].resize(e, bs_field_ResizeType.NorthEast); }
        resizeNorthWest.onmousedown = function(e) { bs_field_ppWindows[moveDivId].resize(e, bs_field_ResizeType.NorthWest); }
        resizeSourthEast.onmousedown = function(e) { bs_field_ppWindows[moveDivId].resize(e, bs_field_ResizeType.SourthEast); }
        resizeSourthWest.onmousedown = function(e) { bs_field_ppWindows[moveDivId].resize(e, bs_field_ResizeType.SourthWest); }

        popWindow.appendChild(resizeWest);
        popWindow.appendChild(resizeEast);
        popWindow.appendChild(resizeNorth);
        popWindow.appendChild(resizeSourth);
        popWindow.appendChild(resizeNorthEast);
        popWindow.appendChild(resizeNorthWest);
        popWindow.appendChild(resizeSourthEast);
        popWindow.appendChild(resizeSourthWest);
    }
    this.targetDocument.body.appendChild(popWindow);
    if (!this.isHtmlContent) {
        //给pop window's iframe 绑定focus事件
        if (windowFrame.addEventListener)
            windowFrame = windowFrame.contentWindow;
        bs_event_AddEvent(windowFrame, "focus",
                function() {
                    var clickId = "";
                    if (null != bs_field_ppWindowDocSelected) {
                        clickId = bs_field_ppWindowDocSelected.id;
                        if (clickId != moveDivId) {
                            var curWindow = bs_field_ppWindows[clickId];
                            curWindow.blur(bs_field_ppWindowDocSelected);

                            if (bs_field_IsShowTaskBar && curWindow.isShowInTaskBar)
                                bs_field_TaskItems[curWindow.taskitemid].blur(null);
                        }
                    }
                    if (clickId != moveDivId) {
                        var clickWindow = bs_field_ppWindows[moveDivId];
                        clickWindow.focus(null);

                        if (bs_field_IsShowTaskBar && clickWindow.isShowInTaskBar)
                            bs_field_TaskItems[clickWindow.taskitemid].focus(null);
                    }
                }
            );
    }
    if (this.isHtmlContent && null != this.htmlContent) {
        if (typeof this.htmlContent == "string") {
            windowMain.innerHTML = this.htmlContent;
        }
        else if (this.htmlContent.length && this.htmlContent.length != undefined) {
            var oLength = this.htmlContent.length;
            for (var i = 0; i < oLength; i++)
                windowMain.appendChild(this.htmlContent[i]);
        }
        else if (typeof this.htmlContent == "object") {
            windowMain.appendChild(this.htmlContent);
        }
    }
    bs_field_ppWindowsIdList[windowCount] = this.id;
    bs_field_ppWindows[this.id] = this;
    //创建taskitem
    if (bs_field_IsShowTaskBar && this.isShowInTaskBar) {
        var taskItem = new bs_TaskItem(this.taskitemid, this.id, this.title, bs_field_Status_Selected);
        taskItem.createTaskItem();
    }

    //切换当前选中窗体
    if (null != bs_field_ppWindowDocSelected && this.isShowInTaskBar) {
        var curWindow = bs_field_ppWindows[bs_field_ppWindowDocSelected.id];
        curWindow.blur(bs_field_ppWindowDocSelected);

        if (bs_field_IsShowTaskBar && curWindow.isShowInTaskBar)
            bs_field_TaskItems[curWindow.taskitemid].blur();
    }
    var windowDocument = this.focus(popWindow);
    if (this.zIndex > 0) {
        windowDocument.style.zIndex = this.zIndex;
    }
}
//pop window event
bs_Window.prototype.focus = function(_windowDoc) {
    this.status = bs_field_Status_Selected;
    var windowDoc = _windowDoc;
    if (!windowDoc || null == windowDoc)
        windowDoc = this.targetDocument.getElementById(this.id);
    //    if (this.zIndex == 0) {
    //        windowDoc.style.zIndex = "9991";
    //    }
    windowDoc.className = windowDoc.className.replace("active", "normal");
    if (this.display == bs_field_windowDisplay.Hide) {
        windowDoc.style.display = "block";
        this.display = bs_field_windowDisplay.Show;
    }
    var titleRef = this.targetDocument.getElementById(this.titleid);
    if (null != titleRef) {
        titleRef.className = "window-title window-title-hover";
    }
    __setCurSelWindow(windowDoc);
    return windowDoc;
}
bs_Window.prototype.blur = function(_windowDoc) {
    this.status = bs_field_Status_Normal;
    var windowDoc = _windowDoc;
    if (!windowDoc || null == windowDoc)
        windowDoc = this.targetDocument.getElementById(this.id);
    //windowDoc.style.zIndex = "9990";
    windowDoc.className = windowDoc.className.replace("normal", "active");
    __setCurSelWindow(null);
    var titleRef = this.targetDocument.getElementById(this.titleid);
    if (null != titleRef) {
        titleRef.className = "window-title";
    }
}
bs_Window.prototype.show = function(_windowDoc) {
    this.status = bs_field_Status_Selected;
    this.display = bs_field_windowDisplay.Show;
    var windowDoc = _windowDoc;
    if (!windowDoc || null == windowDoc)
        windowDoc =this.targetDocument.getElementById(this.id);
    windowDoc.style.display = "block";
    //windowDoc.style.zIndex = "9991";
    __setCurSelWindow(windowDoc);
}
bs_Window.prototype.hide = function(_windowDoc) {
    this.status = bs_field_Status_Normal;
    this.display = bs_field_windowDisplay.Hide;
    var windowDoc = _windowDoc;
    if (!windowDoc || null == windowDoc)
        windowDoc =this.targetDocument.getElementById(this.id);
    windowDoc.style.display = "none";
    __setCurSelWindow(null);
}

bs_Window.prototype.del = function(_windowDoc) {
    var windowDoc = _windowDoc;
    if (!windowDoc || null == windowDoc)
        windowDoc = this.targetDocument.getElementById(this.id);
    windowDoc.parentNode.removeChild(windowDoc);
    delete bs_field_ppWindows[this.id];
    var windowCount = bs_field_ppWindowsIdList.length;
    for (var n = 0; n < windowCount; n++) {
        if (bs_field_ppWindowsIdList[n].toString() == this.id) {
            bs_field_ppWindowsIdList.splice(n, 1);
            break;
        }
    }
    __setCurSelWindow(null);
}

bs_Window.prototype.move = function() {
    dragF.targetWindow = this.targetWindow;
    dragF.drag(this.id);
}

bs_Window.prototype.refresh = function() {
    var allFrames = this.targetDocument.getElementById(this.mainid).getElementsByTagName("iframe");
    if (!allFrames || null == allFrames || undefined == allFrames || allFrames.length == 0)
        return;
    var tFrame = allFrames[0];
    if (null != tFrame && undefined != tFrame && this.isHtmlContent == false) {
        setTimeout(function() { tFrame.src = this.url; }, 1);
    }
}

//拖拽的方向
var bs_field_ResizeType = { North: 1, Sourth: 2, West: 3, East: 4, NorthWest: 5, NorthEast: 6, SourthEast: 7, SourthWest: 8 };
var bs_field_ResizeForwards = {
    1: "n-resize",
    2: "s-resize",
    3: "w-resize",
    4: "e-resize",
    5: "nw-resize",
    6: "ne-resize",
    7: "se-resize",
    8: "sw-resize"
}

bs_Window.prototype.resize = function(e, _resizeType) {
    if (this.sizestatus != bs_field_windowStatus.Normal)
        return false;

    var e = e ? e : this.targetWindow.event;
    //弹出移动虚线框
    var movePanel = __createDocDiv(this.targetDocument, "windowMovePanel", "window-move-panel", "");
    var mBorder = __createDocDiv(this.targetDocument, "windowMoveBorder", "window-move-border", "");
    mBorder.style.width = __UnitAddPx(this.width + 2);
    mBorder.style.height = __UnitAddPx(this.height + 2);
    mBorder.style.left = __UnitAddPx(this.left - 1);
    mBorder.style.top = __UnitAddPx(this.top - 1);
    mBorder.style.cursor = bs_field_ResizeForwards[_resizeType];
    this.targetDocument.body.appendChild(movePanel);
    this.targetDocument.body.appendChild(mBorder);

    //记录异动前的坐标位置
    var tempX = e.clientX;
    var tempY = e.clientY;
    var bodyWidth = this.targetDocument.body.clientWidth;
    var bodyHeight = this.targetDocument.body.clientHeight;
    var windowDoc = this.targetDocument.getElementById(this.id);
    var mFrame = windowDoc.getElementsByTagName("iframe")[0];

    this.targetDocument.ondragstart = "return false;";
    this.targetDocument.onselectstart = "return false;";
    this.targetDocument.onselect = "document.selection.empty();";

    var ppWindow = bs_field_ppWindows[this.id];
    this.targetDocument.onmousemove = function(e) {
        var e = e ? e : ppWindow.targetWindow.event;
        var moveX = e.clientX - tempX;
        var moveY = e.clientY - tempY;

        if (_resizeType == bs_field_ResizeType.East || _resizeType == bs_field_ResizeType.SourthEast || _resizeType == bs_field_ResizeType.NorthEast) {
            var w = ppWindow.width + 2 + moveX;
            if (w > bs_field_ppWindow_MinWidth) {
                mBorder.style.width = __UnitAddPx(w);
            }
        }
        if (_resizeType == bs_field_ResizeType.Sourth || _resizeType == bs_field_ResizeType.SourthEast || _resizeType == bs_field_ResizeType.SourthWest) {
            var h = ppWindow.height + 2 + moveY;
            if (h > bs_field_ppWindow_MinHeight) {
                mBorder.style.height = __UnitAddPx(h);
            }
        }

        if (_resizeType == bs_field_ResizeType.North || _resizeType == bs_field_ResizeType.NorthWest || _resizeType == bs_field_ResizeType.NorthEast) {
            var h = ppWindow.height + 2 + moveY * (-1);
            var top = ppWindow.top - 1 + moveY;
            if (h > bs_field_ppWindow_MinHeight && top >= 0) {
                mBorder.style.height = __UnitAddPx(h);
                mBorder.style.top = __UnitAddPx(top);
            }
        }
        if (_resizeType == bs_field_ResizeType.West || _resizeType == bs_field_ResizeType.NorthWest || _resizeType == bs_field_ResizeType.SourthWest) {
            var w = ppWindow.width + 2 + moveX * (-1);
            var left = ppWindow.left - 1 + moveX;
            if (w > bs_field_ppWindow_MinWidth && left >= 0) {
                mBorder.style.width = __UnitAddPx(w);
                mBorder.style.left = __UnitAddPx(left);
            }
        }

        if (ppWindow.targetWindow.event)
            e.returnValue = false; 	/* 阻止ie下a,img的默认事件 */
    }

    this.targetDocument.onmouseup = function(e) {
        if (!ppWindow)
            return false;
        var moveMain = ppWindow.targetDocument.getElementById(ppWindow.mainid);
        if (_resizeType == bs_field_ResizeType.East || _resizeType == bs_field_ResizeType.SourthEast || _resizeType == bs_field_ResizeType.NorthEast) {
            ppWindow.width = __UnitRemovePx(mBorder.style.width) - 2;
            windowDoc.style.width = ppWindow.width + "px";
            if (null != mFrame && undefined != mFrame) {
                mFrame.style.width = ppWindow.width + "px";
            }
            moveMain.style.width = ppWindow.width + "px";
        }
        if (_resizeType == bs_field_ResizeType.Sourth || _resizeType == bs_field_ResizeType.SourthEast || _resizeType == bs_field_ResizeType.SourthWest) {
            ppWindow.height = __UnitRemovePx(mBorder.style.height) - 2;
            windowDoc.style.height = ppWindow.height + "px";
            if (null != mFrame && undefined != mFrame) {
                mFrame.style.height = ppWindow.height - bs_field_ppWindowTitle_Height - 1 + "px";
            }
            moveMain.style.height = ppWindow.height - bs_field_ppWindowTitle_Height - 1 + "px";
        }
        if (_resizeType == bs_field_ResizeType.North || _resizeType == bs_field_ResizeType.NorthWest || _resizeType == bs_field_ResizeType.NorthEast) {
            ppWindow.top = __UnitRemovePx(mBorder.style.top) + 1;
            ppWindow.height = __UnitRemovePx(mBorder.style.height) - 2;
            windowDoc.style.top = ppWindow.top + "px";
            windowDoc.style.height = ppWindow.height + "px";
            if (null != mFrame && undefined != mFrame) {
                mFrame.style.height = ppWindow.height - bs_field_ppWindowTitle_Height - 1 + "px";
            }
            moveMain.style.height = ppWindow.height - bs_field_ppWindowTitle_Height - 1 + "px";
        }
        if (_resizeType == bs_field_ResizeType.West || _resizeType == bs_field_ResizeType.NorthWest || _resizeType == bs_field_ResizeType.SourthWest) {
            ppWindow.left = __UnitRemovePx(mBorder.style.left) + 1;
            ppWindow.width = __UnitRemovePx(mBorder.style.width) - 2;
            windowDoc.style.left = ppWindow.left + "px";
            windowDoc.style.width = ppWindow.width + "px";
            if (null != mFrame && undefined != mFrame) {
                mFrame.style.width = ppWindow.width + "px";
            }
            moveMain.style.width = ppWindow.width + "px";
        }

        if (mBorder.releaseCapture)
            mBorder.releaseCapture();
        else if (ppWindow.targetWindow.captureEvents)
            ppWindow.targetWindow.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);

        ppWindow.targetDocument.onmousemove = null;
        ppWindow.targetDocument.onmouseup = null;
        ppWindow.targetDocument.ondragstart = null;
        ppWindow.targetDocument.onselectstart = null;
        ppWindow.targetDocument.onselect = null;
        mBorder.parentNode.removeChild(mBorder);
        movePanel.parentNode.removeChild(movePanel);
    }

    //如果提供了事件对象，则这是一个非IE浏览器
    if (e && e.stopPropagation)// 因此它支持W3C的stopPropagation()方法　　
        e.stopPropagation();
    else//否则，我们需要使用IE的方式来取消事件冒泡　　
        this.targetWindow.event.cancelBubble = true;
    if (e && e.preventDefault)
        e.preventDefault();

}


function __setCurSelWindow(_o) {
    if (null != _o) {
        var theFrame = _o.getElementsByTagName("iframe")[0];
        if (null != theFrame && undefined != theFrame) {
            theFrame.focus();
        }
    }
    bs_field_ppWindowDocSelected = _o;
}

function __showDisplayWindow() {
    //遍历当前打开的window，选择上一个window
    var windowCount = bs_field_ppWindowsIdList.length;
    for (var i = 0; i < windowCount; i++) {
        var oWindow = bs_field_ppWindows[bs_field_ppWindowsIdList[i]];
        if (oWindow.display == bs_field_windowDisplay.Show) {
            oWindow.focus(null);
            if (bs_field_IsShowTaskBar && oWindow.isShowInTaskBar) {
                bs_field_TaskItems[oWindow.taskitemid].focus(null);
            }
            break;
        }
    }
}

var bs_field_buttonType = { Min: 1, Max: 2, Normal: 3, Close: 4 };
var bs_field_ButtonClass = "window-button";
var bs_field_HtButtonClass = {
    1: "window-button-min-out",
    2: "window-button-max-out",
    3: "window-button-normal-out",
    4: "window-button-close-out"
}

function bs_WindowButton(_id, _targetId, _type) {
    this.id = _id;
    this.targetId = _targetId;
    this.type = _type;
    this.targetWindow = window;
    this.targetDocument = document;
    this.createButton = function() {
        this.targetDocument = this.targetWindow.document;
        var btnClass = bs_field_ButtonClass + " " + bs_field_HtButtonClass[this.type];
        var btn = __createDocDiv(this.targetDocument, this.id, btnClass, "");
        var targetDoc = this.targetDocument;
        btn.onmouseover = function() {
            targetDoc.onmousemove = null;
            this.className = this.className.replace("out", "over");
        }
        btn.onmouseout = function() {
            this.className = this.className.replace("over", "out");
        }
        var btnInstance = this;
        btn.onmousedown = function(e) {
            btnInstance.onmousedown(e);
        }
        btn.onmouseup = function(e) {
            btnInstance.onmouseup(e);
        }
        btn.onclick = function(e) {
            btnInstance.onclick(e);
        }
        return btn;
    }
}
//button event
bs_WindowButton.prototype.onmousedown = function(e) {
    //如果提供了事件对象，则这是一个非IE浏览器
    if (e && e.stopPropagation)// 因此它支持W3C的stopPropagation()方法　　
        e.stopPropagation();
    else//否则，我们需要使用IE的方式来取消事件冒泡
        this.targetWindow.event.cancelBubble = true;
    if (e && e.preventDefault)
        e.preventDefault();
}
bs_WindowButton.prototype.onmouseup = function(e) {
    if (e && e.stopPropagation)　
        e.stopPropagation();
    else
        this.targetWindow.event.cancelBubble = true;
    if (e && e.preventDefault)
        e.preventDefault();
}
bs_WindowButton.prototype.onclick = function(e) {
    var t = this.targetDocument.getElementById(this.targetId);
    var ppWindow = bs_field_ppWindows[this.targetId];
    if (this.type == bs_field_buttonType.Close) {
        if (null != ppWindow.windowClosing && undefined != ppWindow.windowClosing) {
            ppWindow.windowClosing();
        }

        if (bs_field_IsShowTaskBar && ppWindow.isShowInTaskBar) {
            var taskitem = bs_field_TaskItems[ppWindow.taskitemid];
            taskitem.del(null);
        }
        ppWindow.del(t);
        if (ppWindow.isShowInTaskBar) {
            __showDisplayWindow();
        }
    }
    else if (this.type == bs_field_buttonType.Max) {
        var bodyWidth = this.targetDocument.body.clientWidth;
        var bodyHeight = this.targetDocument.body.clientHeight;

        t.style.left = 0 + "px";
        t.style.top = 0 + "px";
        t.style.width = bodyWidth - 2 + "px";
        t.style.height = bodyHeight - 2 + "px";

        var tFrame = t.getElementsByTagName("iframe")[0];
        if (null != tFrame && undefined != tFrame) {
            tFrame.style.width = bodyWidth - 2 + "px";
            tFrame.style.height = bodyHeight - bs_field_ppWindowTitle_Height - 3 + "px";
        }
        var windowMainDoc = this.targetDocument.getElementById(ppWindow.mainid);
        if (null != windowMainDoc && undefined != windowMainDoc) {
            windowMainDoc.style.width = bodyWidth - 2 + "px";
            windowMainDoc.style.height = bodyHeight - bs_field_ppWindowTitle_Height - 3 + "px";
        }

        var normal = new bs_WindowButton(this.targetId + "_btnnormal", this.targetId, bs_field_buttonType.Normal);
        normal.targetWindow = this.targetWindow;
        var btnNormal = normal.createButton();
        var btnMax = this.targetDocument.getElementById(this.id);
        btnMax.parentNode.replaceChild(btnNormal, btnMax);
        ppWindow.sizestatus = bs_field_windowStatus.Max;
        return false;
    }
    else if (this.type == bs_field_buttonType.Normal) {
        if (!t || !ppWindow)
            return false;
        ppWindow.sizestatus = bs_field_windowStatus.Normal;
        t.style.left = ppWindow.left + "px";
        t.style.top = ppWindow.top + "px";

        var tFrame = t.getElementsByTagName("iframe")[0];
        if (tFrame) {
            tFrame.style.width = ppWindow.width + "px";
            tFrame.style.height = ppWindow.height - bs_field_ppWindowTitle_Height - 1 + "px";
        }
        var windowMainDoc = this.targetDocument.getElementById(ppWindow.mainid);
        if (null != windowMainDoc && undefined != windowMainDoc) {
            windowMainDoc.style.width = ppWindow.width + "px";
            windowMainDoc.style.height = ppWindow.height - bs_field_ppWindowTitle_Height - 1 + "px";
        }

        t.style.width = ppWindow.width + "px";
        t.style.height = ppWindow.height + "px";
        var max = new bs_WindowButton(this.targetId + "_btnmax", this.targetId, bs_field_buttonType.Max);
        max.targetWindow = this.targetWindow;
        var btnMax = max.createButton();
        var btnNormal = this.targetDocument.getElementById(this.id);
        btnNormal.parentNode.replaceChild(btnMax, btnNormal);
        return false;
    }
    else if (this.type == bs_field_buttonType.Min) {
        ppWindow.hide(t);
        if (bs_field_IsShowTaskBar && ppWindow.isShowInTaskBar) {
            var taskitem = bs_field_TaskItems[ppWindow.taskitemid];
            taskitem.blur();
        }
        __showDisplayWindow();
    }
    //如果提供了事件对象，则这是一个非IE浏览器
    if (e && e.stopPropagation)// 因此它支持W3C的stopPropagation()方法　　
        e.stopPropagation();
    else//否则，我们需要使用IE的方式来取消事件冒泡
        this.targetWindow.event.cancelBubble = true;
    if (e && e.preventDefault)
        e.preventDefault();
}

function __createDiv(_id, _class , _html) {
    var o = document.createElement("div");
    if (null != _id && "" != _id)
        o.id = _id;
    if (null != _class && "" != _class)
        o.className = _class;
    if (null != _html && "" != _html)
        o.innerHTML = _html;
    return o;
}

function __createDocDiv(__parentDoc, _id, _class, _html) {
    var o = __parentDoc.createElement("div");
    if (null != _id && "" != _id)
        o.id = _id;
    if (null != _class && "" != _class)
        o.className = _class;
    if (null != _html && "" != _html)
        o.innerHTML = _html;
    return o;
}

function __setStyle(_o, _width, _height, _left, _top) {
    if (null != _width && "" != _width) {
        _o.style.width = (_width + "").replace("px","") + "px";
    }
    if (null != _height && "" != _height) {
        _o.style.height = (_height + "").replace("px", "") + "px";
    }
    if (null != _left && "" != _left) {
        _o.style.left = (_left + "").replace("px", "") + "px";
    }
    if (null != _top && "" != _top) {
        _o.style.top = (_top + "").replace("px", "") + "px";
    }
}


/*move the ppwindow*/
var dragF = {
    targetWindow: window,
    drag: function(obj) {
        var o = obj;
        if (typeof o == "string") {
            o = dragF.targetWindow.document.getElementById(obj);
        }
        o.onmousedown = function(e) {
            var e = e ? e : dragF.targetWindow.event;
            if (!dragF.targetWindow.event) { e.preventDefault(); }     /* 阻止标注浏览器下拖动a,img的默认事件 */

            var ppWindow = bs_field_ppWindows[o.id];
            if (ppWindow.sizestatus != bs_field_windowStatus.Normal)
                return false;

            //切换当前选中window
            if (bs_field_ppWindowDocSelected && bs_field_ppWindowDocSelected != o) {
                bs_field_ppWindows[bs_field_ppWindowDocSelected.id].blur(bs_field_ppWindowDocSelected);
            }
            ppWindow.focus(o);

            //弹出移动虚线框
            var mBorder = __createDocDiv(ppWindow.targetDocument, "windowMoveBorder", "window-move-border", "");
            var movePanel = __createDocDiv(ppWindow.targetDocument, "windowMovePanel", "window-move-panel", "");
            mBorder.style.width = __UnitAddPx(ppWindow.width + 2);
            mBorder.style.height = __UnitAddPx(ppWindow.height + 2);
            mBorder.style.left = __UnitAddPx(ppWindow.left - 1);
            mBorder.style.top = __UnitAddPx(ppWindow.top - 1);
            ppWindow.targetDocument.body.appendChild(movePanel);
            ppWindow.targetDocument.body.appendChild(mBorder);

            //记录异动前的坐标位置
            var tempX = o.offsetLeft;
            var tempY = o.offsetTop;
            dragF.x = e.clientX;
            dragF.y = e.clientY;

            ppWindow.targetDocument.ondragstart = function() { return false; }
            ppWindow.targetDocument.onselectstart = function() { return false; }
            ppWindow.targetDocument.onselect = function() { ppWindow.targetDocument.selection.empty(); }

            if (o.setCapture)
                o.setCapture();
            else if (ppWindow.targetWindow.captureEvents)
                ppWindow.targetWindow.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
            var bodyWidth = ppWindow.targetDocument.body.clientWidth;
            var bodyHeight = ppWindow.targetDocument.body.clientHeight;

            ppWindow.targetDocument.onmousemove = function(e) {
                var e = e ? e : ppWindow.targetWindow.event;
                var nLeftMax = bodyWidth - ppWindow.width - 3;
                var nLeft = tempX + e.clientX - dragF.x;
                if (nLeft && nLeft < 0)
                    mBorder.style.left = "0px";
                else if (nLeft && nLeft > nLeftMax)
                    mBorder.style.left = __UnitAddPx(nLeftMax);
                else
                    mBorder.style.left = __UnitAddPx(nLeft);

                var nTopMax = bodyHeight - ppWindow.height - 3;
                var nTop = tempY + e.clientY - dragF.y;
                if (nTop && nTop < 0)
                    mBorder.style.top = "0px";
                else if (nTop && nTop > nTopMax)
                    mBorder.style.top = __UnitAddPx(nTopMax);
                else
                    mBorder.style.top = __UnitAddPx(nTop);

                if (ppWindow.targetWindow.event)
                    e.returnValue = false; 	/* 阻止ie下a,img的默认事件 */
            }

            ppWindow.targetDocument.onmouseup = function() {
                if (!ppWindow)
                    return false;
                var mBorderLeft = __UnitRemovePx(mBorder.style.left);
                var mBorderTop = __UnitRemovePx(mBorder.style.top);
                ppWindow.left = mBorderLeft == 0 ? 0 : (mBorderLeft + 1);
                ppWindow.top = mBorderTop == 0 ? 0 : (mBorderTop + 1);
                //bs_field_ppWindows[o.id] = ppWindow;

                o.style.left = __UnitAddPx(ppWindow.left);
                o.style.top = __UnitAddPx(ppWindow.top);
                if (o.releaseCapture)
                    o.releaseCapture();
                else if (ppWindow.targetWindow.captureEvents)
                    ppWindow.targetWindow.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);

                ppWindow.targetDocument.onmousemove = null;
                ppWindow.targetDocument.onmouseup = null;
                ppWindow.targetDocument.ondragstart = null;
                ppWindow.targetDocument.onselectstart = null;
                ppWindow.targetDocument.onselect = null;

                o.onmousedown = null;
                mBorder.parentNode.removeChild(mBorder);
                movePanel.parentNode.removeChild(movePanel);
            }
        }
    }
}

function __GetNodeByClass(_nodeList, _className) {
    if (null == _nodeList || _nodeList.length == 0)
        return null;
    var n = _nodeList.length;
    for (var i = 0; i < n; i++) {
        if (_nodeList[i]["className"] && null != _nodeList[i]["className"]) {
            if (_nodeList[i]["className"].toString().indexOf(_className) >= 0)
                return _nodeList[i];
        }
    }
    return null;
}

function __UnitAddPx(_o) {
    if (null == _o)
        return "";
    _o = _o + "";
    return _o.replace("px", "") + "px";
}
function __UnitRemovePx(_o) {
    if (null == _o || "" == _o)
        return "";
    var oInt = parseInt(_o.toString().replace("px", ""));
    return oInt;
}



//taskbar
var bs_field_IsShowTaskBar = false;
var bs_field_TaskBarRef = null;

//target window of the taskbar
var bs_field_TaskBarTargetWindow = window;
//target document of the taskbar
var bs_field_TaskBarTargetDoc = document;

var bs_field_TaskBarTargetElement = null;
var bs_field_TaskBarClassName = "taskbar";
var bs_field_TaskBarId = "window_taskbar";
var bs_field_TaskBarHeight = 26;

var bs_field_TaskItemClassName = "taskitem";
var bs_field_TaskItemClassName_normal = "taskitem-normal";
var bs_field_TaskItemClassName_sel = "taskitem-sel";
var bs_field_TaskItems = new Object();
var bs_field_TaskItemHeight = 20;

var bs_field_Status_Normal = 1;
var bs_field_Status_Selected = 2;
//任务栏中显示文字的长度
var bs_field_TextItemLength = 7;

function bs_event_ShowTaskBar(_targetWindow, _parentElement) {
    bs_field_IsShowTaskBar = true;
    if (null != _targetWindow && undefined != _targetWindow) {
        bs_field_TaskBarTargetWindow = _targetWindow;
        bs_field_TaskBarTargetDoc = _targetWindow.document;
    }
    bs_field_TaskBarTargetElement = _parentElement;
    var taskBar = __createDocDiv(bs_field_TaskBarTargetDoc, bs_field_TaskBarId, bs_field_TaskBarClassName, "");
    taskBar.style.height = __UnitAddPx(bs_field_TaskBarHeight);
    //taskBar.innerHTML = "<input type='button' class='btnWindowHome' value='' />";
    bs_field_TaskBarTargetElement.appendChild(taskBar);
    bs_field_TaskBarRef = taskBar;
}

function bs_TaskItem(_id, _windowid, _title, _status) {
    this.id = _id;
    this.windowid = _windowid;
    this.title = _title;
    this.status = _status;
    this.createTaskItem = function() {
        var taskItemClassName = this.status == bs_field_Status_Normal ? this.getNormalClass() : this.getSelectClass();
        var inHtml = __cutString(this.title, bs_field_TextItemLength);
        var taskItem = __createDocDiv(bs_field_TaskBarTargetDoc, this.id, taskItemClassName, inHtml);
        taskItem.style.height = __UnitAddPx(bs_field_TaskItemHeight);
        taskItem.style.marginTop = __UnitAddPx((bs_field_TaskBarHeight - bs_field_TaskItemHeight - 4) / 2);
        taskItem.style.lineHeight = __UnitAddPx(bs_field_TaskItemHeight);
        taskItem.title = this.title;

        var itemInstance = this;
        taskItem.onmousedown = function(e) { itemInstance.onmousedown(e); }
        taskItem.onmouseup = function(e) { itemInstance.onmouseup(e); }
        taskItem.onclick = function(e) { itemInstance.onclick(e); }
        //禁止选中其中的文字
        taskItem.onselectstart = "return false";
        taskItem.unselectable = "on";
        taskItem.oncontextmenu = function(e) { itemInstance.showmenu(e); return false; }

        if (null != bs_field_ppWindowDocSelected) {
            var ppWindow = bs_field_ppWindows[bs_field_ppWindowDocSelected.id];
            var curSelTaskItem = bs_field_TaskBarTargetDoc.getElementById(ppWindow.taskitemid);
            if (curSelTaskItem) {
                curSelTaskItem.className = this.getNormalClass();
            }
        }
        bs_field_TaskBarRef.appendChild(taskItem);
        bs_field_TaskItems[this.id] = this;
    }
}

//taskitem event
bs_TaskItem.prototype.getNormalClass = function() {
    return bs_field_TaskItemClassName + " " + bs_field_TaskItemClassName_normal;
}
bs_TaskItem.prototype.getSelectClass = function() {
    return bs_field_TaskItemClassName + " " + bs_field_TaskItemClassName_sel;
}
bs_TaskItem.prototype.blur = function(_taskItemDoc) {
    this.status = bs_field_Status_Normal;
    var taskItemDoc = _taskItemDoc;
    if(!taskItemDoc || null == taskItemDoc)
        taskItemDoc = bs_field_TaskBarTargetDoc.getElementById(this.id);
    taskItemDoc.className = this.getNormalClass();
}
bs_TaskItem.prototype.focus = function(_taskItemDoc) {
    this.status = bs_field_Status_Selected;
    var taskItemDoc = _taskItemDoc;
    if (!taskItemDoc || null == taskItemDoc)
        taskItemDoc = bs_field_TaskBarTargetDoc.getElementById(this.id);
    taskItemDoc.className = this.getSelectClass();
}
bs_TaskItem.prototype.del = function(_taskItemDoc) {
    var taskItemDoc = _taskItemDoc;
    if (!taskItemDoc || null == taskItemDoc)
        taskItemDoc = bs_field_TaskBarTargetDoc.getElementById(this.id);
    taskItemDoc.parentNode.removeChild(taskItemDoc);
    bs_field_TaskItems[this.id] = null;
    delete bs_field_TaskItems[this.id];
}

bs_TaskItem.prototype.onmousedown = function(e) {
    e = e || bs_field_TaskBarTargetWindow.event;
    if (e && e.stopPropagation)　　
        e.stopPropagation();
    else
        bs_field_TaskBarTargetWindow.event.cancelBubble = true;
    if (e && e.preventDefault)
        e.preventDefault();
    return false;
}

bs_TaskItem.prototype.onmouseup = function(e) {

    if (e && e.stopPropagation)
        e.stopPropagation();
    else
        bs_field_TaskBarTargetWindow.event.cancelBubble = true;
    if (e && e.preventDefault)
        e.preventDefault();
    return false;
}

bs_TaskItem.prototype.onclick = function(e) {
    //bs_field_TaskBarTargetDoc.getElementById(this.id).focus();
    //如果当前处于未选中状态
    if (this.status == bs_field_Status_Normal) {
        if (null != bs_field_ppWindowDocSelected) {
            var curWindow = bs_field_ppWindows[bs_field_ppWindowDocSelected.id];
            curWindow.blur(bs_field_ppWindowDocSelected);
            //如果显示任务栏标签则切换任务栏标签
            if (bs_field_IsShowTaskBar && curWindow.isShowInTaskBar)
                bs_field_TaskItems[curWindow.taskitemid].blur(null);
        }
        this.focus(null);
        var ppWindow = bs_field_ppWindows[this.windowid];
        ppWindow.focus(null);
    }
    else if (this.status == bs_field_Status_Selected) {
        var ppWindow = bs_field_ppWindows[this.windowid];
        ppWindow.hide(bs_field_ppWindowDocSelected);
        this.blur(null);
        __showDisplayWindow();
    }
}

var bs_field_TaskMenuId = "taskitemmenu";
var bs_field_TaskItemMenuClass = "taskitem-menu";
//var bs_field_TaskitemMenuHeight = 40;
var bs_field_TaskItemCount = 2;
var bs_field_MousePoint = { X: 0, Y: 0 };
bs_TaskItem.prototype.showmenu = function(e) {
    var e = e ? e : bs_field_TaskBarTargetWindow.event;
    if (!bs_field_TaskBarTargetWindow.event) { e.preventDefault(); }
    var x = e.clientX;
    var y = e.clientY;
    var taskitem = bs_field_TaskItems[this.id];
    var ppWindow = bs_field_ppWindows[this.windowid];
    var menu = __createDocDiv(bs_field_TaskBarTargetDoc, bs_field_TaskMenuId, bs_field_TaskItemMenuClass, "");
    menu.style.left = x + 2 + "px";
    //menu.style.height = __UnitAddPx(bs_field_TaskitemMenuHeight);
    menu.style.top = y - bs_field_TaskItemCount * 20 - bs_field_TaskItemCount * 3 - 3  - 5 + "px";
    //关闭命令
    var aClose = bs_field_TaskBarTargetDoc.createElement("a");
    aClose.tabIndex = Math.max - 1;
    aClose.href = "#";
    aClose.innerHTML = "× 关闭";
    aClose.onclick = function() {
        ppWindow.targetDocument.getElementById(ppWindow.id + "_btnclose").click();
        menu.parentNode.removeChild(menu);
    }
    if (!ppWindow.isHtmlContent) {
        //刷新命令
        var aRefresh = bs_field_TaskBarTargetDoc.createElement("a");
        aRefresh.tabIndex = Math.max - 2;
        aRefresh.href = "#";
        aRefresh.innerHTML = "○ 刷新";
        aRefresh.onclick = function() {
            var tFrame = ppWindow.targetDocument.getElementById(ppWindow.mainid).getElementsByTagName("iframe")[0];
            if (null != tFrame && undefined != tFrame) {
                tFrame.setAttribute("src", ppWindow.url);
            }
            menu.parentNode.removeChild(menu);
        }
        menu.appendChild(aRefresh);
    }
    menu.appendChild(aClose);
    var menuOtherRef = bs_field_TaskBarTargetDoc.getElementById(bs_field_TaskMenuId);
    if (null != menuOtherRef && undefined != menuOtherRef)
        menuOtherRef.parentNode.removeChild(menuOtherRef);
    bs_field_TaskBarTargetDoc.body.appendChild(menu);
    //注册获取焦点的事件中，注册blur事件,并加入tabIndex属性兼容浏览器的blur和focus事件
    menu.tabIndex = Math.max;
    menu.hideFocus = "true";
    bs_event_AddEvent(menu, "focus", focusTaskMenu);
    setTimeout(function() { menu.focus(); }, 0);
}

function focusTaskMenu(e) {
    var menuRef = bs_field_TaskBarTargetDoc.getElementById(bs_field_TaskMenuId);
    bs_event_AddEvent(menuRef, "blur",
        function() {
            var curDocument = null;
            setTimeout(function() {
                curDocument = top.window.document.activeElement;
                if (curDocument && curDocument != null) {
                    if (curDocument == menuRef || curDocument.parentNode == menuRef || curDocument.parentNode.parentNode == menuRef)
                        return;
                }
                setTimeout(function() {
                    menuRef = bs_field_TaskBarTargetDoc.getElementById(bs_field_TaskMenuId);
                    if (null != menuRef && undefined != menuRef) {
                        menuRef.parentNode.removeChild(menuRef);
                    }
                }, 1);
            }, 5);
        }
    );
}
function topMouseMove(e) {
    bs_field_MousePoint.X = e.clientX;
    bs_field_MousePoint.Y = e.clientY;
}

function __cutString(_str, _legnth) {
    if (null == _str || "" == _str)
        return _str;
    var len = _str.length;
    if (len > _legnth)
        return _str.substring(0, _legnth - 2) + "...";
    return _str;
}

function bs_event_AddEvent(_target, _eventType, _eventHandler) {
    if (_target.addEventListener) {
        _target.addEventListener(_eventType, _eventHandler, true);
    }
    else {
        _target.attachEvent("on" + _eventType, _eventHandler);
    }
}

function bs_event_RemoveEvent(_target, _eventType, _eventHandler) {
    if (_target.removeEventListener) {
        _target.removeEventListener( _eventType, _eventHandler, false);
    }
    else {
        _target.detachEvent("on" + _eventType, _eventHandler);
    }
}

function getStyle(o, styleName) {
    if (o.currentStyle) {
        return o.currentStyle[styleName];
    }
    else {
        var oStyles = o.ownerDocument.defaultView.getComputedStyle(o, null);
        return oStyles[styleName];
    }
}
//窗体创建工厂
var windowFactory = new Object();
windowFactory.topWindowArray = new Array();
windowFactory.windowResult = { cancel: "cancel", ok: "ok" }
//在loyout.js中获取menuFrame和mainFrame
windowFactory.mainWindow = null;
windowFactory.menuWindow = null;

windowFactory.setWindowArguments = function(_windowArguments, _widow) {
    if (_windowArguments.title)
        _widow.title = _windowArguments.title;
    if (_windowArguments.width)
        _widow.width = _windowArguments.width;
    if (_windowArguments.height)
        _widow.height = _windowArguments.height;
    if (_windowArguments.url)
        _widow.url = _windowArguments.url;
    if (_windowArguments.zindex)
        _widow.zIndex = _windowArguments.zindex;
    if (_windowArguments.ishtml)
        _widow.isHtmlContent = _windowArguments.ishtml;
    if (_windowArguments.target) {
        if (_windowArguments.target == "top")
            _widow.targetWindow = top.window;
        else if (_windowArguments.target == "workframe")
            _widow.targetWindow = top.windowFactory.mainWindow;
    }
    if (undefined != _windowArguments.resize && null != _windowArguments.resize)
        _widow.isResize = _windowArguments.resize;
    if (undefined != _windowArguments.move && null != _windowArguments.move)
        _widow.isMove = _windowArguments.move;
    if (undefined != _windowArguments.maxbox && null != _windowArguments.maxbox)
        _widow.isIncludeMax = _windowArguments.maxbox;
    if (undefined != _windowArguments.minbox && null != _windowArguments.minbox)
        _widow.isIncludeMin = _windowArguments.minbox;
    if (undefined != _windowArguments.intaskbar && null != _windowArguments.intaskbar)
        _widow.isShowInTaskBar = _windowArguments.intaskbar;
    if (undefined != _windowArguments.closeFunction && null != _windowArguments.closeFunction)
        _widow.windowClosing = function() { _windowArguments.closeFunction(); }

    if (undefined != _windowArguments.windowKey && null != _windowArguments.windowKey)
        _widow.windowKey = _windowArguments.windowKey;
    //传给window的参数windowArguments
    if (undefined != _windowArguments.windowArguments && null != _windowArguments.windowArguments)
        _widow.windowArguments = _windowArguments.windowArguments;
    if (undefined != _windowArguments.iconURL && null != _windowArguments.iconURL)
        _widow.iconURL = _windowArguments.iconURL;
    if (undefined != _windowArguments.closebox && null != _windowArguments.closebox)
        _widow.isIncludeClose = _windowArguments.closebox;
        
}

//当前活动窗口（bs_Window)
windowFactory.activeWindow = function() {
    if (bs_field_ppWindowDocSelected && undefined != bs_field_ppWindowDocSelected && null != bs_field_ppWindowDocSelected)
        return bs_field_ppWindows[bs_field_ppWindowDocSelected.id];
    return null;
}

windowFactory.activeDocumentWindow = function() {
    return bs_field_ppWindowDocSelected;
}
windowFactory.topAlert = function(content, yesFunction) {
    var alertWindow = new bs_Window();
    alertWindow.zIndex = 10000;
    alertWindow.title = "系统提示";
    alertWindow.width = 200;
    alertWindow.height = 100;
    alertWindow.targetWindow = top.window;
    alertWindow.isResize = false;
    alertWindow.isIncludeMax = false;
    alertWindow.isIncludeMin = false;
    alertWindow.isShowInTaskBar = false;
    alertWindow.isMove = false;
    alertWindow.isHtmlContent = true;
    var addFunction = function() {
        if (yesFunction && null != yesFunction && undefined != yesFunction)
            yesFunction();
        bs_event_hideGrayBox(top.window.document);
        if (top.Bluesky.component.Masklayer) {
            top.Bluesky.component.Masklayer.remove();
        }
        top.window.document.ondragstart = null;
        top.window.document.onselectstart = null;
        top.window.document.onselect = null;
        top.window.document.body.removeAttribute("style");
    }
    alertWindow.windowClosing = addFunction;

    var textDiv = __createDocDiv(alertWindow.targetDocument, "", "alert-text", content);
    var buttonDiv = __createDocDiv(alertWindow.targetDocument, "", "alert-button", "");
    //var contentDiv = __createDocDiv(alertWindow.targetDocument, "", "", "");
    var arrContent = new Array();
    var inputButton = alertWindow.targetDocument.createElement("input");
    inputButton.type = "button";
    inputButton.value = " 确定 ";
    inputButton.className = "btn-normal";
    inputButton.onclick = function() {
        var btnClose = alertWindow.targetDocument.getElementById(alertWindow.id + "_btnclose");
        if (null != btnClose && undefined != btnClose) {
            btnClose.click();
        }
    }
    top.window.document.ondragstart = function() { return false; }
    top.window.document.onselectstart = function() { return false; }
    top.window.document.onselect = function() { top.window.document.selection.empty(); }
    top.window.document.body.setAttribute("style", "-moz-user-select:none;");

    buttonDiv.appendChild(inputButton);
    //contentDiv.appendChild(textDiv);
    //contentDiv.appendChild(buttonDiv);
    arrContent[0] = textDiv;
    arrContent[1] = buttonDiv;

    alertWindow.htmlContent = arrContent;
    //bs_event_showGrayBox(top.window.document);
    if (top.Bluesky.component.Masklayer) {
        top.Bluesky.component.Masklayer.create();
    }
    alertWindow.createWindow();
    //inputButton.focus();
}

windowFactory.targetAlert = function(targetWindow, content, yesFunction) {
    var alertWindow = new bs_Window();
    alertWindow.zIndex = 9999;
    alertWindow.title = "系统提示";
    alertWindow.width = 200;
    alertWindow.height = 100;
    alertWindow.targetWindow = targetWindow;
    alertWindow.isResize = false;
    alertWindow.isIncludeMax = false;
    alertWindow.isIncludeMin = false;
    alertWindow.isShowInTaskBar = false;
    alertWindow.isMove = false;
    alertWindow.isHtmlContent = true;
    var addFunction = function() {
        if (yesFunction && null != yesFunction && undefined != yesFunction)
            yesFunction();
        //bs_event_hideGrayBox(targetWindow.document);
        if (targetWindow.Bluesky.component.Masklayer) {
            targetWindow.Bluesky.component.Masklayer.remove();
        }
        targetWindow.document.ondragstart = null;
        targetWindow.document.onselectstart = null;
        targetWindow.document.onselect = null;
    }
    alertWindow.windowClosing = addFunction;

    var textDiv = __createDocDiv(alertWindow.targetDocument, "", "alert-text", content);
    var buttonDiv = __createDocDiv(alertWindow.targetDocument, "", "alert-button", "");
    var contentDiv = __createDocDiv(alertWindow.targetDocument, "", "", "");

    var inputButton = alertWindow.targetDocument.createElement("input");
    inputButton.type = "button";
    inputButton.value = " 确定 ";
    inputButton.className = "btn-normal";
    inputButton.onclick = function() {
        var btnClose = alertWindow.targetDocument.getElementById(alertWindow.id + "_btnclose");
        if (null != btnClose && undefined != btnClose) {
            btnClose.click();
        }
    }
    targetWindow.document.ondragstart = function() { return false; }
    targetWindow.document.onselectstart = function() { return false; }
    targetWindow.document.onselect = function() { top.window.document.selection.empty(); }

    buttonDiv.appendChild(inputButton);
    contentDiv.appendChild(textDiv);
    contentDiv.appendChild(buttonDiv);

    alertWindow.htmlContent = contentDiv;
    //bs_event_showGrayBox(targetWindow.document);
    if (targetWindow.Bluesky.component.Masklayer) {
        targetWindow.Bluesky.component.Masklayer.create();
    }
    alertWindow.createWindow();

    inputButton.focus();
    targetWindow.document.getElementById(alertWindow.id).style.cursor = "default";
}

windowFactory.targetConfirm = function(_windowArguments, targetWindow, content, yesFunction) {
    _windowArguments.zindex = 9999;
    _windowArguments.resize = false;
    _windowArguments.maxbox = false;
    _windowArguments.minbox = false;
    _windowArguments.intaskbar = false;
    _windowArguments.move = false;
    _windowArguments.ishtml = false;
    var confirmWindow = new bs_Window();
    confirmWindow.zIndex = 9999;
    confirmWindow.title = "系统提示";
    confirmWindow.width = 300;
    confirmWindow.height = 150;
    confirmWindow.targetWindow = targetWindow;
    confirmWindow.isResize = false;
    confirmWindow.isIncludeMax = false;
    confirmWindow.isIncludeMin = false;
    confirmWindow.isShowInTaskBar = false;
    confirmWindow.isMove = false;
    confirmWindow.isHtmlContent = true;
    var addFunction = function() {
        //bs_event_hideGrayBox(targetWindow.document);
        if (targetWindow.Bluesky.component.Masklayer) {
            targetWindow.Bluesky.component.Masklayer.remove();
        }
        targetWindow.document.ondragstart = null;
        targetWindow.document.onselectstart = null;
        targetWindow.document.onselect = null;
    }
    confirmWindow.windowClosing = addFunction;

    var textDiv = __createDocDiv(confirmWindow.targetDocument, "", "alert-text", content);
    var buttonDiv = __createDocDiv(confirmWindow.targetDocument, "", "confirm-button", "");
    var contentDiv = __createDocDiv(confirmWindow.targetDocument, "", "", "");

    var inputButton = confirmWindow.targetDocument.createElement("input");
    inputButton.type = "button";
    inputButton.value = " 确定 ";
    inputButton.className = "btn-normal";
    inputButton.onclick = function() {
        var btnClose = confirmWindow.targetDocument.getElementById(confirmWindow.id + "_btnclose");
        if (null != btnClose && undefined != btnClose) {
            btnClose.click();
            if (yesFunction && null != yesFunction && undefined != yesFunction)
                yesFunction();
        }
    }

    var inputCancel = confirmWindow.targetDocument.createElement("input");
    inputCancel.type = "button";
    inputCancel.value = " 取消 ";
    inputCancel.className = "btn-normal";
    inputCancel.onclick = function() {
        var btnClose = confirmWindow.targetDocument.getElementById(confirmWindow.id + "_btnclose");
        if (null != btnClose && undefined != btnClose) {
            btnClose.click();
        }
    }

    targetWindow.document.ondragstart = function() { return false; }
    targetWindow.document.onselectstart = function() { return false; }
    targetWindow.document.onselect = function() { top.window.document.selection.empty(); }

    buttonDiv.appendChild(inputCancel);
    buttonDiv.appendChild(inputButton);

    contentDiv.appendChild(textDiv);
    contentDiv.appendChild(buttonDiv);

    confirmWindow.htmlContent = contentDiv;
    //bs_event_showGrayBox(targetWindow.document);
    if (targetWindow.Bluesky.component.Masklayer) {
        targetWindow.Bluesky.component.Masklayer.create();
    }
    confirmWindow.createWindow();

    inputButton.focus();
    targetWindow.document.getElementById(confirmWindow.id).style.cursor = "default";
}

windowFactory.targetFocusForm = function(_windowArguments) {
    var targetForm = new bs_Window();
    targetForm.zIndex = 9999;
    targetForm.isResize = false;
    targetForm.isIncludeMax = false;
    targetForm.isIncludeMin = false;
    targetForm.isShowInTaskBar = false;
    targetForm.isMove = false;
    targetForm.isHtmlContent = false;
    windowFactory.setWindowArguments(_windowArguments, targetForm);

    targetForm.windowClosing = function() {
        if (undefined != _windowArguments.closeFunction && null != _windowArguments.closeFunction) {
            var tFrames = targetForm.targetDocument.getElementById(targetForm.id).getElementsByTagName("iframe");
            if (tFrames && tFrames.length > 0) {
                var tFrameSingle = tFrames[0];
                if (tFrameSingle.contentWindow.window.windowReturnValue)
                    targetForm.windowReturnValue = tFrameSingle.contentWindow.window.windowReturnValue;
                if (tFrameSingle.contentWindow.window.windowResult)
                    targetForm.windowResult = tFrameSingle.contentWindow.window.windowResult;
            }
            _windowArguments.closeFunction(targetForm.windowResult, targetForm.windowReturnValue);
        }
        if (targetForm.targetWindow == top.window) {
            windowFactory.topWindowArray.splice(windowFactory.topWindowArray.length - 1, 1);
        }
        //bs_event_hideGrayBox(targetForm.targetDocument);
        if (targetForm.targetWindow.Bluesky.component.Masklayer) {
            targetForm.targetWindow.Bluesky.component.Masklayer.remove();
        }
    }

    //bs_event_showGrayBox(targetForm.targetDocument);
    if (targetForm.targetWindow.Bluesky.component.Masklayer) {
        targetForm.targetWindow.Bluesky.component.Masklayer.create();
    }
    targetForm.createWindow();

    if (targetForm.targetWindow == top.window) {
        windowFactory.topWindowArray[windowFactory.topWindowArray.length] = targetForm;
    }
}

//弹出模式窗口
windowFactory.topFocusForm = function(_windowArguments) {
    _windowArguments.target = top.window;
    _windowArguments.zindex = 9999 + windowFactory.topWindowArray.length * 2;
    this.targetFocusForm(_windowArguments);
}

windowFactory.closeTopFocusForm = function() {
    var btnClose = top.document.getElementById(windowFactory.topWindowArray[windowFactory.topWindowArray.length - 1].id + "_btnclose");
    if (null != btnClose && undefined != btnClose) {
        setTimeout(function() { btnClose.click(); }, 0);
    }
}
windowFactory.getWindowByTitle = function(_title) {
    for (var id in bs_field_ppWindows) {
        var ppWindow = bs_field_ppWindows[id];
        if (ppWindow.title == _title && !ppWindow.isHtmlContent) {
            var tFrame = ppWindow.targetDocument.getElementById(ppWindow.frameid);
            if (tFrame && undefined != tFrame && null != tFrame)
                return tFrame.contentWindow;
        }
    }
    return null;
}
//设置当前活动窗口的返回值
windowFactory.setWindowReturnValue = function(_returnValue) {
    var tWindow = this.getActiveWindow();
    if (undefined == tWindow || null == tWindow)
        return;
    tWindow.windowReturnValue = _returnValue;
}

//当前活动窗口
windowFactory.getActiveWindow = function() {
    if (undefined == bs_field_ppWindowDocSelected || null == bs_field_ppWindowDocSelected)
        return null;
    return bs_field_ppWindows[bs_field_ppWindowDocSelected.id];
}


//刷新主功能菜单
windowFactory.refreshMenuWindow = function() {
    var menuWindowRef = top.windowFactory.menuWindow;
    if (!menuWindowRef || undefined == menuWindowRef || null == menuWindowRef)
        return null;
    setTimeout(function() { top.windowFactory.refreshWindow(menuWindowRef); }, 0);
}

//刷新整个网站
windowFactory.refreshRoot = function() {
    setTimeout(function() { top.windowFactory.refreshWindow(top.window); }, 10);
}

//刷新window
windowFactory.refreshWindow = function(_windowRef) {
    if ((typeof _windowRef).toLowerCase() == "string")
        _windowRef = top.windowFactory.getWindowByTitle(_windowRef);
    _windowRef.location.href = _windowRef.location.href;
}