<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="WebWorld.SystemManage.Login" %>
<script type="text/javascript">
    function loginIn() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_UserName.ClientID %>", message: "账户名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Password.ClientID %>", message: "密码不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_VCode.ClientID %>", message: "验证码不能为空！", ishint: true }))
            return false;
        var strUserName = Bluesky("#<%=txt_UserName.ClientID %>").value();
        var strPassword = Bluesky("#<%=txt_Password.ClientID %>").value();
        var oVCode = Bluesky("#<%=txt_VCode.ClientID %>");
        Bluesky("#btnLogin").disabled(true).value("请稍后...");
        var ajax = Bluesky.Ajax({
            type: "get",
            url: "Server/SystemManage/Login.ashx",
            data: { action: "Login", uid: strUserName, pwd: strPassword, vcode: oVCode.value() },
            dataType: "json",
            success: function(response) {
                if (response.json.success == true) {
                    top.location.href = "Index.aspx";
                }
                else if (response.json.success == false) {
                    top.Bluesky.MessageBox.alert({
                        message: response.json.text,
                        callback: function() {
                            Bluesky("#vcode").click();
                            oVCode.focus().select();
                        }
                    });
                }
                else {
                    top.Bluesky.MessageBox.alert(response);
                }
                Bluesky("#btnLogin").disabled(false).value("登陆");
            },
            fail: function() {
                Bluesky("#btnLogin").disabled(false).value("登陆");
                ajax.abort();
                top.Bluesky.MessageBox.alert("请求失败！");
            }
        });
    }

    Bluesky.ready(function() {
        Bluesky("#<%=txt_UserName.ClientID %>").focus();
        setTimeout(function() {
            var tdCode = Bluesky("#td_vcode");
            if (!tdCode.firstChild) {
                tdCode.append(Bluesky.create("img", { id: "vcode", src: "Server/SystemManage/Login.ashx?action=VCode&r=" + Math.random(), title: "看不清，点击更换验证码！" }).addEvent("click", function() {
                    Bluesky("#vcode").attr("src", "Server/SystemManage/Login.ashx?action=VCode&r=" + Math.random());
                }));
            }
        },1000);
    });
</script>
<table cellpadding="0" cellspacing="8" width="100%">
    <tr><td colspan="2" height="20"></td></tr>
    <tr>
        <td width="35%" align="right" nowrap>用户名：</td>
        <td width="65%"><input type="text" id="txt_UserName" runat="server" class="txt-normal" style="width:140px;" tabindex="1" /></td>
    </tr>
    <tr>
        <td align="right" nowrap>密码：</td>
        <td><input type="password" id="txt_Password" runat="server" class="txt-normal" style="width:140px;"  tabindex="2"/></td>
    </tr>
    <tr>
        <td align="right" nowrap>验证码：</td>
        <td><input type="text" id="txt_VCode" runat="server" class="txt-normal" autocomplete="off" style="width:117px;" tabindex="3" /></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td id="td_vcode"></td>
    </tr>
    <tr>
        <td></td>
        <td>
            <input type="submit" id="btnLogin" class="btn-normal" onclick="if(!loginIn()) return false;" value=" 登 录 "/>
            <input type="reset" id="btnReset" value=" 重 置 " class="btn-normal" />
        </td>
    </tr>
</table>