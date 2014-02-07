<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CodeTest.ascx.cs" Inherits="WebWorld.FunctionControls.CodeTest" %>
<%@ Register TagPrefix="BS" Namespace="DataBase.UserControls" Assembly="DataBase" %>
<div class="content">
    <table  cellpadding="0" cellspacing="0" height="100%" width="100%">
    <tr height="100%">
        <td>
            <table cellpadding="5" cellspacing="5" width="100%">
                <tr><td colspan="4"></td></tr>
                <tr>
                    <td colspan="4" align="center">
                        <input type="button" class="btn-normal" id="btnConnect" runat="server" onserverclick="btnConnect_Click" value="转换为日期" />
                        <input type="button" class="btn-normal" id="btnTicks" runat="server" onserverclick="btnTicks_Click" value="转换为Ticks" />
                        <input type="button" class="btn-normal" id="btnMD5" runat="server" onserverclick="btnMD5_Click" value="MD5" />
                        <input type="button" class="btn-normal" id="btnSubmit" onclick="fnSubmit();" value="确认窗口" />
                    </td>
                </tr>
                <tr>
                    <td nowrap align="center" valign="middle">输入：</td>
                    <td><textarea id="txt_Input" runat="server" rows="15" cols="15"></textarea></td>
                    <td nowrap align="center" valign="middle">输出：</td>
                    <td><textarea id="txt_Output" runat="server" rows="15" cols="15"></textarea></td>
                </tr>
            </table>
        </td>
    </tr>
    </table>
</div>
<script language="javascript">
    function fnSubmit() {
        top.windowFactory.targetConfirm(window, "测试窗口所了解对方撒娇的发送的发送大是大非设定法撒旦法阿萨德分数段发生的防撒旦分数段发生的设定发生的f", function() { document.getElementById("txt_test").value = "确定" });
    }
</script>