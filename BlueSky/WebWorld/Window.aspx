<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Window.aspx.cs" Inherits="WebWorld.Window" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="Include/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="Include/js/jquery.json.js" type="text/javascript"></script>
    <script src="Include/js/Bluesky.js" type="text/javascript"></script>
    <link href="Include/css/main.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/form.js" type="text/javascript"></script>
    <link href="Include/css/Bluesky.Masklayer.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/Bluesky.Masklayer.js" type="text/javascript"></script>
    <link href="Include/css/Bluesky.Window.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/Bluesky.Tabs.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/Bluesky.Tabs.js" type="text/javascript"></script>
    <script src="Include/js/utils.js" type="text/javascript"></script>
    <link href="Include/css/menu.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/menu.js" type="text/javascript"></script>
    <link href="Include/css/panel.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/panel.js" type="text/javascript"></script>
    <link href="Include/css/Bluesky.MessageBox.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/Bluesky.MessageBox.js" type="text/javascript"></script>
</head>
<body onload="setTimeout(function() { Bluesky.component.Masklayer.remove(); }, 100);">
    <form id="form1" runat="server" onsubmit="setTimeout(function() { Bluesky.component.Masklayer.loading(); }, 0);">
        <script type="text/javascript">setTimeout(function() { Bluesky.component.Masklayer.loading(); }, 0);</script>
        <asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>
        <input type="hidden" runat="server" id="hiddenSelectedValue" />
        <input type="hidden" runat="server" id="hiddenReturnUrl" />
        <input type="hidden" runat="server" id="hiddenExtraParameters" />
    </form>
</body>
</html>
