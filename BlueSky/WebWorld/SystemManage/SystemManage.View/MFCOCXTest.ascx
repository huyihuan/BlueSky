<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MFCOCXTest.ascx.cs" Inherits="WebWorld.SystemManage.SystemManage.View.MFCOCXTest" %>

<script type="text/javascript">
    function OCXFn() {
        var ocx = document.getElementById("OCXCOM");
        
        if (ocx) {
            ocx.PreviewOptions.AllowEdit = false;
            ocx.LoadPreparedReportFromFile("E:\\GitHub\\BlueSky\\BlueSky\\WebWorld\\Include\\html\\preparedfile.fp3");
            ocx.ShowPreparedReport();    
            //ocx.LoadReportFromFile("http:\\localhost:8011\\Include\\html\\xx.fr3");
            //ocx.ShowReport();
        }
        else
        {
            alert("no");
        }
    }
</script>
<object id="OCXCOM" classid="clsid:4764040E-4222-4DEC-9F2E-82D46E212B3A" width="0" height="0"></object>
<input type="button" value="hello" onclick="OCXFn();" />