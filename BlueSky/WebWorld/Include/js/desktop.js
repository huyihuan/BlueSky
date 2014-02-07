var documentStore = document;
var timerInterval = null;
function $(_id) {
    return documentStore.getElementById(_id.toString());
}

window.onload = function() {
    //加载桌面应用
    setTimeout(function() { $("deskFrame").src = "DesktopMain.aspx"; }, 0);
    //设置任务栏高度和主桌面的高度
    timerInterval = setInterval(function() { desktopInit(); }, 200);
}

//window改变大小后，重新计算窗体的大小
window.onresize = function() {
    setTimeout(function() { setDesktopHeight(); }, 0);
}

function desktopInit() {
    var divTaskBar = $("divTaskBar");
    if (null == divTaskBar && undefined == divTaskBar)
        return;
    var taskbarHeight = getHeight(divTaskBar);
    if (isNaN(taskbarHeight) || taskbarHeight <= 0)
        return;
    setDesktopHeight();
    //隐藏loading层
    setTimeout(function() { bs_event_hideLoadingLayer(window.top.document); }, 0);
    clearInterval(timerInterval);
}

function setDesktopHeight() {
    var docHeight = documentStore.body.clientHeight;
    var divTaskBar = $("divTaskBar");
    var taskbarHeight = 0;
    if (null != divTaskBar && undefined != divTaskBar) {
        taskbarHeight = getHeight(divTaskBar);
        if (isNaN(taskbarHeight))
            taskbarHeight = 0;
    }
    var desktopHeight = docHeight - taskbarHeight;
    $("deskFrame").style.height = desktopHeight + "px";
    $("divDesktop").style.height = desktopHeight + "px";
}

function getHeight(_o) {
    var h = getDocStyle(_o, "height");
    return parseInt(h.toString().replace("px", ""));
}
function getDocStyle(o, styleName) {
    if (o.currentStyle) {
        return o.currentStyle[styleName];
    }
    else {
        var oStyles = o.ownerDocument.defaultView.getComputedStyle(o, null);
        return oStyles[styleName];
    }
}