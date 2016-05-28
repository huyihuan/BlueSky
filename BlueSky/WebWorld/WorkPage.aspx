<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkPage.aspx.cs" Inherits="WebWorld.WorkPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>工作区</title>
    <script src="BlueSky/Bluesky.js" type="text/javascript"></script>
    <link href="BlueSky/Bluesky.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/Bluesky.Window.css" rel="stylesheet" type="text/css" />
    <link href="Include/css/Bluesky.Masklayer.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/Bluesky.Masklayer.js" type="text/javascript"></script>
    <link href="Include/css/tab.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/tabs.js" type="text/javascript"></script>
    <link href="Include/css/panel.css" rel="stylesheet" type="text/css" />
    <script src="Include/js/panel.js" type="text/javascript"></script>
    <script src="Include/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            var panel = new BlueSky.Panel();
            panel.init({
                parent: "panel1",
                width: "100%",
                height: "100%",
                splitPanel: [{ title: "MSDN", url: "http://m.cnblogs.com"}]
            });

            var panel2 = new BlueSky.Panel();
            panel2.init({
                parent: "panel2",
                width: "100%",
                height: "100%",
                splitPanel: [{ title: "博客园", url: "http://m.cnblogs.com"}]
            });

            var panel3 = new BlueSky.Panel();
            panel3.init({
                parent: "panel3",
                width: "100%",
                height: "100%",
                splitPanel: [{ title: "百度新闻", url: "http://m.cnblogs.com"}]
            });

            delete panel;
            delete panel2;
            delete panel3;
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="box-scroll">
            <table width="100%" height="100%" cellpadding="4" cellspacing="4" border="0">
                <tr>
                    <td id="panel1" height="50%" width="50%"></td>
                    <td id="panel2" height="50%" width="50%"></td>
                </tr>
                <tr><td colspan="2"  id="panel3"></td></tr>
            </table>
        </div>
    </form>
</body>
</html>
