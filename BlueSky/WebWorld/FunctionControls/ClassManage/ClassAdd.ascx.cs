using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using DataBase;
using WebWorld.Class;

namespace WebWorld.FunctionControls.ClassManage
{
    public partial class ClassAdd : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strXmlClassDocument = hiddenClassXMLDocument.Value.Trim();
            if ("" == strXmlClassDocument)
            {
                PageUtil.PageAlert(this.Page, "请添加班级后保存");
                return;
            }
            XmlDocument xmlClass = new XmlDocument();
            try
            {
                xmlClass.LoadXml(string.Format("<Classes>{0}</Classes>" ,strXmlClassDocument));
            }
            catch(Exception ee)
            {
                PageUtil.PageAlert(this.Page, "保存失败!");
                return;
            }
            XmlNodeList classNodeList = xmlClass.GetElementsByTagName("Class");
            foreach (XmlNode classNode in classNodeList)
            {
                //添加主表记录——班级
                ClassItem classItem = new ClassItem();
                XmlNode nodeClassName = classNode.SelectSingleNode("ClassName");
                if (null == nodeClassName)
                    continue;
                string strClassName = nodeClassName.InnerText;
                if ("" == strClassName.Trim())
                    continue;
                classItem.ClassName = strClassName;

                XmlNode nodeNumber = classNode.SelectSingleNode("StudentNumber");
                if (null != nodeNumber)
                    classItem.StudentNumber = Util.ParseInt(nodeNumber.InnerText, 0);

                int nClassId = classItem.Save();
                if (nClassId <= 0)
                    continue;

                
                XmlNodeList studentNodeList = classNode.SelectNodes("Students/Student");
                foreach (XmlNode studentNode in studentNodeList)
                { 
                    //添加字表记录——学生
                    StudentItem studentItem = new StudentItem();
                    studentItem.ClassItemId = nClassId;

                    XmlNode nodeStudentName = studentNode.SelectSingleNode("Name");
                    if (null == nodeStudentName)
                        continue;
                    string strStudentName = nodeStudentName.InnerText;
                    if ("" == strStudentName.Trim())
                        continue;
                    studentItem.Name = strStudentName;

                    XmlNode nodeAge = studentNode.SelectSingleNode("Age");
                    if (null != nodeAge)
                        studentItem.Age = Util.ParseInt(nodeAge.InnerText, 0);
                    studentItem.Save();
                }
            }

            PageUtil.PageAppendScript(this.Page, "alert('保存成功！');window.location.href = 'Window.aspx?value=FunctionControls/ClassManage/ClassManage.ascx'");
        }
    }
}