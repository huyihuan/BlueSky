<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="WebWorld.Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="BlueSky/Bluesky.js" type="text/javascript"></script>
    <link href="Include/css/menu.css" rel="stylesheet" type="text/css" />
    <link href="BlueSky/Bluesky.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="Include/js/menu.js" type="text/javascript"></script>
    <script src="BlueSky/plugins/Bluesky.Tabs.js" type="text/javascript"></script>
    <link href="BlueSky/themes/gray/css/Bluesky.Tabs.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/form.js" type="text/javascript"></script>

    <script type="text/javascript">
        function nodeClick(_node) {
            if (undefined == _node)
                return;
            var nodeType = Bluesky(_node).attr("_type");
            if(nodeType == "son") {
                var windowArguments = createWindowArguments(_node);
                top.layout.goWindow(windowArguments);
            }
        }
        var tabsWrapper,tabs;
        //Bluesky.ready(function() {
        window.onload = function() {
            tabsWrapper = Bluesky("#tabs");
            tabs = Bluesky.component.Tabs.create({
                renderTo: "#tabs",
                width: tabsWrapper.width(),
                height: tabsWrapper.height(),
                activeIndex: 0,
                items: [
			                {
			                    title: "",
			                    tip: "系统功能",
			                    sliding: true,
			                    closeable: false,
			                    showIcon: true,
			                    iconURL: "include/image/icons/house.png",
			                    contentElement: "#tab_nav"
			                },
			                {
			                    title: "",
			                    tip: "日常办公",
			                    sliding: true,
			                    closeable: false,
			                    showIcon: true,
			                    iconURL: "include/image/icons/clock.png",
			                    contentElement: "#tab_work"
			                },
			                {
			                    title: "",
			                    tip: "邮箱",
			                    sliding: true,
			                    closeable: false,
			                    showIcon: true,
			                    iconURL: "include/image/icons/email.png",
			                    contentElement: "#tab_email"
			                }
                        ]
            });
        };

        Bluesky(window).addEvent("resize", function() {
            tabs.resize({ width: tabsWrapper.width(), height: tabsWrapper.height() });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="tabs" class="box-fixed"></div>
    <div id="tab_nav" class="box-fixed"><asp:PlaceHolder ID="ph_MenuContainer" runat="server"></asp:PlaceHolder></div>
    <div id="tab_work" class="box-fixed">工作相关功能</div>
    <div id="tab_email" class="box-fixed">邮箱</div>
    </form>
</body>
</html>
