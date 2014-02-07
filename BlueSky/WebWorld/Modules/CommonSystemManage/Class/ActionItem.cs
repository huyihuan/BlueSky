using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebWorld.Modules.CommonSystemManage.Class
{
    public class ActionItem : DataBase.Interface.IEntity
    {
        public string GetTableName() { return "ActionItem"; }
        public string GetKeyName() { return "Id"; }
    }
}
