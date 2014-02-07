<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="WebWorld.Modules.CommonSystemManage.Login" %>
<script type="text/javascript">
    function getVCode(_imgRef) {
        if (_imgRef)
            _imgRef.src = "/Include/html/VCode.aspx?rnd=" + Math.random();
    }

    function loginIn() {
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_UserName.ClientID %>", message: "账户名称不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_Password.ClientID %>", message: "密码不能为空！", ishint: true }))
            return false;
        if (!Utils.vText({ vtype: Utils.vType.Empty, vid: "<%=txt_VCode.ClientID %>", message: "验证码不能为空！", ishint: true }))
            return false;
        return true;
    }
</script>
<table cellpadding="0" cellspacing="5" width="100%">
    <tr><td colspan="2" height="20"></td></tr>
    <tr>
        <td align="right" nowrap>用户名：</td>
        <td><input type="text" id="txt_UserName" runat="server" class="txt-normal" style="width:140px;" /></td>
    </tr>
    <tr>
        <td align="right" nowrap>密码：</td>
        <td><input type="password" id="txt_Password" runat="server" class="txt-normal" style="width:140px;" /></td>
    </tr>
    <tr>
        <td align="right" nowrap>验证码：</td>
        <td><input type="text" id="txt_VCode" runat="server" class="txt-normal"  style="width:80px;" /><img onclick="getVCode(this);" title="看不清？点击更换验证码！" src="/Include/html/VCode.aspx" class="img-vcode" /></td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td></td>
        <td>
            <input type="button" id="btnLogin" runat="server" class="btn-normal" onclick="if(!loginIn()) return false;" value="登录" onserverclick="btnLogin_Click" />
            <input type="reset" id="btnReset" value="重置" class="btn-normal" />
        </td>
    </tr>
</table>