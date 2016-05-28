<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesktopMain.aspx.cs" Inherits="WebWorld.DesktopMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="Include/css/main.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/desktopmain.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/window.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/graybox.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/graybox.js" type="text/javascript"></script>
    <script src="Include/js/window.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function() {
            var targetElement = window.top.document.getElementById("divTaskBar");
            bs_event_ShowTaskBar(window.top, targetElement);
            targetElement.style.height = __UnitAddPx(bs_field_TaskBarHeight + 1);
        }
        function showWindow(_url, _title, _width, _height) {
            bs_event_createWorkWindow(_url, _title, _width, _height);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%=strDesktopFunctions %>
    </div>
    </form>
</body>
</html>
