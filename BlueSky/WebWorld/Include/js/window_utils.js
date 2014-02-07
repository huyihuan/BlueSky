var windowFactory = new Object();
windowFactory.currentTopFocusForm = null;
//在loyout.js中获取menuFrame和mainFrame
windowFactory.mainWindow = null;
windowFactory.menuWindow = null;

//刷新主功能菜单
windowFactory.refreshMenuWindow = function() {
    var menuWindowRef = top.windowFactory.menuWindow;
    if (!menuWindowRef || undefined == menuWindowRef || null == menuWindowRef)
        return null;
    setTimeout(function() { windowFactory.refreshWindow(menuWindowRef); }, 0);
}

//当前活动窗口（bs_Window)
windowFactory.activeWindow = function() {
    var mainWindowRef = top.windowFactory.mainWindow;
    if (!mainWindowRef || undefined == mainWindowRef || null == mainWindowRef)
        return null;
    var activeWindowRef = mainWindowRef.bs_field_ppWindowDocSelected;
    if (activeWindowRef && undefined != activeWindowRef && null != activeWindowRef)
        return mainWindowRef.bs_field_ppWindows[activeWindowRef.id];
    return null;
}

//弹出模式窗口
windowFactory.topFocusForm = function(_title, _url, _width, _height, closeFunction) {
    var mainWindowRef = top.windowFactory.mainWindow;
    if (!mainWindowRef || undefined == mainWindowRef || null == mainWindowRef)
        return null;
    mainWindowRef.windowFactory.topFocusForm(_title, _url, _width, _height, closeFunction);
}

//关闭当前模式窗口
windowFactory.closeTopFocusForm = function() {
    var mainWindowRef = top.windowFactory.mainWindow;
    if (!mainWindowRef || undefined == mainWindowRef || null == mainWindowRef)
        return null;
    //延迟关闭topFocusForm
    setTimeout(function() { mainWindowRef.windowFactory.closeTopFocusForm(); }, 20);
}

//刷新整个网站
windowFactory.refreshRoot = function() {
    setTimeout(function() { windowFactory.refreshWindow(top.window); }, 0);
}

//刷新window
windowFactory.refreshWindow = function(_windowRef) {
    if ((typeof _windowRef).toLowerCase() == "string")
        _windowRef = windowFactory.getWindowByTitle(_windowRef);
    _windowRef.location.href = _windowRef.location.href;
}