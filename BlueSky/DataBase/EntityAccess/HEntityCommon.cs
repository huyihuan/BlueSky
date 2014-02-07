using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
using DataBase.DBHelper;
using System.Data;
using System.Collections;

namespace DataBase
{
    public class HEntityCommon
    {
        static string[] alInt = new string[] { "int32", "int", "double", "long" };
        static string[] alString = new string[] { "string", "datetime" };

        //private object __currentObj;
        //private Type __currentType;
        //private FieldInfo[] __alFields;
        //private string __currentKeyName;
        //private object __currentKeyValue;
        //private string __currentTableName;

        //public object CurrentObj
        //{
        //    get { return this.__currentObj; }
        //    set { this.__currentObj = value; }
        //}
        //public Type CurrentType
        //{
        //    get { return this.__currentType; }
        //    set { this.__currentType = value; }
        //}
        //public FieldInfo[] CurrentFields
        //{
        //    get { return this.__alFields; }
        //    set { this.__alFields = value; }
        //}
        //public string CurrentKeyName
        //{
        //    get { return this.__currentKeyName; }
        //    set { this.__currentKeyName = value; }
        //}
        //public object CurrentKeyValue
        //{
        //    get { return this.__currentKeyValue; }
        //    set { this.__currentKeyValue = value; }
        //}
        //public string CurrentTableName
        //{
        //    get { return this.__currentTableName; }
        //    set { this.__currentTableName = value; }
        //}

        private ActiveEntity _activeEntity;

        public HEntityCommon()
        { }

        public HEntityCommon(object _oE)
        {
            if (!(_oE is Interface.IEntity))
                return;
            this._activeEntity = new ActiveEntity(_oE);
        }

        public static HEntityCommon HEntity(object _oE)
        {
            if (null == _oE)
                return null;
            HEntityCommon oEntity = new HEntityCommon(_oE);
            return oEntity;
        }

        #region 私有方法

        private static object[] ChangeTableToEntitys(HEntityCommon _oE, DataTable __dt)
        {
            if (null == __dt || __dt.Rows.Count == 0)
                return null;
            int nRowNum = __dt.Rows.Count;
            ArrayList alItems = new ArrayList();
            for (int i = 0; i < nRowNum; i++)
            {
                DataRow rowItem = __dt.Rows[i];
                object oInstance = Activator.CreateInstance(_oE._activeEntity.ObjectType);
                foreach (FieldInfo f in _oE._activeEntity.ObjectFields)
                {
                    if (rowItem[f.Name] is DBNull)
                        f.SetValue(oInstance, null);
                    else
                        f.SetValue(oInstance, rowItem[f.Name]);
                }
                alItems.Add(oInstance);
            }
            return (object[])alItems.ToArray(_oE._activeEntity.ObjectType);
        }

        private string _GetParamter(object _oValue, FieldInfo _field)
        {
            string strName = _field.Name;
            string strTpName = _field.FieldType.Name.ToLower();
            //数据库中int默认为0，所以当int型字段为0时跳过，不添加入参数列表
            if (strTpName == "int32" && (int)_oValue == 0)
            {
                return null;
            }
            if (alInt.Contains(strTpName))
            {
                return string.Format("[{0}]={1}", strName, _oValue);
            }
            else if (alString.Contains(strTpName))
            {
                return string.Format("[{0}]='{1}'", strName, _oValue);
            }
            return "";
        }

        private string _CreateSelectWhere(HEntityCommon _oE)
        {
            int nFieldNum = _oE._activeEntity.ObjectFields.Length;
            StringBuilder sb = new StringBuilder("1=1");
            for (int i = 0; i < nFieldNum; i++)
            {
                object oValue = _oE._activeEntity.ObjectFields[i].GetValue(_oE._activeEntity.ActiveObject);
                if (null != oValue)
                {
                    string strSingleWhere = _GetParamter(oValue, _oE._activeEntity.ObjectFields[i]);
                    if (!string.IsNullOrEmpty(strSingleWhere))
                        sb.Append(string.Format(" and {0}", strSingleWhere));
                }
            }
            return sb.ToString();
        }

        #endregion

        public object Instance()
        {
            object oI = Activator.CreateInstance(this._activeEntity.ObjectType);
            return oI;
        }

        public object EntityClone()
        {
            object oClone = Activator.CreateInstance(this._activeEntity.ObjectType);
            foreach (FieldInfo f in this._activeEntity.ObjectFields)
            {
                object oValue = f.GetValue(this._activeEntity.ActiveObject);
                f.SetValue(oClone, oValue);
            }
            return oClone;
        }

        public int EntityCount()
        {
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = "count(1)";
            query.SqlWhereString = _CreateSelectWhere(this);
            object oCount = HDBOperation.QueryScalar(query.ToString());
            return (int)oCount;
        }

        public int EntityCount(string _strFilter)
        {
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = "count(1)";
            query.SqlWhereString = _strFilter;
            object oCount = HDBOperation.QueryScalar(query.ToString());
            return (int)oCount;
        }

        public object[] EntityList()
        {
            string strWhere = _CreateSelectWhere(this);
            string strCacheKey = string.Format("{0}-{1}", this._activeEntity.TableName, strWhere);
            if (CacheUtil.IsExistCache(strCacheKey))
            {
                object oCache = CacheUtil.GetValue(strCacheKey);
                if (null != oCache)
                    return (object[])oCache;
            }
            List<string> lt = new List<string>();
            for (int i = 0; i < this._activeEntity.ObjectFields.Length; i++)
                lt.Add(string.Format("[{0}]", this._activeEntity.ObjectFields[i].Name));
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = string.Join(",", lt.ToArray());
            query.SqlWhereString = strWhere;
            DataSet ds = HDBOperation.QueryDataSet(query.ToString());
            object[] alObjects = ChangeTableToEntitys(this, ds.Tables[0]);
            CacheUtil.SetValue(this._activeEntity.TableName, alObjects);
            return alObjects;
        }

        public object[] EntityList(string _strFilter)
        {
            string strCacheKey = string.Format("{0}-{1}", this._activeEntity.TableName, _strFilter);
            if (CacheUtil.IsExistCache(strCacheKey))
            {
                object oCache = CacheUtil.GetValue(strCacheKey);
                if (null != oCache)
                    return (object[])oCache;
            }
            List<string> lt = new List<string>();
            for (int i = 0; i < this._activeEntity.ObjectFields.Length; i++)
                lt.Add(string.Format("[{0}]", this._activeEntity.ObjectFields[i].Name));
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = string.Join(",", lt.ToArray());
            query.SqlWhereString = _strFilter;
            DataSet ds = HDBOperation.QueryDataSet(query.ToString());
            object[] alObjects = ChangeTableToEntitys(this, ds.Tables[0]);
            CacheUtil.SetValue(strCacheKey, alObjects);
            return alObjects;
        }

        public object[] EntityList(string _strFilter, string _strSort)
        {
            if (!string.IsNullOrEmpty(_strSort))
                _strFilter += string.Format(" order by {0}", _strSort.Trim());
            return EntityList(_strFilter);
        }

        public object[] EntityList(string _strFilter, string _strSort, int _nPageIndex, int _nPageSize)
        {
            string strCacheKey = string.Format("{0}-{1}-{2}-{3}", this._activeEntity.TableName, _strFilter, _nPageIndex, _nPageSize);
            if (CacheUtil.IsExistCache(strCacheKey))
            {
                object oCache = CacheUtil.GetValue(strCacheKey);
                if (null != oCache)
                    return (object[])oCache;
            }
            List<string> lt = new List<string>();
            for (int i = 0; i < this._activeEntity.ObjectFields.Length; i++)
                lt.Add(string.Format("[{0}]", this._activeEntity.ObjectFields[i].Name));
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;

            #region 分页方案(not in),在百万条数据内效率高
            //query.SqlContent = string.Format("top {0} {1}", _nPageSize, string.Join(",", lt.ToArray()));
            //if (!string.IsNullOrEmpty(_strFilter))
            //    query.SqlWhereString += _strFilter + " and ";
            //query.SqlWhereString += string.Format("{0} not in (select top (({1} - 1) * {2}) {0} from {3} order by {0} )", this._activeEntity.KeyName, _nPageIndex, _nPageSize, this._activeEntity.TableName);
            //if (!string.IsNullOrEmpty(_strSort))
            //    query.SqlWhereString += " order by " + _strSort;
            #endregion

            #region 分页方案(ROW_NUMBER()),在百万条数据以上效率高
            query.SqlContent = string.Format("{0} from (select ROW_NUMBER() over(order by {1}) as nRow,{0}", string.Join(",", lt.ToArray()), this._activeEntity.KeyName);
            if (!string.IsNullOrEmpty(_strFilter))
                query.SqlWhereString += string.Format("where {0} ", _strFilter);
            query.SqlWhereString += string.Format(") xTemp where nRow >= (({0} - 1) * {1} + 1) and nRow <= ({0}*{1})", _nPageIndex, _nPageSize);
            if (!string.IsNullOrEmpty(_strSort))
                query.SqlWhereString += " order by " + _strSort;
            #endregion

            DataSet ds = HDBOperation.QueryDataSet(query.ToString());
            object[] alObjects = ChangeTableToEntitys(this, ds.Tables[0]);
            CacheUtil.SetValue(strCacheKey, alObjects);
            return alObjects;
        }

        public int EntitySave()
        {
            bool IsEntity = this._activeEntity.ActiveObject is DataBase.Interface.IEntity;
            if (!IsEntity || null == this._activeEntity.KeyName || "" == this._activeEntity.KeyName)
                return -1;
            int nKeyValue = Util.ParseInt(this._activeEntity.KeyValue + "", -1);
            if (nKeyValue <= 0)
            {
                //insert
                int nNum = this._activeEntity.ObjectFields.Length;
                if (nNum == 0)
                    return -1;
                int nMaxId = GetNextKey();
                StringBuilder sbContent = new StringBuilder();
                StringBuilder sbWhere = new StringBuilder();
                string strSingle = "";
                for (int i = 0; i < nNum; i++)
                {
                    string strFieldName = this._activeEntity.ObjectFields[i].Name;
                    if (strFieldName != this._activeEntity.KeyName)
                    {
                        object oValue = this._activeEntity.ObjectFields[i].GetValue(this._activeEntity.ActiveObject);
                        if (null == oValue) oValue = "null";
                        strSingle = alInt.Contains(this._activeEntity.ObjectFields[i].FieldType.Name.ToLower()) ? oValue + "" : string.Format("'{0}'", oValue);
                    }
                    else
                    {
                        strSingle = nMaxId + "";
                        this._activeEntity.ObjectFields[i].SetValue(this._activeEntity.ActiveObject, nMaxId);
                    }
                    if (0 != i)
                    {
                        sbContent.Append(",");
                        sbWhere.Append(",");
                    }
                    sbContent.Append(string.Format("[{0}]", strFieldName));
                    sbWhere.Append(strSingle);
                }
                HSqlFactory query = new HSqlFactory();
                query.SqlType = HSqlType.Insert;
                query.SqlTableName = this._activeEntity.TableName;
                query.SqlContent = sbContent.ToString();
                query.SqlWhereString = sbWhere.ToString();
                try
                {
                    HDBOperation.QueryNonQuery(query.ToString());
                }
                catch (Exception ee)
                {
                    return -1;
                }
                nKeyValue = nMaxId;
            }
            else
            {
                //old obj to update
                int nNum = this._activeEntity.ObjectFields.Length;
                if (nNum == 0)
                {
                    return -1;
                }
                string strUpdateKey = string.Format("{0}={1}", this._activeEntity.KeyName, this._activeEntity.KeyValue);
                StringBuilder sb = new StringBuilder();
                string strSingle = "";
                for (int i = 0; i < nNum; i++)
                {
                    string strFieldName = this._activeEntity.ObjectFields[i].Name;
                    if (strFieldName == this._activeEntity.KeyName)
                        continue;
                    object oValue = this._activeEntity.ObjectFields[i].GetValue(this._activeEntity.ActiveObject);
                    if (null == oValue) oValue = "null";
                    strSingle = alInt.Contains(this._activeEntity.ObjectFields[i].FieldType.Name.ToLower()) ? string.Format("[{0}]={1}", strFieldName, oValue) : string.Format("[{0}]='{1}'", strFieldName, oValue);
                    if (sb.Length != 0)
                        sb.Append(",");
                    sb.Append(strSingle);
                }
                HSqlFactory query = new HSqlFactory();
                query.SqlType = HSqlType.Update;
                query.SqlTableName = this._activeEntity.TableName;
                query.SqlWhereString = strUpdateKey;
                query.SqlContent = sb.ToString();
                try
                {
                    HDBOperation.QueryNonQuery(query.ToString());
                }
                catch (Exception ee)
                {
                    return -1;
                }
            }
            //清除整表缓存
            CacheUtil.Clear(this._activeEntity.TableName);
            //清除该表相关条件缓存
            CacheUtil.ClearContainsKey(this._activeEntity.TableName + "-");

            //添加缓存(缓存的存储格式和EntityList的缓存格式相同，以防当出现"KeyName=KeyValue"类型的条件时两个方法可以存储相同的缓存)
            string strCacheKey = string.Format("{0}-[{1}]={2}", this._activeEntity.TableName, this._activeEntity.KeyName ,nKeyValue);
            ArrayList alCaches = new ArrayList();
            alCaches.Add(this._activeEntity.ActiveObject);
            CacheUtil.SetValue(strCacheKey, alCaches.ToArray(this._activeEntity.ObjectType));
            return nKeyValue;
        }

        public int EntityDelete()
        {
            bool IsEntity = this._activeEntity.ActiveObject is DataBase.Interface.IEntity;
            if (!IsEntity || null == this._activeEntity.KeyName || "" == this._activeEntity.KeyName)
                return -1;
            int nKeyValue = Util.ParseInt(this._activeEntity.KeyValue + "", -1);
            if (nKeyValue <= 0)
                return -1;
            string strUpdateKey = string.Format("[{0}]={1}", this._activeEntity.KeyName, this._activeEntity.KeyValue);
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Delete;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlWhereString = strUpdateKey;
            HDBOperation.QueryNonQuery(query.ToString());
            //清除整表缓存
            CacheUtil.Clear(this._activeEntity.TableName);
            //清除该表相关条件缓存
            CacheUtil.ClearContainsKey(this._activeEntity.TableName + "-");
            return nKeyValue;
        }

        public int GetNextKey()
        {
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = string.Format("Max([{0}])", this._activeEntity.KeyName);
            query.SqlWhereString = "";
            object oValue = HDBOperation.QueryScalar(query.ToString());
            int nMax = Util.ParseInt(oValue + "", 0);
            nMax++;
            return nMax;
        }

    }

    sealed class ActiveEntity
    {
        public object ActiveObject;
        public Type ObjectType;
        public FieldInfo[] ObjectFields;
        public string KeyName;
        public object KeyValue;
        public string TableName;

        private static Hashtable htTypeInformation = new Hashtable();
        private static string strCacheTypeKey = "BlueSky_EntityTypeList";

        static ActiveEntity()
        {
            if (CacheUtil.IsExistCache(strCacheTypeKey))
            {
                htTypeInformation = (Hashtable)CacheUtil.GetValue(strCacheTypeKey);
            }
            else
            {
                CacheUtil.SetValue(strCacheTypeKey, htTypeInformation, 3600);
            }
        }

        public ActiveEntity()
        { }
        public ActiveEntity(object _oE)
        {
            this.ActiveObject = _oE;
            this.ObjectType = _oE.GetType();

            if (htTypeInformation.ContainsKey(this.ObjectType.Name))
            {
                Hashtable htType = (Hashtable)htTypeInformation[this.ObjectType.Name];
                this.ObjectFields = (FieldInfo[])htType["ObjectFields"];
                this.KeyName = htType["KeyName"] + "";
                this.TableName = htType["TableName"] + "";
            }
            else
            {
                FieldInfo[] alAllFields = this.ObjectType.GetFields();
                List<FieldInfo> ltFields = new List<FieldInfo>(alAllFields);
                foreach (FieldInfo field in alAllFields)
                {
                    if (field.IsStatic)
                        ltFields.Remove(field);
                }
                this.ObjectFields = ltFields.ToArray();
                MethodInfo e = this.ObjectType.GetMethod("GetKeyName");
                this.KeyName = e.Invoke(_oE, null) + "";
                e = this.ObjectType.GetMethod("GetTableName");
                this.TableName = e.Invoke(_oE, null) + "";
                //add to cache
                Hashtable htType = new Hashtable();
                htType["ObjectFields"] = this.ObjectFields;
                htType["KeyName"] =  this.KeyName;
                htType["TableName"] =  this.TableName;
                htTypeInformation[this.ObjectType.Name] = htType;
            }
            FieldInfo key = this.ObjectType.GetField(this.KeyName);
            this.KeyValue = key.GetValue(this.ActiveObject);
        }
    }
}
