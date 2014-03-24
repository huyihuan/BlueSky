function setClass(setObj, className) {
    $(setObj).attr("class", className);
}
function getClass(getObj) {
    return $(getObj).attr("class");
}

$(document).ready(function() {
    $("span.tree-node-head-normal").each(function() {
        $(this).mouseover(function() { $(this).addClass("tree-node-head-hover"); });
        $(this).mouseout(function() { $(this).removeClass("tree-node-head-hover"); });
        $(this).click(function() {
            var currentSelectNode = $("span.tree-node-head-active");
            if (null != currentSelectNode && currentSelectNode.length > 0) {
                __setClassToNormal(currentSelectNode[0]);
            }
            __setClassToActive(this);
        });
    });
});

function nodePlusMinusExchange(parentNode) {
    if (null == parentNode || undefined == parentNode)
        return;
    var nodeType = parentNode.getAttribute("_type");
    if (nodeType == "parent") {
        //父节点图片的切换
        var currentSelectFolder = $(parentNode).parent().find("span[class^='tree-node-folder']");
        if (null != currentSelectFolder && currentSelectFolder.length > 0) {
            __toggleFolder(currentSelectFolder[0]);
        }

        //切换展开和收缩图标
        var currentSelectPlus = $(parentNode).parent().find("span[class*='tree-node-dot']");
        if (null != currentSelectPlus && currentSelectPlus.length > 0) {
            __togglePlusMinus(currentSelectPlus[0]);
        }

        //子菜单的显示与隐藏
        var currentSelectSons = $(parentNode).parent().next();
        if (null != currentSelectSons && currentSelectSons.length > 0) {
            if (getClass(currentSelectSons[0]) == "tree-nodes-box") {
                $(currentSelectSons[0]).toggle();
            }
        }
    }
}

function createWindowArguments(_nodeRef) {
    var windowArguments = {};
    if (!_nodeRef || undefined == _nodeRef || null == _nodeRef)
        return windowArguments;
    var name = _nodeRef.getAttribute("_name");
    if (name && undefined != name && null != name && "" != name)
        windowArguments.title = name;

    var value = _nodeRef.getAttribute("_value");
    if (value && undefined != value && null != value && "" != value) {
        value = "Window.aspx?fn=" + value;
        windowArguments.url = value;
    }

    var icon = _nodeRef.getAttribute("_icon");
    if (icon && undefined != icon && null != icon && "" != icon)
        windowArguments.icon = icon;

    var target = _nodeRef.getAttribute("_target");
    if (target && undefined != target && null != target && "" != target)
        windowArguments.target = target;
    //窗口的宽度，整型，最小值200
    var width = _nodeRef.getAttribute("_width");
    if (width && undefined != width && null != width && "" != width) {
        var nWidth = BlueSky.util.parseInt(width, 200);
        if (nWidth < 200)
            nWidth = 200;
        windowArguments.width = nWidth;
    }
    //窗口的高度，整型，最小值200
    var height = _nodeRef.getAttribute("_height");
    if (height && undefined != height && null != height && "" != height) {
        var nHeight = top.Utils.parseInt(height, 200);
        if (nHeight < 200)
            nHeight = 200;
        windowArguments.height = nHeight;
    }

    var resize = _nodeRef.getAttribute("_resize");
    if (resize && undefined != resize && null != resize && "" != resize)
        windowArguments.resize = resize == "1" ? true : false;

    var move = _nodeRef.getAttribute("_move");
    if (move && undefined != move && null != move && "" != move)
        windowArguments.move = move == "1" ? true : false;

    var maxbox = _nodeRef.getAttribute("_maxbox");
    if (maxbox && undefined != maxbox && null != maxbox && "" != maxbox)
        windowArguments.maxbox = maxbox == "1" ? true : false;
        
    var minbox = _nodeRef.getAttribute("_minbox");
    if (minbox && undefined != minbox && null != minbox && "" != minbox)
        windowArguments.minbox = minbox == "1" ? true : false;
        
    var intaskbar = _nodeRef.getAttribute("_intaskbar");
    if (intaskbar && undefined != intaskbar && null != intaskbar && "" != intaskbar)
        windowArguments.intaskbar = intaskbar == "1" ? true : false;

    //WindowKey: the id of the function
    var wKey = _nodeRef.getAttribute("_tag");
    if (wKey)
        windowArguments.windowKey = wKey;
        
    return windowArguments;
}

function __setClassToActive(headObj) {
    var thisClass = getClass(headObj);
    setClass(headObj, __normalToActive(thisClass));
}
function __setClassToNormal(headObj) {
    var thisClass = getClass(headObj);
    setClass(headObj, __activeToNormal(thisClass));
}

function __normalToActive(__folder) {
    return __folder.replace("normal", "active");
}
function __activeToNormal(__folder) {
    return __folder.replace("active", "normal");
}

function __toggleFolder(headObj) {
    var thisClass = getClass(headObj);
    if (thisClass.indexOf("normal") >= 0)
        thisClass = __normalToActive(thisClass);
    else
        thisClass = __activeToNormal(thisClass);
    setClass(headObj, thisClass);
}

function __togglePlusMinus(headObj) {
    var thisClass = getClass(headObj);
    var reClass = __plusToMinus(thisClass);
    if (thisClass == reClass)
        reClass = __minusToPlus(thisClass);
    setClass(headObj, reClass);
}

function __plusToMinus(__plus) {
    return __plus.replace("plus", "minus");
}
function __minusToPlus(__minus) {
    return __minus.replace("minus", "plus");
}