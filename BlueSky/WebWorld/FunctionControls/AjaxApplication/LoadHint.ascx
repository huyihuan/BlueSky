<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadHint.ascx.cs" Inherits="WebWorld.FunctionControls.AjaxApplication.LoadHint" %>
<script type="text/javascript" src="../../Include/js/ajax.js"></script>
<script type="text/javascript">
    function loadImage() {
        var xhr = new xmlHttpRequest();
        if (null == xhr || typeof xhr == undefined)
            return;
        xhr.onreadystatechange = xmlReadyStateChange;
        xhr.open("get", "Default.html", true);
        xhr.send(null);
    }

    function xmlReadyStateChange() {
        if (this.readyState == 3) {
            var contentLength = this.getResponseHeader("Content-Length");
            if (!confirm("您将从服务器下载的资源大小 " + contentLength + " bytes,确定下载？")) {
                this.abort();
                return;
            }
        }
    }

    function getFunctions() {
        $.ajax({
            type: "GET",
            sync: true,
            url: "FunctionControls/AjaxApplication/FunctionHandler.ashx",
            success: function(_json) {
                var dataObj = JSON.parse(_json);
                alert(dataObj.Name);
                var strJson = JSON.stringify(dataObj);
                alert(strJson);
            }
        });
    }
</script>
<input type="button" value="下载图片" onclick="loadImage();" />
<input type="button" value="获取Json" onclick="getFunctions();" />
