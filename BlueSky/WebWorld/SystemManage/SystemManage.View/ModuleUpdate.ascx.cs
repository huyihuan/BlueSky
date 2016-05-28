using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebBase.Utilities;
using WebBase.SystemClass;
using System.Xml;

using System.Collections;
using BlueSky.Utilities;

namespace WebWorld.SystemManage
{
    public partial class ModuleUpdate : System.Web.UI.UserControl
    {
        //public int nId = -1;
        public List<string> ltModuleConfigName = new List<string>(new string[] { "Name", "Key", "Description", "Controller", "OrderId","IconName", "Functions" });
        public List<string> ltFnConfigName = new List<string>(new string[] { "Name", "Key", "Description", "IconName", "OrderId", "Actions" });
        public List<string> ltActionConfigName = new List<string>(new string[] { "Name", "Key", "Description", "IconName", "OrderId", "ActionType", "ActionValue", "IsDefault", "ControlName", "Tip", "Target", "EntityCount", "IsPopup", "Width", "Height", "IsResize", "IsMove", "IsIncludeMinBox", "IsIncludeMaxBox", "IsShowInTaskBar" });
        public List<string> ltActionConfigNotNull = new List<string>(new string[] { "Name", "Key", "Description", "IconName", "OrderId", "ActionType", "IsDefault", "ControlName", "Target", "EntityCount", "IsPopup" });
        protected void Page_Load(object sender, EventArgs e)
        {
            //nId = PageUtil.GetQueryInt(this.Request, "id", -1);
            //if (!IsPostBack)
            //{
            //    SystemModule oUpdate = SystemModule.Get(nId);
            //    if (null != oUpdate)
            //        lbl_ModuleInfomation.Text = string.Format("【{0}({1})】", oUpdate.Name, oUpdate.Controller);
            //}
        }

        private void _setMessage(string _strMsg, bool _bAppend)
        {
            if (!_bAppend)
                txt_UpdateResult.Value = "";
            if (_bAppend && txt_UpdateResult.Value != "")
                _strMsg = "\n —> " + _strMsg;
            txt_UpdateResult.Value += _strMsg;
        }

        protected void btn_UpdateModule_Click(object sender, EventArgs e)
        {
            txt_UpdateResult.Value = "";
            string strConfigFullName = file_ModuleConfig.Value;
            if (string.IsNullOrEmpty(strConfigFullName))
            {
                _setMessage("请先选择模块配置文件！", true);
                return;
            }
            string strFileName = System.IO.Path.GetFileName(strConfigFullName);
            string strFileEx = strFileName.Substring(strFileName.IndexOf("."));
            if (strFileEx != ".cfg.xml")
            {
                _setMessage("请上传文件后缀名称为“.cfg.xml”的配置文件！", true);
                return;
            }

            string strUploadPath = string.Format("{0}\\{1}", Server.MapPath(SystemUtil.GetVirtualSysUploadPath()), strFileName);
            try
            {
                file_ModuleConfig.PostedFile.SaveAs(strUploadPath);
                _setMessage("上传配置文件成功！", true);
            }
            catch (Exception ee)
            {
                _setMessage("上传配置文件失败！", true);
                _setMessage("程序终止！", true);
                return;
            }

            XmlDocument xmlModuleConfig = new XmlDocument();
            try
            {
                xmlModuleConfig.Load(strUploadPath);
                _setMessage("读取配置文件成功！", true);
            }
            catch (Exception ee)
            {
                _setMessage("读取配置文件失败！", true);
                _setMessage("程序终止！", true);
                return;
            }
            //模块配置文件有效性验证
            if (!_vModuleConfig(xmlModuleConfig))
                return;

            XmlNode moduleNode = xmlModuleConfig.SelectSingleNode("Module");
            //if (!_vModuleNode(moduleNode))
            //    return;
            Hashtable htFns = new Hashtable(); //该模块下的所有功能

            //配置模块基本信息
            string strModuleKey = _GetChildNodeText(moduleNode, "Key");
            SystemModule oModuleExist = SystemModule.Get(strModuleKey);
            bool bModuleAdd = null == oModuleExist;
            if (bModuleAdd)
            {
                oModuleExist = new SystemModule();
                oModuleExist.Key = strModuleKey;
            }
            oModuleExist.Name = _GetChildNodeText(moduleNode, "Name");
            oModuleExist.Description = _GetChildNodeText(moduleNode, "Description");
            oModuleExist.Controller = _GetChildNodeText(moduleNode, "Controller");
            oModuleExist.IconName = _GetChildNodeText(moduleNode, "IconName");
            oModuleExist.OrderId = TypeUtil.ParseInt(_GetChildNodeText(moduleNode, "OrderId"), 1);

            int nModuleId = SystemModule.Save(oModuleExist);
            _setMessage("模块信息保存" + (nModuleId <= 0 ? "失败！" : "成功！"), true);
            if(nModuleId <= 0)
                return;

            //检测模块功能根节点是否自动新增
            SystemFunction rootFn = SystemFunction.Get(strModuleKey);
            if (null == rootFn)
            {
                _setMessage(string.Format("模块根节点不存在！"), true);
                return;
            }

            //配置模块功能信息
            XmlNodeList nlFnsRoot = moduleNode.SelectNodes("Functions/Function");
            foreach (XmlNode nodeRootFn in nlFnsRoot)
            {
                //if (!_vFnNode(nodeRootFn))
                //    return;
                string strFnKey = _GetChildNodeText(nodeRootFn, "Key");
                SystemFunction oFnExist = SystemFunction.Get(strFnKey);
                bool bFnAdd = null == oFnExist;
                if (bFnAdd)
                {
                    oFnExist = new SystemFunction();
                    oFnExist.Key = strFnKey;
                    oFnExist.Level = 2;
                }
                oFnExist.ParentId = rootFn.Id;
                oFnExist.ModuleId = nModuleId;
                oFnExist.Name = _GetChildNodeText(nodeRootFn, "Name");
                oFnExist.Description = _GetChildNodeText(nodeRootFn, "Description");
                oFnExist.IconName = _GetChildNodeText(nodeRootFn, "IconName");
                oFnExist.OrderId = TypeUtil.ParseInt(_GetChildNodeText(nodeRootFn, "OrderId"), 1);
                int nFnId = SystemFunction.Save(oFnExist);
                if (nFnId <= 0)
                {
                    _setMessage(string.Format("功能“{0}”信息保存失败！", oFnExist.Name), true);
                    continue;
                }
                _setMessage(string.Format("功能“{0}”信息保存成功！", oFnExist.Name), true);
                if (!bModuleAdd)
                {
                    htFns[strFnKey] = 1;
                }
                //功能Action信息保存
                Hashtable htActions = new Hashtable();
                XmlNodeList nlActions = nodeRootFn.SelectNodes("Actions/Action");
                foreach (XmlNode nodeAction in nlActions)
                {
                    //if (!_vActionNode(nodeAction))
                    //    return;
                    string strActionKey = _GetChildNodeText(nodeAction, "Key");
                    SystemAction oActionExist = SystemAction.Get(strActionKey);
                    bool bActionAdd = null == oActionExist;
                    if (bActionAdd)
                    {
                        oActionExist = new SystemAction();
                        oActionExist.Key = strActionKey;
                    }
                    oActionExist.FunctionId = nFnId;
                    oActionExist.Name = _GetChildNodeText(nodeAction, "Name");
                    oActionExist.Description = _GetChildNodeText(nodeAction, "Description");
                    oActionExist.ActionType = _GetChildNodeText(nodeAction, "ActionType");
                    oActionExist.ActionValue = _GetChildNodeText(nodeAction, "ActionValue");
                    oActionExist.IsDefault = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "IsDefault"), 2);
                    oActionExist.ControlName = _GetChildNodeText(nodeAction, "ControlName");
                    oActionExist.Tip = _GetChildNodeText(nodeAction, "Tip");
                    oActionExist.IconName = _GetChildNodeText(nodeAction, "IconName");
                    oActionExist.Target = _GetChildNodeText(nodeAction, "Target");
                    oActionExist.EntityCount = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "EntityCount"), 0);
                    oActionExist.IsPopup = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "IsPopup"), 2);
                    oActionExist.Width = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "Width"), 0);
                    oActionExist.Height = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "Height"), 0);
                    oActionExist.IsResize = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "IsResize"), 2);
                    oActionExist.IsMove = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "IsMove"), 2);
                    oActionExist.IsIncludeMinBox = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "IsIncludeMinBox"), 2);
                    oActionExist.IsIncludeMaxBox = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "IsIncludeMaxBox"), 2);
                    oActionExist.IsShowInTaskBar = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "IsShowInTaskBar"), 2);
                    oActionExist.OrderId = TypeUtil.ParseInt(_GetChildNodeText(nodeAction, "OrderId"), 1);
                    int nActionId = SystemAction.Save(oActionExist);
                    if (nActionId <= 0)
                    {
                        _setMessage(string.Format("功能“{0}”的动作“{1}”信息保存失败！", oFnExist.Name, oActionExist.Name), true);
                        continue;
                    }
                    if (!bFnAdd)
                    {
                        htActions[strActionKey] = 1;
                    }
                }

                //删除该功能下的冗余操作（Action）
                if (!bFnAdd)
                {
                    SystemAction[] alAtions = SystemAction.GetFunctionAction(nFnId);
                    foreach (SystemAction action in alAtions)
                    {
                        if (!htActions.ContainsKey(action.Key))
                            SystemAction.Delete(action.Id);
                    }
                    _setMessage(string.Format("删除功能【{0}】冗余操作(Action)完成！", oFnExist.Name), true);
                }
            }

            ////删除该模块下的冗余功能（Function）
            if (!bModuleAdd)
            { 
                SystemFunction moduleRootFn = SystemFunction.Get(strModuleKey);
                if(null == moduleRootFn)
                {
                    _setMessage("删除模块冗余功能(Function)失败！", true);
                    return;
                }

                SystemFunction[] alFns = SystemFunction.GetFunctions(moduleRootFn.Id, false);
                foreach (SystemFunction function in alFns)
                {
                    if (!htFns.ContainsKey(function.Key))
                        SystemFunction.Delete(function.Id);
                }
                _setMessage("删除模块冗余功能(Function)成功！", true);
            }

            _setMessage("模块配置完成，请刷新浏览器！", true);
        }

        private bool _vModuleConfig(XmlDocument _xmlModuleConfig)
        {
            XmlNode moduleNode = _xmlModuleConfig.SelectSingleNode("Module");
            if (!_vModuleNode(moduleNode))
                return false;
            XmlNodeList nlFnsRoot = moduleNode.SelectNodes("Functions/Function");
            foreach (XmlNode nodeRootFn in nlFnsRoot)
            {
                if (!_vFnNode(nodeRootFn))
                    return false;
                XmlNodeList nlActions = nodeRootFn.SelectNodes("Actions/Action");
                foreach (XmlNode nodeAction in nlActions)
                {
                    if (!_vActionNode(nodeAction))
                        return false;
                }
            }
            return true;
        }

        private bool _vModuleNode(XmlNode _nodeModule)
        {
            if (_nodeModule.ChildNodes.Count != ltModuleConfigName.Count)
            {
                _setMessage("模块配置参数个数不正确！", true);
                return false;
            }
            XmlNodeList moduleConfigs = _nodeModule.ChildNodes;
            foreach (XmlNode node in moduleConfigs)
            {
                if (!ltModuleConfigName.Contains(node.Name))
                {
                    _setMessage(string.Format("未知模块配置参数“{0}”！", node.Name), true);
                    return false;
                }
                if (string.IsNullOrEmpty(node.InnerText.Trim()))
                {
                    _setMessage(string.Format("模块配置参数“{0}”的值为空！", node.Name), true);
                    return false;
                }
            }
            return true;
        }

        private bool _vFnNode(XmlNode _nodeFn)
        {
            if (_nodeFn.ChildNodes.Count != ltFnConfigName.Count)
            {
                _setMessage("功能配置参数个数不正确！", true);
                return false;
            }
            XmlNodeList fnConfigs = _nodeFn.ChildNodes;
            foreach (XmlNode node in fnConfigs)
            {
                if (!ltFnConfigName.Contains(node.Name))
                {
                    _setMessage(string.Format("未知功能配置参数“{0}”！", node.Name), true);
                    return false;
                }
                if (string.IsNullOrEmpty(node.InnerText.Trim()))
                {
                    _setMessage(string.Format("功能配置参数“{0}”的值为空！", node.Name), true);
                    return false;
                }
            }
            return true;
        }

        private bool _vActionNode(XmlNode _nodeAction)
        {
            if (_nodeAction.ChildNodes.Count != ltActionConfigName.Count)
            {
                _setMessage("动作配置参数个数不正确！", true);
                return false;
            }
            XmlNodeList actionConfigs = _nodeAction.ChildNodes;
            foreach (XmlNode node in actionConfigs)
            {
                if (!ltActionConfigName.Contains(node.Name))
                {
                    _setMessage(string.Format("未知动作配置参数“{0}”！", node.Name), true);
                    return false;
                }
                if (ltActionConfigNotNull.Contains(node.InnerText.Trim()) && string.IsNullOrEmpty(node.InnerText.Trim()))
                {
                    _setMessage(string.Format("动作配置参数“{0}”的值为空！", node.Name), true);
                    return false;
                }
            }
            return true;
        }

        private string _GetChildNodeText(XmlNode _parentNode, string _strName)
        {
            return _parentNode.SelectSingleNode(_strName).InnerText.Trim();
        }
    }
}