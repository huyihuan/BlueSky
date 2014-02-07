<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="WebWorld.Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="Include/css/menu.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="Include/js/menu.js" type="text/javascript"></script>
    <link href="Include/css/tab.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/tabs.js" type="text/javascript"></script>
    <script src="Include/js/form.js" type="text/javascript"></script>

    <link href="Include/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function nodeClick(parentNode) {
            if (null == parentNode || undefined == parentNode)
                return;
            var nodeType = parentNode.getAttribute("_type");
            if(nodeType == "son") {
                var windowArguments = createWindowArguments(parentNode);
                top.goWindow(windowArguments);
            }
        }


        $(document).ready(function() {
            var tabContainer = new BlueSky.TabContainer();
            tabContainer.init({ tWindow: window, parent: "divTabContainer" });

            var tab = new BlueSky.Tab();
            tab.init({ tabContent: "导航",key:"navigation", frameTarget: "divMenuFrames", frameUrl: "", isClose: false, initActive: true });
            tabContainer.addTab(tab);

            var tab1 = new BlueSky.Tab();
            tab1.init({ tabContent: "日常", key: "usually", frameTarget: "divMenuFrames", frameUrl: "", isClose: false, initActive: false });
            tabContainer.addTab(tab1);
            var tab2 = new BlueSky.Tab();
            tab2.init({ tabContent: "邮箱", key: "email", frameTarget: "divMenuFrames", frameUrl: "", initActive: false });
            tabContainer.addTab(tab2);

            window.formUtil.initList({ listObject: ".treelist", minusHeight: 34, resize: true });
        });
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" height="100%">
    <tr><td height="5"></td></tr>
        <tr><td height="27"><div id="divTabContainer"></div><div></td></tr>
        <tr><td height="1"><div id="divMenuFrames" style="height:1px;overflow:hidden;"></div></td></tr>
        <tr><td height="100%"><asp:PlaceHolder ID="ph_MenuContainer" runat="server"></asp:PlaceHolder></td></tr>
    </table>
    </form>
</body>
</html>
