<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClassAdd.ascx.cs" Inherits="WebWorld.FunctionControls.ClassManage.ClassAdd" %>
<script type="text/javascript">
    var listContainer = null;
    $(document).ready(function() {
        var docSize = Utils.documentSize(document);
        listContainer = document.getElementById("listContainer");
        listContainer.style.height = docSize.height - 26 + "px";
        hashTable[0] = new Array();
        addItem();
    });

    window.onresize = function() {
        var docSize = Utils.documentSize(document);
        listContainer.style.height = docSize.height - 26 + "px";
    }
</script>
<div class="content">
    <table  cellpadding="0" cellspacing="0" height="100%" width="100%">
        <tr>
            <td><div class="toolBar">&nbsp;<input type="button" class="btn-normal" onclick="window.location.href = 'Window.aspx?value=FunctionControls/ClassManage/ClassManage.ascx'" value=" 返 回 " />&nbsp;<input type="button" value="添加一行" class="btn-normal" onclick="addItem();" /><input id="btnSave" style="float:right;margin-right:10px;" type="button" value=" 保 存 " class="btn-normal" runat="server" onclick="if(!saveClasses()) return false;" onserverclick="btnSave_Click" /></div></td>
        </tr>
        <tr>
            <td valign="top">
                <div id="listContainer" style="overflow:auto;">
                    <table class="table-list-top" id="tbClassList" cellpadding="0" cellspacing="0" width="100%">
                        <tbody>
                        <tr>
                            <td align="center" class="td-header">序号</td>
                            <td align="center" class="td-header">班级名称</td>
                            <td align="center" class="td-header">人数</td>
                            <td align="center" colspan="2" class="td-header-last">操作</td>
                        </tr>
                        </tbody>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<input type="hidden" id="hiddenClassXMLDocument" runat="server" value="" />

<script type="text/javascript">
    var hashTable = new Array();
    function addItem() {
        var tbClassList = document.getElementById("tbClassList");
        var trClassNumber = tbClassList.getElementsByTagName("tr").length;
        var trClass = tbClassList.insertRow(trClassNumber);
        addItemCell(trClass, 0, "td-content", trClassNumber);
        addItemCell(trClass, 1, "td-content", "<input type='text' size='20' class='txt-normal' /><font class='font-hint'>*</font>");
        addItemCell(trClass, 2, "td-content", "<input type='text' size='10' class='txt-normal' />");
        addItemCell(trClass, 3, "td-content", "<a href='javascript:addStudents(" + trClassNumber + ");'>添加学生</a>");
        addItemCell(trClass, 4, "td-content-last", "<a href='javascript:deleteItem(" + trClassNumber + ");'>删除</a>");

        hashTable[trClassNumber] = new Array();
        
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
        var tbClassList = document.getElementById("tbClassList");
        var trRows = tbClassList.rows;
        var trRowsCount = trRows.length;
        tbClassList.deleteRow(_trIndex);
        delete hashTable[_trIndex];

        if (_trIndex == trRowsCount - 1)
            return;
        if (_trIndex < trRowsCount - 1) {
            for (var rowIndex = _trIndex; rowIndex < trRowsCount; rowIndex++) {
                trRows[rowIndex].cells[0].innerHTML = rowIndex;
                trRows[rowIndex].cells[3].innerHTML = "<a href='javascript:addStudents(" + rowIndex + ");'>添加学生</a>";
                trRows[rowIndex].cells[4].innerHTML = "<a href='javascript:deleteItem(" + rowIndex + ");'>删除</a>";

                hashTable[rowIndex] = hashTable[rowIndex + 1];
            }
        }
    }

    function addStudents(_trIndex) {
        var tbClassList = document.getElementById("tbClassList");
        var inputClassName = tbClassList.rows[_trIndex].cells[1].getElementsByTagName("input")[0];
        var strClassName = inputClassName.value;
        var classInfomation = { name: strClassName, students: hashTable[_trIndex] }
        var arrStudents = window.showModalDialog("FunctionControls/ClassManage/AddStudents.aspx", classInfomation, "status:no;help:no;edge:raised;dialogHeight:450px;dialogWidth:550px;");
        hashTable[_trIndex] = arrStudents;
    }

    function getCellInputValue(_trRef, _cellIndex) {
        var inputRef = _trRef.cells[_cellIndex].getElementsByTagName("input")[0];
        if (inputRef)
            return inputRef.value;
        return "";
    }

    function saveClasses() {
        debugger;
        var xmlClass = "";
        var tbClassList = document.getElementById("tbClassList");
        var trRows = tbClassList.rows;
        var trRowNumber = trRows.length;
        for (var trIndex = 1; trIndex < trRowNumber; trIndex++) {
            //此处可以对数据进行全面的验证
            var strClassName = getCellInputValue(trRows[trIndex], 1);
            if (!strClassName || strClassName.trim() == "") {
                if (hashTable[trIndex] && hashTable[trIndex] != "") {
                    if (confirm("第" + trIndex + "行班级已经录入学生信息，但没有录入班级名称，是否放弃？"))
                        continue;
                    else
                        return false;
                }
                continue;
            }
            var strStudentNumber = getCellInputValue(trRows[trIndex], 2);
            if (strStudentNumber && "" != strStudentNumber && isNaN(strStudentNumber)) {
                alert("班级人数只能填写数字！");
                return false;
            }
            xmlClass += "<Class>";
            xmlClass += "<ClassName>" + strClassName + "</ClassName>";
            xmlClass += "<StudentNumber>" + strStudentNumber + "</StudentNumber>";
            xmlClass += "<Students>" + createStudentNode(trIndex) + "</Students>";
            xmlClass += "</Class>";
        }
        if (xmlClass.trim() != "") {
            document.getElementById("<%=hiddenClassXMLDocument.ClientID %>").value = xmlClass;
            return true;
        }
        alert("请输入班级后再保存数据！");
        return false;
    }

    function createStudentNode(_trIndex) {
        var arrStudents = hashTable[_trIndex];
        if (null == arrStudents || arrStudents.length <= 0)
            return "";
        var xmlStudents = "";
        var nCount = arrStudents.length;
        for (var i = 0; i < nCount; i++) {
            var student = arrStudents[i];
            if (student) {
                xmlStudents += "<Student>";
                xmlStudents += "<Name>" + student.name + "</Name>";
                xmlStudents += "<Age>" + student.age + "</Age>";
                xmlStudents += "</Student>";
            }
        }
        return xmlStudents;
    }
</script>