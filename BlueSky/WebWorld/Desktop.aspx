<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Desktop.aspx.cs" Inherits="WebWorld.Desktop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="Include/css/main.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/desktop.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/graybox.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/graybox.js" type="text/javascript"></script>
    <script src="Include/js/desktop.js" type="text/javascript"></script>

    <link href="Include/css/window.css" rel="stylesheet" type="text/css" />
</head>
<body scroll="no">
    <form id="form1" runat="server">
        <script type="text/javascript">
            bs_event_showLoadingLayer(document);
        </script>
        <div id="divDesktop">
            <iframe id="deskFrame" allowtransparency="true" name="daskFrame" frameborder="0" scrolling="no"></iframe>
        </div>
        <div id="divTaskBar"></div>
    </form>
</body>
</html>
