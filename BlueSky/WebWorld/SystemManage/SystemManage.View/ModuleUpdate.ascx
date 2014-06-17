<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleUpdate.ascx.cs" Inherits="WebWorld.SystemManage.ModuleUpdate" %>
<table  cellpadding="0" cellspacing="10">
    <tr>
        <td>
            模块配置文件：<input type="file" id="file_ModuleConfig" class="txt-file" runat="server" style="width:415px;" title="请选择模块的配置文件（*.cfg.xml）！" />&nbsp;
            <input type="button" id="btn_UpdateModule" runat="server" onserverclick="btn_UpdateModule_Click" value="更 新" class="btn-normal" />
        </td>
    </tr>
    <tr>
        <td>
            <textarea id="txt_UpdateResult" class="txt-mutil" runat="server" style="width:570px;height:280px;" value="更新结果..." readonly="readonly"></textarea>
        </td>
    </tr>
</table>