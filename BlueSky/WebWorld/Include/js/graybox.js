//各个层的垂直高度值z-index
//loadinglayer:9999;
//popWindowSelf-normal:9990;
//popWindowSelf-active:9991;
//TaskBar:9993;
//popWindowParent-normal:9995;
//popWindowParent-active:9996;
//popWindowTop-normal:9997;
//popWindowTop-active:9998;
var bs_field_loadingLayerId = "loadingLayer";
var bs_field_loadingLayerClass = "graybox-loading";
var bs_field_loadingImageUrl = "Include/image/loading.gif";
var bs_field_loadingImageWidth = 20;
var bs_field_loadingImageHeight = 20;
var bs_field_grayBoxClassName = "grayBox";
var bs_field_grayBoxId = "grayBox";
var bs_filed_grayBoxArray = new Array();

//show loading layer
function bs_event_showLoadingLayer(_targetDoc) {
    if (!_targetDoc || undefined == _targetDoc || null == _targetDoc)
        return false;
    var oldLayer = _targetDoc.getElementById(bs_field_loadingLayerId);
    if (oldLayer)
        return false;
    var loadLayer = _targetDoc.createElement("div");
    loadLayer.id = bs_field_loadingLayerId;
    loadLayer.className = bs_field_loadingLayerClass;
    
    var offsetwidth = _targetDoc.body.offsetWidth;
    var offsetheight = _targetDoc.body.offsetHeight;
    var loadImage = _targetDoc.createElement("img");
    loadImage.setAttribute("src", bs_field_loadingImageUrl);
    loadImage.style.marginLeft = (offsetwidth - bs_field_loadingImageWidth) / 2 + "px";
    loadImage.style.marginTop = (offsetheight - bs_field_loadingImageHeight) / 2 + "px";
    loadLayer.appendChild(loadImage);
    if (_targetDoc.body.childNodes.length == 0)
        _targetDoc.body.appendChild(grayBox);
    else
        _targetDoc.body.insertBefore(loadLayer, _targetDoc.body.childNodes[0]);
}
function bs_event_hideLoadingLayer(_targetDoc) {
    if (!_targetDoc || undefined == _targetDoc || null == _targetDoc)
        return false;
    var loadLayer = _targetDoc.getElementById(bs_field_loadingLayerId);
    if (loadLayer)
        _targetDoc.body.removeChild(loadLayer);
}


function bs_event_showGrayBox(_targetDoc) {
    if (!_targetDoc || undefined == _targetDoc || null == _targetDoc)
        return false;
    var grayBox = _targetDoc.createElement("div");
    grayBox.id = "grayBox" + (bs_filed_grayBoxArray.length + 1);
    grayBox.className = bs_field_grayBoxClassName;
    bs_filed_grayBoxArray[bs_filed_grayBoxArray.length] = grayBox.id;
    grayBox.style.zIndex = 9998 + (bs_filed_grayBoxArray.length - 1) * 2;
    if (_targetDoc.body.childNodes.length == 0)
        _targetDoc.body.appendChild(grayBox);
    else
        _targetDoc.body.insertBefore(grayBox, _targetDoc.body.childNodes[0]);
}
function bs_event_hideGrayBox(_targetDoc) {
    if (!_targetDoc || undefined == _targetDoc || null == _targetDoc)
        return false;
    var grayBox = _targetDoc.getElementById(bs_filed_grayBoxArray[bs_filed_grayBoxArray.length - 1]);
    if (grayBox) {
        _targetDoc.body.removeChild(grayBox);
        bs_filed_grayBoxArray.splice(bs_filed_grayBoxArray.length - 1, 1);
    }
}