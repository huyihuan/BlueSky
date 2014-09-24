<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActionImage.ascx.cs" Inherits="WebWorld.SystemManage.ActionImage" %>
<%@ Register TagPrefix="BS" Namespace="WebBase.UserControls" Assembly="WebBase" %>
<script type="text/javascript">
    $(document).ready(function() {
        window.formUtil.initList({ listObject: ".list-container", minusHeight: 62, resize: false });
        var imgNameExist = document.getElementById("<%=hiddenImageSelected.ClientID %>").value + "";
        if ("" != imgNameExist)
            selectedImage(imgNameExist);
        else {
            //调用window.windowArguments参数时，需要调用setTimeout函数来延时调用
            setTimeout(function() {
                if (null != window.arguments) {
                    selectedImage(window.arguments.imageName);
                }
            }, 200);
        }
    });

    function selectedImage(_imageName) {
        var imgSelect = document.getElementById("<%=img_Selected.ClientID %>");
        imgSelect.title = _imageName;
        imgSelect.src = "<%=strActionImagePath %>" + "\\" + _imageName;
        document.getElementById("<%=hiddenImageSelected.ClientID %>").value = _imageName;
    }
    function save(){
        var imgSelect = document.getElementById("<%=img_Selected.ClientID %>");
        window.dialogResult = { imageName: imgSelect.title, src: imgSelect.src };
        top.layout.closeActiveWindow();
    }
</script>
<style type="text/css">
    .action-image-box{width:40px;height:40px;float:left;}
    .action-image-box a{padding:10px;display:block;cursor:pointer;}
    .action-image-box a:hover{background-color:#EEEEEE;}
</style>
<table  cellpadding="0" cellspacing="0" height="100%" width="100%">
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td width="60%" align="left" valign="middle">
            &nbsp;<input type="button" value="返 回" class="btn-normal" onclick="top.layout.closeActiveWindow();"  />
            &nbsp;当前选择图片：<img id="img_Selected" style="width:16px;height:16px;" align="absMiddle" runat="server" src="" title="" />
        </td>
        <td width="40%" align="right"><input id="btn_OK" type="button" value="确 定" onclick="save();" class="btn-normal" />&nbsp;</td>
    </tr>
    <tr><td colspan="2" height="5"></td></tr>
    <tr>
        <td valign="top" colspan="2">
            <div class="list-container">
                <asp:Label ID="lbl_ImageList" runat="server"></asp:Label>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <BS:PagerNavication ID="PagerNavication" PageSizeList="10,30,50" PageSize="10" runat="server" OnPagerIndexChaged="PagerNavication_PagerIndexChanged"></BS:PagerNavication>
        </td>
    </tr>
</table>
<input type="hidden" runat="server" id="hiddenImageSelected" />