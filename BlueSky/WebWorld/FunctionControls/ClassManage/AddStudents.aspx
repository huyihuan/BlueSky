<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddStudents.aspx.cs" Inherits="WebWorld.FunctionControls.ClassManage.AddStudents" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="../../Include/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="../../Include/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Include/js/utils.js" type="text/javascript"></script>
    <script type="text/javascript">
        var saveFlag = false;
        $(document).ready(function() {
            var docSize = Utils.documentSize(document);
            var listContainer = document.getElementById("listContainer");
            listContainer.style.height = docSize.height - 26 + "px";
            var classInformation = window.dialogArguments;
            window.document.title = classInformation.name;
            var arrStudents = classInformation.students;
            if (arrStudents) {
                var nCount = arrStudents.length;
                for (var i = 0; i < nCount; i++) {
                    var student = arrStudents[i];
                    if (student) {
                        addItem(student.name, student.age);
                    }
                }
            }
            addItem();
        });

        window.onbeforeunload = function() {
            if (!saveFlag) {
                return "您将要离开本界面数据将不会保存，如要保存数据请点击【确定】按钮，是否确定离开？";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content">
    <table  cellpadding="0" cellspacing="0" height="100%" width="100%">
        <tr>
            <td><div class="toolBar">&nbsp;<input type="button" value=" 返 回 " class="btn-normal" onclick="window.returnValue=null;saveFlag=true;window.close();" />&nbsp;<input type="button" value="添加一行" class="btn-normal" onclick="addItem('','');" /><input type="button" style="float:right;margin-right:10px;" value=" 确 认 " class="btn-normal" onclick="saveStudents();"/></div></td>
        </tr>
        <tr>
            <td valign="top">
                <div id="listContainer" style="overflow:auto;">
                    <table class="table-list-top" id="tbStudentList" cellpadding="0" cellspacing="0" width="100%">
                        <tbody>
                        <tr>
                            <td align="center" class="td-header">序号</td>
                            <td align="center" class="td-header">学生姓名</td>
                            <td align="center" class="td-header-last">年龄</td>
                            <td align="center" class="td-header-last">操作</td>
                        </tr>
                        </tbody>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
    </form>
</body>
<script type="text/javascript">
    function addItem(_name, _age) {
        _name = _name ? _name : "";
        _age = _age ? _age : "";
        var tbStudentList = document.getElementById("tbStudentList");
        var trStudentNumber = tbStudentList.getElementsByTagName("tr").length;
        var trStudent = tbStudentList.insertRow(trStudentNumber);
        addItemCell(trStudent, 0, "td-content", trStudentNumber);
        addItemCell(trStudent, 1, "td-content", "<input type='text' size='20' class='txt-normal' value='" + _name + "' /><font class='font-hint'>*</font>");
        addItemCell(trStudent, 2, "td-content", "<input type='text' size='10' class='txt-normal' value='" + _age + "' />");
        addItemCell(trStudent, 3, "td-content-last", "<a href='javascript:deleteItem(" + trStudentNumber + ");'>删除</a>");
    }

    function addItemCell(_trRef, _index, _className, _innerHTML) {
        var tdCell = _trRef.insertCell(_index);
        tdCell.align = "center";
        tdCell.className = _className;
        tdCell.innerHTML = _innerHTML + "";
    }

    function deleteItem(_trIndex) {
        if (isNaN(_trIndex))
            return;
        var tbStudentList = document.getElementById("tbStudentList");
        var trRows = tbStudentList.rows;
        var trRowsCount = trRows.length;
        tbStudentList.deleteRow(_trIndex);

        if (_trIndex == trRowsCount - 1)
            return;
        if (_trIndex < trRowsCount - 1) {
            for (var rowIndex = _trIndex; rowIndex < trRowsCount; rowIndex++) {
                trRows[rowIndex].cells[0].innerHTML = rowIndex;
                trRows[rowIndex].cells[3].innerHTML = "<a href='javascript:deleteItem(" + rowIndex + ");'>删除</a>";
            }
        }
    }

    function saveStudents() {
        var arrStudents = new Array();
        var tbStudentList = document.getElementById("tbStudentList");
        var trRows = tbStudentList.rows;
        var trRowNumber = trRows.length;
        for (var trIndex = 1; trIndex < trRowNumber; trIndex++) {
            //此处可以对数据进行全面的验证
            var student = new Object();
            var sName = getCellInputValue(trRows[trIndex], 1);
            if (sName == undefined || sName.trim() == "")
                continue;
            var sAge = getCellInputValue(trRows[trIndex], 2);
            if (sAge && "" != sAge && isNaN(sAge)) {
                alert("学生年龄只能填写数字！");
                return;
            }
            student.name = sName;
            student.age = sAge;
            arrStudents[trIndex - 1] = student;
        }
        saveFlag = true;
        window.returnValue = arrStudents;
        window.close();
    }

    function getCellInputValue(_trRef, _cellIndex) {
        var inputRef = _trRef.cells[_cellIndex].getElementsByTagName("input")[0];
        if (inputRef)
            return inputRef.value;
        return "";
    }
</script>
</html>
